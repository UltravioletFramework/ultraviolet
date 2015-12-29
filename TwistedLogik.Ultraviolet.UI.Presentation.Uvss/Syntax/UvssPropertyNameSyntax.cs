using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property name.
    /// </summary>
    public class UvssPropertyNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyNameSyntax"/> class.
        /// </summary>
        internal UvssPropertyNameSyntax(
            SyntaxToken attachedPropertyOwnerName,
            SyntaxToken period,
            SyntaxToken propertyName)
            : base(SyntaxKind.PropertyName)
        {
            this.AttachedPropertyOwnerName = attachedPropertyOwnerName;
            this.Period = period;
            this.PropertyName = propertyName;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedPropertyOwnerName;
                case 1: return Period;
                case 2: return PropertyName;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the type which owns the attached property, if this name describes an attached property.
        /// </summary>
        public SyntaxToken AttachedPropertyOwnerName;

        /// <summary>
        /// The period that separates the owner type from the property name.
        /// </summary>
        public SyntaxToken Period;

        /// <summary>
        /// The name of the property.
        /// </summary>
        public SyntaxToken PropertyName;
    }
}
