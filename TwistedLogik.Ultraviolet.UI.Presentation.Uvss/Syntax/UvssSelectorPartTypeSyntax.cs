using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies the selected type.
    /// </summary>
    public sealed class UvssSelectorPartTypeSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartTypeSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartTypeSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartTypeSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartTypeSyntax(
            UvssIdentifierSyntax selectedTypeIdentifier,
            SyntaxToken exclamationMarkToken)
            : base(SyntaxKind.SelectorPartType)
        {
            this.SelectedTypeIdentifier = selectedTypeIdentifier;
            ChangeParent(selectedTypeIdentifier);

            this.ExclamationMarkToken = exclamationMarkToken;
            ChangeParent(exclamationMarkToken);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SelectedTypeIdentifier;
                case 1: return ExclamationMarkToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected type.
        /// </summary>
        public UvssIdentifierSyntax SelectedTypeIdentifier { get; internal set; }

        /// <summary>
        /// Gets the optional exclamation mark token which indicates
        /// that this is an exact type.
        /// </summary>
        public SyntaxToken ExclamationMarkToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartType(this);
        }
    }
}
