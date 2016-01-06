using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the root node of a UVSS document.
    /// </summary>
    public sealed class UvssDocumentSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocumentSyntax"/> class.
        /// </summary>
        internal UvssDocumentSyntax(
            SyntaxList<SyntaxNode> content,
            SyntaxToken endOfFileToken)
            : base(SyntaxKind.Document)
        {
            this.Content = content;
            ChangeParent(content.Node);

            this.EndOfFileToken = endOfFileToken;
            ChangeParent(endOfFileToken);

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Content.Node;
                case 1: return EndOfFileToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the document's content.
        /// </summary>
        public SyntaxList<SyntaxNode> Content { get; internal set; }

        /// <summary>
        /// Gets a collection of the document's rule sets.
        /// </summary>
        public IEnumerable<UvssRuleSetSyntax> RuleSets
        {
            get
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var ruleSet = Content[i] as UvssRuleSetSyntax;
                    if (ruleSet != null)
                        yield return ruleSet;
                }
            }
        }

        /// <summary>
        /// Gets a collection of the document's storyboards.
        /// </summary>
        public IEnumerable<UvssStoryboardSyntax> Storyboards
        {
            get
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var storyboard = Content[i] as UvssStoryboardSyntax;
                    if (storyboard != null)
                        yield return storyboard;
                }
            }
        }

        /// <summary>
        /// Gets the document's end-of-file token.
        /// </summary>
        public SyntaxToken EndOfFileToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitDocument(this);
        }
    }
}
