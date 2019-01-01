using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
            /// <param name="lengthInSource">The length of the line of text in source characters.</param>
            /// <param name="lengthInShaped">The length of the line of text in shaped characters.</param>
            /// <param name="terminatingLineBreakLength">The source length of the line break which terminates the line, if there is one.</param>
            /// <param name="settings">The layout settings.</param>
            public void WriteLineInfo(TextLayoutCommandStream output, 
                Int32 lineWidth, Int32 lineHeight, Int32 lengthInCommands, Int32 lengthInGlyphs, Int32 lengthInSource, Int32 lengthInShaped, Int32 terminatingLineBreakLength, ref TextLayoutSettings settings)
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
                output.Seek(LineInfoCommandIndex);
                unsafe
                {
                    var ptr = (TextLayoutLineInfoCommand*)output.Data;
                    ptr->Offset = offset;
                    ptr->LineWidth = lineWidth;
                    ptr->LineHeight = lineHeight;
                    ptr->LengthInCommands = lengthInCommands;
                    ptr->LengthInGlyphs = lengthInGlyphs;
                    ptr->LengthInSource = lengthInSource;
                    ptr->LengthInShaped = lengthInShaped;
                    ptr->TerminatingLineBreakSourceLength = terminatingLineBreakLength;
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
                AdvanceLineToNextCommand(0, 0, 1, 0, 0, 0);
            }

            /// <summary>
            /// Advances the layout state past the current layout command.
            /// </summary>
            /// <param name="width">The width in pixels which the command contributes to the current line.</param>
            /// <param name="height">The height in pixels which the command contributes to the current line.</param>
            /// <param name="lengthInCommands">The number of layout commands which were ultimately produced in the output stream by this command.</param>
            /// <param name="lengthInGlyphs">The length of the command in rendered glyphs.</param>
            /// <param name="lengthInSource">The length of the command in source characters.</param>
            /// <param name="lengthInShaped">Thge length of the command in shaped characters.</param>
            /// <param name="isLineBreak">A value indicating whether the command is a line break.</param>
            public void AdvanceLineToNextCommand(Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInGlyphs, Int32 lengthInSource, Int32 lengthInShaped, Boolean isLineBreak = false)
            {
                PositionX += width;
                LineLengthInCommands += lengthInCommands;
                LineLengthInGlyphs += lengthInGlyphs;
                LineLengthInSource += lengthInSource;
                LineLengthInShaped += lengthInShaped;
                LineWidth += width;
                LineHeight = Math.Max(LineHeight, height);

                if (isLineBreak)
                    terminatingLineBreakLength = lengthInSource;

                TotalGlyphLength += lengthInGlyphs;
                TotalSourceLength += lengthInSource;
                TotalShapedLength += lengthInShaped;
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
            /// <param name="glyphOffset">The line break's offset within the stream of glyphs.</param>
            /// <param name="sourceOffset">The line break's offset within the source text.</param>
            /// <param name="sourceLength">The line break's length within the source text..</param>
            /// <param name="shapedOffset">The line break's offset within the shaping buffer.</param>
            /// <param name="settings">The current layout settings.</param>
            public void AdvanceLayoutToNextLineWithBreak(TextLayoutCommandStream output, Int32 glyphOffset, 
                Int32 sourceOffset, Int32 sourceLength, Int32 shapedOffset, ref TextLayoutSettings settings)
            {
                var lineSpacing = settings.Font.GetFace(UltravioletFontStyle.Regular).LineSpacing;

                var lineHeightCurrent = LineHeight;
                if (lineHeightCurrent == 0)
                    lineHeightCurrent = lineSpacing;
                
                output.WriteLineBreak(new TextLayoutLineBreakCommand(glyphOffset, 1, sourceOffset, sourceLength));
                AdvanceLineToNextCommand(0, lineHeightCurrent, 1, 1, sourceLength, 0, isLineBreak: true);

                AdvanceLayoutToNextLine(output, ref settings);
                AdvanceLineToNextCommand(0, 0, 0, 0, 0, 0);

                LineHeightTentative = lineSpacing;
            }

            /// <summary>
            /// Finalizes the current line by writing the line's metadata to the command stream and resetting
            /// state values which are associated with the current line.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void FinalizeLine(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if (LineHeight == 0)
                    LineHeight = LineHeightTentative;

                WriteLineInfo(output, LineWidth, LineHeight, LineLengthInCommands, 
                    LineLengthInGlyphs, LineLengthInSource, LineLengthInShaped, terminatingLineBreakLength ?? 0, ref settings);

                PositionX = 0;
                PositionY += LineHeight;
                ActualWidth = Math.Max(ActualWidth, LineWidth);
                ActualHeight += LineHeight;
                LineCount++;
                LineWidth = 0;
                LineHeight = 0;
                LineHeightTentative = 0;
                LineLengthInGlyphs = 0;
                LineLengthInSource = 0;
                LineLengthInShaped = 0;
                LineLengthInCommands = 0;
                LineInfoCommandIndex = output.Count;
                LineBreakCommand = null;
                LineBreakOffsetInput = null;
                LineBreakOffsetOutput = null;
                terminatingLineBreakLength = null;
                BrokenTextSizeBeforeBreak = null;
                BrokenTextSizeAfterBreak = null;
            }

            /// <summary>
            /// Finalizes the layout by writing the block's metadata to the command stream.
            /// </summary>
            /// <param name="output">The <see cref="TextLayoutCommandStream"/> which is being populated.</param>
            /// <param name="settings">The current layout settings.</param>
            public void FinalizeLayout(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if (LineHeightTentative > 0 || LineHeight > 0)
                    FinalizeLine(output, ref settings);

                WriteBlockInfo(output, ActualWidth, ActualHeight, LineCount, ref settings);

                output.Settings = settings;
                output.Bounds = Bounds;
                output.ActualWidth = ActualWidth;
                output.ActualHeight = ActualHeight;
                output.TotalGlyphLength = TotalGlyphLength;
                output.TotalSourceLength = TotalSourceLength;
                output.TotalShapedLength = TotalShapedLength;
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
            public unsafe Boolean ReplaceLastBreakingSpaceWithLineBreak(TextLayoutCommandStream output, ref TextLayoutSettings settings)
            {
                if (!LineBreakCommand.HasValue || !LineBreakOffsetInput.HasValue || !LineBreakOffsetOutput.HasValue)
                    return false;

                var sizeBeforeBreak = BrokenTextSizeBeforeBreak.Value;
                var sizeAfterBreak = BrokenTextSizeAfterBreak.Value;
                var brokenCommandSize = Size2.Zero;
                var brokenCommandGlyphOffset = 0;
                var brokenCommandGlyphLength = 0;
                var brokenCommandSourceOffset = 0;
                var brokenCommandSourceLength = 0;
                var brokenCommandShapedOffset = 0;
                var brokenCommandShapedLength = 0;

                var newLineHeight = sizeAfterBreak.Height;
                if (newLineHeight == 0)
                    newLineHeight = settings.Font.GetFace(UltravioletFontStyle.Regular).LineSpacing;

                // Truncate the command which is being broken.
                output.Seek(LineBreakCommand.Value);
                unsafe
                {
                    var cmd = (TextLayoutTextCommand*)output.Data;
                    var shape = (settings.Options & TextLayoutOptions.Shape) == TextLayoutOptions.Shape;

                    brokenCommandGlyphOffset = cmd->GlyphOffset;
                    brokenCommandGlyphLength = cmd->GlyphLength;
                    brokenCommandSourceOffset = cmd->SourceOffset;
                    brokenCommandSourceLength = cmd->SourceLength;
                    brokenCommandShapedOffset = cmd->ShapedOffset;
                    brokenCommandShapedLength = cmd->ShapedLength;
                    brokenCommandSize = cmd->Bounds.Size;

                    cmd->GlyphLength = LineBreakOffsetOutput.Value;
                    cmd->SourceLength = LineBreakOffsetInput.Value;
                    cmd->ShapedOffset = (shape && settings.Direction == TextDirection.RightToLeft) ? cmd->ShapedOffset + 1 : cmd->ShapedOffset;
                    cmd->ShapedLength = LineBreakOffsetOutput.Value;

                    cmd->TextWidth = (Int16)sizeBeforeBreak.Width;
                    cmd->TextHeight = (Int16)sizeBeforeBreak.Height;
                }
                output.SeekNextCommand();

                // Insert a line break, a new line, and the second half of the truncated text.
                var part1GlyphLength = LineBreakOffsetOutput.Value;
                var part1SourceLength = LineBreakOffsetInput.Value;
                var part1ShapedLength = LineBreakOffsetOutput.Value;

                var part2GlyphOffset = brokenCommandGlyphOffset + (LineBreakOffsetOutput.Value + 1);
                var part2GlyphLength = brokenCommandGlyphLength - (part1GlyphLength + 1);
                var part2SourceOffset = brokenCommandSourceOffset + (LineBreakOffsetInput.Value + 1);
                var part2SourceLength = brokenCommandSourceLength - (part1SourceLength + 1);
                var part2ShapedOffset = brokenCommandShapedOffset + (LineBreakOffsetOutput.Value + 1);
                var part2ShapedLength = brokenCommandShapedLength - (part1ShapedLength + 1);
                var part2IsNotDegenerate = (part2GlyphLength > 0);

                var numberOfObjects = part2IsNotDegenerate ? 3 : 2;
                var numberOfBytes =
                    sizeof(TextLayoutLineBreakCommand) +
                    sizeof(TextLayoutLineInfoCommand) +
                    (part2IsNotDegenerate ? sizeof(TextLayoutTextCommand) : 0);

                var insertionPosition = output.InternalObjectStream.PositionInObjects;

                output.InternalObjectStream.ReserveInsert(numberOfObjects, numberOfBytes);

                *(TextLayoutLineBreakCommand*)output.Data = new TextLayoutLineBreakCommand(part2GlyphOffset - 1, 1, brokenCommandSourceOffset + part1SourceLength, 1);
                output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineBreakCommand));

                *(TextLayoutCommandType*)output.Data = TextLayoutCommandType.LineInfo;
                output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineInfoCommand));

                if (part2IsNotDegenerate)
                {
                    var glyphOffset = part2GlyphOffset;
                    var glyphLength = part2GlyphLength;

                    var sourceOffset = part2SourceOffset;
                    var sourceLength = part2SourceLength;

                    var shapedOffset = part2ShapedOffset;
                    var shapedLength = part2ShapedLength;

                    *(TextLayoutTextCommand*)output.InternalObjectStream.Data = new TextLayoutTextCommand(
                        glyphOffset, glyphLength, sourceOffset, sourceLength, shapedOffset, shapedLength,
                        0, PositionY + LineHeight, (Int16)sizeAfterBreak.Width, (Int16)sizeAfterBreak.Height);
                    output.InternalObjectStream.FinalizeObject(sizeof(TextLayoutTextCommand));
                }

                // Add the line break command to the broken line.
                AdvanceLineToNextCommand(0, 0, 1, 1, 1, 1);

                // Recalculate the parameters for the broken line.
                output.Seek(LineInfoCommandIndex + 1);

                var brokenLineWidth = 0;
                var brokenLineHeight = 0;
                var brokenLineLengthInGlyphs = 0;
                var brokenLineLengthInSource = 0;
                var brokenLineLengthInShaped = 0;
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
                                brokenLineLengthInGlyphs += cmd->GlyphLength;
                                brokenLineLengthInSource += cmd->SourceLength;
                                brokenLineLengthInShaped += cmd->ShapedLength;
                            }
                            break;

                        case TextLayoutCommandType.Icon:
                            {
                                var cmd = (TextLayoutIconCommand*)output.Data;
                                brokenLineWidth += cmd->Bounds.Width;
                                brokenLineHeight = Math.Max(brokenLineHeight, cmd->Bounds.Height);
                                brokenLineLengthInGlyphs += 1;
                                brokenLineLengthInSource += cmd->SourceLength;
                            }
                            break;

                        case TextLayoutCommandType.LineBreak:
                            {
                                var cmd = (TextLayoutLineBreakCommand*)output.Data;
                                brokenLineLengthInGlyphs += cmd->GlyphLength;
                                brokenLineLengthInShaped += 1;
                                brokenLineLengthInSource += cmd->SourceLength;
                            }
                            break;
                    }
                    brokenLineLengthInCommands++;
                    output.SeekNextCommand();
                }

                // Finalize the broken line.
                terminatingLineBreakLength = 1;
                TotalSourceLength = (TotalSourceLength - LineLengthInSource) + brokenLineLengthInSource;
                TotalShapedLength = (TotalShapedLength - LineLengthInShaped) + brokenLineLengthInShaped;
                TotalGlyphLength = (TotalGlyphLength - LineLengthInGlyphs) + brokenLineLengthInGlyphs;
                LineWidth = brokenLineWidth;
                LineHeight = brokenLineHeight;
                LineLengthInSource = brokenLineLengthInSource;
                LineLengthInShaped = brokenLineLengthInShaped;
                LineLengthInGlyphs = brokenLineLengthInGlyphs;
                LineLengthInCommands = brokenLineLengthInCommands;
                FinalizeLine(output, ref settings);

                // Fixup token bounds and update parameters for new line.
                LineInfoCommandIndex = insertionPosition + 1;
                while (output.StreamPositionInObjects < output.Count)
                {
                    var width = 0;
                    var height = 0;
                    var lengthInCommands = 0;
                    var lengthInGlyphs = 0;
                    var lengthInSource = 0;
                    var lengthInShaped = 0;

                    switch (*(TextLayoutCommandType*)output.Data)
                    {
                        case TextLayoutCommandType.Text:
                            {
                                var cmd = (TextLayoutTextCommand*)output.Data;
                                width = cmd->TextWidth;
                                height = cmd->TextHeight;
                                lengthInCommands = 1;
                                lengthInGlyphs = cmd->GlyphLength;
                                lengthInSource = cmd->SourceLength;
                                lengthInShaped = cmd->ShapedLength;
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
                                lengthInGlyphs = 1;
                                lengthInSource = cmd->SourceLength;
                                lengthInShaped = 0;
                                cmd->IconX = PositionX;
                                cmd->IconY = PositionY;
                            }
                            break;

                        case TextLayoutCommandType.LineBreak:
                            {
                                var cmd = (TextLayoutLineBreakCommand*)output.Data;
                                lengthInGlyphs = cmd->GlyphLength;
                                lengthInSource = cmd->SourceLength;
                                lengthInShaped = 0;
                            }
                            break;
                    }

                    AdvanceLineToNextCommand(width, height, lengthInCommands, lengthInGlyphs, lengthInSource, lengthInShaped);
                    output.SeekNextCommand();
                }

                return true;
            }

            /// <summary>
            /// Gets or sets the metadata for the fallback font which is currently active.
            /// </summary>
            public FallbackFontInfo? FallbackFontInfo { get; set; }

            /// <summary>
            /// Gets or sets the fallback font which is currently active.
            /// </summary>
            public UltravioletFont FallbackFont { get; set; }

            /// <summary>
            /// Gets or sets the x-coordinate at which the next token will be placed.
            /// </summary>
            public Int32 PositionX { get; set; }

            /// <summary>
            /// Gets or sets the y-coordinate at which the next token will be placed.
            /// </summary>
            public Int32 PositionY { get; set; }

            /// <summary>
            /// Gets or sets the index of the command token that contains the metadata for the current line.
            /// </summary>
            public Int32 LineInfoCommandIndex { get; set; }

            /// <summary>
            /// Gets or sets the number of lines in the laid-out text.
            /// </summary>
            public Int32 LineCount { get; set; }

            /// <summary>
            /// Gets or sets the width of the current line in pixels.
            /// </summary>
            public Int32 LineWidth { get; set; }

            /// <summary>
            /// Gets or sets the height of the current line in pixels.
            /// </summary>
            public Int32 LineHeight { get; set; }

            /// <summary>
            /// Gets or sets the tentative height of the line in pixels. This height will only be used if
            /// the line has no rendered tokens.
            /// </summary>
            public Int32 LineHeightTentative { get; set; }

            /// <summary>
            /// Gets or sets the length of the current line in glyphs.
            /// </summary>
            public Int32 LineLengthInGlyphs { get; set; }

            /// <summary>
            /// Gets or sets the length of the current line in the source text.
            /// </summary>
            public Int32 LineLengthInSource { get; set; }

            /// <summary>
            /// Gets or sets the length of the current line in shaped characters.
            /// </summary>
            public Int32 LineLengthInShaped { get; set; }

            /// <summary>
            /// Gets or sets the length of the current line in commands.
            /// </summary>
            public Int32 LineLengthInCommands { get; set; }

            /// <summary>
            /// Gets or sets the width of the area which is occupied by text after layout is performed.
            /// </summary>
            public Int32 ActualWidth { get; set; }

            /// <summary>
            /// Gets or sets the height of the area which is occupied by text after layout is performed.
            /// </summary>
            public Int32 ActualHeight { get; set; }

            /// <summary>
            /// Gets or sets the total length of the text in rendered glyphs.
            /// </summary>
            public Int32 TotalGlyphLength { get; set; }

            /// <summary>
            /// Gets or sets the total length of the text in source characters.
            /// </summary>
            public Int32 TotalSourceLength { get; set; }

            /// <summary>
            /// Gets or sets the total length of the text in shaped characters.
            /// </summary>
            public Int32 TotalShapedLength { get; set; }

            /// <summary>
            /// Gets or sets the offset within the current parser token at which to begin processing.
            /// </summary>
            public Int32? ParserTokenOffset { get; set; }

            /// <summary>
            /// Gets or sets the index of the command that contains the point at which the current line will break.
            /// </summary>
            public Int32? LineBreakCommand { get; set; }

            /// <summary>
            /// Gets or sets the offset within the accumulated input text at which the current line will break.
            /// </summary>
            public Int32? LineBreakOffsetInput { get; set; }

            /// <summary>
            /// Gets or sets the offset within the accumulated output text at which the current line will break.
            /// </summary>
            public Int32? LineBreakOffsetOutput { get; set; }

            /// <summary>
            /// Gets or sets the size of the pre-break portion of the text which contains this line's break point.
            /// </summary>
            public Size2? BrokenTextSizeBeforeBreak { get; set; }

            /// <summary>
            /// Gets or sets the size of the post-break portion of the text which contains this line's break point.
            /// </summary>
            public Size2? BrokenTextSizeAfterBreak { get; set; }

            /// <summary>
            /// Gets the bounds of the text after layout has been performed, relative to the layout area.
            /// </summary>
            public Rectangle Bounds
            {
                get
                {
                    return new Rectangle(minLineOffset ?? 0, minBlockOffset ?? 0, ActualWidth, ActualHeight);
                }
            }

            /// <summary>
            /// This method corrects the offsets of lines in a layout which is right- or center-aligned but which
            /// did not have a constrained horizontal layout space.
            /// </summary>
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

            // State values.
            private Int32? minBlockOffset;
            private Int32? minLineOffset;
            private Int32? terminatingLineBreakLength;
        }
    }
}
