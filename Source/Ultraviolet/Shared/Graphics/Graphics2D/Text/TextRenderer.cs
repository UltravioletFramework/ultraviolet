using System;
using System.Collections.Generic;
using System.Security;
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
    public sealed unsafe class TextRenderer
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
            var glyph = GetGlyphOrInsertionPointAtPosition(input, x, y, out line, GlyphSearchMode.SearchGlyphs);

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
                snapToLine ? GlyphSearchMode.SearchGlyphsSnapToLine : GlyphSearchMode.SearchGlyphs);

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return result;
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
            var result = GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPositionTemp, GlyphSearchMode.SearchInsertionPoints) ?? 0;

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
            Contract.EnsureRange(index >= 0 && index < input.TotalLength, nameof(index));

            var glyphCountSeen = 0;
            
            var boundsFound = false;
            var bounds = Rectangle.Empty;

            var lineOffsetInCommands = 0;
            var lineOffsetInGlyphs = 0;
            var lineLengthInCommands = 0;
            var lineLengthInGlyphs = 0;
            var lineIndex = -1;
            var lineWidth = 0;
            var lineHeight = 0;
            var lineIsTerminatedByLineBreak = false;

            var settings = input.Settings;
            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);

            var source = (input.SourceText.SourceString != null) ?
                new StringSource(input.SourceText.SourceString) :
                new StringSource(input.SourceText.SourceStringBuilder);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var blockOffset = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;
            var offsetLineX = 0;
            var offsetLineY = 0;

            input.SeekNextCommand();

            // NOTE: If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines)
            {
                SkipToLineContainingGlyph(input, index, ref lineIndex, ref offsetLineX, ref offsetLineY, 
                    ref lineWidth, ref lineHeight, ref lineLengthInCommands, ref lineLengthInGlyphs, ref lineIsTerminatedByLineBreak, ref glyphCountSeen);

                lineOffsetInCommands = input.StreamPositionInObjects;
                lineOffsetInGlyphs = glyphCountSeen;
            }

            // Seek through the remaining commands until we find the one that contains our glyph.
            while (!boundsFound && input.StreamPositionInObjects < input.Count)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        {
                            ProcessLineInfo(input, ref lineIndex, ref offsetLineX, ref offsetLineY,
                                ref lineWidth, ref lineHeight, ref lineLengthInCommands, ref lineLengthInGlyphs);

                            lineOffsetInCommands = input.StreamPositionInObjects;
                            lineOffsetInGlyphs = glyphCountSeen;
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
                            var cmd = (TextLayoutTextCommand*)input.Data;
                            if (glyphCountSeen + cmd->TextLength > index)
                            {
                                var text = source.CreateStringSegmentFromSameSource(cmd->TextOffset, cmd->TextLength);

                                var glyphIndexWithinText = index - glyphCountSeen;
                                var glyphOffset = (glyphIndexWithinText == 0) ? 0 : fontFace.MeasureString(text, 0, glyphIndexWithinText).Width;
                                var glyphSize = fontFace.MeasureGlyph(text, glyphIndexWithinText);
                                var glyphPosition = spanLineHeight ? new Point2(cmd->Bounds.Location.X + offsetLineX + glyphOffset, cmd->Bounds.Location.Y) :
                                    cmd->GetAbsolutePosition(offsetLineX + glyphOffset, blockOffset, lineHeight);

                                bounds = new Rectangle(glyphPosition, spanLineHeight ? new Size2(glyphSize.Width, lineHeight) : glyphSize);
                                boundsFound = true;
                            }
                            glyphCountSeen += cmd->TextLength;
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            var cmd = (TextLayoutIconCommand*)input.Data;
                            if (++glyphCountSeen > index)
                            {
                                var glyphSize = cmd->Bounds.Size;
                                var glyphPosition = spanLineHeight ? new Point2(cmd->Bounds.Location.X + offsetLineX, cmd->Bounds.Location.Y) :
                                    cmd->GetAbsolutePosition(offsetLineX, blockOffset, lineHeight);

                                bounds = new Rectangle(glyphPosition, spanLineHeight ? new Size2(glyphSize.Width, lineHeight) : glyphSize);
                                boundsFound = true;
                            }
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.LineBreak:
                        {
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            if (glyphCountSeen + cmd->Length > index)
                            {
                                bounds = new Rectangle(Math.Max(0, offsetLineX + lineWidth - 1), blockOffset + offsetLineY, 0, lineHeight);
                                boundsFound = true;
                            }
                            glyphCountSeen += cmd->Length;
                        }
                        input.SeekNextCommand();
                        break;

                    default:
                        {
                            var change = ProcessStylingCommand(input, cmdType, TextRendererStacks.Style | TextRendererStacks.Font, ref bold, ref italic, ref source);
                            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
                            {
                                RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            }
                        }
                        input.SeekNextCommand();
                        break;
                }
            }

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            lineInfo = new LineInfo(input, lineIndex, lineOffsetInCommands, lineOffsetInGlyphs, offsetLineX, offsetLineY, 
                lineWidth, lineHeight, lineLengthInCommands, lineLengthInGlyphs);

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
            
            if (input.TotalLength == index)
            {
                var lineDefaultHeight = (input.Settings.Font == null) ? 0 :
                    input.Settings.Font.GetFace(SpriteFontStyle.Regular).LineSpacing;

                lineInfo = (input.TotalLength > 0) ? input.GetLineInfo(input.LineCount - 1) : 
                    new LineInfo(input, 0, 0, 0, 0, 0, 0, lineDefaultHeight, 0, 0);

                glyphBounds = null;
                return new Rectangle(lineInfo.X + lineInfo.Width, lineInfo.Y, 0, lineInfo.Height);
            }
            else
            {
                var glyphBoundsValue = GetGlyphBounds(input, index, out lineInfo, true);
                
                glyphBounds = glyphBoundsValue;
                return new Rectangle(glyphBoundsValue.Left, glyphBoundsValue.Top, 0, glyphBoundsValue.Height);
            }
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
        public void RegisterFont(String name, SpriteFont font)
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
        /// Draws a string of formatted text using the specified <see cref="SpriteBatch"/> instance.
        /// </summary>
        private RectangleF DrawInternal(SpriteBatch spriteBatch, TextLayoutCommandStream input, Vector2 position, Color defaultColor, Int32 start, Int32 count)
        {
            if (input.Settings.Font == null)
                throw new ArgumentException(UltravioletStrings.InvalidLayoutSettings);

            var settings = input.Settings;
            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);
            var color = defaultColor;
            var lastColorOutsideLink = defaultColor;

            var availableHeight = settings.Height ?? Int32.MaxValue;
            var blockOffset = 0;
            var lineIndex = -1;
            var lineOffset = 0;
            var linePosition = 0;
            var lineWidth = 0;
            var lineHeight = 0;
            var lineLengthInCommands = 0;
            var lineLengthInGlyphs = 0;

            var charsSeen = 0;
            var charsMax = (count == Int32.MaxValue) ? Int32.MaxValue : start + count - 1;

            var source = new StringSource(input.SourceText);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();
            
            var linkAtCursor = GetLinkIndexAtCursor(input);

            input.Seek(0);

            while (input.StreamPositionInObjects < input.Count)
            {
                if (charsSeen > charsMax)
                    break;

                var cmdType = *(TextLayoutCommandType*)input.Data;
                switch (cmdType)
                {
                    case TextLayoutCommandType.BlockInfo:
                        ProcessBlockInfo(input, out blockOffset);
                        break;

                    case TextLayoutCommandType.LineInfo:
                        {
                            ProcessLineInfo(input, ref lineIndex, ref lineOffset, ref linePosition, 
                                ref lineWidth, ref lineHeight, ref lineLengthInCommands, ref lineLengthInGlyphs);
                            if (blockOffset + linePosition + lineHeight > availableHeight)
                            {
                                input.SeekEnd();
                            }
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        DrawText(spriteBatch, input, fontFace, ref source,
                            position.X + lineOffset, position.Y + blockOffset, lineHeight, start, charsMax, color, ref charsSeen);
                        break;

                    case TextLayoutCommandType.Icon:
                        DrawIcon(spriteBatch, input, 
                            position.X + lineOffset, position.Y + blockOffset, lineHeight, start, count, lastColorOutsideLink, ref charsSeen);
                        break;
                        
                    case TextLayoutCommandType.LineBreak:
                        {
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            charsSeen += cmd->Length;
                        }
                        input.SeekNextCommand();
                        break;

                    default:
                        {
                            var change = ProcessStylingCommand(input, cmdType, TextRendererStacks.All, ref bold, ref italic, ref source);
                            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
                            {
                                RefreshFont(ref settings, bold, italic, out font, out fontFace);
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
        /// Draws a text command.
        /// </summary>
        private void DrawText(SpriteBatch spriteBatch, TextLayoutCommandStream input, SpriteFontFace fontFace, ref StringSource source,
            Single x, Single y, Int32 lineHeight, Int32 start, Int32 end, Color color, ref Int32 charsSeen)
        {
            var wasDrawnToCompletion = true;

            var cmd = (TextLayoutTextCommand*)input.Data;
            var cmdText = source.CreateStringSegmentFromSubstring(cmd->TextOffset, cmd->TextLength);
            if (cmdText.Equals("\n"))
            {
                charsSeen += cmdText.Length;
                input.SeekNextCommand();
                return;
            }

            var cmdLength = cmdText.Length;
            var cmdPosition = Vector2.Zero;
            var cmdGlyphShaderContext = default(GlyphShaderContext);
            var cmdOffset = 0;

            var isTextVisible = (start < charsSeen + cmdLength);
            if (isTextVisible)
            {
                var tokenStart = charsSeen;
                var tokenEnd = tokenStart + cmdText.Length - 1;

                var isTextPartiallyVisible = ((tokenStart < start && tokenEnd >= start) || (tokenStart <= end && tokenEnd > end));
                if (isTextPartiallyVisible)
                {
                    wasDrawnToCompletion = false;

                    var subStart = (charsSeen > start) ? 0 : start - charsSeen;
                    var subEnd = Math.Min(end, charsSeen + cmdText.Length - 1) - charsSeen;
                    var subLength = 1 + (subEnd - subStart);
                    cmdOffset = (subStart == 0) ? 0 : fontFace.MeasureString(cmdText, 0, subStart).Width;
                    cmdText = cmdText.Substring(subStart, subLength);
                }

                cmdPosition = cmd->GetAbsolutePositionVector(x + cmdOffset, y, lineHeight);
                cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, charsSeen, input.TotalLength);

                spriteBatch.DrawString(cmdGlyphShaderContext, fontFace, cmdText, cmdPosition, color);
            }

            charsSeen += cmdLength;
            input.SeekNextCommand();

            var isTextSplitByHyphen = (input.StreamPositionInObjects < input.Count && *(TextLayoutCommandType*)input.Data == TextLayoutCommandType.Hyphen);
            if (isTextSplitByHyphen)
            {
                if (wasDrawnToCompletion)
                {
                    var hyphenatedGlyph = cmdText[cmdText.Length - 1];
                    var hyphenatedTextWidth = fontFace.MeasureString(cmdText).Width;
                    var hyphenatedTextKerning = fontFace.Kerning.Get(hyphenatedGlyph, '-');

                    cmdPosition = new Vector2(cmdPosition.X + hyphenatedTextWidth + hyphenatedTextKerning, cmdPosition.Y);
                    cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, charsSeen - 1, input.TotalLength);

                    spriteBatch.DrawString(cmdGlyphShaderContext, fontFace, "-", cmdPosition, color);
                }

                input.SeekNextCommand();
            }
        }

        /// <summary>
        /// Draws an icon command.
        /// </summary>
        private void DrawIcon(SpriteBatch spriteBatch, TextLayoutCommandStream input,
            Single x, Single y, Int32 lineHeight, Int32 start, Int32 end, Color color, ref Int32 charsSeen)
        {
            var cmd = (TextLayoutIconCommand*)input.Data;

            var isIconVisible = (start < charsSeen + 1);
            if (isIconVisible)
            {
                var icon = input.GetIcon(cmd->IconIndex);
                var iconWidth = (Single)(icon.Width ?? icon.Icon.Controller.Width);
                var iconHeight = (Single)(icon.Height ?? icon.Icon.Controller.Height);
                var iconPosition = cmd->GetAbsolutePositionVector(x, y, lineHeight);
                var iconRotation = 0f;

                var cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, charsSeen, input.TotalLength);
                if (cmdGlyphShaderContext.IsValid)
                {
                    var glyphData = new GlyphData();
                    glyphData.Glyph = '\x0000';
                    glyphData.Pass = 0;
                    glyphData.X = iconPosition.X;
                    glyphData.Y = iconPosition.Y;
                    glyphData.ScaleX = 1.0f;
                    glyphData.ScaleY = 1.0f;
                    glyphData.Color = color;
                    glyphData.ClearDirtyFlags();

                    cmdGlyphShaderContext.Execute(ref glyphData, charsSeen);

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

            charsSeen += 1;
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
        private void PushFont(SpriteFont font)
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
        private void RefreshFont(ref TextLayoutSettings settings, Boolean bold, Boolean italic, out SpriteFont font, out SpriteFontFace fontFace)
        {
            font = (fontStack.Count == 0) ? settings.Font : fontStack.Peek().Value;
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
            ref Boolean bold, ref Boolean italic, ref StringSource source)
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
                        source = new StringSource(input.GetSourceStringBuilder(cmd->SourceIndex));
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
        private void ProcessLineInfo(TextLayoutCommandStream input, ref Int32 index, ref Int32 offset, ref Int32 position, 
            ref Int32 width, ref Int32 height, ref Int32 lengthInCommands, ref Int32 lengthInGlyphs)
        {
            var cmd = (TextLayoutLineInfoCommand*)input.Data;
            index++;
            offset = cmd->Offset;
            position = position + height;
            width = cmd->LineWidth;
            height = cmd->LineHeight;
            lengthInCommands = cmd->LengthInCommands;
            lengthInGlyphs = cmd->LengthInGlyphs;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified coordinates.
        /// </summary>
        private void SkipToLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, ref Int32 lineIndex, ref Int32 lineOffset, ref Int32 linePosition, 
            ref Int32 lineWidth, ref Int32 lineHeight, ref Int32 lineLengthInCommands, ref Int32 lineLengthInGlyphs, ref Boolean lineIsTerminatedByLineBreak, ref Int32 glyphCountSeen)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;
                lineIndex++;
                lineOffset = cmd->Offset;
                lineWidth = cmd->LineWidth;
                lineHeight = cmd->LineHeight;
                lineLengthInCommands = cmd->LengthInCommands;
                lineLengthInGlyphs = cmd->LengthInGlyphs;
                lineIsTerminatedByLineBreak = cmd->TerminatedByLineBreak;

                if (y >= linePosition && y < linePosition + lineHeight)
                    break;

                glyphCountSeen += cmd->LengthInGlyphs;
                linePosition += cmd->LineHeight;
            }
            while (input.SeekNextLine());
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified glyph.
        /// </summary>
        private void SkipToLineContainingGlyph(TextLayoutCommandStream input, Int32 glyph, ref Int32 lineIndex, ref Int32 lineOffset, ref Int32 linePosition, 
            ref Int32 lineWidth, ref Int32 lineHeight, ref Int32 lineLengthInCommands, ref Int32 lineLengthInGlyphs, ref Boolean lineIsTerminatedByLineBreak, ref Int32 glyphCountSeen)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;

                if (glyphCountSeen + cmd->LengthInGlyphs > glyph)
                    break;

                linePosition += lineHeight;

                lineIndex++;
                lineOffset = cmd->Offset;
                lineWidth = cmd->LineWidth;
                lineHeight = cmd->LineHeight;
                lineLengthInCommands = cmd->LengthInCommands;
                lineLengthInGlyphs = cmd->LengthInGlyphs;
                lineIsTerminatedByLineBreak = cmd->TerminatedByLineBreak;

                glyphCountSeen += cmd->LengthInGlyphs;
            }
            while (input.SeekNextLine());
        }

        /// <summary>
        /// Gets the index of the glyph within the specified text that contains the specified position.
        /// </summary>
        private Int32? GetGlyphAtPositionWithinText(SpriteFontFace fontFace, ref StringSegment text, ref Int32 position, out Int32 glyphWidth, out Int32 glyphHeight)
        {
            var glyphPosition = 0;
            var glyphCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var glyphSize = fontFace.MeasureGlyph(text, i);
                if (position >= glyphPosition && position < glyphPosition + glyphSize.Width)
                {
                    position = glyphPosition;
                    glyphWidth = glyphSize.Width;
                    glyphHeight = glyphSize.Height;
                    return glyphCount;
                }
                glyphPosition += glyphSize.Width;
                glyphCount++;
            }
            position = 0;
            glyphWidth = 0;
            glyphHeight = 0;
            return null;
        }
        
        /// <summary>
        /// Gets the index of the glyph or insertion point which is closest to the specified position in layout-relative space.
        /// </summary>
        private Int32? GetGlyphOrInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32? lineAtPosition, GlyphSearchMode searchMode)
        {
            var searchInsertionPoints = (searchMode == GlyphSearchMode.SearchInsertionPoints);
            var searchSnapToLine = (searchMode == GlyphSearchMode.SearchGlyphsSnapToLine);

            lineAtPosition = searchInsertionPoints ? 0 : (Int32?)null;

            if (input.Count == 0)
                return searchInsertionPoints ? 0 : (Int32?)null;

            if (searchInsertionPoints)
            {
                if (y < 0 || input.Count == 0)
                    return 0;
            }
            else
            {
                if (x < 0 || y < 0 || input.Count == 0)
                    return null;
            }

            var glyphCountSeen = 0;
            var glyphFound = false;
            var glyph = default(Int32?);
            var glyphBounds = Rectangle.Empty;
            var glyphWasLineBreak = false;

            var settings = input.Settings;
            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);

            var source = (input.SourceText.SourceString != null) ?
                new StringSource(input.SourceText.SourceString) :
                new StringSource(input.SourceText.SourceStringBuilder);
            
            input.Seek(0);

            var blockOffset = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;
            var offsetLineX = 0;
            var offsetLineY = blockOffset;

            // If our search point comes before the start of the block, then
            // the only possible answer is the first glyph in the block.
            if (y < blockOffset)
            {
                if (searchInsertionPoints)
                {
                    lineAtPosition = 0;
                    return 0;
                }
                lineAtPosition = null;
                return null;
            }

            // If our search point comes after the end of the block, then
            // the only possible answer is the last glyph in the block.
            if (y >= blockOffset + input.ActualHeight)
            {
                if (searchInsertionPoints)
                {
                    lineAtPosition = input.LineCount - 1;
                    return input.TotalLength;
                }
                lineAtPosition = null;
                return null;
            }

            var lineIndex = -1;
            var lineWidth = 0;
            var lineHeight = 0;
            var lineStartInGlyphs = 0;
            var lineLengthInCommands = 0;
            var lineLengthInGlyphs = 0;
            var lineIsTerminatedByLineBreak = false;

            input.SeekNextCommand();

            // If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines)
            {
                SkipToLineAtPosition(input, x, y, ref lineIndex, ref offsetLineX, ref offsetLineY, 
                    ref lineWidth, ref lineHeight, ref lineLengthInCommands, ref lineLengthInGlyphs, ref lineIsTerminatedByLineBreak, ref glyphCountSeen);
                input.SeekNextCommand();
                
                // If our search point comes before the beginning of the line that it's on,
                // then the only possible answer is the first glyph on the line.
                if (x < offsetLineX)
                {
                    lineAtPosition = lineIndex;
                    return (searchInsertionPoints || searchSnapToLine) ? glyphCountSeen : default(Int32?);
                }

                lineStartInGlyphs = glyphCountSeen;
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
                                lineAtPosition = lineIndex;
                                return (searchInsertionPoints || searchSnapToLine) ? 
                                    Math.Max(0, glyphCountSeen - (glyphWasLineBreak ? 1 : 0)) : default(Int32?);
                            }

                            ProcessLineInfo(input, ref lineIndex, ref offsetLineX, ref offsetLineY, 
                                ref lineWidth, ref lineHeight, ref lineLengthInCommands, ref lineLengthInGlyphs);

                            lineStartInGlyphs = glyphCountSeen;

                            // Determine whether we expect the glyph that we're searching for to be on the current
                            // line, then check to see if our search point comes before the begining of the line. If
                            // it does, then the only possible answer is the first glyph on the line.
                            glyphIsInCurrentLine = (y >= offsetLineY && y < offsetLineY + lineHeight);
                            if (glyphIsInCurrentLine)
                            {
                                lineAtPosition = lineIndex;

                                if (x < offsetLineX)
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
                                var tokenBounds = cmd->GetAbsoluteBounds(offsetLineX, blockOffset, lineHeight);
                                if (x >= tokenBounds.Left && x < tokenBounds.Right)
                                {
                                    var text = source.CreateStringSegmentFromSameSource(cmd->TextOffset, cmd->TextLength);
                                    var glyphPos = (x - tokenBounds.Left);
                                    var glyphWidth = 0;
                                    var glyphHeight = 0;
                                    glyph = glyphCountSeen + GetGlyphAtPositionWithinText(fontFace, ref text, ref glyphPos, out glyphWidth, out glyphHeight);
                                    glyphBounds = new Rectangle(tokenBounds.X + glyphPos, tokenBounds.Y, glyphWidth, glyphHeight);
                                    glyphFound = true;
                                }
                            }
                            glyphCountSeen += cmd->TextLength;
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            // If we expect the glyph to be on the current line, then compare our search position to
                            // the bounds of this icon command. If the icon command contains our search position, then
                            // the icon glyph must be our answer.
                            if (glyphIsInCurrentLine)
                            {
                                var iconCmd = (TextLayoutIconCommand*)input.Data;
                                var iconBounds = iconCmd->GetAbsoluteBounds(offsetLineX, blockOffset, lineHeight);
                                if (x >= iconBounds.Left && x < iconBounds.Right)
                                {
                                    glyph = glyphCountSeen;
                                    glyphBounds = iconBounds;
                                    glyphFound = true;
                                }
                            }
                            glyphCountSeen++;
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.LineBreak:
                        {
                            // Line breaks have no width, and therefore cannot contain the search position;
                            // skip past this command, but make a note of having seen it (after switch).
                            var cmd = (TextLayoutLineBreakCommand*)input.Data;
                            glyphCountSeen += cmd->Length;
                        }
                        input.SeekNextCommand();
                        break;

                    default:
                        {
                            var change = ProcessStylingCommand(input, cmdType, 
                                TextRendererStacks.Style | TextRendererStacks.Font | TextRendererStacks.Link, ref bold, ref italic, ref source);
                            if ((change & TextRendererStateChange.ChangeFont) == TextRendererStateChange.ChangeFont)
                            {
                                RefreshFont(ref settings, bold, italic, out font, out fontFace);
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
                if (glyph.HasValue)
                {
                    var max = (lineStartInGlyphs + lineLengthInGlyphs - (lineIsTerminatedByLineBreak ? 1 : 0));
                    return Math.Min(max, (x - glyphBounds.Center.X < 0) ? glyph.Value : glyph.Value + 1);
                }
                lineAtPosition = input.LineCount - 1;
                return input.TotalLength;
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
        private readonly Stack<TextStyleScoped<SpriteFont>> fontStack = new Stack<TextStyleScoped<SpriteFont>>();
        private readonly Stack<TextStyleScoped<Color>> colorStack = new Stack<TextStyleScoped<Color>>();
        private readonly Stack<TextStyleScoped<GlyphShader>> glyphShaderStack = new Stack<TextStyleScoped<GlyphShader>>();
        private readonly Stack<Int16> linkStack = new Stack<Int16>();
    }
}
