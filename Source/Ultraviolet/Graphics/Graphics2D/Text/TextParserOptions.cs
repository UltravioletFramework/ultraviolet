using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the set of options that can be specified to control text parsing.
    /// </summary>
    [Flags]
    public enum TextParserOptions
    {
        /// <summary>
        /// No parser options specified.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Specifies that the parser should treat command codes as if they were raw text.
        /// </summary>
        IgnoreCommandCodes = 0x0001,
    }
}
