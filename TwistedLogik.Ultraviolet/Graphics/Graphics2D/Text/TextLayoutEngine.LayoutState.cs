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

                minBlockOffset = (minBlockOffset.HasValue) ? Math.Min(minBlockOffset.Value, offset) : offset;
            }

            /// <summary>
            /// Writes the specified metadata to the <see cref="TextLayoutCommandType.LineInfo"/> command for this text.
            /// </summary>
            /// <param name="output">The command stream to which commands are being written.</param>
            /// <param name="lineWidth">The width of the line in pixels.</param>
            /// <param name="lineHeight">The height of the line in pixels.</param>
            /// <param name="lengthInCommands">The length of the line of text in commands.</param>
            /// <param name="lengthInGlyphs">The length of the line of text in glyphs.</param>
            /// <param name="settings">The layout settings.</param>
            [SecuritySafeCritical]
            public void WriteLineInfo(TextLayoutCommandStream output, Int16 lineWidth, Int16 lineHeight, Int16 lengthInCommands, Int16 lengthInGlyphs, ref TextLayoutSettings settings)
            {
                var offset = 0;

                if (settings.Width.HasValue)
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
                    ptr->LengthInCommands = lengthInCommands;
                    ptr->LengthInGlyphs = lengthInGlyphs;
                }
                output.Seek(output.Count);

                minLineOffset = (minLineOffset.HasValue) ? Math.Min(minLineOffset.Value, offset) : offset;
            }
            
            /// <summary>
            /// Advances the layout state past the current layout command, assuming that the command is a styling command
            /// with zero size and zero length in characters.
            /// </summary>
            public void AdvanceLineToNextCommand()
            {
                AdvanceLineToNextCommand(0, 0, 1, 0, false);
            }
            
            /// <summary>
            /// Advances the layout state past the current layout command.
            /// </summary>
            /// <param name="width">The width in pixels which the command contributes to the current line.</param>
            /// <param name="height">The height in pixels which the command contributes to the current line.</param>
            /// <param name="lengthInCommands">The number of layout commands which were ultimately produced in the output stream by this command.</param>
            /// <param name="lengthInText">The number of characters of text which are represented by this command.</param>
            /// <param name="isWhiteSpace">A value indicating whether this command represents white space.</param>
            public void AdvanceLineToNextCommand(Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInText, Boolean isWhiteSpace)
            {
                positionX += width;
                lineLengthInCommands += lengthInCommands;
                lineLengthInText += lengthInText;
                lineTrailingWhiteSpaceCount = isWhiteSpace ? (lineTrailingWhiteSpaceCount + lengthInText) : 0;
                lineTrailingWhiteSpaceWidth = isWhiteSpace ? (lineTrailingWhiteSpaceWidth + width) : 0;
                lineWidth += width;
                lineHeight = Math.Max(lineHeight, height);
                totalLength += lengthInText;
            }
            
            /// <summary>
            /// Advances the layout state to the next line of text.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void AdvanceLayoutToNextLine(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                FinalizeLine(output, ref settings);
                output.WriteLineInfo();
            }

            /// <summary>
            /// Advances the layout state to the next line of text after inserting a line break character at the end of the current line.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void AdvanceLayoutToNextLineWithBreak(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                var lineHeightCurrent = lineHeight;
                if (lineHeightCurrent == 0)
                    lineHeight = settings.Font.GetFace(SpriteFontStyle.Regular).LineSpacing;

                // HACK: we're pretending this isn't white space until I fix how white space is handled
                output.WriteLineBreak();
                AdvanceLineToNextCommand(0, lineHeightCurrent, 1, 1, false);

                AdvanceLayoutToNextLine(output, ref settings);
                AdvanceLineToNextCommand(0, lineHeightCurrent, 0, 0, true);
            }

            /// <summary>
            /// Finalizes the current line by writing the line's metadata to the command stream and resetting
            /// state values which are associated with the current line.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void FinalizeLine(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if ((settings.Options & TextLayoutOptions.PreserveTrailingWhiteSpace) != TextLayoutOptions.PreserveTrailingWhiteSpace)
                {
                    lineWidth -= lineTrailingWhiteSpaceWidth;
                    lineLengthInText -= lineTrailingWhiteSpaceCount;
                    totalLength -= lineTrailingWhiteSpaceCount;
                }

                WriteLineInfo(output, (Int16)lineWidth, (Int16)lineHeight, (Int16)lineLengthInCommands, (Int16)lineLengthInText, ref settings);

                positionX = 0;
                positionY += lineHeight;
                actualWidth = Math.Max(actualWidth, lineWidth);
                actualHeight += lineHeight;
                lineTrailingWhiteSpaceCount = 0;
                lineTrailingWhiteSpaceWidth = 0;
                lineCount++;
                lineWidth = 0;
                lineHeight = 0;
                lineLengthInText = 0;
                lineLengthInCommands = 0;
                lineInfoCommandIndex = output.Count;
            }

            /// <summary>
            /// Finalizes the layout by writing the block's metadata to the command stream.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void FinalizeLayout(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if (LineHeight > 0)
                    FinalizeLine(output, ref settings);

                WriteBlockInfo(output, (Int16)ActualWidth, (Int16)ActualHeight, LineCount, ref settings);

                output.Settings = settings;
                output.Bounds = Bounds;
                output.ActualWidth = ActualWidth;
                output.ActualHeight = ActualHeight;
                output.TotalLength = TotalLength;
                output.LineCount = LineCount;
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
            /// Gets or sets the number of trailing white space characters on the current line.
            /// </summary>
            public Int32 LineTrailingWhiteSpaceCount
            {
                get { return lineTrailingWhiteSpaceCount; }
                set { lineTrailingWhiteSpaceCount = value; }
            }

            /// <summary>
            /// Gets or sets the width of the current line's trailing white space.
            /// </summary>
            public Int32 LineTrailingWhiteSpaceWidth
            {
                get { return lineTrailingWhiteSpaceWidth; }
                set { lineTrailingWhiteSpaceWidth = value; }
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

            /// <summary>
            /// Gets or sets an offset indicating the current position in a token that is being split across multiple lines.
            /// </summary>
            public Int32 TokenSplitOffset
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the layout engine is in the process of splitting a token across multiple lines.
            /// </summary>
            public Boolean TokenSplitInProgress
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the bounds of the text after layout has been performed, relative to the layout area.
            /// </summary>
            public Rectangle Bounds
            {
                get
                {
                    return new Rectangle(minLineOffset ?? 0, minBlockOffset ?? 0, actualWidth, actualHeight);
                }
            }

            // Property values.
            private Int32 positionX;
            private Int32 positionY;
            private Int32 lineInfoCommandIndex;
            private Int32 lineTrailingWhiteSpaceCount;
            private Int32 lineTrailingWhiteSpaceWidth;
            private Int32 lineCount;
            private Int32 lineWidth;
            private Int32 lineHeight;
            private Int32 lineLengthInText;
            private Int32 lineLengthInCommands;
            private Int32 actualWidth;
            private Int32 actualHeight;
            private Int32 totalLength;
            private Int32? minBlockOffset;
            private Int32? minLineOffset;
        }
    }
}
