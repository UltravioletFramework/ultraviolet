using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Contains methods for parsing an Ultraviolet Stylesheet (UVSS) lexical token stream into
    /// an instance of the <see cref="UvssDocument"/> class.
    /// </summary>
    internal sealed partial class UvssParser
    {
        /// <summary>
        /// Parses a lexical token stream into an instance of the <see cref="UvssDocument"/> class.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="tokens">The token stream to parse.</param>
        /// <returns>The new instance of <see cref="UvssDocument"/> that was created.</returns>
        public UvssDocument Parse(String source, IList<UvssLexerToken> tokens)
        {
            Contract.Require(source, "source");
            Contract.Require(tokens, "input");

            var state = new UvssParserState(source, tokens);

            var rules = new List<UvssRule>();
            var rule  = default(UvssRule);

            while ((rule = ConsumeRule(state)) != null) 
            {
                rules.Add(rule);
            }

            return new UvssDocument(rules);
        }

        /// <summary>
        /// Throws an exception indicating that a syntax error was reached.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="state">The parser state.</param>
        private static void ThrowSyntaxException(StringResource message, UvssParserState state)
        {
            const Int32 CalloutLength = 32;

            var messageStart  = Math.Max(0, state.Position - (CalloutLength / 2));
            var messageLength = Math.Min(CalloutLength, state.Tokens.Count - messageStart);

            var tokenFirst = state.Tokens[messageStart];
            var tokenLast  = state.Tokens[messageStart + messageLength - 1];

            var tokenSourceLength = 0;
            for (int i = messageStart; i < messageStart + messageLength; i++)
            {
                tokenSourceLength += state.Tokens[i].Length;
            }
            var tokenSource = state.Source.Substring(tokenFirst.Start, tokenSourceLength);

            throw new UvssException(message.Format(tokenSource));
        }

        /// <summary>
        /// Determines whether the specified string is a selector for an element.
        /// </summary>
        /// <param name="s">The string to evaluate.</param>
        /// <returns><c>true</c> if the specified string is a selector for an element; otherwise, false.</returns>
        private static Boolean IsSelectorForElement(String s)
        {
            return !IsSelectorForID(s) && !IsSelectorForClass(s);
        }

        /// <summary>
        /// Determines whether the specified string is a selector for an ID.
        /// </summary>
        /// <param name="s">The string to evaluate.</param>
        /// <returns><c>true</c> if the specified string is a selector for an ID; otherwise, false.</returns>
        private static Boolean IsSelectorForID(String s)
        {
            return s[0] == '#';
        }

        /// <summary>
        /// Determines whether the specified string is a selector for a class.
        /// </summary>
        /// <param name="s">The string to evaluate.</param>
        /// <returns><c>true</c> if the specified string is a selector for a class; otherwise, false.</returns>
        private static Boolean IsSelectorForClass(String s)
        {
            return s[0] == '.';
        }

        /// <summary>
        /// Retrieves all of the tokens between a matching pair of tokens.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="start">The type of the first token in the matching pair.</param>
        /// <param name="end">The type of the second token in the matching pair.</param>
        /// <returns>A collection containing the tokens between the specified matching pair of tokens.</returns>
        private static IList<UvssLexerToken> GetTokensBetweenMatchingPair(UvssParserState state, UvssLexerTokenType start, UvssLexerTokenType end)
        {
            if (state.CurrentToken.TokenType != start)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var level  = 1;
            var tokens = new List<UvssLexerToken>();

            state.Advance();

            while (true)
            {
                if (state.IsPastEndOfStream)
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxUnterminatedSequence, state);

                var token = state.Consume();
                if (token.TokenType == start)
                {
                    level++;
                }
                else if (token.TokenType == end)
                {
                    level--;
                }

                if (level == 0)
                {
                    break;
                }

                tokens.Add(token);
            }

            return tokens;
        }

        /// <summary>
        /// Retrieves all of the tokens between a matching pair of curly braces.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A collection containing the tokens between the specified matching pair of tokens.</returns>
        private static IList<UvssLexerToken> GetTokensBetweenCurlyBraces(UvssParserState state)
        {
            return GetTokensBetweenMatchingPair(state, UvssLexerTokenType.OpenCurlyBrace, UvssLexerTokenType.CloseCurlyBrace);
        }

        /// <summary>
        /// Retrieves all of the tokens between a matching pair of parentheses.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A collection containing the tokens between the specified matching pair of tokens.</returns>
        private static IList<UvssLexerToken> GetTokensBetweenParentheses(UvssParserState state)
        {
            return GetTokensBetweenMatchingPair(state, UvssLexerTokenType.OpenParenthesis, UvssLexerTokenType.CloseParenthesis);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS rule.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssRule"/> object representing the rule that was consumed.</returns>
        private static UvssRule ConsumeRule(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var selectors = ConsumeSelectorList(state);
            var styles    = ConsumeStyleList(state);

            return new UvssRule(selectors, styles);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS selector list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssSelectorList"/> object representing the rule that was consumed.</returns>
        private static UvssSelectorList ConsumeSelectorList(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var selectors = new List<UvssSelector>();

            while (true)
            {
                var selector = ConsumeSelector(state);
                selectors.Add(selector);

                state.AdvanceBeyondWhiteSpace();

                if (state.IsPastEndOfStream)
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                if (state.CurrentToken.TokenType != UvssLexerTokenType.Comma)
                    break;

                state.Advance();
            }

            return new UvssSelectorList(selectors);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS selector.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssSelector"/> object representing the rule that was consumed.</returns>
        private static UvssSelector ConsumeSelector(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var parts = new List<UvssSelectorPart>();

            while (true)
            {
                var part = ConsumeSelectorPart(state);
                if (part != null)
                {
                    parts.Add(part);
                }

                if (state.IsPastEndOfStream)
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                if (state.CurrentToken.TokenType == UvssLexerTokenType.Comma)
                    break;

                if (state.CurrentToken.TokenType != UvssLexerTokenType.WhiteSpace)
                    break;

                state.Advance();
            }

            return new UvssSelector(parts);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS selector part.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssSelectorPart"/> object representing the rule that was consumed.</returns>
        private static UvssSelectorPart ConsumeSelectorPart(UvssParserState state)
        {
            var element = default(String);
            var id      = default(String);
            var classes = new List<String>();
            var valid   = false;

            while (true)
            {
                if (state.IsPastEndOfStream)
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                var token = state.CurrentToken;

                if (token.TokenType == UvssLexerTokenType.WhiteSpace)
                    break;

                if (token.TokenType == UvssLexerTokenType.Comma)
                    break;

                if (token.TokenType == UvssLexerTokenType.OpenCurlyBrace)
                    break;

                if (token.TokenType == UvssLexerTokenType.Identifier)
                {
                    state.Advance();

                    if (IsSelectorForElement(token.Value))
                    {
                        if (element != null)
                            ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                        valid   = true;
                        element = token.Value;
                        continue;
                    }

                    if (IsSelectorForID(token.Value))
                    {
                        if (id != null)
                            ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                        valid = true;
                        id    = token.Value;
                        continue;
                    }

                    valid = true;
                    classes.Add(token.Value);
                    continue;
                }

                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);
            }

            return valid ? new UvssSelectorPart(element, id, classes) : null;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStyleList"/> object representing the rule that was consumed.</returns>
        private static UvssStyleList ConsumeStyleList(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var styleListTokens = GetTokensBetweenCurlyBraces(state);
            var styleListState  = new UvssParserState(state.Source, styleListTokens);

            var style  = default(UvssStyle);
            var styles = new List<UvssStyle>();

            while ((style = ConsumeStyle(styleListState)) != null)
            {
                styles.Add(style);
            }

            return new UvssStyleList(styles);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStyle"/> object representing the rule that was consumed.</returns>
        private static UvssStyle ConsumeStyle(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var nameToken = state.TryConsumeNonWhiteSpace();
            if (nameToken == null || nameToken.Value.TokenType != UvssLexerTokenType.Identifier)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var colonToken = state.TryConsumeNonWhiteSpace();
            if (colonToken == null || colonToken.Value.TokenType != UvssLexerTokenType.Colon)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var valueTokens = new List<UvssLexerToken>();
            while (!state.IsPastEndOfStream)
            {
                var token = state.Consume();
                if (token.TokenType == UvssLexerTokenType.Semicolon)
                {
                    break;
                }
                valueTokens.Add(token);
            }

            var name  = nameToken.Value.Value;
            var value = String.Join(String.Empty, valueTokens.Select(x => x.Value));

            return new UvssStyle(name, value);
        }
    }
}
