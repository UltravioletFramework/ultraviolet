using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for loading data sources for compilation.
    /// </summary>
    internal static class DataSourceLoader
    {
        /// <summary>
        /// Gets a collection of <see cref="DataSourceWrapperInfo"/> objects for all of the views defined within the specified root directory
        /// as well as all of the component templates which are currently registered with the Ultraviolet context.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="root">The root directory to search for views.</param>
        /// <returns>A collection of <see cref="DataSourceWrapperInfo"/> instances which represent the views and templates which were found.</returns>
        public static IEnumerable<DataSourceWrapperInfo> GetDataSourceWrapperInfos(IExpressionCompilerState state, String root)
        {
            var viewDefinitions = RecursivelySearchForViews(root, root);
            var viewModelInfos = RetrieveDataSourceWrapperInfos(state, viewDefinitions);

            var templateDefinitions = RetrieveTemplateDefinitions(state);
            var templateModelInfos = RetrieveDataSourceWrapperInfos(state, templateDefinitions);

            return Enumerable.Union(viewModelInfos, templateModelInfos);
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataSourceWrapperInfo"/> that represents the specified data source wrapper.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinition">The data source definition for which to retrieve data source wrapper info.</param>
        /// <returns>The <see cref="DataSourceWrapperInfo"/> that was created to represent the specified data source.</returns>
        public static DataSourceWrapperInfo GetDataSourceWrapperInfo(IExpressionCompilerState state, DataSourceDefinition dataSourceDefinition)
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

            var frameworkTemplates = new Dictionary<String, XElement>();
            FindFrameworkTemplateElements(dataSourceDefinition.Definition, frameworkTemplates);

            var frameworkTemplateWrapperDefs = frameworkTemplates.Select(x =>
                DataSourceDefinition.FromView(dataSourceDefinition.DataSourceWrapperNamespace, x.Key, dataSourceDefinition.DefinitionPath, x.Value)).ToList();

            var frameworkTemplateWrapperInfos = frameworkTemplateWrapperDefs.Select(definition =>
                GetDataSourceWrapperInfoForFrameworkTemplate(state, dataSourceReferences, dataSourceImports, definition)).ToList();

            return new DataSourceWrapperInfo()
            {
                References = dataSourceReferences,
                Imports = dataSourceImports,
                DataSourceDefinition = dataSourceDefinition,
                DataSourcePath = dataSourceDefinition.DefinitionPath,
                DataSourceType = dataSourceWrappedType,
                DataSourceWrapperName = dataSourceWrapperName,
                Expressions = dataSourceWrapperExpressions,
                DependentWrapperInfos = frameworkTemplateWrapperInfos
            };
        }

        /// <summary>
        /// Creates a new <see cref="DataSourceWrapperInfo"/> instance which represents a particular framework template.
        /// </summary>
        public static DataSourceWrapperInfo GetDataSourceWrapperInfoForFrameworkTemplate(IExpressionCompilerState state,
            IEnumerable<String> references, IEnumerable<String> imports, DataSourceDefinition definition)
        {
            var dataSourceWrappedTypeAttr = definition.Definition.Attribute("ViewModelType");
            if (dataSourceWrappedTypeAttr == null)
            {
                throw new BindingExpressionCompilationErrorException(definition.Definition, definition.DefinitionPath,
                    PresentationStrings.TemplateMustSpecifyViewModelType);
            }

            var dataSourceWrappedType = Type.GetType(dataSourceWrappedTypeAttr.Value, false);
            if (dataSourceWrappedType == null)
            {
                throw new BindingExpressionCompilationErrorException(dataSourceWrappedTypeAttr, definition.DefinitionPath,
                    PresentationStrings.ViewModelTypeNotFound.Format(dataSourceWrappedTypeAttr.Value));
            }

            var dataSourceDefinition = definition;
            var dataSourceWrapperName = definition.DataSourceWrapperName;

            var expressions = new List<BindingExpressionInfo>();
            foreach (var element in dataSourceDefinition.Definition.Elements())
            {
                FindBindingExpressionsInDataSource(state,
                    dataSourceDefinition, dataSourceWrappedType, element, expressions);
            }
            expressions = CollapseDataSourceExpressions(expressions);

            return new DataSourceWrapperInfo()
            {
                References = references,
                Imports = imports,
                DataSourceDefinition = dataSourceDefinition,
                DataSourceType = dataSourceWrappedType,
                DataSourceWrapperName = dataSourceWrapperName,
                Expressions = expressions
            };
        }

        /// <summary>
        /// Creates a <see cref="DataSourceDefinition"/> structure from the specified XML file.
        /// </summary>
        /// <param name="namespace">The namespace within which to place the compiled view model.</param>
        /// <param name="name">The name of the data source's view model.</param>
        /// <param name="path">The path to the file that defines the data source.</param>
        /// <returns>The instance of <see cref="DataSourceDefinition"/> that was created, or <see langword="null"/> if the specified
        /// XML does not contain a valid view.</returns>
        public static DataSourceDefinition? CreateDataSourceDefinitionFromFile(String @namespace, String name, String path)
        {
            XDocument xdocument;
            try
            {
                xdocument = XDocument.Load(path, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                if (!String.Equals("UIPanelDefinition", xdocument.Root.Name.LocalName, StringComparison.InvariantCulture) ||
                    !String.Equals("Upf", (String)xdocument.Root.Attribute("Provider") ?? "Upf", StringComparison.InvariantCulture))
                {
                    return null;
                }
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
        public static DataSourceDefinition? CreateDataSourceDefinitionFromXml(String @namespace, String name, String xml)
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
        public static DataSourceDefinition? CreateDataSourceDefinitionFromXml(XDocument xdocument, String @namespace, String name, String path)
        {
            if (xdocument.Root.Name.LocalName != "UIPanelDefinition")
                return null;

            var viewdef = xdocument.Root.Element("View");
            if (viewdef == null)
                return null;

            return DataSourceDefinition.FromView(@namespace, name, path, viewdef);
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
        private static DependencyProperty FindDependencyOrAttachedPropertyByName(IExpressionCompilerState state, String name, Type ownerType)
        {
            if (ExpressionUtil.IsAttachedProperty(name, out var container, out var property))
            {
                if (!state.GetKnownType(container, out var containerType))
                    return null;

                return DependencyProperty.FindByName(property, containerType);
            }
            return DependencyProperty.FindByName(name, ownerType);
        }
        
        /// <summary>
        /// Searches the specified XML element tree for binding expressions and adds them to the specified collection.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinition">The data source definition for the data source which is being compiled.</param>
        /// <param name="dataSourceWrappedType">The type for which a data source wrapper is being compiled.</param>
        /// <param name="element">The root of the XML element tree to search.</param>
        /// <param name="expressions">The list to populate with any binding expressions that are found.</param>
        private static void FindBindingExpressionsInDataSource(IExpressionCompilerState state, DataSourceDefinition dataSourceDefinition,
            Type dataSourceWrappedType, XElement element, List<BindingExpressionInfo> expressions)
        {
            var templateAnnotation = element.Annotation<FrameworkTemplateNameAnnotation>();
            if (templateAnnotation != null)
                return;

            var elementName = element.Name.LocalName;
            var elementType = UvmlTypeAnalysis.GetPlaceholderType(dataSourceWrappedType, elementName);
            if (elementType != null || state.GetKnownType(elementName, out elementType))
            {
                var attrs = Enumerable.Union(
                    element.Attributes().Select(x =>
                        new { Object = (XObject)x, Name = x.Name.LocalName, Value = x.Value }),
                    element.Elements().Where(x => x.Name.LocalName.StartsWith(elementName + ".")).Select(x =>
                        new { Object = (XObject)x, Name = x.Name.LocalName, Value = x.Value }));

                foreach (var attr in attrs)
                {
                    var attrValue = attr.Value;
                    if (!BindingExpressions.IsBindingExpression(attrValue))
                        continue;

                    var dprop = FindDependencyOrAttachedPropertyByName(state, attr.Name, elementType);
                    if (dprop == null)
                    {
                        throw new BindingExpressionCompilationErrorException(attr.Object, dataSourceDefinition.DefinitionPath,
                            CompilerStrings.OnlyDependencyPropertiesCanBeBound.Format(attr.Name));
                    }

                    var expText = BindingExpressions.GetBindingMemberPathPart(attrValue);
                    var expProp = GetBindablePropertyOnDataSource(dataSourceWrappedType, expText);
                    var expType = expProp?.PropertyType ?? dprop.PropertyType;
                    if (typeof(DataTemplate).IsAssignableFrom(expType))
                        continue;

                    expressions.Add(new BindingExpressionInfo(attr.Object, attrValue, expType) { GenerateGetter = true });
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
                            if (!state.GetElementDefaultProperty(elementType, out defaultProperty) || defaultProperty == null)
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
                                elementValue, expProp?.PropertyType ?? dprop.PropertyType)
                            { GenerateGetter = true });
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
        /// Searches the specified XML tree for any elements which are annotated as framework templates.
        /// </summary>
        private static void FindFrameworkTemplateElements(XElement root, IDictionary<String, XElement> elements)
        {
            var annotation = root.Annotation<FrameworkTemplateNameAnnotation>();
            if (annotation != null)
                elements[annotation.Name] = root;

            var children = root.Elements();
            foreach (var child in children)
                FindFrameworkTemplateElements(child, elements);
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
                    var name = $"__Wrapper_{Path.GetFileNameWithoutExtension(file)}_{Guid.NewGuid():N}";
                    var definition = CreateDataSourceDefinitionFromFile(null, name, file);
                    if (definition != null)
                    {
                        UvmlLoader.AddUvmlAnnotations(
                            definition.Value.DataSourceWrapperName, definition.Value.Definition);

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
        private static IEnumerable<DataSourceDefinition> RetrieveTemplateDefinitions(IExpressionCompilerState state)
        {
            if (state.ComponentTemplateManager == null)
                return Enumerable.Empty<DataSourceDefinition>();

            var templateDefs = from template in state.ComponentTemplateManager
                               select DataSourceDefinition.FromComponentTemplate(template.Key, template.Value.Root.Element("View"));
            var templateDefsList = templateDefs.ToList();

            foreach (var templateDef in templateDefsList)
                UvmlLoader.AddUvmlAnnotations(templateDef.DataSourceWrapperName, templateDef.Definition);

            return templateDefsList;
        }

        /// <summary>
        /// Creates an instance of <see cref="DataSourceWrapperInfo"/> for each of the specified data source definitions.
        /// </summary>
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceDefinitions">The collection of <see cref="DataSourceDefinition"/> objects for which to create <see cref="DataSourceWrapperInfo"/> instances.</param>
        /// <returns>A collection containing the <see cref="DataSourceWrapperInfo"/> instances which were created.</returns>
        private static IEnumerable<DataSourceWrapperInfo> RetrieveDataSourceWrapperInfos(IExpressionCompilerState state, IEnumerable<DataSourceDefinition> dataSourceDefinitions)
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
    }
}
