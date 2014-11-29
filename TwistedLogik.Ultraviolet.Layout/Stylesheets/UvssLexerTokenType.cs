
namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents the types of tokens produced by the <see cref="UvssLexer"/> class.
    /// </summary>
    internal enum UvssLexerTokenType
    {
        /// <summary>
        /// Lexically significant whitespace.
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// A named identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// A numeric value.
        /// </summary>
        Number,

        /// <summary>
        /// A string literal.
        /// </summary>
        String,

        /// <summary>
        /// An open parenthesis.
        /// </summary>
        OpenParenthesis,

        /// <summary>
        /// A close parenthesis.
        /// </summary>
        CloseParenthesis,

        /// <summary>
        /// An open curly brace.
        /// </summary>
        OpenCurlyBrace,

        /// <summary>
        /// A close curly brace.
        /// </summary>
        CloseCurlyBrace,

        /// <summary>
        /// A colon.
        /// </summary>
        Colon,

        /// <summary>
        /// A semicolon.
        /// </summary>
        Semicolon,

        /// <summary>
        /// A comma.
        /// </summary>
        Comma,
    }
}
