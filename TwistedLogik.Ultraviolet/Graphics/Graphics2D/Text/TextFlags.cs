using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// The set of flags used to specify how formatted text is rendered.
    /// </summary>
    [Flags]
    public enum TextFlags
    {
        /// <summary>
        /// Standard alignment.
        /// </summary>
        Standard = TextFlags.AlignLeft | TextFlags.AlignTop,

        /// <summary>
        /// Horizontally aligns the text against the left edge of the layout area.
        /// </summary>
        AlignLeft   = 0x0001,

        /// <summary>
        /// Horizontally aligns the text in the center of the layout area.
        /// </summary>
        AlignCenter = 0x0002,

        /// <summary>
        /// Horizontally aligns the text against the right edge of the layout area.
        /// </summary>
        AlignRight  = 0x0004,

        /// <summary>
        /// Vertically aligns the text against the top edge of the layout area.
        /// </summary>
        AlignTop    = 0x0010,

        /// <summary>
        /// Vertically aligns the text in the middle of the layout area.
        /// </summary>
        AlignMiddle = 0x0020,

        /// <summary>
        /// Vertically aligns the text against the bottom edge of the layout area.
        /// </summary>
        AlignBottom = 0x0040,
    }
}
