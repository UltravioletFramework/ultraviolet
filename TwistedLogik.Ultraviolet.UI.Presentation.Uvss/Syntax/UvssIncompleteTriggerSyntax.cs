using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a trigger node which does not have enough information to determine
    /// what kind of trigger it should be.
    /// </summary>
    public sealed class UvssIncompleteTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIncompleteTriggerSyntax"/> class.
        /// </summary>
        internal UvssIncompleteTriggerSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIncompleteTriggerSyntax"/> class.
        /// </summary>
        internal UvssIncompleteTriggerSyntax(
            SyntaxToken triggerKeyword)
            : base(SyntaxKind.IncompleteTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            ChangeParent(triggerKeyword);

            SlotCount = 1;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitIncompleteTrigger(this);
        }
    }
}
