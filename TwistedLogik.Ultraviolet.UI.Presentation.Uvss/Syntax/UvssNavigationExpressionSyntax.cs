using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS navigation expression.
    /// </summary>
    public class UvssNavigationExpressionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionSyntax()
            : base(SyntaxKind.NavigationExpression)
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
