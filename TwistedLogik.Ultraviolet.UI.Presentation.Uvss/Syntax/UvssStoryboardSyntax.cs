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
            SyntaxToken atSignToken,
            SyntaxToken nameToken,
            SyntaxToken loopToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.Storyboard)
        {
            this.AtSignToken = atSignToken;
            this.NameToken = nameToken;
            this.LoopToken = loopToken;
            this.Body = body;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AtSignToken;
                case 1: return NameToken;
                case 2: return LoopToken;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The at sign token that marks the declaration as a storyboard.
        /// </summary>
        public SyntaxToken AtSignToken;

        /// <summary>
        /// The storyboard's name.
        /// </summary>
        public SyntaxToken NameToken;

        /// <summary>
        /// The storyboard's loop value.
        /// </summary>
        public SyntaxToken LoopToken;

        /// <summary>
        /// The storyboard's body.
        /// </summary>
        public UvssBlockSyntax Body;
    }
}
