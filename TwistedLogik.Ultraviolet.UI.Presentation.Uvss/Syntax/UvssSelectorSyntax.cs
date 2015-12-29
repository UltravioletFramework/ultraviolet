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
        internal UvssSelectorSyntax()
            : base(SyntaxKind.Selector)
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
