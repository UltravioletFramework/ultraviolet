using System;
using System.Collections.Generic;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents an engine for laying out formatted text.
    /// </summary>
    public sealed partial class TextLayoutEngine
    {
        /// <summary>
        /// Registers a style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to register.</param>
        /// <param name="style">The style to register.</param>
        public void RegisterStyle(String name, TextStyle style)
        {
            Contract.RequireNotEmpty(name, "name");

            registeredStyles.Add(name, style);
        }

        /// <summary>
        /// Registers the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register.</param>
        public void RegisterFont(String name, SpriteFont font)
        {
            Contract.RequireNotEmpty(name, "name");
            Contract.Require(font, "font");

            registeredFonts.Add(name, font);
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
            Contract.RequireNotEmpty(name, "name");
            Contract.Require(icon, "icon");

            registeredIcons.Add(name, new TextIconInfo(icon, width, height));
        }

        /// <summary>
        /// Registers the glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to register.</param>
        /// <param name="shader">The glyph shader to register.</param>
        public void RegisterGlyphShader(String name, GlyphShader shader)
        {
            Contract.RequireNotEmpty(name, "name");
            Contract.Require(shader, "shader");

            registeredGlyphShaders.Add(name, shader);
        }

        /// <summary>
        /// Unregisters the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to unregister.</param>
        /// <returns><c>true</c> if the style was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterStyle(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredStyles.Remove(name);
        }

        /// <summary>
        /// Unregisters the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to unregister.</param>
        /// <returns><c>true</c> if the font was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterFont(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredFonts.Remove(name);
        }

        /// <summary>
        /// Unregisters the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to unregister.</param>
        /// <returns><c>true</c> if the icon was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterIcon(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredIcons.Remove(name);
        }

        /// <summary>
        /// Unregisters the glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to unregister.</param>
        /// <returns><c>true</c> if the glyph shader was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterGlyphShader(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredGlyphShaders.Remove(name);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The parsed text which will be laid out according to the specified settings.</param>
        /// <param name="output">The layout command stream which will be populated with commands by this operation.</param>
        /// <param name="settings">A <see cref="TextLayoutSettings"/> structure which contains the settings for this operation.</param>
        public void CalculateLayout(TextParserTokenStream input, TextLayoutCommandStream output, TextLayoutSettings settings)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            if (settings.Font == null)
                throw new ArgumentException(UltravioletStrings.InvalidLayoutSettings);

            var state = new LayoutState();
            
            var index = 0;

            var bold = (settings.Style == SpriteFontStyle.Bold || settings.Style == SpriteFontStyle.BoldItalic);
            var italic = (settings.Style == SpriteFontStyle.Italic || settings.Style == SpriteFontStyle.BoldItalic);

            try
            {
                output.Clear();
                output.SourceText = input.SourceText;

                output.AcquirePointers();                
                output.WriteBlockInfo();
                output.WriteLineInfo();

                state.LineInfoCommandIndex = 1;

                var currentFont = settings.Font;
                var currentFontFace = settings.Font.GetFace(SpriteFontStyle.Regular);

                while (index < input.Count)
                {
                    var token = input[index];

                    currentFontFace = default(SpriteFontFace);
                    currentFont = GetCurrentFont(ref settings, bold, italic, out currentFontFace);

                    switch (token.TokenType)
                    {
                        case TextParserTokenType.Text:
                            {
                                if (token.IsNewLine)
                                {
                                    state.AdvanceToNextLine(output, ref settings);
                                }
                                else
                                {
                                    if (!AccumulateText(input, output, currentFontFace, ref index, ref state, ref settings))
                                        break;
                                }
                            }
                            break;

                        case TextParserTokenType.Icon:
                            {
                                var icon = default(TextIconInfo);
                                var iconIndex = RegisterIconWithCommandStream(output, token.Text, out icon);
                                var iconSize = MeasureToken(currentFont, token);

                                if (state.PositionX + iconSize.Width > (settings.Width ?? Int32.MaxValue))
                                    state.AdvanceToNextLine(output, ref settings);

                                if (state.PositionY + iconSize.Height > (settings.Height ?? Int32.MaxValue))
                                    break;

                                var iconBounds = new Rectangle(state.PositionX, state.PositionY, iconSize.Width, iconSize.Height);
                                output.WriteIcon(new TextLayoutIconCommand(iconIndex, icon.Width, icon.Height, iconBounds));
                                state.AdvanceToNextCommand(iconBounds.Width, iconBounds.Height, 1, false);
                            }
                            break;

                        case TextParserTokenType.ToggleBold:
                            {
                                output.WriteToggleBold();
                                state.AdvanceToNextCommand();
                                bold = !bold;
                            }
                            break;

                        case TextParserTokenType.ToggleItalic:
                            {
                                output.WriteToggleItalic();
                                state.AdvanceToNextCommand();
                                italic = !italic;
                            }
                            break;

                        case TextParserTokenType.PushFont:
                            {
                                var pushedFont = default(SpriteFont);
                                var pushedFontIndex = RegisterFontWithCommandStream(output, token.Text, out pushedFont);
                                output.WritePushFont(new TextLayoutFontCommand(pushedFontIndex));
                                state.AdvanceToNextCommand();
                                PushFont(pushedFont);
                            }
                            break;

                        case TextParserTokenType.PushColor:
                            {
                                var pushedColor = ParseColor(token.Text);
                                output.WritePushColor(new TextLayoutColorCommand(pushedColor));
                                state.AdvanceToNextCommand();
                            }
                            break;

                        case TextParserTokenType.PushStyle:
                            {
                                var pushedStyle = default(TextStyle);
                                var pushedStyleIndex = RegisterStyleWithCommandStream(output, token.Text, out pushedStyle);
                                output.WritePushStyle(new TextLayoutStyleCommand(pushedStyleIndex));
                                state.AdvanceToNextCommand();
                                PushStyle(pushedStyle, ref bold, ref italic);
                            }
                            break;

                        case TextParserTokenType.PushGlyphShader:
                            {
                                var pushedGlyphShader = default(GlyphShader);
                                var pushedGlyphShaderIndex = RegisterGlyphShaderWithCommandStream(output, token.Text, out pushedGlyphShader);
                                output.WritePushGlyphShader(new TextLayoutGlyphShaderCommand(pushedGlyphShaderIndex));
                                state.AdvanceToNextCommand();
                            }
                            break;

                        case TextParserTokenType.PopFont:
                            {
                                output.WritePopFont();
                                state.AdvanceToNextCommand();
                                PopFont();
                            }
                            break;

                        case TextParserTokenType.PopColor:
                            output.WritePopColor();
                            state.AdvanceToNextCommand();
                            break;

                        case TextParserTokenType.PopStyle:
                            {
                                output.WritePopStyle();
                                state.AdvanceToNextCommand();
                                PopStyle(ref bold, ref italic);
                            }
                            break;

                        case TextParserTokenType.PopGlyphShader:
                            output.WritePopGlyphShader();
                            state.AdvanceToNextCommand();
                            break;

                        default:
                            throw new InvalidOperationException("TODO");
                    }

                    index++;
                }

                if (state.LineLengthInCommands > 0)
                    state.AdvanceToNextLine(output, ref settings, false);

                state.WriteBlockInfo(output, (Int16)state.ActualWidth, (Int16)state.ActualHeight, state.LineCount, ref settings);

                output.Settings = settings;
                output.ActualWidth = state.ActualWidth;
                output.ActualHeight = state.ActualHeight;
                output.TotalLength = state.TotalLength;

                output.ReleasePointers();
            }
            finally
            {
                ClearLayoutStacks();
            }
        }

        /// <summary>
        /// Parses a <see cref="Color"/> value from the specified string segment.
        /// </summary>
        private static Color ParseColor(StringSegment text)
        {
            if (text.Length != 8)
                throw new FormatException();

            var a = (Int32)GetHexNumberValue(text.Substring(0, 2));
            var r = (Int32)GetHexNumberValue(text.Substring(2, 2));
            var g = (Int32)GetHexNumberValue(text.Substring(4, 2));
            var b = (Int32)GetHexNumberValue(text.Substring(6, 2));

            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Returns the numeric value associated with the specified hex digit.
        /// </summary>
        private static UInt32 GetHexDigitValue(Char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'A':
                case 'a':
                    return 10;
                case 'B':
                case 'b':
                    return 11;
                case 'C':
                case 'c':
                    return 12;
                case 'D':
                case 'd':
                    return 13;
                case 'E':
                case 'e':
                    return 14;
                case 'F':
                case 'f':
                    return 15;

                default:
                    throw new ArgumentException("c");
            }
        }

        /// <summary>
        /// Returns the numeric value associated with the specified hex number.
        /// </summary>
        private static UInt32 GetHexNumberValue(StringSegment str)
        {
            var value = 0u;
            var factor = 1u;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                var digit = GetHexDigitValue(str[i]);
                value += digit * factor;
                factor *= 16;
            }
            return value;
        }

        /// <summary>
        /// Clears all of the layout engine's layout parameter stacks.
        /// </summary>
        private void ClearLayoutStacks()
        {
            styleStack.Clear();
            fontStack.Clear();

            sourceString = null;
            sourceStringBuilder = null;
        }

        /// <summary>
        /// Pushes a style onto the style stack.
        /// </summary>
        /// <param name="style">The style to push onto the stack.</param>
        /// <param name="bold">A value indicating whether the current font face is bold.</param>
        /// <param name="italic">A value indicating whether the current font face is italic.</param>
        private void PushStyle(TextStyle style, ref Boolean bold, ref Boolean italic)
        {
            var instance = new TextStyleInstance(style, bold, italic);
            styleStack.Push(instance);

            if (style.Font != null)
                PushFont(style.Font);

            if (style.Bold.HasValue)
                bold = style.Bold.Value;

            if (style.Italic.HasValue)
                italic = style.Italic.Value;
        }

        /// <summary>
        /// Pushes a font onto the font stack.
        /// </summary>
        /// <param name="font">The font to push onto the stack.</param>
        private void PushFont(SpriteFont font)
        {
            var scope = styleStack.Count;
            fontStack.Push(new TextStyleScoped<SpriteFont>(font, scope));
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
            if (fontStack.Count == 0)
                return;

            var scope = styleStack.Count;
            if (fontStack.Peek().Scope != scope)
                return;

            fontStack.Pop();
        }

        /// <summary>
        /// Pops the current style scope off of the stacks.
        /// </summary>
        private void PopStyleScope()
        {
            var scope = styleStack.Count;

            while (fontStack.Count > 0 && fontStack.Peek().Scope == scope)
                fontStack.Pop();
        }

        /// <summary>
        /// Accumulates sequential text tokens into a single text command.
        /// </summary>
        private Boolean AccumulateText(TextParserTokenStream input, TextLayoutCommandStream output, SpriteFontFace font, ref Int32 index, ref LayoutState state, ref TextLayoutSettings settings)
        {
            var first = input[index];
            var firstSize = MeasureToken(font, first, GetNextTextToken(input, index));

            if (state.PositionX + firstSize.Width > (settings.Width ?? Int32.MaxValue))
                state.AdvanceToNextLine(output, ref settings);

            if (state.PositionY + firstSize.Height > (settings.Height ?? Int32.MaxValue))
                return false;

            if (first.IsWhiteSpace && state.LineLengthInText == 0)
                return true;

            var start = first.Text.Start;
            var length = first.Text.Length;

            var x = state.PositionX;
            var y = state.PositionY;
            var width = firstSize.Width;
            var height = firstSize.Height;

            if (!IsSegmentForCurrentSource(first.Text))
            {
                var isFirstSource = (sourceString == null && sourceStringBuilder == null);

                sourceString = first.Text.SourceString;
                sourceStringBuilder = first.Text.SourceStringBuilder;

                // NOTE: To save memory, we can elide the first change source command if it's just going to change to the input source.
                if (!isFirstSource || input.SourceText.SourceString != sourceString || input.SourceText.SourceStringBuilder != sourceStringBuilder)
                {
                    if (sourceString != null)
                    {
                        var sourceIndex = output.RegisterSourceString(sourceString);
                        output.WriteChangeSourceString(new TextLayoutSourceStringCommand(sourceIndex));
                    }
                    else
                    {
                        var sourceIndex = output.RegisterSourceStringBuilder(sourceStringBuilder);
                        output.WriteChangeSourceStringBuilder(new TextLayoutSourceStringBuilderCommand(sourceIndex));
                    }
                }
            }

            state.AdvanceToNextCommand(firstSize.Width, firstSize.Height, first.Text.Length, first.IsWhiteSpace);
            state.LineLengthInCommands--;

            while (index + 1 < input.Count)
            {
                var token = input[index + 1];
                if (token.TokenType != TextParserTokenType.Text || token.IsNewLine || !IsSegmentForCurrentSource(token.Text))
                    break;
                
                var tokenSize = MeasureToken(font, token, GetNextTextToken(input, index + 1));
                if (state.PositionX + tokenSize.Width > (settings.Width ?? Int32.MaxValue))
                    break;

                if (token.Text.Start != start + length)
                    break;

                width = width + tokenSize.Width;
                height = Math.Max(height, tokenSize.Height);
                length = length + token.Text.Length;

                state.AdvanceToNextCommand(tokenSize.Width, tokenSize.Height, token.Text.Length, token.IsWhiteSpace);
                state.LineLengthInCommands--;
                index++;
            }

            if (length > 0)
            {
                state.LineLengthInCommands++;
                output.WriteText(new TextLayoutTextCommand(start, length, new Rectangle(x, y, width, height)));
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified string segment uses the layout engine's current source string as its source.
        /// </summary>
        private Boolean IsSegmentForCurrentSource(StringSegment segment)
        {
            if (sourceString == null && sourceStringBuilder == null)
                return false;

            if (sourceString != null)
                return segment.SourceString == sourceString;

            return segment.SourceStringBuilder == sourceStringBuilder;
        }

        /// <summary>
        /// Calculates the size of the specified parser token when rendered according to the current layout state.
        /// </summary>
        /// <param name="font">The current font face.</param>
        /// <param name="tokenCurrent">The token to measure.</param>
        /// <param name="tokenNext">The token after <paramref name="tokenCurrent"/>, excluding command tokens.</param>
        /// <returns>The size of the specified token in pixels.</returns>
        private Size2 MeasureToken(SpriteFontFace font, TextParserToken tokenCurrent, TextParserToken? tokenNext = null)
        {
            switch (tokenCurrent.TokenType)
            {
                case TextParserTokenType.Icon:
                    {
                        TextIconInfo icon;
                        if (!registeredIcons.TryGetValue(tokenCurrent.Text, out icon))
                            throw new InvalidOperationException(UltravioletStrings.UnrecognizedIcon.Format(tokenCurrent.Text));

                        return new Size2(icon.Width ?? icon.Icon.Controller.Width, icon.Height ?? icon.Icon.Controller.Height);
                    }

                case TextParserTokenType.Text:
                    {
                        var size = font.MeasureString(tokenCurrent.Text);
                        if (tokenNext.HasValue)
                        {
                            var tokenNextValue = tokenNext.GetValueOrDefault();
                            if (tokenNextValue.TokenType == TextParserTokenType.Text && !tokenNextValue.Text.IsEmpty && !tokenNextValue.IsNewLine)
                            {
                                var charLast = tokenCurrent.Text[tokenCurrent.Text.Length - 1];
                                var charNext = tokenNextValue.Text[0];
                                var kerning = font.Kerning.Get(charLast, charNext);
                                return new Size2(size.Width + kerning, size.Height);
                            }
                        }
                        return size;
                    }
            }
            return Size2.Zero;
        }

        /// <summary>
        /// Registers the specified style with the command stream and returns its resulting index.
        /// </summary>
        public Int32 RegisterStyleWithCommandStream(TextLayoutCommandStream output, StringSegment name, out TextStyle style)
        {
            if (!registeredStyles.TryGetValue(name, out style))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedStyle.Format(name));

            return output.RegisterStyle(name, style);
        }

        /// <summary>
        /// Registers the specified icon with the command stream and returns its resulting index.
        /// </summary>
        private Int32 RegisterIconWithCommandStream(TextLayoutCommandStream output, StringSegment name, out TextIconInfo icon)
        {
            if (!registeredIcons.TryGetValue(name, out icon))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedIcon.Format(name));

            return output.RegisterIcon(name, icon);
        }

        /// <summary>
        /// Registers the specified font with the command stream and returns its resulting index.
        /// </summary>
        private Int32 RegisterFontWithCommandStream(TextLayoutCommandStream output, StringSegment name, out SpriteFont font)
        {
            if (!registeredFonts.TryGetValue(name, out font))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedFont.Format(name));

            return output.RegisterFont(name, font);
        }

        /// <summary>
        /// Registers the specified glyph shader with the command stream and returns its resulting index.
        /// </summary>
        private Int32 RegisterGlyphShaderWithCommandStream(TextLayoutCommandStream output, StringSegment name, out GlyphShader glyphShader)
        {
            if (!registeredGlyphShaders.TryGetValue(name, out glyphShader))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedGlyphShader.Format(name));

            return output.RegisterGlyphShader(name, glyphShader);
        }

        /// <summary>
        /// Gets the next text token after the specified index, if one exists and there are no intervening
        /// visible tokens (excluding commands).
        /// </summary>
        private TextParserToken? GetNextTextToken(TextParserTokenStream input, Int32 index)
        {
            for (int i = index + 1; i < input.Count; i++)
            {
                var token = input[i];
                if (token.TokenType == TextParserTokenType.Text)
                    return token;
                
                if (token.TokenType == TextParserTokenType.Icon)
                    break;
            }
            return null;
        }

        /// <summary>
        /// Gets the currently active font.
        /// </summary>
        private SpriteFont GetCurrentFont(ref TextLayoutSettings settings, Boolean bold, Boolean italic, out SpriteFontFace face)
        {
            var font = (fontStack.Count == 0) ? settings.Font : fontStack.Peek().Value;
            face = font.GetFace(bold, italic);
            return font;
        }

        // Registered styles, icons, fonts, and glyph shaders.
        private readonly Dictionary<StringSegment, TextStyle> registeredStyles =
            new Dictionary<StringSegment, TextStyle>();
        private readonly Dictionary<StringSegment, TextIconInfo> registeredIcons =
            new Dictionary<StringSegment, TextIconInfo>();
        private readonly Dictionary<StringSegment, SpriteFont> registeredFonts =
            new Dictionary<StringSegment, SpriteFont>();
        private readonly Dictionary<StringSegment, GlyphShader> registeredGlyphShaders =
            new Dictionary<StringSegment, GlyphShader>();

        // Layout parameter stacks.
        private readonly Stack<TextStyleInstance> styleStack = new Stack<TextStyleInstance>();
        private readonly Stack<TextStyleScoped<SpriteFont>> fontStack = new Stack<TextStyleScoped<SpriteFont>>();

        // The current source string.
        private String sourceString;
        private StringBuilder sourceStringBuilder;
    }
}
