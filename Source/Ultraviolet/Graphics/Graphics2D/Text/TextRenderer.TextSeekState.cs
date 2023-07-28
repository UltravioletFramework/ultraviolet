using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextRenderer
    {
        /// <summary>
        /// Represents the data required to seek through lines of text for querying or rendering.
        /// </summary>
        private struct TextSeekState
        {
            /// <summary>
            /// The index of the line which is being searched.
            /// </summary>
            public Int32 LineIndex;
            
            /// <summary>
            /// Gets the horizontal offset of the line within its block of text.
            /// </summary>
            public Int32 LineOffsetX;

            /// <summary>
            /// Gets the vertical offset of the line within its block of text.
            /// </summary>
            public Int32 LineOffsetY;

            /// <summary>
            /// Gets the vertical position of the line, including any block offset.
            /// </summary>
            public Int32 LinePositionY;

            /// <summary>
            /// Gets the width ofthe line being searched in pixels.
            /// </summary>
            public Int32 LineWidth;

            /// <summary>
            /// Gets the height of the line being searched in pixels.
            /// </summary>
            public Int32 LineHeight;

            /// <summary>
            /// Gets the length of the line being searched in commands.
            /// </summary>
            public Int32 LineLengthInCommands;

            /// <summary>
            /// Gets the length of the line being searched in source characters.
            /// </summary>
            public Int32 LineLengthInSource;

            /// <summary>
            /// Gets the length of the line being searched in shaped characters.
            /// </summary>
            public Int32 LineLengthInShaped;

            /// <summary>
            /// Gets the length of the line being searched in glyphs.
            /// </summary>
            public Int32 LineLengthInGlyphs;

            /// <summary>
            /// Gets the index of the first command on the line.
            /// </summary>
            public Int32 LineStartInCommands;

            /// <summary>
            /// Gets the index of the first source character on the line.
            /// </summary>
            public Int32 LineStartInSource;

            /// <summary>
            /// Gets the index of the first shaped character on the line.
            /// </summary>
            public Int32 LineStartInShaped;

            /// <summary>
            /// Gets the index of the first glyph on the line.
            /// </summary>
            public Int32 LineStartInGlyphs;

            /// <summary>
            /// The number of source characters that have been seen while searching through lines.
            /// </summary>
            public Int32 NumberOfSourceCharactersSeen;

            /// <summary>
            /// The number of shaped characters that have been seen while searching through lines.
            /// </summary>
            public Int32 NumberOfShapedCharactersSeen;

            /// <summary>
            /// The number of glyphs that have been seen while searching through lines.
            /// </summary>
            public Int32 NumberOfGlyphsSeen;

            /// <summary>
            /// The source length of the line break which terminates the line, if there is one.
            /// </summary>
            public Int32 TerminatingLineBreakLength;
        }
    }
}
