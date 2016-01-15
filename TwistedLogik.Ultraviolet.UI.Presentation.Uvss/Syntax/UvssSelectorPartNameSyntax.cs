using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies the selected name.
    /// </summary>
    public sealed class UvssSelectorPartNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartNameSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartNameSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartNameSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartNameSyntax(
            SyntaxToken hashToken,
            UvssIdentifierSyntax selectedNameIdentifier)
            : base(SyntaxKind.SelectorPartName)
        {
            this.HashToken = hashToken;
            ChangeParent(hashToken);

            this.SelectedNameIdentifier = selectedNameIdentifier;
            ChangeParent(selectedNameIdentifier);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return HashToken;
                case 1: return SelectedNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector component's leading hash token.
        /// </summary>
        public SyntaxToken HashToken { get; internal set; }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected element.
        /// </summary>
        public UvssIdentifierSyntax SelectedNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartName(this);
        }
    }
}
