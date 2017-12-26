using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the stacks used by <see cref="TextRenderer"/> to track its current styling state.
    /// </summary>
    [Flags]
    internal enum TextRendererStacks
    {
        /// <summary>
        /// No stack.
        /// </summary>
        None,

        /// <summary>
        /// The style stack.
        /// </summary>
        Style = 0x0001,

        /// <summary>
        /// The font stack.
        /// </summary>
        Font = 0x0002,

        /// <summary>
        /// The color stack.
        /// </summary>
        Color = 0x0004,

        /// <summary>
        /// The glyph shader stack.
        /// </summary>
        GlyphShader = 0x0008,

        /// <summary>
        /// The link stack.
        /// </summary>
        Link = 0x0010,

        /// <summary>
        /// All of the renderer's stacks.
        /// </summary>
        All = Style | Font | Color | GlyphShader | Link
    }
}
