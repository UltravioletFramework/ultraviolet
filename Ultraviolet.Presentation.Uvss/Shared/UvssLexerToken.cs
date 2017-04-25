using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents one of the tokens produced by the UVML lexer.
    /// </summary>
    public struct UvssLexerToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssLexerToken"/> structure.
        /// </summary>
        /// <param name="type">A <see cref="UvssLexerTokenType"/> value that indicates the token's type.</param>
        /// <param name="sourceOffset">The token's offset within the source text.</param>
        /// <param name="sourceLength">The token's length within the source text.</param>
        /// <param name="sourceLine">The index of the line on which the token appears in the source text.</param>
        /// <param name="sourceColumn">The index of the column in which the token appears in the source text.</param>
        /// <param name="text">The token text.</param>
        public UvssLexerToken(UvssLexerTokenType type,
            Int32 sourceOffset, Int32 sourceLength, Int32 sourceLine, Int32 sourceColumn, String text)
        {
            this.Type = type;
            this.SourceOffset = sourceOffset;
            this.SourceLength = sourceLength;
            this.SourceLine = sourceLine;
            this.SourceColumn = sourceColumn;
            this.Text = text;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{Type} \"{Text}\" ({SourceLine}, {SourceColumn})";

        /// <summary>
        /// Gets the token's type.
        /// </summary>
        public UvssLexerTokenType Type { get; }

        /// <summary>
        /// Gets the token's offset within the source text.
        /// </summary>
        public Int32 SourceOffset { get; }

        /// <summary>
        /// Gets the token's length within the source text.
        /// </summary>
        public Int32 SourceLength { get; }

        /// <summary>
        /// Gets the index of the line on which the token appears in the source text.
        /// </summary>
        public Int32 SourceLine { get; }

        /// <summary>
        /// Gets the index of the column in which the token appears in the source text.
        /// </summary>
        public Int32 SourceColumn { get; }

        /// <summary>
        /// Gets the token text.
        /// </summary>
        public String Text { get; }
    }
}
