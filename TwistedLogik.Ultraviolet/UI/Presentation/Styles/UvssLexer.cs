using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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
            var output     = new List<UvssLexerToken>();
            var ix         = 0;
            var braces     = 0;
            var parens     = 0;
            var storyboard = false;
            var arglist    = false;

            while (ix < input.Length)
            {
                if (ConsumeWhiteSpace(input, output, ref ix))
                    continue;
                if (!storyboard && !arglist)
                {
                    if (braces > 0)
                    {
                        if (ConsumeStyleName(input, output, ref ix))
                            continue;
                        if (ConsumeStyleQualifier(input, output, ref ix))
                            continue;
                    }
                    else
                    {
                        if (ConsumePseudoClass(input, output, ref ix))
                            continue;
                    }
                }
                if (ConsumeIdentifier(input, output, ref ix, ref storyboard))
                    continue;
                if (ConsumeNumber(input, output, ref ix))
                    continue;
                if (ConsumeString(input, output, ref ix))
                    continue;
                if (ConsumeOpenParenthesis(input, output, ref ix))
                {
                    parens++;
                    arglist = true;
                    continue;
                }
                if (ConsumeCloseParenthesis(input, output, ref ix))
                {
                    parens--;
                    if (parens == 0)
                    {
                        arglist = false;
                    }
                    continue;
                }
                if (ConsumeOpenCurlyBrace(input, output, ref ix))
                {
                    braces++;
                    continue;
                }
                if (ConsumeCloseCurlyBrace(input, output, ref ix))
                {
                    braces--;
                    if (braces == 0)
                    {
                        storyboard = false;
                    }
                    continue;
                }
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
        /// Attempts to consume a StyleName token.
        /// </summary>
        private static Boolean ConsumeStyleName(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!IsValidStartStyleName(input[ix]))
                return false;

            var start     = ix++;
            var length    = 1;
            var qualified = false;

            while (ix < input.Length && IsValidInStyleName(input[ix], ref qualified)) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.StyleName, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the start of a StyleName token.
        /// </summary>
        private static Boolean IsValidStartStyleName(Char c)
        {
            return Char.IsLetter(c) || c == '-';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a StyleName token.
        /// </summary>
        private static Boolean IsValidInStyleName(Char c, ref Boolean qualified)
        {
            if (c == '.')
            {
                if (qualified)
                {
                    return false;
                }
                qualified = true;
                return true;
            }
            return Char.IsLetterOrDigit(c) || c == '_' || c == '-';
        }

        /// <summary>
        /// Attempts to consume a StyleQualifier token.
        /// </summary>
        private static Boolean ConsumeStyleQualifier(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!IsValidStartStyleQualifier(input[ix]))
                return false;

            var start     = ix++;
            var length    = 1;

            while (ix < input.Length && IsValidInStyleQualifier(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.StyleQualifier, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the start of a StyleQualifier token.
        /// </summary>
        private static Boolean IsValidStartStyleQualifier(Char c)
        {
            return c == '!';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a StyleQualifier token.
        /// </summary>
        private static Boolean IsValidInStyleQualifier(Char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_' || c == '-';
        }

        /// <summary>
        /// Attempts to consume a PseudoClass token.
        /// </summary>
        private static Boolean ConsumePseudoClass(String input, IList<UvssLexerToken> output, ref Int32 ix)
        {
            if (!IsValidStartPseudoClass(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;

            while (ix < input.Length && IsValidInPseudoClass(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.PseudoClass, start, length, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the start of a PseudoClass token.
        /// </summary>
        private static Boolean IsValidStartPseudoClass(Char c)
        {
            return c == ':';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a PseudoClass token.
        /// </summary>
        private static Boolean IsValidInPseudoClass(Char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_' || c == '-';
        }

        /// <summary>
        /// Attempts to consume an Identifier token.
        /// </summary>
        private static Boolean ConsumeIdentifier(String input, IList<UvssLexerToken> output, ref Int32 ix, ref Boolean storyboard)
        {
            if (!IsValidStartIdentifier(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;

            while (ix < input.Length && IsValidInIdentifier(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.Identifier, start, length, value);
            output.Add(token);

            if (value.StartsWith("@"))
                storyboard = true;

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the start of an Identifier token.
        /// </summary>
        private static Boolean IsValidStartIdentifier(Char c)
        {
            return Char.IsLetter(c) || c == '_' || c == '.' || c == '#' || c == '@';
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
