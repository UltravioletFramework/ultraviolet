using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a method which determines the color with which to draw a link.
    /// </summary>
    /// <param name="target">The link target.</param>
    /// <param name="visited">A value indicating whether the link has been visited.</param>
    /// <param name="hovering">A value indicating whether the link is currently under the cursor.</param>
    /// <param name="active">A value indicating whether the link is currently active (i.e. being clicked).</param>
    /// <param name="currentColor">The color to apply to the specified link.</param>
    /// <returns>The color with which the link should be drawn.</returns>
    public delegate Color LinkColorizer(String target, Boolean visited, Boolean hovering, Boolean active, Color currentColor);

    /// <summary>
    /// Represents a method which executes a link.
    /// </summary>
    /// <param name="target">The target of the link being executed.</param>
    /// <returns><see langword="true"/> if the link was executed successfully;
    /// otherwise, <see langword="false"/>.</returns>
    public delegate Boolean LinkClickHandler(String target);

    /// <summary>
    /// Represents a method which retrieves a value indicating whether the 
    /// specified link target has been visited.
    /// </summary>
    /// <param name="target">The link target to evaluate.</param>
    /// <returns><see langword="true"/> if the specified link target has been visited;
    /// otherwise, <see langword="false"/>.</returns>
    public delegate Boolean LinkStateEvaluator(String target);

    /// <summary>
    /// Contains methods for rendering formatted text.
    /// </summary>
    public sealed unsafe partial class TextRenderer
    {
        /// <summary>
        /// Updates the position of the cursor relative to the specified command stream's text.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The cursor's position relative to the text's layout area, 
        /// or <see langword="null"/> to indicate that the cursor is not over the text.</param>
        public void UpdateCursor(TextLayoutCommandStream input, Point2? position)
        {
            Contract.Require(input, nameof(input));

            input.UpdateCursor(position);
        }

        /// <summary>
        /// Updates the position of the cursor relative to the specified command stream's text.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate of the cursor relative to the text's layout area.</param>
        /// <param name="y">The y-coordinate of the cursor relative to the text's layout area.</param>
        public void UpdateCursor(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            Contract.Require(input, nameof(input));

            input.UpdateCursor(x, y);
        }

        /// <summary>
        /// Deactivates the specified command stream's currently active link, if it has one.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <returns><see langword="true"/> if a link was deactivated; otherwise, <see langword="false"/>.</returns>
        public Boolean DeactivateLink(TextLayoutCommandStream input)
        {
            Contract.Require(input, nameof(input));

            var hadActivatedLinkIndex = input.ActiveLinkIndex.HasValue;
            input.ActivateLink(null);
            return hadActivatedLinkIndex;
        }

        /// <summary>
        /// Activates the link at the position within the input command stream specified by the value of
        /// the <see cref="TextLayoutCommandStream.CursorPosition"/> property, if any such link exists,
        /// and deactivates all other links within the command stream.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <returns><see langword="true"/> if a link was found and activated; otherwise, <see langword="false"/>.</returns>
        public Boolean ActivateLinkAtCursor(TextLayoutCommandStream input)
        {
            Contract.Require(input, nameof(input));

            var position = input.CursorPosition;
            if (position == null)
            {
                input.ActivateLink(null);
                return false;
            }

            return ActivateLinkAtPosition(input, position.Value.X, position.Value.Y);
        }

        /// <summary>
        /// Activates the link at the specified position within the input command stream, if any such link exists,
        /// and deactivates all other links within the command stream.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The bounds-relative x-coordinate to search for a link.</param>
        /// <param name="y">The bounds-relative y-coordinate to search for a link.</param>
        /// <returns><see langword="true"/> if a link was found and activated; otherwise, <see langword="false"/>.</returns>
        public Boolean ActivateLinkAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            Contract.Require(input, nameof(input));
            
            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var linkIndex = GetLinkIndexAtPosition(input, x, y);

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            input.ActivateLink(linkIndex);
            return input.ActiveLinkIndex.HasValue;
        }

        /// <summary>
        /// Executes the specified command stream's currently active link, if it has one.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="onlyIfLinkContainsCursor">A value specifying whether the link should only be activated
        /// if it currently contains the cursor position as specified by the value of the layout
        /// stream's <see cref="TextLayoutCommandStream.CursorPosition"/> property.</param>
        /// <returns><see langword="true"/> if a link is found and successfully executed; otherwise, <see langword="false"/>.</returns>
        public Boolean ExecuteActivatedLink(TextLayoutCommandStream input, Boolean onlyIfLinkContainsCursor = true)
        {
            Contract.Require(input, nameof(input));

            if (input.ActiveLinkIndex == null)
                return false;

            if (onlyIfLinkContainsCursor)
            {
                if (GetLinkIndexAtCursor(input) != input.ActiveLinkIndex)
                {
                    input.ActivateLink(null);
                    return false;
                }
            }

            var target = input.GetLinkTarget(input.ActiveLinkIndex.Value);
            var result = false;

            if (LinkClickHandler != null)
                result = LinkClickHandler(target);

            input.ActivateLink(null);

            return result;
        }

        /// <summary>
        /// Gets the index of the link at the cursor position specified by the value
        /// of the <see cref="TextLayoutCommandStream.CursorPosition"/> property.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <returns>The index of the link at the specified position within the input command
        /// stream's layout area, or <see langword="null"/> if there no such link.</returns>
        public Int16? GetLinkIndexAtCursor(TextLayoutCommandStream input)
        {
            Contract.Require(input, nameof(input));

            if (input.CursorPosition == null)
                return null;

            var position = input.CursorPosition.Value;
            return GetLinkIndexAtPosition(input, position.X, position.Y);
        }

        /// <summary>
        /// Gets the index of the link at the specified position within the 
        /// input command stream's layout area.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The layout-relative x-coordinate of the position to evaluate.</param>
        /// <param name="y">The layout-relative y-coordinate of the position to evaluate.</param>
        /// <returns>The index of the link at the specified position within the input command
        /// stream's layout area, or <see langword="null"/> if there no such link.</returns>
        public Int16? GetLinkIndexAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            Contract.Require(input, nameof(input));

            var linkIndex = default(Int16?);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var line = default(Int32?);
            var glyph = GetGlyphOrInsertionPointAtPosition(input, x, y, out line, InsertionPointSearchMode.BeforeGlyph);

            if (glyph != null)
                linkIndex = linkStack.Count > 0 ? linkStack.Peek() : (Int16?)null;

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return linkIndex;
        }

        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="stretch">If <see langword="true"/>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <see langword="null"/> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Vector2 position, Boolean stretch = false)
        {
            return GetLineAtPosition(input, (Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="stretch">If <see langword="true"/>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <see langword="null"/> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Point2 position, Boolean stretch = false)
        {
            return GetLineAtPosition(input, position.X, position.Y);
        }

        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <param name="stretch">If <see langword="true"/>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <see langword="null"/> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, Boolean stretch = false)
        {
            Contract.Require(input, nameof(input));

            if (x < 0 || y < 0 || input.Count == 0)
                return null;

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var offsetX = 0;
            var offsetY = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;

            if (y < offsetY || y >= offsetY + input.ActualHeight)
                return null;

            input.SeekNextLine();

            for (int i = 0; i < input.LineCount; i++)
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;
                offsetX = cmd->Offset;

                if (y >= offsetY && y < offsetY + cmd->LineHeight)
                {
                    if (stretch || (x >= offsetX && x < offsetX + cmd->LineWidth))
                    {
                        return i;
                    }
                    break;
                }

                offsetY += cmd->LineHeight;
                input.SeekNextLine();
            }

            if (acquiredPointers)
                input.ReleasePointers();

            return null;
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position)
        {
            Int32? lineAtPosition;
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position, out Int32? lineAtPosition)
        {
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="snapToLine">A value indicating whether the search position should be snapped to the nearest line of text.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position, Boolean snapToLine, out Int32? lineAtPosition)
        {
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, snapToLine, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position)
        {
            Int32? lineAtPosition;
            return GetGlyphAtPosition(input, position.X, position.Y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position, out Int32? lineAtPosition)
        {
            return GetGlyphAtPosition(input, position.X, position.Y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="snapToLine">A value indicating whether the search position should be snapped to the nearest line of text.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position, Boolean snapToLine, out Int32? lineAtPosition)
        {
            return GetGlyphAtPosition(input, position.X, position.Y, snapToLine, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            Int32? lineAtPosition;
            return GetGlyphAtPosition(input, x, y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32? lineAtPosition)
        {
            return GetGlyphAtPosition(input, x, y, false, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <param name="snapToLine">A value indicating whether the search position should be snapped to the nearest line of text.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, Boolean snapToLine, out Int32? lineAtPosition)
        {
            Contract.Require(input, nameof(input));

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            var result = GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPosition, 
                snapToLine ? InsertionPointSearchMode.SnapToLine : InsertionPointSearchMode.BeforeGlyph);

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return result;
        }

        /// <summary>
        /// Gets the index of the glyph which corresponds to the specified character index in the source text.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The character index for which to find the corresponding glyph index.</param>
        /// <returns>The glyph index which corresponds to the specified character index.</returns>
        public Int32 GetGlyphAtCharacterIndex(TextLayoutCommandStream input, Int32 index)
        {
            Contract.Require(input, nameof(input));
            Contract.EnsureRange(index >= 0 && index <= input.TotalSourceLength, nameof(index));

            if (index == input.TotalSourceLength)
                return input.TotalGlyphLength;

            var trueMatchFound = false;
            var bestMatchFound = false;
            var bestMatchGlyph = Int32.MaxValue;
            var bestMatchSource = Int32.MaxValue;

            var settings = input.Settings;
            var rtl = (settings.Direction == TextDirection.RightToLeft);
            var shaped = (settings.Options & TextLayoutOptions.Shape) == TextLayoutOptions.Shape;
            var bold = (settings.Style == UltravioletFontStyle.Bold || settings.Style == UltravioletFontStyle.BoldItalic);
            var italic = (settings.Style == UltravioletFontStyle.Italic || settings.Style == UltravioletFontStyle.BoldItalic);
            var source = shaped ? input.GetShapedStringBuilder() : CreateSourceUnionFromSegmentOrigin(input.SourceText);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);
            input.SeekNextLine();

            var seekState = new TextSeekState { LineIndex = -1 };
            SkipToLineContainingSourceCharacter(input, index, ref seekState);

            for (int i = 0; i < seekState.LineLengthInCommands && !trueMatchFound; i++)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineBreak:
                        {
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            if (index >= cmd->SourceOffset && index < cmd->SourceOffset + cmd->SourceLength)
                                trueMatchFound = true;

                            if (trueMatchFound || index < cmd->SourceOffset && cmd->SourceOffset < bestMatchSource)
                            {
                                bestMatchFound = true;
                                bestMatchGlyph = cmd->GlyphOffset;
                                bestMatchSource = cmd->SourceOffset;
                            }
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
                            var cmd = (TextLayoutTextCommand*)input.Data;
                            if (index >= cmd->SourceOffset && index < cmd->SourceOffset + cmd->SourceLength)
                            {
                                if (shaped)
                                {
                                    var text = source.CreateShapedStringSegmentFromSameOrigin(cmd->ShapedOffset, cmd->ShapedLength);
                                    for (int j = 0; j < text.Length; j++)
                                    {
                                        var c = text[j];
                                        var cIndexSource = c.SourceIndex;
                                        var cIndexGlyph = cmd->GlyphOffset + (rtl ? (cmd->GlyphLength - 1) - j : j);
                                        if (cIndexSource >= index && cIndexSource < bestMatchSource && cIndexGlyph < bestMatchGlyph)
                                        {
                                            bestMatchFound = true;
                                            bestMatchSource = cIndexSource;
                                            bestMatchGlyph = cIndexGlyph;
                                        }
                                    }
                                    trueMatchFound = true;
                                }
                                else
                                {
                                    var text = source.CreateStringSegmentFromSameOrigin(cmd->SourceOffset, cmd->SourceLength);
                                    for (int j = 0; j < text.Length; j++)
                                    {
                                        var c = text[j];
                                        var cIndexSource = cmd->SourceOffset + j;
                                        var cIndexGlyph = cmd->GlyphOffset + j;
                                        if (cIndexSource >= index && cIndexSource < bestMatchSource && cIndexGlyph < bestMatchGlyph)
                                        {
                                            bestMatchFound = true;
                                            bestMatchSource = cIndexSource;
                                            bestMatchGlyph = cIndexGlyph;
                                        }
                                    }
                                    trueMatchFound = true;
                                }
                                continue;
                            }
                            if (index < cmd->SourceOffset && cmd->SourceOffset < bestMatchSource)
                            {
                                bestMatchFound = true;
                                bestMatchGlyph = cmd->GlyphOffset;
                                bestMatchSource = cmd->SourceOffset;
                            }
                        }
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            var cmd = (TextLayoutIconCommand*)input.Data;
                            if (index >= cmd->SourceOffset && index < cmd->SourceOffset + cmd->SourceLength)
                                trueMatchFound = true;

                            if (trueMatchFound || index < cmd->SourceOffset && cmd->SourceOffset < bestMatchSource)
                            {
                                bestMatchFound = true;
                                bestMatchGlyph = cmd->GlyphOffset;
                                bestMatchSource = cmd->SourceOffset;
                            }
                        }
                        break;

                    default:
                        ProcessStylingCommand(input, cmdType,
                            TextRendererStacks.Style | TextRendererStacks.Font | TextRendererStacks.Link, ref bold, ref italic, ref source);
                        break;
                }

                input.SeekNextCommand();
            }
            
            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return bestMatchFound ? bestMatchGlyph : input.TotalGlyphLength - 1;
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Vector2 position)
        {
            return GetInsertionPointAtPosition(input, (Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified insertion point.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Vector2 position, out Int32 lineAtPosition)
        {
            return GetInsertionPointAtPosition(input, (Int32)position.X, (Int32)position.Y, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Point2 position)
        {
            return GetInsertionPointAtPosition(input, position.X, position.Y);
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified insertion point.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Point2 position, out Int32 lineAtPosition)
        {
            return GetInsertionPointAtPosition(input, position.X, position.Y, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            Int32 lineAtPosition;
            return GetInsertionPointAtPosition(input, x, y, out lineAtPosition);
        }

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified insertion point.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32 lineAtPosition)
        {
            Contract.Require(input, nameof(input));

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            var lineAtPositionTemp = default(Int32?);
            var result = GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPositionTemp, InsertionPointSearchMode.BeforeOrAfterGlyph) ?? 0;

            lineAtPosition = lineAtPositionTemp ?? 0;

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return result;
        }

        /// <summary>
        /// Gets a layout-relative bounding box for the specified line.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the line for which to retrieve a bounding box.</param>
        /// <returns>A layout-relative bounding box for the specified line.</returns>
        public Rectangle GetLineBounds(TextLayoutCommandStream input, Int32 index)
        {
            Contract.Require(input, nameof(input));
            Contract.EnsureRange(index >= 0 && index < input.LineCount, nameof(index));

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var positionX = 0;
            var positionY = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;

            input.SeekNextLine();

            for (var i = 0; i < index; i++)
            {
                positionY += ((TextLayoutLineInfoCommand*)input.Data)->LineHeight;
                input.SeekNextLine();
            }

            var lineInfo = (TextLayoutLineInfoCommand*)input.Data;
            var lineWidth = lineInfo->LineWidth;
            var lineHeight = lineInfo->LineHeight;
            positionX = lineInfo->Offset;

            if (acquiredPointers)
                input.ReleasePointers();

            return new Rectangle(positionX, positionY, lineWidth, lineHeight);
        }

        /// <summary>
        /// Gets a layout-relative bounding box for the specified glyph.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <param name="spanLineHeight">A value indicating whether the returned bounds should span the height of the line.</param>
        /// <returns>A layout-relative bounding box for the specified glyph.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index, Boolean spanLineHeight = false)
        {
            LineInfo lineInfo;
            return GetGlyphBounds(input, index, out lineInfo, spanLineHeight);
        }

        /// <summary>
        /// Gets a layout-relative bounding box for the specified glyph.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <param name="lineInfo">A <see cref="LineInfo"/> structure which will be populated with metadata describing the line that contains the glyph.</param>
        /// <param name="spanLineHeight">A value indicating whether the returned bounds should span the height of the line.</param>
        /// <returns>A layout-relative bounding box for the specified glyph.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index, out LineInfo lineInfo, Boolean spanLineHeight = false)
        {
            Contract.Require(input, nameof(input));
            Contract.EnsureRange(index >= 0 && index < input.TotalGlyphLength, nameof(index));

            var shape = (input.Settings.Options & TextLayoutOptions.Shape) == TextLayoutOptions.Shape;

            var boundsFound = false;
            var bounds = Rectangle.Empty;

            var seekState = new TextSeekState();
            seekState.LineIndex = -1;

            var drawState = new TextDrawState();
            drawState.Source = shape ? input.GetShapedStringBuilder() : CreateSourceUnionFromSegmentOrigin(input.SourceText);
            drawState.Bold = (input.Settings.Style == UltravioletFontStyle.Bold || input.Settings.Style == UltravioletFontStyle.BoldItalic);
            drawState.Italic = (input.Settings.Style == UltravioletFontStyle.Italic || input.Settings.Style == UltravioletFontStyle.BoldItalic);
            drawState.Font = input.Settings.Font;
            drawState.FontFace = input.Settings.Font.GetFace(drawState.Bold, drawState.Italic);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            drawState.BlockOffset = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;

            input.SeekNextCommand();

            // NOTE: If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            if (!input.HasMultipleFontStyles)
                SkipToLineContainingGlyph(input, index, ref seekState);

            // Seek through the remaining commands until we find the one that contains our glyph.
            while (!boundsFound && input.StreamPositionInObjects < input.Count)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;
                var cmdBounds = default(Rectangle?);

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        GetGlyphBounds_LineInfo(input, ref seekState);
                        break;

                    case TextLayoutCommandType.Text:
                        cmdBounds = GetGlyphBounds_Text(input, index, spanLineHeight, ref drawState, ref seekState);
                        break;

                    case TextLayoutCommandType.Icon:
                        cmdBounds = GetGlyphBounds_Icon(input, index, spanLineHeight, ref drawState, ref seekState);
                        break;

                    case TextLayoutCommandType.LineBreak:
                        cmdBounds = GetGlyphBounds_LineBreak(input, index, ref drawState, ref seekState);
                        break;

                    default:
                        GetGlyphBounds_Default(input, cmdType, ref drawState, ref seekState);
                        break;
                }

                if (cmdBounds.HasValue)
                {
                    bounds = cmdBounds.GetValueOrDefault();
                    boundsFound = true;
                }
            }

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            lineInfo = new LineInfo(input, 
                seekState.LineIndex, seekState.LineStartInCommands, seekState.LineStartInSource, seekState.LineStartInGlyphs, 
                seekState.LineOffsetX, seekState.LineOffsetY, seekState.LineWidth, seekState.LineHeight, 
                seekState.LineLengthInCommands, seekState.LineLengthInSource, seekState.LineLengthInGlyphs, seekState.TerminatingLineBreakLength);

            return bounds;
        }

        /// <summary>
        /// Processes otherwise unhandled commands for GetGlyphBounds().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetGlyphBounds_Default(TextLayoutCommandStream input, TextLayoutCommandType cmdType, ref TextDrawState drawState, ref TextSeekState seekState)
        {
            var change = ProcessStylingCommand(input, cmdType, TextRendererStacks.Style | TextRendererStacks.Font, ref drawState.Bold, ref drawState.Italic, ref drawState.Source);
            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
            {
                RefreshFont(input.Settings.Font, drawState.Bold, drawState.Italic, out drawState.Font, out drawState.FontFace);
            }

            input.SeekNextCommand();
        }

        /// <summary>
        /// Processes LineInfo commands for GetGlyphBounds().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetGlyphBounds_LineInfo(TextLayoutCommandStream input, ref TextSeekState seekState)
        {
            ProcessLineInfo(input, ref seekState);
        }

        /// <summary>
        /// Processes Text commands for GetGlyphBounds().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Rectangle? GetGlyphBounds_Text(TextLayoutCommandStream input, Int32 glyphIndex, Boolean spanLineHeight, ref TextDrawState drawState, ref TextSeekState seekState)
        {
            var bounds = default(Rectangle?);
            var cmd = (TextLayoutTextCommand*)input.Data;

            if (seekState.NumberOfGlyphsSeen + cmd->GlyphLength > glyphIndex)
            {
                var glyphOffset = 0;
                var glyphSize = Size2.Zero;

                var shaped = (input.Settings.Options & TextLayoutOptions.Shape) == TextLayoutOptions.Shape;
                if (shaped)
                {
                    var text = drawState.Source.CreateShapedStringSegmentFromSameOrigin(cmd->ShapedOffset, cmd->ShapedLength);
                    var textIsRtl = input.Settings.Direction == TextDirection.RightToLeft;

                    var glyphSourceIndex = glyphIndex - seekState.NumberOfGlyphsSeen;
                    glyphOffset = (glyphSourceIndex == 0) ? 0 : drawState.FontFace.MeasureShapedString(ref text, 0, glyphSourceIndex, textIsRtl).Width;
                    glyphSize = drawState.FontFace.MeasureShapedGlyph(ref text, glyphSourceIndex, textIsRtl);
                }
                else
                {
                    var text = drawState.Source.CreateStringSegmentFromSameOrigin(cmd->SourceOffset, cmd->SourceLength);

                    var glyphSourceIndex = glyphIndex - seekState.NumberOfGlyphsSeen;
                    if (glyphSourceIndex > 0 && Char.IsSurrogatePair(text[glyphSourceIndex - 1], text[glyphSourceIndex]))
                        glyphSourceIndex--;

                    glyphOffset = (glyphSourceIndex == 0) ? 0 : drawState.FontFace.MeasureString(ref text, 0, glyphSourceIndex).Width;
                    glyphSize = drawState.FontFace.MeasureGlyph(ref text, glyphSourceIndex);
                }

                var glyphRelX = (input.Settings.Direction == TextDirection.RightToLeft) ?
                    seekState.LineOffsetX + (cmd->TextWidth - (glyphOffset + glyphSize.Width)) :
                    seekState.LineOffsetX + glyphOffset;
                var glyphRelY = drawState.BlockOffset;
                var glyphPosition = cmd->GetAbsolutePosition(drawState.FontFace, glyphRelX, glyphRelY, seekState.LineWidth, seekState.LineHeight, input.Settings.Direction);
                if (spanLineHeight)
                {
                    glyphPosition.Y = cmd->Bounds.Y;
                }

                bounds = new Rectangle(glyphPosition, spanLineHeight ? new Size2(glyphSize.Width, seekState.LineHeight) : glyphSize);
            }

            seekState.NumberOfSourceCharactersSeen += cmd->SourceLength;
            seekState.NumberOfShapedCharactersSeen += cmd->ShapedLength;
            seekState.NumberOfGlyphsSeen += cmd->GlyphLength;

            input.SeekNextCommand();

            return bounds;
        }

        /// <summary>
        /// Processes Icon commands for GetGlyphBounds();
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Rectangle? GetGlyphBounds_Icon(TextLayoutCommandStream input, Int32 glyphIndex, Boolean spanLineHeight, ref TextDrawState drawState, ref TextSeekState seekState)
        {
            var bounds = default(Rectangle?);
            var cmd = (TextLayoutIconCommand*)input.Data;

            if (seekState.NumberOfGlyphsSeen + 1 > glyphIndex)
            {
                var glyphSize = new Size2(cmd->IconWidth, cmd->IconHeight);

                var glyphRelX = seekState.LineOffsetX;
                var glyphRelY = drawState.BlockOffset;
                var glyphPosition = cmd->GetAbsolutePosition(glyphRelX, glyphRelY, seekState.LineWidth, seekState.LineHeight, input.Settings.Direction);
                if (spanLineHeight)
                {
                    glyphPosition.Y = cmd->Bounds.Y;
                }

                bounds = new Rectangle(glyphPosition, spanLineHeight ? new Size2(glyphSize.Width, seekState.LineHeight) : glyphSize);
            }

            seekState.NumberOfSourceCharactersSeen += cmd->SourceLength;
            seekState.NumberOfGlyphsSeen += 1;

            input.SeekNextCommand();

            return bounds;
        }

        /// <summary>
        /// Processes LineBreak commands for GetGlyphBounds().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Rectangle? GetGlyphBounds_LineBreak(TextLayoutCommandStream input, Int32 glyphIndex, ref TextDrawState drawState, ref TextSeekState seekState)
        {
            var bounds = default(Rectangle?);
            var cmd = (TextLayoutLineBreakCommand*)input.Data;

            if (seekState.NumberOfGlyphsSeen + cmd->SourceLength > glyphIndex)
            {
                var glyphX = (input.Settings.Direction == TextDirection.RightToLeft) ?
                    Math.Max(0, seekState.LineOffsetX - 1) :
                    Math.Max(0, seekState.LineOffsetX + seekState.LineWidth - 1);
                var glyphY = drawState.BlockOffset + seekState.LineOffsetY;

                bounds = new Rectangle(glyphX, glyphY, 0, seekState.LineHeight);
            }

            seekState.NumberOfSourceCharactersSeen += cmd->SourceLength;
            seekState.NumberOfGlyphsSeen += cmd->GlyphLength;

            input.SeekNextCommand();

            return bounds;
        }

        /// <summary>
        /// Gets a layout-relative bounding box for the specified insertion point.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the insertion point for which to retrieve a bounding box.</param>
        /// <returns>A layout-relative bounding box for the specified glyph.</returns>
        public Rectangle GetInsertionPointBounds(TextLayoutCommandStream input, Int32 index)
        {
            var lineInfo = default(LineInfo);
            var glyphBounds = default(Rectangle?);
            return GetInsertionPointBounds(input, index, out lineInfo, out glyphBounds);
        }

        /// <summary>
        /// Gets a layout-relative bounding box for the specified insertion point.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the insertion point for which to retrieve a bounding box.</param>
        /// <param name="lineInfo">A <see cref="LineInfo"/> structure which will be populated with metadata describing the line that contains the insertion point.</param>
        /// <param name="glyphBounds">The bounding box of the glyph that comes after the insertion point, or <see langword="null"/> if there is no such glyph.</param>
        /// <returns>A layout-relative bounding box for the specified glyph.</returns>
        public Rectangle GetInsertionPointBounds(TextLayoutCommandStream input, Int32 index, out LineInfo lineInfo, out Rectangle? glyphBounds)
        {
            Contract.Require(input, nameof(input));

            if (input.TotalGlyphLength == index)
            {
                var lineDefaultHeight = (input.Settings.Font == null) ? 0 :
                    input.Settings.Font.GetFace(UltravioletFontStyle.Regular).LineSpacing;

                lineInfo = (input.TotalGlyphLength > 0) ? input.GetLineInfo(input.LineCount - 1) :
                    new LineInfo(input, 0, 0, 0, 0, 0, 0, 0, lineDefaultHeight, 0, 0, 0, 0);

                glyphBounds = null;
                return new Rectangle((input.Settings.Direction == TextDirection.RightToLeft) ? lineInfo.X : lineInfo.X + lineInfo.Width, 
                    lineInfo.Y, 0, lineInfo.Height);
            }
            else
            {
                var glyphBoundsValue = GetGlyphBounds(input, index, out lineInfo, true);

                glyphBounds = glyphBoundsValue;
                return new Rectangle((input.Settings.Direction == TextDirection.RightToLeft) ? glyphBoundsValue.Right : glyphBoundsValue.Left,
                    glyphBoundsValue.Top, 0, glyphBoundsValue.Height);
            }
        }

        /// <summary>
        /// Removes the registered text shaper.
        /// </summary>
        public void ClearTextShaper()
        {
            layoutEngine.ClearTextShaper();
        }

        /// <summary>
        /// Registers a text shaper.
        /// </summary>
        /// <param name="shaper">The text shaper to register.</param>
        public void RegisterTextShaper(TextShaper shaper)
        {
            layoutEngine.RegisterTextShaper(shaper);
        }

        /// <summary>
        /// Unregisters the text shaper.
        /// </summary>
        /// <returns><see langword="true"/> if the text shaper was unregistered; otherwise <see langword="false"/>.</returns>
        public Boolean UnregisterTextShaper()
        {
            return layoutEngine.UnregisterTextShaper();
        }

        /// <summary>
        /// Removes all registered styles.
        /// </summary>
        public void ClearStyles()
        {
            layoutEngine.ClearStyles();
        }

        /// <summary>
        /// Registers a style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to register.</param>
        /// <param name="style">The style to register.</param>
        public void RegisterStyle(String name, TextStyle style)
        {
            layoutEngine.RegisterStyle(name, style);
        }

        /// <summary>
        /// Unregisters the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to unregister.</param>
        /// <returns><see langword="true"/> if the style was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterStyle(String name)
        {
            return layoutEngine.UnregisterStyle(name);
        }

        /// <summary>
        /// Removes all registered fonts.
        /// </summary>
        public void ClearFonts()
        {
            layoutEngine.ClearFonts();
        }

        /// <summary>
        /// Registers the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register.</param>
        public void RegisterFont(String name, UltravioletFont font)
        {
            layoutEngine.RegisterFont(name, font);
        }

        /// <summary>
        /// Unregisters the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to unregister.</param>
        /// <returns><see langword="true"/> if the font was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterFont(String name)
        {
            return layoutEngine.UnregisterFont(name);
        }

        /// <summary>
        /// Removes all registered fallback fonts.
        /// </summary>
        public void ClearFallbackFonts()
        {
            layoutEngine.ClearFallbackFonts();
        }

        /// <summary>
        /// Registers a fallback font with the layout engine.
        /// </summary>
        /// <param name="name">The name of the fallback font to register.</param>
        /// <param name="start">The first UTF-32 Unicode code point, inclusive, in the range for which this font should be employed.</param>
        /// <param name="end">The last UTF32 Unicode code point, inclusive, in the range for which this font should be employed.</param>
        /// <param name="font">The name of the font to register as a fallback for the specified range.</param>
        public void RegisterFallbackFont(String name, Int32 start, Int32 end, String font)
        {
            layoutEngine.RegisterFallbackFont(name, start, end, font);
        }

        /// <summary>
        /// Removes all registered icons.
        /// </summary>
        public void ClearIcons()
        {
            layoutEngine.ClearIcons();
        }

        /// <summary>
        /// Registers the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to register.</param>
        /// <param name="icon">The icon to register.</param>
        /// <param name="height">The width to which to scale the icon, or null to preserve the sprite's original width.</param>
        /// <param name="width">The height to which to scale the icon, or null to preserve the sprite's original height.</param>
        public void RegisterIcon(String name, SpriteAnimation icon, Int32? width = null, Int32? height = null)
        {
            layoutEngine.RegisterIcon(name, icon, width, height);
        }

        /// <summary>
        /// Unregisters the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to unregister.</param>
        /// <returns><see langword="true"/> if the icon was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterIcon(String name)
        {
            return layoutEngine.UnregisterIcon(name);
        }

        /// <summary>
        /// Removes all registered glyph shaders.
        /// </summary>
        public void ClearGlyphShaders()
        {
            layoutEngine.ClearGlyphShaders();
        }

        /// <summary>
        /// Registers the glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to register.</param>
        /// <param name="shader">The glyph shader to register.</param>
        public void RegisterGlyphShader(String name, GlyphShader shader)
        {
            layoutEngine.RegisterGlyphShader(name, shader);
        }

        /// <summary>
        /// Unregisters the glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to unregister.</param>
        /// <returns><see langword="true"/> if the glyph shader was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterGlyphShader(String name)
        {
            return layoutEngine.UnregisterGlyphShader(name);
        }

        /// <summary>
        /// Lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        public void Parse(String input, TextParserTokenStream output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.Parse(input, output, options);
        }

        /// <summary>
        /// Incrementally lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to parse.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-parsed by this operation.</remarks>
        public void ParseIncremental(String input, Int32 start, Int32 count, TextParserTokenStream output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.ParseIncremental(input, start, count, output, options);
        }

        /// <summary>
        /// Lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringBuilder"/> to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        public void Parse(StringBuilder input, TextParserTokenStream output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.Parse(input, output, options);
        }

        /// <summary>
        /// Incrementally lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringBuilder"/> to parse.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-parsed by this operation.</remarks>
        public void ParseIncremental(StringBuilder input, Int32 start, Int32 count, TextParserTokenStream output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.ParseIncremental(input, start, count, output, options);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The string of text to lay out.</param>
        /// <param name="output">The command stream representing the formatted text.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(String input, TextLayoutCommandStream output, TextLayoutSettings settings)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, output, settings);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The string of text to lay out.</param>
        /// <param name="output">The command stream representing the formatted text.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(StringBuilder input, TextLayoutCommandStream output, TextLayoutSettings settings)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, output, settings);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The parsed text to lay out.</param>
        /// <param name="output">The command stream representing the formatted text.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(TextParserTokenStream input, TextLayoutCommandStream output, TextLayoutSettings settings)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            layoutEngine.CalculateLayout(input, output, settings);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, String input, Vector2 position, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="parserOptions">The parser options to use when parsing the input text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, String input, Vector2 position, Color defaultColor, Int32 start, Int32 count, TextParserOptions parserOptions, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            parser.Parse(input, parserResult, parserOptions);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 position, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="parserOptions">The parser options to use when parsing the input text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 position, Color defaultColor, Int32 start, Int32 count, TextParserOptions parserOptions, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            parser.Parse(input, parserResult, parserOptions);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The collection of parser tokens which will be laid out and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextParserTokenStream input, Vector2 position, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            layoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The collection of parser tokens which will be laid out and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextParserTokenStream input, Vector2 position, Color defaultColor, Int32 start, Int32 count, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            layoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The text layout command stream that describes the text to draw.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Color defaultColor)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            return DrawInternal(spriteBatch, input, position, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The text layout command stream that describes the text to draw.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Color defaultColor, Int32 start, Int32 count)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            return DrawInternal(spriteBatch, input, position, defaultColor, start, count);
        }

        /// <summary>
        /// Gets or sets the delegate which determines the color with which to draw the renderer's links.
        /// </summary>
        public LinkColorizer LinkColorizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the delegate which evaluates which links have been previously visited.
        /// </summary>
        public LinkStateEvaluator LinkStateEvaluator
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the delegate which executes links.
        /// </summary>
        public LinkClickHandler LinkClickHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="StringSourceUnion"/> from the originating string for the specified string segment.
        /// </summary>
        private static StringSourceUnion CreateSourceUnionFromSegmentOrigin(StringSegment segment)
        {
            if (segment.IsBackedByString)
                return (StringSource)segment.SourceString;

            if (segment.IsBackedByStringBuilder)
                return (StringBuilderSource)segment.SourceStringBuilder;

            return new StringSourceUnion();
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        private RectangleF DrawInternal(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Color defaultColor, Int32 start, Int32 count)
        {
            if (input.Settings.Font == null)
                throw new ArgumentException(UltravioletStrings.InvalidLayoutSettings);

            var settings = input.Settings;
            var bold = (settings.Style == UltravioletFontStyle.Bold || settings.Style == UltravioletFontStyle.BoldItalic);
            var italic = (settings.Style == UltravioletFontStyle.Italic || settings.Style == UltravioletFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);
            var color = defaultColor;
            var lastColorOutsideLink = defaultColor;
            var direction = settings.Direction;

            var availableHeight = settings.Height ?? Int32.MaxValue;
            var blockOffset = 0;
            var seekState = new TextSeekState { LineIndex = -1 };

            var glyphsSeen = 0;
            var glyphsMax = (count == Int32.MaxValue) ? Int32.MaxValue : start + count - 1;

            var source = (StringSourceUnion)(StringSegmentSource)input.SourceText;

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();
            
            var linkAtCursor = GetLinkIndexAtCursor(input);

            input.Seek(0);

            while (input.StreamPositionInObjects < input.Count)
            {
                if (glyphsSeen > glyphsMax)
                    break;

                var cmdType = *(TextLayoutCommandType*)input.Data;
                switch (cmdType)
                {
                    case TextLayoutCommandType.BlockInfo:
                        ProcessBlockInfo(input, out blockOffset);
                        break;

                    case TextLayoutCommandType.LineInfo:
                        {
                            ProcessLineInfo(input, ref seekState);
                            if (blockOffset + seekState.LineOffsetY + seekState.LineHeight > availableHeight)
                            {
                                input.SeekEnd();
                            }
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        DrawText(spriteBatch, input, fontFace, ref source,
                            position.X + seekState.LineOffsetX, position.Y + blockOffset, seekState.LineWidth, seekState.LineHeight, start, glyphsMax, color, direction, ref glyphsSeen);
                        break;

                    case TextLayoutCommandType.Icon:
                        DrawIcon(spriteBatch, input, 
                            position.X + seekState.LineOffsetX, position.Y + blockOffset, seekState.LineWidth, seekState.LineHeight, start, count, lastColorOutsideLink, direction, ref glyphsSeen);
                        break;
                        
                    case TextLayoutCommandType.LineBreak:
                        {
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            glyphsSeen += cmd->GlyphLength;
                        }
                        input.SeekNextCommand();
                        break;

                    default:
                        {
                            var change = ProcessStylingCommand(input, cmdType, TextRendererStacks.All, ref bold, ref italic, ref source);
                            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
                            {
                                RefreshFont(input.Settings.Font, bold, italic, out font, out fontFace);
                            }
                            if ((change & TextRendererStateChange.ChangeColor) == TextRendererStateChange.ChangeColor ||
                                (change & TextRendererStateChange.ChangeLink) == TextRendererStateChange.ChangeLink)
                            {
                                var linkIndex = linkStack.Count > 0 ? linkStack.Peek() : (Int16?)null;
                                RefreshColor(input, defaultColor, linkIndex, linkAtCursor, ref color);

                                if (linkStack.Count == 0)
                                    lastColorOutsideLink = color;
                            }
                        }
                        input.SeekNextCommand();
                        break;
                }
            }

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return new RectangleF(position.X + input.Bounds.X, position.Y + input.Bounds.Y, input.Bounds.Width, input.Bounds.Height);
        }

        /// <summary>
        /// Advances the input stream past a text command after it has been drawn.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AdvancePastTextCommand(TextLayoutCommandStream input, Int32 length, ref Int32 glyphsSeen)
        {
            glyphsSeen += length;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Gets a value indicating whether a text command is visible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetIsTextVisible(Int32 length, Int32 start, Int32 end, Int32 glyphsSeen)
        {
            var tokenStart = glyphsSeen;
            var tokenEnd = tokenStart + length - 1;

            return (start < glyphsSeen + length);
        }

        /// <summary>
        /// Gets a value indicating whether a text command is only partially visible due to substring rendering.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetIsTextPartiallyVisible(Int32 length, Int32 start, Int32 end, Int32 glyphsSeen, out Int32 subStart, out Int32 subLength, out Boolean wasDrawnToCompletion)
        {
            var tokenStart = glyphsSeen;
            var tokenEnd = tokenStart + length - 1;
            var isPartiallyVisible = ((tokenStart < start && tokenEnd >= start) || (tokenStart <= end && tokenEnd > end));
            if (isPartiallyVisible)
            {
                wasDrawnToCompletion = false;
                subStart = (glyphsSeen > start) ? 0 : start - glyphsSeen;
                subLength = 1 + ((Math.Min(end, glyphsSeen + length - 1) - glyphsSeen) - subStart);
                return true;
            }
            else
            {
                wasDrawnToCompletion = true;
                subStart = 0;
                subLength = 0;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a text command is split by a hyphen.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetIsTextSplitByHyphen(TextLayoutCommandStream input, UltravioletFontFace font, Boolean wasDrawnToCompletion, out Int32 hyphenGlyphIndex, out Boolean hyphenIsVisible)
        {
            var isSplitByHyphen = (input.StreamPositionInObjects < input.Count && *(TextLayoutCommandType*)input.Data == TextLayoutCommandType.Hyphen);
            if (isSplitByHyphen)
            {
                hyphenGlyphIndex = font.SupportsGlyphIndices ? font.GetGlyphIndex('-') : 0;
                hyphenIsVisible = wasDrawnToCompletion;
                return true;
            }
            else
            {
                hyphenGlyphIndex = 0;
                hyphenIsVisible = false;
                return false;
            }
        }

        /// <summary>
        /// Adjusts the position of a command to account for hyphenation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 AdjustCommandPositionForHyphen(TextDirection direction, Vector2 position, Int32 hyphenWidth, Int32 hyphenatedTextWidth, Size2 hyphenatedTextKerning)
        {
            var cmdX = (direction == TextDirection.RightToLeft) ?
               (position.X - (hyphenWidth - hyphenatedTextKerning.Width)) :
               (position.X + (hyphenatedTextWidth + hyphenatedTextKerning.Width));
            var cmdY = position.Y + hyphenatedTextKerning.Height;
            return new Vector2(cmdX, cmdY);
        }

        /// <summary>
        /// Draws a text command.
        /// </summary>
        private void DrawText(SpriteBatch spriteBatch, TextLayoutCommandStream input, UltravioletFontFace fontFace, ref StringSourceUnion source,
            Single x, Single y, Int32 lineWidth, Int32 lineHeight, Int32 start, Int32 end, Color color, TextDirection direction, ref Int32 glyphsSeen)
        {
            var shaped = source.IsShaped;
            if (shaped)
            {
                var cmd = (TextLayoutTextCommand*)input.Data;
                var cmdText = (cmd->SourceLength == 0) ? ShapedStringSegment.Empty : source.CreateShapedStringSegmentFromSubstring(cmd->ShapedOffset, cmd->ShapedLength);
                if (cmdText.Length == 1 && cmdText[0].GetSpecialCharacter() == '\n')
                {
                    glyphsSeen += cmd->GlyphLength;
                    input.SeekNextCommand();
                    return;
                }
                DrawShapedTextCore(spriteBatch, input, fontFace, ref cmdText, x, y, lineWidth, lineHeight, start, end, color, direction, ref glyphsSeen);
            }
            else
            {
                var cmd = (TextLayoutTextCommand*)input.Data;
                var cmdText = (cmd->SourceLength == 0) ? StringSegment.Empty : source.CreateStringSegmentFromSubstring(cmd->SourceOffset, cmd->SourceLength);
                if (cmdText.Equals("\n"))
                {
                    glyphsSeen += cmd->GlyphLength;
                    input.SeekNextCommand();
                    return;
                }
                DrawTextCore(spriteBatch, input, fontFace, ref cmdText, x, y, lineWidth, lineHeight, start, end, color, direction, ref glyphsSeen);
            }
        }

        /// <summary>
        /// Draws a text command.
        /// </summary>
        private void DrawTextCore(SpriteBatch spriteBatch, TextLayoutCommandStream input, UltravioletFontFace fontFace, ref StringSegment cmdText,
            Single x, Single y, Int32 lineWidth, Int32 lineHeight, Int32 start, Int32 end, Color color, TextDirection direction, ref Int32 glyphsSeen)
        {
            var cmd = (TextLayoutTextCommand*)input.Data;
            var cmdFullLength = cmd->GlyphLength;
            var cmdPosition = Vector2.Zero;
            var cmdWasDrawnCompletely = true;

            if (GetIsTextVisible(cmdFullLength, start, end, glyphsSeen))
            {
                var cmdOffset = 0;
                if (GetIsTextPartiallyVisible(cmdFullLength, start, end, glyphsSeen, out var subStart, out var subLength, out cmdWasDrawnCompletely))
                {
                    cmdOffset = (subStart == 0) ? 0 : fontFace.MeasureString(ref cmdText, 0, subStart).Width;
                    cmdText = cmdText.Substring(subStart, subLength);
                }

                cmdPosition = cmd->GetAbsolutePositionVector(fontFace, x + cmdOffset, y, lineWidth, lineHeight, direction);
                
                var effects = (direction == TextDirection.RightToLeft) ? SpriteEffects.DrawTextReversed : SpriteEffects.None;
                var gscontext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, glyphsSeen, input.TotalGlyphLength);
                spriteBatch.DrawString(gscontext, fontFace, cmdText, cmdPosition, color, 0f, 
                    Vector2.Zero, Vector2.One, effects, 0f, default(SpriteBatchData));
            }

            AdvancePastTextCommand(input, cmdFullLength, ref glyphsSeen);

            if (GetIsTextSplitByHyphen(input, fontFace, cmdWasDrawnCompletely, out var hyphenGlyphIndex, out var hyphenIsVisible))
            {
                if (hyphenIsVisible)
                {
                    var hyphenatedTextWidth = fontFace.MeasureString(ref cmdText).Width;
                    var hyphenatedTextKerning = fontFace.GetHypotheticalKerningInfo(ref cmdText, cmdText.Length - 1, '-');
                    var hyphenWidth = fontFace.MeasureString("-").Width;

                    cmdPosition = AdjustCommandPositionForHyphen(direction, cmdPosition, hyphenWidth, hyphenatedTextWidth, hyphenatedTextKerning);

                    var gscontext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, glyphsSeen - 1, input.TotalGlyphLength);
                    spriteBatch.DrawString(gscontext, fontFace, "-", cmdPosition, color);
                }
                input.SeekNextCommand();
            }
        }

        /// <summary>
        /// Draws a text command.
        /// </summary>
        private void DrawShapedTextCore(SpriteBatch spriteBatch, TextLayoutCommandStream input, UltravioletFontFace fontFace, ref ShapedStringSegment cmdText,
            Single x, Single y, Int32 lineWidth, Int32 lineHeight, Int32 start, Int32 end, Color color, TextDirection direction, ref Int32 glyphsSeen)
        {
            var cmd = (TextLayoutTextCommand*)input.Data;
            var cmdFullLength = cmdText.Length;
            var cmdPosition = Vector2.Zero;
            var cmdWasDrawnCompletely = true;

            if (GetIsTextVisible(cmdText.Length, start, end, glyphsSeen))
            {
                var cmdOffset = 0;
                if (GetIsTextPartiallyVisible(cmdText.Length, start, end, glyphsSeen, out var subStart, out var subLength, out cmdWasDrawnCompletely))
                {
                    cmdOffset = (subStart == 0) ? 0 : fontFace.MeasureShapedString(ref cmdText, 0, subStart).Width;
                    cmdText = cmdText.Substring(subStart, subLength);
                }

                cmdPosition = cmd->GetAbsolutePositionVector(fontFace, x + cmdOffset, y, lineWidth, lineHeight, direction);

                var effects = (direction == TextDirection.RightToLeft) ? SpriteEffects.DrawTextReversed : SpriteEffects.None;
                var gscontext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, glyphsSeen, input.TotalGlyphLength);
                spriteBatch.DrawShapedString(gscontext, fontFace, cmdText, cmdPosition, color, 0f, 
                    Vector2.Zero, Vector2.One, effects, 0f, default(SpriteBatchData));
            }

            AdvancePastTextCommand(input, cmd->GlyphLength, ref glyphsSeen);

            if (GetIsTextSplitByHyphen(input, fontFace, cmdWasDrawnCompletely, out var hyphenGlyphIndex, out var hyphenIsVisible))
            {
                if (hyphenIsVisible)
                {
                    var hyphenatedTextWidth = fontFace.MeasureShapedString(ref cmdText).Width;
                    var hyphenatedTextKerning = fontFace.GetHypotheticalShapedKerningInfo(ref cmdText, cmdText.Length - 1, hyphenGlyphIndex);
                    var hyphenWidth = fontFace.MeasureString("-").Width;

                    cmdPosition = AdjustCommandPositionForHyphen(direction, cmdPosition, hyphenWidth, hyphenatedTextWidth, hyphenatedTextKerning);

                    var gscontext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, glyphsSeen - 1, input.TotalGlyphLength);
                    spriteBatch.DrawString(gscontext, fontFace, "-", cmdPosition, color);
                }

                input.SeekNextCommand();
            }
        }

        /// <summary>
        /// Draws an icon command.
        /// </summary>
        private void DrawIcon(SpriteBatch spriteBatch, TextLayoutCommandStream input,
            Single x, Single y, Int32 lineWidth, Int32 lineHeight, Int32 start, Int32 end, Color color, TextDirection direction, ref Int32 glyphsSeen)
        {
            var cmd = (TextLayoutIconCommand*)input.Data;

            var isIconVisible = (start < glyphsSeen + 1);
            if (isIconVisible)
            {
                var icon = input.GetIcon(cmd->IconIndex);
                var iconWidth = (Single)(icon.Width ?? icon.Icon.Controller.Width);
                var iconHeight = (Single)(icon.Height ?? icon.Icon.Controller.Height);
                var iconPosition = cmd->GetAbsolutePositionVector(x, y, lineWidth, lineHeight, direction);
                var iconRotation = 0f;

                var cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, glyphsSeen, input.TotalGlyphLength);
                if (cmdGlyphShaderContext.IsValid)
                {
                    var glyphData = new GlyphData();
                    glyphData.UnicodeCodePoint = '\x0000';
                    glyphData.Pass = 0;
                    glyphData.X = iconPosition.X;
                    glyphData.Y = iconPosition.Y;
                    glyphData.ScaleX = 1.0f;
                    glyphData.ScaleY = 1.0f;
                    glyphData.Color = color;
                    glyphData.ClearDirtyFlags();

                    cmdGlyphShaderContext.Execute(ref glyphData, glyphsSeen);

                    if (glyphData.DirtyPosition)
                        iconPosition = new Vector2(glyphData.X, glyphData.Y);

                    if (glyphData.DirtyScale)
                    {
                        iconWidth *= glyphData.ScaleX;
                        iconHeight *= glyphData.ScaleY;
                    }

                    if (glyphData.DirtyColor)
                        color = glyphData.Color;
                }

                var iconSprite = icon.Icon;
                var iconController = iconSprite.Controller;
                var iconFrame = iconController.GetFrame();

                spriteBatch.DrawSprite(iconController, iconPosition + iconFrame.Origin, iconWidth, iconHeight, color, iconRotation);
            }

            glyphsSeen += cmd->GlyphLength;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Clears all of the renderer's layout parameter stacks.
        /// </summary>
        private void ClearLayoutStacks()
        {
            styleStack.Clear();
            fontStack.Clear();
            colorStack.Clear();
            glyphShaderStack.Clear();
            linkStack.Clear();
        }

        /// <summary>
        /// Pushes a value onto a style-scoped stack.
        /// </summary>
        private void PushScopedStack<T>(Stack<TextStyleScoped<T>> stack, T value)
        {
            var scope = styleStack.Count;
            stack.Push(new TextStyleScoped<T>(value, scope));
        }

        /// <summary>
        /// Pushes a style onto the style stack.
        /// </summary>
        private void PushStyle(TextStyle style, ref Boolean bold, ref Boolean italic)
        {
            var instance = new TextStyleInstance(style, bold, italic);
            styleStack.Push(instance);

            if (style.Font != null)
                PushFont(style.Font);

            if (style.Color.HasValue)
                PushColor(style.Color.Value);

            if (style.GlyphShaders.Count > 0)
            {
                foreach (var glyphShader in style.GlyphShaders)
                    PushGlyphShader(glyphShader);
            }

            if (style.Bold.HasValue)
                bold = style.Bold.Value;

            if (style.Italic.HasValue)
                italic = style.Italic.Value;
        }

        /// <summary>
        /// Pushes a font onto the font stack.
        /// </summary>
        private void PushFont(UltravioletFont font)
        {
            PushScopedStack(fontStack, font);
        }

        /// <summary>
        /// Pushes a color onto the color stack.
        /// </summary>
        private void PushColor(Color color)
        {
            PushScopedStack(colorStack, color);
        }

        /// <summary>
        /// Pushes a glyph shader onto the glyph shader stack.
        /// </summary>
        private void PushGlyphShader(GlyphShader glyphShader)
        {
            PushScopedStack(glyphShaderStack, glyphShader);
        }

        /// <summary>
        /// Pushes a link onto the link stack.
        /// </summary>
        private void PushLink(Int16 linkIndex)
        {
            linkStack.Push(linkIndex);
        }

        /// <summary>
        /// Pops a value off of a style-scoped stack.
        /// </summary>
        private void PopScopedStack<T>(Stack<TextStyleScoped<T>> stack)
        {
            if (stack.Count == 0)
                return;

            var scope = styleStack.Count;
            if (stack.Peek().Scope != scope)
                return;

            stack.Pop();
        }

        /// <summary>
        /// Pops a style off of the style stack.
        /// </summary>
        private void PopStyle(ref Boolean bold, ref Boolean italic)
        {
            if (styleStack.Count > 0)
            {
                PopStyleScope();

                var instance = styleStack.Pop();
                bold = instance.Bold;
                italic = instance.Italic;
            }
        }

        /// <summary>
        /// Pops a font off of the font stack.
        /// </summary>
        private void PopFont()
        {
            PopScopedStack(fontStack);
        }

        /// <summary>
        /// Pops a color off of the color stack.
        /// </summary>
        private void PopColor()
        {
            PopScopedStack(colorStack);
        }

        /// <summary>
        /// Pops a glyph shader off of the glyph shader stack.
        /// </summary>
        private void PopGlyphShader()
        {
            PopScopedStack(glyphShaderStack);
        }

        /// <summary>
        /// Pops a link off of the link stack.
        /// </summary>
        private void PopLink()
        {
            if (linkStack.Count == 0)
                return;

            linkStack.Pop();
        }

        /// <summary>
        /// Pops the current style scope off of the stacks.
        /// </summary>
        private void PopStyleScope()
        {
            var scope = styleStack.Count;

            while (fontStack.Count > 0 && fontStack.Peek().Scope == scope)
                fontStack.Pop();

            while (colorStack.Count > 0 && colorStack.Peek().Scope == scope)
                colorStack.Pop();

            while (glyphShaderStack.Count > 0 && glyphShaderStack.Peek().Scope == scope)
                glyphShaderStack.Pop();
        }

        /// <summary>
        /// Updates the current font by examining the state of the layout stacks.
        /// </summary>
        private void RefreshFont(UltravioletFont baseFont, Boolean bold, Boolean italic, out UltravioletFont font, out UltravioletFontFace fontFace)
        {
            font = (fontStack.Count == 0) ? baseFont : fontStack.Peek().Value;
            fontFace = font.GetFace(bold, italic);
        }

        /// <summary>
        /// Updates the current text color by examining the state of the layout stacks.
        /// </summary>
        private void RefreshColor(TextLayoutCommandStream input, Color defaultColor, Int16? linkIndex, Int16? linkAtCursor, ref Color color)
        {
            if (linkIndex.HasValue)
            {
                var target = input.GetLinkTarget(linkIndex.Value);
                var visited = LinkStateEvaluator?.Invoke(target) ?? false;
                var hovering = linkIndex == linkAtCursor;
                var active = input.ActiveLinkIndex == linkIndex;

                if (LinkColorizer != null)
                {
                    color = LinkColorizer(target, visited, hovering, active, color);
                }
                else
                {
                    if (active)
                        color = new Color(0xEE, 0x00, 0x00, 0xFF);
                    else
                    {
                        if (visited)
                            color = new Color(0x55, 0x1A, 0x8B, 0xFF);
                        else
                            color = new Color(0x00, 0x00, 0xEE, 0xFF);
                    }
                }
            }
            else
            {
                color = (colorStack.Count == 0) ? defaultColor : colorStack.Peek().Value;
            }
        }

        /// <summary>
        /// Processes a styling command and returns a value specifying which, if any, styling parameters were changed as a result.
        /// </summary>
        private TextRendererStateChange ProcessStylingCommand(TextLayoutCommandStream input, TextLayoutCommandType type, TextRendererStacks stacks,
            ref Boolean bold, ref Boolean italic, ref StringSourceUnion source)
        {
            switch (type)
            {
                case TextLayoutCommandType.ToggleBold:
                    bold = !bold;
                    return TextRendererStateChange.ChangeFont;

                case TextLayoutCommandType.ToggleItalic:
                    italic = !italic;
                    return TextRendererStateChange.ChangeFont;

                case TextLayoutCommandType.PushStyle:
                    if ((stacks & TextRendererStacks.Style) == TextRendererStacks.Style)
                    {
                        var cmd = (TextLayoutStyleCommand*)input.Data;
                        PushStyle(input.GetStyle(cmd->StyleIndex), ref bold, ref italic);
                        return TextRendererStateChange.ChangeFont | TextRendererStateChange.ChangeColor | TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushFont:
                    if ((stacks & TextRendererStacks.Font) == TextRendererStacks.Font)
                    {
                        var cmd = (TextLayoutFontCommand*)input.Data;
                        PushFont(input.GetFont(cmd->FontIndex));
                        return TextRendererStateChange.ChangeFont;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushColor:
                    if ((stacks & TextRendererStacks.Color) == TextRendererStacks.Color)
                    {
                        var cmd = (TextLayoutColorCommand*)input.Data;
                        PushColor(cmd->Color);
                        return TextRendererStateChange.ChangeColor;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushGlyphShader:
                    if ((stacks & TextRendererStacks.GlyphShader) == TextRendererStacks.GlyphShader)
                    {
                        var cmd = (TextLayoutGlyphShaderCommand*)input.Data;
                        PushGlyphShader(input.GetGlyphShader(cmd->GlyphShaderIndex));
                        return TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushLink:
                    if ((stacks & TextRendererStacks.Link) == TextRendererStacks.Link)
                    {
                        var cmd = (TextLayoutLinkCommand*)input.Data;
                        PushLink(cmd->LinkTargetIndex);
                        return TextRendererStateChange.ChangeLink;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopStyle:
                    if ((stacks & TextRendererStacks.Style) == TextRendererStacks.Style)
                    {
                        PopStyle(ref bold, ref italic);
                        return TextRendererStateChange.ChangeFont | TextRendererStateChange.ChangeColor | TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopFont:
                    if ((stacks & TextRendererStacks.Font) == TextRendererStacks.Font)
                    {
                        PopFont();
                        return TextRendererStateChange.ChangeFont;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopColor:
                    if ((stacks & TextRendererStacks.Color) == TextRendererStacks.Color)
                    {
                        PopColor();
                        return TextRendererStateChange.ChangeColor;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopGlyphShader:
                    if ((stacks & TextRendererStacks.GlyphShader) == TextRendererStacks.GlyphShader)
                    {
                        PopGlyphShader();
                        return TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopLink:
                    if ((stacks & TextRendererStacks.Link) == TextRendererStacks.Link)
                    {
                        PopLink();
                        return TextRendererStateChange.ChangeLink;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.ChangeSourceString:
                    {
                        var cmd = (TextLayoutSourceStringCommand*)input.Data;
                        source = new StringSource(input.GetSourceString(cmd->SourceIndex));
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.ChangeSourceStringBuilder:
                    {
                        var cmd = (TextLayoutSourceStringBuilderCommand*)input.Data;
                        source = new StringBuilderSource(input.GetSourceStringBuilder(cmd->SourceIndex));
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.ChangeSourceShapedString:
                    {
                        var cmd = (TextLayoutSourceShapedStringCommand*)input.Data;
                        source = input.GetSourceShapedString(cmd->SourceIndex);
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.ChangeSourceShapedStringBuilder:
                    {
                        var cmd = (TextLayoutSourceShapedStringBuilderCommand*)input.Data;
                        source = input.GetSourceShapedStringBuilder(cmd->SourceIndex);
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.Custom:
                    return TextRendererStateChange.None;
            }

            return TextRendererStateChange.None;
        }

        /// <summary>
        /// Processes a <see cref="TextLayoutCommandType.BlockInfo"/> command.
        /// </summary>
        private void ProcessBlockInfo(TextLayoutCommandStream input, out Int32 offset)
        {
            var cmd = (TextLayoutBlockInfoCommand*)input.Data;
            offset = cmd->Offset;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Processes a <see cref="TextLayoutCommandType.LineInfo"/> command.
        /// </summary>
        private void ProcessLineInfo(TextLayoutCommandStream input, ref TextSeekState state)
        {
            var cmd = (TextLayoutLineInfoCommand*)input.Data;
            state.LineIndex++;
            state.LineOffsetX = cmd->Offset;
            state.LineOffsetY = state.LineOffsetY + state.LineHeight;
            state.LineWidth = cmd->LineWidth;
            state.LineHeight = cmd->LineHeight;
            state.LineLengthInCommands = cmd->LengthInCommands;
            state.LineLengthInSource = cmd->LengthInSource;
            state.LineLengthInGlyphs = cmd->LengthInGlyphs;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified coordinates.
        /// </summary>
        private void SkipToLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, ref TextSeekState state)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;
                state.LineIndex++;
                state.LineWidth = cmd->LineWidth;
                state.LineHeight = cmd->LineHeight;
                state.LineLengthInCommands = cmd->LengthInCommands;
                state.LineLengthInSource = cmd->LengthInSource;
                state.LineLengthInShaped = cmd->LengthInShaped;
                state.LineLengthInGlyphs = cmd->LengthInGlyphs;
                state.LineOffsetX = cmd->Offset;
                state.LineStartInCommands = input.StreamPositionInObjects;
                state.LineStartInSource = state.NumberOfSourceCharactersSeen;
                state.LineStartInShaped = state.NumberOfShapedCharactersSeen;
                state.LineStartInGlyphs = state.NumberOfGlyphsSeen;
                state.TerminatingLineBreakLength = cmd->TerminatingLineBreakSourceLength;

                if (y >= state.LineOffsetY && y < state.LineOffsetY + state.LineHeight)
                    break;

                state.LineOffsetY += cmd->LineHeight;
                state.NumberOfSourceCharactersSeen += cmd->LengthInSource;
                state.NumberOfShapedCharactersSeen += cmd->LengthInShaped;
                state.NumberOfGlyphsSeen += cmd->LengthInGlyphs;
            }
            while (input.SeekNextLine());
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified glyph.
        /// </summary>
        private void SkipToLineContainingGlyph(TextLayoutCommandStream input, Int32 glyphIndex, ref TextSeekState state)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;

                state.LineIndex++;
                state.LineOffsetX = cmd->Offset;
                state.LineWidth = cmd->LineWidth;
                state.LineHeight = cmd->LineHeight;
                state.LineLengthInCommands = cmd->LengthInCommands;
                state.LineLengthInSource = cmd->LengthInSource;
                state.LineLengthInShaped = cmd->LengthInShaped;
                state.LineLengthInGlyphs = cmd->LengthInGlyphs;
                state.TerminatingLineBreakLength = cmd->TerminatingLineBreakSourceLength;

                if (state.NumberOfGlyphsSeen + cmd->LengthInGlyphs > glyphIndex)
                    break;

                state.LineOffsetY += state.LineHeight;
                state.NumberOfSourceCharactersSeen += cmd->LengthInSource;
                state.NumberOfShapedCharactersSeen += cmd->LengthInShaped;
                state.NumberOfGlyphsSeen += cmd->LengthInGlyphs;
            }
            while (input.SeekNextLine());

            state.LineStartInCommands = input.StreamPositionInObjects;
            state.LineStartInSource = state.NumberOfSourceCharactersSeen;
            state.LineStartInShaped = state.NumberOfShapedCharactersSeen;
            state.LineStartInGlyphs = state.NumberOfGlyphsSeen;

            input.SeekNextCommand();
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified source character.
        /// </summary>
        private void SkipToLineContainingSourceCharacter(TextLayoutCommandStream input, Int32 sourceCharacter, ref TextSeekState state)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;

                state.LineIndex++;
                state.LineOffsetX = cmd->Offset;
                state.LineWidth = cmd->LineWidth;
                state.LineHeight = cmd->LineHeight;
                state.LineLengthInCommands = cmd->LengthInCommands;
                state.LineLengthInSource = cmd->LengthInSource;
                state.LineLengthInShaped = cmd->LengthInShaped;
                state.LineLengthInGlyphs = cmd->LengthInGlyphs;
                state.TerminatingLineBreakLength = cmd->TerminatingLineBreakSourceLength;

                if (state.NumberOfSourceCharactersSeen + cmd->LengthInSource > sourceCharacter)
                    break;

                state.LineOffsetY += state.LineHeight;
                state.NumberOfSourceCharactersSeen += cmd->LengthInSource;
                state.NumberOfShapedCharactersSeen += cmd->LengthInShaped;
                state.NumberOfGlyphsSeen += cmd->LengthInGlyphs;
            }
            while (input.SeekNextLine());

            state.LineStartInCommands = input.StreamPositionInObjects;
            state.LineStartInSource = state.NumberOfSourceCharactersSeen;
            state.LineStartInShaped = state.NumberOfShapedCharactersSeen;
            state.LineStartInGlyphs = state.NumberOfGlyphsSeen;

            input.SeekNextCommand();
        }

        /// <summary>
        /// Gets the index of the glyph within the specified text that contains the specified position.
        /// </summary>
        private Int32? GetGlyphAtPositionWithinText(UltravioletFontFace fontFace, ref StringSegment text, ref Int32 position, TextDirection direction, out Int32 glyphWidth, out Int32 glyphHeight)
        {
            var glyphPosition = 0;

            if (direction == TextDirection.RightToLeft)
            {
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    var g1 = text[i];
                    var g2 = (i > 0) ? text[i - 1] : (Char?)null;
                    var glyphSize = fontFace.MeasureGlyph(g1, g2);

                    if (position >= glyphPosition && position < glyphPosition + glyphSize.Width)
                    {
                        position = glyphPosition;
                        glyphWidth = glyphSize.Width;
                        glyphHeight = glyphSize.Height;
                        return i;
                    }

                    glyphPosition += glyphSize.Width;

                    var iNext = i - 1;
                    if (iNext >= 0 && Char.IsSurrogatePair(text[iNext], text[i]))
                        i--;
                }
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var glyphSize = fontFace.MeasureGlyph(ref text, i);

                    if (position >= glyphPosition && position < glyphPosition + glyphSize.Width)
                    {
                        position = glyphPosition;
                        glyphWidth = glyphSize.Width;
                        glyphHeight = glyphSize.Height;
                        return i;
                    }

                    glyphPosition += glyphSize.Width;

                    var iNext = i + 1;
                    if (iNext < text.Length && Char.IsSurrogatePair(text[i], text[iNext]))
                        i++;
                }
            }

            position = 0;
            glyphWidth = 0;
            glyphHeight = 0;
            return null;
        }

        /// <summary>
        /// Gets the index of the glyph within the specified shaped text that contains the specified position.
        /// </summary>
        private Int32? GetGlyphAtPositionWithinShapedText(UltravioletFontFace fontFace, ref ShapedStringSegment text, ref Int32 position, out Int32 glyphWidth, out Int32 glyphHeight)
        {
            var glyphPosition = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var glyphSize = fontFace.MeasureShapedGlyph(ref text, i);
                if (position >= glyphPosition && position < glyphPosition + glyphSize.Width)
                {
                    position = glyphPosition;
                    glyphWidth = glyphSize.Width;
                    glyphHeight = glyphSize.Height;
                    return i;
                }
                glyphPosition += glyphSize.Width;
            }
            position = 0;
            glyphWidth = 0;
            glyphHeight = 0;
            return null;
        }

        /// <summary>
        /// Gets the index of the glyph or insertion point which is closest to the specified position in layout-relative space.
        /// </summary>
        private Int32? GetGlyphOrInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32? lineAtPosition, InsertionPointSearchMode searchMode)
        {
            var searchInsertionPoints = 
                (searchMode == InsertionPointSearchMode.BeforeOrAfterGlyph);
            var searchSnapToLine = 
                (searchMode == InsertionPointSearchMode.SnapToLine);

            lineAtPosition = searchInsertionPoints ? 0 : (Int32?)null;

            if (input.Count == 0)
                return searchInsertionPoints ? 0 : (Int32?)null;

            if (y < 0)
                return searchInsertionPoints ? 0 : (Int32?)null;
            if (x < 0 && !searchInsertionPoints)
                return null;

            var isSurrogatePair = false;
            var sourceCountSeen = 0;
            var shapedCountSeen = 0;
            var glyphCountSeen = 0;
            var glyphFound = false;
            var glyph = default(Int32?);
            var glyphSourceIndex = default(Int32?);
            var glyphBounds = Rectangle.Empty;
            var glyphWasLineBreak = false;
            var seekState = new TextSeekState { LineIndex = -1 };

            var settings = input.Settings;
            var bold = (settings.Style == UltravioletFontStyle.Bold || settings.Style == UltravioletFontStyle.BoldItalic);
            var italic = (settings.Style == UltravioletFontStyle.Italic || settings.Style == UltravioletFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);
            var direction = settings.Direction;
            var rtl = (direction == TextDirection.RightToLeft);

            var source = CreateSourceUnionFromSegmentOrigin(input.SourceText);
            
            input.Seek(0);

            var blockOffset = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;
            seekState.LineOffsetY = blockOffset;

            // If our search point comes before the start of the block, then
            // the only possible answer is the first glyph in the block.
            if (y < blockOffset)
                return searchInsertionPoints ? 0 : (Int32?)null;

            // If our search point comes after the end of the block, then
            // the only possible answer is the last glyph in the block.
            if (y >= blockOffset + input.ActualHeight)
            {
                if (searchInsertionPoints)
                {
                    lineAtPosition = input.LineCount - 1;
                    return input.TotalSourceLength;
                }
                lineAtPosition = null;
                return null;
            }

            input.SeekNextCommand();

            // If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines)
            {
                SkipToLineAtPosition(input, x, y, ref seekState);
                input.SeekNextCommand();

                // If our search point comes before the beginning of the line that it's on,
                // then the only possible answer is the first glyph on the line.
                if ((rtl && x >= seekState.LineOffsetX + seekState.LineWidth) || (!rtl && x < seekState.LineOffsetX))
                {
                    lineAtPosition = seekState.LineIndex;
                    return (searchInsertionPoints || searchSnapToLine) ? glyphCountSeen : default(Int32?);
                }

                sourceCountSeen = seekState.NumberOfSourceCharactersSeen;
                shapedCountSeen = seekState.NumberOfShapedCharactersSeen;
                glyphCountSeen = seekState.NumberOfGlyphsSeen;
            }

            var glyphIsInCurrentLine = canSkipLines;

            // Seek through the remaining commands until we find the one that contains our glyph.
            while (!glyphFound && input.StreamPositionInObjects < input.Count)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        {
                            // If we thought the glyph was in the previous line, but we've gone past the end of the line,
                            // then the only possible answer is the last glyph on the line.
                            if (glyphIsInCurrentLine)
                            {
                                lineAtPosition = seekState.LineIndex;
                                if (searchInsertionPoints)
                                {
                                    return sourceCountSeen - (glyphWasLineBreak ? 1 : 0);
                                }
                                return searchSnapToLine ? glyphCountSeen : default(Int32?);
                            }

                            ProcessLineInfo(input, ref seekState);

                            seekState.LineStartInSource = sourceCountSeen;
                            seekState.LineStartInShaped = shapedCountSeen;
                            seekState.LineStartInGlyphs = glyphCountSeen;

                            // Determine whether we expect the glyph that we're searching for to be on the current
                            // line, then check to see if our search point comes before the begining of the line. If
                            // it does, then the only possible answer is the first glyph on the line.
                            glyphIsInCurrentLine = (y >= seekState.LineOffsetY && y < seekState.LineOffsetY + seekState.LineHeight);
                            if (glyphIsInCurrentLine)
                            {
                                lineAtPosition = seekState.LineIndex;
                                if ((rtl && x >= seekState.LineOffsetX + seekState.LineWidth) || (!rtl && x < seekState.LineOffsetX))
                                    return (searchInsertionPoints || searchSnapToLine) ? glyphCountSeen : default(Int32?);
                            }
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
                            // If we expect the glyph to be on the current line, then compare our search position to
                            // the bounds of this text command. If the text command contains our search position, then
                            // check each of its glyphs individually.
                            var cmd = (TextLayoutTextCommand*)input.Data;
                            if (glyphIsInCurrentLine)
                            {
                                var tokenBounds = cmd->GetAbsoluteBounds(fontFace, seekState.LineOffsetX, blockOffset, seekState.LineWidth, seekState.LineHeight, direction);
                                if (x >= tokenBounds.Left && x < tokenBounds.Right)
                                {
                                    var glyphPos = 0;
                                    var glyphWidth = 0;
                                    var glyphHeight = 0;
                                    var glyphOffsetInText = 0;

                                    if (source.IsShaped)
                                    {
                                        var text = source.CreateShapedStringSegmentFromSameOrigin(cmd->ShapedOffset, cmd->ShapedLength);
                                        glyphPos = (x - tokenBounds.Left);
                                        glyphOffsetInText = GetGlyphAtPositionWithinShapedText(fontFace, ref text, ref glyphPos, out glyphWidth, out glyphHeight) ?? 0;
                                        glyphSourceIndex = text[glyphOffsetInText].SourceIndex;
                                        isSurrogatePair = false;
                                    }
                                    else
                                    {
                                        var text = source.CreateStringSegmentFromSameOrigin(cmd->SourceOffset, cmd->SourceLength);
                                        glyphPos = (x - tokenBounds.Left);
                                        glyphOffsetInText = GetGlyphAtPositionWithinText(fontFace, ref text, ref glyphPos, 
                                            input.Settings.Direction, out glyphWidth, out glyphHeight) ?? 0;
                                        glyphSourceIndex = cmd->SourceOffset + glyphOffsetInText;

                                        var glyphIx = glyphOffsetInText;
                                        var glyphIxNext = glyphIx + 1;
                                        isSurrogatePair = (glyphIxNext < text.Length) && Char.IsSurrogatePair(text[glyphIx], text[glyphIxNext]);
                                    }

                                    glyph = glyphCountSeen + glyphOffsetInText;
                                    glyphBounds = new Rectangle(tokenBounds.X + glyphPos, tokenBounds.Y, glyphWidth, glyphHeight);
                                    glyphFound = true;
                                }
                            }
                            sourceCountSeen += cmd->SourceLength;
                            shapedCountSeen += cmd->ShapedLength;
                            glyphCountSeen += cmd->GlyphLength;
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            // If we expect the glyph to be on the current line, then compare our search position to
                            // the bounds of this icon command. If the icon command contains our search position, then
                            // the icon glyph must be our answer.
                            var iconCmd = (TextLayoutIconCommand*)input.Data;
                            if (glyphIsInCurrentLine)
                            {
                                var iconBounds = iconCmd->GetAbsoluteBounds(seekState.LineOffsetX, blockOffset, seekState.LineWidth, seekState.LineHeight, direction);
                                if (x >= iconBounds.Left && x < iconBounds.Right)
                                {
                                    glyphSourceIndex = iconCmd->SourceOffset;
                                    glyph = glyphCountSeen;
                                    glyphBounds = iconBounds;
                                    glyphFound = true;
                                }
                            }
                            sourceCountSeen += iconCmd->SourceLength;
                            glyphCountSeen += 1;
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.LineBreak:
                        {
                            // Line breaks have no width, and therefore cannot contain the search position;
                            // skip past this command, but make a note of having seen it (after switch).
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            if (!glyphIsInCurrentLine)
                            {
                                sourceCountSeen += cmd->SourceLength;
                                glyphCountSeen += cmd->GlyphLength;
                            }
                        }
                        input.SeekNextCommand();
                        break;

                    default:
                        {
                            var change = ProcessStylingCommand(input, cmdType, 
                                TextRendererStacks.Style | TextRendererStacks.Font | TextRendererStacks.Link, ref bold, ref italic, ref source);
                            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
                            {
                                RefreshFont(input.Settings.Font, bold, italic, out font, out fontFace);
                            }
                        }
                        input.SeekNextCommand();
                        break;
                }

                glyphWasLineBreak = (cmdType == TextLayoutCommandType.LineBreak);
            }

            // If we've found a matching glyph, we need to decide what to do with it. If we're looking for the corresponding
            // insertion point, then determine which side of the glyph our search point fell on and adjust our index accordingly. 
            // Otherwise, just return the glyph's index within the source text.
            if (searchInsertionPoints)
            {
                if (glyphSourceIndex.HasValue)
                {
                    var max = seekState.LineStartInSource + seekState.LineLengthInSource - seekState.TerminatingLineBreakLength;
                    if (input.Settings.Direction == TextDirection.RightToLeft)
                    {
                        return Math.Min(max, (x - glyphBounds.Center.X < 0) ? glyphSourceIndex.Value + (isSurrogatePair ? 2 : 1) : glyphSourceIndex.Value);
                    }
                    else
                    {
                        return Math.Min(max, (x - glyphBounds.Center.X < 0) ? glyphSourceIndex.Value : glyphSourceIndex.Value + (isSurrogatePair ? 2 : 1));
                    }
                }
                lineAtPosition = input.LineCount - 1;
                return input.TotalSourceLength;
            }

            return glyph;
        }

        // The text parser.
        private readonly TextParser parser = new TextParser();
        private readonly TextParserTokenStream parserResult = new TextParserTokenStream();

        // The text layout engine.
        private readonly TextLayoutEngine layoutEngine = new TextLayoutEngine();
        private readonly TextLayoutCommandStream layoutResult = new TextLayoutCommandStream();

        // Layout parameter stacks.
        private readonly Stack<TextStyleInstance> styleStack = new Stack<TextStyleInstance>();
        private readonly Stack<TextStyleScoped<UltravioletFont>> fontStack = new Stack<TextStyleScoped<UltravioletFont>>();
        private readonly Stack<TextStyleScoped<Color>> colorStack = new Stack<TextStyleScoped<Color>>();
        private readonly Stack<TextStyleScoped<GlyphShader>> glyphShaderStack = new Stack<TextStyleScoped<GlyphShader>>();
        private readonly Stack<Int16> linkStack = new Stack<Int16>();
    }
}
