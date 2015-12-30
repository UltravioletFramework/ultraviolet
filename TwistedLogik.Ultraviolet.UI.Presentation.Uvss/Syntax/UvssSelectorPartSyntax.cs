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
            SyntaxList<UvssSelectorSubPartSyntax> subPartsList,
            UvssPseudoClassSyntax pseudoClass)
            : base(SyntaxKind.SelectorPart)
        {
            this.SubPartsList = subPartsList;
            this.PseudoClass = pseudoClass;

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SubPartsList.Node;
                case 1: return PseudoClass;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The selector part's sub-parts.
        /// </summary>
        public SyntaxList<UvssSelectorSubPartSyntax> SubPartsList;

        /// <summary>
        /// The selector's pseudo-class.
        /// </summary>
        public UvssPseudoClassSyntax PseudoClass;
    }
}
