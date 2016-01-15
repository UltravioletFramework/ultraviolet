using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the indexing operator of a UVSS navigation expression.
    /// </summary>
    public sealed class UvssNavigationExpressionIndexerSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionIndexerSyntax"/> class.
        /// </summary>
        public UvssNavigationExpressionIndexerSyntax()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionIndexerSyntax"/> class.
        /// </summary>
        public UvssNavigationExpressionIndexerSyntax(
            SyntaxToken openBracketToken,
            SyntaxToken numberToken,
            SyntaxToken closeBracketToken)
            : base(SyntaxKind.NavigationExpressionIndexer)
        {
            this.OpenBracketToken = openBracketToken;
            ChangeParent(openBracketToken);

            this.NumberToken = numberToken;
            ChangeParent(numberToken);

            this.CloseBracketToken = closeBracketToken;
            ChangeParent(closeBracketToken);

            this.SlotCount = 3;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenBracketToken;
                case 1: return NumberToken;
                case 2: return CloseBracketToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the index operator's open bracket token.
        /// </summary>
        public SyntaxToken OpenBracketToken { get; internal set; }

        /// <summary>
        /// Gets the index operator's close bracket token.
        /// </summary>
        public SyntaxToken NumberToken { get; internal set; }

        /// <summary>
        /// Gets the index operator's close bracket token.
        /// </summary>
        public SyntaxToken CloseBracketToken { get; internal set; }

        /// <summary>
        /// Gets the index value.
        /// </summary>
        public Int32 Value
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitNavigationExpressionIndex(this);
        }
    }
}
