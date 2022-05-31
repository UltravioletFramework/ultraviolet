using System;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Represents an type that has been registered with the Ultraviolet Presentation Foundation.
        /// </summary>
        private class KnownType
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KnownType"/> class.
            /// </summary>
            /// <param name="name">The known type's name in XML.</param>
            /// <param name="type">The known type.</param>
            public KnownType(String name, Type type)
            {
                this.name            = name;
                this.type            = type;
            }

            /// <summary>
            /// Gets the control's name in XML.
            /// </summary>
            public String Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets the control's type.
            /// </summary>
            public Type Type
            {
                get { return type; }
            }

            // Property values.
            private readonly String name;
            private readonly Type type;
        }
    }
}
