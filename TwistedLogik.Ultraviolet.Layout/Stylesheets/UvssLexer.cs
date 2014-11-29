﻿using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a lexer for the Ultraviolet Stylesheet (UVSS) language.
    /// </summary>
    internal sealed class UvssLexer
    {
        /// <summary>
        /// Lexes an Ultraviolet Stylesheet (UVSS) file.
        /// </summary>
        /// <param name="input">The Ultraviolet Stylesheet source code to lex.</param>
        /// <returns>A sequence of lexer tokens produced from the specified input.</returns>
        public IList<UvssLexerToken> Lex(String input)
        {
            var output = new List<UvssLexerToken>();
            var ix = 0;            

            while (ix < input.Length)
            {
                if (ConsumeWhiteSpace(input, output, ref ix))
                    continue;
                if (ConsumeIdentifier(input, output, ref ix))
                    continue;
                if (ConsumeNumber(input, output, ref ix))
                    continue;
                if (ConsumeString(input, output, ref ix))
                    continue;
                if (ConsumeOpenParenthesis(input, output, ref ix))
                    continue;
                if (ConsumeCloseParenthesis(input, output, ref ix))
                    continue;
                if (ConsumeOpenCurlyBrace(input, output, ref ix))
                    continue;
                if (ConsumeCloseCurlyBrace(input, output, ref ix))
                    continue;
                if (ConsumeColon(input, output, ref ix))
                    continue;
                if (ConsumeSemicolon(input, output, ref ix))
                    continue;
                if (ConsumeComma(input, output, ref ix))
                    continue;

                var message = GetSyntaxErrorCallout(input, ix);
                throw new UvssException(LayoutStrings.StylesheetSyntaxError.Format(message));
            }

            return output;
        }

        /// <summary>
        /// Gets the callout string provided by syntax exceptions.
        /// </summary>
        private static String GetSyntaxErrorCallout(String input, Int32 ix)
        {
            const Int32 CalloutLength = 32;

            var messageStart  = Math.Max(0, ix - (CalloutLength / 2));
            var messageLength = Math.Min(CalloutLength, input.Length - messageStart);
            var message       = input.Substring(messageStart, messageLength);

            return message;
        }

        /// <summary>
        /// Attempts to consume a WhiteSpace token.
        /// </summary>
        private static Boolean ConsumeWhiteSpace(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!IsValidInWhiteSpace(input[ix]))
                return false;

            var start  = ix;
            var length = 0;

            while (ix < input.Length && IsValidInWhiteSpace(input[ix])) 
            { 
                ix++;
                length++;
            }

            var token = new UvssLexerToken(UvssLexerTokenType.WhiteSpace, start, length);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a WhiteSpace token.
        /// </summary>
        private static Boolean IsValidInWhiteSpace(Char c)
        {
            return Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Attempts to consume an Identifier token.
        /// </summary>
        private static Boolean ConsumeIdentifier(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!IsValidStartIdentifier(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;

            while (ix < input.Length && IsValidInIdentifier(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.Identifier, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the start of an Identifier token.
        /// </summary>
        private static Boolean IsValidStartIdentifier(Char c)
        {
            return Char.IsLetter(c) || c == '_' || c == '.' || c == '#';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in an Identifier token.
        /// </summary>
        private static Boolean IsValidInIdentifier(Char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_' || c == '-';
        }

        /// <summary>
        /// Attempts to consume a Number token.
        /// </summary>
        private static Boolean ConsumeNumber(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!Char.IsDigit(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;
            var dec    = false;

            while (ix < input.Length && IsValidInNumber(input[ix], ref dec)) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.Number, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a Number token.
        /// </summary>
        private static Boolean IsValidInNumber(Char c, ref Boolean dec)
        {
            if (c == '.')
            {
                if (dec)
                {
                    return false;
                }
                dec = true;
                return true;
            }
            return Char.IsDigit(c);
        }

        /// <summary>
        /// Attempts to consume a String token.
        /// </summary>
        private static Boolean ConsumeString(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (input[ix] != '"')
                return false;

            var start  = ix++;
            var length = 2;

            while (ix < input.Length)
            {
                if (ix + 1 == input.Length && input[ix] != '"')
                {
                    var message = GetSyntaxErrorCallout(input, ix);
                    throw new UvssException(LayoutStrings.StylesheetSyntaxUnterminatedString.Format(message));
                }

                var c = input[ix++];
                if (c == '"')
                    break;

                length++;
            }

            var value = input.Substring(start + 1, length - 2);
            var token = new UvssLexerToken(UvssLexerTokenType.String, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Attempts to consume a punctuation token.
        /// </summary>
        private static Boolean ConsumePunctuation(UvssLexerTokenType type, Char punctuation, String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (input[ix] == punctuation)
            {
                var token = new UvssLexerToken(type, ix, 1);
                output.Add(token);

                ix++;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to consume an OpenParenthesis token.
        /// </summary>
        private static Boolean ConsumeOpenParenthesis(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.OpenParenthesis, '(', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume a CloseParenthesis token.
        /// </summary>
        private static Boolean ConsumeCloseParenthesis(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.CloseParenthesis, ')', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume an OpenCurlyBrace token.
        /// </summary>
        private static Boolean ConsumeOpenCurlyBrace(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.OpenCurlyBrace, '{', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume a CloseCurlyBrace token.
        /// </summary>
        private static Boolean ConsumeCloseCurlyBrace(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.CloseCurlyBrace, '}', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Colon token.
        /// </summary>
        private static Boolean ConsumeColon(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Colon, ':', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Semicolon token.
        /// </summary>
        private static Boolean ConsumeSemicolon(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Semicolon, ';', input, output, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Comma token.
        /// </summary>
        private static Boolean ConsumeComma(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Comma, ',', input, output, ref ix);
        }
    }
}
