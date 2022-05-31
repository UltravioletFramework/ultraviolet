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

        /// <summary>
        /// Indicates that commands which change the text color should be ignored.
        /// </summary>
        IgnoreColorChanges = 0x0004,

        /// <summary>
        /// Indicates that commands which change the font face should be ignored.
        /// </summary>
        IgnoreFontFaceChanges = 0x0008,

        /// <summary>
        /// Indicates that commands which change the font style should be ignored.
        /// </summary>
        IgnoreFontStyleChanges = 0x0010,

        /// <summary>
        /// Indicates that glyph shaders should be ignored.
        /// </summary>
        IgnoreGlyphShaders = 0x0020,

        /// <summary>
        /// Indicates that custom commands should be ignored.
        /// </summary>
        IgnoreCustomCommands = 0x0040,

        /// <summary>
        /// Indicates that commands which change the text style should be ignored.
        /// </summary>
        IgnoreStyleChanges = IgnoreColorChanges | TextLayoutOptions.IgnoreFontFaceChanges | TextLayoutOptions.IgnoreFontStyleChanges | TextLayoutOptions.IgnoreCustomCommands,
    }
}
