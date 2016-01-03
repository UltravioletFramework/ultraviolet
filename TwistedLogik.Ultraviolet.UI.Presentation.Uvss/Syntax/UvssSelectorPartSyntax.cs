using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector part.
    /// </summary>
    public sealed class UvssSelectorPartSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartSyntax(
            SyntaxList<UvssSelectorSubPartSyntax> subParts,
            UvssPseudoClassSyntax pseudoClass)
            : base(SyntaxKind.SelectorPart)
        {
            this.SubParts = subParts;
            ChangeParent(subParts.Node);

            this.PseudoClass = pseudoClass;
            ChangeParent(pseudoClass);

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SubParts.Node;
                case 1: return PseudoClass;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the selector part's sub-parts.
        /// </summary>
        public SyntaxList<UvssSelectorSubPartSyntax> SubParts { get; internal set; }

        /// <summary>
        /// Gets the selector's pseudo-class.
        /// </summary>
        public UvssPseudoClassSyntax PseudoClass { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPart(this);
        }
    }
}
