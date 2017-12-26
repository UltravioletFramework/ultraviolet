namespace Ultraviolet.Presentation.Uvss
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
        /// The token contains a line break.
        /// </summary>
        EndOfLine,

        /// <summary>
        /// The token contains a single line comment.
        /// </summary>
        SingleLineComment,

        /// <summary>
        /// The token contains a multiple line comment.
        /// </summary>
        MultiLineComment,

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
        /// The token contains a comma (,).
        /// </summary>
        Comma,

        /// <summary>
        /// The token contains a colon (:).
        /// </summary>
        Colon,

        /// <summary>
        /// The token contains a semi-colon (;).
        /// </summary>
        SemiColon,

        /// <summary>
        /// The token contains an at sign (@).
        /// </summary>
        AtSign,

        /// <summary>
        /// The token contains a hash symbol (#).
        /// </summary>
        Hash,

        /// <summary>
        /// The token contains a period (.).
        /// </summary>
        Period,

        /// <summary>
        /// The token contains an exclamation mark (!).
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
        /// The token contains an open bracket.
        /// </summary>
        OpenBracket,

        /// <summary>
        /// The token contains a close bracket.
        /// </summary>
        CloseBracket,

        /// <summary>
        /// The token contains an asterisk (*).
        /// </summary>
        Asterisk,

        /// <summary>
        /// The token contains a double greater than symbol (>>).
        /// </summary>
        GreaterThanGreaterThan,

        /// <summary>
        /// The token contains a greater than symbol followed by a question mark (>?).
        /// </summary>
        GreaterThanQuestionMark,
        
        /// <summary>
        /// The token contains the equals symbol (=).
        /// </summary>
        Equals,

        /// <summary>
        /// The token contains the not equals symbol (&lt;&gt;).
        /// </summary>
        NotEquals,

        /// <summary>
        /// The token contains the less than symbol (&lt;).
        /// </summary>
        LessThan,

        /// <summary>
        /// The token contains the less than or equal to symbol (&lt;=).
        /// </summary>
        LessThanEquals,

        /// <summary>
        /// The token contains the greater than symbol (>).
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The token contains the greater than or equal to symbol (>=).
        /// </summary>
        GreaterThanEquals,

        /// <summary>
        /// The token contains a pipe (|).
        /// </summary>
        Pipe,

		/// <summary>
		/// The token contains a directive ($foo).
		/// </summary>
		Directive,
    }
}
