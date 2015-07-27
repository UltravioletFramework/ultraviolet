using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Represents the information needed to generate a view model wrapper.
    /// </summary>
    internal class ViewModelWrapperInfo
    {
        /// <summary>
        /// Gets or sets the view definition for the view model being compiled.
        /// </summary>
        public ViewDefinition ViewDefinition
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
        /// Gets or sets the type of view model being wrapped.
        /// </summary>
        public Type ViewModelType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source code that was generated for the view model wrapper.
        /// </summary>
        public String ViewModelWrapperSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the view model wrapper which is being generated.
        /// </summary>
        public String ViewModelWrapperName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view model's list of defined expressions.
        /// </summary>
        public IList<BindingExpressionInfo> Expressions
        {
            get;
            set;
        }
    }
}
