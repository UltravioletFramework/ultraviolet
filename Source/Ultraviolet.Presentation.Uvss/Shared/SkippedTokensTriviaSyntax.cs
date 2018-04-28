using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a token that could not be parsed.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SkippedTokensTrivia)]
    public sealed class SkippedTokensTriviaSyntax : StructuredTriviaSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedTokensTriviaSyntax"/> class.
        /// </summary>
        /// <param name="tokens">The node that represents the skipped tokens.</param>
        public SkippedTokensTriviaSyntax(
            SyntaxNode tokens)
            : base(SyntaxKind.SkippedTokensTrivia)
        {
            this.Tokens = tokens;
            ChangeParent(tokens);

            this.SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedTokensTriviaSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public SkippedTokensTriviaSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            Contract.Require(reader, nameof(reader));

            this.Tokens = reader.ReadSyntaxNode(version);
            ChangeParent(this.Tokens);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(Tokens, version);
        }

        /// <summary>
        /// Gets the node that represents the skipped tokens.
        /// </summary>
        public SyntaxNode Tokens { get; internal set; }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            if (index == 0)
                return Tokens;

            throw new InvalidOperationException();
        }        
    }
}
