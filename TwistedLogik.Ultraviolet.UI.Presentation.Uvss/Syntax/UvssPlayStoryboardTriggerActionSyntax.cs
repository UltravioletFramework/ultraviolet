using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS play-storyboard trigger action.
    /// </summary>
    public class UvssPlayStoryboardTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlayStoryboardTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlayStoryboardTriggerActionSyntax(
            SyntaxToken playStoryboardKeyword,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.PlayStoryboardTriggerAction)
        {
            this.PlayStoryboardKeyword = playStoryboardKeyword;
            this.Selector = selector;
            this.Value = value;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PlayStoryboardKeyword;
                case 1: return Selector;
                case 2: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The action's "play-storyboard" keyword.
        /// </summary>
        public SyntaxToken PlayStoryboardKeyword;

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
