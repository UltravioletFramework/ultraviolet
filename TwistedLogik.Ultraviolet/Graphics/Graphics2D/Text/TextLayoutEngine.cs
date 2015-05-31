using System;
using System.Collections.Generic;
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
        /// Unregisters the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to unregister.</param>
        /// <returns><c>true</c> if the style was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterStyle(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredStyles.Remove(name);
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
        /// Unregisters the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to unregister.</param>
        /// <returns><c>true</c> if the font was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterFont(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredFonts.Remove(name);
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

            registeredIcons.Add(name, new InlineIconInfo(icon, width, height));
        }

        /// <summary>
        /// Unregisters the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to unregister.</param>
        /// <returns><c>true</c> if the icon was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterIcon(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return registeredIcons.Remove(name);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The parsed text to lay out.</param>
        /// <param name="output">The formatted text with layout information.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(TextParserResult input, TextLayoutResult output, TextLayoutSettings settings)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            ValidateSettings(ref settings);

            Reset(input, output, settings);
            this.output.Clear();

            for (int i = 0; i < input.Count; i++)
            {
                var thisToken = input[i];
                var nextToken = (i + 1 < input.Count) ? (TextParserToken?)input[i + 1] : null;

                TextStyle tokenStyleBase;
                TextStyle tokenStyleActive;
                GetStyle(thisToken.Style, out tokenStyleBase, out tokenStyleActive);

                var tokenFont = (thisToken.Style == 0) ? settings.Font.GetFace(settings.Style) : 
                    GetFontFace(ref tokenStyleBase, ref tokenStyleActive);

                if (thisToken.IsNewLine)
                {
                    AdvanceLine(tokenFont);
                    continue;
                }

                var tokenIcon = GetIcon(ref tokenStyleActive);
                var tokenSize = MeasureToken(thisToken.Text, tokenFont, tokenIcon, nextToken);

                if (settings.Width.HasValue && cx + tokenSize.Width > settings.Width.Value)
                {
                    AdvanceLine(tokenFont);
                }

                if (settings.Height.HasValue && cy + tokenSize.Height > settings.Height.Value)
                {
                    break;
                }

                var tokenBounds = new Rectangle(cx, cy, tokenSize.Width, tokenSize.Height);
                var tokenColor = tokenStyleActive.Color ?? tokenStyleBase.Color;

                var result = new TextLayoutToken(thisToken.Text, tokenBounds, tokenFont, tokenIcon, tokenColor);
                if (AccumulateToken(ref result, thisToken.IsWhiteSpace, nextToken))
                {
                    AdvanceToken(tokenSize, thisToken.IsWhiteSpace);
                }
            }
            AdvanceLine(null);
            PerformFinalLayoutAdjustmentOnText();

            Clear();
        }

        /// <summary>
        /// Validates a set of layout settings.
        /// </summary>
        /// <param name="settings">The layout settings to validate.</param>
        private static void ValidateSettings(ref TextLayoutSettings settings)
        {
            if (settings.Font == null)
                throw new ArgumentException(UltravioletStrings.InvalidLayoutSettings);
        }

        /// <summary>
        /// Measures the specified token.
        /// </summary>
        /// <param name="text">The token's text.</param>
        /// <param name="font">The token's font.</param>
        /// <param name="icon">The token's icon.</param>
        /// <param name="next">The next token in the input stream.</param>
        /// <returns>The token's size in pixels.</returns>
        private static Size2 MeasureToken(StringSegment text, SpriteFontFace font, InlineIconInfo? icon, TextParserToken? next)
        {
            if (icon != null)
            {
                var iconInfo  = icon.Value;
                var animation = iconInfo.Icon;
                return new Size2(iconInfo.Width ?? animation.Controller.Width, iconInfo.Height ?? animation.Controller.Height);
            }
            
            var size = font.MeasureString(text);
            if (next.HasValue)
            {
                var nextValue = next.Value;
                if (!nextValue.Text.IsEmpty && !nextValue.IsNewLine)
                {
                    var charLast = text[text.Length - 1];
                    var charNext = nextValue.Text[0];
                    var kerning = font.Kerning.Get(charLast, charNext);
                    return new Size2(size.Width + kerning, size.Height);
                }
            }
            return size;
        }

        /// <summary>
        /// Clears the layout engine's state.
        /// </summary>
        private void Clear()
        {
            Reset(null, null, default(TextLayoutSettings));
        }

        /// <summary>
        /// Resets the layout engine's state.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="output">The output stream.</param>
        /// <param name="settings">The layout settings.</param>
        private void Reset(TextParserResult input, TextLayoutResult output, TextLayoutSettings settings)
        {
            this.input = input;
            this.output = output;
            this.settings = settings;

            this.cx = 0;
            this.cy = 0;

            this.lineStart      = 0;
            this.lineWidth      = 0;
            this.lineHeight     = 0;
            this.lineWhiteSpace = 0;

            this.textMinX        = Int32.MaxValue;
            this.textMinY        = 0;
            this.textTotalWidth  = 0;
            this.textTotalHeight = 0;

            this.accumulator = null;
        }

        /// <summary>
        /// Advances the layout position by one line.
        /// </summary>
        /// <param name="font">The font associated with the newline token, if there is one.</param>
        private void AdvanceLine(SpriteFontFace font)
        {
            FlushAccumulatedTokens();
            PerformFinalLayoutAdjustmentOnLine();

            if (lineHeight == 0 && font != null)
            {
                lineHeight = font.LineSpacing;
            }

            cx = 0;
            cy = cy + lineHeight;

            textTotalWidth = Math.Max(textTotalWidth, lineWidth);
            textTotalHeight = textTotalHeight + lineHeight;

            lineWidth = 0;
            lineHeight = 0;
            lineWhiteSpace = 0;
            lineStart = output.Count;

            accumulator = null;
        }

        /// <summary>
        /// Advances the layout position by one token.
        /// </summary>
        /// <param name="tokenSize">The size of the current token.</param>
        /// <param name="tokenIsWhiteSpace">A value indicating whether the current token is white space.</param>
        private void AdvanceToken(Size2 tokenSize, Boolean tokenIsWhiteSpace)
        {
            if (tokenSize.Height > lineHeight)
            {
                lineHeight = tokenSize.Height;
            }
            lineWhiteSpace = tokenIsWhiteSpace ? lineWhiteSpace + tokenSize.Width : 0;
            cx = cx + tokenSize.Width;
        }

        /// <summary>
        /// Accumulates the specified token into the line buffer.
        /// Tokens with identical styles are consolidated into single tokens as a result of this process.
        /// </summary>
        /// <param name="token">The token to accumulate into the line buffer.</param>
        /// <param name="tokenIsWhiteSpace">A value indicating whether the current token is white space.</param>
        /// <param name="next">The next token in the input stream.</param>
        /// <returns><c>true</c> if the token was accumulated; otherwise, <c>false</c>.</returns>
        private bool AccumulateToken(ref TextLayoutToken token, Boolean tokenIsWhiteSpace, TextParserToken? next)
        {
            if (accumulator == null)
            {
                if (tokenIsWhiteSpace)
                {
                    return false;
                }
                accumulator = token;
            }
            else
            {
                var existing = accumulator.Value;
                if (!StringSegment.AreSegmentsContiguous(existing.Text, token.Text) || !token.MatchesStyleRef(ref existing))
                {
                    FlushAccumulatedTokens();
                    accumulator = token;
                }
                else
                {
                    var text = StringSegment.CombineSegments(existing.Text, token.Text);
                    var size = MeasureToken(text, token.FontFace, token.Icon, next);
                    var bounds = new Rectangle(existing.Bounds.X, existing.Bounds.Y, size.Width, size.Height);
                    accumulator = new TextLayoutToken(text, bounds, token.FontFace, token.Icon, token.Color);
                }
            }
            lineWidth += token.Bounds.Width;
            return true;
        }

        /// <summary>
        /// Flushes any accumulated tokens to the output stream.
        /// </summary>
        private void FlushAccumulatedTokens()
        {
            if (accumulator == null)
                return;

            output.Add(accumulator.Value);
            accumulator = null;
        }

        /// <summary>
        /// Performs the final layout pass on the current line, adjusting the positions of
        /// its tokens to be centered on the line and to match the layout settings.
        /// </summary>
        private void PerformFinalLayoutAdjustmentOnLine()
        {
            lineWidth -= lineWhiteSpace;

            // Determine the line's starting position.
            var tokenPosition = 0;
            if ((settings.Flags & TextFlags.AlignCenter) == TextFlags.AlignCenter)
            {
                if (settings.Width.HasValue)
                {
                    tokenPosition = (settings.Width.Value - lineWidth) / 2;
                }
            }
            if ((settings.Flags & TextFlags.AlignRight) == TextFlags.AlignRight)
            {
                if (settings.Width.HasValue)
                {
                    tokenPosition = (settings.Width.Value - lineWidth);
                }
            }

            // Reposition the line's tokens.
            for (int i = lineStart; i < output.Count; i++)
            {
                var token = output[i];
                var tokenWidth = token.Bounds.Width;
                var tokenHeight = token.Bounds.Height;
                var tokenX = tokenPosition;
                var tokenY = cy + ((lineHeight - tokenHeight) / 2);
                if (tokenX < textMinX)
                {
                    textMinX = tokenX;
                }
                token.Bounds = new Rectangle(tokenX, tokenY, tokenWidth, tokenHeight);
                tokenPosition = tokenPosition + tokenWidth;
                output[i] = token;
            }
        }

        /// <summary>
        /// Performs the final layout pass on the entire block of formatted text,
        /// adjusting the positions of its lines according to the layout settings.
        /// </summary>
        private void PerformFinalLayoutAdjustmentOnText()
        {
            output.Settings     = settings;
            output.ActualWidth  = textTotalWidth;
            output.ActualHeight = textTotalHeight;

            // Determine the text's starting position.
            var tokenPosition = 0;
            if ((settings.Flags & TextFlags.AlignMiddle) == TextFlags.AlignMiddle)
            {
                if (settings.Height.HasValue)
                {
                    tokenPosition = (settings.Height.Value - textTotalHeight) / 2;
                }
            }
            else if ((settings.Flags & TextFlags.AlignBottom) == TextFlags.AlignBottom)
            {
                if (settings.Height.HasValue)
                {
                    tokenPosition = (settings.Height.Value - textTotalHeight);
                }
            }
            else
            {
                output.Bounds = GetLayoutBounds();
                return;
            }

            // Reposition the text's tokens.
            textMinY = Int32.MaxValue;
            for (int i = 0; i < output.Count; i++)
            {
                var token = output[i];
                var tokenBounds = token.Bounds;
                var tokenWidth = tokenBounds.Width;
                var tokenHeight = tokenBounds.Height;
                var tokenX = tokenBounds.X;
                var tokenY = tokenPosition + tokenBounds.Y;
                if (tokenY < textMinY)
                {
                    textMinY = tokenY;
                }
                token.Bounds = new Rectangle(tokenX, tokenY, tokenWidth, tokenHeight);
                output[i] = token;
            }
            output.Bounds = GetLayoutBounds();
        }

        /// <summary>
        /// Gets the style that corresponds to the specifed style index.
        /// </summary>
        /// <param name="index">The index of the style to retrieve.</param>
        /// <param name="tokenStyleBase">The base style for this token.</param>
        /// <param name="tokenStyleActive">The active style for this token.</param>
        /// <returns>That style that corresponds to the specified style index.</returns>
        private void GetStyle(Int32 index, out TextStyle tokenStyleBase, out TextStyle tokenStyleActive)
        {
            if (index < 0 || index >= input.Styles.Count)
                throw new InvalidOperationException(UltravioletStrings.InvalidToken);

            tokenStyleBase = input.Styles[index];
            tokenStyleActive = tokenStyleBase;

            var preset = tokenStyleBase.Style;
            if (preset != null && tokenStyleBase.Icon == null)
            {
                if (!registeredStyles.TryGetValue(preset.Value, out tokenStyleActive))
                {
                    throw new ArgumentException(UltravioletStrings.UnrecognizedStyle.Format(preset.Value));
                }
            }
        }

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <param name="tokenStyleActive">The base style for the token.</param>
        /// <param name="tokenStyleBase">The active style for the token.</param>
        /// <returns>The font face that corresponds to the specified style.</returns>
        private SpriteFontFace GetFontFace(ref TextStyle tokenStyleBase, ref TextStyle tokenStyleActive)
        {
            SpriteFont font;

            var name = tokenStyleActive.Font ?? tokenStyleBase.Font;
            var bold = tokenStyleActive.Bold ?? tokenStyleBase.Bold ?? false;
            var italic = tokenStyleActive.Italic ?? tokenStyleBase.Italic ?? false;

            if (name.HasValue)
            {
                if (registeredFonts.TryGetValue(name.Value, out font))
                {
                    return font.GetFace(bold, italic);
                }
                throw new ArgumentException(UltravioletStrings.UnrecognizedFont.Format(name));
            }
            return settings.Font.GetFace(bold, italic);
        }

        /// <summary>
        /// Gets the icon that corresponds to the specified style.
        /// </summary>
        /// <param name="style">The style for which to retrieve an icon.</param>
        /// <returns>The icon that corresponds to the specified style.</returns>
        private InlineIconInfo? GetIcon(ref TextStyle style)
        {
            InlineIconInfo icon;
            if (style.Icon.HasValue)
            {
                if (registeredIcons.TryGetValue(style.Icon.Value, out icon))
                {
                    return icon;
                }
                throw new ArgumentException(UltravioletStrings.UnrecognizedIcon.Format(style.Icon.Value));
            }
            return null;
        }

        /// <summary>
        /// Gets the bounds of the laid-out text relative to the layout area..
        /// </summary>
        /// <returns>The bounds of the laid-out text relative to the layout area.</returns>
        private Rectangle GetLayoutBounds()
        {
            var x = (textMinX == Int32.MaxValue) ? 0 : textMinX;
            var y = (textMinY == Int32.MaxValue) ? 0 : textMinY;
            return new Rectangle(x, y, textTotalWidth, textTotalHeight);
        }

        // Registered styles, icons, and fonts.
        private readonly Dictionary<StringSegment, TextStyle> registeredStyles = 
            new Dictionary<StringSegment, TextStyle>();
        private readonly Dictionary<StringSegment, SpriteFont> registeredFonts = 
            new Dictionary<StringSegment, SpriteFont>();
        private readonly Dictionary<StringSegment, InlineIconInfo> registeredIcons = 
            new Dictionary<StringSegment, InlineIconInfo>();

        // Layout state.
        private TextParserResult input;
        private TextLayoutResult output;
        private TextLayoutSettings settings;
        private Int32 cx;
        private Int32 cy;
        private Int32 lineStart;
        private Int32 lineWidth;
        private Int32 lineHeight;
        private Int32 lineWhiteSpace;
        private Int32 textMinX;
        private Int32 textMinY;
        private Int32 textTotalWidth;
        private Int32 textTotalHeight;
        private TextLayoutToken? accumulator;
    }
}
