namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents the types of tokens which are produced by the UVSS lexer.
    /// </summary>
    public enum UvssLexerTokenType
    {
        /// <summary>
        /// The token type is unknown or has not been set.
        /// </summary>
        Unknown,

        /// <summary>
        /// The token contains a comment.
        /// </summary>
        Comment,

        /// <summary>
        /// The token contains white space.
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// The token contains a language keyword.
        /// </summary>
        Keyword,

        /// <summary>
        /// The token contains an identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// The token contains a numeric literal.
        /// </summary>
        Number,

        /// <summary>
        /// The token contains a property value.
        /// </summary>
        Value,

        /// <summary>
        /// The token contains a comma.
        /// </summary>
        Comma,

        /// <summary>
        /// The token contains a colon.
        /// </summary>
        Colon,

        /// <summary>
        /// The token contains a semi-colon.
        /// </summary>
        SemiColon,

        /// <summary>
        /// The token contains the storyboard prefix (@).
        /// </summary>
        AtSign,

        /// <summary>
        /// The token contains the select by name prefix (#).
        /// </summary>
        Hash,

        /// <summary>
        /// The token contains the select by class prefix (.).
        /// </summary>
        Period,

        /// <summary>
        /// The token contains the select by specific type suffix (!).
        /// </summary>
        ExclamationMark,

        /// <summary>
        /// The token contains an open parenthesis.
        /// </summary>
        OpenParenthesis,

        /// <summary>
        /// The token contains a close parenthesis.
        /// </summary>
        CloseParenthesis,

        /// <summary>
        /// The token contains an open curly brace.
        /// </summary>
        OpenCurlyBrace,

        /// <summary>
        /// The token contains a close curly brace.
        /// </summary>
        CloseCurlyBrace,

        /// <summary>
        /// The token contains a universal selector (*).
        /// </summary>
        UniversalSelector,

        /// <summary>
        /// The token contains the templated child combinator (>>).
        /// </summary>
        TemplatedChildCombinator,

        /// <summary>
        /// The token contains the logical child combinator (>?).
        /// </summary>
        LogicalChildCombinator,
        
        /// <summary>
        /// The token contains the equals operator (=).
        /// </summary>
        EqualsOperator,

        /// <summary>
        /// The token contains the not equals operator (&lt;&gt;).
        /// </summary>
        NotEqualsOperator,

        /// <summary>
        /// The token contains the less than operator (&lt;).
        /// </summary>
        LessThanOperator,

        /// <summary>
        /// The token contains the less than or equal to operator (&lt;=).
        /// </summary>
        LessThanEqualsOperator,

        /// <summary>
        /// The token contains the greater than operator (>).
        /// </summary>
        GreaterThanOperator,

        /// <summary>
        /// The token contains the greater than or equal to operator (>=).
        /// </summary>
        GreaterThanEqualsOperator,

        /// <summary>
        /// The token contains the navigation expression operator (|).
        /// </summary>
        NavigationExpressionOperator,
    }
}
