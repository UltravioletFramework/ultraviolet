using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a set of options which change how the layout engine lays out text.
    /// </summary>
    [Flags]
    public enum TextLayoutOptions
    {
        /// <summary>
        /// No layout options.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Indicates that words which are split across multiple lines should be hyphenated.
        /// </summary>
        Hyphenate = 0x0001,

        /// <summary>
        /// Indicates that the text should be shaped during layout.
        /// </summary>
        Shape = 0x0002,
    }
}
