using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents an empty statement.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.EmptyStatement)]
    public sealed class UvssEmptyStatementSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEmptyStatementSyntax"/> class.
        /// </summary>
        internal UvssEmptyStatementSyntax(
            SyntaxToken emptyToken)
            : base(SyntaxKind.EmptyStatement)
        {
            this.EmptyToken = emptyToken;
            ChangeParent(emptyToken);

            this.SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEmptyStatementSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssEmptyStatementSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.EmptyToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.EmptyToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(EmptyToken, version);
        }

        /// <summary>
        /// Gets the empty statement's empty placeholder token.
        /// </summary>
        public SyntaxToken EmptyToken { get; internal set; }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            if (index == 0)
                return EmptyToken;

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEmptyStatement(this);
        }
    }
}
