using System;
using System.Text;
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

            Lex(new StringSource(input), output);
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

            Lex(new StringSource(input), output);
        }

        /// <summary>
        /// Lexes the specified string.
        /// </summary>
        /// <param name="input">The <see cref="StringSource"/> to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        private void Lex(StringSource input, TextLexerResult output)
        {
            output.Clear();

            var ix = 0;
            while (ix < input.Length)
            {
                if (IsStartOfNewline(input, ix))
                {
                    output.Add(ConsumeNewlineToken(input, ref ix));
                    continue;
                }
                if (IsStartOfWhitespace(input, ix))
                {
                    output.Add(ConsumeWhitespaceToken(input, ref ix));
                    continue;
                }
                if (IsEscapedPipe(input, ix))
                {
                    output.Add(ConsumeEscapedPipeToken(input, ref ix));
                    continue;
                }
                if (IsStartOfCommand(input, ix))
                {
                    output.Add(ConsumeCommandToken(input, ref ix));
                    continue;
                }
                if (IsStartOfWord(input, ix))
                {
                    output.Add(ConsumeWordToken(input, ref ix));
                    continue;
                }
                ix++;
            }
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
    }
}
