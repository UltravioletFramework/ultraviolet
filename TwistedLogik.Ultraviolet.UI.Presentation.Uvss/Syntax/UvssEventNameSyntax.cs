using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event name.
    /// </summary>
    public class UvssEventNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventNameSyntax"/> class.
        /// </summary>
        internal UvssEventNameSyntax(
            SyntaxToken attachedEventOwnerName,
            SyntaxToken period,
            SyntaxToken eventName)
            : base(SyntaxKind.EventName)
        {
            this.AttachedEventOwnerName = attachedEventOwnerName;
            this.Period = period;
            this.EventName = eventName;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedEventOwnerName;
                case 1: return Period;
                case 2: return EventName;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the type which owns the attached event, if this name describes an attached event.
        /// </summary>
        public SyntaxToken AttachedEventOwnerName;

        /// <summary>
        /// The period that separates the owner type from the event name.
        /// </summary>
        public SyntaxToken Period;

        /// <summary>
        /// The name of the event.
        /// </summary>
        public SyntaxToken EventName;
    }
}
