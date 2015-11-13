using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a token produced by parsing formatted text.
    /// </summary>
    public struct TextParserToken2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserToken2"/> structure.
        /// </summary>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="text">The token's text.</param>
        internal TextParserToken2(TextParserTokenType tokenType, StringSegment text)
        {
            this.tokenType = tokenType;
            this.text = text;
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
            get { return tokenType == TextParserTokenType.Text && !text.IsEmpty && text[0] == '\n'; }
        }

        // Property values.
        private readonly TextParserTokenType tokenType;
        private readonly StringSegment text;
    }
}
