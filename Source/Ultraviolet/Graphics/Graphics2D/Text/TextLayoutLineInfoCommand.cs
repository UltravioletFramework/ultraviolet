using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
        /// <param name="lengthInSource">The length of the line in source characters.</param>
        /// <param name="lengthInShaped">The length of the line in shaped characters.</param>
        /// <param name="lengthInGlyphs">The length of the line in glyphs.</param>
        /// <param name="terminatingLineBreakLength">The source length of the line break which terminates this line, if there is one.</param>
        public TextLayoutLineInfoCommand(Int32 offset, Int32 lineWidth, Int32 lineHeight, 
            Int32 lengthInCommands, Int32 lengthInSource, Int32 lengthInShaped, Int32 lengthInGlyphs, Int32 terminatingLineBreakLength)
        {
            this.CommandType = TextLayoutCommandType.LineInfo;
            this.Offset = offset;
            this.LineWidth = lineWidth;
            this.LineHeight = lineHeight;
            this.LengthInCommands = lengthInCommands;
            this.LengthInSource = lengthInSource;
            this.LengthInShaped = lengthInShaped;
            this.LengthInGlyphs = lengthInGlyphs;
            this.TerminatingLineBreakSourceLength = terminatingLineBreakLength;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the horizontal offset of the line of text within its layout area.
        /// </summary>
        public Int32 Offset { get; internal set; }

        /// <summary>
        /// Gets the width of the line in pixels.
        /// </summary>
        public Int32 LineWidth { get; internal set; }

        /// <summary>
        /// Gets the height of the line in pixels.
        /// </summary>
        public Int32 LineHeight { get; internal set; }

        /// <summary>
        /// Gets the length of the line of text in commands.
        /// </summary>
        public Int32 LengthInCommands { get; internal set; }

        /// <summary>
        /// Gets the length of the line of text in source characters.
        /// </summary>
        public Int32 LengthInSource { get; internal set; }

        /// <summary>
        /// Gets the length of the line in shaped characters.
        /// </summary>
        public Int32 LengthInShaped { get; internal set; }

        /// <summary>
        /// Gets the length of the line of text in glyphs.
        /// </summary>
        public Int32 LengthInGlyphs { get; internal set; }

        /// <summary>
        /// Gets the source length of the line break which terminates this line, if there is one.
        /// </summary>
        public Int32 TerminatingLineBreakSourceLength { get; internal set; }
    }
}
