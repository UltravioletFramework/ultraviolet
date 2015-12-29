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
        internal UvssPlayStoryboardTriggerActionSyntax()
            : base(SyntaxKind.PlayStoryboardTriggerAction)
        {

        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
