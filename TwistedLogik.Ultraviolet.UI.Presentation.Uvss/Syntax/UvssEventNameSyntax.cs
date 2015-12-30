using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event name.
    /// </summary>
    public sealed class UvssEventNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventNameSyntax"/> class.
        /// </summary>
        internal UvssEventNameSyntax(
            SyntaxToken attachedEventOwnerNameToken,
            SyntaxToken periodToken,
            SyntaxToken eventNameToken)
            : base(SyntaxKind.EventName)
        {
            this.AttachedEventOwnerNameToken = attachedEventOwnerNameToken;
            this.PeriodToken = periodToken;
            this.EventNameToken = eventNameToken;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedEventOwnerNameToken;
                case 1: return PeriodToken;
                case 2: return EventNameToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the type which owns the attached event, if this name describes an attached event.
        /// </summary>
        public SyntaxToken AttachedEventOwnerNameToken;

        /// <summary>
        /// The period that separates the owner type from the event name.
        /// </summary>
        public SyntaxToken PeriodToken;

        /// <summary>
        /// The name of the event.
        /// </summary>
        public SyntaxToken EventNameToken;
    }
}
