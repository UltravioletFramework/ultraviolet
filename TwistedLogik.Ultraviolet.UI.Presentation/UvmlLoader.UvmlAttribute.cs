using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UvmlLoader
    {
        /// <summary>
        /// Represents an attribute on a UVML element.
        /// </summary>
        private class UvmlAttribute
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UvmlAttribute"/> class.
            /// </summary>
            /// <param name="attributeType">A <see cref="UvmlAttributeType"/> value that identifies the type of attribute.</param>
            /// <param name="attachment">The name of the attribute's attachment, if it has one.</param>
            /// <param name="name">The name of the property or event that the attribute represents.</param>
            /// <param name="value">The value of the property or event as declared in the UVML document.</param>
            /// <param name="identifier">The identifier associated with the property or event that the attribute represents, such as a <see cref="DependencyProperty"/> or a <see cref="RoutedEvent"/>.</param>
            public UvmlAttribute(UvmlAttributeType attributeType, String attachment, String name, String value, Object identifier)
            {
                this.attributeType = attributeType;
                this.attachment    = attachment;
                this.name          = name;
                this.value         = value;
                this.identifier    = identifier;
            }

            /// <summary>
            /// Gets a <see cref="UvmlAttributeType"/> value that identifies the type of attribute.
            /// </summary>
            public UvmlAttributeType AttributeType
            {
                get { return attributeType; }
            }

            /// <summary>
            /// Gets the name of the attribute's attachment, if it has one.
            /// </summary>
            public String Attachment
            {
                get { return attachment; }
            }

            /// <summary>
            /// Gets the name of the property or event that the attribute represents.
            /// </summary>
            public String Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets the attribute's value.
            /// </summary>
            public String Value
            {
                get { return value; }
            }

            /// <summary>
            /// Gets the identifier associated with the property or event that the attribute represents, such
            /// as a <see cref="DependencyProperty"/> or a <see cref="RoutedEvent"/>.
            /// </summary>
            public Object Identifier
            {
                get { return identifier; }
            }

            /// <summary>
            /// Gets a value indicating whether this attribute represents a property.
            /// </summary>
            public Boolean IsProperty
            {
                get { return (attributeType == UvmlAttributeType.FrameworkProperty || attributeType == UvmlAttributeType.DependencyProperty); }
            }

            /// <summary>
            /// Gets a value indicating whether this attribute represents an event.
            /// </summary>
            public Boolean IsEvent
            {
                get { return (attributeType == UvmlAttributeType.FrameworkEvent || attributeType == UvmlAttributeType.RoutedEvent); }
            }

            // Property values.
            private readonly UvmlAttributeType attributeType;
            private readonly String attachment;
            private readonly String name;
            private readonly String value;
            private readonly Object identifier;
        }
    }
}
