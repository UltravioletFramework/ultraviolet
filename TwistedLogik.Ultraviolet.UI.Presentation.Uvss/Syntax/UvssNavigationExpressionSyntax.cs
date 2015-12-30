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
            SyntaxToken typeNameToken)
            : base(SyntaxKind.NavigationExpression)
        {
            this.PipeToken = pipeToken;
            ChangeParent(pipeToken);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.AsKeyword = asKeyword;
            ChangeParent(asKeyword);

            this.TypeNameToken = typeNameToken;
            ChangeParent(typeNameToken);

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
                case 3: return TypeNameToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The navigation expression's pipe token.
        /// </summary>
        public SyntaxToken PipeToken { get; internal set; }

        /// <summary>
        /// The navigation expression's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// The navigation expression's "as" keyword.
        /// </summary>
        public SyntaxToken AsKeyword { get; internal set; }

        /// <summary>
        /// The navigation expression's conversion type name.
        /// </summary>
        public SyntaxToken TypeNameToken { get; internal set; }
    }
}
