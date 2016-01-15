using System;
using System.Collections.Generic;

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
        internal UvssRuleSetSyntax()
            : this(default(SeparatedSyntaxList<UvssSelectorWithNavigationExpressionSyntax>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSetSyntax"/> class.
        /// </summary>
        internal UvssRuleSetSyntax(
            SeparatedSyntaxList<UvssSelectorWithNavigationExpressionSyntax> selectors,
            UvssBlockSyntax body)
            : base(SyntaxKind.RuleSet)
        {
            this.Selectors = selectors;
            ChangeParent(selectors.Node);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Selectors.Node;
                case 1: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the rule set's selector list.
        /// </summary>
        public SeparatedSyntaxList<UvssSelectorWithNavigationExpressionSyntax> Selectors { get; internal set; }

        /// <summary>
        /// Gets the rule set's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <summary>
        /// Gets a collection of the rule set's styling rules.
        /// </summary>
        public IEnumerable<UvssRuleSyntax> Rules
        {
            get
            {
                if (Body == null)
                    yield break;

                for (int i = 0; i < Body.Content.Count; i++)
                {
                    var rule = Body.Content[i] as UvssRuleSyntax;
                    if (rule != null)
                        yield return rule;
                }
            }
        }

        /// <summary>
        /// Gets a collection of the rule set's visual transitions.
        /// </summary>
        public IEnumerable<UvssTransitionSyntax> Transitions
        {
            get
            {
                if (Body == null)
                    yield break;

                for (int i = 0; i < Body.Content.Count; i++)
                {
                    var transition = Body.Content[i] as UvssTransitionSyntax;
                    if (transition != null)
                        yield return transition;
                }
            }
        }

        /// <summary>
        /// Gets a collection of the rule set's triggers.
        /// </summary>
        public IEnumerable<UvssTriggerBaseSyntax> Triggers
        {
            get
            {
                if (Body == null)
                    yield break;

                for (int i = 0; i < Body.Content.Count; i++)
                {
                    var trigger = Body.Content[i] as UvssTriggerBaseSyntax;
                    if (trigger != null)
                        yield return trigger;
                }
            }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitRuleSet(this);
        }
    }
}
