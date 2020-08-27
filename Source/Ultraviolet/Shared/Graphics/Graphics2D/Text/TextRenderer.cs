using System;
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

            styleManager.ClearLayoutStacks();

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

            var glyph = GetGlyphOrInsertionPointAtPosition(input, x, y, out _, InsertionPointSearchMode.BeforeGlyph);
            if (glyph != null)
                linkIndex = styleManager.LinkStack.Count > 0 ? styleManager.LinkStack.Peek() : (Int16?)null;

            if (acquiredPointers)
                input.ReleasePointers();

            styleManager.ClearLayoutStacks();

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
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Vector2 position, Boolean stretch = false) =>
            GetLineAtPosition(input, (Int32)position.X, (Int32)position.Y, stretch);

        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="stretch">If <see langword="true"/>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <see langword="null"/> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Point2 position, Boolean stretch = false) =>
            GetLineAtPosition(input, position.X, position.Y, stretch);

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
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position) =>
            GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, false, out _);

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position, out Int32? lineAtPosition) =>
            GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, false, out lineAtPosition);

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
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position, Boolean snapToLine, out Int32? lineAtPosition) =>
            GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, snapToLine, out lineAtPosition);

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position) =>
            GetGlyphAtPosition(input, position.X, position.Y, false, out _);

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified position, regardless of
        /// whether the position corresponds to an actual glyph.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position, out Int32? lineAtPosition) =>
            GetGlyphAtPosition(input, position.X, position.Y, false, out lineAtPosition);

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
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position, Boolean snapToLine, out Int32? lineAtPosition) =>
            GetGlyphAtPosition(input, position.X, position.Y, snapToLine, out lineAtPosition);

        /// <summary>
        /// Gets the index of the glyph at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the glyph at the specified layout-relative position,
        /// or <c>nulll</c> if the specified position is not contained by any glyph.</returns>
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y) => 
            GetGlyphAtPosition(input, x, y, false, out _);

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
        public Int32? GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32? lineAtPosition) =>
            GetGlyphAtPosition(input, x, y, false, out lineAtPosition);

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

            styleManager.ClearLayoutStacks();

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

            styleManager.ClearLayoutStacks();

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
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Vector2 position) =>
            GetInsertionPointAtPosition(input, (Int32)position.X, (Int32)position.Y);

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified insertion point.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Vector2 position, out Int32 lineAtPosition) =>
            GetInsertionPointAtPosition(input, (Int32)position.X, (Int32)position.Y, out lineAtPosition);

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Point2 position) =>
            GetInsertionPointAtPosition(input, position.X, position.Y);

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="lineAtPosition">The index of the line of text that contains the specified insertion point.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Point2 position, out Int32 lineAtPosition) =>
            GetInsertionPointAtPosition(input, position.X, position.Y, out lineAtPosition);

        /// <summary>
        /// Gets the index of the insertion point which is closest to the specified layout-relative position.
        /// </summary>
        /// <remarks>An insertion point represents a position at which new text can be inserted into the formatted text, starting
        /// at index 0 (before the first character) and ending at <see cref="TextLayoutCommandStream.Count"/>, inclusive (after the last character).</remarks>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the insertion point which is closest to the specified layout-relative position.</returns>
        public Int32 GetInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y) =>
            GetInsertionPointAtPosition(input, x, y, out _);

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

            var result = GetGlyphOrInsertionPointAtPosition(input, x, y, out var lineAtPositionTemp, InsertionPointSearchMode.BeforeOrAfterGlyph) ?? 0;
            lineAtPosition = lineAtPositionTemp ?? 0;

            if (acquiredPointers)
                input.ReleasePointers();

            styleManager.ClearLayoutStacks();

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
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index, Boolean spanLineHeight = false) =>
            GetGlyphBounds(input, index, out _, spanLineHeight);

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

            ProcessBlockInfo(input, out var blockOffset);
            drawState.BlockOffset = blockOffset;
            
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
                        GetGlyphBounds_LineInfo(input, blockOffset, ref seekState);
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
                        GetGlyphBounds_Default(input, cmdType, ref drawState);
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

            styleManager.ClearLayoutStacks();

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
        private void GetGlyphBounds_Default(TextLayoutCommandStream input, TextLayoutCommandType cmdType, ref TextDrawState drawState)
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
        private void GetGlyphBounds_LineInfo(TextLayoutCommandStream input, Int32 blockOffset, ref TextSeekState seekState)
        {
            ProcessLineInfo(input, blockOffset, ref seekState);
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
        public Rectangle GetInsertionPointBounds(TextLayoutCommandStream input, Int32 index) =>
            GetInsertionPointBounds(input, index, out _, out _);

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
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The string of text to lay out.</param>
        /// <param name="output">The command stream representing the formatted text.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(String input, TextLayoutCommandStream output, TextLayoutSettings settings)
        {
            Contract.Require(input, nameof(input));
            Contract.Require(output, nameof(output));

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, output, settings);
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

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, output, settings);
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

            LayoutEngine.CalculateLayout(input, output, settings);
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

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, String input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, 0, Int32.MaxValue);
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

            Parser.Parse(input, parserResult, parserOptions);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="parserOptions">The parser options to use when parsing the input text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, String input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, Int32 start, Int32 count, TextParserOptions parserOptions, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            Parser.Parse(input, parserResult, parserOptions);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, start, count);
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

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            Parser.Parse(input, parserResult);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, 0, Int32.MaxValue);
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

            Parser.Parse(input, parserResult, parserOptions);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The string which will be lexed, parsed, laid out, and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="parserOptions">The parser options to use when parsing the input text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, Int32 start, Int32 count, TextParserOptions parserOptions, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            Parser.Parse(input, parserResult, parserOptions);
            LayoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, start, count);
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

            LayoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The collection of parser tokens which will be laid out and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextParserTokenStream input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            LayoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, 0, Int32.MaxValue);
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

            LayoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, null, null, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The collection of parser tokens which will be laid out and drawn.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextParserTokenStream input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, Int32 start, Int32 count, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            LayoutEngine.CalculateLayout(input, layoutResult, settings);

            return DrawInternal(spriteBatch, layoutResult, position, minClip, maxClip, defaultColor, start, count);
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

            return DrawInternal(spriteBatch, input, position, null, null, defaultColor, 0, Int32.MaxValue);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The text layout command stream that describes the text to draw.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            return DrawInternal(spriteBatch, input, position, minClip, maxClip, defaultColor, 0, Int32.MaxValue);
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

            return DrawInternal(spriteBatch, input, position, null, null, defaultColor, start, count);
        }

        /// <summary>
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance with which to draw the formatted text.</param>
        /// <param name="input">The text layout command stream that describes the text to draw.</param>
        /// <param name="position">The position at which to draw the text.</param>
        /// <param name="minClip">A vertical coordinate above which lines will be clipped without being drawn.</param>
        /// <param name="maxClip">A vertical coordinate below which lines will be clipped without being drawn.</param>
        /// <param name="defaultColor">The color with which to draw the text.</param>
        /// <param name="start">The index of the first character to draw.</param>
        /// <param name="count">The number of characters to draw.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Int32? minClip, Int32 maxClip, Color defaultColor, Int32 start, Int32 count)
        {
            Contract.Require(spriteBatch, nameof(spriteBatch));
            Contract.Require(input, nameof(input));

            return DrawInternal(spriteBatch, input, position, minClip, maxClip, defaultColor, start, count);
        }

        /// <summary>
        /// Gets the renderer's parser.
        /// </summary>
        public TextParser Parser { get; } = new TextParser();

        /// <summary>
        /// Gets the renderer's layout engine.
        /// </summary>
        public TextLayoutEngine LayoutEngine { get; } = new TextLayoutEngine();

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
        private RectangleF DrawInternal(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Int32? minClip, Int32? maxClip, Color defaultColor, Int32 start, Int32 count)
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
            var seekState = new TextSeekState { LineIndex = -1 };

            var glyphsSeen = 0;
            var glyphsMax = (count == Int32.MaxValue) ? Int32.MaxValue : start + count - 1;

            var source = (StringSourceUnion)(StringSegmentSource)input.SourceText;

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();
            
            var linkAtCursor = GetLinkIndexAtCursor(input);

            input.Seek(0);
            ProcessBlockInfo(input, out var blockOffset);

            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines && minClip.HasValue)
            {
                SkipToLineAtPosition(input, blockOffset, 0, minClip.Value, ref seekState);
                input.SeekNextCommand();
            }

            while (input.StreamPositionInObjects < input.Count)
            {
                if (glyphsSeen > glyphsMax)
                    break;

                var cmdType = *(TextLayoutCommandType*)input.Data;
                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        {
                            ProcessLineInfo(input, blockOffset, ref seekState);
                            if (seekState.LinePositionY + seekState.LineHeight > availableHeight || (maxClip.HasValue && seekState.LinePositionY > maxClip.Value))
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
                                var linkIndex = styleManager.LinkStack.Count > 0 ? styleManager.LinkStack.Peek() : (Int16?)null;
                                RefreshColor(input, defaultColor, linkIndex, linkAtCursor, ref color);

                                if (styleManager.LinkStack.Count == 0)
                                    lastColorOutsideLink = color;
                            }
                        }
                        input.SeekNextCommand();
                        break;
                }
            }

            if (acquiredPointers)
                input.ReleasePointers();

            styleManager.ClearLayoutStacks();

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
        private Boolean GetIsTextVisible(Int32 length, Int32 start, Int32 glyphsSeen)
        {
            return start < glyphsSeen + length;
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

            if (GetIsTextVisible(cmdFullLength, start, glyphsSeen))
            {
                var cmdOffset = 0;
                if (GetIsTextPartiallyVisible(cmdFullLength, start, end, glyphsSeen, out var subStart, out var subLength, out cmdWasDrawnCompletely))
                {
                    cmdOffset = (subStart == 0) ? 0 : fontFace.MeasureString(ref cmdText, 0, subStart).Width;
                    cmdText = cmdText.Substring(subStart, subLength);
                }

                cmdPosition = cmd->GetAbsolutePositionVector(fontFace, x + cmdOffset, y, lineWidth, lineHeight, direction);
                
                var effects = (direction == TextDirection.RightToLeft) ? SpriteEffects.DrawTextReversed : SpriteEffects.None;
                var gscontext = (styleManager.GlyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : 
                    new GlyphShaderContext(styleManager.GlyphShaderStack, glyphsSeen, input.TotalGlyphLength);
                
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

                    var gscontext = (styleManager.GlyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : 
                        new GlyphShaderContext(styleManager.GlyphShaderStack, glyphsSeen - 1, input.TotalGlyphLength);

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

            if (GetIsTextVisible(cmdText.Length, start, glyphsSeen))
            {
                var cmdOffset = 0;
                if (GetIsTextPartiallyVisible(cmdText.Length, start, end, glyphsSeen, out var subStart, out var subLength, out cmdWasDrawnCompletely))
                {
                    cmdOffset = (subStart == 0) ? 0 : fontFace.MeasureShapedString(ref cmdText, 0, subStart).Width;
                    cmdText = cmdText.Substring(subStart, subLength);
                }

                cmdPosition = cmd->GetAbsolutePositionVector(fontFace, x + cmdOffset, y, lineWidth, lineHeight, direction);

                var effects = (direction == TextDirection.RightToLeft) ? SpriteEffects.DrawTextReversed : SpriteEffects.None;
                var gscontext = (styleManager.GlyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : 
                    new GlyphShaderContext(styleManager.GlyphShaderStack, glyphsSeen, input.TotalGlyphLength);

                spriteBatch.DrawShapedString(gscontext, fontFace, cmdText, cmdPosition, color, 0f, 
                    Vector2.Zero, Vector2.One, effects, 0f, default);
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

                    var gscontext = (styleManager.GlyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : 
                        new GlyphShaderContext(styleManager.GlyphShaderStack, glyphsSeen - 1, input.TotalGlyphLength);

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

                var cmdGlyphShaderContext = (styleManager.GlyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid :
                    new GlyphShaderContext(styleManager.GlyphShaderStack, glyphsSeen, input.TotalGlyphLength);
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
                if (iconFrame != null)
                {
                    spriteBatch.DrawSprite(iconController, iconPosition + iconFrame.Origin, iconWidth, iconHeight, color, iconRotation);
                }
            }

            glyphsSeen += cmd->GlyphLength;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Updates the current font by examining the state of the layout stacks.
        /// </summary>
        private void RefreshFont(UltravioletFont baseFont, Boolean bold, Boolean italic, out UltravioletFont font, out UltravioletFontFace fontFace)
        {
            font = (styleManager.FontStack.Count == 0) ? baseFont : styleManager.FontStack.Peek().Value;
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
                color = (styleManager.ColorStack.Count == 0) ? defaultColor : styleManager.ColorStack.Peek().Value;
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
                        styleManager.PushStyle(input.GetStyle(cmd->StyleIndex), ref bold, ref italic);
                        return TextRendererStateChange.ChangeFont | TextRendererStateChange.ChangeColor | TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushFont:
                    if ((stacks & TextRendererStacks.Font) == TextRendererStacks.Font)
                    {
                        var cmd = (TextLayoutFontCommand*)input.Data;
                        styleManager.PushFont(input.GetFont(cmd->FontIndex));
                        return TextRendererStateChange.ChangeFont;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushColor:
                    if ((stacks & TextRendererStacks.Color) == TextRendererStacks.Color)
                    {
                        var cmd = (TextLayoutColorCommand*)input.Data;
                        styleManager.PushColor(cmd->Color);
                        return TextRendererStateChange.ChangeColor;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushGlyphShader:
                    if ((stacks & TextRendererStacks.GlyphShader) == TextRendererStacks.GlyphShader)
                    {
                        var cmd = (TextLayoutGlyphShaderCommand*)input.Data;
                        styleManager.PushGlyphShader(input.GetGlyphShader(cmd->GlyphShaderIndex));
                        return TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PushLink:
                    if ((stacks & TextRendererStacks.Link) == TextRendererStacks.Link)
                    {
                        var cmd = (TextLayoutLinkCommand*)input.Data;
                        styleManager.PushLink(cmd->LinkTargetIndex);
                        return TextRendererStateChange.ChangeLink;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopStyle:
                    if ((stacks & TextRendererStacks.Style) == TextRendererStacks.Style)
                    {
                        styleManager.PopStyle(ref bold, ref italic);
                        return TextRendererStateChange.ChangeFont | TextRendererStateChange.ChangeColor | TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopFont:
                    if ((stacks & TextRendererStacks.Font) == TextRendererStacks.Font)
                    {
                        styleManager.PopFont();
                        return TextRendererStateChange.ChangeFont;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopColor:
                    if ((stacks & TextRendererStacks.Color) == TextRendererStacks.Color)
                    {
                        styleManager.PopColor();
                        return TextRendererStateChange.ChangeColor;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopGlyphShader:
                    if ((stacks & TextRendererStacks.GlyphShader) == TextRendererStacks.GlyphShader)
                    {
                        styleManager.PopGlyphShader();
                        return TextRendererStateChange.ChangeGlyphShader;
                    }
                    return TextRendererStateChange.None;

                case TextLayoutCommandType.PopLink:
                    if ((stacks & TextRendererStacks.Link) == TextRendererStacks.Link)
                    {
                        styleManager.PopLink();
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
            if (*(TextLayoutCommandType*)input.Data == TextLayoutCommandType.BlockInfo)
            {
                var cmd = (TextLayoutBlockInfoCommand*)input.Data;
                offset = cmd->Offset;
                input.SeekNextCommand();
            }
            else
            {
                offset = 0;
            }
        }

        /// <summary>
        /// Processes a <see cref="TextLayoutCommandType.LineInfo"/> command.
        /// </summary>
        private void ProcessLineInfo(TextLayoutCommandStream input, Int32 blockOffset, ref TextSeekState state)
        {
            var cmd = (TextLayoutLineInfoCommand*)input.Data;
            state.LineIndex++;
            state.LineOffsetX = cmd->Offset;
            state.LineOffsetY = state.LineOffsetY + state.LineHeight;
            state.LinePositionY = state.LineOffsetY + blockOffset;
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
        private void SkipToLineAtPosition(TextLayoutCommandStream input, Int32 blockOffset, Int32 x, Int32 y, ref TextSeekState state)
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

                var linePositionY = state.LineOffsetY + blockOffset;
                if (y >= linePositionY && y < linePositionY + state.LineHeight)
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

            ProcessBlockInfo(input, out var blockOffset);

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

            // If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines)
            {
                SkipToLineAtPosition(input, blockOffset, x, y, ref seekState);
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
                                    return sourceCountSeen;
                                }
                                return searchSnapToLine ? glyphCountSeen : default(Int32?);
                            }

                            ProcessLineInfo(input, blockOffset, ref seekState);

                            seekState.LineStartInSource = sourceCountSeen;
                            seekState.LineStartInShaped = shapedCountSeen;
                            seekState.LineStartInGlyphs = glyphCountSeen;

                            // Determine whether we expect the glyph that we're searching for to be on the current
                            // line, then check to see if our search point comes before the begining of the line. If
                            // it does, then the only possible answer is the first glyph on the line.
                            glyphIsInCurrentLine = (y >= seekState.LinePositionY && y < seekState.LinePositionY + seekState.LineHeight);
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
                                var tokenBounds = cmd->GetAbsoluteBounds(fontFace, seekState.LineOffsetX, seekState.LinePositionY, seekState.LineWidth, seekState.LineHeight, direction);
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
                                var iconBounds = iconCmd->GetAbsoluteBounds(seekState.LineOffsetX, seekState.LinePositionY, seekState.LineWidth, seekState.LineHeight, direction);
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

        // Text and layout buffers.
        private readonly TextParserTokenStream parserResult = new TextParserTokenStream();
        private readonly TextLayoutCommandStream layoutResult = new TextLayoutCommandStream();
        private readonly TextRendererStyleManager styleManager = new TextRendererStyleManager();
    }
}
