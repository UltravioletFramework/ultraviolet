using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS rule.
    /// </summary>
    public class UvssRuleSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSyntax"/> class.
        /// </summary>
        internal UvssRuleSyntax(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken colon,
            UvssPropertyValueWithSemiColonSyntax value)
            : base(SyntaxKind.Rule)
        {
            this.PropertyName = propertyName;
            this.Colon = colon;
            this.Value = value;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PropertyName;
                case 1: return Colon;
                case 2: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the property being styled.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName;

        /// <summary>
        /// The colon that separates the property name from its value.
        /// </summary>
        public SyntaxToken Colon;

        /// <summary>
        /// The styled property value.
        /// </summary>
        public UvssPropertyValueWithSemiColonSyntax Value;
    }
}
