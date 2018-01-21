using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for compiling UPF binding expressions into a managed assembly.
    /// </summary>
    public class RoslynExpressionCompiler : IBindingExpressionCompiler
    {
        /// <inheritdoc/>
        public BindingExpressionCompilationResult Compile(UltravioletContext uv, BindingExpressionCompilerOptions options)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(options, nameof(options));

            if (String.IsNullOrEmpty(options.Input) || String.IsNullOrEmpty(options.Output))
                throw new ArgumentException(PresentationStrings.InvalidCompilerOptions);

            var state = new RoslynExpressionCompilerState(uv)
            {
                GenerateInMemory = options.GenerateInMemory,
                WorkInTemporaryDirectory = options.WorkInTemporaryDirectory,
                WriteErrorsToFile = options.WriteErrorsToFile
            };
            var dataSourceWrapperInfos = DataSourceLoader.GetDataSourceWrapperInfos(state, uv, options.Input);

            var cacheFile = Path.ChangeExtension(options.Output, "cache");
            var cacheNew = CompilerCache.FromDataSourceWrappers(this, dataSourceWrapperInfos);
            if (File.Exists(options.Output))
            {
                var cacheOld = CompilerCache.TryFromFile(cacheFile);
                if (cacheOld != null && !options.IgnoreCache && !cacheOld.IsDifferentFrom(cacheNew))
                    return BindingExpressionCompilationResult.CreateSucceeded();
            }

            var result = CompileViewModels(state, dataSourceWrapperInfos, options.Output);
            if (result.Succeeded)
            {
                if (!options.GenerateInMemory)
                    cacheNew.Save(cacheFile);

                if (!options.WriteCompiledFilesToWorkingDirectory && !options.WorkInTemporaryDirectory)
                    state.DeleteWorkingDirectory();
            }

            if (options.WriteCompiledFilesToWorkingDirectory && !options.WorkInTemporaryDirectory)
                WriteCompiledFilesToWorkingDirectory(state, dataSourceWrapperInfos);

            return result;
        }

        /// <inheritdoc/>
        public BindingExpressionCompilationResult CompileSingleView(UltravioletContext uv, BindingExpressionCompilerOptions options)
        {
            Contract.Require(options, nameof(options));

            if (String.IsNullOrEmpty(options.Input))
                throw new ArgumentException(PresentationStrings.InvalidCompilerOptions);

            var definition = DataSourceLoader.CreateDataSourceDefinitionFromXml(options.RequestedViewModelNamespace, options.RequestedViewModelName, options.Input);
            if (definition == null)
                return BindingExpressionCompilationResult.CreateSucceeded();

            var state = new RoslynExpressionCompilerState(uv)
            {
                GenerateInMemory = options.GenerateInMemory,
                WorkInTemporaryDirectory = options.WorkInTemporaryDirectory,
                WriteErrorsToFile = options.WriteErrorsToFile
            };
            var dataSourceWrapperInfo = DataSourceLoader.GetDataSourceWrapperInfo(state, definition.Value);
            var dataSourceWrapperInfos = new[] { dataSourceWrapperInfo };

            var result = CompileViewModels(state, dataSourceWrapperInfos, null);
            if (result.Succeeded)
            {
                options.Output = dataSourceWrapperInfos[0].DataSourceWrapperSourceCode;
            }
            else
            {
                state.DeleteWorkingDirectory();
            }

            return result;
        }
        
        /// <summary>
        /// Gets the line number of the specified diagnostic.
        /// </summary>
        private static Int32 GetDiagnosticLine(Diagnostic diagnostic)
        {
            return diagnostic.Location.GetMappedLineSpan().StartLinePosition.Line + 1;
        }

        /// <summary>
        /// Gets the column number of the specified diagnostic.
        /// </summary>
        private static Int32 GetDiagnosticColumn(Diagnostic diagnostic)
        {
            return diagnostic.Location.GetMappedLineSpan().StartLinePosition.Character + 1;
        }

        /// <summary>
        /// Compiles the specified collection of view models.
        /// </summary>
        private static BindingExpressionCompilationResult CompileViewModels(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, String output)
        {
            state.DeleteWorkingDirectory();

            var referencedAssemblies = GetDefaultReferencedAssemblies(state);

            var expressionVerificationResult =
                PerformExpressionVerificationCompilationPass(state, models, referencedAssemblies);

            if (expressionVerificationResult.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).Any())
            {
                if (state.WriteErrorsToFile)
                    WriteErrorsToWorkingDirectory(state, models, expressionVerificationResult);

                return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedExpressionValidationPass,
                    CreateBindingExpressionCompilationErrors(state, models, expressionVerificationResult.GetDiagnostics()));
            }

            var setterEliminationPassResult =
                PerformSetterEliminationCompilationPass(state, models, referencedAssemblies);

            var conversionFixupPassResult =
                PerformConversionFixupCompilationPass(state, models, referencedAssemblies, setterEliminationPassResult);

            var finalPassResult =
                PerformFinalCompilationPass(state, state.GenerateInMemory ? null : output, models, referencedAssemblies, conversionFixupPassResult);

            if (finalPassResult.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).Any())
            {
                if (state.WriteErrorsToFile)
                    WriteErrorsToWorkingDirectory(state, models, finalPassResult);

                return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedFinalPass,
                    CreateBindingExpressionCompilationErrors(state, models, finalPassResult.GetDiagnostics()));
            }

            var outputStream = default(Stream);
            try
            {
                outputStream = state.GenerateInMemory ? new MemoryStream() : (Stream)File.OpenWrite(output);

                var emitResult = finalPassResult.Emit(outputStream);
                if (emitResult.Success)
                {
                    var assembly = state.GenerateInMemory ? Assembly.Load(((MemoryStream)outputStream).ToArray()) : null;
                    return BindingExpressionCompilationResult.CreateSucceeded(assembly);
                }
                else
                {
                    return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedEmit,
                        CreateBindingExpressionCompilationErrors(state, models, emitResult.Diagnostics));
                }
            }
            finally
            {
                if (outputStream != null)
                    outputStream.Dispose();
            }

        }

        /// <summary>
        /// Performs the first compilation pass, which generates expression getters in order to verify that the expressions are valid code.
        /// </summary>
        private static Compilation PerformExpressionVerificationCompilationPass(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies)
        {
            Parallel.ForEach(models, model =>
            {
                referencedAssemblies.Add(model.DataSourceType.Assembly.Location);
                foreach (var reference in model.References)
                {
                    referencedAssemblies.Add(reference);
                }

                foreach (var expression in model.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = false;
                }

                WriteSourceCodeForDataSourceWrapper(state, model);
            });

            return CompileDataSourceWrapperSources(state, null, models, referencedAssemblies);
        }

        /// <summary>
        /// Performs the second compilation pass, which generates setters in order to determine which expressions support two-way bindings.
        /// </summary>
        private static Compilation PerformSetterEliminationCompilationPass(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies)
        {
            Parallel.ForEach(models, model =>
            {
                foreach (var expression in model.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = true;
                }

                WriteSourceCodeForDataSourceWrapper(state, model);
            });
            return CompileDataSourceWrapperSources(state, null, models, referencedAssemblies);
        }

        /// <summary>
        /// Performs the third compilation pass, which attempts to fix any errors caused by non-implicit conversions and nullable types that need to be cast to non-nullable types.
        /// </summary>
        private static Compilation PerformConversionFixupCompilationPass(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies, Compilation setterEliminationResult)
        {
            var errors = setterEliminationResult.GetDiagnostics().Where(x => x.Location.IsInSource && x.Severity == DiagnosticSeverity.Error).ToList();

            var fixableErrorNumbers = new List<String>
            {
                "CS0266",
                "CS1502",
                "CS1503"
            };

            Parallel.ForEach(models, model =>
            {
                var dataSourceWrapperFilename = Path.GetFileName(state.GetWorkingFileForDataSourceWrapper(model));
                var dataSourceWrapperErrors = errors.Where(x => Path.GetFileName(x.Location.SourceTree.FilePath) == dataSourceWrapperFilename).ToList();

                foreach (var expression in model.Expressions)
                {
                    var setterErrors = dataSourceWrapperErrors.Where(x =>
                        GetDiagnosticLine(x) >= expression.SetterLineStart &&
                        GetDiagnosticLine(x) <= expression.SetterLineEnd).ToList();
                    var setterIsNullable = Nullable.GetUnderlyingType(expression.Type) != null;
                    
                    expression.GenerateSetter = !setterErrors.Any() || (setterIsNullable && setterErrors.All(x => fixableErrorNumbers.Contains(x.Id)));
                    expression.NullableFixup = setterIsNullable;

                    if (setterErrors.Count == 1 && setterErrors.Single().Id == "CS0266")
                    {
                        var error = setterErrors.Single();
                        var match = regexCS0266.Match(error.GetMessage(CultureInfo.InvariantCulture));
                        expression.CS0266SourceType = match.Groups["source"].Value;
                        expression.CS0266TargetType = match.Groups["target"].Value;
                        expression.GenerateSetter = true;
                    }
                }

                WriteSourceCodeForDataSourceWrapper(state, model);
            });
            return CompileDataSourceWrapperSources(state, null, models, referencedAssemblies);
        }

        /// <summary>
        /// Performs the final compilation pass, which removes invalid expression setters based on the results of the previous pass.
        /// </summary>
        private static Compilation PerformFinalCompilationPass(RoslynExpressionCompilerState state, String output, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies, Compilation nullableFixupResult)
        {
            var errors = nullableFixupResult.GetDiagnostics().Where(x => x.Location.IsInSource && x.Severity == DiagnosticSeverity.Error).ToList();

            Parallel.ForEach(models, model =>
            {
                var dataSourceWrapperFilename = Path.GetFileName(state.GetWorkingFileForDataSourceWrapper(model));
                var dataSourceWrapperErrors = errors.Where(x => Path.GetFileName(x.Location.SourceTree.FilePath) == dataSourceWrapperFilename).ToList();

                foreach (var expression in model.Expressions)
                {
                    if (expression.GenerateSetter && dataSourceWrapperErrors.Any(x =>
                        GetDiagnosticLine(x) >= expression.SetterLineStart &&
                        GetDiagnosticLine(x) <= expression.SetterLineEnd))
                    {
                        expression.GenerateSetter = false;
                    }
                }

                WriteSourceCodeForDataSourceWrapper(state, model);
            });

            return CompileDataSourceWrapperSources(state, output, models, referencedAssemblies);
        }

        /// <summary>
        /// Compiles the specified data source wrapper sources into a managed assembly.
        /// </summary>
        private static Compilation CompileDataSourceWrapperSources(RoslynExpressionCompilerState state, String output, IEnumerable<DataSourceWrapperInfo> infos, IEnumerable<String> references)
        {
            var files = new List<String> { WriteCompilerMetadataFile() };
            var trees = new List<SyntaxTree>();
            var mrefs = references.Distinct().Select(x => MetadataReference.CreateFromFile(Path.IsPathRooted(x) ? x : Assembly.Load(x).Location));

            foreach (var info in infos)
            {
                var path = state.GetWorkingFileForDataSourceWrapper(info);
                files.Add(path);
                trees.Add(CSharpSyntaxTree.ParseText(info.DataSourceWrapperSourceCode, path: path));
                
                File.WriteAllText(path, info.DataSourceWrapperSourceCode);
            }

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("Ultraviolet.Presentation.CompiledExpressions.dll", trees, mrefs, options);

            return compilation;
        }
        
        /// <summary>
        /// Creates a collection containing the assemblies which are referenced by default during compilation.
        /// </summary>
        private static ConcurrentBag<String> GetDefaultReferencedAssemblies(RoslynExpressionCompilerState state)
        {
            var referencedAssemblies = new ConcurrentBag<String>();

            var netStandardRefAsmDir = 
                DependencyFinder.GetNetStandardLibraryDirFromNuGetCache() ?? 
                DependencyFinder.GetNetStandardLibraryDirFromFallback();

            if (netStandardRefAsmDir == null && DependencyFinder.IsNuGetAvailable())
            {
                Directory.CreateDirectory(state.GetWorkingDirectory());

                DependencyFinder.DownloadNuGetExecutable();
                DependencyFinder.InstallNuGetPackage(state, "NETStandard.Library", "2.0.1");

                netStandardRefAsmDir = DependencyFinder.GetNetStandardLibraryDirFromWorkingDir(state.GetWorkingDirectory());
            }

            if (netStandardRefAsmDir == null)
                throw new InvalidOperationException(CompilerStrings.CouldNotLocateReferenceAssemblies);

            referencedAssemblies.Add(Path.Combine(netStandardRefAsmDir, "netstandard.dll"));
            referencedAssemblies.Add(Path.Combine(netStandardRefAsmDir, "mscorlib.dll"));
            referencedAssemblies.Add(Path.Combine(netStandardRefAsmDir, "System.Runtime.dll"));
            referencedAssemblies.Add(Path.Combine(netStandardRefAsmDir, "System.Runtime.Extensions.dll"));
            referencedAssemblies.Add(Path.Combine(netStandardRefAsmDir, "System.Runtime.InteropServices.dll"));
            
            referencedAssemblies.Add(typeof(Contract).Assembly.Location);
            referencedAssemblies.Add(typeof(UltravioletContext).Assembly.Location);
            referencedAssemblies.Add(typeof(PresentationFoundation).Assembly.Location);

            return referencedAssemblies;
        }

        /// <summary>
        /// Builds the source code for the CompilerMetadata class and writes it to a file.
        /// </summary>
        private static String WriteCompilerMetadataFile()
        {
            var writer = new DataSourceWrapperWriter();
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace " + PresentationFoundationView.DataSourceWrapperNamespaceForViews);
            writer.WriteLine("{");
            writer.WriteLine("#pragma warning disable 1591");
            writer.WriteLine("#pragma warning disable 0184");
            writer.WriteLine("// Generated by the UPF Binding Expression Compiler, version {0}", version);
            writer.WriteLine("[System.CLSCompliant(false)]");
            writer.WriteLine("public sealed class CompilerMetadata");
            writer.WriteLine("{");
            writer.WriteLine("public static String Version {{ get {{ return \"{0}\"; }} }}", version);
            writer.WriteLine("}");
            writer.WriteLine("#pragma warning restore 1591");
            writer.WriteLine("#pragma warning restore 0184");
            writer.WriteLine("}");

            var path = Path.Combine(Path.GetTempPath(), "CompilerMetadata.cs");
            File.WriteAllText(path, writer.ToString());

            return path;
        }

        /// <summary>
        /// Writes the source code for the specified data source wrapper.
        /// </summary>
        private static void WriteSourceCodeForDataSourceWrapper(RoslynExpressionCompilerState state,
            DataSourceWrapperInfo dataSourceWrapperInfo)
        {
            using (var writer = new DataSourceWrapperWriter())
            {
                // Using statements
                var imports = Enumerable.Union(new[] { "System" }, dataSourceWrapperInfo.Imports).Distinct().OrderBy(x => x);
                foreach (var import in imports)
                {
                    writer.WriteLine("using {0};", import);
                }
                writer.WriteLine();

                // Namespace declaration
                var @namespace = dataSourceWrapperInfo.DataSourceDefinition.DataSourceWrapperNamespace;
                writer.WriteLine("namespace " + @namespace);
                writer.WriteLine("{");
                writer.WriteLine("#pragma warning disable 1591");
                writer.WriteLine("#pragma warning disable 0184");

                // Data source wrapper class - main
                WriteSourceCodeForDataSourceWrapperClass(state, dataSourceWrapperInfo, writer);

                // Data source wrapper class - dependent
                foreach (var dependentWrapperInfo in dataSourceWrapperInfo.DependentWrapperInfos)
                    WriteSourceCodeForDataSourceWrapperClass(state, dependentWrapperInfo, writer);

                // Namespace complete
                writer.WriteLine("#pragma warning restore 0184");
                writer.WriteLine("#pragma warning restore 1591");
                writer.WriteLine("}");

                dataSourceWrapperInfo.DataSourceWrapperSourceCode = writer.ToString();
            }
        }

        /// <summary>
        /// Writes the source code for an individual data source wrapper class.
        /// </summary>
        private static void WriteSourceCodeForDataSourceWrapperClass(RoslynExpressionCompilerState state,
            DataSourceWrapperInfo dataSourceWrapperInfo, DataSourceWrapperWriter writer)
        {
            // Class declaration
            writer.WriteLine("// Generated by the UPF Binding Expression Compiler, version {0}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            writer.WriteLine("[System.CLSCompliant(false)]");
            writer.WriteLine("[Ultraviolet.Core.Preserve(AllMembers = true)]");
            writer.WriteLine("[Ultraviolet.Presentation.WrappedDataSource(typeof({0}))]", CSharpLanguage.GetCSharpTypeName(dataSourceWrapperInfo.DataSourceType));
            writer.WriteLine("public sealed partial class {0} : {1}", dataSourceWrapperInfo.DataSourceWrapperName, CSharpLanguage.GetCSharpTypeName(typeof(CompiledDataSourceWrapper)));
            writer.WriteLine("{");

            // Constructors
            writer.WriteLine("#region Constructors");
            writer.WriteConstructor(dataSourceWrapperInfo);
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // IDataSourceWrapper
            writer.WriteLine("#region IDataSourceWrapper");
            writer.WriteIDataSourceWrapperImplementation(dataSourceWrapperInfo);
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // Methods
            writer.WriteLine("#region Methods");
            var methods = dataSourceWrapperInfo.DataSourceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (!ExpressionUtil.NeedsWrapper(method))
                    continue;

                writer.WriteWrapperMethod(method);
            }
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // Properties
            writer.WriteLine("#region Properties");
            var properties = dataSourceWrapperInfo.DataSourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (!ExpressionUtil.NeedsWrapper(property))
                    continue;

                writer.WriteWrapperProperty(property);
            }
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // Fields
            writer.WriteLine("#region Fields");
            var fields = dataSourceWrapperInfo.DataSourceType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (!ExpressionUtil.NeedsWrapper(field))
                    continue;

                writer.WriteWrapperProperty(field);
            }
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // Expressions
            writer.WriteLine("#region Expressions");
            for (int i = 0; i < dataSourceWrapperInfo.Expressions.Count; i++)
            {
                var expressionInfo = dataSourceWrapperInfo.Expressions[i];
                writer.WriteExpressionProperty(state, dataSourceWrapperInfo, expressionInfo, i);
            }
            writer.WriteLine("#endregion");
            writer.WriteLine();

            // Special-case binding delegates
            writer.WriteLine("#region Binding Delegates");
            if (typeof(Controls.ContentControl).IsAssignableFrom(dataSourceWrapperInfo.DataSourceType))
            {
                // ContentControl
                writer.WriteLine("public static readonly DataBindingGetter<System.Object> __GetContent = " +
                    "new DataBindingGetter<System.Object>(vm => (({0})vm).Content);", dataSourceWrapperInfo.DataSourceWrapperName);
                writer.WriteLine("public static readonly DataBindingGetter<System.String> __GetContentStringFormat = " +
                    "new DataBindingGetter<System.String>(vm => (({0})vm).ContentStringFormat);", dataSourceWrapperInfo.DataSourceWrapperName);
                writer.WriteLine("public static readonly DataBindingSetter<System.Object> __SetContent = " +
                    "new DataBindingSetter<System.Object>((vm, value) => (({0})vm).Content = value);", dataSourceWrapperInfo.DataSourceWrapperName);
                writer.WriteLine("public static readonly DataBindingSetter<System.String> __SetContentStringFormat = " +
                    "new DataBindingSetter<System.String>((vm, value) => (({0})vm).ContentStringFormat = value);", dataSourceWrapperInfo.DataSourceWrapperName);
            }
            writer.WriteLine("#endregion");

            // Class complete
            writer.WriteLine("}");
        }

        /// <summary>
        /// Writes the source code of the specified collection of wrappers to the working directory.
        /// </summary>
        private static void WriteCompiledFilesToWorkingDirectory(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models)
        {
            var workingDirectory = state.GetWorkingDirectory();
            Directory.CreateDirectory(workingDirectory);

            foreach (var model in models)
            {
                var path = Path.ChangeExtension(Path.Combine(workingDirectory, model.DataSourceWrapperName), "cs");
                File.WriteAllText(path, model.DataSourceWrapperSourceCode);
            }
        }

        /// <summary>
        /// Writes any compiler errors to the working directory.
        /// </summary>
        private static void WriteErrorsToWorkingDirectory(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, Compilation results)
        {
            var logpath = Path.Combine(state.GetWorkingDirectory(), "Compilation Errors.txt");
            try
            {
                File.Delete(logpath);
            }
            catch (DirectoryNotFoundException) { }

            var logdir = Path.GetDirectoryName(logpath);
            Directory.CreateDirectory(logdir);

            // NOTE: Under Mono we seem to get warnings even when "Treat Warnings as Errors" is turned off.
            var trueErrors = results.GetDiagnostics().Where(x => x.Location.IsInSource && x.Severity == DiagnosticSeverity.Error).ToList();
            if (trueErrors.Count > 0)
            {
                var filesWithErrors = trueErrors.Select(x => x.Location.SourceTree.FilePath).Where(x => !String.IsNullOrEmpty(x)).Distinct();
                var filesWithErrorsPretty = new Dictionary<String, String> { { String.Empty, String.Empty } };

                foreach (var fileWithErrors in filesWithErrors)
                {
                    var modelNameForFile = models.Where(x => x.UniqueID.ToString() == Path.GetFileNameWithoutExtension(fileWithErrors))
                        .Select(x => x.DataSourceWrapperName).SingleOrDefault();

                    var prettyFileName = Path.ChangeExtension(modelNameForFile, "cs");
                    filesWithErrorsPretty[fileWithErrors] = prettyFileName;

                    var fileWithErrorsSrc = Path.GetFullPath(fileWithErrors);
                    var fileWithErrorsDst = Path.Combine(state.GetWorkingDirectory(), prettyFileName);
                    File.Copy(fileWithErrorsSrc, fileWithErrorsDst, true);
                }

                var errorStrings = trueErrors.Select(x =>
                    String.Format("{0}\t{1}\t{2}\t{3}", x.Id, x.GetMessage(), filesWithErrorsPretty[x.Location.SourceTree.FilePath ?? String.Empty], GetDiagnosticLine(x)));

                File.WriteAllLines(logpath, Enumerable.Union(new[] { "Code\tDescription\tFile\tLine" }, errorStrings));
            }
        }
        
        /// <summary>
        /// Converts a <see cref="CompilerErrorCollection"/> to a collection of <see cref="BindingExpressionCompilationError"/> objects.
        /// </summary>
        private static List<BindingExpressionCompilationError> CreateBindingExpressionCompilationErrors(RoslynExpressionCompilerState state,
            IEnumerable<DataSourceWrapperInfo> models, ImmutableArray<Diagnostic> diagnostics)
        {
            var result = new List<BindingExpressionCompilationError>();

            var workingDirectory = state.GetWorkingDirectory();

            var errorsByFile = diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error && x.Location.IsInSource)
                .GroupBy(x => Path.GetFileName(x.Location.SourceTree.FilePath)).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var model in models)
            {
                var dataSourceWrapperFilename = Path.GetFileName(state.GetWorkingFileForDataSourceWrapper(model));
                var dataSourceErrors = default(List<Diagnostic>);
                if (errorsByFile.TryGetValue(dataSourceWrapperFilename, out dataSourceErrors))
                {
                    foreach (var dataSourceError in dataSourceErrors)
                    {
                        var fullPathToFile = model.DataSourceWrapperName;
                        if (state.WriteErrorsToFile)
                        {
                            fullPathToFile = Path.GetFullPath(Path.Combine(workingDirectory,
                                Path.ChangeExtension(model.DataSourceWrapperName, "cs")));
                        }

                        var line = GetDiagnosticLine(dataSourceError);
                        var column = GetDiagnosticColumn(dataSourceError);
                        var errno = dataSourceError.Id;
                        var message = dataSourceError.GetMessage(CultureInfo.InvariantCulture);

                        result.Add(new BindingExpressionCompilationError(fullPathToFile, line, column, errno, message));
                    }
                }
            }

            return result;
        }
        
        // Regular expressions for error parsing
        private static readonly Regex regexCS0266 = new Regex(@"Cannot implicitly convert type \'(?<source>\S+)\' to \'(?<target>\S+)\'\. An explicit conversion exists \(are you missing a cast\?\)",
            RegexOptions.Compiled | RegexOptions.Singleline);
    }
}
