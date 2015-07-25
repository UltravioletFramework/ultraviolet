using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    partial class ExpressionCompiler
    {
        /// <summary>
        /// Represents a view definition which is being compiled.
        /// </summary>
        private struct ViewDefinition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ViewDefinition"/> structure.
            /// </summary>
            /// <param name="path">The path to the file that defines the view.</param>
            /// <param name="definition">The XML element that defines the view.</param>
            public ViewDefinition(String path, XElement definition)
            {
                this.path = path;
                this.definition = definition;
            }

            /// <summary>
            /// Gets the path to the file that defines the view.
            /// </summary>
            public String Path
            {
                get { return path; }
            }

            /// <summary>
            /// Gets the XML element that defines the view.
            /// </summary>
            public XElement Definition
            {
                get { return definition; }
            }

            // Property values.
            private readonly String path;
            private readonly XElement definition;
        }
    }
}
