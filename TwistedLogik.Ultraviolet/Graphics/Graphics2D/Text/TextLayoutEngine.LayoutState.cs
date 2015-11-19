using System;
using System.Security;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextLayoutEngine
    {
        /// <summary>
        /// Represents the state of the layout engine during a text layout operation.
        /// </summary>
        private struct LayoutState
        {
            /// <summary>
            /// Writes the specified metadata to the <see cref="TextLayoutCommandType.BlockInfo"/> command for this text.
            /// </summary>
            /// <param name="output">The command stream to which commands are being written.</param>
            /// <param name="blockWidth">The width of the block in pixels.</param>
            /// <param name="blockHeight">The height of the block in pixels.</param>
            /// <param name="lengthInLines">The length of the block of text in lines.</param>
            /// <param name="settings">The layout settings.</param>
            [SecuritySafeCritical]
            public void WriteBlockInfo(TextLayoutCommandStream output, Int16 blockWidth, Int16 blockHeight, Int32 lengthInLines, ref TextLayoutSettings settings)
            {
                var offset = 0;

                if (settings.Height.HasValue)
                {
                    if ((settings.Flags & TextFlags.AlignBottom) == TextFlags.AlignBottom)
                        offset = (settings.Height.Value - blockHeight);
                    if ((settings.Flags & TextFlags.AlignMiddle) == TextFlags.AlignMiddle)
                        offset = (settings.Height.Value - blockHeight) / 2;
                }

                output.Seek(0);
                unsafe
                {
                    var ptr = (TextLayoutBlockInfoCommand*)output.Data;
                    ptr->Offset = offset;
                    ptr->LengthInLines = lengthInLines;
                }
                output.Seek(output.Count);
            }

            /// <summary>
            /// Writes the specified metadata to the <see cref="TextLayoutCommandType.LineInfo"/> command for this text.
            /// </summary>
            /// <param name="output">The command stream to which commands are being written.</param>
            /// <param name="lineWidth">The width of the line in pixels.</param>
            /// <param name="lineHeight">The height of the line in pixels.</param>
            /// <param name="lengthInCommands">The length of the line of text in commands.</param>
            /// <param name="settings">The layout settings.</param>
            [SecuritySafeCritical]
            public void WriteLineInfo(TextLayoutCommandStream output, Int16 lineWidth, Int16 lineHeight, Int32 lengthInCommands, ref TextLayoutSettings settings)
            {
                var offset = 0;

                if (settings.Height.HasValue)
                {
                    if ((settings.Flags & TextFlags.AlignRight) == TextFlags.AlignRight)
                        offset = (settings.Width.Value - lineWidth);
                    if ((settings.Flags & TextFlags.AlignCenter) == TextFlags.AlignCenter)
                        offset = (settings.Width.Value - lineWidth) / 2;
                }

                output.Seek(lineInfoCommandIndex);
                unsafe
                {
                    var ptr = (TextLayoutLineInfoCommand*)output.Data;
                    ptr->Offset = offset;
                    ptr->LineWidth = lineWidth;
                    ptr->LineHeight = lineHeight;
                    ptr->LengthInCommands = lineLengthInCommands;
                }
                output.Seek(output.Count);
            }

            /// <summary>
            /// Advances to the next command on the current line.
            /// </summary>
            public void AdvanceToNextCommand()
            {
                AdvanceToNextCommand(0, 0, 0, false);
            }

            /// <summary>
            /// Advances to the next command on the current line.
            /// </summary>
            /// <param name="width">The command's width in pixels.</param>
            /// <param name="height">The command's height in pixels.</param>
            /// <param name="length">The command's length in characters.</param>
            /// <param name="isWhiteSpace">A value indicating whether the token represents white space.</param>
            public void AdvanceToNextCommand(Int32 width, Int32 height, Int32 length, Boolean isWhiteSpace)
            {
                positionX += width;
                lineLengthInText += length;
                lineLengthInCommands++;
                lineTrailingWhiteSpaceWidth = isWhiteSpace ? (lineTrailingWhiteSpaceWidth + width) : 0;
                lineWidth += width;
                lineHeight = Math.Max(lineHeight, height);
                totalLength += length;
            }

            /// <summary>
            /// Advances the layout engine to the next line.
            /// </summary>
            public void AdvanceToNextLine(TextLayoutCommandStream output, ref TextLayoutSettings settings, Boolean writeLineInfo = true)
            {
                lineWidth -= lineTrailingWhiteSpaceWidth;

                WriteLineInfo(output, (Int16)lineWidth, (Int16)lineHeight, lineLengthInCommands, ref settings);

                positionX = 0;
                positionY += lineHeight;
                actualWidth = Math.Max(actualWidth, lineWidth);
                actualHeight += lineHeight;
                lineTrailingWhiteSpaceWidth = 0;
                lineCount++;
                lineWidth = 0;
                lineHeight = 0;
                lineLengthInText = 0;
                lineLengthInCommands = 0;
                lineInfoCommandIndex = output.Count;

                if (writeLineInfo)
                    output.WriteLineInfo();
            }

            /// <summary>
            /// Gets or sets the x-coordinate at which the next token will be placed.
            /// </summary>
            public Int32 PositionX
            {
                get { return positionX; }
                set { positionX = value; }
            }

            /// <summary>
            /// Gets or sets the y-coordinate at which the next token will be placed.
            /// </summary>
            public Int32 PositionY
            {
                get { return positionY; }
                set { positionY = value; }
            }

            /// <summary>
            /// Gets or sets the index of the command token that contains the metadata for the current line.
            /// </summary>
            public Int32 LineInfoCommandIndex
            {
                get { return lineInfoCommandIndex; }
                set { lineInfoCommandIndex = value; }
            }

            /// <summary>
            /// Gets or sets the width of the current line's trailing white space.
            /// </summary>
            public Int32 LineTrailingWhiteSpaceWidth
            {
                get { return lineTrailingWhiteSpaceWidth; }
                set { LineTrailingWhiteSpaceWidth = value; }
            }

            /// <summary>
            /// Gets or sets the number of lines in the laid-out text.
            /// </summary>
            public Int32 LineCount
            {
                get { return lineCount; }
                set { lineCount = value; }
            }

            /// <summary>
            /// Gets or sets the width of the current line in pixels.
            /// </summary>
            public Int32 LineWidth
            {
                get { return lineWidth; }
                set { lineWidth = value; }
            }

            /// <summary>
            /// Gets or sets the height of the current line in pixels.
            /// </summary>
            public Int32 LineHeight
            {
                get { return lineHeight; }
                set { lineHeight = value; }
            }

            /// <summary>
            /// Gets or sets the number of text characters on the current line.
            /// </summary>
            public Int32 LineLengthInText
            {
                get { return lineLengthInText; }
                set { lineLengthInText = value; }
            }

            /// <summary>
            /// Gets or sets the length of the current line in commands.
            /// </summary>
            public Int32 LineLengthInCommands
            {
                get { return lineLengthInCommands; }
                set { lineLengthInCommands = value; }
            }
            
            /// <summary>
            /// Gets or sets the width of the area which is occupied by text after layout is performed.
            /// </summary>
            public Int32 ActualWidth
            {
                get { return actualWidth; }
                set { actualWidth = value; }
            }

            /// <summary>
            /// Gets or sets the height of the area which is occupied by text after layout is performed.
            /// </summary>
            public Int32 ActualHeight
            {
                get { return actualHeight; }
                set { actualHeight = value; }
            }

            /// <summary>
            /// Gets or sets the total length of the text.
            /// </summary>
            public Int32 TotalLength
            {
                get { return totalLength; }
                set { totalLength = value; }
            }

            // Property values.
            private Int32 positionX;
            private Int32 positionY;
            private Int32 lineInfoCommandIndex;
            private Int32 lineTrailingWhiteSpaceWidth;
            private Int32 lineCount;
            private Int32 lineWidth;
            private Int32 lineHeight;
            private Int32 lineLengthInText;
            private Int32 lineLengthInCommands;
            private Int32 actualWidth;
            private Int32 actualHeight;
            private Int32 totalLength;
        }
    }
}
