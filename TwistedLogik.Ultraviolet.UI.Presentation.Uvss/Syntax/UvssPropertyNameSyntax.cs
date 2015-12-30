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
            SyntaxToken attachedPropertyOwnerNameToken,
            SyntaxToken periodToken,
            SyntaxToken propertyNameToken)
            : base(SyntaxKind.PropertyName)
        {
            this.AttachedPropertyOwnerNameToken = attachedPropertyOwnerNameToken;
            this.PeriodToken = periodToken;
            this.PropertyNameToken = propertyNameToken;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedPropertyOwnerNameToken;
                case 1: return PeriodToken;
                case 2: return PropertyNameToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the type which owns the attached property, if this name describes an attached property.
        /// </summary>
        public SyntaxToken AttachedPropertyOwnerNameToken;

        /// <summary>
        /// The period that separates the owner type from the property name.
        /// </summary>
        public SyntaxToken PeriodToken;

        /// <summary>
        /// The name of the property.
        /// </summary>
        public SyntaxToken PropertyNameToken;
    }
}
