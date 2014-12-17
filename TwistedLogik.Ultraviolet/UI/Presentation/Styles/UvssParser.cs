using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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

            var rules       = new List<UvssRule>();
            var rule        = default(UvssRule);
            var storyboards = new List<UvssStoryboard>();
            var storyboard  = default(UvssStoryboard);

            while (true)
            {
                if ((storyboard = ConsumeStoryboard(state)) != null)
                {
                    storyboards.Add(storyboard);
                    continue;
                }

                if ((rule = ConsumeRule(state)) != null)
                {
                    rules.Add(rule);
                    continue;
                }

                break;
            }

            return new UvssDocument(rules, storyboards);
        }

        /// <summary>
        /// Parses an Ultraviolet Stylesheet selector from the specified token stream.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="tokens">The token stream to parse.</param>
        /// <returns>The new instance of <see cref="UvssSelector"/> that was created.</returns>
        public UvssSelector ParseSelector(String source, IList<UvssLexerToken> tokens)
        {
            Contract.Require(source, "source");
            Contract.Require(tokens, "input");

            var state = new UvssParserState(source, tokens);
            return ConsumeSelector(state, true);
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
        /// Gets a value indicating whether the specified token is a match for the specified parameters.
        /// </summary>
        /// <param name="token">The token to evaluate.</param>
        /// <param name="type">The desired token type.</param>
        /// <param name="value">The desired token value.</param>
        /// <returns><c>true</c> if the specified token is a match; otherwise, <c>false</c>.</returns>
        private static Boolean MatchToken(UvssLexerToken? token, UvssLexerTokenType type, String value = null)
        {
            if (token == null)
                return false;

            if (token.Value.TokenType != type)
                return false;

            if (value != null && !String.Equals(token.Value.Value, value, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
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
        /// Consumes a sequence of tokens representing a storyboard.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboard"/> object representing the storyboard that was consumed.</returns>
        private static UvssStoryboard ConsumeStoryboard(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            if (state.CurrentToken.TokenType != UvssLexerTokenType.Identifier)
                return null;

            if (!state.CurrentToken.Value.StartsWith("@"))
                return null;

            var id           = state.Consume();
            var loopBehavior = ConsumeOptionalLoopBehavior(state);
            var targets      = ConsumeStoryboardTargetList(state);

            return new UvssStoryboard(id.Value.Substring(1), loopBehavior, targets);
        }

        /// <summary>
        /// Optionally consumes a token which represents a loop behavior, if such a token exists at
        /// the current position within the token stream.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>The <see cref="LoopBehavior"/> value which the consumed token represents.</returns>
        private static LoopBehavior ConsumeOptionalLoopBehavior(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            if (state.CurrentToken.TokenType == UvssLexerTokenType.Identifier)
            {
                return ConsumeLoopBehavior(state);
            }

            return LoopBehavior.None;
        }

        /// <summary>
        /// Consumes a token which represents a loop behavior.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>The <see cref="LoopBehavior"/> value which the consumed token represents.</returns>
        private static LoopBehavior ConsumeLoopBehavior(UvssParserState state)
        {
            if (state.CurrentToken.TokenType != UvssLexerTokenType.Identifier)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            if (String.Equals(state.CurrentToken.Value, "none", StringComparison.OrdinalIgnoreCase))
            {
                state.Consume();
                return LoopBehavior.None;
            }
            else if (String.Equals(state.CurrentToken.Value, "loop", StringComparison.OrdinalIgnoreCase))
            {
                state.Consume();
                return LoopBehavior.Loop;
            }
            else if (String.Equals(state.CurrentToken.Value, "reverse", StringComparison.OrdinalIgnoreCase))
            {
                state.Consume();
                return LoopBehavior.Reverse;
            }

            ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);
            return LoopBehavior.None;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a list of storyboard targets.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardTargetCollection"/> object representing the target collection that was consumed.</returns>
        private static UvssStoryboardTargetCollection ConsumeStoryboardTargetList(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var target        = default(UvssStoryboardTarget);
            var targets       = new UvssStoryboardTargetCollection();
            var targetsTokens = GetTokensBetweenCurlyBraces(state);
            var targetsState  = new UvssParserState(state.Source, targetsTokens);

            while ((target = ConsumeStoryboardTarget(targetsState)) != null)
            {
                targets.Add(target);
            }

            return targets;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a storyboard target.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardTarget"/> object representing the target that was consumed.</returns>
        private static UvssStoryboardTarget ConsumeStoryboardTarget(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var targetToken = state.TryConsumeNonWhiteSpace();
            if (!MatchToken(targetToken, UvssLexerTokenType.Identifier, "target"))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var filter = new UvssStoryboardTargetFilter();
            if (state.CurrentToken.TokenType != UvssLexerTokenType.Identifier)
            {
                filter.Add("element");
            }
            else
            {
                while (state.CurrentToken.TokenType == UvssLexerTokenType.Identifier)
                {
                    if (state.IsPastEndOfStream)
                        ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                    var type = state.CurrentToken.Value;
                    filter.Add(type);

                    state.Consume();
                    state.AdvanceBeyondWhiteSpace();
                }
            }

            if (filter.Count == 0)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var selector = default(UvssSelector);
            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                var tokens      = GetTokensBetweenParentheses(state);
                var tokensState = new UvssParserState(state.Source, tokens);
                selector        = ConsumeSelector(tokensState, true);
            }
            state.AdvanceBeyondWhiteSpace();

            var animations = ConsumeStoryboardAnimationList(state);

            return new UvssStoryboardTarget(selector, filter, animations);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a list of storyboard animations.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardAnimationCollection"/> object representing the animation collection that was consumed.</returns>
        private static UvssStoryboardAnimationCollection ConsumeStoryboardAnimationList(UvssParserState state)
        {
            var animation        = default(UvssStoryboardAnimation);
            var animations       = new UvssStoryboardAnimationCollection();
            var animationsTokens = GetTokensBetweenCurlyBraces(state);
            var animationsState  = new UvssParserState(state.Source, animationsTokens);

            while ((animation = ConsumeStoryboardAnimation(animationsState)) != null)
            {
                animations.Add(animation);
            }

            return animations;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a storyboard animation.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardAnimation"/> object representing the animation that was consumed.</returns>
        private static UvssStoryboardAnimation ConsumeStoryboardAnimation(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var animationToken = state.TryConsumeNonWhiteSpace();
            if (!MatchToken(animationToken, UvssLexerTokenType.Identifier, "animation"))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var propertyToken = state.TryConsumeNonWhiteSpace();
            if (!MatchToken(propertyToken, UvssLexerTokenType.Identifier))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var keyframes = ConsumeStoryboardKeyframeList(state);

            return new UvssStoryboardAnimation(propertyToken.Value.Value, keyframes);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a list of storyboard animation keyframes.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardKeyframeCollection"/> object representing the collection of keyframes that was consumed.</returns>
        private static UvssStoryboardKeyframeCollection ConsumeStoryboardKeyframeList(UvssParserState state)
        {
            var keyframe        = default(UvssStoryboardKeyframe);
            var keyframes       = new UvssStoryboardKeyframeCollection();
            var keyframesTokens = GetTokensBetweenCurlyBraces(state);
            var keyframesState = new UvssParserState(state.Source, keyframesTokens);

            while ((keyframe = ConsumeStoryboardKeyframe(keyframesState)) != null)
            {
                keyframes.Add(keyframe);
            }

            return keyframes;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a storyboard keyframe.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardKeyframe"/> object representing the keyframe that was consumed.</returns>
        private static UvssStoryboardKeyframe ConsumeStoryboardKeyframe(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var keyframeToken = state.TryConsumeNonWhiteSpace();
            if (!MatchToken(keyframeToken, UvssLexerTokenType.Identifier, "keyframe"))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var time = 0.0;
            var timeToken = state.TryConsumeNonWhiteSpace();
            if (!MatchToken(timeToken, UvssLexerTokenType.Number))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            time = Double.Parse(timeToken.Value.Value);

            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            var easing = default(String);
            if (state.CurrentToken.TokenType == UvssLexerTokenType.Identifier)
            {
                easing = state.CurrentToken.Value;
                state.Consume();
                state.AdvanceBeyondWhiteSpace();
            }

            var valueTokens = GetTokensBetweenCurlyBraces(state);
            var value       = String.Join(String.Empty, valueTokens.Select(x => x.Value));

            return new UvssStoryboardKeyframe(easing, value, time);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS selector list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssSelectorCollection"/> object representing the selector list that was consumed.</returns>
        private static UvssSelectorCollection ConsumeSelectorList(UvssParserState state)
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

            return new UvssSelectorCollection(selectors);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS selector.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="allowEOF">A value indicating whether hitting the end of file is valid.</param>
        /// <returns>A new <see cref="UvssSelector"/> object representing the selector that was consumed.</returns>
        private static UvssSelector ConsumeSelector(UvssParserState state, Boolean allowEOF = false)
        {
            state.AdvanceBeyondWhiteSpace();

            var parts       = new List<UvssSelectorPart>();
            var pseudoClass = false;

            while (true)
            {
                var part = ConsumeSelectorPart(state, allowEOF);
                if (part != null)
                {
                    if (pseudoClass)
                        ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                    if (!String.IsNullOrEmpty(part.PseudoClass))
                        pseudoClass = true;

                    parts.Add(part);
                }

                if (state.IsPastEndOfStream)
                {
                    if (allowEOF)
                    {
                        break;
                    }
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);
                }

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
        /// <param name="allowEOF">A value indicating whether hitting the end of file is valid.</param>
        /// <returns>A new <see cref="UvssSelectorPart"/> object representing the selector part that was consumed.</returns>
        private static UvssSelectorPart ConsumeSelectorPart(UvssParserState state, Boolean allowEOF = false)
        {
            var element     = default(String);
            var id          = default(String);
            var pseudoClass = default(String);
            var classes     = new List<String>();
            var valid       = false;

            while (true)
            {
                if (state.IsPastEndOfStream)
                {
                    if (allowEOF)
                    {
                        break;
                    }
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);
                }

                var token = state.CurrentToken;

                if (token.TokenType == UvssLexerTokenType.WhiteSpace)
                    break;

                if (token.TokenType == UvssLexerTokenType.Comma)
                    break;

                if (token.TokenType == UvssLexerTokenType.OpenCurlyBrace)
                    break;

                if (token.TokenType == UvssLexerTokenType.PseudoClass)
                {
                    if (!String.IsNullOrEmpty(pseudoClass))
                        ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

                    state.Advance();

                    pseudoClass = token.Value;
                    continue;
                }

                if (token.TokenType == UvssLexerTokenType.Identifier)
                {
                    if (!String.IsNullOrEmpty(pseudoClass))
                        ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

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

            return valid ? new UvssSelectorPart(element, id, pseudoClass, classes) : null;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStyleCollection"/> object representing the style list that was consumed.</returns>
        private static UvssStyleCollection ConsumeStyleList(UvssParserState state)
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

            return new UvssStyleCollection(styles);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStyle"/> object representing the style that was consumed.</returns>
        private static UvssStyle ConsumeStyle(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return null;

            var qualifierImportant = false;

            var nameToken = state.TryConsumeNonWhiteSpace();
            if (nameToken == null || nameToken.Value.TokenType != UvssLexerTokenType.StyleName)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            UvssStyleArgumentsCollection arguments;
            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                arguments = ConsumeStyleArguments(state);
                ValidateStyleArguments(state, nameToken.Value.Value, arguments);
            }
            else
            {
                arguments = new UvssStyleArgumentsCollection(null);
            }

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
                if (token.TokenType == UvssLexerTokenType.StyleQualifier)
                {
                    if (String.Equals("!important", token.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        qualifierImportant = true;
                    }
                    continue;
                }
                valueTokens.Add(token);
            }

            var container = default(String);
            var name      = nameToken.Value.Value;
            var value     = String.Join(String.Empty, valueTokens.Select(x => x.Value));

            if (name.Contains('.'))
            {
                var nameParts = name.Split('.');
                container     = nameParts[0];
                name          = nameParts[1];
            }

            return new UvssStyle(arguments, container, name, value, qualifierImportant);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style argument list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStyleArgumentsCollection"/> object representing the argument list that was consumed.</returns>
        private static UvssStyleArgumentsCollection ConsumeStyleArguments(UvssParserState state)
        {
            var argsTokens = GetTokensBetweenParentheses(state);
            var argsState  = new UvssParserState(state.Source, argsTokens);
            var args       = new List<String>();

            while (true)
            {
                var token = argsState.TryConsumeNonWhiteSpace();
                if (token == null)
                    break;

                if (token.Value.TokenType == UvssLexerTokenType.Identifier ||
                    token.Value.TokenType == UvssLexerTokenType.String ||
                    token.Value.TokenType == UvssLexerTokenType.Number)
                {
                    args.Add(token.Value.Value);
                }

                var comma = argsState.TryConsumeNonWhiteSpace();
                if (comma == null)
                    break;

                if (comma.Value.TokenType != UvssLexerTokenType.Comma)
                    ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, argsState);
            }

            return new UvssStyleArgumentsCollection(args);
        }

        /// <summary>
        /// Validates a style argument list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="style">The name of the style being validated.</param>
        /// <param name="arguments">The style argument list being validated.</param>
        private static void ValidateStyleArguments(UvssParserState state, String style, UvssStyleArgumentsCollection arguments)
        {
            if (!String.Equals(style, "transition", StringComparison.OrdinalIgnoreCase))
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);

            if (arguments.Count != 2 && arguments.Count != 3)
                ThrowSyntaxException(LayoutStrings.StylesheetSyntaxError, state);
        }
    }
}
