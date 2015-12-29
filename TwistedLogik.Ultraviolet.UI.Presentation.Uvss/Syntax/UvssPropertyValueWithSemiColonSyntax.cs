using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value terminated by a semi-colon.
    /// </summary>
    public class UvssPropertyValueWithSemiColonSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithSemiColonSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithSemiColonSyntax(
            SyntaxNode content,
            SyntaxToken semiColon)
            : base(SyntaxKind.PropertyValueWithSemiColon)
        {
            this.Content = content;
            this.SemiColon = semiColon;
            
            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Content;
                case 1: return SemiColon;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The value's content.
        /// </summary>
        public SyntaxNode Content;

        /// <summary>
        /// The value's terminating semi-colon.
        /// </summary>
        public SyntaxToken SemiColon;
    }
}
