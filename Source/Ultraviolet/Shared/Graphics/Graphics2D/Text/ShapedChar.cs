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
        /// <param name="sourceIndex">The index of the unshaped character in the original string which produced this glyph.</param>
        /// <param name="offsetX">The offset of the character along the x-axis, in pixels.</param>
        /// <param name="offsetY">The offset of the character along the y-axis, in pixels.</param>
        /// <param name="advance">The character's advance in pixels.</param>
        public ShapedChar(Int32 glyphIndex, Int32 sourceIndex, Int16 offsetX, Int16 offsetY, Int16 advance)
        {
            this.GlyphIndex = glyphIndex;
            this.SourceIndex = sourceIndex;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.Advance = advance;
        }

        /// <summary>
        /// Gets a <see cref="ShapedChar"/> instance which represents a tab character.
        /// </summary>
        public static ShapedChar Tab => new ShapedChar('\t', 0, Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);

        /// <summary>
        /// Gets a <see cref="ShapedChar"/> instance which represents a newline character.
        /// </summary>
        public static ShapedChar Newline => new ShapedChar('\n', 0, Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);

        /// <summary>
        /// Gets the special character which this shaped character represents, if any.
        /// </summary>
        /// <returns>The special character which this shaped character represents, 
        /// or 0 if it does not represent a special character.</returns>
        public Char GetSpecialCharacter()
        {
            if (OffsetX == Int16.MaxValue && OffsetY == Int16.MaxValue && Advance == Int16.MaxValue)
            {
                return (Char)GlyphIndex;
            }
            return Char.MinValue;
        }

        /// <summary>
        /// Gets the glyph index which this character represents.
        /// </summary>
        public Int32 GlyphIndex { get; }

        /// <summary>
        /// Gets the index of the unshaped character in the original string which produced
        /// this glyph. Multiple glyphs can map to the same character index.
        /// </summary>
        public Int32 SourceIndex { get; }

        /// <summary>
        /// Gets the offset of the character along the x-axis in pixels.
        /// </summary>
        public Int16 OffsetX { get; }

        /// <summary>
        /// Gets the offset the character along the y-axis in pixels.
        /// </summary>
        public Int16 OffsetY { get; }

        /// <summary>
        /// Gets the character's advance in pixels.
        /// </summary>
        public Int16 Advance { get; }
    }
}
