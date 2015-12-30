using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value enclosed in curly braces.
    /// </summary>
    public sealed class UvssPropertyValueWithBracesSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax(
            SyntaxToken openCurlyBraceToken,
            SyntaxToken contentToken,
            SyntaxToken closeCurlyBraceToken)
            : base(SyntaxKind.PropertyValueWithBraces)
        {
            this.OpenCurlyBrace = openCurlyBraceToken;
            this.ContentToken = contentToken;
            this.CloseCurlyBrace = closeCurlyBraceToken;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBrace;
                case 1: return ContentToken;
                case 2: return CloseCurlyBrace;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The value's opening curly brace.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The value's content.
        /// </summary>
        public SyntaxToken ContentToken;

        /// <summary>
        /// The value's closing curly brace.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
