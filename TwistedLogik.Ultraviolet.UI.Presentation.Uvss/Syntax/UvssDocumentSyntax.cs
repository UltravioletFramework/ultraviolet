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
        public UvssDocumentSyntax(SyntaxList<UvssRuleSetSyntax> ruleSetList)
            : base(SyntaxKind.UvssDocument)
        {
            this.RuleSetList = ruleSetList;
            ChangeParent(ruleSetList.Node);

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return RuleSetList.Node;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The document's list of rule sets.
        /// </summary>
        public SyntaxList<UvssRuleSetSyntax> RuleSetList { get; internal set; }
    }
}
