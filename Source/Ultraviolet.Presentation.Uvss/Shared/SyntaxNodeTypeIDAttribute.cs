using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents an attribute which assigns a type identifier to a syntax
    /// node for use during tree serialization.
    /// </summary>
    public sealed class SyntaxNodeTypeIDAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxNodeTypeIDAttribute"/> class.
        /// </summary>
        /// <param name="value">The type's identifier value.</param>
        public SyntaxNodeTypeIDAttribute(Byte value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the type's identifier value.
        /// </summary>
        public Byte Value { get; }
    }
}
