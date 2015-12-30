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
            ChangeParent(atSignToken);

            this.NameToken = nameToken;
            ChangeParent(nameToken);

            this.LoopToken = loopToken;
            ChangeParent(loopToken);

            this.Body = body;
            ChangeParent(body);

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
        public SyntaxToken AtSignToken { get; internal set; }

        /// <summary>
        /// The storyboard's name.
        /// </summary>
        public SyntaxToken NameToken { get; internal set; }

        /// <summary>
        /// The storyboard's loop value.
        /// </summary>
        public SyntaxToken LoopToken { get; internal set; }

        /// <summary>
        /// The storyboard's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitStoryboard(this);
        }
    }
}
