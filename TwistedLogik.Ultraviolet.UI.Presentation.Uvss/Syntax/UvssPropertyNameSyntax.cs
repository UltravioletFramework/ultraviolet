using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property name.
    /// </summary>
    public sealed class UvssPropertyNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyNameSyntax"/> class.
        /// </summary>
        internal UvssPropertyNameSyntax(
            UvssIdentifierBaseSyntax attachedPropertyOwnerNameIdentifier,
            SyntaxToken periodToken,
            UvssIdentifierBaseSyntax propertyNameIdentifier)
            : base(SyntaxKind.PropertyName)
        {
            this.AttachedPropertyOwnerNameIdentifier = attachedPropertyOwnerNameIdentifier;
            ChangeParent(attachedPropertyOwnerNameIdentifier);

            this.PeriodToken = periodToken;
            ChangeParent(periodToken);

            this.PropertyNameIdentifier = propertyNameIdentifier;
            ChangeParent(propertyNameIdentifier);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedPropertyOwnerNameIdentifier;
                case 1: return PeriodToken;
                case 2: return PropertyNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the name of the type which owns the attached property, if this name describes an attached property.
        /// </summary>
        public UvssIdentifierBaseSyntax AttachedPropertyOwnerNameIdentifier { get; internal set; }

        /// <summary>
        /// Gets the period that separates the owner type from the property name.
        /// </summary>
        public SyntaxToken PeriodToken { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public UvssIdentifierBaseSyntax PropertyNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyName(this);
        }
    }
}
