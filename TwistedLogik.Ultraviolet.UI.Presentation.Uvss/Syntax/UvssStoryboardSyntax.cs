using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard.
    /// </summary>
    public class UvssStoryboardSyntax : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardSyntax"/> class.
        /// </summary>
        internal UvssStoryboardSyntax(
            SyntaxToken storyboardKeyword,
            SyntaxToken loop,
            SyntaxToken openCurlyBrace,
            SyntaxNode content,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.Storyboard)
        {
            this.StoryboardKeyword = storyboardKeyword;
            this.Loop = loop;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Content = content;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 5;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return StoryboardKeyword;
                case 1: return Loop;
                case 2: return OpenCurlyBrace;
                case 3: return Content;
                case 4: return CloseCurlyBrace;
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
        /// The open curly brace which introduces the storyboard's target list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The storyboard's content.
        /// </summary>
        public SyntaxNode Content;

        /// <summary>
        /// The close curly brace which terminates the storyboard's target list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
