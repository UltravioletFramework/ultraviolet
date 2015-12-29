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
        internal UvssStoryboardTargetSyntax(
            SyntaxToken targetKeyword,
            SyntaxToken typeName,
            UvssSelectorWithParenthesesSyntax selector,
            SyntaxToken openCurlyBrace,
            SyntaxNode content,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.StoryboardTarget)
        {
            this.TargetKeyword = targetKeyword;
            this.TypeName = typeName;
            this.Selector = selector;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Content = content;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 6;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TargetKeyword;
                case 1: return TypeName;
                case 2: return Selector;
                case 3: return OpenCurlyBrace;
                case 4: return Content;
                case 5: return CloseCurlyBrace;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The target's "target" keyword.
        /// </summary>
        public SyntaxToken TargetKeyword;

        /// <summary>
        /// The target's optional type name.
        /// </summary>
        public SyntaxToken TypeName;

        /// <summary>
        /// The target's selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector;

        /// <summary>
        /// The open curly brace that introduces the target's animation list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The target's content.
        /// </summary>
        public SyntaxNode Content;

        /// <summary>
        /// The close curly brace that terminates the target's animation list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
