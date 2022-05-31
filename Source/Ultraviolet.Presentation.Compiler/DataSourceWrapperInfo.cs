using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Represents the information needed to generate a data source wrapper.
    /// </summary>
    internal class DataSourceWrapperInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceWrapperInfo"/> class.
        /// </summary>
        public DataSourceWrapperInfo()
        {
            UniqueID = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the definition of the data source for which a wrapper is being compiled.
        /// </summary>
        public DataSourceDefinition DataSourceDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the wrapper's list of reference directives.
        /// </summary>
        public IEnumerable<String> References
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the wrapper's list of import directives.
        /// </summary>
        public IEnumerable<String> Imports
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of data source being wrapped.
        /// </summary>
        public Type DataSourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path to the file that defined the data source.
        /// </summary>
        public String DataSourcePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source code that was generated for the data source wrapper.
        /// </summary>
        public String DataSourceWrapperSourceCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the data source wrapper which is being generated.
        /// </summary>
        public String DataSourceWrapperName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data source's list of defined expressions.
        /// </summary>
        public IList<BindingExpressionInfo> Expressions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data source's list of dependent data source wrappers.
        /// </summary>
        public IList<DataSourceWrapperInfo> DependentWrapperInfos
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value which uniquely identifies this wrapper.
        /// </summary>
        public Guid UniqueID
        {
            get;
            private set;
        }
    }
}
