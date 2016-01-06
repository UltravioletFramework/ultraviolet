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
            UvssIdentifierBaseSyntax attachedEventOwnerNameIdentifier,
            SyntaxToken periodToken,
            UvssIdentifierBaseSyntax eventNameIdentifier)
            : base(SyntaxKind.EventName)
        {
            this.AttachedEventOwnerNameIdentifier = attachedEventOwnerNameIdentifier;
            ChangeParent(attachedEventOwnerNameIdentifier);

            this.PeriodToken = periodToken;
            ChangeParent(periodToken);

            this.EventNameIdentifier = eventNameIdentifier;
            ChangeParent(eventNameIdentifier);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AttachedEventOwnerNameIdentifier;
                case 1: return PeriodToken;
                case 2: return EventNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the name of the type which owns the attached event, if this name describes an attached event.
        /// </summary>
        public UvssIdentifierBaseSyntax AttachedEventOwnerNameIdentifier { get; internal set; }

        /// <summary>
        /// Gets the period that separates the owner type from the event name.
        /// </summary>
        public SyntaxToken PeriodToken { get; internal set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public UvssIdentifierBaseSyntax EventNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEventName(this);
        }
    }
}
