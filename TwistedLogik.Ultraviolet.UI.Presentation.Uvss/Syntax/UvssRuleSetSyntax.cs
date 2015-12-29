using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS rule set.
    /// </summary>
    public class UvssRuleSetSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSetSyntax"/> class.
        /// </summary>
        internal UvssRuleSetSyntax(
            SyntaxNode selectorList,
            SyntaxToken openCurlyBrace,
            SyntaxNode content,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.RuleSet)
        {
            this.SelectorList = selectorList;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Content = content;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SelectorList;
                case 1: return OpenCurlyBrace;
                case 2: return Content;
                case 3: return CloseCurlyBrace;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The rule set's selector list.
        /// </summary>
        public SyntaxNode SelectorList;

        /// <summary>
        /// The opening curly brace that introduces the rule set's rule list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The rule set's content.
        /// </summary>
        public SyntaxNode Content;

        /// <summary>
        /// The closing curly brace that terminates the rule set's rule list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;        
    }
}
