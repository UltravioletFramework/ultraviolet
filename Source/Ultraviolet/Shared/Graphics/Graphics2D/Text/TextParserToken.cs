using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a token produced by parsing formatted text.
    /// </summary>
    public struct TextParserToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserToken"/> structure.
        /// </summary>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="text">The token's text.</param>
        /// <param name="sourceOffset">The offset of the first character in the source text that produced this token.</param>
        /// <param name="sourceLength">The number of characters in the source text that produced this token.</param>
        /// <param name="isNonBreakingSpace">A value indicating whether this token represents a non-breaking space.</param>
        internal TextParserToken(TextParserTokenType tokenType, StringSegment text, Int32 sourceOffset, Int32 sourceLength, Boolean isNonBreakingSpace = false)
        {
            this.TokenType = tokenType;
            this.Text = text;
            this.SourceOffset = sourceOffset;
            this.SourceLength = sourceLength;
            this.IsNonBreakingSpace = isNonBreakingSpace;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var fmt = Text.IsEmpty ? "{0}" : "{0} '{1}'";
            return String.Format(fmt, TokenType, Text);
        }

        /// <summary>
        /// Gets the token's type.
        /// </summary>
        public TextParserTokenType TokenType { get; }

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public StringSegment Text { get; }

        /// <summary>
        /// Gets the offset of the first character in the source text that produced this token.
        /// </summary>
        public Int32 SourceOffset { get; }

        /// <summary>
        /// Gets the number of characters in the source text that produced this token.
        /// </summary>
        public Int32 SourceLength { get; }

        /// <summary>
        /// Gets a value indicating whether this token represents a white space character.
        /// </summary>
        public Boolean IsWhiteSpace
        {
            get { return TokenType == TextParserTokenType.Text && !Text.IsEmpty && Char.IsWhiteSpace(Text[0]); }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents a new line character.
        /// </summary>
        public Boolean IsNewLine
        {
            get
            {
                if (TokenType == TextParserTokenType.Text && !Text.IsEmpty)
                {
                    var c = Text[0];
                    return c == '\n' || c == '\r';
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents non-breaking white space.
        /// </summary>
        public Boolean IsNonBreakingSpace { get; }
    }
}
