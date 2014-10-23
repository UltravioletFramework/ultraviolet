using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Contains methods for parsing a stream of formatted text tokens in preparation for layout.
    /// </summary>
    public sealed partial class TextParser
    {
        /// <summary>
        /// Initializes the <see cref="TextParser"/> type.
        /// </summary>
        static TextParser()
        {
            HexDigitValues = new Dictionary<Char, UInt32>();
            PopulateDigitValues(HexDigitValues, "0123456789ABCDEF");

            DecimalDigitValues = new Dictionary<Char, UInt32>();
            PopulateDigitValues(DecimalDigitValues, "0123456789");
        }

        /// <summary>
        /// Parses the specified token stream.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        public void Parse(TextLexerResult input, TextParserResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            var style = new TextStyle();

            output.Clear();
            output.Styles.Clear();
            output.Styles.Add(style);

            foreach (var token in input)
            {
                switch (token.TokenType)
                {
                    case TextLexerTokenType.NewLine:
                    case TextLexerTokenType.WhiteSpace:
                    case TextLexerTokenType.Word:
                    case TextLexerTokenType.Pipe:
                        output.Add(new TextParserToken(token.TokenText, output.Styles.Count - 1));
                        break;

                    case TextLexerTokenType.Command:
                        InterpretCommand(output, token, ref style);
                        break;
                }
            }

            ClearStacks();
        }

        /// <summary>
        /// Populates a lookup table with the numeric value of each digit in the specified string.
        /// </summary>
        /// <param name="lookup">The lookup table to populate.</param>
        /// <param name="digits">The string containing the digits for which to populate values.</param>
        private static void PopulateDigitValues(Dictionary<Char, UInt32> lookup, String digits)
        {
            var value = 0u;
            foreach (var c in digits)
            {
                lookup[Char.ToUpper(c)] = value;
                lookup[Char.ToLower(c)] = value;
                value++;
            }
        }

        /// <summary>
        /// Interprets a value of a string segment containing a number in the given base.
        /// </summary>
        /// <param name="str">The string segment containing the value to interpret.</param>
        /// <param name="base">The base of the numeric value contained by the string segment.</param>
        /// <returns>The interpreted value of the string segment.</returns>
        private static UInt32 InterpretNumericValue(StringSegment str, Int32 @base)
        {
            if (@base == 10) return InterpretDecimalValue(str);
            if (@base == 16) return InterpretHexValue(str);
            throw new NotSupportedException();
        }

        /// <summary>
        /// Interprets a value of a string segment containing a number in base 10.
        /// </summary>
        /// <param name="str">The string segment containing the value to interpret.</param>
        /// <returns>The interpreted value of the string segment.</returns>
        private static UInt32 InterpretDecimalValue(StringSegment str)
        {
            var value = 0u;
            var factor = 1u;
            var digitval = 0u;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (!DecimalDigitValues.TryGetValue(str[i], out digitval))
                    throw new FormatException();

                value += (digitval * factor);
                factor *= 10;
            }
            return value;
        }

        /// <summary>
        /// Interprets a value of a string segment containing a number in base 16.
        /// </summary>
        /// <param name="str">The string segment containing the value to interpret.</param>
        /// <returns>The interpreted value of the string segment.</returns>
        private static UInt32 InterpretHexValue(StringSegment str)
        {
            var value = 0u;
            var factor = 1u;
            var digitval = 0u;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (!HexDigitValues.TryGetValue(str[i], out digitval))
                    throw new FormatException();

                value += (digitval * factor);
                factor *= 16;
            }
            return value;
        }

        /// <summary>
        /// Interprets a command token.
        /// </summary>
        /// <param name="output">The parser's token output stream.</param>
        /// <param name="token">The command token to interpret.</param>
        /// <param name="style">The current text style.</param>
        private void InterpretCommand(TextParserResult output, TextLexerToken token, ref TextStyle style)
        {
            // Toggle bold style.
            if (token.TokenText == "|b|")
            {
                output.AddStyleWithBold(ref style, !(style.Bold ?? false));
                return;
            }

            // Toggle italic style.
            if (token.TokenText == "|i|")
            {
                output.AddStyleWithItalic(ref style, !(style.Italic ?? false));
                return;
            }

            // Set the current color.
            if (token.TokenText.Length == 12 && token.TokenText.Substring(0, 3) == "|c:")
            {
                var hexcode = token.TokenText.Substring(3, 8);
                var packedval = InterpretHexValue(hexcode.Substring(0, 8));
                PushColor(output, ref style, Color.FromArgb(packedval));
                return;
            }

            // Clear the current color.
            if (token.TokenText == "|c|")
            {
                PopColor(output, ref style);
                return;
            }

            // Set the current font.
            if (token.TokenText.Length >= 8 && token.TokenText.Substring(0, 6) == "|font:")
            {
                var name = token.TokenText.Substring(6, token.TokenText.Length - 7);
                PushFont(output, ref style, name);
                return;
            }

            // Clear the current font.
            if (token.TokenText == "|font|")
            {
                PopFont(output, ref style);
                return;
            }

            // Set the preset style.
            if (token.TokenText.Length >= 9 && token.TokenText.Substring(0, 7) == "|style:")
            {
                var name = token.TokenText.Substring(7, token.TokenText.Length - 8);
                PushStyle(output, ref style, name);
                return;
            }

            // Clear the preset style.
            if (token.TokenText == "|style|")
            {
                PopStyle(output, ref style);
                return;
            }

            // Emit an inline icon.
            if (token.TokenText.Length >= 8 && token.TokenText.Substring(0, 6) == "|icon:")
            {
                var name = token.TokenText.Substring(6, token.TokenText.Length - 7);
                output.AddStyleWithIcon(ref style, name);
                output.AddToken(StringSegment.Empty);
                output.AddStyleWithIcon(ref style, null);
                return;
            }

            // Unrecognized or invalid command.
            output.AddToken(token.TokenText);
        }

        /// <summary>
        /// Pushes a color onto the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        /// <param name="color">The color to push onto the stack.</param>
        private void PushColor(TextParserResult output, ref TextStyle style, Color color)
        {
            colorStack.Push(color);
            output.AddStyleWithColor(ref style, color);
        }

        /// <summary>
        /// Pushes a color onto the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        /// <param name="font">The font to push onto the stack.</param>
        private void PushFont(TextParserResult output, ref TextStyle style, StringSegment font)
        {
            fontStack.Push(font);
            output.AddStyleWithFont(ref style, font);
        }

        /// <summary>
        /// Pushes a style preset onto the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        /// <param name="preset">The style preset to push onto the stack.</param>
        private void PushStyle(TextParserResult output, ref TextStyle style, StringSegment preset)
        {
            styleStack.Push(preset);
            output.AddStyleWithStyle(ref style, preset);
        }

        /// <summary>
        /// Pops a color off of the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        private void PopColor(TextParserResult output, ref TextStyle style)
        {
            if (colorStack.Count > 0)
            {
                colorStack.Pop();
                var color = (colorStack.Count > 0) ? colorStack.Peek() : (Color?)null;
                style.Color = color;
                output.AddStyleWithColor(ref style, color);
            }
        }

        /// <summary>
        /// Pops a font off of the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        private void PopFont(TextParserResult output, ref TextStyle style)
        {
            if (fontStack.Count > 0)
            {
                fontStack.Pop();
                var font = (fontStack.Count > 0) ? fontStack.Peek() : (StringSegment?)null;
                style.Font = font;
                output.AddStyleWithFont(ref style, font);
            }
        }

        /// <summary>
        /// Pops a style preset off of the style stack.
        /// </summary>
        /// <param name="output">The parser output stream.</param>
        /// <param name="style">The current style.</param>
        private void PopStyle(TextParserResult output, ref TextStyle style)
        {
            if (styleStack.Count > 0)
            {
                styleStack.Pop();
                var preset = (styleStack.Count > 0) ? styleStack.Peek() : (StringSegment?)null;
                style.Style = preset;
                output.AddStyleWithStyle(ref style, preset);
            }
        }

        /// <summary>
        /// Clears the parser's style stacks.
        /// </summary>
        private void ClearStacks()
        {
            colorStack.Clear();
            fontStack.Clear();
            styleStack.Clear();
        }

        // The numeric values of digits in each supported base.
        private static readonly Dictionary<Char, UInt32> HexDigitValues;
        private static readonly Dictionary<Char, UInt32> DecimalDigitValues;

        // State stacks for the current parse.
        private readonly Stack<Color> colorStack = new Stack<Color>();
        private readonly Stack<StringSegment> fontStack = new Stack<StringSegment>();
        private readonly Stack<StringSegment> styleStack = new Stack<StringSegment>();
    }
}
