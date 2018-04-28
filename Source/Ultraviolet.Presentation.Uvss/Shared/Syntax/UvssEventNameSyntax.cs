using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event name.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.EventName)]
    public sealed class UvssEventNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventNameSyntax"/> class.
        /// </summary>
        internal UvssEventNameSyntax()
            : this(null, null, null)
        { }

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
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventNameSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssEventNameSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.AttachedEventOwnerNameIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.AttachedEventOwnerNameIdentifier);

            this.PeriodToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PeriodToken);

            this.EventNameIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.EventNameIdentifier);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(AttachedEventOwnerNameIdentifier, version);
            writer.Write(PeriodToken, version);
            writer.Write(EventNameIdentifier, version);
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
