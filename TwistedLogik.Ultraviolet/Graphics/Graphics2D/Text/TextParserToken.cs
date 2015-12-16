using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
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
            this.tokenType = tokenType;
            this.text = text;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
            this.isNonBreakingSpace = isNonBreakingSpace;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var fmt = text.IsEmpty ? "{0}" : "{0} '{1}'";
            return String.Format(fmt, tokenType, text);
        }

        /// <summary>
        /// Gets the token's type.
        /// </summary>
        public TextParserTokenType TokenType
        {
            get { return tokenType; }
        }

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public StringSegment Text
        {
            get { return text; }
        }

        /// <summary>
        /// Gets the offset of the first character in the source text that produced this token.
        /// </summary>
        public Int32 SourceOffset
        {
            get { return sourceOffset; }
        }

        /// <summary>
        /// Gets the number of characters in the source text that produced this token.
        /// </summary>
        public Int32 SourceLength
        {
            get { return sourceLength; }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents a white space character.
        /// </summary>
        public Boolean IsWhiteSpace
        {
            get { return tokenType == TextParserTokenType.Text && !text.IsEmpty && Char.IsWhiteSpace(text[0]); }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents a new line character.
        /// </summary>
        public Boolean IsNewLine
        {
            get { return tokenType == TextParserTokenType.Text && !text.IsEmpty && (text[0] == '\n' || text[0] == '\r'); }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents non-breaking white space.
        /// </summary>
        public Boolean IsNonBreakingSpace
        {
            get { return isNonBreakingSpace; }
        }

        // Property values.
        private readonly TextParserTokenType tokenType;
        private readonly StringSegment text;
        private readonly Int32 sourceOffset;
        private readonly Int32 sourceLength;
        private readonly Boolean isNonBreakingSpace;
    }
}
