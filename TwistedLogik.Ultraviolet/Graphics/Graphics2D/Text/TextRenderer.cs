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
        /// Gets the index of the line of text at the specified position relative to the layout area.
        /// </summary>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the line of text at the specified position relative to the layout area.</returns>
        public Int32 GetLineAtPosition(TextLayoutCommandStream input, Vector2 position)
        {
            return GetLineAtPosition(input, (Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the index of the line of text at the specified position relative to the layout area.
        /// </summary>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the line of text at the specified position relative to the layout area.</returns>
        public Int32 GetLineAtPosition(TextLayoutCommandStream input, Point2 position)
        {
            return GetLineAtPosition(input, position.X, position.Y);
        }

        /// <summary>
        /// Gets the index of the line of text at the specified position relative to the layout area.
        /// </summary>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the line of text at the specified position relative to the layout area.</returns>
        public Int32 GetLineAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the index of the glyph at the specified position relative to the layout area.
        /// </summary>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the glyph at the specified position relative to the layout area.</returns>
        public Int32 GetGlyphAtPosition(TextLayoutCommandStream input, Vector2 position)
        {
            return GetGlyphAtPosition(input, (Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified position relative to the layout area.
        /// </summary>
        /// <param name="position">The position to evaluate.</param>
        /// <returns>The index of the glyph at the specified position relative to the layout area.</returns>
        public Int32 GetGlyphAtPosition(TextLayoutCommandStream input, Point2 position)
        {
            return GetGlyphAtPosition(input, position.X, position.Y);
        }

        /// <summary>
        /// Gets the index of the glyph at the specified position relative to the layout area.
        /// </summary>
        /// <param name="x">The x-coordinate to evaluate.</param>
        /// <param name="y">The y-coordinate to evaluate.</param>
        /// <returns>The index of the glyph at the specified position relative to the layout area.</returns>
        public Int32 GetGlyphAtPosition(TextLayoutCommandStream input, Int32 x, Int32 y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a bounding box for the specified line, relative to the text layout area.
        /// </summary>
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
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <returns>A bounding box for the specified glyph, relative to the text layout area.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index)
        {
            Int32 lineHeight;
            return GetGlyphBounds(input, index, out lineHeight);
        }

        /// <summary>
        /// Gets a bounding box for the specified glyph, relative to the text layout area.
        /// </summary>
        /// <param name="index">The index of the glyph for which to retrieve a bounding box.</param>
        /// <param name="lineHeight">The height of the line that contains the specified glyph.</param>
        /// <returns>A bounding box for the specified glyph, relative to the text layout area.</returns>
        public Rectangle GetGlyphBounds(TextLayoutCommandStream input, Int32 index, out Int32 lineHeight)
        {
            Contract.Require(input, "input");
            Contract.EnsureRange(index >= 0 && index < input.TotalLength, "index");

            var boundsFound = false;
            var bounds = Rectangle.Empty;
            lineHeight = 0;

            var settings = input.Settings;

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            var glyphsSeen = 0;

            var sourceString = input.SourceText.SourceString;
            var sourceStringBuilder = input.SourceText.SourceStringBuilder;

            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);

            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);

            var positionX = 0;
            var positionY = ((TextLayoutBlockInfoCommand*)input.Data)->Offset;

            input.SeekNextCommand();

            for (int i = 1; i < input.Count && !boundsFound; i++)
            {
                var cmdType = *(TextLayoutCommandType*)input.Data;

                switch (cmdType)
                {
                    case TextLayoutCommandType.LineInfo:
                        {
                            var cmd = (TextLayoutLineInfoCommand*)input.Data;
                            positionX = cmd->Offset;
                            lineHeight = cmd->LineHeight;
                            input.SeekPastLineInfoCommand();
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);

                            var cmd = (TextLayoutTextCommand*)input.Data;
                            if (glyphsSeen + cmd->TextLength > index)
                            {
                                var cmdText = (sourceString != null) ?
                                    new StringSegment(sourceString, cmd->TextOffset, cmd->TextLength) :
                                    new StringSegment(sourceStringBuilder, cmd->TextOffset, cmd->TextLength);

                                var indexWithinText = index - glyphsSeen;

                                var glyphOffset = (indexWithinText == 0) ? 0 : fontFace.MeasureString(cmdText, 0, indexWithinText).Width;
                                var glyphSize = fontFace.MeasureGlyph(cmdText, indexWithinText);

                                bounds = new Rectangle(
                                    positionX + cmd->Bounds.X + glyphOffset, 
                                    positionY + cmd->Bounds.Y + ((lineHeight - cmd->Bounds.Height) / 2), glyphSize.Width, glyphSize.Height);
                                boundsFound = true;
                            }
                            glyphsSeen += cmd->TextLength;
                            input.SeekPastTextCommand();
                        }
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            var cmd = (TextLayoutIconCommand*)input.Data;
                            if (glyphsSeen + 1 > index)
                            {
                                bounds = new Rectangle(
                                    positionX + cmd->Bounds.X,
                                    positionY + cmd->Bounds.Y + ((lineHeight - cmd->Bounds.Height) / 2), cmd->Bounds.Width, cmd->Bounds.Height);
                                boundsFound = true;
                            }
                            glyphsSeen++;
                            input.SeekPastIconCommand();
                        }
                        break;

                    case TextLayoutCommandType.ToggleBold:
                        {
                            bold = !bold;
                            input.SeekPastToggleBoldCommand();
                        }
                        break;

                    case TextLayoutCommandType.ToggleItalic:
                        {
                            italic = !italic;
                            input.SeekPastToggleItalicCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushStyle:
                        {
                            var cmd = (TextLayoutStyleCommand*)input.Data;
                            var cmdStyle = input.GetStyle(cmd->StyleIndex);
                            PushStyle(cmdStyle, ref bold, ref italic);
                            input.SeekPastPushStyleCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushFont:
                        {
                            var cmd = (TextLayoutFontCommand*)input.Data;
                            var cmdFont = input.GetFont(cmd->FontIndex);
                            PushFont(cmdFont);
                            input.SeekPastPushFontCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushColor:
                        input.SeekPastPushColorCommand();
                        break;

                    case TextLayoutCommandType.PushGlyphShader:
                        input.SeekPastPushGlyphShaderCommand();
                        break;

                    case TextLayoutCommandType.PopStyle:
                        {
                            PopStyle(ref bold, ref italic);
                            input.SeekPastPopStyleCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopFont:
                        {
                            PopFont();
                            input.SeekPastPopFontCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopColor:
                        input.SeekPastPopFontCommand();
                        break;

                    case TextLayoutCommandType.PopGlyphShader:
                        input.SeekPastPopGlyphShaderCommand();
                        break;

                    case TextLayoutCommandType.ChangeSourceString:
                        {
                            var cmd = (TextLayoutSourceStringCommand*)input.Data;
                            sourceString = input.GetSourceString(cmd->SourceIndex);
                            sourceStringBuilder = null;
                        }
                        break;

                    case TextLayoutCommandType.ChangeSourceStringBuilder:
                        {
                            var cmd = (TextLayoutSourceStringBuilderCommand*)input.Data;
                            sourceString = null;
                            sourceStringBuilder = input.GetSourceStringBuilder(cmd->SourceIndex);
                        }
                        break;

                    default:
                        if (i < input.Count)
                        {
                            input.Seek(i + 1);
                        }
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
            
            var parserOptions = settings.GetParserOptions();
            parser.Parse(input, parserResult, parserOptions);

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
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, String input, Vector2 position, Color defaultColor, Int32 start, Int32 count, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");
            
            var parserOptions = settings.GetParserOptions();
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

            var parserOptions = settings.GetParserOptions();
            parser.Parse(input, parserResult, parserOptions);

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
        /// <param name="settings">The settings which are passed to the text layout engine.</param>
        /// <returns>A <see cref="RectangleF"/> which represents the bounding box of the formatted text.</returns>
        public RectangleF Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 position, Color defaultColor, Int32 start, Int32 count, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            var parserOptions = settings.GetParserOptions();
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

            var end = (count == Int32.MaxValue) ? Int32.MaxValue : start + count - 1;

            var settings = input.Settings;
            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);
            var font = settings.Font;
            var fontFace = font.GetFace(bold, italic);
            var color = defaultColor;

            var blockOffset = 0;
            var lineOffset = 0;
            var lineHeight = 0;
            var charsSeen = 0;

            var source = new StringSource(input.SourceText);

            var acquiredPointers = !input.HasAcquiredPointers;
            if (acquiredPointers)
                input.AcquirePointers();

            input.Seek(0);

            for (int i = 0; i < input.Count; i++)
            {
                if (charsSeen >= end)
                    break;

                var cmdType = *(TextLayoutCommandType*)input.Data;
                switch (cmdType)
                {
                    case TextLayoutCommandType.BlockInfo:
                        {
                            var cmd = (TextLayoutBlockInfoCommand*)input.Data;
                            blockOffset = cmd->Offset;
                            input.SeekPastBlockInfoCommand();
                        }
                        break;

                    case TextLayoutCommandType.LineInfo:
                        {
                            var cmd = (TextLayoutLineInfoCommand*)input.Data;
                            lineOffset = cmd->Offset;
                            lineHeight = cmd->LineHeight;
                            input.SeekPastLineInfoCommand();
                        }
                        break;

                    case TextLayoutCommandType.Text:
                        {
                            var cmd = (TextLayoutTextCommand*)input.Data;
                            var cmdText = source.CreateStringSegmentFromSubstring(cmd->TextOffset, cmd->TextLength);
                            var cmdTextOriginalLength = cmdText.Length;

                            if (charsSeen + cmdTextOriginalLength > start)
                            {
                                var tokenOffset = 0;
                                var tokenStart = charsSeen;
                                var tokenEnd = tokenStart + cmdText.Length - 1;

                                if ((tokenStart < start && tokenEnd >= start) || (tokenStart <= end && tokenEnd > end))
                                {
                                    var subStart = (charsSeen > start) ? 0 : start - charsSeen;
                                    var subEnd = Math.Min(end, charsSeen + cmdText.Length - 1) - charsSeen;
                                    var subLength = 1 + (subEnd - subStart);
                                    tokenOffset = (subStart == 0) ? 0 : fontFace.MeasureString(cmdText, 0, subStart).Width;
                                    cmdText = cmdText.Substring(subStart, subLength);
                                }

                                var cmdPosition = new Vector2(
                                    position.X + cmd->Bounds.X + lineOffset + tokenOffset,
                                    position.Y + cmd->Bounds.Y + blockOffset + ((lineHeight - cmd->Bounds.Height) / 2));
                                var cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, charsSeen, input.TotalLength);
                                spriteBatch.DrawString(cmdGlyphShaderContext, fontFace, cmdText, cmdPosition, color);
                            }

                            charsSeen += cmdTextOriginalLength;
                            input.SeekPastTextCommand();
                        }
                        break;

                    case TextLayoutCommandType.Icon:
                        {
                            var cmd = (TextLayoutIconCommand*)input.Data;
                            
                            if (charsSeen + 1 > start)
                            {
                                var cmdIcon = input.GetIcon(cmd->IconIndex);
                                var cmdPositionX = position.X + lineOffset + cmd->Bounds.X;
                                var cmdPositionY = position.Y + blockOffset + cmd->Bounds.Y + ((lineHeight - cmd->Bounds.Height) / 2);
                                var cmdGlyphShaderContext = (glyphShaderStack.Count == 0) ? GlyphShaderContext.Invalid : new GlyphShaderContext(glyphShaderStack, charsSeen, input.TotalLength);
                                if (cmdGlyphShaderContext.IsValid)
                                {
                                    var glyphColor = color;
                                    cmdGlyphShaderContext.Execute('\x0000', ref cmdPositionX, ref cmdPositionY, ref glyphColor, charsSeen);
                                }
                                spriteBatch.DrawSprite(cmdIcon.Icon.Controller, new Vector2(cmdPositionX, cmdPositionY), cmdIcon.Width, cmdIcon.Height, color, 0f);
                            }

                            charsSeen += 1;
                            input.SeekPastIconCommand();
                        }
                        break;

                    case TextLayoutCommandType.ToggleBold:
                        {
                            bold = !bold;
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            input.SeekPastToggleBoldCommand();
                        }
                        break;

                    case TextLayoutCommandType.ToggleItalic:
                        {
                            italic = !italic;
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            input.SeekPastToggleItalicCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushStyle:
                        {
                            var cmd = (TextLayoutStyleCommand*)input.Data;
                            PushStyle(input.GetStyle(cmd->StyleIndex), ref bold, ref italic);
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            RefreshColor(defaultColor, out color);
                            input.SeekPastPushStyleCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushFont:
                        {
                            var cmd = (TextLayoutFontCommand*)input.Data;
                            var cmdFont = input.GetFont(cmd->FontIndex);
                            PushFont(cmdFont);
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            input.SeekPastPushFontCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushColor:
                        {
                            var cmd = (TextLayoutColorCommand*)input.Data;
                            var cmdColor = cmd->Color;
                            PushColor(cmdColor);
                            RefreshColor(defaultColor, out color);
                            input.SeekPastPushColorCommand();
                        }
                        break;

                    case TextLayoutCommandType.PushGlyphShader:
                        {
                            var cmd = (TextLayoutGlyphShaderCommand*)input.Data;
                            PushGlyphShader(input.GetGlyphShader(cmd->GlyphShaderIndex));
                            input.SeekPastPushGlyphShaderCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopStyle:
                        {
                            PopStyle(ref bold, ref italic);
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            RefreshColor(defaultColor, out color);
                            input.SeekPastPopStyleCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopFont:
                        {
                            PopFont();
                            RefreshFont(ref settings, bold, italic, out font, out fontFace);
                            input.SeekPastPopFontCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopColor:
                        {
                            PopColor();
                            RefreshColor(defaultColor, out color);
                            input.SeekPastPopColorCommand();
                        }
                        break;

                    case TextLayoutCommandType.PopGlyphShader:
                        {
                            PopGlyphShader();
                            input.SeekPastPopGlyphShaderCommand();
                        }
                        break;

                    case TextLayoutCommandType.ChangeSourceString:
                        {
                            var cmd = (TextLayoutSourceStringCommand*)input.Data;
                            source = new StringSource(input.GetSourceString(cmd->SourceIndex));
                            input.SeekPastChangeSourceStringCommand();
                        }
                        break;

                    case TextLayoutCommandType.ChangeSourceStringBuilder:
                        {
                            var cmd = (TextLayoutSourceStringBuilderCommand*)input.Data;
                            source = new StringSource(input.GetSourceStringBuilder(cmd->SourceIndex));
                            input.SeekPastChangeSourceStringBuilderCommand();
                        }
                        break;

                    default:
                        if (i < input.Count)
                        {
                            input.Seek(i + 1);
                        }
                        break;
                }
            }

            if (acquiredPointers)
                input.ReleasePointers();

            ClearLayoutStacks();

            return new RectangleF(position.X + input.Bounds.X, position.Y + input.Bounds.Y, input.Bounds.Width, input.Bounds.Height);
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
