using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value terminated by a semi-colon.
    /// </summary>
    public sealed class UvssPropertyValueSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueSyntax(SyntaxToken contentToken)
            : base(SyntaxKind.PropertyValue)
        {
            this.ContentToken = contentToken;
            
            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ContentToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The value's content.
        /// </summary>
        public SyntaxToken ContentToken;
    }
}
