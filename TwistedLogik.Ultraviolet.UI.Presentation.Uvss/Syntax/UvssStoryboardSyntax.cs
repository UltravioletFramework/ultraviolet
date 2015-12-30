using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard.
    /// </summary>
    public sealed class UvssStoryboardSyntax : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardSyntax"/> class.
        /// </summary>
        internal UvssStoryboardSyntax(
            SyntaxToken storyboardKeyword,
            SyntaxToken loopToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.Storyboard)
        {
            this.StoryboardKeyword = storyboardKeyword;
            this.Loop = loopToken;
            this.Body = body;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return StoryboardKeyword;
                case 1: return Loop;
                case 2: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The storyboard's "storyboard" keyword.
        /// </summary>
        public SyntaxToken StoryboardKeyword;

        /// <summary>
        /// The storyboard's loop value.
        /// </summary>
        public SyntaxToken Loop;

        /// <summary>
        /// The storyboard's body.
        /// </summary>
        public UvssBlockSyntax Body;
    }
}
