using System;

namespace Ultraviolet.Presentation
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
            public KnownElement(String name, Type type, String defaultProperty)
                : base(name, type)
            {
                this.defaultProperty = defaultProperty;
            }

            /// <summary>
            /// Gets the element's default property.
            /// </summary>
            public String DefaultProperty
            {
                get { return defaultProperty; }
            }
            
            // Property values.
            private readonly String defaultProperty;
        }
    }
}
