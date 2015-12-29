using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event trigger.
    /// </summary>
    public class UvssEventTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerSyntax"/> class.
        /// </summary>
        internal UvssEventTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken eventKeyword,
            UvssEventNameSyntax eventName,
            SyntaxToken openParen,
            SyntaxNode arguments,
            SyntaxToken closeParen,
            SyntaxToken openCurlyBrace,
            SyntaxNode actions,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.EventTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            this.EventKeyword = eventKeyword;
            this.EventName = eventName;
            this.OpenParen = openParen;
            this.Arguments = arguments;
            this.CloseParen = closeParen;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Actions = actions;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 9;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return EventKeyword;
                case 2: return EventName;
                case 3: return OpenParen;
                case 4: return Arguments;
                case 5: return CloseParen;
                case 6: return OpenCurlyBrace;
                case 7: return Actions;
                case 8: return CloseCurlyBrace;
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
        /// The opening parenthesis that introduces the trigger's argument list.
        /// </summary>
        public SyntaxToken OpenParen;

        /// <summary>
        /// The trigger's argument list.
        /// </summary>
        public SyntaxNode Arguments;

        /// <summary>
        /// The closing parenthesis that terminates the trigger's argument list.
        /// </summary>
        public SyntaxToken CloseParen;

        /// <summary>
        /// The opening curly brace that introduces the trigger's action list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The trigger's action list.
        /// </summary>
        public SyntaxNode Actions;

        /// <summary>
        /// The closing curly brace that terminates the trigger's action list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
