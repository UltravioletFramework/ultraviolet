using System;
using System.IO;
using System.Xml.Linq;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Represents the definition of a view model or control for which expressions are being compiled.
    /// </summary>
    internal struct DataSourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceDefinition"/> structure.
        /// </summary>
        /// <param name="definitionPath">The path to the file that contains the definition.</param>
        /// <param name="dataSourceIdentifier">A string which identifies the defined data source.</param>
        /// <param name="dataSourceWrapperName">The name of the type which will be generated to wrap the defined data source.</param>
        /// <param name="dataSourceWrapperNamespace">The namespace that contains the type which will be generated to wrap the defined data source.</param>
        /// <param name="templatedControl">The type of the control with which this data source is associated, if any.</param>
        /// <param name="definition">The XML element that defines the data source.</param>
        private DataSourceDefinition(String definitionPath, String dataSourceIdentifier, String dataSourceWrapperName,
            String dataSourceWrapperNamespace, Type templatedControl, XElement definition)
        {
            this.definitionPath = definitionPath;
            this.dataSourceIdentifier = dataSourceIdentifier;
            this.dataSourceWrapperName = dataSourceWrapperName;
            this.dataSourceWrapperNamespace = dataSourceWrapperNamespace;
            this.templatedControl = templatedControl;
            this.definition = definition;
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataSourceDefinition"/> for the specified view.
        /// </summary>
        /// <param name="namespace">The namespace within which to place the compiled view model.</param>
        /// <param name="name">The name of the compiled view model class.</param>
        /// <param name="path">The path to the UVML file that defines the view.</param>
        /// <param name="definition">The XML element found in the file at <paramref name="path"/> that defines the view.</param>
        /// <returns>The <see cref="DataSourceDefinition"/> that was created.</returns>
        public static DataSourceDefinition FromView(String @namespace, String name, String path, XElement definition)
        {
            var dataSourceIdentifier = path;
            var dataSourceWrapperName = name;
            var dataSourceWrapperNamespace = @namespace ?? PresentationFoundationView.DataSourceWrapperNamespaceForViews;

            return new DataSourceDefinition(Path.GetFullPath(path), 
                dataSourceIdentifier, dataSourceWrapperName, dataSourceWrapperNamespace, null, definition);
        }

        /// <summary>
        /// Creates a new instance <see cref="DataSourceDefinition"/> for the specified component template.
        /// </summary>
        /// <param name="templatedControl">The type of control to which the template is applied.</param>
        /// <param name="definition">The XML element that defines the component template.</param>
        /// <returns>The <see cref="DataSourceDefinition"/> that was created.</returns>
        public static DataSourceDefinition FromComponentTemplate(Type templatedControl, XElement definition)
        {
            var dataSourceIdentifier = templatedControl.Name;
            var dataSourceWrapperName = $"__Wrapper_{templatedControl.Name}_{Guid.NewGuid():N}";
            var dataSourceWrapperNamespace = PresentationFoundationView.DataSourceWrapperNamespaceForComponentTemplates;

            return new DataSourceDefinition(templatedControl.FullName,
                dataSourceIdentifier, dataSourceWrapperName, dataSourceWrapperNamespace, templatedControl, definition);
        }

        /// <summary>
        /// Gets the path to the file that contains the definition.
        /// </summary>
        public String DefinitionPath
        {
            get { return definitionPath; }
        }

        /// <summary>
        /// Gets a string which identifies the defined data source.
        /// </summary>
        public String DataSourceIdentifier
        {
            get { return dataSourceIdentifier; }
        }

        /// <summary>
        /// Gets the name of the type which will be generated to wrap the defined data source.
        /// </summary>
        public String DataSourceWrapperName
        {
            get { return dataSourceWrapperName; }
        }

        /// <summary>
        /// Gets the namespace that contains the type which will be generated to wrap the defined data source.
        /// </summary>
        public String DataSourceWrapperNamespace
        {
            get { return dataSourceWrapperNamespace; }
        }

        /// <summary>
        /// Gets the type of the control with which this data source is associated, if any.
        /// </summary>
        public Type TemplatedControl
        {
            get { return templatedControl; }
        }

        /// <summary>
        /// Gets the XML element that defines the data source.
        /// </summary>
        public XElement Definition
        {
            get { return definition; }
        }

        // Property values.
        private readonly String definitionPath;
        private readonly String dataSourceIdentifier;
        private readonly String dataSourceWrapperName;
        private readonly String dataSourceWrapperNamespace;
        private readonly Type templatedControl;
        private readonly XElement definition;
    }
}