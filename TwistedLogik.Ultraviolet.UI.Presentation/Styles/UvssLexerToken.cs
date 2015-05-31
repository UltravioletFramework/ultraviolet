using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a token produced by the <see cref="UvssLexer"/> class.
    /// </summary>
    internal struct UvssLexerToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssLexerToken"/> structure.
        /// </summary>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="start">The index of the token within the source text.</param>
        /// <param name="length">The length of the token within the source text.</param>
        /// <param name="line">The line of source code that contains the token.</param>
        /// <param name="value">The token's associated value.</param>
        public UvssLexerToken(UvssLexerTokenType tokenType, Int32 start, Int32 length, Int32 line, String value = null)
        {
            this.tokenType = tokenType;
            this.start     = start;
            this.length    = length;
            this.line      = line;

            switch (tokenType)
            {
                case UvssLexerTokenType.WhiteSpace:
                    this.value = " ";
                    break;

                case UvssLexerTokenType.OpenParenthesis:
                    this.value = "(";
                    break;

                case UvssLexerTokenType.CloseParenthesis:
                    this.value = ")";
                    break;

                case UvssLexerTokenType.OpenCurlyBrace:
                    this.value = "{";
                    break;

                case UvssLexerTokenType.CloseCurlyBrace:
                    this.value = "}";
                    break;

                case UvssLexerTokenType.Colon:
                    this.value = ":";
                    break;

                case UvssLexerTokenType.Semicolon:
                    this.value = ";";
                    break;

                case UvssLexerTokenType.Comma:
                    this.value = ",";
                    break;

                case UvssLexerTokenType.UniversalSelector:
                    this.value = "*";
                    break;

                default:
                    this.value = value;
                    break;
            }
        }

        /// <summary>
        /// Gets the token's type.
        /// </summary>
        public UvssLexerTokenType TokenType
        {
            get { return tokenType; }
        }

        /// <summary>
        /// Gets the index of the token within the source text.
        /// </summary>
        public Int32 Start
        {
            get { return start; }
        }

        /// <summary>
        /// Gets the length of the token in the source text.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
        }

        /// <summary>
        /// Gets the line of source code that contains the token.
        /// </summary>
        public Int32 Line
        {
            get { return line; }
        }

        /// <summary>
        /// Gets the token's associated value, if it has one.
        /// </summary>
        public String Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents a comment.
        /// </summary>
        public Boolean IsComment
        {
            get { return TokenType == UvssLexerTokenType.SingleLineComment || TokenType == UvssLexerTokenType.MultiLineComment; }
        }

        // Property values.
        private readonly UvssLexerTokenType tokenType;
        private readonly Int32 start;
        private readonly Int32 length;
        private readonly Int32 line;
        private readonly String value;
    }
}
