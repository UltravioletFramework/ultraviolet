using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a block of nodes enclosed by curly braces.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Block)]
    public sealed class UvssBlockSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssBlockSyntax"/> class.
        /// </summary>
        internal UvssBlockSyntax()
            : this(null, default(SyntaxList<SyntaxNode>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssBlockSyntax"/> class.
        /// </summary>
        internal UvssBlockSyntax(
            SyntaxToken openCurlyBraceToken,
            SyntaxList<SyntaxNode> content,
            SyntaxToken closeCurlyBraceToken)
            : base(SyntaxKind.Block)
        {
            this.OpenCurlyBraceToken = openCurlyBraceToken;
            ChangeParent(openCurlyBraceToken);

            this.Content = content;
            ChangeParent(content.Node);

            this.CloseCurlyBraceToken = closeCurlyBraceToken;
            ChangeParent(closeCurlyBraceToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssBlockSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssBlockSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenCurlyBraceToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenCurlyBraceToken);

            this.Content = reader.ReadSyntaxList<SyntaxNode>(version);
            ChangeParent(this.Content.Node);

            this.CloseCurlyBraceToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseCurlyBraceToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenCurlyBraceToken, version);
            writer.Write(Content, version);
            writer.Write(CloseCurlyBraceToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBraceToken;
                case 1: return Content.Node;
                case 2: return CloseCurlyBraceToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the open curly brace that introduces the block.
        /// </summary>
        public SyntaxToken OpenCurlyBraceToken { get; internal set; }

        /// <summary>
        /// Gets the list of nodes that make up the block's content.
        /// </summary>
        public SyntaxList<SyntaxNode> Content { get; internal set; }

        /// <summary>
        /// Gets the close curly brace that terminates the block.
        /// </summary>
        public SyntaxToken CloseCurlyBraceToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitBlock(this);
        }
    }
}
