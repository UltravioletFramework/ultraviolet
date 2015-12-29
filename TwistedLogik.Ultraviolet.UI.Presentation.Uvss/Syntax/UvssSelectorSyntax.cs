using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector.
    /// </summary>
    public class UvssSelectorSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax(
            SyntaxNode partsAndCombinators)
            : base(SyntaxKind.Selector)
        {
            this.PartsAndCombinators = partsAndCombinators;

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PartsAndCombinators;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The selector's list of parts and combinators.
        /// </summary>
        public SyntaxNode PartsAndCombinators;
    }
}
