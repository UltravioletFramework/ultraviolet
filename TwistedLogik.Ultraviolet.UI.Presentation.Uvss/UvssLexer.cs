using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents an Ultraviolet StyleSheets (UVSS) lexer which takes a raw string as its input and produces a 
    /// stream of <see cref="UvssLexerToken"/> instances. 
    /// </summary>
    public class UvssLexer
    {
        /// <summary>
        /// Initializes the <see cref="UvssLexer"/> type.
        /// </summary>
        static UvssLexer()
        {
            regexLexer = CreateLexerRegex();
            regexGroupNames = regexLexer.GetGroupNames();
        }

        /// <summary>
        /// Tokenizes the specified Ultraviolet StyleSheets (UVSS) string.
        /// </summary>
        /// <param name="source">The source string to tokenize.</param>
        /// <param name="options">The lexer's configurable options, or null to use the default options.</param>
        /// <returns>A list containing the tokens produced by tokenizing the specified string.</returns>
        public IList<UvssLexerToken> Tokenize(String source, UvssLexerOptions options = null)
        {
            options = options ?? UvssLexerOptions.Default;

            var output = new List<UvssLexerToken>();

            var parenLevel = 0;
            var curlyLevel = 0;

            var lineIndex = 1;
            var columnIndex = 1;

            var position = 0;
            var tokenType = default(UvssLexerTokenType);
            var tokenText = default(String);
            var token = default(UvssLexerToken);

            var readValueImmediately = false;
            var readValueAtNextCurlyBrace = false;

            while (position < source.Length)
            {
                if ((readValueImmediately) ||
                    (readValueAtNextCurlyBrace && tokenType == UvssLexerTokenType.OpenCurlyBrace))
                {
                    token = readValueImmediately ?
                        ReadValueUntilSemiColon(source, position, lineIndex, columnIndex) :
                        ReadValueUntilCloseCurlyBrace(source, position, lineIndex, columnIndex);

                    tokenType = token.Type;
                    tokenText = token.Text;

                    readValueImmediately = false;
                    readValueAtNextCurlyBrace = false;
                }
                else
                {
                    var match = regexLexer.Match(source, position);
                    if (match.Success)
                    {
                        if (!GetTokenInfoFromRegexMatch(match, out tokenType, out tokenText))
                        {
                            var errorMessage = String.Format(UvssStrings.LexerInvalidToken, 
                                match.Value, lineIndex, columnIndex);
                            throw new UvssLexerException(errorMessage, ErrorCodeInvalidToken, lineIndex, columnIndex);
                        }

                        ReadExtendedToken(source, position, ref tokenType, ref tokenText);

                        if (tokenType == UvssLexerTokenType.OpenParenthesis)
                            parenLevel++;

                        if (tokenType == UvssLexerTokenType.CloseParenthesis)
                            parenLevel--;

                        if (tokenType == UvssLexerTokenType.OpenCurlyBrace)
                            curlyLevel++;

                        if (tokenType == UvssLexerTokenType.CloseCurlyBrace)
                            curlyLevel--;

                        HandlePendingValueTokens(parenLevel, curlyLevel, tokenType, tokenText,
                            ref readValueImmediately, ref readValueAtNextCurlyBrace);

                        var sourceOffset = position;
                        var sourceLength = tokenText.Length;
                        var sourceLine = lineIndex;
                        var sourceColumn = columnIndex;

                        token = new UvssLexerToken(tokenType,
                            sourceOffset, sourceLength, sourceLine, sourceColumn, tokenText);
                    }
                    else
                    {
                        var errorMessage = String.Format(UvssStrings.LexerInvalidSymbol,
                            source[position], lineIndex, columnIndex);
                        throw new UvssLexerException(errorMessage, ErrorCodeInvalidSymbol, lineIndex, columnIndex);
                    }
                }

                position += EmitToken(output, token, ref lineIndex, ref columnIndex, options);
            }

            return output;
        }

        /// <summary>
        /// Given a token type name, returns the corresponding <see cref="UvssLexerTokenType"/> value.
        /// </summary>
        /// <param name="name">The name of the token type.</param>
        /// <returns>The <see cref="UvssLexerTokenType"/> value that corresponds to the specified name.</returns>
        private static UvssLexerTokenType GetTokenTypeFromName(String name)
        {
            switch (name)
            {
                case nameof(UvssLexerTokenType.Unknown):
                    return UvssLexerTokenType.Unknown;

                case nameof(UvssLexerTokenType.Comment):
                    return UvssLexerTokenType.Comment;

                case nameof(UvssLexerTokenType.WhiteSpace):
                    return UvssLexerTokenType.WhiteSpace;

                case nameof(UvssLexerTokenType.Keyword):
                    return UvssLexerTokenType.Keyword;

                case nameof(UvssLexerTokenType.Identifier):
                    return UvssLexerTokenType.Identifier;

                case nameof(UvssLexerTokenType.Number):
                    return UvssLexerTokenType.Number;

                case nameof(UvssLexerTokenType.Value):
                    return UvssLexerTokenType.Value;

                case nameof(UvssLexerTokenType.Comma):
                    return UvssLexerTokenType.Comma;

                case nameof(UvssLexerTokenType.Colon):
                    return UvssLexerTokenType.Colon;

                case nameof(UvssLexerTokenType.SemiColon):
                    return UvssLexerTokenType.SemiColon;

                case nameof(UvssLexerTokenType.AtSign):
                    return UvssLexerTokenType.AtSign;

                case nameof(UvssLexerTokenType.Hash):
                    return UvssLexerTokenType.Hash;

                case nameof(UvssLexerTokenType.Period):
                    return UvssLexerTokenType.Period;

                case nameof(UvssLexerTokenType.ExclamationMark):
                    return UvssLexerTokenType.ExclamationMark;

                case nameof(UvssLexerTokenType.OpenParenthesis):
                    return UvssLexerTokenType.OpenParenthesis;

                case nameof(UvssLexerTokenType.CloseParenthesis):
                    return UvssLexerTokenType.CloseParenthesis;

                case nameof(UvssLexerTokenType.OpenCurlyBrace):
                    return UvssLexerTokenType.OpenCurlyBrace;

                case nameof(UvssLexerTokenType.CloseCurlyBrace):
                    return UvssLexerTokenType.CloseCurlyBrace;

                case nameof(UvssLexerTokenType.UniversalSelector):
                    return UvssLexerTokenType.UniversalSelector;

                case nameof(UvssLexerTokenType.TemplatedChildCombinator):
                    return UvssLexerTokenType.TemplatedChildCombinator;

                case nameof(UvssLexerTokenType.LogicalChildCombinator):
                    return UvssLexerTokenType.LogicalChildCombinator;
                    
                case nameof(UvssLexerTokenType.EqualsOperator):
                    return UvssLexerTokenType.EqualsOperator;

                case nameof(UvssLexerTokenType.NotEqualsOperator):
                    return UvssLexerTokenType.NotEqualsOperator;

                case nameof(UvssLexerTokenType.LessThanOperator):
                    return UvssLexerTokenType.LessThanOperator;

                case nameof(UvssLexerTokenType.LessThanEqualsOperator):
                    return UvssLexerTokenType.LessThanEqualsOperator;

                case nameof(UvssLexerTokenType.GreaterThanOperator):
                    return UvssLexerTokenType.GreaterThanOperator;

                case nameof(UvssLexerTokenType.GreaterThanEqualsOperator):
                    return UvssLexerTokenType.GreaterThanEqualsOperator;

                case nameof(UvssLexerTokenType.NavigationExpressionOperator):
                    return UvssLexerTokenType.NavigationExpressionOperator;

                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        /// <summary>
        /// Given a <see cref="Match"/> instance, determines the type and text of the corresponding lexer token.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> instance to evaluate.</param>
        /// <param name="tokenType">The lexed token's type.</param>
        /// <param name="tokenText">The lexed token's text.</param>
        /// <returns>true if the <see cref="Match"/> represented a valid token; otherwise, false.</returns>
        private static Boolean GetTokenInfoFromRegexMatch(Match match,
            out UvssLexerTokenType tokenType, out String tokenText)
        {
            for (int i = 1; i < match.Groups.Count; i++)
            {
                var group = match.Groups[i];
                if (group.Success)
                {
                    tokenType = GetTokenTypeFromName(regexGroupNames[i]);
                    tokenText = group.Value;
                    return true;
                }
            }

            tokenType = UvssLexerTokenType.Unknown;
            tokenText = null;

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether there is an !important qualifier at the specified
        /// position within the source string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which to begin evaluation.</param>
        /// <returns>true if there is an !important qualifier at the specified position; otherwise, false.</returns>
        private static Boolean GetImportantQualifierAtPosition(String source, Int32 position)
        {
            const String ImportantQualifier = "!important";

            for (int i = 0; i < ImportantQualifier.Length; i++)
            {
                if (source[position + i] != ImportantQualifier[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Reads a <see cref="UvssLexerTokenType.Value"/> token starting at the specified position and
        /// ending at the end of the source string or at the next instance of the specified character, whichever
        /// comes first.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which the token begins.</param>
        /// <param name="lineIndex">The index of the current line.</param>
        /// <param name="columnIndex">The index of the current column.</param>
        /// <param name="symbol">The symbol that terminates the value.</param>
        /// <param name="terminateOnQualifiers">If true, the value token will be terminated
        /// upon reaching an !important qualifier.</param>
        /// <returns>A <see cref="UvssLexerToken"/> that represents the specified value.</returns>
        private static UvssLexerToken ReadValueUntilSymbol(String source, Int32 position,
            Int32 lineIndex, Int32 columnIndex, Char symbol, Boolean terminateOnQualifiers)
        {
            var offset = position;
            var length = 0;

            for (int i = position; i < source.Length; i++)
            {
                if (terminateOnQualifiers && GetImportantQualifierAtPosition(source, i))
                    break;

                if (source[i] == symbol)
                    break;

                length++;
            }

            var text = source.Substring(offset, length);
            return new UvssLexerToken(UvssLexerTokenType.Value, offset, length, lineIndex, columnIndex, text);
        }

        /// <summary>
        /// Reads a <see cref="UvssLexerTokenType.Value"/> token starting at the specified position and
        /// ending at the end of the source string or at the next semi-colon, whichever comes first.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which the token begins.</param>
        /// <param name="lineIndex">The index of the current line.</param>
        /// <param name="columnIndex">The index of the current column.</param>
        /// <returns>A <see cref="UvssLexerToken"/> that represents the specified value.</returns>
        private static UvssLexerToken ReadValueUntilSemiColon(String source, Int32 position,
            Int32 lineIndex, Int32 columnIndex)
        {
            return ReadValueUntilSymbol(source, position, lineIndex, columnIndex, ';', true);
        }

        /// <summary>
        /// Reads a <see cref="UvssLexerTokenType.Value"/> token starting at the specified position and
        /// ending at the end of the source string or at the next close curly brace, whichever comes first.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which the token begins.</param>
        /// <param name="lineIndex">The index of the current line.</param>
        /// <param name="columnIndex">The index of the current column.</param>
        /// <returns>A <see cref="UvssLexerToken"/> that represents the specified value.</returns>
        private static UvssLexerToken ReadValueUntilCloseCurlyBrace(String source, Int32 position,
            Int32 lineIndex, Int32 columnIndex)
        {
            return ReadValueUntilSymbol(source, position, lineIndex, columnIndex, '}', false);
        }

        /// <summary>
        /// Reads an extended token, such as a comment, which continues until reaching some termination condition.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which the token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The token's text.</param>
        private static void ReadExtendedToken(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            switch (tokenType)
            {
                case UvssLexerTokenType.Comment:
                    switch (tokenText)
                    {
                        case "//":
                            ReadSingleLineComment(source, position, ref tokenType, ref tokenText);
                            break;

                        case "/*":
                            ReadMultiLineComment(source, position, ref tokenType, ref tokenText);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Reads a single-line comment token starting at the specified position in the source.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The source position at which the comment token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The full text of the single-line comment token.</param>
        private static void ReadSingleLineComment(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            var offset = position;
            var length = tokenText.Length;

            for (int i = position + tokenText.Length; i < source.Length; i++)
            {
                if (source[i] == '\r' || source[i + 1] == '\n')
                {
                    length = i - offset;
                    break;
                }
            }

            tokenText = source.Substring(offset, length);
        }

        /// <summary>
        /// Reads a multi-line comment token starting at the specified position in the source.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The source position at which the comment token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The full text of the multi-line comment token.</param>
        private static void ReadMultiLineComment(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            var offset = position;
            var length = source.Length - position;

            for (int i = position + tokenText.Length; i < source.Length - 1; i++)
            {
                if (source[i] == '*' && source[i + 1] == '/')
                {
                    length = (i - offset) + 2;
                    break;
                }
            }

            tokenText = source.Substring(offset, length);
        }

        /// <summary>
        /// Determines whether there is an upcoming <see cref="UvssLexerTokenType.Value"/> token.
        /// </summary>
        /// <param name="parenLevel">The current level of nesting of parentheses.</param>
        /// <param name="curlyLevel">The current level of nesting of curly braces.</param>
        /// <param name="tokenType">The current token's type.</param>
        /// <param name="tokenText">The current token's text.</param>
        /// <param name="readValueImmediately">A value indicating whether a value should be read
        /// immediately after processing the current token.</param>
        /// <param name="readValueAtNextCurlyBrace">A value indicating whether a value should be read
        /// after processing the next opening curly brace.</param>
        private static void HandlePendingValueTokens(Int32 parenLevel, Int32 curlyLevel, UvssLexerTokenType tokenType,
            String tokenText, ref Boolean readValueImmediately, ref Boolean readValueAtNextCurlyBrace)
        {
            if (tokenType == UvssLexerTokenType.Colon)
            {
                if (curlyLevel == 1 && parenLevel == 0)
                {
                    readValueImmediately = true;
                }
                return;
            }

            if (tokenType == UvssLexerTokenType.EqualsOperator)
            {
                readValueAtNextCurlyBrace = true;
                return;
            }

            if (tokenType == UvssLexerTokenType.Keyword && (
                String.Equals(tokenText, "keyframe", StringComparison.Ordinal) ||
                String.Equals(tokenText, "set", StringComparison.Ordinal) ||
                String.Equals(tokenText, "play-sfx", StringComparison.Ordinal) ||
                String.Equals(tokenText, "play-storyboard", StringComparison.Ordinal)))
            {
                readValueAtNextCurlyBrace = true;
                return;
            }
        }

        /// <summary>
        /// Determines whether the lexer has advanced to a new line and updates the line index accordingly.
        /// In addition, this method moves the column index forward by the length of the current token, 
        /// performing tab expansion as necessary.
        /// </summary>
        /// <param name="text">The text of the token that is being emitted.</param>
        /// <param name="lineIndex">The current line index.</param>
        /// <param name="columnIndex">The current column index.</param>
        /// <param name="options">The lexer's configurable options.</param>
        private static void HandleLineAndColumnTracking(String text, 
            ref Int32 lineIndex, ref Int32 columnIndex, UvssLexerOptions options)
        {
            var newlineMatches = regexNewline.Matches(text);
            if (newlineMatches.Count > 0)
            {
                var lineStartOffset = 0;

                foreach (Match match in newlineMatches)
                {
                    lineIndex++;
                    lineStartOffset = match.Index + match.Length;
                }

                columnIndex = 1 + CalculateTokenLengthInColumns(text, lineStartOffset, options);
            }
            else
            {
                columnIndex += CalculateTokenLengthInColumns(text, 0, options);
            }
        }

        /// <summary>
        /// Calculates the length of the specified token text in columns, performing tab expansion if necessary.
        /// </summary>
        /// <param name="text">The text of the token that is being emitted.</param>
        /// <param name="offset">The offset into the token text at which to begin measuring length.</param>
        /// <param name="options">The lexer's configurable options.</param>
        /// <returns>The length of the specified token text in columns.</returns>
        private static Int32 CalculateTokenLengthInColumns(String text, Int32 offset, UvssLexerOptions options)
        {
            var length = 0;

            for (int i = offset; i < text.Length; i++)
                length += (text[i] == '\t') ? options.TabSize : 1;

            return length;
        }

        /// <summary>
        /// Counts the number of leading white space characters in the specified string.
        /// </summary>
        /// <param name="text">The string in which to count leading white space characters.</param>
        /// <returns>The number of leading white space characters in the specified string.</returns>
        private static Int32 CountLeadingWhiteSpace(String text)
        {
            var length = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (!Char.IsWhiteSpace(text[i]))
                    break;

                length++;
            }

            return length;
        }

        /// <summary>
        /// Counts the number of trailing white space characters in the specified string.
        /// </summary>
        /// <param name="text">The string in which to count trailing white space characters.</param>
        /// <returns>The number of trailing white space characters in the specified string.</returns>
        private static Int32 CountTrailingWhiteSpace(String text)
        {
            var length = 0;

            for (var i = text.Length - 1; i >= 0; i--)
            {
                if (!Char.IsWhiteSpace(text[i]))
                    break;

                length++;
            }

            return (length == text.Length) ? 0 : length;
        }

        /// <summary>
        /// Emits a token into the output stream.
        /// </summary>
        /// <param name="output">The output stream into which to emit the token.</param>
        /// <param name="token">The token to emit into the output stream.</param>
        /// <param name="lineIndex">The current line index.</param>
        /// <param name="columnIndex">The current column index.</param>
        /// <param name="options">The lexer's configurable options.</param>
        /// <returns>The number of characters by which to advance the lexer's position 
        /// within the source string.</returns>
        private static Int32 EmitToken(IList<UvssLexerToken> output, UvssLexerToken token,
            ref Int32 lineIndex, ref Int32 columnIndex, UvssLexerOptions options)
        {
            if (token.Type == UvssLexerTokenType.Value)
            {
                return EmitValueToken(output, token, ref lineIndex, ref columnIndex, options);
            }
            else
            {
                HandleLineAndColumnTracking(token.Text, ref lineIndex, ref columnIndex, options);
                output.Add(token);

                return token.Text.Length;
            }
        }

        /// <summary>
        /// Emits a value token (and potentially leading and trailing white space tokens) into the output stream.
        /// </summary>
        /// <param name="output">The output stream into which to emit the token.</param>
        /// <param name="token">The token to emit into the output stream.</param>
        /// <param name="lineIndex">The current line index.</param>
        /// <param name="columnIndex">The current column index.</param>
        /// <param name="options">The lexer's configurable options.</param>
        /// <returns>The number of characters by which to advance the lexer's position 
        /// within the source string.</returns>
        private static Int32 EmitValueToken(IList<UvssLexerToken> output, UvssLexerToken token,
            ref Int32 lineIndex, ref Int32 columnIndex, UvssLexerOptions options)
        {
            var wsCountLeading = CountLeadingWhiteSpace(token.Text);
            var wsCountTrailing = CountTrailingWhiteSpace(token.Text);
            var wsCountTotal = wsCountLeading + wsCountTrailing;

            // Emit the leading white space token, if there is one.
            if (wsCountLeading > 0)
            {
                var wsTokenLeading = new UvssLexerToken(UvssLexerTokenType.WhiteSpace,
                    token.SourceOffset, wsCountLeading,
                    lineIndex, columnIndex, token.Text.Substring(0, wsCountLeading));

                HandleLineAndColumnTracking(wsTokenLeading.Text, ref lineIndex, ref columnIndex, options);
                output.Add(wsTokenLeading);
            }

            // Emit the value token.
            if (wsCountTotal > 0)
            {
                var trimmedValueToken = new UvssLexerToken(UvssLexerTokenType.Value,
                    token.SourceOffset + wsCountLeading,
                    token.SourceLength - wsCountTotal,
                    lineIndex, columnIndex,
                    token.Text.Substring(wsCountLeading, token.Text.Length - wsCountTotal));

                HandleLineAndColumnTracking(trimmedValueToken.Text, ref lineIndex, ref columnIndex, options);
                output.Add(trimmedValueToken);
            }
            else
            {
                output.Add(token);
            }

            // Emit the trailing white space token, if there is one.
            if (wsCountTrailing > 0)
            {
                var wsTokenTrailing = new UvssLexerToken(UvssLexerTokenType.WhiteSpace,
                    token.SourceOffset + token.SourceLength - wsCountTrailing, wsCountTrailing,
                    lineIndex, columnIndex,
                    token.Text.Substring(token.Text.Length - wsCountTrailing, wsCountTrailing));

                HandleLineAndColumnTracking(wsTokenTrailing.Text, ref lineIndex, ref columnIndex, options);
                output.Add(wsTokenTrailing);
            }

            return token.Text.Length;
        }

        /// <summary>
        /// Creates a new <see cref="Regex"/> instance based on the lexer's production rules.
        /// </summary>
        /// <returns>The <see cref="Regex"/> instance which was created.></returns>
        private static Regex CreateLexerRegex()
        {
            var pattern = String.Join("|", ProductionRules.Select(x => $"(?<{x.Key}>{x.Value})"));
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            return regex;
        }

        // The production rules for each of the lexer's token types, in order from highest priority to lowest.
        private static readonly KeyValuePair<UvssLexerTokenType, String>[] ProductionRules = new[]
        {
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Comment,
                @"\G(//|/\*)"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.WhiteSpace,
                @"\G\s+"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Keyword,
                @"\Gplay-storyboard|" +
                @"\Gset-handled|" +
                @"\Gtransition|" +
                @"\G!important|" +
                @"\Ganimation|" +
                @"\Gplay-sfx|" +
                @"\Gproperty|" +
                @"\Gkeyframe|" +
                @"\Gtrigger(?!-root)|" +
                @"\Ghandled|" +
                @"\Gtarget|" +
                @"\Gevent|" +
                @"\Gset|" +
                @"\Gas"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Identifier,
                @"\G[_\-a-zA-Z][_\-a-zA-Z0-9]*"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Number,
                @"\G\d+(.\d+)?"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Comma,
                @"\G,"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Colon,
                @"\G:"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.SemiColon,
                @"\G;"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.AtSign,
                @"\G@"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Hash,
                @"\G#"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Period,
                @"\G\."),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.ExclamationMark,
                @"\G!"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.OpenParenthesis,
                @"\G\("),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.CloseParenthesis,
                @"\G\)"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.OpenCurlyBrace,
                @"\G\{"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.CloseCurlyBrace,
                @"\G\}"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.UniversalSelector,
                @"\G\*"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.TemplatedChildCombinator,
                @"\G\>\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.LogicalChildCombinator,
                @"\G\>\?"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.NotEqualsOperator,
                @"\G\<\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.LessThanEqualsOperator,
                @"\G\<="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThanEqualsOperator,
                @"\G\>="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.LessThanOperator,
                @"\G\<"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThanOperator,
                @"\G\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.EqualsOperator,
                @"\G="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.NavigationExpressionOperator,
                @"\G\|"),
        };

        // The regex engine used to create tokens.
        private static readonly Regex regexNewline = new Regex("\r\n|\n");
        private static readonly Regex regexLexer;
        private static readonly IList<String> regexGroupNames;

        // Lexer error codes.
        private const String ErrorCodeInvalidToken = "LEX0001";
        private const String ErrorCodeInvalidSymbol = "LEX0002";
    }
}
