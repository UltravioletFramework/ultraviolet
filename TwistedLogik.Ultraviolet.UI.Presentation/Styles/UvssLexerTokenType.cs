
namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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

        /// <summary>
        /// The name of a style, optionally with a container qualifier.
        /// </summary>
        StyleName,

        /// <summary>
        /// A style qualifier, like !important.
        /// </summary>
        StyleQualifier,

        /// <summary>
        /// Represents the child selector (>).
        /// </summary>
        ChildSelector,

        /// <summary>
        /// Represents the universal selector (*).
        /// </summary>
        UniversalSelector,

        /// <summary>
        /// Represents a single line comment (beginning with //).
        /// </summary>
        SingleLineComment,

        /// <summary>
        /// Represents a multi line comment (beginning with /* and ending with */).
        /// </summary>
        MultiLineComment,

        /// <summary>
        /// Represents the "trigger" keyword.
        /// </summary>
        TriggerKeyword,

        /// <summary>
        /// Represents a comparison operator. 
        /// </summary>
        ComparisonOperator,
    }
}
