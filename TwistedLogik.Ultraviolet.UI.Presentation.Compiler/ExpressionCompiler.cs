using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CSharp;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for compiling UPF binding expressions into a managed assembly.
    /// </summary>
    public class ExpressionCompiler : IBindingExpressionCompiler
    {   
        /// <summary>
        /// Gets the type associated with the specified placeholder string in the context of the specified type.
        /// </summary>
        /// <param name="type">The type for which to evaluate placeholder substitutions.</param>
        /// <param name="placeholder">The placeholder string for which to evaluate substitutions.</param>
        /// <returns>The type associated with the specified placeholder, or <see langword="null"/> if there is no such placeholder.</returns>
        public static Type GetPlaceholderType(Type type, String placeholder)
        {
            var attr = type.GetCustomAttributes(typeof(UvmlPlaceholderAttribute), true).Cast<UvmlPlaceholderAttribute>()
                .Where(x => String.Equals(placeholder, x.Placeholder, StringComparison.Ordinal)).SingleOrDefault();
            if (attr != null)
            {
                return attr.Type;
            }
            return null;
        }
        
        /// <inheritdoc/>
        public BindingExpressionCompilationResult Compile(UltravioletContext uv, BindingExpressionCompilerOptions options)
        {
            Contract.Require(uv, "uv");
            Contract.Require(options, "options");

            if (String.IsNullOrEmpty(options.Input) || String.IsNullOrEmpty(options.Output))
                throw new ArgumentException(PresentationStrings.InvalidCompilerOptions);

            var state = CreateCompilerState(uv, options);
            var dataSourceWrapperInfos = GetDataSourceWrapperInfos(state, uv, options.Input);
            
            var cacheFile = Path.ChangeExtension(options.Output, "cache");
            var cacheNew = CompilerCache.FromDataSourceWrappers(dataSourceWrapperInfos);
            if (File.Exists(options.Output))
            {
                var cacheOld = CompilerCache.TryFromFile(cacheFile);
                if (cacheOld != null && !options.IgnoreCache && !cacheOld.IsDifferentFrom(cacheNew))
                    return BindingExpressionCompilationResult.CreateSucceeded();
            }

            var result = CompileViewModels(state, dataSourceWrapperInfos, options.Output);
            if (result.Succeeded)
            {
                cacheNew.Save(cacheFile);

                if (!options.WriteCompiledFilesToWorkingDirectory && !options.WorkInTemporaryDirectory)
                    DeleteWorkingDirectory(state);
            }

            if (options.WriteCompiledFilesToWorkingDirectory && !options.WorkInTemporaryDirectory)
                WriteCompiledFilesToWorkingDirectory(state, dataSourceWrapperInfos);

            return result;
        }

        /// <inheritdoc/>
        public BindingExpressionCompilationResult CompileSingleView(UltravioletContext uv, BindingExpressionCompilerOptions options)
        {
            Contract.Require(options, "options");

            if (String.IsNullOrEmpty(options.Input))
                throw new ArgumentException(PresentationStrings.InvalidCompilerOptions);

            var definition = CreateDataSourceDefinitionFromXml(options.RequestedViewModelNamespace, options.RequestedViewModelName, options.Input);
            if (definition == null)
                return BindingExpressionCompilationResult.CreateSucceeded();

            var state = CreateCompilerState(uv, options);
            var dataSourceWrapperInfo = GetDataSourceWrapperInfo(state, definition.Value);
            var dataSourceWrapperInfos = new[] { dataSourceWrapperInfo };
            
            var result = CompileViewModels(state, dataSourceWrapperInfos, null);
            if (result.Succeeded)
            {
                options.Output = dataSourceWrapperInfos[0].DataSourceWrapperSourceCode;
            }
            else
            {
                DeleteWorkingDirectory(state);
            }

            return result;
        }
        
        /// <summary>
        /// Creates the code provider which is used to compile binding expressions.
        /// </summary>
        /// <returns>An instance of <see cref="CSharpCodeProvider"/> which is used to compile binding expressions.</returns>
        protected virtual CSharpCodeProvider CreateCodeProvider()
        {
            return new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
        }

        /// <summary>
        /// Compiles the specified collection of view models.
        /// </summary>
        private static BindingExpressionCompilationResult CompileViewModels(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, String output)
        {
            DeleteWorkingDirectory(state);

            var referencedAssemblies = GetDefaultReferencedAssemblies();

            var expressionVerificationResult = 
                PerformExpressionVerificationCompilationPass(state, models, referencedAssemblies);

            if (expressionVerificationResult.Errors.Cast<CompilerError>().Where(x => !x.IsWarning).Any())
            {
                if (state.WriteErrorsToFile)
                    WriteErrorsToWorkingDirectory(state, models, expressionVerificationResult);

                return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedExpressionValidationPass,
                    CreateBindingExpressionCompilationErrors(state, models, expressionVerificationResult.Errors));
            }

            var setterEliminationPassResult =
                PerformSetterEliminationCompilationPass(state, models, referencedAssemblies);
            
            var conversionFixupPassResult =
                PerformConversionFixupCompilationPass(state, models, referencedAssemblies, setterEliminationPassResult);

            var finalPassResult = 
                PerformFinalCompilationPass(state, output, models, referencedAssemblies, conversionFixupPassResult);

            if (finalPassResult.Errors.Cast<CompilerError>().Where(x => !x.IsWarning).Any())
            {
                if (state.WriteErrorsToFile)
                    WriteErrorsToWorkingDirectory(state, models, finalPassResult);

                return BindingExpressionCompilationResult.CreateFailed(CompilerStrings.FailedFinalPass,
                    CreateBindingExpressionCompilationErrors(state, models, finalPassResult.Errors));
            }

            return BindingExpressionCompilationResult.CreateSucceeded();
        }

        /// <summary>
        /// Performs the first compilation pass, which generates expression getters in order to verify that the expressions are valid code.
        /// </summary>
        private static CompilerResults PerformExpressionVerificationCompilationPass(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies)
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
        private static CompilerResults PerformSetterEliminationCompilationPass(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies)
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
        private static CompilerResults PerformConversionFixupCompilationPass(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies, CompilerResults setterEliminationResult)
        {
            var errors = setterEliminationResult.Errors.Cast<CompilerError>().ToList();

            var fixableErrorNumbers = new List<String> 
            {
                "CS0266",
                "CS1502",
                "CS1503"
            };

            Parallel.ForEach(models, model =>
            {
                var dataSourceWrapperFilename = Path.GetFileName(GetWorkingFileForDataSourceWrapper(model));
                var dataSourceWrapperErrors = errors.Where(x => Path.GetFileName(x.FileName) == dataSourceWrapperFilename).ToList();

                foreach (var expression in model.Expressions)
                {
                    var setterErrors = dataSourceWrapperErrors.Where(x => x.Line >= expression.SetterLineStart && x.Line <= expression.SetterLineEnd).ToList();
                    var setterIsNullable = Nullable.GetUnderlyingType(expression.Type) != null;

                    expression.GenerateSetter = !setterErrors.Any() || (setterIsNullable && setterErrors.All(x => fixableErrorNumbers.Contains(x.ErrorNumber)));
                    expression.NullableFixup  = setterIsNullable;

                    if (setterErrors.Count == 1 && setterErrors.Single().ErrorNumber == "CS0266")
                    {
                        var error = setterErrors.Single();
                        var match = regexCS0266.Match(error.ErrorText);
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
        private static CompilerResults PerformFinalCompilationPass(ExpressionCompilerState state, String output, IEnumerable<DataSourceWrapperInfo> models, ConcurrentBag<String> referencedAssemblies, CompilerResults nullableFixupResult)
        {
            var errors = nullableFixupResult.Errors.Cast<CompilerError>().ToList();

            Parallel.ForEach(models, model =>
            {
                var dataSourceWrapperFilename = Path.GetFileName(GetWorkingFileForDataSourceWrapper(model));
                var dataSourceWrapperErrors = errors.Where(x => Path.GetFileName(x.FileName) == dataSourceWrapperFilename).ToList();

                foreach (var expression in model.Expressions)
                {
                    if (expression.GenerateSetter && dataSourceWrapperErrors.Any(x => x.Line >= expression.SetterLineStart && x.Line <= expression.SetterLineEnd))
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
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="output">The path to the assembly file which will be created.</param>
        /// <param name="infos">A collection of <see cref="DataSourceWrapperInfo"/> instances containing the source code to compile.</param>
        /// <param name="references">A collection of assembly locations which should be referenced by the compiled assembly.</param>
        /// <returns>A <see cref="CompilerResults"/> instance that represents the result of compilation.</returns>
        private static CompilerResults CompileDataSourceWrapperSources(ExpressionCompilerState state, String output, IEnumerable<DataSourceWrapperInfo> infos, IEnumerable<String> references)
        {
            var options = new CompilerParameters();
            options.OutputAssembly = output;
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.IncludeDebugInformation = false;
            options.TreatWarningsAsErrors = false;
            options.ReferencedAssemblies.AddRange(references.Distinct().ToArray());

            var files = new List<String>();

            foreach (var info in infos)
            {
                var path = GetWorkingFileForDataSourceWrapper(info);
                files.Add(path);

                File.WriteAllText(path, info.DataSourceWrapperSourceCode);
            }
            
            return state.Compiler.CompileAssemblyFromFile(options, files.ToArray());
        }

        /// <summary>
        /// Creates a collection containing the assemblies which are referenced by default during compilation.
        /// </summary>
        /// <returns>A <see cref="ConcurrentBag{String}"/> which contains the default referenced assemblies.</returns>
        private static ConcurrentBag<String> GetDefaultReferencedAssemblies()
        {
            var referencedAssemblies = new ConcurrentBag<String>();
            referencedAssemblies.Add("System.dll");
            referencedAssemblies.Add(typeof(Contract).Assembly.Location);
            referencedAssemblies.Add(typeof(UltravioletContext).Assembly.Location);
            referencedAssemblies.Add(typeof(PresentationFoundation).Assembly.Location);

            return referencedAssemblies;
        }

        /// <summary>
        /// Gets a collection of <see cref="DataSourceWrapperInfo"/> objects for all of the views defined within the specified root directory
        /// as well as all of the component templates which are currently registered with the Ultraviolet context.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="root">The root directory to search for views.</param>
        /// <returns>A collection of <see cref="DataSourceWrapperInfo"/> instances which represent the views and templates which were found.</returns>
        private static IEnumerable<DataSourceWrapperInfo> GetDataSourceWrapperInfos(ExpressionCompilerState state, UltravioletContext uv, String root)
        {
            var viewDefinitions = RecursivelySearchForViews(root, root);
            var viewModelInfos = RetrieveDataSourceWrapperInfos(state, viewDefinitions);

            var templateDefinitions = RetrieveTemplateDefinitions(state);
            var templateModelInfos = RetrieveDataSourceWrapperInfos(state, templateDefinitions);

            return Enumerable.Union(viewModelInfos, templateModelInfos);
        }

        /// <summary>
        /// Creates a <see cref="DataSourceDefinition"/> structure from the specified XML file.
        /// </summary>
        /// <param name="namespace">The namespace within which to place the compiled view model.</param>
        /// <param name="name">The name of the data source's view model.</param>
        /// <param name="path">The path to the file that defines the data source.</param>
        /// <returns>The instance of <see cref="DataSourceDefinition"/> that was created, or <see langword="null"/> if the specified
        /// XML does not contain a valid view.</returns>
        private static DataSourceDefinition? CreateDataSourceDefinitionFromFile(String @namespace, String name, String path)
        {
            var xdocument = default(XDocument);
            try
            {
                xdocument = XDocument.Load(path, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            }
            catch (IOException) { return null; }
            catch (XmlException) { return null; }

            return CreateDataSourceDefinitionFromXml(xdocument, @namespace, name, path);
        }

        /// <summary>
        /// Creates a <see cref="DataSourceDefinition"/> structure for the specified XML string.
        /// </summary>
        /// <param name="namespace">The namespace within which to place the compiled view model.</param>
        /// <param name="name">The name of the data source's view model.</param>
        /// <param name="xml">The XML string to parse.</param>
        /// <returns>The instance of <see cref="DataSourceDefinition"/> that was created, or <see langword="null"/> if the specified
        /// XML does not contain a valid view.</returns>
        private static DataSourceDefinition? CreateDataSourceDefinitionFromXml(String @namespace, String name, String xml)
        {
            var xdocument = default(XDocument);
            try
            {
                xdocument = XDocument.Parse(xml);
            }
            catch (XmlException) { return null; }

            return CreateDataSourceDefinitionFromXml(xdocument, @namespace, name, name);
        }

        /// <summary>
        /// Creates a <see cref="DataSourceDefinition"/> structure for the specified XML document.
        /// </summary>
        /// <param name="xdocument">The XML document from which to load a data source definition.</param>
        /// <param name="namespace">The namespace within which to place the compiled view model.</param>
        /// <param name="name">The name of the data source's view model.</param>
        /// <param name="path">The path to the file that defines the data source.</param>
        /// <returns>The instance of <see cref="DataSourceDefinition"/> that was created, or <see langword="null"/> if the specified
        /// XML does not contain a valid view.</returns>
        private static DataSourceDefinition? CreateDataSourceDefinitionFromXml(XDocument xdocument, String @namespace, String name, String path)
        {
            if (xdocument.Root.Name.LocalName != "UIPanelDefinition")
                return null;

            var viewdef = xdocument.Root.Element("View");
            if (viewdef == null)
                return null;

            return DataSourceDefinition.FromView(@namespace, name, path, viewdef);
        }

        /// <summary>
        /// Recursively searches the specified directory tree for XML files which define UPF views and retrieves the XML elements which define those views.
        /// </summary>
        /// <param name="root">The root directory from which the recursive search began.</param>
        /// <param name="directory">The root of the directory tree to search.</param>
        /// <returns>A collection of <see cref="DataSourceDefinition"/> instances which represent UPF view definitions.</returns>
        private static IEnumerable<DataSourceDefinition> RecursivelySearchForViews(String root, String directory)
        {
            var result = new List<DataSourceDefinition>();

            var files = Directory.GetFiles(directory, "*.xml");
            foreach (var file in files)
            {
                try
                {
                    var name = PresentationFoundationView.GetDataSourceWrapperNameForView(file);
                    var definition = CreateDataSourceDefinitionFromFile(null, name, file);
                    if (definition != null)
                    {
                        result.Add(definition.Value);
                    }
                }
                catch (XmlException) { continue; }
            }

            var subdirs = Directory.GetDirectories(directory);
            foreach (var subdir in subdirs)
            {
                result.AddRange(RecursivelySearchForViews(root, subdir));
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of <see cref="DataSourceDefinition"/> instances for any component templates which
        /// are currently registered with the Ultraviolet Presentation Foundation.
        /// </summary>
        /// <returns>A collection of <see cref="DataSourceDefinition"/> instances which represent UPF component template definitions.</returns>
        private static IEnumerable<DataSourceDefinition> RetrieveTemplateDefinitions(ExpressionCompilerState state)
        {
            if (state.ComponentTemplateManager == null)
                return Enumerable.Empty<DataSourceDefinition>();

            var templateDefs = from template in state.ComponentTemplateManager
                               select DataSourceDefinition.FromComponentTemplate(template.Key, template.Value.Root.Element("View"));
            return templateDefs.ToList();
        }

        /// <summary>
        /// Creates an instance of <see cref="DataSourceWrapperInfo"/> for each of the specified data source definitions.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinitions">The collection of <see cref="DataSourceDefinition"/> objects for which to create <see cref="DataSourceWrapperInfo"/> instances.</param>
        /// <returns>A collection containing the <see cref="DataSourceWrapperInfo"/> instances which were created.</returns>
        private static IEnumerable<DataSourceWrapperInfo> RetrieveDataSourceWrapperInfos(ExpressionCompilerState state, IEnumerable<DataSourceDefinition> dataSourceDefinitions)
        {
            var dataSourceWrapperInfos = new ConcurrentBag<DataSourceWrapperInfo>();

            Parallel.ForEach(dataSourceDefinitions, viewDefinition =>
            {
                var dataSourceWrapperInfo = GetDataSourceWrapperInfo(state, viewDefinition);
                if (dataSourceWrapperInfo == null)
                    return;

                dataSourceWrapperInfos.Add(dataSourceWrapperInfo);
            });

            return dataSourceWrapperInfos;
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataSourceWrapperInfo"/> that represents the specified data source wrapper.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinition">The data source definition for which to retrieve data source wrapper info.</param>
        /// <returns>The <see cref="DataSourceWrapperInfo"/> that was created to represent the specified data source.</returns>
        private static DataSourceWrapperInfo GetDataSourceWrapperInfo(ExpressionCompilerState state, DataSourceDefinition dataSourceDefinition)
        {
            var dataSourceWrappedType = dataSourceDefinition.TemplatedControl;
            if (dataSourceWrappedType == null)
            {
                var definedDataSourceTypeAttr = dataSourceDefinition.Definition.Attribute("ViewModelType");
                var definedDataSourceTypeName = (String)definedDataSourceTypeAttr;
                if (definedDataSourceTypeName == null)
                    return null;
                
                var typeNameCommaIx = definedDataSourceTypeName.IndexOf(',');
                if (typeNameCommaIx < 0)
                {
                    throw new BindingExpressionCompilationErrorException(definedDataSourceTypeAttr, dataSourceDefinition.DefinitionPath,
                        CompilerStrings.ViewModelTypeIsNotFullyQualified.Format(definedDataSourceTypeName));
                }
                
                var definedDataSourceType = Type.GetType(definedDataSourceTypeName);
                if (definedDataSourceType == null)
                {
                    throw new BindingExpressionCompilationErrorException(definedDataSourceTypeAttr, dataSourceDefinition.DefinitionPath,
                        PresentationStrings.ViewModelTypeNotFound.Format(definedDataSourceTypeName));
                }

                dataSourceWrappedType = definedDataSourceType;
            }
            
            var dataSourceWrapperName = dataSourceDefinition.DataSourceWrapperName;
            var dataSourceWrapperExpressions = new List<BindingExpressionInfo>();
            foreach (var element in dataSourceDefinition.Definition.Elements())
            {
                FindBindingExpressionsInDataSource(state, 
                    dataSourceDefinition, dataSourceWrappedType, element, dataSourceWrapperExpressions);
            }

            dataSourceWrapperExpressions = CollapseDataSourceExpressions(dataSourceWrapperExpressions);

            var dataSourceReferences = new List<String>();
            var dataSourceImports = new List<String>();

            var xmlRoot = dataSourceDefinition.Definition.Parent;
            var xmlDirectives = xmlRoot.Elements("Directive");
            foreach (var xmlDirective in xmlDirectives)
            {
                var xmlDirectiveType = (String)xmlDirective.Attribute("Type");
                if (String.IsNullOrEmpty(xmlDirectiveType))
                {
                    throw new BindingExpressionCompilationErrorException(xmlDirective, dataSourceDefinition.DefinitionPath, 
                        CompilerStrings.ViewDirectiveMustHaveType);
                }

                var xmlDirectiveTypeName = xmlDirectiveType.ToLowerInvariant();
                var xmlDirectiveValue = xmlDirective.Value.Trim();
                switch (xmlDirectiveTypeName)
                {
                    case "import":
                        {
                            if (String.IsNullOrEmpty(xmlDirectiveValue))
                            {
                                throw new BindingExpressionCompilationErrorException(xmlDirective, dataSourceDefinition.DefinitionPath,
                                    CompilerStrings.ViewDirectiveHasInvalidValue);
                            }
                            dataSourceImports.Add(xmlDirective.Value.Trim());
                        }
                        break;

                    case "reference":
                        {
                            if (String.IsNullOrEmpty(xmlDirectiveValue))
                            {
                                throw new BindingExpressionCompilationErrorException(xmlDirective, dataSourceDefinition.DefinitionPath,
                                    CompilerStrings.ViewDirectiveHasInvalidValue);
                            }
                            dataSourceReferences.Add(xmlDirective.Value.Trim());
                        }
                        break;

                    default:
                        throw new BindingExpressionCompilationErrorException(xmlDirective, dataSourceDefinition.DefinitionPath,
                            CompilerStrings.ViewDirectiveNotRecognized.Format(xmlDirectiveTypeName));
                }
            }

            return new DataSourceWrapperInfo()
            {
                References = dataSourceReferences,
                Imports = dataSourceImports,
                DataSourceDefinition = dataSourceDefinition,
                DataSourceType = dataSourceWrappedType,
                DataSourceWrapperName = dataSourceWrapperName,
                Expressions = dataSourceWrapperExpressions,
            };
        }

        /// <summary>
        /// Writes the source code for the specified data source wrapper.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceWrapperInfo">The <see cref="DataSourceWrapperInfo"/> that describes the data source wrapper being generated.</param>
        private static void WriteSourceCodeForDataSourceWrapper(ExpressionCompilerState state, DataSourceWrapperInfo dataSourceWrapperInfo)
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

                // Namespace and class declaration
                var @namespace = dataSourceWrapperInfo.DataSourceDefinition.DataSourceWrapperNamespace;
                writer.WriteLine("namespace " + @namespace);
                writer.WriteLine("{");
                writer.WriteLine("#pragma warning disable 1591");
                writer.WriteLine("#pragma warning disable 0184");
                writer.WriteLine("[System.CLSCompliant(false)]");
                writer.WriteLine("[System.CodeDom.Compiler.GeneratedCode(\"UPF Binding Expression Compiler\", \"{0}\")]", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
                writer.WriteLine("public sealed partial class {0} : {1}", dataSourceWrapperInfo.DataSourceWrapperName, writer.GetCSharpTypeName(typeof(CompiledDataSourceWrapper)));
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
                    if (!NeedsWrapper(method))
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
                    if (!NeedsWrapper(property))
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
                    if (!NeedsWrapper(field))
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

                // Source code generation complete
                writer.WriteLine("}");
                writer.WriteLine("#pragma warning restore 0184");
                writer.WriteLine("#pragma warning restore 1591");
                writer.WriteLine("}");

                dataSourceWrapperInfo.DataSourceWrapperSourceCode = writer.ToString();
            }
        }
        
        /// <summary>
        /// Searches the specified XML element tree for binding expressions and adds them to the specified collection.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinition">The data source definition for the data source which is being compiled.</param>
        /// <param name="dataSourceWrappedType">The type for which a data source wrapper is being compiled.</param>
        /// <param name="element">The root of the XML element tree to search.</param>
        /// <param name="expressions">The list to populate with any binding expressions that are found.</param>
        private static void FindBindingExpressionsInDataSource(ExpressionCompilerState state, DataSourceDefinition dataSourceDefinition,
            Type dataSourceWrappedType, XElement element, List<BindingExpressionInfo> expressions)
        {
            var elementName = element.Name.LocalName;
            var elementType = GetPlaceholderType(dataSourceWrappedType, elementName);            
            if (elementType != null || state.GetKnownType(elementName, out elementType))
            {
                var attrs = element.Attributes();
                foreach (var attr in attrs)
                {
                    var attrValue = attr.Value;
                    if (!BindingExpressions.IsBindingExpression(attrValue))
                        continue;
                    
                    var dprop = FindDependencyOrAttachedPropertyByName(state, attr.Name.LocalName, elementType);
                    if (dprop == null)
                    {
                        throw new BindingExpressionCompilationErrorException(attr, dataSourceDefinition.DefinitionPath,
                            CompilerStrings.OnlyDependencyPropertiesCanBeBound.Format(attr.Name.LocalName));
                    }

                    var expText = BindingExpressions.GetBindingMemberPathPart(attrValue);
                    var expProp = GetBindablePropertyOnDataSource(dataSourceWrappedType, expText);

                    expressions.Add(new BindingExpressionInfo(attr, 
                        attrValue, expProp?.PropertyType ?? dprop.PropertyType) { GenerateGetter = true });
                }

                if (element.Nodes().Count() == 1)
                {
                    var singleChild = element.Nodes().Single();
                    if (singleChild.NodeType == XmlNodeType.Text)
                    {
                        var elementValue = ((XText)singleChild).Value;
                        if (BindingExpressions.IsBindingExpression(elementValue))
                        {
                            String defaultProperty;
                            if (!state.GetElementDefaultProperty(elementType, out defaultProperty))
                            {
                                throw new BindingExpressionCompilationErrorException(singleChild, dataSourceDefinition.DefinitionPath,
                                    CompilerStrings.ElementDoesNotHaveDefaultProperty.Format(elementType.Name));
                            }

                            var dprop = FindDependencyOrAttachedPropertyByName(state, defaultProperty, elementType);
                            if (dprop == null)
                            {
                                throw new BindingExpressionCompilationErrorException(singleChild, dataSourceDefinition.DefinitionPath,
                                    CompilerStrings.OnlyDependencyPropertiesCanBeBound.Format(defaultProperty));
                            }

                            var expText = BindingExpressions.GetBindingMemberPathPart(elementValue);
                            var expProp = GetBindablePropertyOnDataSource(dataSourceWrappedType, expText);

                            expressions.Add(new BindingExpressionInfo(singleChild, 
                                elementValue, expProp?.PropertyType ?? dprop.PropertyType) { GenerateGetter = true });
                        }
                    }
                }
            }

            var children = element.Elements();
            foreach (var child in children)
            {
                FindBindingExpressionsInDataSource(state, dataSourceDefinition, 
                    dataSourceWrappedType, child, expressions);
            }
        }

        /// <summary>
        /// Deletes the compiler's working directory.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        private static void DeleteWorkingDirectory(ExpressionCompilerState state)
        {
            try
            {
                Directory.Delete(GetWorkingDirectory(state), true);
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Writes the source code of the specified collection of wrappers to the working directory.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="models">The list of models which were compiled.</param>
        private static void WriteCompiledFilesToWorkingDirectory(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models)
        {
            var workingDirectory = GetWorkingDirectory(state);
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
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="models">The list of models which were compiled.</param>
        /// <param name="results">The results of the previous compilation pass.</param>
        private static void WriteErrorsToWorkingDirectory(ExpressionCompilerState state, IEnumerable<DataSourceWrapperInfo> models, CompilerResults results)
        {
            var logpath = Path.Combine(GetWorkingDirectory(state), "Compilation Errors.txt");
            try
            {
                File.Delete(logpath);
            }
            catch(DirectoryNotFoundException) {	}

            var logdir = Path.GetDirectoryName(logpath);
            Directory.CreateDirectory(logdir);

            // NOTE: Under Mono we seem to get warnings even when "Treat Warnings as Errors" is turned off.
            var trueErrors = results.Errors.Cast<CompilerError>().Where(x => !x.IsWarning).ToList();
            if (trueErrors.Count > 0)
            {
                var filesWithErrors = trueErrors.Select(x => x.FileName).Where(x => !String.IsNullOrEmpty(x)).Distinct();
                var filesWithErrorsPretty = new Dictionary<String, String> { { String.Empty, String.Empty } };
                
                foreach (var fileWithErrors in filesWithErrors)
                {
                    var modelNameForFile = models.Where(x => x.UniqueID.ToString() == Path.GetFileNameWithoutExtension(fileWithErrors))
                        .Select(x => x.DataSourceWrapperName).SingleOrDefault();

                    var prettyFileName = Path.ChangeExtension(modelNameForFile, "cs");
                    filesWithErrorsPretty[fileWithErrors] = prettyFileName;

                    var fileWithErrorsSrc = Path.GetFullPath(fileWithErrors);
                    var fileWithErrorsDst = Path.Combine(GetWorkingDirectory(state), prettyFileName);
                    File.Copy(fileWithErrorsSrc, fileWithErrorsDst, true);
                }

                var errorStrings = trueErrors.Select(x =>
                    String.Format("{0}\t{1}\t{2}\t{3}", x.ErrorNumber, x.ErrorText, filesWithErrorsPretty[x.FileName ?? String.Empty], x.Line));
                
                File.WriteAllLines(logpath, Enumerable.Union(new[] { "Code\tDescription\tFile\tLine" }, errorStrings));
            }
        }

        /// <summary>
        /// Gets the bindable property on the specified data source type which has the specified name,
        /// if such a property exists.
        /// </summary>
        private static PropertyInfo GetBindablePropertyOnDataSource(Type dataSourceType, String name)
        {
            return dataSourceType?.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// Attempts to find the dependency or attached property with the specified name.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="name">The name of the dependency or attached property to retrieve.</param>
        /// <param name="ownerType">The type that references the dependency or attached property.</param>
        /// <returns>The <see cref="DependencyProperty"/> referred to by the specified name, or <see langword="null"/> if there is no such dependency property.</returns>
        private static DependencyProperty FindDependencyOrAttachedPropertyByName(ExpressionCompilerState state, String name, Type ownerType)
        {
            String container;
            String property;
            if (IsAttachedProperty(name, out container, out property))
            {
                Type containerType;
                if (!state.GetKnownType(container, out containerType))
                    return null;

                return DependencyProperty.FindByName(property, containerType);
            }
            return DependencyProperty.FindByName(name, ownerType);
        }

        /// <summary>
        /// Gets a value indicating whether the specified attribute name represents an attached property.
        /// </summary>
        /// <param name="name">The attribute name to evaluate.</param>
        /// <param name="container">The attached property's container type.</param>
        /// <param name="property">The attached property's property name.</param>
        /// <returns><see langword="true"/> if the specified name represents an attached property; otherwise, <see langword="false"/>.</returns>
        private static Boolean IsAttachedProperty(String name, out String container, out String property)
        {
            container = null;
            property = null;

            var delimiterIx = name.IndexOf('.');
            if (delimiterIx >= 0)
            {
                container = name.Substring(0, delimiterIx);
                property = name.Substring(delimiterIx + 1);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified method.
        /// </summary>
        private static Boolean NeedsWrapper(MethodInfo method)
        {
            if (method.IsSpecialName)
                return false;

            if (method.Name.StartsWith("get_") || method.Name.StartsWith("end_"))
                return false;

            if (method.DeclaringType == typeof(Object))
                return false;

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified property.
        /// </summary>
        private static Boolean NeedsWrapper(PropertyInfo property)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified field.
        /// </summary>
        private static Boolean NeedsWrapper(FieldInfo field)
        {
            return true;
        }

        /// <summary>
        /// Gets the working directory for the specified compilation.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <returns>The working directory for the specified compilation.</returns>
        private static String GetWorkingDirectory(ExpressionCompilerState state)
        {
            return state.WorkInTemporaryDirectory ? Path.Combine(Path.GetTempPath(), "UV_CompiledExpressions") : "UV_CompiledExpressions";
        }

        /// <summary>
        /// Gets the name of the file in which the specified data source wrapper's source code is saved during compilation.
        /// </summary>
        private static String GetWorkingFileForDataSourceWrapper(DataSourceWrapperInfo dataSourceWrapperInfo)
        {
            var path = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), dataSourceWrapperInfo.UniqueID.ToString()), "cs");
            return path;
        }

        /// <summary>
        /// Gets the C# name of the specified type, including by-ref specifications.
        /// </summary>
        private static String GetCSharpTypeName(Type type)
        {
            if (type == typeof(void))
                return "void";

            if (type.IsByRef)
            {
                return "ref " + type.GetElementType().FullName;
            }

            return type.FullName;
        }

        /// <summary>
        /// Gets the source text for the specified parameter when it is part of a parameter list.
        /// </summary>
        private static String GetParameterText(ParameterInfo parameter)
        {
            return GetCSharpTypeName(parameter.ParameterType) + " " + parameter.Name;
        }

        /// <summary>
        /// Gets the source text for the specified parameter when it is part of an argument list.
        /// </summary>
        private static String GetArgumentText(ParameterInfo parameter)
        {
            if (parameter.IsOut)
            {
                return "out " + parameter.Name;
            }
            if (parameter.ParameterType.IsByRef)
            {
                return "ref " + parameter.Name;
            }
            return parameter.Name;
        }

        /// <summary>
        /// Collapses any redundant expressions in the specified collection into a single instance.
        /// </summary>
        private static List<BindingExpressionInfo> CollapseDataSourceExpressions(List<BindingExpressionInfo> expressions)
        {
            var collapsed = from exp in expressions
                            group exp by new
                            {
                                exp.Expression,
                                exp.Type,
                            }
                            into g
                            select g;

            return collapsed.Select(x => x.First()).ToList();
        }

        /// <summary>
        /// Converts a <see cref="CompilerErrorCollection"/> to a collection of <see cref="BindingExpressionCompilationError"/> objects.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="models">The list of models which were compiled.</param>
        /// <param name="errors">The collection of errors produced during compilation.</param>
        /// <returns>A list containing the converted errors.</returns>
        private static List<BindingExpressionCompilationError> CreateBindingExpressionCompilationErrors(ExpressionCompilerState state,
            IEnumerable<DataSourceWrapperInfo> models, CompilerErrorCollection errors)
        {
            var result = new List<BindingExpressionCompilationError>();

            var workingDirectory = GetWorkingDirectory(state);

            var errorsByFile = errors.Cast<CompilerError>()
                .Where(x => !x.IsWarning).GroupBy(x => Path.GetFileName(x.FileName)).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var model in models)
            {
                var dataSourceWrapperFilename = Path.GetFileName(GetWorkingFileForDataSourceWrapper(model));
                var dataSourceErrors = default(List<CompilerError>);
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

                        result.Add(new BindingExpressionCompilationError(fullPathToFile,
                            dataSourceError.Line, dataSourceError.Column, dataSourceError.ErrorNumber, dataSourceError.ErrorText));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExpressionCompilerState"/> from the specified set of compiler options.
        /// </summary>
        private ExpressionCompilerState CreateCompilerState(UltravioletContext uv, BindingExpressionCompilerOptions options)
        {
            var compiler = CreateCodeProvider();
            var state = new ExpressionCompilerState(uv, compiler);
            state.WorkInTemporaryDirectory = options.WorkInTemporaryDirectory;
            state.WriteErrorsToFile = options.WriteErrorsToFile;

            return state;
        }

        // Regular expressions for error parsing
        private static readonly Regex regexCS0266 = new Regex(@"Cannot implicitly convert type \'(?<source>\S+)\' to \'(?<target>\S+)\'\. An explicit conversion exists \(are you missing a cast\?\)", 
            RegexOptions.Compiled | RegexOptions.Singleline);        
    }
}
