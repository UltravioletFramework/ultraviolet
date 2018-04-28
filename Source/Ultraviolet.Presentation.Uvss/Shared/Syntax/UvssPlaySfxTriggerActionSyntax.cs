using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS play-sfx trigger action.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PlaySfxTriggerAction)]
    public sealed class UvssPlaySfxTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlaySfxTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlaySfxTriggerActionSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlaySfxTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlaySfxTriggerActionSyntax(
            SyntaxToken playSfxKeyword,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.PlaySfxTriggerAction)
        {
            this.PlaySfxKeyword = playSfxKeyword;
            ChangeParent(playSfxKeyword);

            this.Value = value;
            ChangeParent(value);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlaySfxTriggerActionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPlaySfxTriggerActionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PlaySfxKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PlaySfxKeyword);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.Value);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PlaySfxKeyword, version);
            writer.Write(Value, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PlaySfxKeyword;
                case 1: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the action's "play-sfx" keyword.
        /// </summary>
        public SyntaxToken PlaySfxKeyword { get; internal set; }

        /// <summary>
        /// Gets the action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPlaySfxTriggerAction(this);
        }
    }
}
