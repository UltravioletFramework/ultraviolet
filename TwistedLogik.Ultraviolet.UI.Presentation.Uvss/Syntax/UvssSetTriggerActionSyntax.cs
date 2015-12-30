using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS set trigger action.
    /// </summary>
    public sealed class UvssSetTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssSetTriggerActionSyntax(
            SyntaxToken setKeyword,
            UvssPropertyNameSyntax propertyName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.SetTriggerAction)
        {
            this.SetKeyword = setKeyword;
            this.PropertyName = propertyName;
            this.Selector = selector;
            this.Value = value;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SetKeyword;
                case 1: return PropertyName;
                case 2: return Selector;
                case 3: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The action's "set" keyword.
        /// </summary>
        public SyntaxToken SetKeyword;

        /// <summary>
        /// The action's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName;

        /// <summary>
        /// The action's optional selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector;

        /// <summary>
        /// The action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value;
    }
}
