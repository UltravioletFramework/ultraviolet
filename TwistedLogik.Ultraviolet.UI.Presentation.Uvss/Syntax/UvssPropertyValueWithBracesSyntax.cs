using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value enclosed in curly braces.
    /// </summary>
    public class UvssPropertyValueWithBracesSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax(
            SyntaxToken openCurlyBrace,
            SyntaxNode content,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.PropertyValueWithBraces)
        {
            this.OpenCurlyBrace = openCurlyBrace;
            this.Content = content;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBrace;
                case 1: return Content;
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
        public SyntaxNode Content;

        /// <summary>
        /// The value's closing curly brace.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
