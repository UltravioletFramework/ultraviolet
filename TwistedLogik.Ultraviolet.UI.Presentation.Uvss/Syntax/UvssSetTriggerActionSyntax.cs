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
            ChangeParent(setKeyword);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.Selector = selector;
            ChangeParent(selector);

            this.Value = value;
            ChangeParent(value);

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
        /// Gets the action's "set" keyword.
        /// </summary>
        public SyntaxToken SetKeyword { get; internal set; }

        /// <summary>
        /// Gets the action's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the action's optional selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector { get; internal set; }

        /// <summary>
        /// Gets the action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSetTriggerAction(this);
        }
    }
}
