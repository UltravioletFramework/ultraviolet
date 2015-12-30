using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value terminated by a semi-colon.
    /// </summary>
    public sealed class UvssPropertyValueWithSemiColonSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithSemiColonSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithSemiColonSyntax(
            SyntaxToken contentToken,
            SyntaxToken semiColonToken)
            : base(SyntaxKind.PropertyValueWithSemiColon)
        {
            this.ContentToken = contentToken;
            this.SemiColonToken = semiColonToken;
            
            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ContentToken;
                case 1: return SemiColonToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The value's content.
        /// </summary>
        public SyntaxToken ContentToken;

        /// <summary>
        /// The value's terminating semi-colon.
        /// </summary>
        public SyntaxToken SemiColonToken;
    }
}
