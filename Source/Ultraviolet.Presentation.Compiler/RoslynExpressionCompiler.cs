using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
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
            var dataSourceWrapperInfos = DataSourceLoader.GetDataSourceWrapperInfos(state, options.Input);

            var cacheFile = Path.ChangeExtension(options.Output, "cache");
            var cacheNew = CompilerCache.FromDataSourceWrappers(this, dataSourceWrapperInfos);
            if (File.Exists(options.Output))
            {
                var cacheOld = CompilerCache.TryFromFile(cacheFile);
                if (cacheOld != null && !options.IgnoreCache && !cacheOld.IsDifferentFrom(cacheNew))
                    return BindingExpressionCompilationResult.CreateSucceeded();
            }

            var result = CompileViewModels(state, dataSourceWrapperInfos, options.Output, options.GenerateDebugAssembly);
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

            var result = CompileViewModels(state, dataSourceWrapperInfos, null, options.GenerateDebugAssembly);
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
        private static BindingExpressionCompilationResult CompileViewModels(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, String output, Boolean debug)
        {
            state.DeleteWorkingDirectory();

            var referencedAssemblies = GetDefaultReferencedAssemblies(state);

            var initialPassResult =
                PerformInitialCompilationPass(state, models, referencedAssemblies, debug);

            var fixupPassResult =
                PerformSyntaxTreeFixup(initialPassResult, models);

            if (fixupPassResult.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).Any())
            {
                if (state.WriteErrorsToFile)
                    WriteErrorsToWorkingDirectory(state, models, fixupPassResult);

                return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedFinalPass,
                    CreateBindingExpressionCompilationErrors(state, models, fixupPassResult.GetDiagnostics()));
            }

            return EmitCompilation(state, models, output, fixupPassResult);
        }

        /// <summary>
        /// Emits the specified compilation to either an in-memory stream or a file.
        /// </summary>
        private static BindingExpressionCompilationResult EmitCompilation(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, String output, Compilation compilation)
        {
            var outputStream = default(Stream);
            try
            {
                outputStream = state.GenerateInMemory ? new MemoryStream() : (Stream)File.OpenWrite(output);
                
                var options = new EmitOptions(outputNameOverride: "Ultraviolet.Presentation.CompiledExpressions.dll",
                    debugInformationFormat: DebugInformationFormat.PortablePdb, fileAlignment: 512, baseAddress: 0x11000000);
                var emitResult = compilation.Emit(outputStream, options: options);
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
        /// Performs the first compilation pass, which the semantic model which will be used for fixup.
        /// </summary>
        private static Compilation PerformInitialCompilationPass(RoslynExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies, Boolean debug)
        {
            Parallel.ForEach(models, model =>
            {
                var lastSeenDataSourceAssembly = default(Assembly);
                for (var dataSourceType = model.DataSourceType; dataSourceType != null; dataSourceType = dataSourceType.BaseType)
                {
                    var dataSourceAssembly = dataSourceType.Assembly;
                    if (dataSourceAssembly.FullName.StartsWith("System."))
                        break;

                    if (dataSourceAssembly != lastSeenDataSourceAssembly)
                    {
                        lastSeenDataSourceAssembly = dataSourceAssembly;
                        referencedAssemblies.Add(dataSourceAssembly.Location);
                    }
                }

                foreach (var reference in model.References)
                    referencedAssemblies.Add(reference);

                foreach (var expression in model.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = true;
                    expression.NullableFixup = false;
                }

                WriteSourceCodeForDataSourceWrapper(state, model);
            });

            return CompileDataSourceWrapperSources(state, null, models, referencedAssemblies, debug);
        }

        /// <summary>
        /// Compiles the specified data source wrapper sources into a Roslyn compilation object.
        /// </summary>
        private static Compilation CompileDataSourceWrapperSources(RoslynExpressionCompilerState state, String output, IEnumerable<DataSourceWrapperInfo> infos, IEnumerable<String> references, Boolean debug)
        {
            var trees = new ConcurrentBag<SyntaxTree>() { CSharpSyntaxTree.ParseText(WriteCompilerMetadataFile(debug), CSharpParseOptions.Default, "CompilerMetadata.cs") };
            var mrefs = references.Distinct().Select(x => MetadataReference.CreateFromFile(Path.IsPathRooted(x) ? x : Assembly.Load(x).Location));
            
            Parallel.ForEach(infos, info =>
            {
                var path = state.GetWorkingFileForDataSourceWrapper(info);
                File.WriteAllText(path, info.DataSourceWrapperSourceCode);

                trees.Add(CSharpSyntaxTree.ParseText(info.DataSourceWrapperSourceCode, path: path));                
            });

            var optimization = debug ? OptimizationLevel.Debug : OptimizationLevel.Release;
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: optimization, deterministic: true);
            var compilation = CSharpCompilation.Create("Ultraviolet.Presentation.CompiledExpressions.dll", trees, mrefs, options);

            return compilation;
        }
        
        /// <summary>
        /// Performs fixup steps on the specified compilation.
        /// </summary>
        private static Compilation PerformSyntaxTreeFixup(Compilation compilation, IEnumerable<DataSourceWrapperInfo> infos)
        {
            var syncObject = new Object();
            var trees = compilation.SyntaxTrees.ToList();
            var result = compilation.Clone();

            Parallel.ForEach(trees, tree =>
            {
                var changed = false;

                var semanticModel = default(SemanticModel);
                var oldTree = (CSharpSyntaxTree)tree;
                var newTree = (CSharpSyntaxTree)tree;

                var info = infos.Where(x => String.Equals(x.UniqueID.ToString(),
                    Path.GetFileNameWithoutExtension(oldTree.FilePath), StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                
                if (info == null)
                    return;

                lock (syncObject)
                    semanticModel = result.GetSemanticModel(oldTree, true);

                newTree = RewriteSyntaxTree(oldTree, semanticModel, sm => new FixupExpressionPropertiesRewriter(sm));
                if (newTree != oldTree)
                {
                    lock (syncObject)
                        result = result.ReplaceSyntaxTree(oldTree, newTree);

                    oldTree = newTree;
                    changed = true;
                }

                lock (syncObject)
                    semanticModel = result.GetSemanticModel(oldTree, true);

                newTree = RewriteSyntaxTree(oldTree, semanticModel, sm => new RemoveUnnecessaryDataBindingSetterFieldsRewriter(sm));
                if (newTree != oldTree)
                {
                    lock (syncObject)
                        result = result.ReplaceSyntaxTree(oldTree, newTree);

                    oldTree = newTree;
                    changed = true;
                }

                if (changed)
                {
                    lock (syncObject)
                        info.DataSourceWrapperSourceCode = newTree.ToString();
                }
            });
            
            return result;
        }

        /// <summary>
        /// Rewrites a syntax tree using the specified rewriter.
        /// </summary>
        private static CSharpSyntaxTree RewriteSyntaxTree(CSharpSyntaxTree tree, SemanticModel semanticModel, Func<SemanticModel, CSharpSyntaxRewriter> rewriter)
        {
            var rewrittenRoot = rewriter(semanticModel).Visit(tree.GetRoot());
            if (rewrittenRoot != tree.GetRoot())
            {
                return (CSharpSyntaxTree)CSharpSyntaxTree.Create((CSharpSyntaxNode)rewrittenRoot, tree.Options, tree.FilePath, tree.Encoding);
            }

            return tree;
        }

        /// <summary>
        /// Creates a collection containing the assemblies which are referenced by default during compilation.
        /// </summary>
        private static ConcurrentBag<String> GetDefaultReferencedAssemblies(RoslynExpressionCompilerState state)
        {
            var referencedAssemblies = new ConcurrentBag<String>();

            var netStandardRefAdditionalPaths = new List<String>();
            var netStandardRefAsmDir = 
                DependencyFinder.GetNetStandardLibraryDirFromNuGetCache(netStandardRefAdditionalPaths) ??
                DependencyFinder.GetNetStandardLibraryDirFromFallback(netStandardRefAdditionalPaths);

            if (netStandardRefAsmDir == null && DependencyFinder.IsNuGetAvailable())
            {
                Directory.CreateDirectory(state.GetWorkingDirectory());

                DependencyFinder.DownloadNuGetExecutable();
                DependencyFinder.InstallNuGetPackage(state, "NETStandard.Library", "2.0.3");

                netStandardRefAsmDir = DependencyFinder.GetNetStandardLibraryDirFromWorkingDir(state.GetWorkingDirectory());
            }

            if (netStandardRefAsmDir == null)
            {
                if (UltravioletPlatformInfo.CurrentRuntime == UltravioletRuntime.CoreCLR &&
                    UltravioletPlatformInfo.CurrentRuntimeVersion.Major > 2)
                {
                    throw new InvalidOperationException(CompilerStrings.CouldNotLocateReferenceAssembliesCore3);
                }
                else
                {
                    throw new InvalidOperationException(CompilerStrings.CouldNotLocateReferenceAssemblies);
                }
            }

            var refDllFiles = Directory.GetFiles(netStandardRefAsmDir, "*.dll");
            foreach (var refDllFile in refDllFiles)
            {
                referencedAssemblies.Add(refDllFile);
            }
            foreach (var refDllFile in netStandardRefAdditionalPaths)
            {
                referencedAssemblies.Add(refDllFile);
            }

            referencedAssemblies.Add(typeof(Contract).Assembly.Location);
            referencedAssemblies.Add(typeof(UltravioletContext).Assembly.Location);
            referencedAssemblies.Add(typeof(PresentationFoundation).Assembly.Location);

            return referencedAssemblies;
        }

        /// <summary>
        /// Builds the source code for the CompilerMetadata class.
        /// </summary>
        private static String WriteCompilerMetadataFile(Boolean debug)
        {
            var writer = new DataSourceWrapperWriter();
            var applicationAsm = Assembly.GetEntryAssembly();
            var applicationVersion = applicationAsm == null ? "1.0.0.0" : FileVersionInfo.GetVersionInfo(applicationAsm.Location).FileVersion;
            var compilerVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Reflection;");
            writer.WriteLine("using System.Runtime.Versioning;");
            writer.WriteLine();
            writer.WriteLine("[assembly: TargetFramework(\".NETStandard,Version=v2.1\", FrameworkDisplayName = \"\")]");
            writer.WriteLine("[assembly: AssemblyCompany(\"Ultraviolet Framework\")]");
            writer.WriteLine("[assembly: AssemblyConfiguration(\"" + (debug ? "Debug" : "Release") + "\")]");
            writer.WriteLine("[assembly: AssemblyFileVersion(\"{0}\")]", applicationVersion);
            writer.WriteLine("[assembly: AssemblyInformationalVersion(\"{0}\")]", applicationVersion);
            writer.WriteLine("[assembly: AssemblyProduct(\"Compiled Binding Expressions\")]");
            writer.WriteLine("[assembly: AssemblyTitle(\"Compiled Binding Expressions\")]");
            writer.WriteLine("[assembly: AssemblyVersion(\"{0}\")]", applicationVersion);
            writer.WriteLine();
            writer.WriteLine("namespace " + PresentationFoundationView.DataSourceWrapperNamespaceForViews);
            writer.WriteLine("{");
            writer.WriteLine("#pragma warning disable 1591");
            writer.WriteLine("#pragma warning disable 0184");
            writer.WriteLine("// Generated by the UPF Binding Expression Compiler, version {0}", compilerVersion);
            writer.WriteLine("[System.CLSCompliant(false)]");
            writer.WriteLine("public sealed class CompilerMetadata");
            writer.WriteLine("{");
            writer.WriteLine("public static String Version {{ get {{ return \"{0}\"; }} }}", compilerVersion);
            writer.WriteLine("}");
            writer.WriteLine("#pragma warning restore 1591");
            writer.WriteLine("#pragma warning restore 0184");
            writer.WriteLine("}");
            return writer.ToString();
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
            var trueErrors = results.GetDiagnostics().Where(x => x.Location.IsInSource && x.Location.SourceTree.FilePath != "CompilerMetadata.cs" && x.Severity == DiagnosticSeverity.Error).ToList();
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
        /// Converts an array of <see cref="Diagnostic"/> instances to a collection of <see cref="BindingExpressionCompilationError"/> objects.
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
    }
}
