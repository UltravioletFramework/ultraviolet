using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains configurable options for the UVSS lexer.
    /// </summary>
    public class UvssLexerOptions
    {
        /// <summary>
        /// Gets or sets the number of spaces to which tab characters are expanded.
        /// </summary>
        public Int32 TabSize { get; set; } = 4;

        /// <summary>
        /// A default set of lexer options.
        /// </summary>
        public static readonly UvssLexerOptions Default = new UvssLexerOptions();
    }
}
