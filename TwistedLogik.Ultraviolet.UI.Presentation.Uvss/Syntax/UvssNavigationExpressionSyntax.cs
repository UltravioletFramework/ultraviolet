using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS navigation expression.
    /// </summary>
    public sealed class UvssNavigationExpressionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionSyntax(
            SyntaxToken pipeToken,
            UvssPropertyNameSyntax propertyName,
            SyntaxToken asKeyword,
            UvssIdentifierBaseSyntax typeNameIdentifier)
            : base(SyntaxKind.NavigationExpression)
        {
            this.PipeToken = pipeToken;
            ChangeParent(pipeToken);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.AsKeyword = asKeyword;
            ChangeParent(asKeyword);

            this.TypeNameIdentifier = typeNameIdentifier;
            ChangeParent(typeNameIdentifier);

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PipeToken;
                case 1: return PropertyName;
                case 2: return AsKeyword;
                case 3: return TypeNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the navigation expression's pipe token.
        /// </summary>
        public SyntaxToken PipeToken { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's "as" keyword.
        /// </summary>
        public SyntaxToken AsKeyword { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's conversion type name.
        /// </summary>
        public UvssIdentifierBaseSyntax TypeNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitNavigationExpression(this);
        }
    }
}
