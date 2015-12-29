using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector with enclosing parentheses.
    /// </summary>
    public class UvssSelectorWithParenthesesSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithParenthesesSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithParenthesesSyntax(
            SyntaxToken openParen,
            UvssSelectorSyntax selector,
            SyntaxToken closeParen)
            : base(SyntaxKind.SelectorWithParentheses)
        {
            this.OpenParen = openParen;
            this.Selector = selector;
            this.CloseParen = closeParen;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParen;
                case 1: return Selector;
                case 2: return CloseParen;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The open parenthesis that introduces the selector.
        /// </summary>
        public SyntaxToken OpenParen;

        /// <summary>
        /// The enclosed selector.
        /// </summary>
        public UvssSelectorSyntax Selector;

        /// <summary>
        /// The close parenthesis that terminates the selector.
        /// </summary>
        public SyntaxToken CloseParen;
    }
}
