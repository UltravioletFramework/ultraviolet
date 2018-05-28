using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a Unicode character with shaping information.
    /// </summary>
    public partial struct ShapedChar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedChar"/> structure.
        /// </summary>
        /// <param name="glyphIndex">The glyph index which this character represents.</param>
        /// <param name="offsetX">The offset of the character along the x-axis, in pixels.</param>
        /// <param name="offsetY">The offset of the character along the y-axis, in pixels.</param>
        /// <param name="advanceX">The character's advance along the x-axis, in pixels.</param>
        /// <param name="advanceY">The character's advance along the y-axis, in pixels.</param>
        public ShapedChar(Int32 glyphIndex, Int16 offsetX, Int16 offsetY, Int16 advanceX, Int16 advanceY)
        {
            this.GlyphIndex = glyphIndex;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.AdvanceX = advanceX;
            this.AdvanceY = advanceY;
        }

        /// <summary>
        /// Gets the glyph index which this character represents.
        /// </summary>
        public Int32 GlyphIndex { get; }

        /// <summary>
        /// Gets the offset of the character along the x-axis, in pixels.
        /// </summary>
        public Int16 OffsetX { get; }

        /// <summary>
        /// Gets the offset the character along the y-axis, in pixels.
        /// </summary>
        public Int16 OffsetY { get; }

        /// <summary>
        /// Gets the character's advance along the x-axis, in pixels.
        /// </summary>
        public Int16 AdvanceX { get; }

        /// <summary>
        /// Gets the character's advance along the y-axis, in pixels.
        /// </summary>
        public Int16 AdvanceY { get; }
    }
}
