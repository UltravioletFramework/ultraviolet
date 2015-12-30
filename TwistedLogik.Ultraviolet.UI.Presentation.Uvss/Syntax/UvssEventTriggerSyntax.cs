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
            UvssBlockSyntax body)
            : base(SyntaxKind.EventTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            this.EventKeyword = eventKeyword;
            this.EventName = eventName;
            this.ArgumentList = argumentList;
            this.Body = body;

            SlotCount = 5;
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
                case 4: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword;

        /// <summary>
        /// The trigger's "event" keyword.
        /// </summary>
        public SyntaxToken EventKeyword;

        /// <summary>
        /// The trigger's event name.
        /// </summary>
        public UvssEventNameSyntax EventName;

        /// <summary>
        /// The trigger's argument list.
        /// </summary>
        public UvssEventTriggerArgumentList ArgumentList;

        /// <summary>
        /// The trigger's body.
        /// </summary>
        public UvssBlockSyntax Body;
    }
}
