using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a change to the state of the text renderer as a result of processing a layout command.
    /// </summary>
    [Flags]
    internal enum TextRendererStateChange
    {
        /// <summary>
        /// No change.
        /// </summary>
        None,

        /// <summary>
        /// The current font has changed.
        /// </summary>
        ChangeFont = 0x0001,

        /// <summary>
        /// The current color has changed.
        /// </summary>
        ChangeColor = 0x0002,

        /// <summary>
        /// The current glyph shader has changed.
        /// </summary>
        ChangeGlyphShader = 0x0004,

        /// <summary>
        /// The current link has changed.
        /// </summary>
        ChangeLink = 0x0008,
    }
}
