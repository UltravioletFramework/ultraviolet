using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event trigger.
    /// </summary>
    public sealed class UvssEventTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerSyntax"/> class.
        /// </summary>
        internal UvssEventTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken eventKeyword,
            UvssEventNameSyntax eventName,
            UvssEventTriggerArgumentList argumentList,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.EventTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            ChangeParent(triggerKeyword);

            this.EventKeyword = eventKeyword;
            ChangeParent(eventKeyword);

            this.EventName = eventName;
            ChangeParent(eventName);

            this.ArgumentList = argumentList;
            ChangeParent(argumentList);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 6;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return EventKeyword;
                case 2: return EventName;
                case 3: return ArgumentList;
                case 4: return QualifierToken;
                case 5: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's "event" keyword.
        /// </summary>
        public SyntaxToken EventKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's event name.
        /// </summary>
        public UvssEventNameSyntax EventName { get; internal set; }

        /// <summary>
        /// Gets the trigger's argument list.
        /// </summary>
        public UvssEventTriggerArgumentList ArgumentList { get; internal set; }

        /// <summary>
        /// Gets the trigger's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the trigger's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEventTrigger(this);
        }
    }
}
