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
            var output                = new List<UvssLexerToken>();
            var ix                    = 0;
            var line                  = 0;
            var braces                = 0;
            var parens                = 0;
            var storyboard            = false;
            var arglist               = false;
            var trigger               = false;
            var triggerInit           = false;
            var triggerInitEvent      = false;
            var triggerBracesExpected = 0;
            var triggerBracesSeen     = 0;

            while (ix < input.Length)
            {
                if (ConsumeWhiteSpace(input, output, ref line, ref ix))
                    continue;
                if (ConsumeSingleLineComment(input, output, line, ref ix))
                    continue;
                if (ConsumeMultiLineComment(input, output, ref line, ref ix))
                    continue;

                if (triggerInit || triggerInitEvent)
                {
                    if (ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                    {
                        if (triggerInitEvent)
                        {
                            triggerInitEvent = false;
                            continue;
                        }
                        else
                        {
                            var triggerType = output[output.Count - 1].Value;
                            if (String.Equals(triggerType, "property", StringComparison.InvariantCultureIgnoreCase))
                            {
                                triggerInit           = false;
                                triggerInitEvent      = false;
                                triggerBracesExpected = 1;
                                continue;
                            }
                            if (String.Equals(triggerType, "event", StringComparison.InvariantCultureIgnoreCase))
                            {
                                triggerInit           = false;
                                triggerInitEvent      = true;
                                triggerBracesExpected = 0;
                                continue;
                            }
                            throw new UvssException(PresentationStrings.StylesheetInvalidTriggerType.Format(line, triggerType));
                        }
                    }
                }
                else
                {
                    if (trigger)
                    {
                        if (triggerBracesExpected == triggerBracesSeen)
                        {
                            if (ConsumeComma(input, output, line, ref ix))
                            {
                                triggerBracesExpected++;
                                continue;
                            }
                            else
                            {
                                trigger = false;
                            }
                        }
                        if (ConsumeComparisonOperator(input, output, ref line, ref ix))
                            continue;
                    }

                    if (!storyboard && !arglist && !trigger)
                    {
                        if (braces == 1)
                        {
                            if (ConsumeStyleName(input, output, line, ref ix))
                            {
                                var token = output[output.Count - 1];
                                if (token.TokenType == UvssLexerTokenType.TriggerKeyword)
                                {
                                    triggerBracesExpected = 0;
                                    triggerBracesSeen     = 0;
                                    trigger               = true;
                                    triggerInit           = true;
                                }
                                continue;
                            }
                            if (ConsumeStyleQualifier(input, output, line, ref ix))
                                continue;
                        }
                    }
                    if (ConsumeChildSelector(input, output, line, ref ix))
                        continue;
                    if (ConsumeUniversalSelector(input, output, line, ref ix))
                        continue;
                    if (ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                        continue;
                    if (ConsumeNumber(input, output, line, ref ix))
                        continue;
                    if (ConsumeString(input, output, line, ref ix))
                        continue;
                    if (ConsumeOpenParenthesis(input, output, line, ref ix))
                    {
                        parens++;
                        arglist = true;
                        continue;
                    }
                    if (ConsumeCloseParenthesis(input, output, line, ref ix))
                    {
                        parens--;
                        if (parens == 0)
                        {
                            arglist = false;
                        }
                        continue;
                    }
                    if (ConsumeOpenCurlyBrace(input, output, line, ref ix))
                    {
                        braces++;
                        continue;
                    }
                    if (ConsumeCloseCurlyBrace(input, output, line, ref ix))
                    {
                        braces--;
                        if (trigger)
                        {
                            triggerBracesSeen++;
                        }
                        if (braces == 0)
                        {
                            storyboard = false;
                        }
                        continue;
                    }
                    if (ConsumeColon(input, output, line, ref ix))
                        continue;
                    if (ConsumeSemicolon(input, output, line, ref ix))
                        continue;
                    if (ConsumeComma(input, output, line, ref ix))
                        continue;
                }

                throw new UvssException(PresentationStrings.StylesheetInvalidCharacter.Format(line, input[ix]));
            }

            return output;
        }

        /// <summary>
        /// Attempts to consume a WhiteSpace token.
        /// </summary>
        private static Boolean ConsumeWhiteSpace(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
        {
            if (!IsValidStartWhiteSpace(input[ix]))
                return false;

            var start   = ix;
            var length  = 0;
            var newline = false;

            do
            {
                if (input[ix] == '\n')
                {
                    newline = true;
                }
                ix++;
                length++;
            } while (ix < input.Length && IsValidInWhiteSpace(input[ix]));

            var token = new UvssLexerToken(UvssLexerTokenType.WhiteSpace, start, length, line);
            output.Add(token);

            if (newline)
            {
                line++;
            }

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a WhiteSpace token.
        /// </summary>
        private static Boolean IsValidStartWhiteSpace(Char c)
        {
            return Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Evaluates whether the specified character is valid in a WhiteSpace token.
        /// </summary>
        private static Boolean IsValidInWhiteSpace(Char c)
        {
            return c != '\n' && Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the beginning of a SingleLineComment token.
        /// </summary>
        private static Boolean IsValidStartSingleLineComment(String input, Int32 ix)
        {
            return ix + 1 < input.Length && input[ix] == '/' && input[ix + 1] == '/';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the end of a SingleLineComment token.
        /// </summary>
        private static Boolean IsValidEndSingleLineComment(String input, Int32 ix)
        {
            return input[ix] == '\r' || input[ix] == '\n';
        }

        /// <summary>
        /// Attempts to consume a SingleLineComment token.
        /// </summary>
        private static Boolean ConsumeSingleLineComment(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (!IsValidStartSingleLineComment(input, ix))
                return false;

            var start  = ix + 2;
            var length = 0;

            for (var position = start; position < input.Length; position++)
            {
                if (IsValidEndSingleLineComment(input, position))
                    break;

                length++;
            }

            var totalLength = length + "//".Length;
            var token = new UvssLexerToken(UvssLexerTokenType.SingleLineComment, ix, totalLength, line, input.Substring(start, length));
            output.Add(token);

            ix += totalLength;

            return true;
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the beginning of a MultiLineComment token.
        /// </summary>
        private static Boolean IsValidStartMultiLineComment(String input, Int32 ix)
        {
            return ix + 1 < input.Length && input[ix] == '/' && input[ix + 1] == '*';
        }

        /// <summary>
        /// Evaluates whether the specified character is valid at the end of a MultiLineComment token.
        /// </summary>
        private static Boolean IsValidEndMultiLineComment(String input, Int32 ix)
        {
            return ix + 1 < input.Length && input[ix] == '*' && input[ix + 1] == '/';
        }

        /// <summary>
        /// Attempts to consume a MultiLineComment token.
        /// </summary>
        private static Boolean ConsumeMultiLineComment(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
        {
            if (!IsValidStartMultiLineComment(input, ix))
                return false;

            var start  = ix + 2;
            var length = 0;

            for (var position = start; position < input.Length; position++)
            {
                if (input[position] == '\n')
                    line++;

                if (IsValidEndMultiLineComment(input, position))
                    break;

                length++;
            }

            var totalLength = length + "/**/".Length;
            var token = new UvssLexerToken(UvssLexerTokenType.MultiLineComment, ix, totalLength, line, input.Substring(start, length));
            output.Add(token);

            ix += totalLength;

            return true;
        }

        /// <summary>
        /// Attempts to consume a ComparisonOperator token.
        /// </summary>
        private static Boolean ConsumeComparisonOperator(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
        {
            var c1 = input[ix];
            var c2 = ix + 1 < input.Length ? input[ix + 1] : default(Char);
            var op = "";

            switch (c1)
            {
                case '=':
                    op = "=";
                    break;

                case '<':
                    if (c2 == '>')
                    {
                        op = "<>";
                        break;
                    }
                    if (c2 == '=')
                    {
                        op = "<=";
                        break;
                    }
                    op = "<";
                    break;

                case '>':
                    if (c2 == '=')
                    {
                        op = ">=";
                        break;
                    }
                    op = ">";
                    break;

                default:
                    return false;
            }

            var token = new UvssLexerToken(UvssLexerTokenType.ComparisonOperator, ix, op.Length, line, op);
            output.Add(token);

            ix += op.Length;

            return true;
        }

        /// <summary>
        /// Attempts to consume a StyleName token.
        /// </summary>
        private static Boolean ConsumeStyleName(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (!IsValidStartStyleName(input[ix]))
                return false;

            var start     = ix++;
            var length    = 1;
            var qualified = false;

            while (ix < input.Length && IsValidInStyleName(input[ix], ref qualified)) { ix++; length++; }

            var value = input.Substring(start, length);
            var type  = String.Equals(value, "trigger", StringComparison.InvariantCultureIgnoreCase) ? UvssLexerTokenType.TriggerKeyword : UvssLexerTokenType.StyleName;
            var token = new UvssLexerToken(type, start, length, line, value);
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
        private static Boolean ConsumeStyleQualifier(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (!IsValidStartStyleQualifier(input[ix]))
                return false;

            var start     = ix++;
            var length    = 1;

            while (ix < input.Length && IsValidInStyleQualifier(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.StyleQualifier, start, length, line, value);
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
        /// Attempts to consume an Identifier token.
        /// </summary>
        private static Boolean ConsumeIdentifier(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix, ref Boolean storyboard)
        {
            if (!IsValidStartIdentifier(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;

            while (ix < input.Length && IsValidInIdentifier(input[ix])) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.Identifier, start, length, line, value);
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
        /// Attempts to consume a ChildSelector token.
        /// </summary>
        private static Boolean ConsumeChildSelector(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (input[ix] != '>')
                return false;

            var token = new UvssLexerToken(UvssLexerTokenType.ChildSelector, ix++, 1, line);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Attempts to consume a UniversalSelector token.
        /// </summary>
        private static Boolean ConsumeUniversalSelector(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (input[ix] != '*')
                return false;

            var token = new UvssLexerToken(UvssLexerTokenType.UniversalSelector, ix++, 1, line);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Attempts to consume a Number token.
        /// </summary>
        private static Boolean ConsumeNumber(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (!Char.IsDigit(input[ix]))
                return false;

            var start  = ix++;
            var length = 1;
            var dec    = false;

            while (ix < input.Length && IsValidInNumber(input[ix], ref dec)) { ix++; length++; }

            var value = input.Substring(start, length);
            var token = new UvssLexerToken(UvssLexerTokenType.Number, start, length, line, value);
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
        private static Boolean ConsumeString(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (input[ix] != '"')
                return false;

            var start  = ix++;
            var length = 2;

            while (ix < input.Length)
            {
                if (ix + 1 == input.Length && input[ix] != '"')
                {
                    throw new UvssException(PresentationStrings.StylesheetSyntaxUnterminatedString.Format(line));
                }

                var c = input[ix++];
                if (c == '"')
                    break;

                length++;
            }

            var value = input.Substring(start + 1, length - 2);
            var token = new UvssLexerToken(UvssLexerTokenType.String, start, length, line, value);
            output.Add(token);

            return true;
        }

        /// <summary>
        /// Attempts to consume a punctuation token.
        /// </summary>
        private static Boolean ConsumePunctuation(UvssLexerTokenType type, Char punctuation, String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            if (input[ix] == punctuation)
            {
                var token = new UvssLexerToken(type, ix, 1, line);
                output.Add(token);

                ix++;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to consume an OpenParenthesis token.
        /// </summary>
        private static Boolean ConsumeOpenParenthesis(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.OpenParenthesis, '(', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume a CloseParenthesis token.
        /// </summary>
        private static Boolean ConsumeCloseParenthesis(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.CloseParenthesis, ')', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume an OpenCurlyBrace token.
        /// </summary>
        private static Boolean ConsumeOpenCurlyBrace(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.OpenCurlyBrace, '{', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume a CloseCurlyBrace token.
        /// </summary>
        private static Boolean ConsumeCloseCurlyBrace(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.CloseCurlyBrace, '}', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Colon token.
        /// </summary>
        private static Boolean ConsumeColon(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Colon, ':', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Semicolon token.
        /// </summary>
        private static Boolean ConsumeSemicolon(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Semicolon, ';', input, output, line, ref ix);
        }

        /// <summary>
        /// Attempts to consume a Comma token.
        /// </summary>
        private static Boolean ConsumeComma(String input, IList<UvssLexerToken> output, Int32 line, ref Int32 ix)
        {
            return ConsumePunctuation(UvssLexerTokenType.Comma, ',', input, output, line, ref ix);
        }
    }
}
