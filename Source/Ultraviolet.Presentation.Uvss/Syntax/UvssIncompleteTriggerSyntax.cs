using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a trigger node which does not have enough information to determine
    /// what kind of trigger it should be.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.IncompleteTrigger)]
    public sealed class UvssIncompleteTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIncompleteTriggerSyntax"/> class.
        /// </summary>
        internal UvssIncompleteTriggerSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIncompleteTriggerSyntax"/> class.
        /// </summary>
        internal UvssIncompleteTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.IncompleteTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            ChangeParent(triggerKeyword);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIncompleteTriggerSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssIncompleteTriggerSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.TriggerKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TriggerKeyword);

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
            writer.Write(QualifierToken, version);
            writer.Write(Body, version);
        }
        
        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return QualifierToken;
                case 2: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the trigger's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        public override IEnumerable<UvssTriggerActionBaseSyntax> Actions =>
            Enumerable.Empty<UvssTriggerActionBaseSyntax>();

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitIncompleteTrigger(this);
        }
    }
}
