using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
        /// <param name="offsetInSource">The line's offset within the source string.</param>
        /// <param name="offsetInGlyphs">The line's offset within the glyph buffer (or the source string if the text is not shaped).</param>
        /// <param name="x">The x-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="y">The y-coordinate of the line's top-left corner relative to its layout area.</param>
        /// <param name="width">The line's width in pixels.</param>
        /// <param name="height">The line's height in pixels.</param>
        /// <param name="lengthInCommands">The line's length in commands.</param>
        /// <param name="lengthInSource">The line's length in source characters.</param>
        /// <param name="lengthInGlyphs">The line's length in glyphs.</param>
        /// <param name="terminatingLineBreakLength">The length of the line's terminating line break in the source text.</param>
        internal LineInfo(TextLayoutCommandStream source, Int32 lineIndex, Int32 offsetInCommands, Int32 offsetInSource, Int32 offsetInGlyphs, 
            Int32 x, Int32 y, Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInSource, Int32 lengthInGlyphs, Int32 terminatingLineBreakLength)
        {
            this.Source = source;
            this.LineIndex = lineIndex;
            this.OffsetInCommands = offsetInCommands;
            this.OffsetInSource = offsetInSource;
            this.OffsetInGlyphs = offsetInGlyphs;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.LengthInCommands = lengthInCommands;
            this.LengthInSource = lengthInSource;
            this.LengthInGlyphs = lengthInGlyphs;
            this.TerminatingLineBreakLength = terminatingLineBreakLength;
        }

        /// <summary>
        /// Gets the <see cref="TextLayoutCommandStream"/> from which this line originated.
        /// </summary>
        public TextLayoutCommandStream Source { get; }

        /// <summary>
        /// Gets the index of the line within its layout.
        /// </summary>
        public Int32 LineIndex { get; }

        /// <summary>
        /// Gets the index of the <see cref="TextLayoutCommandType.LineInfo"/> command that contains this line's metadata.
        /// </summary>
        public Int32 OffsetInCommands { get; }

        /// <summary>
        /// Gets the line's offset within the source string.
        /// </summary>
        public Int32 OffsetInSource { get; }

        /// <summary>
        /// Gets the line's offset within the glyph buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 OffsetInGlyphs { get; }

        /// <summary>
        /// Gets the x-coordinate of the line's top-left corner relative to its layout area.
        /// </summary>
        public Int32 X { get; }

        /// <summary>
        /// Gets the y-coordinate of the line's top-left corner relative to its layout area.
        /// </summary>
        public Int32 Y { get; }

        /// <summary>
        /// Gets the line's width in pixels.
        /// </summary>
        public Int32 Width { get; }

        /// <summary>
        /// Gets the line's height in pixels.
        /// </summary>
        public Int32 Height { get; }

        /// <summary>
        /// Gets the line's length in commands.
        /// </summary>
        public Int32 LengthInCommands { get; }

        /// <summary>
        /// Gets the line's length in source characters.
        /// </summary>
        public Int32 LengthInSource { get; }

        /// <summary>
        /// Gets the line's length in glyphs.
        /// </summary>
        public Int32 LengthInGlyphs { get; }

        /// <summary>
        /// Gets the length of the line's terminating line break in the source text.
        /// </summary>
        public Int32 TerminatingLineBreakLength { get; }
    }
}
