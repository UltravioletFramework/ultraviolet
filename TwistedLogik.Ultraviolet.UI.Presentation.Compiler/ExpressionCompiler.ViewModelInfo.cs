using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    partial class ExpressionCompiler
    {
        /// <summary>
        /// Represents the information needed to generate an inherited view model.
        /// </summary>
        private class ViewModelInfo
        {
            /// <summary>
            /// Gets or sets the view definition for this view model.
            /// </summary>
            public ViewDefinition ViewDefinition
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the type from which the view model inherits.
            /// </summary>
            public Type ViewModelParentType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the source code that was generated for the view model.
            /// </summary>
            public String ViewModelSource
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the name of the view model being generated.
            /// </summary>
            public String ViewModelName
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
}
