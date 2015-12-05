using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the metadata for a line of formatted text.
    /// </summary>
    public struct LineInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineInfo"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="y">The y-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="width">The line's width in pixels.</param>
        /// <param name="height">The line's height in pixels.</param>
        /// <param name="lengthInCommands">The line's length in commands.</param>
        /// <param name="lengthInGlyphs">The line's length in glyphs.</param>
        internal LineInfo(Int32 x, Int32 y, Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInGlyphs)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.lengthInCommands = lengthInCommands;
            this.lengthInGlyphs = lengthInGlyphs;
        }

        /// <summary>
        /// Gets the x-coordinate of the line's top-left corner relative to its layout area.
        /// </summary>
        public Int32 X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the line's top-left corner relative to its layout area.
        /// </summary>
        public Int32 Y
        {
            get { return y; }
        }
        
        /// <summary>
        /// Gets the line's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the line's height in pixels.
        /// </summary>
        public Int32 Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the line's length in commands.
        /// </summary>
        public Int32 LengthInCommands
        {
            get { return lengthInCommands; }
        }

        /// <summary>
        /// Gets the line's length in glyphs.
        /// </summary>
        public Int32 LengthInGlyphs
        {
            get { return lengthInGlyphs; }
        }

        // Property values.
        private readonly Int32 x;
        private readonly Int32 y;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 lengthInCommands;
        private readonly Int32 lengthInGlyphs;
    }
}
