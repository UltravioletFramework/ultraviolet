using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a token produced by the formatted text lexer.
    /// </summary>
    public struct TextLexerToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLexerToken"/> class.
        /// </summary>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The token's text.</param>
        internal TextLexerToken(TextLexerTokenType tokenType, StringSegment tokenText)
        {
            this.tokenType = tokenType;
            this.tokenText = tokenText;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("{0} \"{1}\"", TokenType.ToString().ToUpper(), TokenText);
        }

        /// <summary>
        /// Gets the token type.
        /// </summary>
        public TextLexerTokenType TokenType
        {
            get { return tokenType; }
            internal set { tokenType = value; }
        }

        /// <summary>
        /// Gets the token text.
        /// </summary>
        public StringSegment TokenText
        {
            get { return tokenText; }
            internal set { tokenText = value; }
        }

        // Property values.
        private TextLexerTokenType tokenType;
        private StringSegment tokenText;
    }
}
