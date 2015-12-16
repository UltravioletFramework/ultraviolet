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
            public void WriteBlockInfo(TextLayoutCommandStream output, Int32 blockWidth, Int32 blockHeight, Int32 lengthInLines, ref TextLayoutSettings settings)
            {
                var offset = 0;

                if (settings.Height.HasValue)
                {
                    if ((settings.Flags & TextFlags.AlignBottom) == TextFlags.AlignBottom)
                        offset = (settings.Height.Value - blockHeight);
                    else if ((settings.Flags & TextFlags.AlignMiddle) == TextFlags.AlignMiddle)
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
            /// <param name="terminatedByLineBreak">A value indicating whether the line is terminated by a line break.</param>
            /// <param name="settings">The layout settings.</param>
            [SecuritySafeCritical]
            public void WriteLineInfo(TextLayoutCommandStream output, 
                Int32 lineWidth, Int32 lineHeight, Int32 lengthInCommands, Int32 lengthInGlyphs, Boolean terminatedByLineBreak, ref TextLayoutSettings settings)
            {
                var offset = 0;

                if (settings.Width.HasValue)
                {
                    if ((settings.Flags & TextFlags.AlignRight) == TextFlags.AlignRight)
                        offset = (settings.Width.Value - lineWidth);
                    else if ((settings.Flags & TextFlags.AlignCenter) == TextFlags.AlignCenter)
                        offset = (settings.Width.Value - lineWidth) / 2;
                }

                var outputStreamPosition = output.StreamPositionInObjects;
                output.Seek(lineInfoCommandIndex);
                unsafe
                {
                    var ptr = (TextLayoutLineInfoCommand*)output.Data;
                    ptr->Offset = offset;
                    ptr->LineWidth = lineWidth;
                    ptr->LineHeight = lineHeight;
                    ptr->LengthInCommands = lengthInCommands;
                    ptr->LengthInGlyphs = lengthInGlyphs;
                    ptr->TerminatedByLineBreak = terminatedByLineBreak;
                }
                output.Seek(outputStreamPosition);

                minLineOffset = (minLineOffset.HasValue) ? Math.Min(minLineOffset.Value, offset) : offset;
            }

            /// <summary>
            /// Advances the layout state past the current layout command, assuming that the command is a styling command
            /// with zero size and zero length in characters.
            /// </summary>
            public void AdvanceLineToNextCommand()
            {
                AdvanceLineToNextCommand(0, 0, 1, 0);
            }

            /// <summary>
            /// Advances the layout state past the current layout command.
            /// </summary>
            /// <param name="width">The width in pixels which the command contributes to the current line.</param>
            /// <param name="height">The height in pixels which the command contributes to the current line.</param>
            /// <param name="lengthInCommands">The number of layout commands which were ultimately produced in the output stream by this command.</param>
            /// <param name="lengthInText">The number of characters of text which are represented by this command.</param>
            /// <param name="isLineBreak">A value indicating whether the command is a line break.</param>
            public void AdvanceLineToNextCommand(Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInText, Boolean isLineBreak = false)
            {
                positionX += width;
                lineLengthInCommands += lengthInCommands;
                lineLengthInText += lengthInText;
                lineWidth += width;
                lineHeight = Math.Max(lineHeight, height);

                if (isLineBreak)
                    lineIsTerminatedByLineBreak = true;

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
            /// <param name="length">The number of characters in the line break.</param>
            /// <param name="settings">The current layout settings.</param>
            public void AdvanceLayoutToNextLineWithBreak(TextLayoutCommandStream output, Int32 length, ref TextLayoutSettings settings)
            {
                var lineHeightCurrent = lineHeight;
                if (lineHeightCurrent == 0)
                    lineHeight = settings.Font.GetFace(SpriteFontStyle.Regular).LineSpacing;

                output.WriteLineBreak(new TextLayoutLineBreakCommand(length));
                AdvanceLineToNextCommand(0, lineHeightCurrent, 1, length, isLineBreak: true);

                AdvanceLayoutToNextLine(output, ref settings);
                AdvanceLineToNextCommand(0, lineHeightCurrent, 0, 0);
            }

            /// <summary>
            /// Finalizes the current line by writing the line's metadata to the command stream and resetting
            /// state values which are associated with the current line.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void FinalizeLine(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                WriteLineInfo(output, lineWidth, lineHeight, lineLengthInCommands, lineLengthInText, lineIsTerminatedByLineBreak, ref settings);

                positionX = 0;
                positionY += lineHeight;
                actualWidth = Math.Max(actualWidth, lineWidth);
                actualHeight += lineHeight;
                lineCount++;
                lineWidth = 0;
                lineHeight = 0;
                lineLengthInText = 0;
                lineLengthInCommands = 0;
                lineInfoCommandIndex = output.Count;
                lineBreakCommand = null;
                lineBreakOffset = null;
                lineIsTerminatedByLineBreak = false;
                brokenTextSizeBeforeBreak = null;
                brokenTextSizeAfterBreak = null;
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

                WriteBlockInfo(output, ActualWidth, ActualHeight, LineCount, ref settings);

                output.Settings = settings;
                output.Bounds = Bounds;
                output.ActualWidth = ActualWidth;
                output.ActualHeight = ActualHeight;
                output.TotalLength = TotalLength;
                output.LineCount = LineCount;

                if (!settings.Width.HasValue)
                {
                    if ((settings.Flags & TextFlags.AlignCenter) == TextFlags.AlignCenter ||
                        (settings.Flags & TextFlags.AlignRight) == TextFlags.AlignRight)
                    {
                        FixHorizontalAlignmentForUnconstrainedLayout(output, ref settings);
                    }
                }
            }

            /// <summary>
            /// Replaces the last breaking space on the current line with a line break.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            [SecuritySafeCritical]
            public unsafe Boolean ReplaceLastBreakingSpaceWithLineBreak(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if (!lineBreakCommand.HasValue || !lineBreakOffset.HasValue)
                    return false;

                var sizeBeforeBreak = brokenTextSizeBeforeBreak.Value;
                var sizeAfterBreak = brokenTextSizeAfterBreak.Value;
                var brokenCommandSize = Size2.Zero;
                var brokenCommandOffset = 0;
                var brokenCommandLength = 0;

                var newLineHeight = sizeAfterBreak.Height;
                if (newLineHeight == 0)
                    newLineHeight = settings.Font.GetFace(SpriteFontStyle.Regular).LineSpacing;

                // Truncate the command which is being broken.
                output.Seek(lineBreakCommand.Value);
                unsafe
                {
                    var cmd = (TextLayoutTextCommand*)output.Data;

                    brokenCommandOffset = cmd->TextOffset;
                    brokenCommandLength = cmd->TextLength;
                    brokenCommandSize = cmd->Bounds.Size;

                    cmd->TextLength = lineBreakOffset.Value;
                    cmd->TextWidth = (Int16)sizeBeforeBreak.Width;
                    cmd->TextHeight = (Int16)sizeBeforeBreak.Height;
                }
                output.SeekNextCommand();

                // Insert a line break, a new line, and the second half of the truncated text.
                var part1Length = lineBreakOffset.Value;
                var part2Offset = brokenCommandOffset + (lineBreakOffset.Value + 1);
                var part2Length = brokenCommandLength - (part1Length + 1);
                var part2IsNotDegenerate = (part2Length > 0);

                var numberOfObjects = part2IsNotDegenerate ? 3 : 2;
                var numberOfBytes =
                    sizeof(TextLayoutLineBreakCommand) +
                    sizeof(TextLayoutLineInfoCommand) +
                    (part2IsNotDegenerate ? sizeof(TextLayoutTextCommand) : 0);

                var insertionPosition = output.InternalObjectStream.PositionInObjects;

                output.InternalObjectStream.ReserveInsert(numberOfObjects, numberOfBytes);

                *(TextLayoutLineBreakCommand*)output.Data = new TextLayoutLineBreakCommand(1);
                output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineBreakCommand));

                *(TextLayoutCommandType*)output.Data = TextLayoutCommandType.LineInfo;
                output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineInfoCommand));

                if (part2IsNotDegenerate)
                {
                    var textOffset = part2Offset;
                    var textLength = part2Length;

                    *(TextLayoutTextCommand*)output.InternalObjectStream.Data = new TextLayoutTextCommand(textOffset, textLength,
                        0, positionY + lineHeight, (Int16)sizeAfterBreak.Width, (Int16)sizeAfterBreak.Height);
                    output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutTextCommand));
                }

                // Add the line break command to the broken line.
                AdvanceLineToNextCommand(0, 0, 1, 1);

                // Recalculate the parameters for the broken line.
                output.Seek(LineInfoCommandIndex + 1);

                var brokenLineWidth = 0;
                var brokenLineHeight = 0;
                var brokenLineLengthInText = 0;
                var brokenLineLengthInCommands = 0;

                var cmdType = TextLayoutCommandType.None;
                while ((cmdType = *(TextLayoutCommandType*)output.Data) != TextLayoutCommandType.LineInfo)
                {
                    switch (cmdType)
                    {
                        case TextLayoutCommandType.Text:
                            {
                                var cmd = (TextLayoutTextCommand*)output.Data;
                                brokenLineWidth += cmd->TextWidth;
                                brokenLineHeight = Math.Max(brokenLineHeight, cmd->TextHeight);
                                brokenLineLengthInText += cmd->TextLength;
                            }
                            break;

                        case TextLayoutCommandType.Icon:
                            {
                                var cmd = (TextLayoutIconCommand*)output.Data;
                                brokenLineWidth += cmd->Bounds.Width;
                                brokenLineHeight = Math.Max(brokenLineHeight, cmd->Bounds.Height);
                                brokenLineLengthInText += 1;
                            }
                            break;

                        case TextLayoutCommandType.LineBreak:
                            {
                                var cmd = (TextLayoutLineBreakCommand*)output.Data;
                                brokenLineLengthInText += cmd->Length;
                            }
                            break;
                    }
                    brokenLineLengthInCommands++;
                    output.SeekNextCommand();
                }

                // Finalize the broken line.
                totalLength = (totalLength - lineLengthInText) + brokenLineLengthInText;
                lineWidth = brokenLineWidth;
                lineHeight = brokenLineHeight;
                lineLengthInText = brokenLineLengthInText;
                lineLengthInCommands = brokenLineLengthInCommands;
                FinalizeLine(output, ref settings);

                // Fixup token bounds and update parameters for new line.
                LineInfoCommandIndex = insertionPosition + 1;
                while (output.StreamPositionInObjects < output.Count)
                {
                    var width = 0;
                    var height = 0;
                    var lengthInCommands = 0;
                    var lengthInText = 0;

                    switch (*(TextLayoutCommandType*)output.Data)
                    {
                        case TextLayoutCommandType.Text:
                            {
                                var cmd = (TextLayoutTextCommand*)output.Data;
                                width = cmd->TextWidth;
                                height = cmd->TextHeight;
                                lengthInCommands = 1;
                                lengthInText = cmd->TextLength;
                                cmd->TextX = PositionX;
                                cmd->TextY = PositionY;
                            }
                            break;

                        case TextLayoutCommandType.Icon:
                            {
                                var cmd = (TextLayoutIconCommand*)output.Data;
                                width = cmd->IconWidth;
                                height = cmd->IconHeight;
                                lengthInCommands = 1;
                                lengthInText = 1;
                                cmd->IconX = PositionX;
                                cmd->IconY = PositionY;
                            }
                            break;

                        case TextLayoutCommandType.LineBreak:
                            {
                                var cmd = (TextLayoutLineBreakCommand*)output.Data;
                                lengthInText += cmd->Length;
                            }
                            break;
                    }

                    AdvanceLineToNextCommand(width, height, lengthInCommands, lengthInText);
                    output.SeekNextCommand();
                }

                return true;
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
            /// Gets or sets the offset within the current parser token at which to begin processing.
            /// </summary>
            public Int32? ParserTokenOffset
            {
                get { return parserTokenOffset; }
                set { parserTokenOffset = value; }
            }

            /// <summary>
            /// Gets or sets the index of the command that contains the point at which the current line will break.
            /// </summary>
            public Int32? LineBreakCommand
            {
                get { return lineBreakCommand; }
                set { lineBreakCommand = value; }
            }

            /// <summary>
            /// Gets or sets the offset within <see cref="LineBreakCommand"/> at which the line will break.
            /// </summary>
            public Int32? LineBreakOffset
            {
                get { return lineBreakOffset; }
                set { lineBreakOffset = value; }
            }

            /// <summary>
            /// Gets or sets the size of the pre-break portion of the text which contains this line's break point.
            /// </summary>
            public Size2? BrokenTextSizeBeforeBreak
            {
                get { return brokenTextSizeBeforeBreak; }
                set { brokenTextSizeBeforeBreak = value; }
            }

            /// <summary>
            /// Gets or sets the size of the post-break portion of the text which contains this line's break point.
            /// </summary>
            public Size2? BrokenTextSizeAfterBreak
            {
                get { return brokenTextSizeAfterBreak; }
                set { brokenTextSizeAfterBreak = value; }
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

            /// <summary>
            /// This method corrects the offsets of lines in a layout which is right- or center-aligned but which
            /// did not have a constrained horizontal layout space.
            /// </summary>
            [SecuritySafeCritical]
            private unsafe void FixHorizontalAlignmentForUnconstrainedLayout(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                output.Seek(0);

                while (output.SeekNextLine())
                {
                    var lineInfo = (TextLayoutLineInfoCommand*)output.InternalObjectStream.Data;

                    if ((settings.Flags & TextFlags.AlignRight) == TextFlags.AlignRight)
                        lineInfo->Offset = (output.ActualWidth - lineInfo->LineWidth);
                    else if ((settings.Flags & TextFlags.AlignCenter) == TextFlags.AlignCenter)
                        lineInfo->Offset = (output.ActualWidth - lineInfo->LineWidth) / 2;
                }
            }

            // Property values.
            private Int32 positionX;
            private Int32 positionY;
            private Int32 lineInfoCommandIndex;
            private Int32 lineCount;
            private Int32 lineWidth;
            private Int32 lineHeight;
            private Int32 lineLengthInText;
            private Int32 lineLengthInCommands;
            private Int32 actualWidth;
            private Int32 actualHeight;
            private Int32 totalLength;
            private Int32? parserTokenOffset;
            private Int32? lineBreakCommand;
            private Int32? lineBreakOffset;
            private Size2? brokenTextSizeBeforeBreak;
            private Size2? brokenTextSizeAfterBreak;
            private Int32? minBlockOffset;
            private Int32? minLineOffset;
            private Boolean lineIsTerminatedByLineBreak;
        }
    }
}
