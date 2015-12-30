using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector sub-part.
    /// </summary>
    public sealed class UvssSelectorSubPartSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorSubPartSyntax(
            SyntaxToken leadingQualifierToken,
            SyntaxToken textToken,
            SyntaxToken trailingQualifierToken)
            : base(SyntaxKind.SelectorSubPart)
        {
            this.LeadingQualifierToken = leadingQualifierToken;
            ChangeParent(leadingQualifierToken);

            this.TextToken = textToken;
            ChangeParent(textToken);

            this.TrailingQualifierToken = trailingQualifierToken;
            ChangeParent(trailingQualifierToken);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return LeadingQualifierToken;
                case 1: return TextToken;
                case 2: return TrailingQualifierToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The sub-part's leading qualifier.
        /// </summary>
        public SyntaxToken LeadingQualifierToken { get; internal set; }

        /// <summary>
        /// The sub-part's text.
        /// </summary>
        public SyntaxToken TextToken { get; internal set; }

        /// <summary>
        /// The sub-part's trailing qualifier.
        /// </summary>
        public SyntaxToken TrailingQualifierToken { get; internal set; }
    }
}
