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
        internal UvssSelectorSubPartSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorSubPartSyntax(
            SyntaxToken leadingQualifierToken,
            UvssIdentifierBaseSyntax subPartIdentifier,
            SyntaxToken trailingQualifierToken)
            : base(SyntaxKind.SelectorSubPart)
        {
            this.LeadingQualifierToken = leadingQualifierToken;
            ChangeParent(leadingQualifierToken);

            this.SubPartIdentifier = subPartIdentifier;
            ChangeParent(subPartIdentifier);

            this.TrailingQualifierToken = trailingQualifierToken;
            ChangeParent(trailingQualifierToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return LeadingQualifierToken;
                case 1: return SubPartIdentifier;
                case 2: return TrailingQualifierToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the sub-part's leading qualifier.
        /// </summary>
        public SyntaxToken LeadingQualifierToken { get; internal set; }

        /// <summary>
        /// Gets the sub-part's identifier.
        /// </summary>
        public UvssIdentifierBaseSyntax SubPartIdentifier { get; internal set; }

        /// <summary>
        /// Gets the sub-part's trailing qualifier.
        /// </summary>
        public SyntaxToken TrailingQualifierToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorSubPart(this);
        }
    }
}
