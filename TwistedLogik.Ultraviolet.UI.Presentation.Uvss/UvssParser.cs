using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Diagnostics;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

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
        public static UvssDocumentSyntax Parse(String source)
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
        /// Updates the position of the specified node to the match the position of its first token.
        /// </summary>
        private static TNode WithPosition<TNode>(TNode node, Int32 defaultpos = 0)
            where TNode : SyntaxNode
        {
            node.Position = node.GetFirstToken()?.Position ?? defaultpos;
            return node;
        }

        /// <summary>
        /// Gets the node position that corresponds to the specified position in the lexer token stream.
        /// </summary>
        private static Int32 GetNodePositionFromLexerPosition(IList<UvssLexerToken> input, Int32 position)
        {
            if (position >= input.Count)
            {
                if (input.Count == 0)
                    return 0;

                var token = input[input.Count - 1];
                return token.SourceOffset + token.SourceLength;
            }
            return input[position].SourceOffset;
        }

        /// <summary>
        /// Counts the number of trivia tokens starting at the specified position in the lexer stream.
        /// </summary>
        private static Int32 CountTrivia(
            IList<UvssLexerToken> input, Int32 position, Boolean acceptEndOfLine = true)
        {
            var count = 0;

            while (position < input.Count)
            {
                var token = input[position];

                if (!acceptEndOfLine && token.Type == UvssLexerTokenType.EndOfLine)
                    break;

                position++;

                if (!IsTrivia(token))
                    break;

                count++;
            }

            return count;
        }

        /// <summary>
        /// Gets a value indicating whether the specified node occurs at the end of its line.
        /// </summary>
        /// <param name="node">The node to evaluate.</param>
        /// <returns>true if the node occurs at the end of its line; otherwise, false.</returns>
        private static Boolean IsEndOfLine(SyntaxNode node)
        {
            if (node == null)
                return false;

            var trivia = node.GetTrailingTrivia();
            if (trivia == null)
                return false;

            if (trivia.IsList)
            {
                for (int i = 0; i < trivia.SlotCount; i++)
                {
                    var child = trivia.GetSlot(i);
                    if (child != null && child.Kind == SyntaxKind.EndOfLineTrivia)
                        return true;
                }
                return false;
            }
            else
            {
                return trivia.Kind == SyntaxKind.EndOfLineTrivia;
            }
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
                token.Type == UvssLexerTokenType.WhiteSpace ||
                token.Type == UvssLexerTokenType.Unknown;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind is trivia.
        /// </summary>
        private static Boolean IsTrivia(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.EndOfLineTrivia ||
                kind == SyntaxKind.MultiLineCommentTrivia ||
                kind == SyntaxKind.SingleLineCommentTrivia ||
                kind == SyntaxKind.WhitespaceTrivia ||
                kind == SyntaxKind.SkippedTokensTrivia;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind is a leading qualifier for a selector.
        /// </summary>
        private static Boolean IsSelectorQualifier(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.HashToken ||
                kind == SyntaxKind.PeriodToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind is a selector combinator.
        /// </summary>
        private static Boolean IsSelectorCombinator(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.GreaterThanToken ||
                kind == SyntaxKind.GreaterThanGreaterThanToken ||
                kind == SyntaxKind.GreaterThanQuestionMarkToken ||
                kind == SyntaxKind.SpaceToken;
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified token kind terminates a selector.
        /// </summary>
        private static Boolean IsSelectorTerminator(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.AtSignToken ||
                kind == SyntaxKind.CommaToken ||
                kind == SyntaxKind.PipeToken ||
                kind == SyntaxKind.OpenParenthesesToken ||
                kind == SyntaxKind.CloseParenthesesToken ||
                kind == SyntaxKind.OpenCurlyBraceToken ||
                kind == SyntaxKind.CloseCurlyBraceToken ||
                kind == SyntaxKind.OpenBracketToken ||
                kind == SyntaxKind.CloseBracketToken ||
                kind == SyntaxKind.EndOfFileToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind is potentially the 
        /// beginning of a new selector part.
        /// </summary>
        private static Boolean IsPotentiallyStartOfSelectorPart(SyntaxKind kind)
        {
            return
                !IsSelectorCombinator(kind) && 
                !IsSelectorTerminator(kind) && 
                !IsTrivia(kind);
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

                case UvssLexerTokenType.Asterisk:
                    return SyntaxKind.AsteriskToken;

                case UvssLexerTokenType.GreaterThanGreaterThan:
                    return SyntaxKind.GreaterThanGreaterThanToken;

                case UvssLexerTokenType.GreaterThanQuestionMark:
                    return SyntaxKind.GreaterThanQuestionMarkToken;

                case UvssLexerTokenType.Equals:
                    return SyntaxKind.EqualsToken;

                case UvssLexerTokenType.NotEquals:
                    return SyntaxKind.NotEqualsToken;

                case UvssLexerTokenType.LessThan:
                    return SyntaxKind.LessThanToken;

                case UvssLexerTokenType.LessThanEquals:
                    return SyntaxKind.LessThanEqualsToken;

                case UvssLexerTokenType.GreaterThan:
                    return SyntaxKind.GreaterThanToken;

                case UvssLexerTokenType.GreaterThanEquals:
                    return SyntaxKind.GreaterThanEqualsToken;

                case UvssLexerTokenType.Pipe:
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
            var leadingTrivia = AccumulateTrivia(input, ref position,
                treatWhiteSpaceAsCombinator: treatWhiteSpaceAsMeaningful, 
                treatCurrentTokenAsTrivia: false,
                isLeading: true);
            var leadingTriviaNode = ConvertTriviaList(leadingTrivia);

            if (position >= input.Count)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, null, leadingTriviaNode, null)
                {
                    Position = GetNodePositionFromLexerPosition(input, position)
                };
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

            return new SyntaxToken(tokenKind, tokenText, leadingTrivia, trailingTrivia)
            {
                Position = token.SourceOffset - (leadingTrivia?.FullWidth ?? 0)
            };
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax trivia.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <param name="isSkippedToken">A value indicating whether this is a skipped token.</param>
        /// <returns>The converted syntax trivia.</returns>
        private static SyntaxNode ConvertTrivia(UvssLexerToken token, Boolean isSkippedToken = false)
        {
            if (token.Type == UvssLexerTokenType.Unknown || isSkippedToken)
            {
                var skippedTokensKind = SyntaxKindFromLexerTokenType(token);
                var skippedTokensTrivia = new SkippedTokensTriviaSyntax(
                    new SyntaxToken(skippedTokensKind, token.Text))
                {
                    Position = token.SourceOffset
                };
                return skippedTokensTrivia;
            }
            else
            {
                var tokenKind = SyntaxKindFromLexerTokenType(token);
                var tokenText = token.Text;

                return new StructurelessSyntaxTrivia(
                    tokenKind, tokenText)
                {
                    Position = token.SourceOffset
                };
            }
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

            var list = builder.ToList();
            if (list.Node != null)
            {
                list.Node.Position = list[0].Position;
            }
            return list.Node;
        }

        /// <summary>
        /// Accepts a content node for a document.
        /// </summary>
        private static SyntaxNode AcceptDocumentContent(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (nextTokenKind == SyntaxKind.EndOfFileToken)
                return null;

            if (IsPotentiallyStartOfSelectorPart(nextTokenKind))
            {
                return ExpectRuleSet(input, ref position);
            }
            else
            {
                switch (nextTokenKind)
                {
                    /* Storyboard */
                    case SyntaxKind.AtSignToken:
                        return ExpectStoryboard(input, ref position);

                    /* ??? */
                    default:
                        return ExpectEmptyStatement(input, ref position);
                }
            }
        }

        /// <summary>
        /// Produces a missing list.
        /// </summary>
        private static SyntaxList<TSyntax> MissingList<TSyntax>(
            IList<UvssLexerToken> input, Int32 position)
            where TSyntax : SyntaxNode
        {
            var node = new SyntaxList.MissingList()
            {
                Position = GetNodePositionFromLexerPosition(input, position)
            };
            return new SyntaxList<TSyntax>(node);
        }

        /// <summary>
        /// Accepts a list of syntax nodes which are parsed using <paramref name="itemParser"/>.
        /// </summary>
        private static SyntaxList<TItem> AcceptList<TItem>(
            IList<UvssLexerToken> input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser) where TItem : SyntaxNode
        {
            var builder = default(SyntaxListBuilder<TItem>);
            var nodepos = GetNodePositionFromLexerPosition(input, position);
            
            while (position < input.Count)
            {
                var item = itemParser(input, ref position, builder.Count);
                if (item == null)
                    break;

                if (builder.IsNull)
                    builder = SyntaxListBuilder<TItem>.Create();

                builder.Add(item);
            }

            var list = builder.IsNull ? default(SyntaxList<TItem>) : builder.ToList();
            if (list.Node != null)
            {
                list.Node.Position = nodepos;
            }
            return list;
        }

        /// <summary>
        /// Produces a missing separated list.
        /// </summary>
        private static SeparatedSyntaxList<TSyntax> MissingSeparatedList<TSyntax>(
            IList<UvssLexerToken> input, Int32 position)
            where TSyntax : SyntaxNode
        {
            var node = new SyntaxList.MissingList()
            {
                Position = GetNodePositionFromLexerPosition(input, position)
            };
            return new SeparatedSyntaxList<TSyntax>(node);
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
            var nodepos = GetNodePositionFromLexerPosition(input, position);

            while (position < input.Count)
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

            var list = builder.IsNull ? default(SeparatedSyntaxList<TItem>) : builder.ToList();
            if (list.Node != null)
            {
                list.Node.Position = nodepos;
            }
            return list;
        }

        /// <summary>
        /// Produces a missing block of nodes.
        /// </summary>
        private static UvssBlockSyntax MissingBlock(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            var block = SyntaxFactory.Block(children);
            block.Position = GetNodePositionFromLexerPosition(input, position);
            block.IsMissing = true;
            return block;
        }

        /// <summary>
        /// Parses a block of nodes.
        /// </summary>
        private static UvssBlockSyntax ParseBlock(
            IList<UvssLexerToken> input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenCurlyBraceToken)
                return null;

            var openCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            var contentList = SyntaxListBuilder<SyntaxNode>.Create();
            if (!openCurlyBraceToken.IsMissing)
            {
                while (SyntaxKindFromNextToken(input, position) != SyntaxKind.CloseCurlyBraceToken)
                {
                    var contentNode = contentParser(input, ref position, contentList.Count);
                    if (contentNode == null)
                        break;

                    contentList.Add(contentNode);
                }
            }

            var closeCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.CloseCurlyBraceToken);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingToken(ref diagnostics, openCurlyBraceToken);

            if (closeCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingToken(ref diagnostics, closeCurlyBraceToken);

            var block = WithPosition(new UvssBlockSyntax(
                openCurlyBraceToken,
                contentList.ToList(),
                closeCurlyBraceToken));

            block.SetDiagnostics(diagnostics);

            return block;
        }

        /// <summary>
        /// Accepts a block of nodes.
        /// </summary>
        private static UvssBlockSyntax AcceptBlock(
            IList<UvssLexerToken> input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser)
        {
            return ParseBlock(input, ref position, contentParser, true);
        }

        /// <summary>
        /// Expects a block of nodes and produces a missing node if one is not found.
        /// </summary>
        private static UvssBlockSyntax ExpectBlock(
            IList<UvssLexerToken> input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser)
        {
            return ParseBlock(input, ref position, contentParser, false);
        }

        /// <summary>
        /// Produces a missing identifier.
        /// </summary>
        private static UvssIdentifierSyntax MissingIdentifier(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssIdentifierSyntax(
                MissingToken(SyntaxKind.IdentifierToken, input, position)));
        }

        /// <summary>
        /// Parses an identifier.
        /// </summary>
        private static UvssIdentifierBaseSyntax ParseIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.OpenBracketToken)
            {
                var openBracketToken =
                    ExpectToken(input, ref position, SyntaxKind.OpenBracketToken);

                var start = position;
                var text = new StringBuilder();

                while (position < input.Count && input[position].Type != UvssLexerTokenType.CloseBracket)
                {
                    text.Append(input[position].Text);
                    position++;
                }

                var identifierToken =
                    new SyntaxToken(SyntaxKind.IdentifierToken, text.ToString())
                    {
                        Position = GetNodePositionFromLexerPosition(input, start)
                    };

                var closeBracketToken =
                    ExpectToken(input, ref position, SyntaxKind.CloseBracketToken);

                return WithPosition(new UvssEscapedIdentifierSyntax(
                    openBracketToken,
                    identifierToken,
                    closeBracketToken));
            }
            else
            {
                if (accept && nextKind != SyntaxKind.IdentifierToken)
                    return null;

                var identifierToken =
                    ExpectToken(input, ref position, SyntaxKind.IdentifierToken);

                return WithPosition(new UvssIdentifierSyntax(
                    identifierToken));
            }
        }

        /// <summary>
        /// Accepts an identifier or qualifier identifier.
        /// </summary>
        private static UvssIdentifierBaseSyntax AcceptIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseIdentifier(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects an identifier or qualified identifier and produces a missing node if one is not found.
        /// </summary>
        private static UvssIdentifierBaseSyntax ExpectIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseIdentifier(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing property name.
        /// </summary>
        private static UvssPropertyNameSyntax MissingPropertyName(
            IList<UvssLexerToken> input, Int32 position)
        {
            return new UvssPropertyNameSyntax(
                null,
                null,
                MissingIdentifier(input, position));
        }

        /// <summary>
        /// Parses a property name.
        /// </summary>
        private static UvssPropertyNameSyntax ParsePropertyName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.IdentifierToken)
                return null;

            var firstPart =
                ExpectIdentifier(input, ref position);

            var periodToken =
                AcceptToken(input, ref position, SyntaxKind.PeriodToken);

            if (periodToken != null)
            {
                var attachedPropertyOwnerNameIdentifier =
                    firstPart;

                var propertyNameIdentifier =
                    ExpectIdentifier(input, ref position);

                return WithPosition(new UvssPropertyNameSyntax(
                    attachedPropertyOwnerNameIdentifier,
                    periodToken,
                    propertyNameIdentifier));
            }
            else
            {
                var propertyNameIdentifier = firstPart;

                return WithPosition(new UvssPropertyNameSyntax(
                    null,
                    null,
                    propertyNameIdentifier));
            }
        }

        /// <summary>
        /// Accepts a property name.
        /// </summary>
        private static UvssPropertyNameSyntax AcceptPropertyName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyName(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a property name and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyNameSyntax ExpectPropertyName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyName(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing event name.
        /// </summary>
        private static UvssEventNameSyntax MissingEventName(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssEventNameSyntax(
                null,
                null,
                MissingIdentifier(input, position)));
        }

        /// <summary>
        /// Parses an event name.
        /// </summary>
        private static UvssEventNameSyntax ParseEventName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.IdentifierToken)
                return null;

            var firstPart =
                ExpectIdentifier(input, ref position);

            var periodToken =
                AcceptToken(input, ref position, SyntaxKind.PeriodToken);

            if (periodToken != null)
            {
                var attachedEventOwnerNameIdentifier =
                    firstPart;

                var EventNameIdentifier =
                    ExpectIdentifier(input, ref position);

                return WithPosition(new UvssEventNameSyntax(
                    attachedEventOwnerNameIdentifier,
                    periodToken,
                    EventNameIdentifier));
            }
            else
            {
                var EventNameIdentifier = firstPart;

                return WithPosition(new UvssEventNameSyntax(
                    null,
                    null,
                    EventNameIdentifier));
            }
        }

        /// <summary>
        /// Accepts an event name.
        /// </summary>
        private static UvssEventNameSyntax AcceptEventName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseEventName(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects an event name and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventNameSyntax ExpectEventName(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseEventName(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax MissingNavigationExpression(
            IList<UvssLexerToken> input, Int32 position)
        {
            return new UvssNavigationExpressionSyntax(
                MissingToken(SyntaxKind.PipeToken, input, position),
                MissingPropertyName(input, position),
                MissingToken(SyntaxKind.AsKeyword, input, position),
                MissingIdentifier(input, position));
        }

        /// <summary>
        /// Parses a navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax ParseNavigationExpression(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.PipeToken)
                return null;

            var pipeToken =
                ExpectToken(input, ref position, SyntaxKind.PipeToken);

            var propertyName =
                ExpectPropertyName(input, ref position);

            var asKeyword =
                ExpectToken(input, ref position, SyntaxKind.AsKeyword);

            var typeNameIdentifier =
                ExpectIdentifier(input, ref position);

            return WithPosition(new UvssNavigationExpressionSyntax(
                pipeToken,
                propertyName,
                asKeyword,
                typeNameIdentifier));
        }

        /// <summary>
        /// Accepts a navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax AcceptNavigationExpression(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseNavigationExpression(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a navigation expression and produces a missing node if one is not found.
        /// </summary>
        private static UvssNavigationExpressionSyntax ExpectNavigationExpression(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseNavigationExpression(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing selector.
        /// </summary>
        private static UvssSelectorSyntax MissingSelector(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssSelectorSyntax(
                MissingList<SyntaxNode>(input, position),
                null));
        }

        /// <summary>
        /// Parses a selector.
        /// </summary>
        private static UvssSelectorSyntax ParseSelector(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var components = AcceptList(
                input, ref position, AcceptSelectorPartOrCombinator);

            if (accept && components.Node == null)
                return null;

            var navigationExpression =
                AcceptNavigationExpression(input, ref position);

            return WithPosition(new UvssSelectorSyntax(
                components,
                navigationExpression));
        }

        /// <summary>
        /// Accepts a selector.
        /// </summary>
        private static UvssSelectorSyntax AcceptSelector(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelector(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorSyntax ExpectSelector(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelector(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Parses a visual descendant combinator.
        /// </summary>
        private static SyntaxToken ParseVisualDescendantCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.SpaceToken)
                return null;

            return ExpectToken(input, ref position, SyntaxKind.SpaceToken);
        }

        /// <summary>
        /// Accepts a visual descendant combinator.
        /// </summary>
        private static SyntaxToken AcceptVisualDescendantCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseVisualDescendantCombinator(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a visual descendant combinator and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectVisualDescendantCombinator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseVisualDescendantCombinator(input, ref position, listIndex, false);
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
            if (IsPotentiallyStartOfSelectorPart(nextTokenKind))
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
        /// Produces a missing selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax MissingSelectorPart(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssSelectorPartSyntax(
                MissingList<UvssSelectorSubPartSyntax>(input, position),
                null));
        }

        /// <summary>
        /// Parses a selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax ParseSelectorPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.AsteriskToken)
            {
                var asteriskToken =
                    ExpectToken(input, ref position, SyntaxKind.AsteriskToken);

                var pseudoClass =
                    AcceptPseudoClass(input, ref position);

                return WithPosition(new UvssUniversalSelectorPartSyntax(
                    asteriskToken,
                    pseudoClass));
            }
            else
            {
                var subParts = AcceptList(input, ref position, AcceptSelectorSubPart);

                if (accept && subParts.Node == null)
                    return null;

                var pseudoClass =
                    AcceptPseudoClass(input, ref position);

                return WithPosition(new UvssSelectorPartSyntax(
                    subParts,
                    pseudoClass));
            }
        }

        /// <summary>
        /// Accepts a selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax AcceptSelectorPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorPart(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a selector part and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorPartBaseSyntax ExpectSelectorPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorPart(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing pseudo-class.
        /// </summary>
        private static UvssPseudoClassSyntax MissingPseudoClass(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssPseudoClassSyntax(
                MissingToken(SyntaxKind.ColonToken, input, position),
                MissingIdentifier(input, position)));
        }

        /// <summary>
        /// Parses a pseudo-class.
        /// </summary>
        private static UvssPseudoClassSyntax ParsePseudoClass(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.ColonToken)
                return null;

            var colonToken =
                ExpectToken(input, ref position, SyntaxKind.ColonToken);
            
            var classNameIdentifier =
                ExpectIdentifier(input, ref position);

            return WithPosition(new UvssPseudoClassSyntax(
                colonToken,
                classNameIdentifier));
        }

        /// <summary>
        /// Accepts a pseudo-class.
        /// </summary>
        private static UvssPseudoClassSyntax AcceptPseudoClass(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePseudoClass(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a pseudo-class and produces a missing node if one is not found.
        /// </summary>
        private static UvssPseudoClassSyntax ExpectPseudoClass(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePseudoClass(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing selector sub-part.
        /// </summary>
        private static UvssSelectorSubPartSyntax MissingSelectorSubPart(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssSelectorSubPartSyntax(
                null,
                MissingIdentifier(input, position),
                null));
        }
        
        /// <summary>
        /// Parses the primary identifier for a selector sub-part.
        /// </summary>
        private static UvssIdentifierBaseSyntax ParseSelectorSubPartIdentifier(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var startpos = GetNodePositionFromLexerPosition(input, position);
            var start = position + CountTrivia(input, position);
            var end = start;
            var length = 0;

            while (end < input.Count)
            {
                var current = input[end];
                var currentKind = SyntaxKindFromLexerTokenType(current);

                if (IsSelectorCombinator(currentKind) ||
                    IsSelectorTerminator(currentKind) ||
                    IsSelectorQualifier(currentKind) ||
                    IsTrivia(currentKind))
                {
                    break;
                }

                length += current.SourceLength;
                end++;
            }

            if (start == end)
                return accept ? null : MissingIdentifier(input, position);

            var builder = new StringBuilder(length);
            for (int i = start; i < end; i++)
                builder.Append(input[i].Text);

            var identifierTokenLeadingTrivia = AccumulateTrivia(input, ref position, isLeading: true);
            position += (end - start);
            var identifierTokenTrailingTrivia = AccumulateTrivia(input, ref position);

            var identifierToken = new SyntaxToken(SyntaxKind.IdentifierToken, builder.ToString());

            var identifier = new UvssIdentifierSyntax(identifierToken)
                .WithLeadingTrivia(identifierTokenLeadingTrivia)
                .WithTrailingTrivia(identifierTokenTrailingTrivia);

            identifierToken.Position = startpos;

            return WithPosition(identifier);
        }

        /// <summary>
        /// Parses a selector sub-part.
        /// </summary>
        private static UvssSelectorSubPartSyntax ParseSelectorSubPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (listIndex > 0 && input[position].Type == UvssLexerTokenType.WhiteSpace)
                return accept ? null : MissingSelectorSubPart(input, position);

            var leadingQualifierToken =
                AcceptToken(input, ref position, SyntaxKind.HashToken, SyntaxKind.PeriodToken);

            var subPartIdentifier =
                ParseSelectorSubPartIdentifier(input, ref position, 0, false);

            if (accept && leadingQualifierToken == null && subPartIdentifier.IsMissing)
                return null;

            var trailingQualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ExclamationMarkToken);

            /* NOTE:
            /* Selectors are the only place in the language where white space potentially has 
            /* meaning, which is REALLY ANNOYING. Basically, after each selector part, we have to check
            /* to see if we have trailing white space that isn't followed by another combinator. If we do,
            /* we need to yank it out of the trivia and change our position so that the next pass through 
            /* the AcceptSelectorPartOrCombinator() method will see that it's sitting on a white space. */
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (!IsSelectorCombinator(nextKind) && !IsSelectorTerminator(nextKind))
            {
                var trailingToken = trailingQualifierToken ?? (SyntaxNode)subPartIdentifier ?? leadingQualifierToken;
                var trailingWhiteSpaceIndex = 0;

                if (HasTrailingWhiteSpace(trailingToken, out trailingWhiteSpaceIndex))
                {
                    var pos = trailingToken.Position;
                    SliceTrailingTrivia(trailingToken, trailingWhiteSpaceIndex, ref position);
                    trailingToken.Position = pos;
                }
            }

            return WithPosition(new UvssSelectorSubPartSyntax(
                leadingQualifierToken,
                subPartIdentifier,
                trailingQualifierToken));
        }

        /// <summary>
        /// Accepts a selector sub-part.
        /// </summary>
        private static UvssSelectorSubPartSyntax AcceptSelectorSubPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorSubPart(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a selector sub-part and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorSubPartSyntax ExpectSelectorSubPart(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorSubPart(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax MissingSelectorWithParentheses(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssSelectorWithParenthesesSyntax(
                MissingToken(SyntaxKind.OpenCurlyBraceToken, input, position),
                MissingSelector(input, position),
                MissingToken(SyntaxKind.CloseParenthesesToken, input, position)));
        }

        /// <summary>
        /// Parses a parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax ParseSelectorWithParentheses(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var selector =
                ExpectSelector(input, ref position);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return WithPosition(new UvssSelectorWithParenthesesSyntax(
                openParenToken,
                selector,
                closeParenToken));
        }

        /// <summary>
        /// Accepts a parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax AcceptSelectorWithParentheses(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorWithParentheses(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a parentheses-enclosed selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax ExpectSelectorWithParentheses(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseSelectorWithParentheses(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Parses a property value token from the lexer stream.
        /// </summary>
        private static SyntaxToken ParsePropertyValueToken(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept, SyntaxKind terminator)
        {
            var startpos = GetNodePositionFromLexerPosition(input, position);
            var start = position + CountTrivia(input, position, acceptEndOfLine: false);
            var end = start;
            var length = 0;
            var lastNonWhitespace = start;

            while (end < input.Count)
            {
                var current = input[end];
                var currentKind = SyntaxKindFromLexerTokenType(current);

                if (currentKind == terminator || 
                    currentKind == SyntaxKind.ImportantKeyword ||
                    currentKind == SyntaxKind.EndOfLineTrivia)
                {
                    break;
                }

                if (currentKind != SyntaxKind.WhitespaceTrivia &&
                    currentKind != SyntaxKind.SpaceToken)
                {
                    lastNonWhitespace = end;
                }

                length += current.SourceLength;
                end++;
            }

            end = Math.Min(end, lastNonWhitespace + 1);

            if (start == end)
                return accept ? null : MissingToken(SyntaxKind.PropertyValueToken, input, position);

            var builder = new StringBuilder(length);
            for (int i = start; i < end; i++)
                builder.Append(input[i].Text);

            var valueTokenLeadingTrivia = AccumulateTrivia(input, ref position);
            position += (end - start);
            var valueTokenTrailingTrivia = AccumulateTrivia(input, ref position);

            var valueToken = new SyntaxToken(SyntaxKind.PropertyValueToken, builder.ToString())
                .WithLeadingTrivia(valueTokenLeadingTrivia)
                .WithTrailingTrivia(valueTokenTrailingTrivia);

            valueToken.Position = startpos;

            return valueToken;
        }

        /// <summary>
        /// Accepts a property value token.
        /// </summary>
        private static SyntaxToken AcceptPropertyValueToken(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueToken(input, ref position, 0, true, SyntaxKind.SemiColonToken);
        }

        /// <summary>
        /// Expects a property value token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueToken(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueToken(input, ref position, 0, false, SyntaxKind.SemiColonToken);
        }

        /// <summary>
        /// Accepts a brace-enclosed property value token.
        /// </summary>
        private static SyntaxToken AcceptPropertyValueTokenWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueToken(input, ref position, 0, true, SyntaxKind.CloseCurlyBraceToken);
        }

        /// <summary>
        /// Expects a brace-enclosed property value token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueTokenWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueToken(input, ref position, 0, false, SyntaxKind.CloseCurlyBraceToken);
        }

        /// <summary>
        /// Produces a missing property value.
        /// </summary>
        private static UvssPropertyValueSyntax MissingPropertyValue(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssPropertyValueSyntax(
                MissingToken(SyntaxKind.PropertyValueToken, input, position)));
        }

        /// <summary>
        /// Parses a property value.
        /// </summary>
        private static UvssPropertyValueSyntax ParsePropertyValue(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.PropertyValueToken)
                return null;

            var contentToken =
                ExpectPropertyValueToken(input, ref position);

            return WithPosition(new UvssPropertyValueSyntax(
                contentToken));
        }

        /// <summary>
        /// Accepts a property value.
        /// </summary>
        private static UvssPropertyValueSyntax AcceptPropertyValue(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValue(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueSyntax ExpectPropertyValue(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValue(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax MissingPropertyValueWithBraces(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssPropertyValueWithBracesSyntax(
                MissingToken(SyntaxKind.OpenCurlyBraceToken, input, position),
                MissingToken(SyntaxKind.PropertyValueToken, input, position),
                MissingToken(SyntaxKind.CloseCurlyBraceToken, input, position)));
        }

        /// <summary>
        /// Parses a brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax ParsePropertyValueWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenCurlyBraceToken)
                return null;

            var openCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            var contentToken =
                ExpectPropertyValueTokenWithBraces(input, ref position);

            var closeCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.CloseCurlyBraceToken);

            return WithPosition(new UvssPropertyValueWithBracesSyntax(
                openCurlyBraceToken,
                contentToken,
                closeCurlyBraceToken));
        }

        /// <summary>
        /// Accepts a brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax AcceptPropertyValueWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueWithBraces(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a brace-enclosed property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax ExpectPropertyValueWithBraces(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyValueWithBraces(input, ref position, listIndex, false);
        }
        
        /// <summary>
        /// Accepts any node which is valid inside of a rule set body.
        /// </summary>
        private static SyntaxNode AcceptRuleSetBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (nextTokenKind == SyntaxKind.CloseCurlyBraceToken || nextTokenKind == SyntaxKind.EndOfFileToken)
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

            return ExpectEmptyStatement(input, ref position, listIndex);
        }

        /// <summary>
        /// Produces a missing rule set.
        /// </summary>
        private static UvssRuleSetSyntax MissingRuleSet(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssRuleSetSyntax(
                MissingSeparatedList<UvssSelectorSyntax>(input, position),
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses a rule set.
        /// </summary>
        private static UvssRuleSetSyntax ParseRuleSet(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var selectors =
                AcceptSeparatedList(input, ref position, AcceptSelector, AcceptComma);

            if (accept && selectors.Node == null)
                return null;

            var body =
                ExpectBlock(input, ref position, AcceptRuleSetBodyNode);

            return WithPosition(new UvssRuleSetSyntax(
                selectors,
                body));
        }

        /// <summary>
        /// Accepts a rule set.
        /// </summary>
        private static UvssRuleSetSyntax AcceptRuleSet(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseRuleSet(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a rule set and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSetSyntax ExpectRuleSet(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseRuleSet(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Parses a rule.
        /// </summary>
        private static UvssRuleSyntax ParseRule(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var propertyName = accept ?
                AcceptPropertyName(input, ref position) :
                ExpectPropertyName(input, ref position);
            
            if (accept && propertyName == null)
                return null;

            var hasLineEnded = IsEndOfLine(propertyName);

            var colonToken = hasLineEnded ? MissingToken(SyntaxKind.ColonToken, input, position) :
                ExpectToken(input, ref position, SyntaxKind.ColonToken);

            hasLineEnded = hasLineEnded | IsEndOfLine(colonToken);

            var value = hasLineEnded ? MissingPropertyValue(input, position) :
                ExpectPropertyValue(input, ref position);

            hasLineEnded = hasLineEnded | IsEndOfLine(value);

            var qualifierToken = hasLineEnded ? null :
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            hasLineEnded = hasLineEnded | IsEndOfLine(qualifierToken);

            var semiColonToken = hasLineEnded ? MissingToken(SyntaxKind.SemiColonToken, input, position) :
                ExpectToken(input, ref position, SyntaxKind.SemiColonToken);

            return WithPosition(new UvssRuleSyntax(
                propertyName,
                colonToken,
                value,
                qualifierToken,
                semiColonToken));
        }

        /// <summary>
        /// Accepts a rule.
        /// </summary>
        private static UvssRuleSyntax AcceptRule(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseRule(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a rule and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSyntax ExpectRule(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseRule(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing transition.
        /// </summary>
        private static UvssTransitionSyntax MissingTransition(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssTransitionSyntax(
                MissingToken(SyntaxKind.TransitionKeyword, input, position),
                MissingTransitionArgumentList(input, position),
                MissingToken(SyntaxKind.ColonToken, input, position),
                MissingPropertyValue(input, position),
                null,
                MissingToken(SyntaxKind.SemiColonToken, input, position)));
        }

        /// <summary>
        /// Parses a transition.
        /// </summary>
        private static UvssTransitionSyntax ParseTransition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TransitionKeyword)
                return null;

            var transitionKeyword =
                ExpectToken(input, ref position, SyntaxKind.TransitionKeyword);
            
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

            return WithPosition(new UvssTransitionSyntax(
                transitionKeyword,
                argumentList,
                colonToken,
                value,
                qualifierToken,
                semiColonToken));
        }

        /// <summary>
        /// Accepts a transition.
        /// </summary>
        private static UvssTransitionSyntax AcceptTransition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTransition(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a transition and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionSyntax ExpectTransition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTransition(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax MissingTransitionArgumentList(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssTransitionArgumentListSyntax(
                MissingToken(SyntaxKind.OpenParenthesesToken, input, position),
                MissingSeparatedList<SyntaxNode>(input, position),
                MissingToken(SyntaxKind.CloseParenthesesToken, input, position)));
        }

        /// <summary>
        /// Parses a transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax ParseTransitionArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var arguments =
                AcceptSeparatedList<SyntaxNode>(input, ref position, AcceptIdentifier, AcceptComma);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return WithPosition(new UvssTransitionArgumentListSyntax(
                openParenToken,
                arguments,
                closeParenToken));
        }

        /// <summary>
        /// Accepts a transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax AcceptTransitionArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTransitionArgumentList(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a transition argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionArgumentListSyntax ExpectTransitionArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTransitionArgumentList(input, ref position, listIndex, false);
        }
        
        /// <summary>
        /// Accepts an event trigger argument.
        /// </summary>
        private static SyntaxNode AcceptEventTriggerArgument(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return AcceptToken(input, ref position,
                SyntaxKind.HandledKeyword,
                SyntaxKind.SetHandledKeyword);
        }

        /// <summary>
        /// Produces a missing event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList MissingEventTriggerArgumentList(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssEventTriggerArgumentList(
                MissingToken(SyntaxKind.OpenParenthesesToken, input, position),
                MissingSeparatedList<SyntaxNode>(input, position),
                MissingToken(SyntaxKind.CloseParenthesesToken, input, position)));
        }

        /// <summary>
        /// Parses an event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList ParseEventTriggerArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var argumentList =
                AcceptSeparatedList(input, ref position, AcceptEventTriggerArgument, AcceptComma);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            return WithPosition(new UvssEventTriggerArgumentList(
                openParenToken,
                argumentList,
                closeParenToken));
        }

        /// <summary>
        /// Accepts an event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList AcceptEventTriggerArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseEventTriggerArgumentList(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects an event trigger argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventTriggerArgumentList ExpectEventTriggerArgumentList(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseEventTriggerArgumentList(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing comparison operator.
        /// </summary>
        private static SyntaxToken MissingComparisonOperator(
            IList<UvssLexerToken> input, Int32 position)
        {
            return MissingToken(SyntaxKind.None, input, position);
        }

        /// <summary>
        /// Parses a comparison operator.
        /// </summary>
        private static SyntaxToken ParseComparisonOperator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);

            switch (nextTokenKind)
            {
                case SyntaxKind.EqualsToken:
                    return ExpectToken(input, ref position, SyntaxKind.EqualsToken);

                case SyntaxKind.NotEqualsToken:
                    return ExpectToken(input, ref position, SyntaxKind.NotEqualsToken);

                case SyntaxKind.GreaterThanToken:
                    return ExpectToken(input, ref position, SyntaxKind.GreaterThanToken);

                case SyntaxKind.GreaterThanEqualsToken:
                    return ExpectToken(input, ref position, SyntaxKind.GreaterThanEqualsToken);

                case SyntaxKind.LessThanToken:
                    return ExpectToken(input, ref position, SyntaxKind.LessThanToken);

                case SyntaxKind.LessThanEqualsToken:
                    return ExpectToken(input, ref position, SyntaxKind.LessThanEqualsToken);
            }

            return accept ? null : MissingComparisonOperator(input, position);
        }

        /// <summary>
        /// Accepts a comparison operator.
        /// </summary>
        private static SyntaxToken AcceptComparisonOperator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseComparisonOperator(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a comparison operator and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectComparisonOperator(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseComparisonOperator(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax MissingPropertyTriggerCondition(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssPropertyTriggerConditionSyntax(
                MissingPropertyName(input, position),
                MissingComparisonOperator(input, position),
                MissingPropertyValueWithBraces(input, position)));
        }

        /// <summary>
        /// Parses a property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax ParsePropertyTriggerCondition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var propertyName = accept ?
                AcceptPropertyName(input, ref position) :
                ExpectPropertyName(input, ref position);

            if (propertyName == null)
                return null;

            var comparisonOperatorToken =
                ExpectComparisonOperator(input, ref position);

            var propertyValue =
                ExpectPropertyValueWithBraces(input, ref position);

            return WithPosition(new UvssPropertyTriggerConditionSyntax(
                propertyName,
                comparisonOperatorToken,
                propertyValue));
        }

        /// <summary>
        /// Accepts a property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax AcceptPropertyTriggerCondition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyTriggerCondition(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a property trigger condition and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax ExpectPropertyTriggerCondition(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParsePropertyTriggerCondition(input, ref position, listIndex, false);
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
        /// Parses a proeprty trigger.
        /// </summary>
        private static UvssPropertyTriggerSyntax ParsePropertyTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TriggerKeyword)
                return null;

            var triggerKeyword =
                ExpectToken(input, ref position, SyntaxKind.TriggerKeyword);

            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.PropertyKeyword)
            {
                RestoreToken(input, ref position, triggerKeyword);
                return null;
            }

            var propertyKeyword =
                ExpectToken(input, ref position, SyntaxKind.PropertyKeyword);

            var conditions =
                AcceptSeparatedList(input, ref position, AcceptPropertyTriggerCondition);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var body =
                ExpectBlock(input, ref position, AcceptTriggerAction);

            return WithPosition(new UvssPropertyTriggerSyntax(
                triggerKeyword,
                propertyKeyword,
                conditions,
                qualifierToken,
                body));
        }

        /// <summary>
        /// Accepts a property trigger.
        /// </summary>
        private static UvssPropertyTriggerSyntax AcceptPropertyTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParsePropertyTrigger(input, ref position, listPosition, true);
        }

        /// <summary>
        /// Expects a property trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssPropertyTriggerSyntax ExpectPropertyTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParsePropertyTrigger(input, ref position, listPosition, false) ??
                new UvssPropertyTriggerSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Parses an event trigger.
        /// </summary>
        private static UvssEventTriggerSyntax ParseEventTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TriggerKeyword)
                return null;

            var triggerKeyword =
                ExpectToken(input, ref position, SyntaxKind.TriggerKeyword);

            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.EventKeyword)
            {
                RestoreToken(input, ref position, triggerKeyword);
                return null;
            }

            var eventKeyword =
                ExpectToken(input, ref position, SyntaxKind.EventKeyword);

            var eventName =
                ExpectEventName(input, ref position);

            var argumentList =
                AcceptEventTriggerArgumentList(input, ref position);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var body =
                ExpectBlock(input, ref position, AcceptTriggerAction);

            return WithPosition(new UvssEventTriggerSyntax(
                triggerKeyword,
                eventKeyword,
                eventName,
                argumentList,
                qualifierToken,
                body));
        }

        /// <summary>
        /// Accepts an event trigger.
        /// </summary>
        private static UvssEventTriggerSyntax AcceptEventTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParseEventTrigger(input, ref position, listPosition, true);
        }

        /// <summary>
        /// Expects an event trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssEventTriggerSyntax ExpectEventTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParseEventTrigger(input, ref position, listPosition, false) ??
                new UvssEventTriggerSyntax() { IsMissing = true };
        }
        
        /// <summary>
        /// Parses an incomplete trigger.
        /// </summary>
        private static UvssIncompleteTriggerSyntax ParseIncompleteTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TriggerKeyword)
                return null;

            var triggerKeyword =
                ExpectToken(input, ref position, SyntaxKind.TriggerKeyword);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var body =
                ExpectBlock(input, ref position, AcceptTriggerAction);

            return WithPosition(new UvssIncompleteTriggerSyntax(
                triggerKeyword,
                qualifierToken,
                body));
        }

        /// <summary>
        /// Accepts an incomplete trigger.
        /// </summary>
        private static UvssIncompleteTriggerSyntax AcceptIncompleteTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParseIncompleteTrigger(input, ref position, listPosition, true);
        }

        /// <summary>
        /// Expects an incomplete trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssIncompleteTriggerSyntax ExpectIncompleteTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParseIncompleteTrigger(input, ref position, listPosition, false);
        }

        /// <summary>
        /// Produces a missing trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax MissingTrigger(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssIncompleteTriggerSyntax(
                MissingToken(SyntaxKind.TriggerKeyword, input, position),
                null,
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses a trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax ParseTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var propertyTrigger = 
                AcceptPropertyTrigger(input, ref position, listIndex);

            if (propertyTrigger != null)
                return propertyTrigger;

            var eventTrigger =
                AcceptEventTrigger(input, ref position, listIndex);

            if (eventTrigger != null)
                return eventTrigger;

            return accept ? null : ExpectIncompleteTrigger(input, ref position);
        }

        /// <summary>
        /// Accepts a trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax AcceptTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTrigger(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a trigger and produces a missing node if one is not found.
        /// </summary>
        private static UvssTriggerBaseSyntax ExpectTrigger(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseTrigger(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax MissingStoryboard(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssStoryboardSyntax(
                MissingToken(SyntaxKind.AtSignToken, input, position),
                MissingIdentifier(input, position),
                MissingIdentifier(input, position),
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Accepts nodes which are valid inside of the body of a storyboard.
        /// </summary>
        private static SyntaxNode AcceptStoryboardBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var storyboardTarget = AcceptStoryboardTarget(input, ref position, listIndex);
            if (storyboardTarget != null)
                return storyboardTarget;

            return ExpectEmptyStatement(input, ref position, listIndex);
        }

        /// <summary>
        /// Parses a storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax ParseStoryboard(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.AtSignToken)
                return null;

            var atSignToken =
                ExpectToken(input, ref position, SyntaxKind.AtSignToken);

            var nameIdentifier =
                ExpectIdentifier(input, ref position);

            var loopIdentifier =
                AcceptIdentifier(input, ref position);

            var body =
                ExpectBlock(input, ref position, AcceptStoryboardBodyNode);

            return WithPosition(new UvssStoryboardSyntax(
                atSignToken,
                nameIdentifier,
                loopIdentifier,
                body));
        }

        /// <summary>
        /// Accepts a storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax AcceptStoryboard(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseStoryboard(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a storyboard declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardSyntax ExpectStoryboard(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseStoryboard(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Accepts any node which is valid in the body of a storyboard target.
        /// </summary>
        private static SyntaxNode AcceptStoryboardTargetBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var animation = AcceptAnimation(input, ref position);
            if (animation != null)
                return animation;
            
            return ExpectEmptyStatement(input, ref position, listIndex);
        }

        /// <summary>
        /// Produces a missing storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax MissingStoryboardTarget(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssStoryboardTargetSyntax(
                MissingToken(SyntaxKind.TargetKeyword, input, position),
                null,
                null,
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses a storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax ParseStoryboardTarget(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TargetKeyword)
                return null;

            var targetKeyword =
                ExpectToken(input, ref position, SyntaxKind.TargetKeyword);

            var typeNameIdentifier =
                AcceptIdentifier(input, ref position);

            var selector =
                AcceptSelectorWithParentheses(input, ref position);

            var body =
                ExpectBlock(input, ref position, AcceptStoryboardTargetBodyNode);

            return WithPosition(new UvssStoryboardTargetSyntax(
                targetKeyword,
                typeNameIdentifier,
                selector,
                body));
        }

        /// <summary>
        /// Accepts a storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax AcceptStoryboardTarget(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseStoryboardTarget(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects a storyboard target declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardTargetSyntax ExpectStoryboardTarget(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseStoryboardTarget(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Accepts any node which is valid in the body of an animation.
        /// </summary>
        private static SyntaxNode AcceptAnimationBodyNode(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var animationKeyframe = AcceptAnimationKeyframe(input, ref position);
            if (animationKeyframe != null)
                return animationKeyframe;

            return ExpectEmptyStatement(input, ref position, listIndex);
        }

        /// <summary>
        /// Produces a missing animation declaration.
        /// </summary>
        private static UvssAnimationSyntax MissingAnimation(
            IList<UvssLexerToken> input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssAnimationSyntax(
                MissingToken(SyntaxKind.Animation, input, position),
                MissingPropertyName(input, position),
                null,
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses an animation declaration.
        /// </summary>
        private static UvssAnimationSyntax ParseAnimation(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.AnimationKeyword)
                return null;

            var animationKeyword =
                ExpectToken(input, ref position, SyntaxKind.AnimationKeyword);
            
            var propertyName =
                ExpectPropertyName(input, ref position);

            var navigationExpression =
                AcceptNavigationExpression(input, ref position);

            var body =
                ExpectBlock(input, ref position, AcceptAnimationBodyNode);

            var animation = WithPosition(new UvssAnimationSyntax(
                animationKeyword,
                propertyName,
                navigationExpression,
                body));

            var diagnostics = default(ICollection<DiagnosticInfo>);
            if (!animationKeyword.IsMissing)
            {
                if (propertyName.IsMissing)
                    DiagnosticInfo.ReportAnimationMissingPropertyName(ref diagnostics, animation);
            }
            animation.SetDiagnostics(diagnostics);

            return animation;
        }

        /// <summary>
        /// Accepts an animation declaration.
        /// </summary>
        private static UvssAnimationSyntax AcceptAnimation(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseAnimation(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects an animation declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationSyntax ExpectAnimation(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseAnimation(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Produces a missing animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax MissingAnimationKeyframe(
            IList<UvssLexerToken> input, Int32 position)
        {
            return WithPosition(new UvssAnimationKeyframeSyntax(
                MissingToken(SyntaxKind.KeyframeKeyword, input, position),
                MissingToken(SyntaxKind.NumberToken, input, position),
                null,
                MissingPropertyValueWithBraces(input, position)));
        }

        /// <summary>
        /// Parses an animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax ParseAnimationKeyframe(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.KeyframeKeyword)
                return null;

            var keyframeKeyword =
                ExpectToken(input, ref position, SyntaxKind.KeyframeKeyword);

            var timeToken =
                ExpectToken(input, ref position, SyntaxKind.NumberToken);

            var easingIdentifier =
                AcceptIdentifier(input, ref position);

            var value =
                ExpectPropertyValueWithBraces(input, ref position);

            return WithPosition(new UvssAnimationKeyframeSyntax(
                keyframeKeyword,
                timeToken,
                easingIdentifier,
                value));
        }

        /// <summary>
        /// Accepts an animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax AcceptAnimationKeyframe(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseAnimationKeyframe(input, ref position, listIndex, true);
        }

        /// <summary>
        /// Expects an animation keyframe and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationKeyframeSyntax ExpectAnimationKeyframe(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return ParseAnimationKeyframe(input, ref position, listIndex, false);
        }

        /// <summary>
        /// Parses an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax ParseEmptyStatement(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex, Boolean accept)
        {
            var trivia = AccumulateTrivia(input, ref position, 
                treatWhiteSpaceAsCombinator: false,
                treatCurrentTokenAsTrivia: true,
                isLeading: true);

            var emptyToken = new SyntaxToken(SyntaxKind.EmptyToken, null)
            { Position = GetNodePositionFromLexerPosition(input, position) };

            var emptyStatement = new UvssEmptyStatementSyntax(emptyToken)
                .WithLeadingTrivia(trivia);

            return WithPosition(emptyStatement);
        }

        /// <summary>
        /// Accepts an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax AcceptEmptyStatement(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listIndex = 0)
        {
            return null;
        }

        /// <summary>
        /// Expects an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax ExpectEmptyStatement(
            IList<UvssLexerToken> input, ref Int32 position, Int32 listPosition = 0)
        {
            return ParseEmptyStatement(input, ref position, listPosition, false);
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
        /// Produces a missing token of the specified kind.
        /// </summary>
        private static SyntaxToken MissingToken(SyntaxKind kind,
            IList<UvssLexerToken> input, Int32 position)
        {
            var token = new SyntaxToken(kind, null)
            {
                IsMissing = true,
                Position = GetNodePositionFromLexerPosition(input, position)
            };
            return token;
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
                var missingToken = new SyntaxToken(expectedKind, null)
                {
                    IsMissing = true,
                    Position = GetNodePositionFromLexerPosition(input, position)
                };
                return missingToken;
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
                leadingTriviaCount = (leadingTrivia.IsList) ? leadingTrivia.SlotCount : 1;

            var trailingTrivia = token.GetTrailingTrivia();
            var trailingTriviaCount = 0;

            if (trailingTrivia != null)
                trailingTriviaCount = (trailingTrivia.IsList) ? trailingTrivia.SlotCount : 1;

            var tokenCount = (token.Kind == SyntaxKind.EndOfFileToken) ? 0 : 1;
            position -= tokenCount + (leadingTriviaCount + trailingTriviaCount);
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

                var builder = SyntaxListBuilder<SyntaxTrivia>.Create();
                builder.AddRange(trivia, 0, index);
                                
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
        /// <param name="treatCurrentTokenAsTrivia">A value indicating whether to treat
        /// the current token as trivia, even if it otherwise wouldn't be considered trivia.</param>
        /// <param name="isLeading">A value indicating whether this is leading trivia.</param>
        /// <returns>The list of accumulated trivia, or null if no trivia was accumulated.</returns>
        private static IList<SyntaxTrivia> AccumulateTrivia(
            IList<UvssLexerToken> input, ref Int32 position, 
            Boolean treatWhiteSpaceAsCombinator = false, 
            Boolean treatCurrentTokenAsTrivia = false,
            Boolean isLeading = false)
        {
            var triviaList = default(List<SyntaxTrivia>);

            var treatNextTokenAsSkipped = false;
            var treatNextNonTriviaTokenAsTrivia = treatCurrentTokenAsTrivia;

            while (position < input.Count)
            {
                if (!IsTrivia(input[position], treatWhiteSpaceAsCombinator))
                {
                    if (!treatNextNonTriviaTokenAsTrivia)
                        break;

                    treatNextNonTriviaTokenAsTrivia = false;
                    treatNextTokenAsSkipped = true;
                }

                if (triviaList == null)
                    triviaList = new List<SyntaxTrivia>();

                var trivia = (SyntaxTrivia)ConvertTrivia(input[position], treatNextTokenAsSkipped);
                treatNextTokenAsSkipped = false;

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
