using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard target.
    /// </summary>
    public class UvssStoryboardTargetSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class.
        /// </summary>
        internal UvssStoryboardTargetSyntax()
            : base(SyntaxKind.StoryboardTarget)
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
