using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies a selected class.
    /// </summary>
    public sealed class UvssSelectorPartClassSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartClassSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartClassSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartClassSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartClassSyntax(
            SyntaxToken periodToken,
            UvssIdentifierSyntax selectedClassIdentifier)
            : base(SyntaxKind.SelectorPartClass)
        {
            this.PeriodToken = periodToken;
            ChangeParent(periodToken);

            this.SelectedClassIdentifier = selectedClassIdentifier;
            ChangeParent(selectedClassIdentifier);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PeriodToken;
                case 1: return SelectedClassIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector component's leading period token.
        /// </summary>
        public SyntaxToken PeriodToken { get; internal set; }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected class.
        /// </summary>
        public UvssIdentifierSyntax SelectedClassIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartClass(this);
        }
    }
}
