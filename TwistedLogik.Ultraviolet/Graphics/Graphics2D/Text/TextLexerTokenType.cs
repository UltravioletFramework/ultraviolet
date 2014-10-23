
namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the types of formatted text lexer tokens.
    /// </summary>
    public enum TextLexerTokenType
    {
        /// <summary>
        /// The token represents a line break.
        /// </summary>
        NewLine,

        /// <summary>
        /// The token represents white space.
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// The token represents a parser command.
        /// </summary>
        Command,

        /// <summary>
        /// The token represents a word containing letters, numbers, or other characters.
        /// </summary>
        Word,

        /// <summary>
        /// The token represents an escaped pipe (|) symbol.
        /// </summary>
        Pipe,
    }
}
