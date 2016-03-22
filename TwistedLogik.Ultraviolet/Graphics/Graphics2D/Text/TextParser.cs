using System;
using System.Text;
using System.Threading;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lexer/parser which takes a string as input and produces a stream of formatted text tokens.
    /// </summary>
    public sealed partial class TextParser
    {
        /// <summary>
        /// Registers a new custom command which can be interpreted by the <see cref="TextParser"/> class.
        /// </summary>
        /// <param name="name">The name of the command to register.</param>
        /// <returns>The command's identifier within the text parsing system.</returns>
        public static Byte RegisterCustomCommand(String name)
        {
            const Int32 MaximumCustomCommandCount = Byte.MaxValue - (Int32)TextParserTokenType.Custom;
            
            lock (customCommands)
            {
                if (customCommands.Count == MaximumCustomCommandCount)
                    throw new InvalidOperationException("TODO");

                customCommands.Add(name);

                return (Byte)(customCommands.Count - 1);
            }
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

            output.Clear();
            Parse(new StringSource(input), output, 0, input.Length, options);
        }

        /// <summary>
        /// Incrementally lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to parse.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="result">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-parsed by this operation.</remarks>
        public IncrementalResult ParseIncremental(String input, Int32 start, Int32 count, TextParserTokenStream result, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, "input");
            Contract.Require(result, "output");
            Contract.EnsureRange(start >= 0, "start");

            return ParseIncremental(new StringSource(input), start, count, result, options);
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

            output.Clear();
            Parse(new StringSource(input), output, 0, input.Length, options);
        }

        /// <summary>
        /// Incrementally lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringBuilder"/> to parse.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="result">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-parsed by this operation.</remarks>
        public IncrementalResult ParseIncremental(StringBuilder input, Int32 start, Int32 count, TextParserTokenStream result, TextParserOptions options = TextParserOptions.None)
        {
            Contract.Require(input, "input");
            Contract.Require(result, "output");
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count >= 0 && start + count <= input.Length, "count");

            return ParseIncremental(new StringSource(input), start, count, result, options);
        }

        /// <summary>
        /// Lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringSource"/> to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="index">The index at which to begin parsing the input string.</param>
        /// <param name="count">the number of characters to parse.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        private void Parse(StringSource input, TextParserTokenStream output, Int32 index, Int32 count, TextParserOptions options = TextParserOptions.None)
        {
            var bound = index + count;
            while (index < bound)
            {
                if (IsStartOfNewline(input, index))
                {
                    output.Add(ConsumeNewlineToken(input, options, ref index));
                    continue;
                }
                if (IsStartOfNonBreakingSpace(input, index))
                {
                    output.Add(ConsumeNonBreakingSpaceToken(input, options, ref index));
                    continue;
                }
                if (IsStartOfBreakingSpace(input, index))
                {
                    output.Add(ConsumeBreakingSpaceToken(input, options, ref index));
                    continue;
                }
                if (IsEscapedPipe(input, index, options))
                {
                    output.Add(ConsumeEscapedPipeToken(input, options, ref index));
                    continue;
                }
                if (IsStartOfCommand(input, index))
                {
                    output.Add(ConsumeCommandToken(input, options, ref index));
                    continue;
                }
                if (IsStartOfWord(input, index))
                {
                    output.Add(ConsumeWordToken(input, options, ref index));
                    continue;
                }
                index++;
            }

            output.SourceText = input.CreateStringSegment();
            output.ParserOptions = options;
        }

        /// <summary>
        /// Incrementally lexes and parses the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringSource"/> to parse.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="output">The parsed token stream.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>An <see cref="IncrementalResult"/> structure that represents the result of the operation.</returns>
        /// <remarks>Incremental parsing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-parsed by this operation.</remarks>
        private IncrementalResult ParseIncremental(StringSource input, Int32 start, Int32 count, TextParserTokenStream output, TextParserOptions options = TextParserOptions.None)
        {
            var inputLengthOld = output.SourceText.Length;
            var inputLengthNew = input.Length;
            var inputLengthDiff = inputLengthNew - inputLengthOld;

            Int32 ix1, ix2;
            FindTokensInfluencedBySubstring(output, start, count - inputLengthDiff, out ix1, out ix2);

            var token1 = output[ix1];
            var token2 = output[ix2];
            
            var invalidatedTokenCount = 1 + (ix2 - ix1);
            output.RemoveRange(ix1, invalidatedTokenCount);

            var lexStart = token1.SourceOffset;
            var lexCount = inputLengthDiff + (token2.SourceOffset + token2.SourceLength) - lexStart;
            var parserBuffer = incrementalParserBuffer.Value;
            Parse(input, parserBuffer, lexStart, lexCount);
            
            output.SourceText = input.CreateStringSegment();
            output.InsertRange(ix1, parserBuffer);

            var affectedOffset = ix1;
            var affectedCount = parserBuffer.Count;

            parserBuffer.Clear();

            return new IncrementalResult(affectedOffset, affectedCount);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a newline token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a newline token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfNewline(StringSource input, Int32 ix)
        {
            return input[ix] == '\n' || input[ix] == '\r';
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a non-breaking white space token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a non-breaking white space token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfNonBreakingSpace(StringSource input, Int32 ix)
        {
            return input[ix] == '\u00A0';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a non-breaking white space token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a non-breaking white space token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfNonBreakingSpace(StringSource input, Int32 ix)
        {
            return input[ix] != '\u00A0';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a breaking white space token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a breaking white space token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfBreakingSpace(StringSource input, Int32 ix)
        {
            return input[ix] != '\u00A0' && Char.IsWhiteSpace(input[ix]) && !IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a breaking white space token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a breaking white space token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfBreakingSpace(StringSource input, Int32 ix)
        {
            return input[ix] == '\u00A0' || !Char.IsWhiteSpace(input[ix]) || IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is an escaped pipe.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns><c>true</c> if the specified character is an escaped pipe; otherwise, <c>false</c>.</returns>
        private static Boolean IsEscapedPipe(StringSource input, Int32 ix, TextParserOptions options)
        {
            if ((options & TextParserOptions.IgnoreCommandCodes) == TextParserOptions.IgnoreCommandCodes)
                return false;

            return input[ix] == '|' && (ix + 1 >= input.Length || input[ix + 1] == '|' || Char.IsWhiteSpace(input[ix + 1]));
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a command token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a command token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfCommand(StringSource input, Int32 ix)
        {
            return input[ix] == '|';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a command token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a command token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfCommand(StringSource input, Int32 ix)
        {
            return input[ix] == '|';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a word token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a word token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfWord(StringSource input, Int32 ix)
        {
            return !Char.IsWhiteSpace(input[ix]) && !IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a word token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a word token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfWord(StringSource input, Int32 ix)
        {
            return Char.IsWhiteSpace(input[ix]) || IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Retrieves a newline token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeNewlineToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var sourceLength = 1;
            if (input[ix] == '\r' && ix + 1 < input.Length && input[ix + 1] == '\n')
                sourceLength = 2;

            var sourceOffset = ix;
            ix += sourceLength;

            var segment = input.CreateStringSegmentFromSameSource(sourceOffset, sourceLength);
            return ParseLexerToken(LexedTokenType.NewLine, segment, sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Retrieves a non-breaking white space token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeNonBreakingSpaceToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var start = ix++;

            while (ix < input.Length && !IsEndOfNonBreakingSpace(input, ix))
                ix++;

            var sourceOffset = start;
            var sourceLength = ix - start;
            var segment = input.CreateStringSegmentFromSameSource(sourceOffset, sourceLength);
            return ParseLexerToken(LexedTokenType.NonBreakingWhiteSpace, segment, sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Retrieves a breaking white space token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeBreakingSpaceToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var start = ix++;

            while (ix < input.Length && !IsEndOfBreakingSpace(input, ix))
                ix++;

            var sourceOffset = start;
            var sourceLength = ix - start;
            var segment = input.CreateStringSegmentFromSameSource(sourceOffset, sourceLength);
            return ParseLexerToken(LexedTokenType.BreakingWhiteSpace, segment, sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Retrieves an escaped pipe token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeEscapedPipeToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var start = ix++;
            
            if (ix < input.Length && input[ix] == '|')
                ix++;

            var sourceOffset = start;
            var sourceLength = 1;
            return ParseLexerToken(LexedTokenType.Pipe, "|", sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Retrieves a command token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeCommandToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var valid = false;
            var start = ix++;
            while (ix < input.Length)
            {
                if (Char.IsWhiteSpace(input[ix]))
                    break;

                if (IsEndOfCommand(input, ix++)) 
                {
                    valid = true;
                    break; 
                }
            }

            var sourceOffset = start;
            var sourceLength = ix - start;
            var segment = input.CreateStringSegmentFromSameSource(sourceOffset, sourceLength);
            return ParseLexerToken(valid ? LexedTokenType.Command : LexedTokenType.Word, segment, sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Retrieves a word token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="options">The parser options.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextParserToken ConsumeWordToken(StringSource input, TextParserOptions options, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length && !IsEndOfWord(input, ix))
                ix++;

            var sourceOffset = start;
            var sourceLength = ix - start;
            var segment = input.CreateStringSegmentFromSameSource(sourceOffset, sourceLength);
            return ParseLexerToken(LexedTokenType.Word, segment, sourceOffset, sourceLength, options);
        }

        /// <summary>
        /// Parses a lexer token.
        /// </summary>
        /// <param name="tokenType">A <see cref="LexedTokenType"/> value specifying the type of token produced by the lexer.</param>
        /// <param name="tokenText">The text associated with the lexer token.</param>
        /// <param name="sourceOffset">The offset of the first character in the source text that produced the token.</param>
        /// <param name="sourceLength">The number of characters in the source text that produced the token.</param>
        /// <param name="options">A set of <see cref="TextParserOptions"/> values that specify how the text should be parsed.</param>
        /// <returns>The parsed token.</returns>
        private static TextParserToken ParseLexerToken(LexedTokenType tokenType, StringSegment tokenText, Int32 sourceOffset, Int32 sourceLength, TextParserOptions options)
        {
            var isIgnoringCommandCodes = (options & TextParserOptions.IgnoreCommandCodes) == TextParserOptions.IgnoreCommandCodes;

            if (tokenType == LexedTokenType.Command && !isIgnoringCommandCodes)
                return ParseCommandToken(tokenText, sourceOffset, sourceLength);

            return new TextParserToken(TextParserTokenType.Text, tokenText, sourceOffset, sourceLength, 
                tokenType == LexedTokenType.NonBreakingWhiteSpace);
        }

        /// <summary>
        /// Parses a command token.
        /// </summary>
        /// <param name="tokenText">The text associated with the lexer token.</param>
        /// <param name="sourceOffset">The offset of the first character in the source text that produced the token.</param>
        /// <param name="sourceLength">The number of characters in the source text that produced the token.</param>
        /// <returns>The parsed token.</returns>
        private static TextParserToken ParseCommandToken(StringSegment tokenText, Int32 sourceOffset, Int32 sourceLength)
        {
            // Toggle bold style.
            if (tokenText == "|b|")
            {
                return new TextParserToken(TextParserTokenType.ToggleBold, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Toggle italic style.
            if (tokenText == "|i|")
            {
                return new TextParserToken(TextParserTokenType.ToggleItalic, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Set the current color.
            if (tokenText.Length == 12 && tokenText.Substring(0, 3) == "|c:")
            {
                var hexcode = tokenText.Substring(3, 8);
                return new TextParserToken(TextParserTokenType.PushColor, hexcode, sourceOffset, sourceLength);
            }

            // Clear the current color.
            if (tokenText == "|c|")
            {
                return new TextParserToken(TextParserTokenType.PopColor, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Set the current font.
            if (tokenText.Length >= 8 && tokenText.Substring(0, 6) == "|font:")
            {
                var name = tokenText.Substring(6, tokenText.Length - 7);
                return new TextParserToken(TextParserTokenType.PushFont, name, sourceOffset, sourceLength);
            }

            // Clear the current font.
            if (tokenText == "|font|")
            {
                return new TextParserToken(TextParserTokenType.PopFont, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Set the current glyph shader.
            if (tokenText.Length >= 10 && tokenText.Substring(0, 8) == "|shader:")
            {
                var name = tokenText.Substring(8, tokenText.Length - 9);
                return new TextParserToken(TextParserTokenType.PushGlyphShader, name, sourceOffset, sourceLength);
            }

            // Clear the current glyph shader.
            if (tokenText == "|shader|")
            {
                return new TextParserToken(TextParserTokenType.PopGlyphShader, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Set the preset style.
            if (tokenText.Length >= 9 && tokenText.Substring(0, 7) == "|style:")
            {
                var name = tokenText.Substring(7, tokenText.Length - 8);
                return new TextParserToken(TextParserTokenType.PushStyle, name, sourceOffset, sourceLength);
            }

            // Clear the preset style.
            if (tokenText == "|style|")
            {
                return new TextParserToken(TextParserTokenType.PopStyle, StringSegment.Empty, sourceOffset, sourceLength);
            }

            // Emit an inline icon.
            if (tokenText.Length >= 8 && tokenText.Substring(0, 6) == "|icon:")
            {
                var name = tokenText.Substring(6, tokenText.Length - 7);
                return new TextParserToken(TextParserTokenType.Icon, name, sourceOffset, sourceLength);
            }

            // Emit a custom command.
            lock (customCommands)
            {
                for (int i = customCommands.Count - 1; i >= 0; i--)
                {
                    var command = customCommands[i];
                    if (tokenText.Length >= 2 + command.Length && tokenText.Substring(1, command.Length) == command)
                    {
                        var id = (Byte)i;
                        var text = tokenText[1 + command.Length] == ':' ?
                            tokenText.Substring(2 + command.Length, tokenText.Length - (3 + command.Length)) : StringSegment.Empty;
                        return new TextParserToken((TextParserTokenType)((Int32)TextParserTokenType.Custom + id),
                            text, sourceOffset, sourceLength);
                    }
                }
            }

            // Unrecognized or invalid command.
            return new TextParserToken(TextParserTokenType.Text, tokenText, sourceOffset, sourceLength);
        }
        
        /// <summary>
        /// Finds the index of the first and last tokens which are potentially affected by changes in the specified substring of the source text.
        /// </summary>
        private void FindTokensInfluencedBySubstring(TextParserTokenStream result, Int32 start, Int32 count, out Int32 ix1, out Int32 ix2)
        {
            var position = 0;
            var end = start + count;

            var ix1Found = false;
            var ix2Found = false;

            ix1 = 0;
            ix2 = result.Count - 1;

            for (int i = 0; i < result.Count; i++)
            {
                var token = result[i];
                var tokenStart = token.SourceOffset;
                var tokenEnd = tokenStart + token.SourceLength;

                if (!ix1Found && (start >= tokenStart && start <= tokenEnd))
                {
                    ix1 = i;
                    ix1Found = true;
                }

                if (!ix2Found && (end >= tokenStart && end < tokenEnd))
                {
                    ix2 = i;
                    ix2Found = true;
                }

                if (ix1Found && ix2Found)
                    break;

                position += token.SourceLength;
            }
        }

        // Registered custom commands.
        private static readonly List<String> customCommands = new List<String>();

        // A temporary buffer used by the incremental parser.
        private static readonly ThreadLocal<TextParserTokenStream> incrementalParserBuffer = 
            new ThreadLocal<TextParserTokenStream>(() => new TextParserTokenStream());
    }
}
