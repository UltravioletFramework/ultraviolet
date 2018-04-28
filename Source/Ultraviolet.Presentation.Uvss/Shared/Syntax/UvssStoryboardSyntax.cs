using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Storyboard)]
    public sealed class UvssStoryboardSyntax : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardSyntax"/> class.
        /// </summary>
        internal UvssStoryboardSyntax()
            : this(null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardSyntax"/> class.
        /// </summary>
        internal UvssStoryboardSyntax(
            SyntaxToken atSignToken,
            UvssIdentifierBaseSyntax nameIdentifier,
            UvssIdentifierBaseSyntax loopIdentifier,
            UvssBlockSyntax body)
            : base(SyntaxKind.Storyboard)
        {
            this.AtSignToken = atSignToken;
            ChangeParent(atSignToken);

            this.NameIdentifier = nameIdentifier;
            ChangeParent(nameIdentifier);

            this.LoopIdentifier = loopIdentifier;
            ChangeParent(loopIdentifier);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssStoryboardSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.AtSignToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.AtSignToken);

            this.NameIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.NameIdentifier);

            this.LoopIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.LoopIdentifier);

            this.Body = reader.ReadSyntaxNode<UvssBlockSyntax>(version);
            ChangeParent(this.Body);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(AtSignToken, version);
            writer.Write(NameIdentifier, version);
            writer.Write(LoopIdentifier, version);
            writer.Write(Body, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AtSignToken;
                case 1: return NameIdentifier;
                case 2: return LoopIdentifier;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the at sign token that marks the declaration as a storyboard.
        /// </summary>
        public SyntaxToken AtSignToken { get; internal set; }

        /// <summary>
        /// Gets the storyboard's name.
        /// </summary>
        public UvssIdentifierBaseSyntax NameIdentifier { get; internal set; }

        /// <summary>
        /// Gets the storyboard's loop value.
        /// </summary>
        public UvssIdentifierBaseSyntax LoopIdentifier { get; internal set; }

        /// <summary>
        /// Gets the storyboard's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitStoryboard(this);
        }
    }
}
