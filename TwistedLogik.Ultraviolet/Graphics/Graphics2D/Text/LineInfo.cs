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
        /// <param name="source">The command stream from which this line was retrieved.</param>
        /// <param name="lineIndex">The index of the line within its layout.</param>
        /// <param name="offsetInCommands">The index of the command that contains the line's metadata.</param>
        /// <param name="offsetInGlyphs">The index of the first glyph in the line.</param>
        /// <param name="x">The x-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="y">The y-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="width">The line's width in pixels.</param>
        /// <param name="height">The line's height in pixels.</param>
        /// <param name="lengthInCommands">The line's length in commands.</param>
        /// <param name="lengthInGlyphs">The line's length in glyphs.</param>
        internal LineInfo(TextLayoutCommandStream source, Int32 lineIndex, Int32 offsetInCommands, Int32 offsetInGlyphs, 
            Int32 x, Int32 y, Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInGlyphs)
        {
            this.source = source;
            this.lineIndex = lineIndex;
            this.offsetInCommands = offsetInCommands;
            this.offsetInGlyphs = offsetInGlyphs;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.lengthInCommands = lengthInCommands;
            this.lengthInGlyphs = lengthInGlyphs;
        }

        /// <summary>
        /// Gets the <see cref="TextLayoutCommandStream"/> from which this line originated.
        /// </summary>
        public TextLayoutCommandStream Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the index of the line within its layout.
        /// </summary>
        public Int32 LineIndex
        {
            get { return lineIndex; }
        }

        /// <summary>
        /// Gets the index of the <see cref="TextLayoutCommandType.LineInfo"/> command that contains this line's metadata.
        /// </summary>
        public Int32 OffsetInCommands
        {
            get { return offsetInCommands; }
        }

        /// <summary>
        /// Gets the index of the first glyph in the line.
        /// </summary>
        public Int32 OffsetInGlyphs
        {
            get { return offsetInGlyphs; }
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
        private readonly TextLayoutCommandStream source;
        private readonly Int32 lineIndex;
        private readonly Int32 offsetInCommands;
        private readonly Int32 offsetInGlyphs;
        private readonly Int32 x;
        private readonly Int32 y;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 lengthInCommands;
        private readonly Int32 lengthInGlyphs;
    }
}
