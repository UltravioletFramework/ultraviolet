using System;

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
        public UvssDocumentSyntax(SyntaxList<SyntaxNode> ruleSetAndStoryboardList)
            : base(SyntaxKind.UvssDocument)
        {
            this.RuleSetAndStoryboardList = ruleSetAndStoryboardList;
            ChangeParent(ruleSetAndStoryboardList.Node);

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return RuleSetAndStoryboardList.Node;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The document's list of rule sets and storyboards.
        /// </summary>
        public SyntaxList<SyntaxNode> RuleSetAndStoryboardList { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitDocument(this);
        }
    }
}
