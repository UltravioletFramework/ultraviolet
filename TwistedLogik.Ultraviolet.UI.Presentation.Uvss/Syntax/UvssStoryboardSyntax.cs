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
        internal UvssStoryboardSyntax()
            : this(null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardSyntax"/> class.
        /// </summary>
        internal UvssStoryboardSyntax(
            SyntaxToken atSignToken,
            UvssIdentifierBaseSyntax nameIdentifier,
            UvssIdentifierBaseSyntax loopIdentifier,
            UvssBlockSyntax body)
            : base(SyntaxKind.Storyboard)
        {
            this.AtSignToken = atSignToken;
            ChangeParent(atSignToken);

            this.NameIdentifier = nameIdentifier;
            ChangeParent(nameIdentifier);

            this.LoopIdentifier = loopIdentifier;
            ChangeParent(loopIdentifier);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AtSignToken;
                case 1: return NameIdentifier;
                case 2: return LoopIdentifier;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the at sign token that marks the declaration as a storyboard.
        /// </summary>
        public SyntaxToken AtSignToken { get; internal set; }

        /// <summary>
        /// Gets the storyboard's name.
        /// </summary>
        public UvssIdentifierBaseSyntax NameIdentifier { get; internal set; }

        /// <summary>
        /// Gets the storyboard's loop value.
        /// </summary>
        public UvssIdentifierBaseSyntax LoopIdentifier { get; internal set; }

        /// <summary>
        /// Gets the storyboard's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitStoryboard(this);
        }
    }
}
