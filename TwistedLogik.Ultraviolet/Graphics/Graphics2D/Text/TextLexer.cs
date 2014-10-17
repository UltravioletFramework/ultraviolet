using System;
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
        /// <param name="input">The string to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        public void Lex(String input, TextLexerResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

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
        /// <returns>true if the specified character is the start of a newline token; otherwise, false.</returns>
        private static Boolean IsStartOfNewline(String input, Int32 ix)
        {
            return input[ix] == '\n';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a whitespace token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the start of a whitespace token; otherwise, false.</returns>
        private static Boolean IsStartOfWhitespace(String input, Int32 ix)
        {
            return Char.IsWhiteSpace(input[ix]) && !IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a whitespace token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the end of a whitespace token; otherwise, false.</returns>
        private static Boolean IsEndOfWhitespace(String input, Int32 ix)
        {
            return !Char.IsWhiteSpace(input, ix) || IsStartOfNewline(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is an escaped pipe.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is an escaped pipe; otherwise, false.</returns>
        private static Boolean IsEscapedPipe(String input, Int32 ix)
        {
            return input[ix] == '|' && (ix + 1 >= input.Length || input[ix + 1] == '|' || IsStartOfWhitespace(input, ix + 1) || IsStartOfNewline(input, ix + 1));
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a command token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the start of a command token; otherwise, false.</returns>
        private static Boolean IsStartOfCommand(String input, Int32 ix)
        {
            return input[ix] == '|';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a command token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the end of a command token; otherwise, false.</returns>
        private static Boolean IsEndOfCommand(String input, Int32 ix)
        {
            return input[ix] == '|';
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the start of a word token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the start of a word token; otherwise, false.</returns>
        private static Boolean IsStartOfWord(String input, Int32 ix)
        {
            return !IsStartOfNewline(input, ix) && !IsStartOfWhitespace(input, ix) && !IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a word token.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index of the character to evaluate.</param>
        /// <returns>true if the specified character is the end of a word token; otherwise, false.</returns>
        private static Boolean IsEndOfWord(String input, Int32 ix)
        {
            return IsStartOfNewline(input, ix) || IsStartOfWhitespace(input, ix) || IsStartOfCommand(input, ix);
        }

        /// <summary>
        /// Retrieves a newline token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeNewlineToken(String input, ref Int32 ix)
        {
            var segment = new StringSegment(input, ix++, 1);
            return new TextLexerToken(TextLexerTokenType.NewLine, segment);
        }

        /// <summary>
        /// Retrieves a whitespace token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeWhitespaceToken(String input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length && !IsEndOfWhitespace(input, ix))
            {
                ix++;
            }
            var segment = new StringSegment(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.WhiteSpace, segment);
        }

        /// <summary>
        /// Retrieves an escaped pipe token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeEscapedPipeToken(String input, ref Int32 ix)
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
        private static TextLexerToken ConsumeCommandToken(String input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length)
            {
                if (IsEndOfCommand(input, ix++)) 
                {
                    break; 
                }
            }            
            var segment = new StringSegment(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.Command, segment);
        }

        /// <summary>
        /// Retrieves a word token from the input stream, beginning at the specified character.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="ix">The index at which to begin consuming token characters.</param>
        /// <returns>The token that was created.</returns>
        private static TextLexerToken ConsumeWordToken(String input, ref Int32 ix)
        {
            var start = ix++;
            while (ix < input.Length && !IsEndOfWord(input, ix))
            {
                ix++;
            }
            var segment = new StringSegment(input, start, ix - start);
            return new TextLexerToken(TextLexerTokenType.Word, segment);
        }
    }
}
