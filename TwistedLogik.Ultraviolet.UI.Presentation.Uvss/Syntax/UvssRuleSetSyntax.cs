using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS rule set.
    /// </summary>
    public sealed class UvssRuleSetSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSetSyntax"/> class.
        /// </summary>
        internal UvssRuleSetSyntax(
            SeparatedSyntaxList<UvssSelectorSyntax> selectorList,
            UvssBlockSyntax body)
            : base(SyntaxKind.RuleSet)
        {
            this.SelectorList = selectorList;
            ChangeParent(selectorList.Node);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SelectorList.Node;
                case 1: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The rule set's selector list.
        /// </summary>
        public SeparatedSyntaxList<UvssSelectorSyntax> SelectorList { get; internal set; }

        /// <summary>
        /// The rule set's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitRuleSet(this);
        }
    }
}
