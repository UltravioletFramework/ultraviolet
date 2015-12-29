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
        internal UvssSelectorWithParenthesesSyntax()
            : base(SyntaxKind.SelectorWithParentheses)
        {

        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
