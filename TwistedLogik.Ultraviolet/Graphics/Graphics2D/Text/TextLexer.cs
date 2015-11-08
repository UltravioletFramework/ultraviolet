using System;
using System.Text;
using System.Threading;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lexer which takes a string as input and produces a stream of formatted text tokens.
    /// </summary>
    public sealed partial class TextLexer
    {
        /// <summary>
        /// Lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        public void Lex(String input, TextLexerResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            output.Clear();
            Lex(new StringSource(input), output, 0, input.Length);
        }

        /// <summary>
        /// Incrementally lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to lex.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="result">The lexed token stream.</param>
        /// <remarks>Incremental lexing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-lexed by this operation.</remarks>
        public void LexIncremental(String input, Int32 start, Int32 count, TextLexerResult result)
        {
            Contract.Require(input, "input");
            Contract.Require(result, "output");
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count >= 0 && start + count <= input.Length, "count");

            LexIncremental(new StringSource(input), start, count, result);
        }

        /// <summary>
        /// Lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringBuilder"/> to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        internal void Lex(StringBuilder input, TextLexerResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            output.Clear();
            Lex(new StringSource(input), output, 0, input.Length);
        }

        /// <summary>
        /// Incrementally lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringBuilder"/> to lex.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="result">The lexed token stream.</param>
        /// <remarks>Incremental lexing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-lexed by this operation.</remarks>
        internal void LexIncremental(StringBuilder input, Int32 start, Int32 count, TextLexerResult result)
        {
            Contract.Require(input, "input");
            Contract.Require(result, "output");
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count >= 0 && start + count <= input.Length, "count");

            LexIncremental(new StringSource(input), start, count, result);
        }

        /// <summary>
        /// Lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringSource"/> to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        /// <param name="index">The index at which to begin lexing the input string.</param>
        /// <param name="count">the number of characters to lex.</param>
        private void Lex(StringSource input, TextLexerResult output, Int32 index, Int32 count)
        {
            var bound = index + count;
            while (index < bound)
            {
                if (IsStartOfNewline(input, index))
                {
                    output.Add(ConsumeNewlineToken(input, ref index));
                    continue;
                }
                if (IsStartOfWhitespace(input, index))
                {
                    output.Add(ConsumeWhitespaceToken(input, ref index));
                    continue;
                }
                if (IsEscapedPipe(input, index))
                {
                    output.Add(ConsumeEscapedPipeToken(input, ref index));
                    continue;
                }
                if (IsStartOfCommand(input, index))
                {
                    output.Add(ConsumeCommandToken(input, ref index));
                    continue;
                }
                if (IsStartOfWord(input, index))
                {
                    output.Add(ConsumeWordToken(input, ref index));
                    continue;
                }
                index++;
            }
        }

        /// <summary>
        /// Incrementally lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringSource"/> to lex.</param>
        /// <param name="start">The index of the first character that was changed.</param>
        /// <param name="count">The number of characters that were changed.</param>
        /// <param name="result">The lexed token stream.</param>
        /// <remarks>Incremental lexing provides a performance benefit when relatively small changes are being made
        /// to a large source text. Only tokens which are potentially influenced by changes within the specified substring
        /// of the source text are re-lexed by this operation.</remarks>
        private void LexIncremental(StringSource input, Int32 start, Int32 count, TextLexerResult result)
        {
            var inputLengthOld = (result.Count == 0) ? 0 : result[0].TokenText.SourceLength;
            var inputLengthNew = input.Length;
            var inputLengthDiff = inputLengthNew - inputLengthOld;

            Int32 ix1, ix2;
            FindTokensInfluencedBySubstring(result, start, count, out ix1, out ix2);

            var token1 = result[ix1];
            var token2 = result[ix2];
            
            var invalidatedTokenCount = 1 + (ix2 - ix1);
            result.RemoveRange(ix1, invalidatedTokenCount);
            
            var lexStart = token1.TokenText.Start;
            var lexCount = inputLengthDiff + (token2.TokenText.Start + token2.TokenText.Length) - lexStart;
            var lexBuffer = incrementalLexerBuffer.Value;
            Lex(input, lexBuffer, lexStart, lexCount);

            // NOTE: We still have to touch all of the old tokens for memory reasons.
            // If we don't update them to reference the new source text, then they'll continue to maintain a reference
            // to the old one, and therefore prevent it from being garbage collected. If lots of small edits are being
            // performed on a very large source text, this could quickly suck up a lot of heap memory!

            for (int i = 0; i < ix1; i++)
            {
                var token = result[i];
                token.TokenText = (input.String == null) ?
                    new StringSegment(input.StringBuilder, token.TokenText.Start, token.TokenText.Length) :
                    new StringSegment(input.String, token.TokenText.Start, token.TokenText.Length);
            }

            for (int i = ix1; i < result.Count; i++)
            {
                var token = result[i];
                token.TokenText = (input.String == null) ?
                    new StringSegment(input.StringBuilder, inputLengthDiff + token.TokenText.Start, token.TokenText.Length) :
                    new StringSegment(input.String, inputLengthDiff + token.TokenText.Start, token.TokenText.Length);
            }

            result.InsertRange(ix1, lexBuffer);

            lexBuffer.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a newline token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a newline token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfNewline(StringSource input, Int32 ix)
        {
            return input[ix] == '\n';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a whitespace token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the start of a whitespace token; otherwise, <c>false</c>.</returns>
        private static Boolean IsStartOfWhitespace(StringSource input, Int32 ix)
        {
            return Char.IsWhiteSpace(input[ix]) && !IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a whitespace token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a whitespace token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfWhitespace(StringSource input, Int32 ix)
        {
            return !Char.IsWhiteSpace(input[ix]) || IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is an escaped pipe.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is an escaped pipe; otherwise, <c>false</c>.</returns>
        private static Boolean IsEscapedPipe(StringSource input, Int32 ix)
        {
            return input[ix] == '|' && (ix + 1 >= input.Length || input[ix + 1] == '|' || IsStartOfWhitespace(input, ix + 1) || IsStartOfNewline(input, ix + 1));
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
            return !IsStartOfNewline(input, ix) && !IsStartOfWhitespace(input, ix) && !IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a word token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns><c>true</c> if the specified character is the end of a word token; otherwise, <c>false</c>.</returns>
        private static Boolean IsEndOfWord(StringSource input, Int32 ix)
        {
            return IsStartOfNewline(input, ix) || IsStartOfWhitespace(input, ix) || IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Retrieves a newline token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeNewlineToken(StringSource input, ref Int32 ix)
        {
            var segment = CreateStringSegmentFromStringSource(input, ix++, 1);
            return new TextLexerToken(TextLexerTokenType.NewLine, segment);
        }

        /// <summary>
        /// Retrieves a whitespace token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeWhitespaceToken(StringSource input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length && !IsEndOfWhitespace(input, ix))
            {
                ix++;
            }
            var segment = CreateStringSegmentFromStringSource(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.WhiteSpace, segment);
        }

        /// <summary>
        /// Retrieves an escaped pipe token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeEscapedPipeToken(StringSource input, ref Int32 ix)
        {
            ix++;
            if (ix < input.Length && input[ix] == '|')
            {
                ix++;
            }
            return new TextLexerToken(TextLexerTokenType.Pipe, "|");
        }

        /// <summary>
        /// Retrieves a command token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeCommandToken(StringSource input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length)
            {
                if (IsEndOfCommand(input, ix++)) 
                {
                    break; 
                }
            }
            var segment = CreateStringSegmentFromStringSource(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.Command, segment);
        }

        /// <summary>
        /// Retrieves a word token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeWordToken(StringSource input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length && !IsEndOfWord(input, ix))
            {
                ix++;
            }
            var segment = CreateStringSegmentFromStringSource(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.Word, segment);
        }

        /// <summary>
        /// Creates a new <see cref="StringSegment"/> from the specified <see cref="StringSource"/>.
        /// </summary>
        /// <param name="source">The <see cref="StringSource"/> from which to create the string segment.</param>
        /// <param name="start">The string segment's starting index.</param>
        /// <param name="length">The string segment's length.</param>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        private static StringSegment CreateStringSegmentFromStringSource(StringSource source, Int32 start, Int32 length)
        {
            if (source.String != null)
            {
                return new StringSegment(source.String, start, length);
            }
            if (source.StringBuilder != null)
            {
                return new StringSegment(source.StringBuilder, start, length);
            }
            return StringSegment.Empty;
        }

        /// <summary>
        /// Finds the index of the first and last tokens which are potentially affected by changes in the specified substring of the source text.
        /// </summary>
        private void FindTokensInfluencedBySubstring(TextLexerResult result, Int32 start, Int32 count, out Int32 ix1, out Int32 ix2)
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
                var tokenStart = token.TokenText.Start;
                var tokenEnd = tokenStart + token.TokenText.Length;

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

                position += token.TokenText.Length;
            }
        }

        // A temporary buffer used by the incremental lexer.
        private static readonly ThreadLocal<TextLexerResult> incrementalLexerBuffer = 
            new ThreadLocal<TextLexerResult>(() => new TextLexerResult());
    }
}
