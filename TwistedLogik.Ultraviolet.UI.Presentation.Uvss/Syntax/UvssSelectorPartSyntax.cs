using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector part.
    /// </summary>
    public class UvssSelectorPartSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartSyntax(
            SyntaxNode subParts)
            : base(SyntaxKind.SelectorPart)
        {
            this.SubParts = subParts;

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SubParts;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The selector part's sub-parts.
        /// </summary>
        public SyntaxNode SubParts;
    }
}
