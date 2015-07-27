using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        /// <inheritdoc/>
        public void Compile(UltravioletContext uv, String root, String output)
        {
            Contract.Require(uv, "uv");
            Contract.RequireNotEmpty(root, "root");

            var compiler = new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
            DeleteWorkingDirectory();

            var viewDefinitions = RecursivelySearchForViews(root, root);
            var viewModelInfos = RetrieveViewModelInfos(uv, viewDefinitions);
            
            var viewModelReferences = new ConcurrentBag<String>();
            viewModelReferences.Add("TwistedLogik.Nucleus.dll");
            viewModelReferences.Add("TwistedLogik.Ultraviolet.dll");
            viewModelReferences.Add("TwistedLogik.Ultraviolet.UI.Presentation.dll"); 

            var expressionVerificationResult = PerformExpressionVerificationCompilationPass(compiler, viewModelInfos, viewModelReferences);
            if (expressionVerificationResult.Errors.Count > 0)
            {
                WriteErrorsToWorkingDirectory(expressionVerificationResult);
                throw new InvalidOperationException(CompilerStrings.FailedExpressionValidationPass);
            }

            var setterEliminationResult = 
                PerformSetterEliminationCompilationPass(compiler, viewModelInfos, viewModelReferences);

            var finalPassResult = PerformFinalCompilationPass(compiler, output, viewModelInfos, viewModelReferences, setterEliminationResult);
            if (finalPassResult.Errors.Count > 0)
            {
                WriteErrorsToWorkingDirectory(finalPassResult);
                throw new InvalidOperationException(CompilerStrings.FailedFinalPass);
            }

            DeleteWorkingDirectory();
        }

        /// <summary>
        /// Performs the first compilation pass, which generates expression getters in order to verify that the expressions are valid code.
        /// </summary>
        private static CompilerResults PerformExpressionVerificationCompilationPass(CSharpCodeProvider compiler, IEnumerable<ViewModelWrapperInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences)
        {
            Parallel.ForEach(viewModelInfos, viewModelInfo =>
            {
                viewModelReferences.Add(viewModelInfo.ViewModelType.Assembly.Location);
                foreach (var reference in viewModelInfo.References)
                {
                    viewModelReferences.Add(reference);
                }

                foreach (var expression in viewModelInfo.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = false;
                }

                WriteSourceCodeForViewModel(viewModelInfo);
            });

            return CompileViewModelSources(compiler, null, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Performs the second compilation pass, which generates setters in order to determine which expressions support two-way bindings.
        /// </summary>
        private static CompilerResults PerformSetterEliminationCompilationPass(CSharpCodeProvider compiler, IEnumerable<ViewModelWrapperInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences)
        {
            Parallel.ForEach(viewModelInfos, viewModelInfo =>
            {
                foreach (var expression in viewModelInfo.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = true;
                }
                WriteSourceCodeForViewModel(viewModelInfo);
            });
            return CompileViewModelSources(compiler, null, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Performs the final compilation pass, which removes invalid expression setters based on the results of the previous pass.
        /// </summary>
        private static CompilerResults PerformFinalCompilationPass(CSharpCodeProvider compiler, String output, IEnumerable<ViewModelWrapperInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences, CompilerResults setterEliminationResult)
        {
            var errors = setterEliminationResult.Errors.Cast<CompilerError>().ToList();

            Parallel.ForEach(viewModelInfos, viewModelInfo =>
            {
                var viewModelFilename = Path.GetFileName(GetWorkingFileForViewModel(viewModelInfo));
                var viewModelErrors = errors.Where(x => Path.GetFileName(x.FileName) == viewModelFilename).ToList();

                foreach (var expression in viewModelInfo.Expressions)
                {
                    expression.GenerateSetter = !viewModelErrors.Any(x => x.Line >= expression.SetterLineStart && x.Line <= expression.SetterLineEnd);
                }

                WriteSourceCodeForViewModel(viewModelInfo);
            });

            return CompileViewModelSources(compiler, output, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Compiles the specified view model sources into a managed assembly.
        /// </summary>
        /// <param name="compiler">The compiler with which to compile the view model sources.</param>
        /// <param name="infos">A collection of <see cref="ViewModelWrapperInfo"/> instances containing the source code to compile.</param>
        /// <param name="references">A collection of assembly locations which should be referenced by the compiled assembly.</param>
        /// <returns>A <see cref="CompilerResults"/> instance that represents the result of compilation.</returns>
        private static CompilerResults CompileViewModelSources(CSharpCodeProvider compiler, String output, IEnumerable<ViewModelWrapperInfo> infos, IEnumerable<String> references)
        {
            var writeToFile = (output != null);

            var options = new CompilerParameters();
            options.OutputAssembly = output;
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.IncludeDebugInformation = false;
            options.ReferencedAssemblies.AddRange(references.Distinct().ToArray());

            var dir = Directory.CreateDirectory(WorkingDirectory);
            var files = new List<String>();

            foreach (var info in infos)
            {
                var path = GetWorkingFileForViewModel(info);
                files.Add(path);

                File.WriteAllText(path, info.ViewModelWrapperSource);
            }

            return compiler.CompileAssemblyFromFile(options, files.ToArray());
        }

        /// <summary>
        /// Recursively searches the specified directory tree for XML files which define UPF views and retrieves the XML elements which define those views.
        /// </summary>
        /// <param name="root">The root directory from which the recursive search began.</param>
        /// <param name="directory">The root of the directory tree to search.</param>
        /// <returns>A collection of <see cref="XElement"/> instances which represent UPF view definitions.</returns>
        private static IEnumerable<ViewDefinition> RecursivelySearchForViews(String root, String directory)
        {
            var result = new List<ViewDefinition>();

            var files = Directory.GetFiles(directory, "*.xml");
            foreach (var file in files)
            {
                try
                {
                    var xml = XDocument.Load(file);

                    if (xml.Root.Name.LocalName != "UIPanelDefinition")
                        continue;

                    var viewdef = xml.Root.Element("View");
                    if (viewdef == null)
                        continue;

                    result.Add(new ViewDefinition(file, viewdef));
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
        /// Creates an instance of <see cref="ViewModelWrapperInfo"/> for each of the specified view definitions.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewDefinitions">The collection of <see cref="ViewDefinition"/> objects for which to create <see cref="ViewModelWrapperInfo"/> instances.</param>
        /// <returns>A collection containing the <see cref="ViewModelWrapperInfo"/ > instances which were created.</returns>
        private static IEnumerable<ViewModelWrapperInfo> RetrieveViewModelInfos(UltravioletContext uv, IEnumerable<ViewDefinition> viewDefinitions)
        {
            var viewModelInfos = new ConcurrentBag<ViewModelWrapperInfo>();

            Parallel.ForEach(viewDefinitions, viewDefinition =>
            {
                var viewModelInfo = GetViewModelInfo(uv, viewDefinition);
                if (viewModelInfo == null)
                    return;

                viewModelInfos.Add(viewModelInfo);
            });

            return viewModelInfos;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewModelWrapperInfo"/> that represents the specified view model.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewdef">The view definition for which to retrieve view model info.</param>
        /// <returns>The <see cref="ViewModelWrapperInfo"/> that was created to represent the specified view's view model.</returns>
        private static ViewModelWrapperInfo GetViewModelInfo(UltravioletContext uv, ViewDefinition viewdef)
        {
            var definedViewModelTypeName = (String)viewdef.Definition.Attribute("ViewModelType");
            if (definedViewModelTypeName == null)
                return null;

            var typeNameCommaIx = definedViewModelTypeName.IndexOf(',');
            if (typeNameCommaIx < 0)
                throw new InvalidOperationException(CompilerStrings.ViewModelTypeIsNotFullyQualified.Format(viewdef.Path));

            var definedViewModelTypeAssemblyName = definedViewModelTypeName.Substring(typeNameCommaIx + 1).Trim();
            var definedViewModelTypeAssembly = Assembly.Load(definedViewModelTypeAssemblyName);

            var definedViewModelType = Type.GetType(definedViewModelTypeName);
            if (definedViewModelType.IsSealed)
                throw new InvalidOperationException(CompilerStrings.ViewModelIsSealed.Format(definedViewModelType.Name));

            if (definedViewModelType.IsAbstract)
                throw new InvalidOperationException(CompilerStrings.ViewModelIsAbstract.Format(definedViewModelType.Name));
            
            var viewModelWrapperName = PresentationFoundationView.GetViewModelWrapperNameFromAssetPath(viewdef.Path);
            var viewModelWrapperExpressions = new List<BindingExpressionInfo>();
            foreach (var element in viewdef.Definition.Elements())
            {
                FindBindingExpressionsInView(uv, element, viewModelWrapperExpressions);
            }

            viewModelWrapperExpressions = CollapseViewModelExpressions(viewModelWrapperExpressions);

            var viewModelReferences = new List<String>();
            var viewModelImports = new List<String>();

            var xmlRoot = viewdef.Definition.Parent;
            var xmlDirectives = xmlRoot.Elements("Directive");
            foreach (var xmlDirective in xmlDirectives)
            {
                var xmlDirectiveType = (String)xmlDirective.Attribute("Type");
                if (String.IsNullOrEmpty(xmlDirectiveType))
                    throw new InvalidDataException(UltravioletStrings.ViewDirectiveMustHaveType.Format(viewdef.Path));

                switch (xmlDirectiveType.ToLowerInvariant())
                {
                    case "import":
                        viewModelImports.Add(xmlDirective.Value.Trim());
                        break;

                    case "reference":
                        viewModelReferences.Add(xmlDirective.Value.Trim());
                        break;
                }
            }

            return new ViewModelWrapperInfo()
            {
                References = viewModelReferences,
                Imports = viewModelImports,
                ViewDefinition = viewdef,
                ViewModelType = definedViewModelType,
                ViewModelWrapperName = viewModelWrapperName,
                Expressions = viewModelWrapperExpressions,
            };
        }

        /// <summary>
        /// Writes the source code for the inherited view model for the specified view.
        /// </summary>
        /// <param name="viewModelInfo">The <see cref="ViewModelWrapperInfo"/> that describes the view model being generated.</param>
        private static void WriteSourceCodeForViewModel(ViewModelWrapperInfo viewModelInfo)
        {
            using (var writer = new ViewModelWrapperWriter())
            {
                // Using statements
                var imports = Enumerable.Union(new[] { "System" }, viewModelInfo.Imports).Distinct().OrderBy(x => x);
                foreach (var import in imports)
                {
                    writer.WriteLine("using {0};", import);
                }
                writer.WriteLine();

                // Namespace and class declaration
                writer.WriteLine("namespace " + PresentationFoundationView.GetViewModelWrapperNamespace());
                writer.WriteLine("{");
                writer.WriteLine("public sealed class {0} : {1}", viewModelInfo.ViewModelWrapperName, writer.GetCSharpTypeName(typeof(IViewModelWrapper)));
                writer.WriteLine("{");

                // Constructors
                writer.WriteLine("#region Constructors");
                writer.WriteConstructor(viewModelInfo);
                writer.WriteLine("#endregion");
                writer.WriteLine();

                // IViewModelWrapper
                writer.WriteLine("#region IViewModelWrapper");
                writer.WriteIViewModelWrapperImplementation(viewModelInfo);
                writer.WriteLine("#endregion");
                writer.WriteLine();

                // Methods
                writer.WriteLine("#region Methods");
                var methods = viewModelInfo.ViewModelType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
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
                var properties = viewModelInfo.ViewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
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
                var fields = viewModelInfo.ViewModelType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
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
                for (int i = 0; i < viewModelInfo.Expressions.Count; i++)
                {
                    var expressionInfo = viewModelInfo.Expressions[i];
                    writer.WriteExpressionProperty(expressionInfo, i);
                }
                writer.WriteLine("#endregion");

                // Source code generation complete
                writer.WriteLine("}");
                writer.WriteLine("}");

                viewModelInfo.ViewModelWrapperSource = writer.ToString();
            }
        }

        /// <summary>
        /// Searches the specified XML element tree for binding expressions and adds them to the specified collection.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="element">The root of the XML element tree to search.</param>
        /// <param name="expressions">The list to populate with any binding expressions that are found.</param>
        private static void FindBindingExpressionsInView(UltravioletContext uv, XElement element, List<BindingExpressionInfo> expressions)
        {
            var upf = uv.GetUI().GetPresentationFoundation();

            Type elementType;
            if (upf.GetKnownType(element.Name.LocalName, out elementType))
            {
                var attrs = element.Attributes();
                foreach (var attr in attrs)
                {
                    var attrValue = attr.Value;
                    if (!BindingExpressions.IsBindingExpression(attrValue))
                        continue;

                    var dprop = DependencyProperty.FindByName(attrValue, elementType);
                    if (dprop == null)
                        throw new InvalidOperationException(CompilerStrings.OnlyDependencyPropertiesCanBeBound.Format(attrValue));

                    expressions.Add(new BindingExpressionInfo(attrValue, dprop.PropertyType) { GenerateGetter = true });
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
                            if (!upf.GetElementDefaultProperty(elementType, out defaultProperty))
                                throw new InvalidOperationException(CompilerStrings.ElementDoesNotHaveDefaultProperty.Format(elementType.Name));

                            var dprop = DependencyProperty.FindByName(defaultProperty, elementType);
                            if (dprop == null)
                                throw new InvalidOperationException(CompilerStrings.OnlyDependencyPropertiesCanBeBound.Format(defaultProperty));

                            expressions.Add(new BindingExpressionInfo(elementValue, dprop.PropertyType) { GenerateGetter = true });
                        }
                    }
                }
            }

            var children = element.Elements();
            foreach (var child in children)
            {
                FindBindingExpressionsInView(uv, child, expressions);
            }
        }

        /// <summary>
        /// Deletes the compiler's working directory.
        /// </summary>
        private static void DeleteWorkingDirectory()
        {
            try
            {
                Directory.Delete(WorkingDirectory, true);
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Writes any compiler errors to the working directory.
        /// </summary>
        private static void WriteErrorsToWorkingDirectory(CompilerResults results)
        {
            var logpath = Path.Combine(WorkingDirectory, "Compilation Errors.txt");
            File.Delete(logpath);

            if (results.Errors.Count > 0)
            {
                var errorStrings = results.Errors.Cast<CompilerError>().Select(x =>
                    String.Format("{0}\t{1}\t{2}\t{3}", x.ErrorNumber, x.ErrorText, Path.GetFileName(x.FileName), x.Line));

                File.WriteAllLines(logpath, Enumerable.Union(new[] { "Code\tDescription\tFile\tLine" }, errorStrings));
            }
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
        /// Gets the name of the file in which the specified view model's source code is saved during compilation.
        /// </summary>
        private static String GetWorkingFileForViewModel(ViewModelWrapperInfo viewModelInfo)
        {
            var path = Path.ChangeExtension(Path.Combine(WorkingDirectory, viewModelInfo.ViewModelWrapperName), "cs");
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
        private static List<BindingExpressionInfo> CollapseViewModelExpressions(List<BindingExpressionInfo> expressions)
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

        // The name of the temporary directory in which the compiler operates.
        private const String WorkingDirectory = "UV_CompiledExpressions";
    }
}
