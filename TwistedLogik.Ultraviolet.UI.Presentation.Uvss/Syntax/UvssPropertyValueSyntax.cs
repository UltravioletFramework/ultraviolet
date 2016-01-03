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
            ChangeParent(contentToken);

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
        /// Gets the value's content token.
        /// </summary>
        public SyntaxToken ContentToken { get; internal set; }

        /// <inheritdoc/>
        public override String Value
        {
            get { return ContentToken?.Text; }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyValue(this);
        }
    }
}
