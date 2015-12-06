using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Contains methods for rendering formatted text.
    /// </summary>
    [SecuritySafeCritical]
    public sealed unsafe class TextRenderer
    {
        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="stretch">If <c>true</c>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <c>null</c> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Vector2 position, Boolean stretch = false)
        {
            return GetLineAtPosition(input, (Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the index of the line of text at the specified layout-relative position.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="position">The position to evaluate.</param>
        /// <param name="stretch">If <c>true</c>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <c>null</c> if the specified position is not contained by any line.</returns>
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
        /// <param name="stretch">If <c>true</c>, a line is considered to fill the entire horizontal extent of the 
        /// layout area, regardless of the line's actual width.</param>
        /// <returns>The index of the line of text at the specified layout-relative position, 
        /// or <c>null</c> if the specified position is not contained by any line.</returns>
        public Int32? GetLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, Boolean stretch = false)
        {
            Contract.Require(input, "input");

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
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, out lineAtPosition);
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
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y, out lineAtPosition);
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
            return GetGlyphAtPosition(input, position.X, position.Y, out lineAtPosition);
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
            return GetGlyphAtPosition(input, position.X, position.Y, out lineAtPosition);
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
            return GetGlyphAtPosition(input, x, y, out lineAtPosition);
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
            Contract.Require(input, "input");

            return GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPosition, false);
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
            Contract.Require(input, "input");

            Int32? lineAtPosition;
            return GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPosition, true) ?? 0;
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
            Contract.Require(input, "input");

            var lineAtPositionTemp = default(Int32?);
            var result = GetGlyphOrInsertionPointAtPosition(input, x, y, out lineAtPositionTemp, true) ?? 0;

            lineAtPosition = lineAtPositionTemp ?? 0;

            return result;
        }

        /// <summary>
        /// Gets a bounding box for the specified line, relative to the text layout area.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the line for which to retrieve a bounding box.</param>
        /// <returns>A bounding box for the specified line, relative to the text layout area.</returns>
        public Rectangle GetLineBounds(TextLayoutCommandStream input, Int32 index)
        {
            Contract.Require(input, "input");
            Contract.EnsureRange(index >= 0 && index < input.LineCount, "index");

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
        /// Gets a bounding box for the specified glyph, relative to the text layout area.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <returns>A bounding box for the specified glyph, relative to the text layout area.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index)
        {
            Int32 lineWidth, lineHeight;
            return GetGlyphBounds(input, index, out lineWidth, out lineHeight);
        }

        /// <summary>
        /// Gets a bounding box for the specified glyph, relative to the text layout area.
        /// </summary>
        /// <param name="input">The command stream that contains the layout information to evaluate.</param>
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <param name="lineWidth">The width of the line that contains the specified glyph.</param>
        /// <param name="lineHeight">The height of the line that contains the specified glyph.</param>
        /// <returns>A bounding box for the specified glyph, relative to the text layout area.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index, out Int32 lineWidth, out Int32 lineHeight)
        {
            Contract.Require(input, "input");
            Contract.EnsureRange(index >= 0 && index < input.TotalLength, "index");

            var glyphCountSeen = 0;

            var boundsOnLineBreak = false;
            var boundsFound = false;
            var bounds = Rectangle.Empty;

            var lineIndex = -1;
            lineWidth = 0;
            lineHeight = 0;

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
                SkipToLineContainingGlyph(input, index, ref glyphCountSeen);

            // Seek through the remaining commands until we find the one that contains our glyph.
            while (!boundsFound && input.StreamPositionInObjects < input.Count)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;
                if (cmdType != TextLayoutCommandType.LineInfo && boundsOnLineBreak)
                    throw new InvalidOperationException(UltravioletStrings.LineBreakNotFollowedByNewLine);

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        {
                            ProcessLineInfo(input, ref lineIndex, ref offsetLineX, ref offsetLineY, ref lineWidth, ref lineHeight);
                            if (boundsOnLineBreak)
                            {
                                boundsFound = true;
                                bounds = new Rectangle(offsetLineX, blockOffset + offsetLineY, 0, lineHeight);
                            }
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
                                var glyphPosition = cmd->GetAbsolutePosition(offsetLineX + glyphOffset, blockOffset, lineHeight);

                                bounds = new Rectangle(glyphPosition, glyphSize);
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
                                var glyphPosition = cmd->GetAbsolutePosition(offsetLineX, blockOffset, lineHeight);
                                bounds = new Rectangle(glyphPosition, cmd->Bounds.Size);
                                boundsFound = true;
                            }
                        }
                        input.SeekNextCommand();
                        break;

                    case TextLayoutCommandType.LineBreak:
                        {
                            if (++glyphCountSeen > index)
                                boundsOnLineBreak = true;
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

            return bounds;
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
        /// <returns><c>true</c> if the style was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterStyle(String name)
        {
            return layoutEngine.UnregisterStyle(name);
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
        /// <returns><c>true</c> if the font was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterFont(String name)
        {
            return layoutEngine.UnregisterFont(name);
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
        /// <returns><c>true</c> if the icon was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterIcon(String name)
        {
            return layoutEngine.UnregisterIcon(name);
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
        /// <returns><c>true</c> if the glyph shader was unregistered; otherwise, <c>false</c>.</returns>
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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

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
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            return DrawInternal(spriteBatch, input, position, defaultColor, start, count);
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

            var availableHeight = settings.Height ?? Int32.MaxValue;
            var blockOffset = 0;
            var lineIndex = -1;
            var lineOffset = 0;
            var linePosition = 0;
            var lineWidth = 0;
            var lineHeight = 0;

            var charsSeen = 0;
            var charsMax = (count == Int32.MaxValue) ? Int32.MaxValue : start + count - 1;

            var source = new StringSource(input.SourceText);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

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
                            ProcessLineInfo(input, ref lineIndex, ref lineOffset, ref linePosition, ref lineWidth, ref lineHeight);
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
                            position.X + lineOffset, position.Y + blockOffset, lineHeight, start, count, color, ref charsSeen);
                        break;
                        
                    case TextLayoutCommandType.LineBreak:
                        {
                            charsSeen++;
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
                            if ((change & TextRendererStateChange.ChangeColor) == TextRendererStateChange.ChangeColor)
                            {
                                RefreshColor(defaultColor, out color);
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
                charsSeen += 1;
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

                spriteBatch.DrawSprite(icon.Icon.Controller, iconPosition, iconWidth, iconHeight, color, iconRotation);
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
        private void RefreshColor(Color defaultColor, out Color color)
        {
            color = (colorStack.Count == 0) ? defaultColor : colorStack.Peek().Value;
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
        private void ProcessLineInfo(TextLayoutCommandStream input, ref Int32 index, ref Int32 offset, ref Int32 position, ref Int32 width, ref Int32 height)
        {
            var cmd = (TextLayoutLineInfoCommand*)input.Data;
            index++;
            offset = cmd->Offset;
            position = position + height;
            width = cmd->LineWidth;
            height = cmd->LineHeight;
            input.SeekNextCommand();
        }

        /// <summary>
        /// Moves the specified command stream forward to the beginning of the line that contains the specified coordinates.
        /// </summary>
        private void SkipToLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y,
            ref Int32 lineIndex, ref Int32 linePosition, ref Int32 lineWidth, ref Int32 lineHeight, ref Int32 glyphCountSeen)
        {
            do
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;
                lineIndex++;
                lineWidth = cmd->LineWidth;
                lineHeight = cmd->LineHeight;

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
        private void SkipToLineContainingGlyph(TextLayoutCommandStream input, Int32 glyph, ref Int32 glyphCountSeen)
        {
            while (true)
            {
                var cmd = (TextLayoutLineInfoCommand*)input.Data;
                if (glyphCountSeen + cmd->LengthInGlyphs > glyph)
                    break;

                glyphCountSeen += cmd->LengthInGlyphs;
                input.SeekNextLine();
            }
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
                    glyphWidth = glyphSize.Width;
                    glyphHeight = glyphSize.Height;
                    return glyphCount;
                }
                glyphPosition += glyphSize.Width;
                glyphCount++;
            }
            glyphWidth = 0;
            glyphHeight = 0;
            return null;
        }

        /// <summary>
        /// Gets the index of the glyph or insertion point which is closest to the specified position in layout-relative space.
        /// </summary>
        private Int32? GetGlyphOrInsertionPointAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y, out Int32? lineAtPosition, Boolean getInsertionPoint)
        {
            lineAtPosition = null;

            if (getInsertionPoint)
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

            if (y < blockOffset)
            {
                if (getInsertionPoint)
                {
                    lineAtPosition = 0;
                    return 0;
                }
                lineAtPosition = null;
                return null;
            }

            if (y >= blockOffset + input.ActualHeight)
            {
                if (getInsertionPoint)
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

            input.SeekNextCommand();

            // NOTE: If we only have a single font style, we can optimize by entirely skipping past lines prior to the one
            // that contains the position we're interested in, because we don't need to process any commands that those lines contain.
            var canSkipLines = !input.HasMultipleFontStyles;
            if (canSkipLines)
            {
                SkipToLineAtPosition(input, x, y, ref lineIndex, ref offsetLineY, ref lineWidth, ref lineHeight, ref glyphCountSeen);
                input.SeekNextCommand();
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
                            if (glyphIsInCurrentLine)
                                return getInsertionPoint ? glyphCountSeen : (Int32?)null;

                            ProcessLineInfo(input, ref lineIndex, ref offsetLineX, ref offsetLineY, ref lineWidth, ref lineHeight);
                            glyphIsInCurrentLine = (y >= offsetLineY && y < offsetLineY + lineHeight);

                            if (glyphIsInCurrentLine)
                                lineAtPosition = lineIndex;
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
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
                            glyphCountSeen++;
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

            if (getInsertionPoint)
            {
                if (glyph.HasValue)
                {
                    return (x - glyphBounds.Center.X < 0) ? glyph.Value : glyph.Value + 1;
                }
                else
                {
                    lineAtPosition = input.LineCount - 1;
                    return input.TotalLength;
                }
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
    }
}
