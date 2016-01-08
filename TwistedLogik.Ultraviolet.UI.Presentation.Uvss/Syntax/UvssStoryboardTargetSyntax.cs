using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard target.
    /// </summary>
    public sealed class UvssStoryboardTargetSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class.
        /// </summary>
        internal UvssStoryboardTargetSyntax()
            : this(null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class.
        /// </summary>
        internal UvssStoryboardTargetSyntax(
            SyntaxToken targetKeyword,
            UvssIdentifierBaseSyntax typeNameIdentifier,
            UvssSelectorWithParenthesesSyntax selector,
            UvssBlockSyntax body)
            : base(SyntaxKind.StoryboardTarget)
        {
            this.TargetKeyword = targetKeyword;
            ChangeParent(targetKeyword);

            this.TypeNameIdentifier = typeNameIdentifier;
            ChangeParent(typeNameIdentifier);

            this.Selector = selector;
            ChangeParent(selector);

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
                case 0: return TargetKeyword;
                case 1: return TypeNameIdentifier;
                case 2: return Selector;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the target's "target" keyword.
        /// </summary>
        public SyntaxToken TargetKeyword { get; internal set; }

        /// <summary>
        /// Gets the target's optional type name.
        /// </summary>
        public UvssIdentifierBaseSyntax TypeNameIdentifier { get; internal set; }

        /// <summary>
        /// Gets the target's selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector { get; internal set; }

        /// <summary>
        /// Gets the target's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitStoryboardTarget(this);
        }
    }
}
