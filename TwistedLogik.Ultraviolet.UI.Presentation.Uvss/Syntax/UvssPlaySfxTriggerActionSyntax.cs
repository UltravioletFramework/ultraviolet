using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS play-sfx trigger action.
    /// </summary>
    public class UvssPlaySfxTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlaySfxTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlaySfxTriggerActionSyntax(
            SyntaxToken playSfxKeyword,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.PlaySfxTriggerAction)
        {
            this.PlaySfxKeyword = playSfxKeyword;
            this.Value = value;

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PlaySfxKeyword;
                case 1: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The action's "play-sfx" keyword.
        /// </summary>
        public SyntaxToken PlaySfxKeyword;

        /// <summary>
        /// The action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value;
    }
}
