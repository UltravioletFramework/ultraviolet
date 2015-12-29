using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector sub-part.
    /// </summary>
    public class UvssSelectorSubPartSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorSubPartSyntax(
            SyntaxToken leadingQualifier,
            SyntaxToken text,
            SyntaxToken trailingQualifier)
            : base(SyntaxKind.SelectorSubPart)
        {
            this.LeadingQualifier = leadingQualifier;
            this.Text = text;
            this.TrailingQualifier = trailingQualifier;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return LeadingQualifier;
                case 1: return Text;
                case 2: return TrailingQualifier;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The sub-part's leading qualifier.
        /// </summary>
        public SyntaxToken LeadingQualifier;

        /// <summary>
        /// The sub-part's text.
        /// </summary>
        public SyntaxToken Text;

        /// <summary>
        /// The sub-part's trailing qualifier.
        /// </summary>
        public SyntaxToken TrailingQualifier;
    }
}
