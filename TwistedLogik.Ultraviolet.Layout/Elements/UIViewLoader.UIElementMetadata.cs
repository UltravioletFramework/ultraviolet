using System;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    partial class UIViewLoader
    {
        /// <summary>
        /// Represents the metadata for a UI element which can be instantiated by the view loader.
        /// </summary>
        private struct UIElementMetadata
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UIElementMetadata"/> structure.
            /// </summary>
            /// <param name="name">The element's name in XML.</param>
            /// <param name="type">The element's type.</param>
            /// <param name="constructor">The element's constructor.</param>
            /// <param name="defaultProperty">The element's default property.</param>
            public UIElementMetadata(String name, Type type, ConstructorInfo constructor, String defaultProperty)
            {
                this.name            = name;
                this.type            = type;
                this.constructor     = constructor;
                this.defaultProperty = defaultProperty;
            }

            /// <summary>
            /// Gets the element's name in XML.
            /// </summary>
            public String Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets the element's type.
            /// </summary>
            public Type Type
            {
                get { return type; }
            }

            /// <summary>
            /// Gets the element's constructor.
            /// </summary>
            public ConstructorInfo Constructor
            {
                get { return constructor; }
            }

            /// <summary>
            /// Gets the element's default property.
            /// </summary>
            public String DefaultProperty
            {
                get { return defaultProperty; }
            }

            // Property values.
            private readonly String name;
            private readonly Type type;
            private readonly ConstructorInfo constructor;
            private readonly String defaultProperty;
        }
    }
}
