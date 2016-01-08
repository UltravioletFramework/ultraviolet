using System;
using System.Linq;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;
using TwistedLogik.Nucleus;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a method which produces a parsed node from the specified inputs.
    /// </summary>
    /// <typeparam name="TNode">The type of node which is produced.</typeparam>
    /// <param name="input">The lexer token stream.</param>
    /// <param name="position">The current position in the lexer token stream.</param>
    /// <param name="listIndex">If the node is going to be used as a part of a list, the node's
    /// index within that list; otherwise, 0.</param>
    /// <returns>The node which was produced.</returns>
    internal delegate TNode UvssParserDelegate<TNode>(
        IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex) where TNode : SyntaxNode;

    /// <summary>
    /// Contains methods for parsing UVSS source text into an abstract syntax tree.
    /// </summary>
    public static class UvssParser
    {
        /// <summary>
        /// Parses the specified source text and produces an abstract syntax tree.
        /// </summary>
        /// <param name="source">The source text to parse.</param>
        /// <returns>The <see cref="SyntaxNode"/> at the root of the parsed syntax tree.</returns>
        public static SyntaxNode Parse(String source)
        {
            Contract.Require(source, "source");

            var input = lexer.Tokenize(source);
            var position = 0;

            var contentNodes = new List<SyntaxNode>();

            while (position < input.Count)
            {
                var contentNode = AcceptDocumentContent(input, ref position);
                if (contentNode == null)
                    break;

                contentNodes.Add(contentNode);
            }

            var contentBuilder = SyntaxListBuilder<SyntaxNode>.Create();
            contentBuilder.AddRange(contentNodes);

            var endOfFileToken = GetNextToken(input, ref position);
            if (endOfFileToken.Kind != SyntaxKind.EndOfFileToken)
                throw new InvalidOperationException();

            return new UvssDocumentSyntax(
                contentBuilder.ToList(),
                endOfFileToken);
        }

        /// <summary>
        /// Gets a value indicating whether the specified lexer token contains trivia.
        /// </summary>
        /// <param name="token">The token to evaluate.</param>
        /// <param name="treatWhiteSpaceAsCombinator">A value indicating whether white space
        /// should be treated as a selector combinator, rather than trivia.</param>
        /// <returns>true if the token contains trivia; otherwise, false.</returns>
        private static Boolean IsTrivia(UvssLexerToken token, Boolean treatWhiteSpaceAsCombinator = false)
        {
            if (token.Type == UvssLexerTokenType.WhiteSpace && treatWhiteSpaceAsCombinator)
                return false;

            return
                token.Type == UvssLexerTokenType.EndOfLine ||
                token.Type == UvssLexerTokenType.SingleLineComment ||
                token.Type == UvssLexerTokenType.MultiLineComment ||
                token.Type == UvssLexerTokenType.WhiteSpace;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind is potentially the 
        /// beginning of a new selector part.
        /// </summary>
        /// <param name="kind">The <see cref="SyntaxKind"/> value to evaluate.</param>
        /// <returns>true if the token kind is potentially the beginning of a new selector
        /// part; otherwise, false.</returns>
        private static Boolean IsPotentialSelectorPart(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.IdentifierToken ||
                kind == SyntaxKind.HashToken ||
                kind == SyntaxKind.PeriodToken ||
                kind == SyntaxKind.OpenBracketToken ||
                kind == SyntaxKind.AsteriskToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified syntax node has any trailing white space trivia.
        /// </summary>
        /// <param name="node">The syntax node to evaluate.</param>
        /// <param name="index">The index of the first trailing white space 
        /// trivia within the node's list of trivia.</param>
        /// <returns>true if trailing white space was found; otherwise, false.</returns>
        private static Boolean HasTrailingWhiteSpace(SyntaxNode node, out Int32 index)
        {
            index = 0;

            var trivia = node.GetTrailingTrivia();
            if (trivia == null)
                return false;

            if (trivia.IsList)
            {
                for (int i = 0; i < trivia.SlotCount; i++)
                {
                    var child = trivia.GetSlot(i);
                    if (child != null && child.Kind == SyntaxKind.WhitespaceTrivia)
                    {
                        index = i;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return trivia.Kind == SyntaxKind.WhitespaceTrivia;
            }
        }

        /// <summary>
        /// Determines the <see cref="SyntaxKind"/> that corresponds to the contents
        /// of the specified lexer token.
        /// </summary>
        /// <param name="token">The lexer token to evaluate.</param>
        /// <returns>The <see cref="SyntaxKind"/> value that corresponds to the specified token.</returns>
        private static SyntaxKind SyntaxKindFromLexerTokenType(UvssLexerToken token)
        {
            switch (token.Type)
            {
                case UvssLexerTokenType.Unknown:
                    return SyntaxKind.None;

                case UvssLexerTokenType.EndOfLine:
                    return SyntaxKind.EndOfLineTrivia;

                case UvssLexerTokenType.SingleLineComment:
                    return SyntaxKind.SingleLineCommentTrivia;

                case UvssLexerTokenType.MultiLineComment:
                    return SyntaxKind.MultiLineCommentTrivia;

                case UvssLexerTokenType.WhiteSpace:
                    return SyntaxKind.WhitespaceTrivia;

                case UvssLexerTokenType.Keyword:
                    return UvssKeyword.GetKeywordKindFromText(token.Text);

                case UvssLexerTokenType.Identifier:
                    return SyntaxKind.IdentifierToken;

                case UvssLexerTokenType.Number:
                    return SyntaxKind.NumberToken;

                case UvssLexerTokenType.Value:
                    return SyntaxKind.PropertyValueToken;

                case UvssLexerTokenType.Comma:
                    return SyntaxKind.CommaToken;

                case UvssLexerTokenType.Colon:
                    return SyntaxKind.ColonToken;

                case UvssLexerTokenType.SemiColon:
                    return SyntaxKind.SemiColonToken;

                case UvssLexerTokenType.AtSign:
                    return SyntaxKind.AtSignToken;

                case UvssLexerTokenType.Hash:
                    return SyntaxKind.HashToken;

                case UvssLexerTokenType.Period:
                    return SyntaxKind.PeriodToken;

                case UvssLexerTokenType.ExclamationMark:
                    return SyntaxKind.ExclamationMarkToken;

                case UvssLexerTokenType.OpenParenthesis:
                    return SyntaxKind.OpenParenthesesToken;

                case UvssLexerTokenType.CloseParenthesis:
                    return SyntaxKind.CloseParenthesesToken;

                case UvssLexerTokenType.OpenCurlyBrace:
                    return SyntaxKind.OpenCurlyBraceToken;

                case UvssLexerTokenType.CloseCurlyBrace:
                    return SyntaxKind.CloseCurlyBraceToken;

                case UvssLexerTokenType.OpenBracket:
                    return SyntaxKind.OpenBracketToken;

                case UvssLexerTokenType.CloseBracket:
                    return SyntaxKind.CloseBracketToken;

                case UvssLexerTokenType.UniversalSelector:
                    return SyntaxKind.AsteriskToken;

                case UvssLexerTokenType.TemplatedChildCombinator:
                    return SyntaxKind.GreaterThanGreaterThanToken;

                case UvssLexerTokenType.LogicalChildCombinator:
                    return SyntaxKind.GreaterThanQuestionMarkToken;

                case UvssLexerTokenType.EqualsOperator:
                    return SyntaxKind.EqualsToken;

                case UvssLexerTokenType.NotEqualsOperator:
                    return SyntaxKind.NotEqualsToken;

                case UvssLexerTokenType.LessThanOperator:
                    return SyntaxKind.LessThanToken;

                case UvssLexerTokenType.LessThanEqualsOperator:
                    return SyntaxKind.LessThanEqualsToken;

                case UvssLexerTokenType.GreaterThanOperator:
                    return SyntaxKind.GreaterThanToken;

                case UvssLexerTokenType.GreaterThanEqualsOperator:
                    return SyntaxKind.GreaterThanEqualsToken;

                case UvssLexerTokenType.NavigationExpressionOperator:
                    return SyntaxKind.PipeToken;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Seeks ahead in the lexer stream and returns the <see cref="SyntaxKind"/> that corresponds
        /// to the next non-trivia lexer token.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position in the lexer token stream.</param>
        /// <param name="treatWhiteSpaceAsMeaningful">A value indicating whether white space
        /// should be treated as meaningful token, rather than trivia.</param>
        /// <returns>The <see cref="SyntaxKind"/> that corresponds to the next non-trivia token.</returns>
        private static SyntaxKind SyntaxKindFromNextToken(
            IList<UvssLexerToken> input, Int32 position, Boolean treatWhiteSpaceAsMeaningful = false)
        {
            while (position < input.Count)
            {
                var token = input[position];
                if (!IsTrivia(token, treatWhiteSpaceAsMeaningful))
                {
                    var kind = SyntaxKindFromLexerTokenType(token);
                    if (kind == SyntaxKind.WhitespaceTrivia && treatWhiteSpaceAsMeaningful)
                        return SyntaxKind.SpaceToken;

                    return kind;
                }
                position++;
            }
            return SyntaxKind.EndOfFileToken;
        }

        /// <summary>
        /// Gets the next syntax token which is represented in the lexer token stream.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position within the lexer token stream.</param>
        /// <param name="treatWhiteSpaceAsMeaningful">A value indicating whether white space
        /// should be treated as meaningful token, rather than trivia.</param>
        /// <returns>The next syntax token.</returns>
        private static SyntaxToken GetNextToken(
            IList<UvssLexerToken> input, ref Int32 position, Boolean treatWhiteSpaceAsMeaningful = false)
        {
            var leadingTrivia = AccumulateTrivia(input, ref position, treatWhiteSpaceAsMeaningful, isLeading: true);
            var leadingTriviaNode = ConvertTriviaList(leadingTrivia);

            if (position >= input.Count)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, null, leadingTriviaNode, null);
            }
            else
            {
                var token = input[position];
                position++;

                var trailingTrivia = AccumulateTrivia(input, ref position);
                var trailingTriviaNode = ConvertTriviaList(trailingTrivia);

                return ConvertToken(token, 
                    leadingTriviaNode, trailingTriviaNode, treatWhiteSpaceAsMeaningful);
            }
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax token.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <param name="treatWhiteSpaceAsMeaningful">A value indicating whether white space
        /// should be treated as meaningful token, rather than trivia.</param>
        /// <returns>The converted syntax token.</returns>
        private static SyntaxToken ConvertToken(UvssLexerToken token,
            Boolean treatWhiteSpaceAsMeaningful = false)
        {
            return ConvertToken(token, null, null, treatWhiteSpaceAsMeaningful);
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax token.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <param name="leadingTrivia">The token's leading trivia, if any.</param>
        /// <param name="trailingTrivia">The token's trailing trivia, if any.</param>
        /// <param name="treatWhiteSpaceAsMeaningful">A value indicating whether white space
        /// should be treated as meaningful token, rather than trivia.</param>
        /// <returns>The converted syntax token.</returns>
        private static SyntaxToken ConvertToken(UvssLexerToken token,
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null,
            Boolean treatWhiteSpaceAsMeaningful = false)
        {
            var tokenKind = SyntaxKindFromLexerTokenType(token);
            var tokenText = token.Text;

            if (tokenKind == SyntaxKind.WhitespaceTrivia && treatWhiteSpaceAsMeaningful)
                tokenKind = SyntaxKind.SpaceToken;

            return new SyntaxToken(tokenKind, tokenText, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax trivia.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <returns>The converted syntax trivia.</returns>
        private static SyntaxNode ConvertTrivia(UvssLexerToken token)
        {
            var tokenKind = SyntaxKindFromLexerTokenType(token);
            var tokenText = token.Text;

            return new SyntaxTrivia(tokenKind, tokenText);
        }

        /// <summary>
        /// Converts a collection of trivia to a trivia list node.
        /// </summary>
        /// <param name="trivia">The collection of trivia to convert.</param>
        /// <returns>The trivia list node which was created from the trivia collection.</returns>
        private static SyntaxNode ConvertTriviaList(IEnumerable<SyntaxNode> trivia)
        {
            if (trivia == null)
                return null;

            var count = trivia.Count();
            if (count == 1)
                return trivia.Single();

            var builder = SyntaxListBuilder<SyntaxNode>.Create();
            builder.AddRange(trivia);

            return builder.ToListNode();
        }
        
        /// <summary>
        /// Accepts a content node for a document.
        /// </summary>
        private static SyntaxNode AcceptDocumentContent(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (nextTokenKind == SyntaxKind.None || nextTokenKind == SyntaxKind.EndOfFileToken)
                return null;

            if (IsPotentialSelectorPart(nextTokenKind))
            {
                return ExpectRuleSet(input, ref position);
            }
            else
            {
                // TODO: Handle unhappy path cases
                switch (nextTokenKind)
                {
                    case SyntaxKind.AtSignToken:
                        return ExpectStoryboard(input, ref position);

                    case SyntaxKind.TargetKeyword:
                        return ExpectStoryboardTarget(input, ref position);

                    case SyntaxKind.KeyframeKeyword:
                        return ExpectAnimationKeyframe(input, ref position);
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Accepts a list of syntax nodes which are parsed using <paramref name="itemParser"/>.
        /// </summary>
        private static SyntaxList<TItem> AcceptList<TItem>(
            IList<UvssLexerToken> input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser) where TItem : SyntaxNode
        {
            var builder = default(SyntaxListBuilder<TItem>);

            while (true)
            {
                var item = itemParser(input, ref position, builder.Count);
                if (item == null)
                    break;

                if (builder.IsNull)
                    builder = SyntaxListBuilder<TItem>.Create();

                builder.Add(item);
            }

            return builder.ToList();
        }

        /// <summary>
        /// Accepts a separated list of syntax nodes which are parsed using <paramref name="itemParser"/>
        /// and separated by commas.
        /// </summary>
        private static SeparatedSyntaxList<TItem> AcceptSeparatedList<TItem>(
            IList<UvssLexerToken> input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser) where TItem : SyntaxNode
        {
            return AcceptSeparatedList(input, ref position, itemParser, AcceptComma);
        }

        /// <summary>
        /// Accepts a separated list of syntax nodes which are parsed using <paramref name="itemParser"/>
        /// and separated by tokens which are parsed using <paramref name="separatorParser"/>.
        /// </summary>
        private static SeparatedSyntaxList<TItem> AcceptSeparatedList<TItem>(
            IList<UvssLexerToken> input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser,
            UvssParserDelegate<SyntaxToken> separatorParser) where TItem : SyntaxNode
        {
            var builder = default(SeparatedSyntaxListBuilder<TItem>);

            while (true)
            {
                var item = itemParser(input, ref position, builder.Count);
                if (item == null)
                    break;

                if (builder.IsNull)
                    builder = SeparatedSyntaxListBuilder<TItem>.Create();

                builder.Add(item);

                var separator = separatorParser(input, ref position, builder.Count);
                if (separator == null)
                    break;

                builder.AddSeparator(separator);
            }

            return builder.ToList();
        }

        /// <summary>
        /// Accepts a block of nodes.
        /// </summary>
        private static UvssBlockSyntax AcceptBlock(
            IList<UvssLexerToken> input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser = null)
        {
            contentParser = contentParser ?? AcceptDocumentContent;

            var openCurlyBraceToken =
                AcceptToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            if (openCurlyBraceToken == null)
                return null;

            var contentList = SyntaxListBuilder<SyntaxNode>.Create();
            while (SyntaxKindFromNextToken(input, position) != SyntaxKind.CloseCurlyBraceToken)
            {
                var contentNode = contentParser(input, ref position, contentList.Count);
                if (contentNode == null)
                    break;

                contentList.Add(contentNode);
            }

            var closeCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.CloseCurlyBraceToken);

            return new UvssBlockSyntax(
                openCurlyBraceToken,
                contentList.ToList(),
                closeCurlyBraceToken);
        }

        /// <summary>
        /// Expects a block of nodes and produces a missing node if one is not found.
        /// </summary>
        private static UvssBlockSyntax ExpectBlock(
            IList<UvssLexerToken> input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser = null)
        {
            return AcceptBlock(input, ref position, contentParser) ??
                new UvssBlockSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts an identifier or qualifier identifier.
        /// </summary>
        private static UvssIdentifierBaseSyntax AcceptIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.OpenBracketToken)
            {
                var openBracketToken =
                    ExpectToken(input, ref position, SyntaxKind.OpenBracketToken);

                var identifierToken =
                    ExpectToken(input, ref position, SyntaxKind.IdentifierToken);

                var closeBracketToken =
                    ExpectToken(input, ref position, SyntaxKind.CloseBracketToken);

                return new UvssEscapedIdentifierSyntax(
                    openBracketToken,
                    identifierToken,
                    closeBracketToken);
            }
            else
            {
                if (nextKind == SyntaxKind.IdentifierToken)
                {
                    var identifierToken =
                        ExpectToken(input, ref position, SyntaxKind.IdentifierToken);

                    return new UvssIdentifierSyntax(
                        identifierToken);
                }
            }
            return null;
        }

        /// <summary>
        /// Expects an identifier or qualified identifier and produces a missing node if one is not found.
        /// </summary>
        private static UvssIdentifierBaseSyntax ExpectIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptIdentifier(input, ref position, listIndex) ??
                new UvssIdentifierSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a property name.
        /// </summary>
        private static UvssPropertyNameSyntax AcceptPropertyName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var firstPart =
                AcceptIdentifier(input, ref position);

            if (firstPart == null)
                return null;

            var periodToken =
                AcceptToken(input, ref position, SyntaxKind.PeriodToken);

            if (periodToken != null)
            {
                var attachedPropertyOwnerNameIdentifier =
                    firstPart;

                var propertyNameIdentifier =
                    ExpectIdentifier(input, ref position);

                return new UvssPropertyNameSyntax(
                    attachedPropertyOwnerNameIdentifier,
                    periodToken,
                    propertyNameIdentifier);
            }
            else
            {
                var propertyNameIdentifier = firstPart;

                return new UvssPropertyNameSyntax(
                    null,
                    null,
                    propertyNameIdentifier);
            }
        }

        /// <summary>
        /// Expects a property name and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyNameSyntax ExpectPropertyName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptPropertyName(input, ref position) ??
                new UvssPropertyNameSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts an event name.
        /// </summary>
        private static UvssEventNameSyntax AcceptEventName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var firstPart =
                AcceptIdentifier(input, ref position);

            if (firstPart == null)
                return null;

            var periodToken =
                AcceptToken(input, ref position, SyntaxKind.PeriodToken);

            if (periodToken != null)
            {
                var attachedEventOwnerNameIdentifier =
                    firstPart;

                var EventNameIdentifier =
                    ExpectIdentifier(input, ref position);

                return new UvssEventNameSyntax(
                    attachedEventOwnerNameIdentifier,
                    periodToken,
                    EventNameIdentifier);
            }
            else
            {
                var EventNameIdentifier = firstPart;

                return new UvssEventNameSyntax(
                    null,
                    null,
                    EventNameIdentifier);
            }
        }

        /// <summary>
        /// Expects an event name and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventNameSyntax ExpectEventName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptEventName(input, ref position) ??
                new UvssEventNameSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax AcceptNavigationExpression(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var pipeToken =
                AcceptToken(input, ref position, SyntaxKind.PipeToken);

            if (pipeToken == null)
                return null;

            var propertyName =
                ExpectPropertyName(input, ref position);

            var asKeyword =
                ExpectToken(input, ref position, SyntaxKind.AsKeyword);

            var typeNameIdentifier =
                ExpectIdentifier(input, ref position);

            return new UvssNavigationExpressionSyntax(
                pipeToken,
                propertyName,
                asKeyword,
                typeNameIdentifier);
        }

        /// <summary>
        /// Expects a navigation expression and produces a missing node if one is not found.
        /// </summary>
        private static UvssNavigationExpressionSyntax ExpectNavigationExpression(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptNavigationExpression(input, ref position) ??
                new UvssNavigationExpressionSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a selector.
        /// </summary>
        private static UvssSelectorSyntax AcceptSelector(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var components = AcceptList(
                input, ref position, AcceptSelectorPartOrCombinator);

            if (components.Node == null)
                return null;

            var navigationExpression =
                AcceptNavigationExpression(input, ref position);

            return new UvssSelectorSyntax(
                components,
                navigationExpression);
        }

        /// <summary>
        /// Expects a selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorSyntax ExpectSelector(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptSelector(input, ref position, listIndex) ??
                new UvssSelectorSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a visual descendant combinator.
        /// </summary>
        private static SyntaxToken AcceptVisualDescendantCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptToken(input, ref position, SyntaxKind.SpaceToken);
        }

        /// <summary>
        /// Expects a visual descendant combinator and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectVisualDescendantCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptVisualDescendantCombinator(input, ref position) ??
                new SyntaxToken(SyntaxKind.SpaceToken, null) { IsMissing = true };
        }
        
        /// <summary>
        /// Accepts a selector part or combinator.
        /// </summary>
        private static SyntaxNode AcceptSelectorPartOrCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            if (listIndex > 0 && input[position].Type == UvssLexerTokenType.WhiteSpace)
                return ExpectVisualDescendantCombinator(input, ref position);

            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (IsPotentialSelectorPart(nextTokenKind))
            {
                return ExpectSelectorPart(input, ref position);
            }
            else
            {
                switch (SyntaxKindFromNextToken(input, position))
                {
                    case SyntaxKind.GreaterThanToken:
                        return ExpectToken(input, ref position, SyntaxKind.GreaterThanToken);

                    case SyntaxKind.GreaterThanQuestionMarkToken:
                        return ExpectToken(input, ref position, SyntaxKind.GreaterThanQuestionMarkToken);

                    case SyntaxKind.GreaterThanGreaterThanToken:
                        return ExpectToken(input, ref position, SyntaxKind.GreaterThanGreaterThanToken);

                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Accepts a selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax AcceptSelectorPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.AsteriskToken)
            {
                var asteriskToken =
                    ExpectToken(input, ref position, SyntaxKind.AsteriskToken);

                var pseudoClass =
                    AcceptPseudoClass(input, ref position);

                return new UvssUniversalSelectorPartSyntax(
                    asteriskToken,
                    pseudoClass);
            }
            else
            {
                var subParts = AcceptList(input, ref position, AcceptSelectorSubPart);
                if (subParts == null)
                    return null;

                var pseudoClass =
                    AcceptPseudoClass(input, ref position);

                return new UvssSelectorPartSyntax(
                    subParts,
                    pseudoClass);
            }
        }

        /// <summary>
        /// Expects a selector part and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorPartBaseSyntax ExpectSelectorPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptSelectorPart(input, ref position) ??
                new UvssSelectorPartSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a pseudo-class.
        /// </summary>
        private static UvssPseudoClassSyntax AcceptPseudoClass(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var colonToken =
                AcceptToken(input, ref position, SyntaxKind.ColonToken);

            if (colonToken == null)
                return null;

            var classNameIdentifier =
                ExpectIdentifier(input, ref position);

            return new UvssPseudoClassSyntax(
                colonToken,
                classNameIdentifier);
        }

        /// <summary>
        /// Expects a pseudo-class and produces a missing node if one is not found.
        /// </summary>
        private static UvssPseudoClassSyntax ExpectPseudoClass(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptPseudoClass(input, ref position) ??
                new UvssPseudoClassSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a selector sub-part.
        /// </summary>
        private static UvssSelectorSubPartSyntax AcceptSelectorSubPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            if (listIndex > 0 && input[position].Type == UvssLexerTokenType.WhiteSpace)
                return null;

            var leadingQualifierToken =
                AcceptToken(input, ref position, SyntaxKind.HashToken, SyntaxKind.PeriodToken);

            var subPartIdentifier =
                ExpectIdentifier(input, ref position);

            if (leadingQualifierToken == null && subPartIdentifier.IsMissing)
                return null;

            var trailingQualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ExclamationMarkToken);

            /* NOTE:
            /* Selectors are the only place in the language where white space potentially has 
            /* meaning, which is REALLY ANNOYING. Basically, after each selector part, we have to check
            /* to see if we have trailing white space that isn't followed by another combinator. If we do,
            /* we need to yank it out of the trivia and change our position so that the next pass through 
            /* the AcceptSelectorPartOrCombinator() method will see that it's sitting on a white space. */            
            if (IsPotentialSelectorPart(SyntaxKindFromNextToken(input, position)))
            {
                var trailingToken = trailingQualifierToken ?? (SyntaxNode)subPartIdentifier ?? leadingQualifierToken;
                var trailingWhiteSpaceIndex = 0;

                if (HasTrailingWhiteSpace(trailingToken, out trailingWhiteSpaceIndex))
                {
                    SliceTrailingTrivia(trailingToken, trailingWhiteSpaceIndex, ref position);
                }
            }

            return new UvssSelectorSubPartSyntax(
                leadingQualifierToken,
                subPartIdentifier,
                trailingQualifierToken);
        }

        /// <summary>
        /// Expects a selector sub-part and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorSubPartSyntax ExpectSelectorSubPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptSelectorSubPart(input, ref position, listIndex) ??
                new UvssSelectorSubPartSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax AcceptSelectorWithParentheses(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var openParenToken =
                AcceptToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            if (openParenToken == null)
                return null;

            var selector =
                ExpectSelector(input, ref position);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return new UvssSelectorWithParenthesesSyntax(
                openParenToken,
                selector,
                closeParenToken);
        }

        /// <summary>
        /// Expects a parentheses-enclosed selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax ExpectSelectorWithParentheses(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptSelectorWithParentheses(input, ref position) ??
                new UvssSelectorWithParenthesesSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a property value.
        /// </summary>
        private static UvssPropertyValueSyntax AcceptPropertyValue(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var contentToken =
                AcceptToken(input, ref position, SyntaxKind.PropertyValueToken);

            if (contentToken == null)
                return null;

            return new UvssPropertyValueSyntax(
                contentToken);
        }

        /// <summary>
        /// Expects a property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueSyntax ExpectPropertyValue(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptPropertyValue(input, ref position) ??
                new UvssPropertyValueSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax AcceptPropertyValueWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var openCurlyBraceToken =
                AcceptToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            if (openCurlyBraceToken == null)
                return null;

            var contentToken =
                ExpectToken(input, ref position, SyntaxKind.PropertyValueToken);

            var closeCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.CloseCurlyBraceToken);

            return new UvssPropertyValueWithBracesSyntax(
                openCurlyBraceToken,
                contentToken,
                closeCurlyBraceToken);
        }

        /// <summary>
        /// Expects a brace-enclosed property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax ExpectPropertyValueWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptPropertyValueWithBraces(input, ref position) ??
                new UvssPropertyValueWithBracesSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts any node which is valid inside of a rule set body.
        /// </summary>
        private static SyntaxNode AcceptRuleSetBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (nextTokenKind == SyntaxKind.None)
                return null;

            switch (nextTokenKind)
            {
                case SyntaxKind.IdentifierToken:
                case SyntaxKind.OpenBracketToken:
                    return ExpectRule(input, ref position);

                case SyntaxKind.TransitionKeyword:
                    return ExpectTransition(input, ref position);

                case SyntaxKind.TriggerKeyword:
                    return ExpectTrigger(input, ref position);
            }

            return null;
        }

        /// <summary>
        /// Accepts a rule set.
        /// </summary>
        private static UvssRuleSetSyntax AcceptRuleSet(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var selectors =
                AcceptSeparatedList(input, ref position, AcceptSelector, AcceptComma);

            if (selectors.Node == null)
                return null;

            var body =
                ExpectBlock(input, ref position, AcceptRuleSetBodyNode);

            return new UvssRuleSetSyntax(
                selectors,
                body);
        }

        /// <summary>
        /// Expects a rule set and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSetSyntax ExpectRuleSet(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptRuleSet(input, ref position) ??
                new UvssRuleSetSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a rule.
        /// </summary>
        private static UvssRuleSyntax AcceptRule(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var propertyName =
                AcceptPropertyName(input, ref position);

            if (propertyName == null)
                return null;

            var colonToken =
                ExpectToken(input, ref position, SyntaxKind.ColonToken);

            var value =
                ExpectPropertyValue(input, ref position);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var semiColonToken =
                ExpectToken(input, ref position, SyntaxKind.SemiColonToken);

            return new UvssRuleSyntax(
                propertyName,
                colonToken,
                value,
                qualifierToken,
                semiColonToken);
        }

        /// <summary>
        /// Expects a rule and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSyntax ExpectRule(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptRule(input, ref position) ??
                new UvssRuleSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a transition.
        /// </summary>
        private static UvssTransitionSyntax AcceptTransition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var transitionKeyword =
                AcceptToken(input, ref position, SyntaxKind.TransitionKeyword);

            if (transitionKeyword == null)
                return null;

            var argumentList =
                ExpectTransitionArgumentList(input, ref position);

            var colonToken =
                ExpectToken(input, ref position, SyntaxKind.ColonToken);

            var value =
                ExpectPropertyValue(input, ref position);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var semiColonToken =
                ExpectToken(input, ref position, SyntaxKind.SemiColonToken);

            return new UvssTransitionSyntax(
                transitionKeyword,
                argumentList,
                colonToken,
                value,
                qualifierToken,
                semiColonToken);
        }

        /// <summary>
        /// Expects a transition and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionSyntax ExpectTransition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptTransition(input, ref position) ??
                new UvssTransitionSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax AcceptTransitionArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var openParenToken =
                AcceptToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var arguments =
                AcceptSeparatedList<SyntaxNode>(input, ref position, AcceptIdentifier, AcceptComma);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return new UvssTransitionArgumentListSyntax(
                openParenToken,
                arguments,
                closeParenToken);
        }

        /// <summary>
        /// Expects a transition argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionArgumentListSyntax ExpectTransitionArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptTransitionArgumentList(input, ref position) ??
                new UvssTransitionArgumentListSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts an event trigger argument.
        /// </summary>
        private static SyntaxNode AcceptEventTriggerArgument(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            // TODO: Unknown
            return AcceptToken(input, ref position,
                SyntaxKind.HandledKeyword,
                SyntaxKind.SetHandledKeyword);
        }

        /// <summary>
        /// Accepts an event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList AcceptEventTriggerArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var openParenToken =
                AcceptToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            if (openParenToken == null)
                return null;

            var argumentList =
                AcceptSeparatedList(input, ref position, AcceptEventTriggerArgument, AcceptComma);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return new UvssEventTriggerArgumentList(
                openParenToken,
                argumentList,
                closeParenToken);
        }

        /// <summary>
        /// Expects an event trigger argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventTriggerArgumentList ExpectEventTriggerArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptEventTriggerArgumentList(input, ref position, listIndex) ??
                new UvssEventTriggerArgumentList() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a comparison operator.
        /// </summary>
        private static SyntaxToken AcceptComparisonOperator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptToken(input, ref position,
                SyntaxKind.EqualsToken,
                SyntaxKind.NotEqualsToken,
                SyntaxKind.GreaterThanToken,
                SyntaxKind.LessThanToken,
                SyntaxKind.GreaterThanEqualsToken,
                SyntaxKind.LessThanEqualsToken);
        }

        /// <summary>
        /// Expects a comparison operator and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectComparisonOperator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptComparisonOperator(input, ref position) ??
                new SyntaxToken(SyntaxKind.None, null) { IsMissing = true };
        }

        /// <summary>
        /// Accepts a property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax AcceptPropertyTriggerCondition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var propertyName =
                AcceptPropertyName(input, ref position);

            if (propertyName == null)
                return null;

            var comparisonOperatorToken =
                ExpectComparisonOperator(input, ref position);

            var propertyValue =
                ExpectPropertyValueWithBraces(input, ref position);

            return new UvssPropertyTriggerConditionSyntax(
                propertyName,
                comparisonOperatorToken,
                propertyValue);
        }

        /// <summary>
        /// Expects a property trigger condition and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax ExpectPropertyTriggerCondition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptPropertyTriggerCondition(input, ref position, listIndex) ??
                new UvssPropertyTriggerConditionSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a trigger action.
        /// </summary>
        private static UvssTriggerActionBaseSyntax AcceptTriggerAction(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var actionKeyword =
                AcceptToken(input, ref position, SyntaxKind.PlayStoryboardKeyword, SyntaxKind.PlaySfxKeyword, SyntaxKind.SetKeyword);

            if (actionKeyword == null)
                return null;

            switch (actionKeyword.Kind)
            {
                case SyntaxKind.PlayStoryboardKeyword:
                    {
                        var playStoryboardKeyword = 
                            actionKeyword;

                        var selector =
                            AcceptSelectorWithParentheses(input, ref position);

                        var value =
                            ExpectPropertyValueWithBraces(input, ref position);

                        return new UvssPlayStoryboardTriggerActionSyntax(
                            playStoryboardKeyword,
                            selector,
                            value);
                    }

                case SyntaxKind.PlaySfxKeyword:
                    {
                        var playSfxKeyword = 
                            actionKeyword;

                        var value =
                            ExpectPropertyValueWithBraces(input, ref position);

                        return new UvssPlaySfxTriggerActionSyntax(
                            playSfxKeyword,
                            value);
                    }

                case SyntaxKind.SetKeyword:
                    {
                        var setKeyword = 
                            actionKeyword;

                        var propertyName =
                            ExpectPropertyName(input, ref position);

                        var selector =
                            AcceptSelectorWithParentheses(input, ref position);

                        var value =
                            ExpectPropertyValueWithBraces(input, ref position);

                        return new UvssSetTriggerActionSyntax(
                            setKeyword,
                            propertyName,
                            selector,
                            value);
                    }

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Accepts a trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax AcceptTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var triggerKeyword =
                AcceptToken(input, ref position, SyntaxKind.TriggerKeyword);

            if (triggerKeyword == null)
                return null;

            var eventKeyword =
                AcceptToken(input, ref position, SyntaxKind.EventKeyword);

            if (eventKeyword != null)
            {
                // Event trigger

                var eventName =
                    ExpectEventName(input, ref position);

                var argumentList =
                    ExpectEventTriggerArgumentList(input, ref position);

                var qualifierToken =
                    AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

                var body =
                    ExpectBlock(input, ref position, AcceptTriggerAction);

                return new UvssEventTriggerSyntax(
                    triggerKeyword,
                    eventKeyword,
                    eventName,
                    argumentList,
                    qualifierToken,
                    body);
            }
            else
            {
                var propertyKeyword =
                    AcceptToken(input, ref position, SyntaxKind.PropertyKeyword);

                if (propertyKeyword != null)
                {
                    // Property trigger

                    var conditions =
                        AcceptSeparatedList(input, ref position, AcceptPropertyTriggerCondition, AcceptComma);

                    var qualifierToken =
                        AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

                    var body =
                        ExpectBlock(input, ref position, AcceptTriggerAction);

                    return new UvssPropertyTriggerSyntax(
                        triggerKeyword,
                        propertyKeyword,
                        conditions,
                        qualifierToken,
                        body);
                }
                else
                {
                    // Incomplete trigger

                    return new UvssIncompleteTriggerSyntax(
                        triggerKeyword);
                }
            }
        }

        /// <summary>
        /// Expects a trigger and produces a missing node if one is not found.
        /// </summary>
        private static UvssTriggerBaseSyntax ExpectTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var trigger = AcceptTrigger(input, ref position);
            if (trigger == null)
            {
                // TODO
                throw new InvalidOperationException();
            }
            return trigger;
        }

        /// <summary>
        /// Accepts a storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax AcceptStoryboard(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var atSignToken =
                AcceptToken(input, ref position, SyntaxKind.AtSignToken);

            if (atSignToken == null)
                return null;

            var nameIdentifier =
                ExpectIdentifier(input, ref position);

            var loopIdentifier =
                AcceptIdentifier(input, ref position);

            var body =
                ExpectBlock(input, ref position);

            return new UvssStoryboardSyntax(
                atSignToken,
                nameIdentifier,
                loopIdentifier,
                body);
        }

        /// <summary>
        /// Expects a storyboard declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardSyntax ExpectStoryboard(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptStoryboard(input, ref position) ??
                new UvssStoryboardSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts any node which is valid in the body of a storyboard target.
        /// </summary>
        private static SyntaxNode AcceptStoryboardTargetBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptAnimation(input, ref position);
        }
        
        /// <summary>
        /// Accepts a storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax AcceptStoryboardTarget(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var targetKeyword =
                AcceptToken(input, ref position, SyntaxKind.TargetKeyword);

            if (targetKeyword == null)
                return null;

            var typeNameIdentifier =
                AcceptIdentifier(input, ref position);

            var selector =
                AcceptSelectorWithParentheses(input, ref position);

            var body =
                ExpectBlock(input, ref position, AcceptStoryboardTargetBodyNode);

            return new UvssStoryboardTargetSyntax(
                targetKeyword,
                typeNameIdentifier,
                selector,
                body);
        }

        /// <summary>
        /// Expects a storyboard target declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardTargetSyntax ExpectStoryboardTarget(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptStoryboardTarget(input, ref position) ??
                new UvssStoryboardTargetSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts any node which is valid in the body of an animation.
        /// </summary>
        private static SyntaxNode AcceptAnimationBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptAnimationKeyframe(input, ref position);
        }

        /// <summary>
        /// Accepts an animation declaration.
        /// </summary>
        private static UvssAnimationSyntax AcceptAnimation(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var animationKeyword =
                AcceptToken(input, ref position, SyntaxKind.AnimationKeyword);

            if (animationKeyword == null)
                return null;

            var propertyName =
                ExpectPropertyName(input, ref position);

            var navigationExpression =
                AcceptNavigationExpression(input, ref position);

            var body =
                ExpectBlock(input, ref position, AcceptAnimationBodyNode);

            return new UvssAnimationSyntax(
                animationKeyword,
                propertyName,
                navigationExpression,
                body);
        }

        /// <summary>
        /// Expects an animation declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationSyntax ExpectAnimation(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptAnimation(input, ref position) ??
                new UvssAnimationSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts an animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax AcceptAnimationKeyframe(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var keyframeKeyword =
                ExpectToken(input, ref position, SyntaxKind.KeyframeKeyword);

            var timeToken =
                ExpectToken(input, ref position, SyntaxKind.NumberToken);

            var easingIdentifier =
                AcceptIdentifier(input, ref position);

            var value =
                ExpectPropertyValueWithBraces(input, ref position);

            return new UvssAnimationKeyframeSyntax(
                keyframeKeyword,
                timeToken,
                easingIdentifier,
                value);
        }

        /// <summary>
        /// Expects an animation keyframe and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationKeyframeSyntax ExpectAnimationKeyframe(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptAnimationKeyframe(input, ref position) ??
                   new UvssAnimationKeyframeSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Accepts a comma token.
        /// </summary>
        private static SyntaxToken AcceptComma(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptToken(input, ref position, SyntaxKind.CommaToken);
        }

        /// <summary>
        /// Expects a comma token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectComma(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ExpectToken(input, ref position, SyntaxKind.CommaToken);
        }

        /// <summary>
        /// Accepts a syntax token of the specified kind.
        /// </summary>
        private static SyntaxToken AcceptToken(
            IList<UvssLexerToken> input, ref Int32 position, SyntaxKind acceptedKind)
        {
            var treatWhiteSpaceAsCombinator = (acceptedKind == SyntaxKind.SpaceToken);

            var foundKind = SyntaxKindFromNextToken(input, position, treatWhiteSpaceAsCombinator);
            if (foundKind == acceptedKind)
            {
                return GetNextToken(input, ref position, treatWhiteSpaceAsCombinator);
            }
            return null;
        }

        /// <summary>
        /// Accepts a syntax token of any of the specified kinds.
        /// </summary>
        private static SyntaxToken AcceptToken(
            IList<UvssLexerToken> input, ref Int32 position, params SyntaxKind[] acceptedKinds)
        {
            var foundKind = SyntaxKindFromNextToken(input, position);
            if (acceptedKinds != null && acceptedKinds.Contains(foundKind))
            {
                return GetNextToken(input, ref position);
            }
            return null;
        }

        /// <summary>
        /// Expects a syntax token of the specified kind and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectToken(
            IList<UvssLexerToken> input, ref Int32 position, SyntaxKind expectedKind)
        {
            var treatWhiteSpaceAsCombinator = (expectedKind == SyntaxKind.SpaceToken);

            var token = GetNextToken(input, ref position, treatWhiteSpaceAsCombinator);
            if (token.Kind != expectedKind)
            {
                RestoreToken(input, ref position, token);
                return new SyntaxToken(expectedKind, null) { IsMissing = true };
            }
            return token;
        }
        
        /// <summary>
        /// Restores the specified token to the input stream and modifies the current stream
        /// position accordingly.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position in the lexer token stream.</param>
        /// <param name="token">The token to restore to the stream.</param>
        private static void RestoreToken(
            IList<UvssLexerToken> input, ref Int32 position, SyntaxToken token)
        {
            var leadingTrivia = token.GetLeadingTrivia();
            var leadingTriviaCount = 0;

            if (leadingTrivia != null)
                leadingTriviaCount -= (leadingTrivia.IsList) ? leadingTrivia.SlotCount : 1;

            var trailingTrivia = token.GetTrailingTrivia();
            var trailingTriviaCount = 0;

            if (trailingTrivia != null)
                trailingTriviaCount -= (trailingTrivia.IsList) ? trailingTrivia.SlotCount : 1;

            position -= 1 + (leadingTriviaCount + trailingTriviaCount);
        }

        /// <summary>
        /// Slices the specified node's trailing trivia list at the specified index.
        /// </summary>
        /// <param name="node">The node to modify.</param>
        /// <param name="index">The index at which to slice the trivia list.</param>
        /// <param name="position">The current position within the lexer stream.</param>
        private static void SliceTrailingTrivia(SyntaxNode node, Int32 index, ref Int32 position)
        {
            var trivia = node.GetTrailingTrivia();
            if (trivia.IsList)
            {
                if (index >= trivia.SlotCount)
                    throw new ArgumentOutOfRangeException(nameof(index));

                if (trivia.SlotCount == 0)
                {
                    node.ChangeTrailingTrivia(null);
                    return;
                }

                if (position == 0)
                {
                    position -= trivia.SlotCount;
                    node.ChangeTrailingTrivia(null);
                    return;
                }

                var builder = new SyntaxListBuilder<SyntaxTrivia>();
                builder.AddRange((SyntaxList<SyntaxTrivia>)trivia, 0, index);

                position -= (trivia.SlotCount - index);
                node.ChangeTrailingTrivia(builder.ToListNode());
            }
            else
            {
                if (index > 0)
                    throw new ArgumentOutOfRangeException(nameof(index));

                position--;
                node.ChangeTrailingTrivia(null);
            }
        }

        /// <summary>
        /// Accumulates trivia into a collection starting at the specified position in the input stream.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position in the lexer token stream.</param>
        /// <param name="treatWhiteSpaceAsCombinator">A value indicating whether white space
        /// should be treated as a selector combinator, rather than trivia.</param>
        /// <param name="isLeading">A value indicating whether this is leading trivia.</param>
        /// <returns>The list of accumulated trivia, or null if no trivia was accumulated.</returns>
        private static IList<SyntaxTrivia> AccumulateTrivia(
            IList<UvssLexerToken> input, ref Int32 position, Boolean treatWhiteSpaceAsCombinator = false, Boolean isLeading = false)
        {
            var triviaList = default(List<SyntaxTrivia>);

            while (position < input.Count)
            {
                if (!IsTrivia(input[position], treatWhiteSpaceAsCombinator))
                    break;

                if (triviaList == null)
                    triviaList = new List<SyntaxTrivia>();

                var trivia = (SyntaxTrivia)ConvertTrivia(input[position]);

                triviaList.Add(trivia);
                position++;

                if (trivia.Kind == SyntaxKind.EndOfLineTrivia && !isLeading)
                    break;
            }

            return triviaList;
        }

        // The lexer used to create token streams from source text.
        private static UvssLexer lexer = new UvssLexer();
    }
}
