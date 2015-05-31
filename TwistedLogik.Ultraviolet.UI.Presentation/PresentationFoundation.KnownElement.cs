using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Represents an element that has been registered with the Ultraviolet Presentation Foundation.
        /// </summary>
        private class KnownElement : KnownType
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KnownElement"/> class.
            /// </summary>
            /// <param name="name">The known element's name in XML.</param>
            /// <param name="type">The known element's type.</param>
            /// <param name="defaultProperty">The element's default property.</param>
            /// <param name="layout">The XML document that defines the element's layout, if it has one.</param>
            public KnownElement(String name, Type type, String defaultProperty, XDocument layout = null)
                : base(name, type)
            {
                this.defaultProperty = defaultProperty;
                this.layout          = layout;
            }

            /// <summary>
            /// Gets the element's default property.
            /// </summary>
            public String DefaultProperty
            {
                get { return defaultProperty; }
            }

            /// <summary>
            /// Gets the XML document that defines the element's layout, if it has one.
            /// </summary>
            public XDocument Layout
            {
                get { return layout; }
            }

            // Property values.
            private readonly String defaultProperty;
            private readonly XDocument layout;
        }
    }
}
