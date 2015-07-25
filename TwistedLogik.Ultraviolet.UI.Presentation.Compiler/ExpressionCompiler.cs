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
    public static partial class ExpressionCompiler
    {
        /// <summary>
        /// Traverses the directory tree rooted in <paramref name="root"/> and builds an assembly containing the compiled binding
        /// expressions of any UPF views which are found therein.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="root">The path to the root directory to search.</param>
        /// <returns>An assembly which contains the compiled binding expressions for the specified directory tree.</returns>
        public static Assembly Compile(UltravioletContext uv, String root)
        {
            Contract.Require(uv, "uv");
            Contract.RequireNotEmpty(root, "root");

            var compiler = new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
            DeleteWorkingDirectory();

            var viewDefinitions = RecursivelySearchForViews(root, root);
            var viewModelInfos = RetrieveViewModelInfos(uv, viewDefinitions);

            var viewModelReferences = new ConcurrentBag<String>();
            viewModelReferences.Add(typeof(UIView).Assembly.Location);
            viewModelReferences.Add(typeof(PresentationFoundationView).Assembly.Location);

            var expressionVerificationResult = PerformExpressionVerificationCompilationPass(compiler, viewModelInfos, viewModelReferences);
            if (expressionVerificationResult.Errors.Count > 0)
            {
                WriteErrorsToWorkingDirectory(expressionVerificationResult);
                throw new InvalidOperationException(CompilerStrings.FailedExpressionValidationPass);
            }

            var setterEliminationResult = PerformSetterEliminationCompilationPass(compiler, viewModelInfos, viewModelReferences);
            if (setterEliminationResult.Errors.Count > 0)
            {
                var finalPassResult = PerformFinalCompilationPass(compiler, viewModelInfos, viewModelReferences, setterEliminationResult);
                if (finalPassResult.Errors.Count > 0)
                {
                    WriteErrorsToWorkingDirectory(finalPassResult);
                    throw new InvalidOperationException(CompilerStrings.FailedFinalPass);
                }

                DeleteWorkingDirectory();
                return finalPassResult.CompiledAssembly;
            }
            else
            {
                DeleteWorkingDirectory();
                return setterEliminationResult.CompiledAssembly;
            }
        }

        /// <summary>
        /// Performs the first compilation pass, which generates expression getters in order to verify that the expressions are valid code.
        /// </summary>
        private static CompilerResults PerformExpressionVerificationCompilationPass(CSharpCodeProvider compiler, IEnumerable<ViewModelInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences)
        {
            Parallel.ForEach(viewModelInfos, viewModelInfo =>
            {
                viewModelReferences.Add(viewModelInfo.ViewModelParentType.Assembly.Location);

                foreach (var expression in viewModelInfo.Expressions)
                {
                    expression.GenerateGetter = true;
                    expression.GenerateSetter = false;
                }

                WriteSourceCodeForViewModel(viewModelInfo);
            });

            return CompileViewModelSources(compiler, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Performs the second compilation pass, which generates setters in order to determine which expressions support two-way bindings.
        /// </summary>
        private static CompilerResults PerformSetterEliminationCompilationPass(CSharpCodeProvider compiler, IEnumerable<ViewModelInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences)
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
            return CompileViewModelSources(compiler, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Performs the final compilation pass, which removes invalid expression setters based on the results of the previous pass.
        /// </summary>
        private static CompilerResults PerformFinalCompilationPass(CSharpCodeProvider compiler, IEnumerable<ViewModelInfo> viewModelInfos, ConcurrentBag<String> viewModelReferences, CompilerResults setterEliminationResult)
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

            return CompileViewModelSources(compiler, viewModelInfos, viewModelReferences);
        }

        /// <summary>
        /// Compiles the specified view model sources into a managed assembly.
        /// </summary>
        /// <param name="compiler">The compiler with which to compile the view model sources.</param>
        /// <param name="infos">A collection of <see cref="ViewModelInfo"/> instances containing the source code to compile.</param>
        /// <param name="references">A collection of assembly locations which should be referenced by the compiled assembly.</param>
        /// <returns>A <see cref="CompilerResults"/> instance that represents the result of compilation.</returns>
        private static CompilerResults CompileViewModelSources(CSharpCodeProvider compiler, IEnumerable<ViewModelInfo> infos, IEnumerable<String> references)
        {
            var options = new CompilerParameters();
            options.OutputAssembly = "TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions.dll";
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

                File.WriteAllText(path, info.ViewModelSource);
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
        /// Creates an instance of <see cref="ViewModelInfo"/> for each of the specified view definitions.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewDefinitions">The collection of <see cref="ViewDefinition"/> objects for which to create <see cref="ViewModelInfo"/> instances.</param>
        /// <returns>A collection containing the <see cref="ViewModelInfo"/ > instances which were created.</returns>
        private static IEnumerable<ViewModelInfo> RetrieveViewModelInfos(UltravioletContext uv, IEnumerable<ViewDefinition> viewDefinitions)
        {
            var viewModelInfos = new ConcurrentBag<ViewModelInfo>();

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
        /// Creates a new instance of <see cref="ViewModelInfo"/> that represents the specified view model.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="viewdef">The view definition for which to retrieve view model info.</param>
        /// <returns>The <see cref="ViewModelInfo"/> that was created to represent the specified view's view model.</returns>
        private static ViewModelInfo GetViewModelInfo(UltravioletContext uv, ViewDefinition viewdef)
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

            var pathComponents = viewdef.Path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            pathComponents[pathComponents.Length - 1] = Path.GetFileNameWithoutExtension(pathComponents[pathComponents.Length - 1]);

            var inheritedViewModelName = String.Format("{0}_VM_Impl", String.Join("_", pathComponents));
            var inheritedViewModelExpressions = new List<BindingExpressionInfo>();
            foreach (var element in viewdef.Definition.Elements())
            {
                FindBindingExpressionsInView(uv, element, inheritedViewModelExpressions);
            }

            return new ViewModelInfo()
            {
                ViewDefinition = viewdef,
                ViewModelParentType = definedViewModelType,
                ViewModelName = inheritedViewModelName,
                Expressions = inheritedViewModelExpressions,
            };
        }

        /// <summary>
        /// Writes the source code for the inherited view model for the specified view.
        /// </summary>
        /// <param name="viewModelInfo">The <see cref="ViewModelInfo"/> that describes the view model being generated.</param>
        private static void WriteSourceCodeForViewModel(ViewModelInfo viewModelInfo)
        {
            var lineCount = 1;

            using (var stringWriter = new StringWriter())
            {
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    var write = new Action<String>(text => { writer.Write(text); lineCount += text.Count(c => c == '\n'); });
                    var writeLine = new Action<String>(text => { writer.WriteLine(text); lineCount += 1 + text.Count(c => c == '\n'); });

                    writeLine("using System;");
                    writeLine(String.Empty);

                    writeLine("namespace TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions");
                    writeLine("{");
                    writer.Indent++;

                    writeLine("public sealed class " + viewModelInfo.ViewModelName + " : " + viewModelInfo.ViewModelParentType.FullName);
                    writeLine("{");
                    writer.Indent++;

                    var ctors = viewModelInfo.ViewModelParentType.GetConstructors();
                    foreach (var ctor in ctors)
                    {
                        if (ctor.IsPrivate)
                            continue;

                        var ctorParams = ctor.GetParameters();

                        write("public " + viewModelInfo.ViewModelName + "(");
                        write(String.Join(", ", ctorParams.Select(x => String.Format("{0} {1}", x.ParameterType.FullName, x.Name))));
                        write(") : base(");
                        write(String.Join(", ", ctorParams.Select(x => x.Name)));
                        writeLine(")");

                        writeLine("{");
                        writer.Indent++;

                        writer.Indent--;
                        writeLine("}");
                    }

                    for (int i = 0; i < viewModelInfo.Expressions.Count; i++)
                    {
                        var expInfo = viewModelInfo.Expressions[i];

                        writeLine("// " + expInfo.Expression);
                        writeLine("public " + expInfo.Type.FullName  + " Expression" + i);
                        writeLine("{");
                        writer.Indent++;

                        var expMemberPath = BindingExpressions.GetBindingMemberPathPart(expInfo.Expression);
                        var expFormatString = BindingExpressions.GetBindingFormatStringPart(expInfo.Expression);

                        if (expInfo.GenerateGetter)
                        {
                            expInfo.GetterLineStart = lineCount;

                            if (expInfo.Type == typeof(String))
                            {
                                if (String.IsNullOrEmpty(expFormatString))
                                {
                                    expFormatString = "{0}";
                                }
                                writeLine(String.Format("get {{ return String.Format(\"{0}\", {1}); }}", expFormatString, expMemberPath));
                            }
                            else
                            {
                                writeLine(String.Format("get {{ return {0}; }}", expMemberPath));
                            }

                            expInfo.GetterLineEnd = lineCount - 1;
                        }
                        if (expInfo.GenerateSetter)
                        {
                            expInfo.SetterLineStart = lineCount;

                            writeLine(String.Format("set {{ {0} = value; }}", expMemberPath));

                            expInfo.SetterLineEnd = lineCount - 1;
                        }

                        writer.Indent--;
                        writeLine("}");
                    }

                    writer.Indent--;
                    writeLine("}");

                    writer.Indent--;
                    writeLine("}");
                }

                viewModelInfo.ViewModelSource = stringWriter.ToString();
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
        /// Gets the name of the file in which the specified view model's source code is saved during compilation.
        /// </summary>
        private static String GetWorkingFileForViewModel(ViewModelInfo viewModelInfo)
        {
            var path = Path.ChangeExtension(Path.Combine(WorkingDirectory, viewModelInfo.ViewModelName), "cs");
            return path;
        }

        // The name of the temporary directory in which the compiler operates.
        private const String WorkingDirectory = "UV_CompiledExpressions";
    }
}
