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
        internal UvssNavigationExpressionSyntax(
            SyntaxToken navigationExpressionOperator,
            SyntaxToken propertyName,
            SyntaxToken asKeyword,
            SyntaxToken typeName)
            : base(SyntaxKind.NavigationExpression)
        {
            this.NavigationExpressionOperator = navigationExpressionOperator;
            this.PropertyName = propertyName;
            this.AsKeyword = asKeyword;
            this.TypeName = typeName;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return NavigationExpressionOperator;
                case 1: return PropertyName;
                case 2: return AsKeyword;
                case 3: return TypeName;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The navigation expression's "|" operator.
        /// </summary>
        public SyntaxToken NavigationExpressionOperator;

        /// <summary>
        /// The navigation expression's property name.
        /// </summary>
        public SyntaxToken PropertyName;

        /// <summary>
        /// The navigation expression's "as" keyword.
        /// </summary>
        public SyntaxToken AsKeyword;

        /// <summary>
        /// The navigation expression's conversion type name.
        /// </summary>
        public SyntaxToken TypeName;
    }
}
