using System;
using System.Reflection;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class PresentationFrameworkManager
    {
        /// <summary>
        /// Represents an element that has been registered with the Ultraviolet Presentation Framework.
        /// </summary>
        private class RegisteredElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisteredElement"/> class.
            /// </summary>
            /// <param name="name">The control's name in XML.</param>
            /// <param name="type">The control's type.</param>
            /// <param name="constructor">The control's constructor.</param>
            /// <param name="defaultProperty">The control's default property.</param>
            /// <param name="layout">The XML document that defines the control's layout, if it has one.</param>
            public RegisteredElement(String name, Type type, ConstructorInfo constructor, String defaultProperty, XDocument layout = null)
            {
                this.name            = name;
                this.type            = type;
                this.constructor     = constructor;
                this.defaultProperty = defaultProperty;
                this.layout          = layout;
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

            /// <summary>
            /// Gets the control's constructor.
            /// </summary>
            public ConstructorInfo Constructor
            {
                get { return constructor; }
            }

            /// <summary>
            /// Gets the control's default property.
            /// </summary>
            public String DefaultProperty
            {
                get { return defaultProperty; }
            }

            /// <summary>
            /// Gets the XML document that defines the control's layout, if it has one.
            /// </summary>
            public XDocument Layout
            {
                get { return layout; }
            }

            // Property values.
            private readonly String name;
            private readonly Type type;
            private readonly ConstructorInfo constructor;
            private readonly String defaultProperty;
            private readonly XDocument layout;
        }
    }
}
