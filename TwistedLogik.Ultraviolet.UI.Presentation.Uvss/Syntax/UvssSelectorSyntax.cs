using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector.
    /// </summary>
    public sealed class UvssSelectorSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax(SyntaxList<SyntaxNode> partsAndCombinatorsList)
            : base(SyntaxKind.Selector)
        {
            this.PartsAndCombinatorsList = partsAndCombinatorsList;
            ChangeParent(partsAndCombinatorsList.Node);

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PartsAndCombinatorsList.Node;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The selector's list of parts and combinators.
        /// </summary>
        public SyntaxList<SyntaxNode> PartsAndCombinatorsList { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelector(this);
        }
    }
}
