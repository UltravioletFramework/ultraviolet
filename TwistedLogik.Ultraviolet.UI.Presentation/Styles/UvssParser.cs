using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Contains methods for parsing an Ultraviolet Style Sheet (UVSS) lexical token stream into
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
        /// Parses an Ultraviolet Style Sheet selector from the specified token stream.
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
        /// Gets the specified line of source code.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="line">The line of source code to retrieve.</param>
        /// <returns>The line of source code that was retrieved.</returns>
        private static String GetSourceLine(UvssParserState state, Int32 line)
        {
            var lineNumber  = state.CurrentToken.Line;
            var lineTokens  = state.Tokens.Where(x => !x.Value.StartsWith("\n") && x.Line == lineNumber);
            var errorLine   = String.Join(String.Empty, lineTokens.Select(x => x.Value)).Trim();

            return errorLine;
        }

        /// <summary>
        /// Throws an exception indicating that an expected token was not found.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="token">The invalid token.</param>
        /// <param name="expected">The expected token type.</param>
        private static void ThrowExpectedToken(UvssParserState state, UvssLexerToken token, UvssLexerTokenType expected)
        {
            var lineNumber = token.Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxExpectedToken.Format(lineNumber, token.TokenType, expected));
        }

        /// <summary>
        /// Throws an exception indicating that an expected value was not found.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="token">The invalid token.</param>
        /// <param name="expected">The expected value.</param>
        private static void ThrowExpectedValue(UvssParserState state, UvssLexerToken token, String expected)
        {
            var lineNumber = token.Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxExpectedValue.Format(lineNumber, token.Value, expected));
        }

        /// <summary>
        /// Throws an exception indicating that an unexpected token was reached.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="token">The invalid token.</param>
        private static void ThrowUnexpectedToken(UvssParserState state, UvssLexerToken token)
        {
            var lineNumber = token.Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxUnexpectedToken.Format(lineNumber, token.TokenType));
        }

        /// <summary>
        /// Throws an exception indicating that an unexpected value was reached.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="token">The invalid token.</param>
        private static void ThrowUnexpectedValue(UvssParserState state, UvssLexerToken token)
        {
            var lineNumber = token.Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxUnexpectedValue.Format(lineNumber, token.Value));
        }

        /// <summary>
        /// Throws an exception indicating that the end of file was reached unexpectedly.
        /// </summary>
        /// <param name="state">The parser state.</param>
        private static void ThrowUnexpectedEOF(UvssParserState state)
        {
            var lineNumber = state.Tokens.Last().Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxUnexpectedEOF.Format(lineNumber));
        }

        /// <summary>
        /// Throws an exception indicating that an unterminated sequence was encountered.
        /// </summary>
        /// <param name="state">The parser state.</param>
        private static void ThrowUnterminatedSequence(UvssParserState state)
        {
            var lineNumber = state.Tokens.Last().Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxUnterminatedSequence.Format(lineNumber));
        }

        /// <summary>
        /// Throws an exception indicating that a style has an invalid argument list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="style">The name of the style with invalid arguments.</param>
        private static void ThrowInvalidStyleArguments(UvssParserState state, String style)
        {
            var lineNumber = state.Tokens.Last().Line;
            throw new UvssException(PresentationStrings.StyleSheetSyntaxInvalidStyleArgs.Format(lineNumber, style));
        }

        /// <summary>
        /// Throws an exception if the specified token does not match the specified parameters.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="token">The token to evaluate.</param>
        /// <param name="type">The desired token type.</param>
        /// <param name="value">The desired token value.</param>
        private static void MatchTokenOrFail(UvssParserState state, UvssLexerToken? token, UvssLexerTokenType type, String value = null)
        {
            if (token == null)
                ThrowUnexpectedEOF(state);

            if (token.Value.TokenType != type)
                ThrowExpectedToken(state, token.Value, type);

            if (value != null && !String.Equals(token.Value.Value, value, StringComparison.OrdinalIgnoreCase))
                ThrowExpectedValue(state, token.Value, value);
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
                ThrowExpectedToken(state, state.CurrentToken, start);

            var level  = 1;
            var tokens = new List<UvssLexerToken>();

            state.Advance();

            while (true)
            {
                if (state.IsPastEndOfStream)
                    ThrowUnterminatedSequence(state);

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
        /// Gets the source string between a matching pair of curly braces.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>The source string between the specified matching pair of tokens.</returns>
        private static String GetStringBetweenCurlyBraces(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var valueTokens = GetTokensBetweenCurlyBraces(state);
            var value       = String.Join(String.Empty, valueTokens.Select(x => x.Value)).Trim();

            return value;
        }

        /// <summary>
        /// Advances the parser state beyond any current white space. If the end of the stream is reached,
        /// a syntax exception is thrown.
        /// </summary>
        /// <param name="state">The parser state.</param>
        private static void AdvanceBeyondWhiteSpaceOrFail(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
            {
                ThrowUnexpectedEOF(state);
            }
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
            var styles    = default(UvssStyleCollection);
            var triggers  = default(UvssTriggerCollection);
            ConsumeStyleList(state, out styles, out triggers);

            return new UvssRule(selectors, styles, triggers);
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
            AdvanceBeyondWhiteSpaceOrFail(state);

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
                ThrowExpectedToken(state, state.CurrentToken, UvssLexerTokenType.Identifier);

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

            ThrowExpectedValue(state, state.CurrentToken, "none|loop|reverse");
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
            MatchTokenOrFail(state, targetToken, UvssLexerTokenType.Identifier, "target");

            var filter     = ConsumeStoryboardTargetFilter(state);
            var selector   = ConsumeStoryboardTargetSelector(state);
            var animations = ConsumeStoryboardAnimationList(state);

            return new UvssStoryboardTarget(selector, filter, animations);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a storyboard target filter.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssStoryboardTargetFilter"/> object representing the filter that was consumed.</returns>
        private static UvssStoryboardTargetFilter ConsumeStoryboardTargetFilter(UvssParserState state)
        {
            AdvanceBeyondWhiteSpaceOrFail(state);

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
                        ThrowUnexpectedEOF(state);

                    var type = state.CurrentToken.Value;
                    filter.Add(type);

                    state.Consume();
                    state.AdvanceBeyondWhiteSpace();
                }
            }

            return filter;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a storyboard target selector.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A new <see cref="UvssSelector"/> object representing the selector that was consumed.</returns>
        private static UvssSelector ConsumeStoryboardTargetSelector(UvssParserState state)
        {
            var selector = default(UvssSelector);

            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                var tokens      = GetTokensBetweenParentheses(state);
                var tokensState = new UvssParserState(state.Source, tokens);
                selector        = ConsumeSelector(tokensState, true);
            }
            
            state.AdvanceBeyondWhiteSpace();

            return selector;
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
            MatchTokenOrFail(state, animationToken, UvssLexerTokenType.Identifier, "animation");

            var propertyToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, propertyToken, UvssLexerTokenType.Identifier);

            var propertyNameStart  = propertyToken.Value.Start;
            var propertyNameLength = propertyToken.Value.Length;

            while (true)
            {
                if (state.CurrentToken.TokenType != UvssLexerTokenType.Identifier)
                    break;

                propertyNameLength += state.CurrentToken.Length;

                state.Consume();
            }

            AdvanceBeyondWhiteSpaceOrFail(state);

            var keyframes = ConsumeStoryboardKeyframeList(state);
            return new UvssStoryboardAnimation(state.Source.Substring(propertyNameStart, propertyNameLength), keyframes);
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
            MatchTokenOrFail(state, keyframeToken, UvssLexerTokenType.Identifier, "keyframe");

            var timeToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, timeToken, UvssLexerTokenType.Number);

            AdvanceBeyondWhiteSpaceOrFail(state);

            var easing      = ConsumeOptionalEasingFunction(state);
            var valueTokens = GetTokensBetweenCurlyBraces(state);
            var value       = String.Join(String.Empty, valueTokens.Select(x => x.Value));
            var time        = Double.Parse(timeToken.Value.Value);

            return new UvssStoryboardKeyframe(easing, value, time);
        }

        /// <summary>
        /// Consumes an optional token which represents an easing function.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A string which represents one of the standard easing functions.</returns>
        private static String ConsumeOptionalEasingFunction(UvssParserState state)
        {
            if (state.CurrentToken.TokenType == UvssLexerTokenType.Identifier)
            {
                var easing = state.CurrentToken.Value;

                state.Advance();
                state.AdvanceBeyondWhiteSpace();

                return easing;
            }
            return null;
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

                AdvanceBeyondWhiteSpaceOrFail(state);

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
                var part = ConsumeSelectorPart(state, allowEOF, !pseudoClass, parts.Any());
                if (part != null)
                {
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
                    ThrowUnexpectedEOF(state);
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
        /// <param name="allowPseudoClass">A value indicating whether parts with pseduo classes are valid.</param>
        /// <param name="allowChild">A value indicating whether this selector part can be an immediate child.</param>
        /// <returns>A new <see cref="UvssSelectorPart"/> object representing the selector part that was consumed.</returns>
        private static UvssSelectorPart ConsumeSelectorPart(UvssParserState state, Boolean allowEOF, Boolean allowPseudoClass, Boolean allowChild)
        {
            var element     = default(String);
            var id          = default(String);
            var pseudoClass = default(String);
            var classes     = new List<String>();
            var valid       = false;
            var child       = false;
            var universal   = false;

            while (true)
            {
                if (state.IsPastEndOfStream)
                {
                    if (allowEOF && (!child || valid))
                    {
                        break;
                    }
                    ThrowUnexpectedEOF(state);
                }

                var token = state.CurrentToken;

                if (token.TokenType == UvssLexerTokenType.WhiteSpace ||
                    token.TokenType == UvssLexerTokenType.Comma ||
                    token.TokenType == UvssLexerTokenType.OpenCurlyBrace)
                {
                    if (child && !valid)
                    {
                        ThrowUnexpectedToken(state, token);
                    }
                    break;
                }

                if (token.TokenType == UvssLexerTokenType.ChildSelector)
                {
                    if (!allowChild)
                        ThrowUnexpectedToken(state, token);

                    child = true;

                    state.Advance();
                    state.AdvanceBeyondWhiteSpace();

                    continue;
                }

                if (token.TokenType == UvssLexerTokenType.Identifier || token.TokenType == UvssLexerTokenType.UniversalSelector)
                {
                    if (!String.IsNullOrEmpty(pseudoClass))
                        ThrowUnexpectedToken(state, token);

                    state.Advance();

                    if (token.TokenType == UvssLexerTokenType.UniversalSelector)
                    {
                        valid     = true;
                        universal = true;
                        continue;
                    }
                    else
                    {
                        if (IsSelectorForElement(token.Value))
                        {
                            if (element != null || universal)
                                ThrowUnexpectedValue(state, token);

                            valid   = true;
                            element = token.Value;
                            continue;
                        }
                    }

                    if (IsSelectorForID(token.Value))
                    {
                        if (id != null)
                            ThrowUnexpectedValue(state, token);

                        valid = true;
                        id    = token.Value;
                        continue;
                    }

                    valid = true;
                    classes.Add(token.Value);
                    continue;
                }

                if (token.TokenType == UvssLexerTokenType.Colon)
                {
                    if (!valid)
                        ThrowUnexpectedToken(state, token);

                    state.Advance();

                    var identifier = state.TryConsume();
                    if (identifier == null)
                        ThrowUnexpectedEOF(state);

                    if (identifier.Value.TokenType != UvssLexerTokenType.Identifier)
                        ThrowExpectedToken(state, identifier.Value, UvssLexerTokenType.Identifier);

                    pseudoClass = identifier.Value.Value;
                    break;
                }

                if (valid)
                    break;

                ThrowUnexpectedToken(state, token);
            }

            return valid ? new UvssSelectorPart(child, element, id, pseudoClass, classes) : null;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="styles">A <see cref="UvssStyleCollection"/> object representing the style list that was consumed.</param>
        /// <param name="triggers">A <see cref="UvssTriggerCollection"/> object representing the trigger list that was consumed.</param>
        private static void ConsumeStyleList(UvssParserState state, out UvssStyleCollection styles, out UvssTriggerCollection triggers)
        {
            state.AdvanceBeyondWhiteSpace();

            var styleListTokens = GetTokensBetweenCurlyBraces(state);
            var styleListState  = new UvssParserState(state.Source, styleListTokens);
            var tempStyles      = new List<UvssStyle>();
            var tempTriggers    = new List<Trigger>();

            while (ConsumeStyleOrTrigger(styleListState, tempStyles, tempTriggers))
            { }

            styles   = new UvssStyleCollection(tempStyles);
            triggers = new UvssTriggerCollection(tempTriggers);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS style or UVSS trigger.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="styles">The list containing the styles being created.</param>
        /// <param name="triggers">The list containing the triggers being created.</param>
        /// <returns><c>true</c> if a style or trigger was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumeStyleOrTrigger(UvssParserState state, List<UvssStyle> styles, List<Trigger> triggers)
        {
            state.AdvanceBeyondWhiteSpace();

            if (state.IsPastEndOfStream)
                return false;

            var qualifierImportant = false;

            var nameToken = state.TryConsumeNonWhiteSpace();
            if (nameToken.HasValue && nameToken.Value.TokenType == UvssLexerTokenType.TriggerKeyword)
            {
                return ConsumeTrigger(state, triggers);
            }
            MatchTokenOrFail(state, nameToken, UvssLexerTokenType.StyleName);

            AdvanceBeyondWhiteSpaceOrFail(state);

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
            MatchTokenOrFail(state, colonToken, UvssLexerTokenType.Colon);

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
            var value     = String.Join(String.Empty, valueTokens.Select(x => x.Value)).Trim();

            if (name.Contains('.'))
            {
                var nameParts = name.Split('.');
                container     = nameParts[0];
                name          = nameParts[1];
            }

            var style = new UvssStyle(arguments, container, name, value, qualifierImportant);
            styles.Add(style);

            return true;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS trigger.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="triggers">The list containing the triggers being created.</param>
        /// <returns><c>true</c> if a trigger was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumeTrigger(UvssParserState state, List<Trigger> triggers)
        {
            AdvanceBeyondWhiteSpaceOrFail(state);

            var triggerTypeToken = state.Consume();
            MatchTokenOrFail(state, triggerTypeToken, UvssLexerTokenType.Identifier);

            if (String.Equals(triggerTypeToken.Value, "property", StringComparison.InvariantCultureIgnoreCase))
            {
                return ConsumePropertyTrigger(state, triggers);
            }

            if (String.Equals(triggerTypeToken.Value, "event", StringComparison.InvariantCultureIgnoreCase))
            {
                return ConsumeEventTrigger(state, triggers);
            }

            ThrowExpectedValue(state, triggerTypeToken, "property|event");
            return false;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS property trigger.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="triggers">The list containing the triggers being created.</param>
        /// <returns><c>true</c> if a trigger was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumePropertyTrigger(UvssParserState state, List<Trigger> triggers)
        {
            var conditions = ConsumePropertyTriggerConditionList(state);

            var trigger = new PropertyTrigger();
            trigger.Conditions.AddRange(conditions);

            if (!ConsumeTriggerActions(state, trigger))
                return false;

            triggers.Add(trigger);

            return true;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS property trigger condition list.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>A collection containing the trigger condition list that was parsed.</returns>
        private static IEnumerable<PropertyTriggerCondition> ConsumePropertyTriggerConditionList(UvssParserState state)
        {
            var conditions = new List<PropertyTriggerCondition>();

            while (true)
            {
                var condition = ConsumePropertyTriggerCondition(state);
                conditions.Add(condition);

                state.AdvanceBeyondWhiteSpace();

                if (state.IsPastEndOfStream || state.CurrentToken.TokenType != UvssLexerTokenType.Comma)
                    break;

                state.Consume();
            }

            return conditions;
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS property trigger condition.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <returns>The trigger condition that was parsed.</returns>
        private static PropertyTriggerCondition ConsumePropertyTriggerCondition(UvssParserState state)
        {
            state.AdvanceBeyondWhiteSpace();

            var propertyToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, propertyToken, UvssLexerTokenType.StyleName);

            state.AdvanceBeyondWhiteSpace();

            var opToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, opToken, UvssLexerTokenType.ComparisonOperator);

            var opValue = default(TriggerComparisonOp);
            switch (opToken.Value.Value)
            {
                case "=":
                    opValue = TriggerComparisonOp.Equals;
                    break;

                case "<>":
                    opValue = TriggerComparisonOp.NotEquals;
                    break;

                case "<":
                    opValue = TriggerComparisonOp.LessThan;
                    break;

                case "<=":
                    opValue = TriggerComparisonOp.LessThanOrEqualTo;
                    break;

                case ">":
                    opValue = TriggerComparisonOp.GreaterThan;
                    break;

                case ">=":
                    opValue = TriggerComparisonOp.GreaterThanOrEqualTo;
                    break;
            }

            state.AdvanceBeyondWhiteSpace();

            var valueTokens = GetTokensBetweenCurlyBraces(state);
            var value       = String.Join(String.Empty, valueTokens.Select(x => x.Value)).Trim();

            return new PropertyTriggerCondition(opValue, propertyToken.Value.Value, value);
        }

        /// <summary>
        /// Consumes a sequence of tokens representing a UVSS event trigger.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="triggers">The list containing the triggers being created.</param>
        /// <returns><c>true</c> if a trigger was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumeEventTrigger(UvssParserState state, List<Trigger> triggers)
        {
            state.AdvanceBeyondWhiteSpace();

            var eventNameToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, eventNameToken, UvssLexerTokenType.StyleName);

            state.AdvanceBeyondWhiteSpace();

            var handled = false;
            var setHandled = false;

            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                state.Consume();

                while (true)
                {
                    if (state.CurrentToken.TokenType == UvssLexerTokenType.CloseParenthesis)
                    {
                        state.Consume();
                        break;
                    }

                    var argToken = state.TryConsumeNonWhiteSpace();
                    MatchTokenOrFail(state, argToken, UvssLexerTokenType.Identifier);

                    if (String.Equals(argToken.Value.Value, "handled", StringComparison.InvariantCultureIgnoreCase))
                    {
                        handled = true;
                        continue;
                    }

                    if (String.Equals(argToken.Value.Value, "set-handled", StringComparison.InvariantCultureIgnoreCase))
                    {
                        setHandled = true;
                        continue;
                    }

                    return false;
                }
            }

            var trigger = new EventTrigger(eventNameToken.Value.Value, handled, setHandled);

            if (!ConsumeTriggerActions(state, trigger))
                return false;

            triggers.Add(trigger);

            return true;
        }

        /// <summary>
        /// Consumes a list of trigger actions.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="trigger">The trigger to populate with actions.</param>
        /// <returns><c>true</c> if a trigger action list was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumeTriggerActions(UvssParserState state, Trigger trigger)
        {
            var openCurlyToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, openCurlyToken, UvssLexerTokenType.OpenCurlyBrace);

            while (true)
            {
                state.AdvanceBeyondWhiteSpace();

                var nextToken = state.TryConsumeNonWhiteSpace();
                if (nextToken.HasValue && nextToken.Value.TokenType == UvssLexerTokenType.CloseCurlyBrace)
                {
                    return true;
                }

                MatchTokenOrFail(state, nextToken, UvssLexerTokenType.Identifier);

                if (String.Equals(nextToken.Value.Value, "set", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!ConsumeSetTriggerAction(state, trigger))
                        return false;

                    continue;
                }

                if (String.Equals(nextToken.Value.Value, "play-sfx", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!ConsumePlaySfxTriggerAction(state, trigger))
                        return false;

                    continue;
                }

                if (String.Equals(nextToken.Value.Value, "play-storyboard", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!ConsumePlayStoryboardTriggerAction(state, trigger))
                        return false;

                    continue;
                }

                ThrowExpectedValue(state, nextToken.Value, "set|play-sfx|play-storyboard");
            }
        }

        /// <summary>
        /// Consumes a 'set' trigger action.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="trigger">The trigger to populate with actions.</param>
        /// <returns><c>true</c> if a trigger action was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumeSetTriggerAction(UvssParserState state, Trigger trigger)
        {
            var selector = default(UvssSelector);

            state.AdvanceBeyondWhiteSpace();

            var propertyNameToken = state.TryConsumeNonWhiteSpace();
            MatchTokenOrFail(state, propertyNameToken, UvssLexerTokenType.StyleName);

            state.AdvanceBeyondWhiteSpace();

            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                var selectorOpenParensToken = state.TryConsumeNonWhiteSpace();
                MatchTokenOrFail(state, selectorOpenParensToken, UvssLexerTokenType.OpenParenthesis);

                selector = ConsumeSelector(state, false);

                var selectorCloseParensToken = state.TryConsumeNonWhiteSpace();
                MatchTokenOrFail(state, selectorCloseParensToken, UvssLexerTokenType.CloseParenthesis);
            }

            state.AdvanceBeyondWhiteSpace();

            var value  = GetStringBetweenCurlyBraces(state);
            var action = new SetTriggerAction(propertyNameToken.Value.Value, selector, value);

            trigger.Actions.Add(action);

            return true;
        }

        /// <summary>
        /// Consumes a 'play-sfx' trigger action.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="trigger">The trigger to populate with actions.</param>
        /// <returns><c>true</c> if a trigger action was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumePlaySfxTriggerAction(UvssParserState state, Trigger trigger)
        {
            var value  = GetStringBetweenCurlyBraces(state);

            SourcedAssetID sfxAssetID;
            if (!SourcedAssetID.TryParse(value, out sfxAssetID))
                throw new UvssException(PresentationStrings.InvalidAssetIdentifier.Format(value));

            var action = new PlaySoundEffectTriggerAction(sfxAssetID);

            trigger.Actions.Add(action);

            return true;
        }

        /// <summary>
        /// Consumes a 'play-storyboard' trigger action.
        /// </summary>
        /// <param name="state">The parser state.</param>
        /// <param name="trigger">The trigger to populate with actions.</param>
        /// <returns><c>true</c> if a trigger action was successfully consumed; otherwise, <c>false</c>.</returns>
        private static Boolean ConsumePlayStoryboardTriggerAction(UvssParserState state, Trigger trigger)
        {
            var selector = default(UvssSelector);

            state.AdvanceBeyondWhiteSpace();

            if (state.CurrentToken.TokenType == UvssLexerTokenType.OpenParenthesis)
            {
                var selectorOpenParensToken = state.TryConsumeNonWhiteSpace();
                MatchTokenOrFail(state, selectorOpenParensToken, UvssLexerTokenType.OpenParenthesis);

                selector = ConsumeSelector(state, false);

                var selectorCloseParensToken = state.TryConsumeNonWhiteSpace();
                MatchTokenOrFail(state, selectorCloseParensToken, UvssLexerTokenType.CloseParenthesis);
            }

            state.AdvanceBeyondWhiteSpace();

            var value  = GetStringBetweenCurlyBraces(state);
            var action = new PlayStoryboardTriggerAction(value, selector);

            trigger.Actions.Add(action);

            return true;
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
                    ThrowExpectedToken(state, comma.Value, UvssLexerTokenType.Comma);
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
                ThrowInvalidStyleArguments(state, style);

            if (arguments.Count != 2 && arguments.Count != 3)
                ThrowInvalidStyleArguments(state, style);
        }
    }
}
