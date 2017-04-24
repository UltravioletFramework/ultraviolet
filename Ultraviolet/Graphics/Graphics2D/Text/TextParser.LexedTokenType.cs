
namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextParser
    {
        /// <summary>
        /// Represents the types of tokens produced by the formatted text lexer.
        /// </summary>
        private enum LexedTokenType
        {
            /// <summary>
            /// The token represents a line break.
            /// </summary>
            NewLine,

            /// <summary>
            /// The token represents breaking white space.
            /// </summary>
            BreakingWhiteSpace,

            /// <summary>
            /// The token represents non-breaking white space.
            /// </summary>
            NonBreakingWhiteSpace,

            /// <summary>
            /// The token represents a word containing letters, numbers, or other characters.
            /// </summary>
            Word,

            /// <summary>
            /// The token represents an escaped pipe (|) symbol.
            /// </summary>
            Pipe,

            /// <summary>
            /// The token represents a parser command.
            /// </summary>
            Command,
        }
    }
}