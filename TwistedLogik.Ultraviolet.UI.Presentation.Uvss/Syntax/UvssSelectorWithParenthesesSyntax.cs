using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector with enclosing parentheses.
    /// </summary>
    public sealed class UvssSelectorWithParenthesesSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithParenthesesSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithParenthesesSyntax(
            SyntaxToken openParenToken,
            UvssSelectorSyntax selector,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.SelectorWithParentheses)
        {
            this.OpenParenToken = openParenToken;
            ChangeParent(openParenToken);

            this.Selector = selector;
            ChangeParent(selector);

            this.CloseParenToken = closeParenToken;
            ChangeParent(closeParenToken);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return Selector;
                case 2: return CloseParenToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The open parenthesis that introduces the selector.
        /// </summary>
        public SyntaxToken OpenParenToken { get; internal set; }

        /// <summary>
        /// The enclosed selector.
        /// </summary>
        public UvssSelectorSyntax Selector { get; internal set; }

        /// <summary>
        /// The close parenthesis that terminates the selector.
        /// </summary>
        public SyntaxToken CloseParenToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorWithParentheses(this);
        }
    }
}
