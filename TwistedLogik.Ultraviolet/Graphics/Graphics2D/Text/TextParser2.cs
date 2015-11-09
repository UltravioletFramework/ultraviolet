using System;
using System.Threading;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Contains methods for parsing a stream of formatted text tokens in preparation for layout.
    /// </summary>
    public sealed class TextParser2
    {
        /// <summary>
        /// Parses the specified token stream.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        public void Parse(TextLexerResult input, TextParserResult2 output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            output.Clear();

            Parse(input, 0, input.Count, output, options);
        }

        /// <summary>
        /// Incrementally parses the specified token stream.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="start">The index of the first token that was changed.</param>
        /// <param name="count">The number of tokens that were changed.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified range of
        /// lexer tokens are re-parsed by this operation.</remarks>
        public IncrementalResult ParseIncremental(TextLexerResult input, Int32 start, Int32 count, TextParserResult2 output, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count >= 0 && start + count <= input.Count, "count");

            var tokenCountOld = output.Count;
            var tokenCountNew = input.Count;
            var tokenCountDiff = tokenCountNew - tokenCountOld;

            output.RemoveRange(start, count - tokenCountDiff);

            var parseBuffer = incrementalParserBuffer.Value;
            Parse(input, start, count, parseBuffer, options);

            output.Source = input.Source;
            output.InsertRange(start, parseBuffer);

            var affectedOffset = start;
            var affectedCount = parseBuffer.Count;

            parseBuffer.Clear();

            return new IncrementalResult(affectedOffset, affectedCount);
        }

        /// <summary>
        /// Parses the specified token stream.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="count">The index of the first token in the input stream to parse.</param>
        /// <param name="start">The number of input tokens to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        private void Parse(TextLexerResult input, Int32 start, Int32 count, TextParserResult2 output, TextParserOptions options)
        {
            var isIgnoringCommandCodes = (options & TextParserOptions.IgnoreCommandCodes) == TextParserOptions.IgnoreCommandCodes;

            for (int i = 0; i < count; i++)
            {
                var inputToken = input[start + i];
                if (inputToken.TokenType == TextLexerTokenType.Command && !isIgnoringCommandCodes)
                {
                    InterpretCommand(output, inputToken);
                }
                else
                {
                    var outputToken = new TextParserToken2(TextParserTokenType.Text, inputToken.TokenText);
                    output.Add(outputToken);
                }
            }

            output.Source = input.Source;
        }

        /// <summary>
        /// Interprets a command token.
        /// </summary>
        /// <param name="output">The parser's token output stream.</param>
        /// <param name="inputToken">The command token to interpret.</param>
        private void InterpretCommand(TextParserResult2 output, TextLexerToken inputToken)
        {
            // Toggle bold style.
            if (inputToken.TokenText == "|b|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.ToggleBold, StringSegment.Empty));
                return;
            }

            // Toggle italic style.
            if (inputToken.TokenText == "|i|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.ToggleItalic, StringSegment.Empty));
                return;
            }

            // Set the current color.
            if (inputToken.TokenText.Length == 12 && inputToken.TokenText.Substring(0, 3) == "|c:")
            {
                var hexcode = inputToken.TokenText.Substring(3, 8);
                output.Add(new TextParserToken2(TextParserTokenType.PushColor, hexcode));
                return;
            }

            // Clear the current color.
            if (inputToken.TokenText == "|c|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.PopColor, StringSegment.Empty));
                return;
            }

            // Set the current font.
            if (inputToken.TokenText.Length >= 8 && inputToken.TokenText.Substring(0, 6) == "|font:")
            {
                var name = inputToken.TokenText.Substring(6, inputToken.TokenText.Length - 7);
                output.Add(new TextParserToken2(TextParserTokenType.PushFont, name));
                return;
            }

            // Clear the current font.
            if (inputToken.TokenText == "|font|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.PopFont, StringSegment.Empty));
                return;
            }

            // Set the current glyph shader.
            if (inputToken.TokenText.Length >= 10 && inputToken.TokenText.Substring(0, 8) == "|shader:")
            {
                var name = inputToken.TokenText.Substring(8, inputToken.TokenText.Length - 9);
                output.Add(new TextParserToken2(TextParserTokenType.PushGlyphShader, name));
                return;
            }

            // Clear the current glyph shader.
            if (inputToken.TokenText == "|shader|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.PopGlyphShader, StringSegment.Empty));
                return;
            }

            // Set the preset style.
            if (inputToken.TokenText.Length >= 9 && inputToken.TokenText.Substring(0, 7) == "|style:")
            {
                var name = inputToken.TokenText.Substring(7, inputToken.TokenText.Length - 8);
                output.Add(new TextParserToken2(TextParserTokenType.PushStyle, name));
                return;
            }

            // Clear the preset style.
            if (inputToken.TokenText == "|style|")
            {
                output.Add(new TextParserToken2(TextParserTokenType.PopStyle, StringSegment.Empty));
                return;
            }

            // Emit an inline icon.
            if (inputToken.TokenText.Length >= 8 && inputToken.TokenText.Substring(0, 6) == "|icon:")
            {
                var name = inputToken.TokenText.Substring(6, inputToken.TokenText.Length - 7);
                output.Add(new TextParserToken2(TextParserTokenType.Icon, name));
                return;
            }

            // Unrecognized or invalid command.
            output.Add(new TextParserToken2(TextParserTokenType.Text, inputToken.TokenText));
        }

        // A temporary buffer used by the incremental parser.
        private static readonly ThreadLocal<TextParserResult2> incrementalParserBuffer =
            new ThreadLocal<TextParserResult2>(() => new TextParserResult2());
    }
}
