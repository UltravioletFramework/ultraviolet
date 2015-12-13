using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command containing information about the subsequent line of text.
    /// </summary>
    public struct TextLayoutLineInfoCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutLineInfoCommand"/> structure.
        /// </summary>
        /// <param name="offset">The horizontal offset of the line within its layout area.</param>
        /// <param name="lineWidth">The width of the line in pixels.</param>
        /// <param name="lineHeight">The height of the line in pixels.</param>
        /// <param name="lengthInCommands">The length of the line of text in commands.</param>
        /// <param name="lengthInGlyphs">The length of the line in glyphs.</param>
        /// <param name="terminatedByLineBreak">A value indicating whether this line is terminated by a line break.</param>
        public TextLayoutLineInfoCommand(Int32 offset, Int32 lineWidth, Int32 lineHeight, Int32 lengthInCommands, Int32 lengthInGlyphs, Boolean terminatedByLineBreak)
        {
            this.commandType = TextLayoutCommandType.LineInfo;
            this.offset = offset;
            this.lineWidth = lineWidth;
            this.lineHeight = lineHeight;
            this.lengthInCommands = lengthInCommands;
            this.lengthInGlyphs = lengthInGlyphs;
            this.terminatedByLineBreak = terminatedByLineBreak;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the horizontal offset of the line of text within its layout area.
        /// </summary>
        public Int32 Offset
        {
            get { return offset; }
            internal set { offset = value; }
        }

        /// <summary>
        /// Gets the width of the line in pixels.
        /// </summary>
        public Int32 LineWidth
        {
            get { return lineWidth; }
            internal set { lineWidth = value; }
        }

        /// <summary>
        /// Gets the height of the line in pixels.
        /// </summary>
        public Int32 LineHeight
        {
            get { return lineHeight; }
            internal set { lineHeight = value; }
        }

        /// <summary>
        /// Gets the length of the line of text in commands.
        /// </summary>
        public Int32 LengthInCommands
        {
            get { return lengthInCommands; }
            internal set { lengthInCommands = value; }
        }

        /// <summary>
        /// Gets the length of the line of text in glyphs.
        /// </summary>
        public Int32 LengthInGlyphs
        {
            get { return lengthInGlyphs; }
            internal set { lengthInGlyphs = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this line is terminated by a line break.
        /// </summary>
        public Boolean TerminatedByLineBreak
        {
            get { return terminatedByLineBreak; }
            internal set { terminatedByLineBreak = value; }
        }

        // Property values.
        private TextLayoutCommandType commandType;
        private Int32 offset;
        private Int32 lineWidth;
        private Int32 lineHeight;
        private Int32 lengthInCommands;
        private Int32 lengthInGlyphs;
        private Boolean terminatedByLineBreak;
    }
}
