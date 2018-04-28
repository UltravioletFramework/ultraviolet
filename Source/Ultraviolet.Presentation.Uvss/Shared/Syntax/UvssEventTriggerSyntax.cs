using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event trigger.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.EventTrigger)]
    public sealed class UvssEventTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerSyntax"/> class.
        /// </summary>
        internal UvssEventTriggerSyntax()
            : this(null, null, null, null, null, null)
        { }
        
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
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssEventTriggerSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.TriggerKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TriggerKeyword);

            this.EventKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.EventKeyword);

            this.EventName = reader.ReadSyntaxNode<UvssEventNameSyntax>(version);
            ChangeParent(this.EventName);

            this.ArgumentList = reader.ReadSyntaxNode<UvssEventTriggerArgumentList>(version);
            ChangeParent(this.ArgumentList);

            this.QualifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.QualifierToken);

            this.Body = reader.ReadSyntaxNode<UvssBlockSyntax>(version);
            ChangeParent(this.Body);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(TriggerKeyword, version);
            writer.Write(EventKeyword, version);
            writer.Write(EventName, version);
            writer.Write(ArgumentList, version);
            writer.Write(QualifierToken, version);
            writer.Write(Body, version);
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
        public override IEnumerable<UvssTriggerActionBaseSyntax> Actions
        {
            get
            {
                if (Body != null)
                {
                    for (int i = 0; i < Body.Content.Count; i++)
                    {
                        var node = Body.Content[i] as UvssTriggerActionBaseSyntax;
                        if (node != null)
                            yield return node;
                    }
                }
            }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEventTrigger(this);
        }
    }
}
