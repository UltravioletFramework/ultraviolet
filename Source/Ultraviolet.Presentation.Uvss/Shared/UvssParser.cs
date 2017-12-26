using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Uvss.Diagnostics;
using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a method which produces a parsed node from the specified inputs.
    /// </summary>
    /// <typeparam name="TNode">The type of node which is produced.</typeparam>
    /// <param name="input">The lexer token stream.</param>
    /// <param name="position">The current position in the lexer token stream.</param>
    /// <returns>The node which was produced.</returns>
    internal delegate TNode UvssParserDelegate<TNode>(
        UvssLexerStream input, ref Int32 position) where TNode : SyntaxNode;

    /// <summary>
    /// Contains methods for parsing UVSS source text into an abstract syntax tree.
    /// </summary>
    public static class UvssParser
    {
        /// <summary>
        /// Represents a method which assigns diagnostics to skipped tokens.
        /// </summary>
        private delegate void SkippedTokensDiagnosticsReporter(
            ref ICollection<DiagnosticInfo> diagnostics, SkippedTokensTriviaSyntax trivia);

        /// <summary>
        /// Initializes the <see cref="UvssParser"/> type.
        /// </summary>
        static UvssParser()
        {
            recognizedCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Parses the specified source text and produces an abstract syntax tree.
        /// </summary>
        /// <param name="source">The source text to parse.</param>
        /// <param name="options">A set of <see cref="UvssParserOptions"/> values specifying the parser options.</param>
        /// <returns>The <see cref="SyntaxNode"/> at the root of the parsed syntax tree.</returns>
        public static UvssDocumentSyntax Parse(String source, UvssParserOptions options = UvssParserOptions.None)
        {
            Contract.Require(source, nameof(source));

            var input = UvssLexer.Tokenize(source);
            var position = 0;
            var isDirectiveValid = true;

            var contentNodes = new List<SyntaxNode>();

            while (!input.IsPastEndOfStream(position))
            {
                if (position > 0)
                    input.Trim(position);

                var contentNode = AcceptDocumentContent(input, ref position, ref isDirectiveValid, options);
                if (contentNode == null)
                    break;

                contentNodes.Add(contentNode);
            }

            var contentBuilder = SyntaxListBuilder<SyntaxNode>.Create();
            contentBuilder.AddRange(contentNodes);

            var endOfFileToken = GetNextToken(input, ref position);
            if (endOfFileToken.Kind != SyntaxKind.EndOfFileToken)
                throw new InvalidOperationException();

            AddDiagnosticsToSkippedSyntaxTrivia(endOfFileToken, DiagnosticInfo.ReportUnexpectedTokenInDocumentContent);

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
            var firstToken = node.GetFirstToken();
            if (firstToken != null)
            {
                node.Position = firstToken.Position;
                node.Line = firstToken.Line;
                node.Column = firstToken.Column;
            }
            else
            {
                node.Position = defaultpos;
                node.Line = 0;
                node.Column = 0;
            }
            return node;
        }

        /// <summary>
        /// Gets the node position that corresponds to the specified position in the lexer token stream.
        /// </summary>
        private static void GetNodePositionFromLexerPosition(UvssLexerStream input, Int32 position, SyntaxNode node)
        {
            if (input.IsPastEndOfStream(position))
            {
                if (input.Count == 0)
                {
                    node.Position = 0;
                    node.Line = 0;
                    node.Column = 0;
                }
                else
                {
                    var token = input[input.Count - 1];
                    node.Position = token.SourceOffset + token.SourceLength;
                    node.Line = token.SourceLine;
                    node.Column = token.SourceColumn;
                }
            }
            else
            {
                var token = input[position];
                node.Position = token.SourceOffset;
                node.Line = token.SourceLine;
                node.Column = token.SourceColumn;
            }
        }

        /// <summary>
        /// Counts the number of trivia tokens starting at the specified position in the lexer stream.
        /// </summary>
        private static Int32 CountTrivia(
            UvssLexerStream input, Int32 position, Boolean acceptEndOfLine = true)
        {
            var count = 0;

            while (!input.IsPastEndOfStream(position))
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
        /// Gets a value indicating whether the specified token is the first non-whitespace on its line.
        /// </summary>
        private static Boolean IsFirstNonWhiteSpaceTokenOnLine(
            UvssLexerStream input, SyntaxToken token, UvssParserOptions options)
        {
            var position = token.Position + token.GetLeadingTriviaWidth();

            var leadingTrivia = token.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                if (leadingTrivia.IsList)
                {
                    for (int i = leadingTrivia.SlotCount - 1; i >= 0; i--)
                    {
                        var child = leadingTrivia.GetSlot(i);
                        if (child != null)
                        {
                            if (child.Kind == SyntaxKind.EndOfLineTrivia)
                                break;

                            if (child.Kind == SyntaxKind.WhitespaceTrivia)
                                position -= child.FullWidth;
                            else
                                return false;
                        }
                    }
                }
                else
                {
                    if (leadingTrivia.Kind == SyntaxKind.EndOfLineTrivia)
                        position = leadingTrivia.Position + leadingTrivia.FullWidth;

                    if (leadingTrivia.Kind == SyntaxKind.WhitespaceTrivia)
                        position -= leadingTrivia.FullWidth;
                    else
                        return false;
                }
            }

            if (position == 0 && (options & UvssParserOptions.PartialDocumentStartsOnEmptyLine) != 0)
                return false;

            return input.IsStartOfLine(position);
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
        /// <returns>true if the token contains trivia; otherwise, false.</returns>
        private static Boolean IsTrivia(UvssLexerToken token)
        {
            return
                token.Type == UvssLexerTokenType.EndOfLine ||
                token.Type == UvssLexerTokenType.SingleLineComment ||
                token.Type == UvssLexerTokenType.MultiLineComment ||
                token.Type == UvssLexerTokenType.WhiteSpace;
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
                kind == SyntaxKind.GreaterThanQuestionMarkToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified token kind terminates a selector.
        /// </summary>
        private static Boolean IsSelectorTerminator(SyntaxKind kind)
        {
            return
                kind == SyntaxKind.ExclamationMarkToken ||
                kind == SyntaxKind.AtSignToken ||
                kind == SyntaxKind.ColonToken ||
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
                kind == SyntaxKind.AsteriskToken ||
                kind == SyntaxKind.IdentifierToken ||
                kind == SyntaxKind.PeriodToken ||
                kind == SyntaxKind.HashToken ||
                kind == SyntaxKind.ExclamationMarkToken ||
                kind == SyntaxKind.ColonToken;
        }

        /// <summary>
        /// Gets a value indicating whether the specified text is a valid identifier.
        /// </summary>
        private static Boolean IsValidIdentifier(String text)
        {
            return regexValidIdentifier.IsMatch(text);
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

                case UvssLexerTokenType.Directive:
                    return SyntaxKind.DirectiveToken;

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
        /// <returns>The <see cref="SyntaxKind"/> that corresponds to the next non-trivia token.</returns>
        private static SyntaxKind SyntaxKindFromNextToken(
            UvssLexerStream input, Int32 position)
        {
            String text;
            return SyntaxKindFromNextToken(input, position, out text);
        }

        /// <summary>
        /// Seeks ahead in the lexer stream and returns the <see cref="SyntaxKind"/> that corresponds
        /// to the next non-trivia lexer token.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position in the lexer token stream.</param>
        /// <param name="text">The text of the next token in the stream.</param>
        /// <returns>The <see cref="SyntaxKind"/> that corresponds to the next non-trivia token.</returns>
        private static SyntaxKind SyntaxKindFromNextToken(
            UvssLexerStream input, Int32 position, out String text)
        {
            while (!input.IsPastEndOfStream(position))
            {
                var token = input[position];
                if (!IsTrivia(token))
                {
                    text = token.Text;
                    return SyntaxKindFromLexerTokenType(token);
                }
                position++;
            }
            text = null;
            return SyntaxKind.EndOfFileToken;
        }

        /// <summary>
        /// Gets the next syntax token which is represented in the lexer token stream.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position within the lexer token stream.</param>
        /// <returns>The next syntax token.</returns>
        private static SyntaxToken GetNextToken(
            UvssLexerStream input, ref Int32 position)
        {
            var leadingTrivia = AccumulateTrivia(input, ref position,
                isCurrentTokenTrivia: false,
                isLeading: true);
            var leadingTriviaNode = ConvertTriviaList(leadingTrivia);

            if (input.IsPastEndOfStream(position))
            {
                var token = new SyntaxToken(SyntaxKind.EndOfFileToken, null, leadingTriviaNode, null);
                GetNodePositionFromLexerPosition(input, position, token);
                return token;
            }
            else
            {
                var token = input[position];
                position++;

                var trailingTrivia = AccumulateTrivia(input, ref position);
                var trailingTriviaNode = ConvertTriviaList(trailingTrivia);

                return ConvertToken(token,
                    leadingTriviaNode, trailingTriviaNode);
            }
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax token.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <returns>The converted syntax token.</returns>
        private static SyntaxToken ConvertToken(UvssLexerToken token)
        {
            return ConvertToken(token, null, null);
        }

        /// <summary>
        /// Converts a lexer token to the corresponding syntax token.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <param name="leadingTrivia">The token's leading trivia, if any.</param>
        /// <param name="trailingTrivia">The token's trailing trivia, if any.</param>
        /// <returns>The converted syntax token.</returns>
        private static SyntaxToken ConvertToken(UvssLexerToken token,
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            var tokenKind = SyntaxKindFromLexerTokenType(token);
            var tokenText = token.Text;
            
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
                    Position = token.SourceOffset,
                    Line = token.SourceLine,
                    Column = token.SourceColumn
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
                    Position = token.SourceOffset,
                    Line = token.SourceLine,
                    Column = token.SourceColumn
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
            UvssLexerStream input, ref Int32 position, ref Boolean isDirectiveValid, UvssParserOptions options)
        {
            var nextTokenText = String.Empty;
            var nextTokenKind = SyntaxKindFromNextToken(input, position, out nextTokenText);
            if (nextTokenKind == SyntaxKind.EndOfFileToken)
                return null;

            if (IsPotentiallyStartOfSelectorPart(nextTokenKind))
            {
                var ruleSet = ExpectRuleSet(input, ref position);
                isDirectiveValid = false;
                return ruleSet;
            }
            else
            {
                switch (nextTokenKind)
                {
                    /* Storyboard */
                    case SyntaxKind.AtSignToken:
                        {
                            var storyboard = ExpectStoryboard(input, ref position);
                            isDirectiveValid = false;
                            return storyboard;
                        }

                    /* Directive */
                    case SyntaxKind.DirectiveToken:
                        {
                            switch (nextTokenText?.Substring(1))
                            {
                                case "culture":
                                    return ExpectCultureDirective(input, ref position, isDirectiveValid, options);

                                default:
                                    return ExpectUnknownDirective(input, ref position, isDirectiveValid, options);
                            }
                        }

                    /* ??? */
                    default:
                        {
                            var empty = ExpectEmptyStatement(input, ref position, kind =>
                            {
                                return
                                    IsPotentiallyStartOfSelectorPart(kind) ||
                                    kind == SyntaxKind.AtSignToken ||
                                    kind == SyntaxKind.DirectiveToken ||
                                    kind == SyntaxKind.EndOfFileToken;
                            });
                            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInDocumentContent);
                            isDirectiveValid = false;
                            return empty;
                        }
                }
            }
        }

        /// <summary>
        /// Produces a missing list.
        /// </summary>
        private static SyntaxList<TSyntax> MissingList<TSyntax>(
            UvssLexerStream input, Int32 position)
            where TSyntax : SyntaxNode
        {
            var node = new SyntaxList.MissingList();
            GetNodePositionFromLexerPosition(input, position, node);
            return new SyntaxList<TSyntax>(node);
        }

        /// <summary>
        /// Accepts a list of syntax nodes which are parsed using <paramref name="itemParser"/>.
        /// </summary>
        private static SyntaxList<TItem> AcceptList<TItem>(
            UvssLexerStream input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser) where TItem : SyntaxNode
        {
            var builder = default(SyntaxListBuilder<TItem>);
            var nodepos = position;

            while (true)
            {
                var item = itemParser(input, ref position);
                if (item == null || (item.IsMissing && builder.IsNull))
                    break;

                if (builder.IsNull)
                    builder = SyntaxListBuilder<TItem>.Create();

                builder.Add(item);

                if (item.IsMissing)
                    break;
            }

            var list = builder.IsNull ? default(SyntaxList<TItem>) : builder.ToList();
            if (list.Node != null)
            {
                GetNodePositionFromLexerPosition(input, nodepos, list.Node);
            }
            return list;
        }

        /// <summary>
        /// Produces a missing separated list.
        /// </summary>
        private static SeparatedSyntaxList<TSyntax> MissingSeparatedList<TSyntax>(
            UvssLexerStream input, Int32 position)
            where TSyntax : SyntaxNode
        {
            var node = new SyntaxList.MissingList();
            GetNodePositionFromLexerPosition(input, position, node);
            return new SeparatedSyntaxList<TSyntax>(node);
        }

        /// <summary>
        /// Accepts a separated list of syntax nodes which are parsed using <paramref name="itemParser"/>
        /// and separated by commas.
        /// </summary>
        private static SeparatedSyntaxList<TItem> AcceptSeparatedList<TItem>(
            UvssLexerStream input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser) where TItem : SyntaxNode
        {
            return AcceptSeparatedList(input, ref position, itemParser, AcceptComma);
        }

        /// <summary>
        /// Accepts a separated list of syntax nodes which are parsed using <paramref name="itemParser"/>
        /// and separated by tokens which are parsed using <paramref name="separatorParser"/>.
        /// </summary>
        private static SeparatedSyntaxList<TItem> AcceptSeparatedList<TItem>(
            UvssLexerStream input, ref Int32 position,
            UvssParserDelegate<TItem> itemParser,
            UvssParserDelegate<SyntaxToken> separatorParser) where TItem : SyntaxNode
        {
            var builder = default(SeparatedSyntaxListBuilder<TItem>);
            var nodepos = position;
            
            while (true)
            {
                var item = itemParser(input, ref position);
                if (item == null || (item.IsMissing && builder.IsNull))
                    break;

                if (builder.IsNull)
                    builder = SeparatedSyntaxListBuilder<TItem>.Create();

                builder.Add(item);

                if (item.IsMissing)
                    break;

                var separator = separatorParser(input, ref position);
                if (separator == null)
                    break;

                builder.AddSeparator(separator);
            }

            var list = builder.IsNull ? default(SeparatedSyntaxList<TItem>) : builder.ToList();
            if (list.Node != null)
            {
                GetNodePositionFromLexerPosition(input, nodepos, list.Node);

                var diagnosticsArray = list.Node.GetDiagnosticsArray();
                var diagnostics = (ICollection<DiagnosticInfo>)diagnosticsArray?.ToList();

                if (list.Count > 0 && list[list.Count - 1].IsMissing)
                    DiagnosticInfo.ReportMissingNode(ref diagnostics, list[list.Count - 1]);

                list.Node.SetDiagnostics(diagnostics);
            }
            return list;
        }

        /// <summary>
        /// Parses an unrecognized directive.
        /// </summary>
        private static UvssUnknownDirectiveSyntax ParseUnknownDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options, Boolean accept)
        {
            var directiveToken =
                ExpectToken(input, ref position, SyntaxKind.DirectiveToken);

            if (accept && directiveToken.IsMissing)
                return null;

            var directive = WithPosition(new UvssUnknownDirectiveSyntax(
                directiveToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (!isDirectiveValid)
            {
                DiagnosticInfo.ReportDirectiveAtInvalidPosition(ref diagnostics, directive);
            }
            else
            {
                if (!IsFirstNonWhiteSpaceTokenOnLine(input, directive.DirectiveToken, options))
                    DiagnosticInfo.ReportDirectiveMustBeFirstNonWhiteSpaceOnLine(ref diagnostics, directive);
            }

            DiagnosticInfo.ReportUnknownDirective(ref diagnostics, directive);

            directive.SetDiagnostics(diagnostics);

            return directive;
        }

        /// <summary>
        /// Accepts an unrecognized directive.
        /// </summary>
        private static UvssUnknownDirectiveSyntax AcceptUnknownDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options)
        {
            return ParseUnknownDirective(input, ref position, isDirectiveValid, options, true);
        }

        /// <summary>
        /// Expects an unrecognized directive and produces a missing node if one is not found.
        /// </summary>
        private static UvssUnknownDirectiveSyntax ExpectUnknownDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options)
        {
            return ParseUnknownDirective(input, ref position, isDirectiveValid, options, false);
        }

        /// <summary>
        /// Produces a missing $culture directive.
        /// </summary>
        private static UvssCultureDirectiveSyntax MissingCultureDirective(
            UvssLexerStream input, Int32 position)
        {
            return WithPosition(new UvssCultureDirectiveSyntax(
                MissingToken(SyntaxKind.CultureDirective, input, position),
                MissingPropertyValueWithBraces(input, position)));
        }

        /// <summary>
        /// Parses a $culture directive.
        /// </summary>
        private static UvssCultureDirectiveSyntax ParseCultureDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options, Boolean accept)
        {
            var nextTokenText = String.Empty;
            var nextToken = SyntaxKindFromNextToken(input, position, out nextTokenText);

            if (nextToken != SyntaxKind.DirectiveToken || nextTokenText != "$culture")
                return accept ? null : MissingCultureDirective(input, position);

            var directiveToken =
                ExpectToken(input, ref position, SyntaxKind.DirectiveToken);

            var cultureValue =
                ExpectPropertyValueWithBraces(input, ref position);

            var directive = WithPosition(new UvssCultureDirectiveSyntax(
                directiveToken,
                cultureValue));
            
            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (!isDirectiveValid)
            {
                DiagnosticInfo.ReportDirectiveAtInvalidPosition(ref diagnostics, directive);
            }
            else
            {
                if (!IsFirstNonWhiteSpaceTokenOnLine(input, directive.DirectiveToken, options))
                    DiagnosticInfo.ReportDirectiveMustBeFirstNonWhiteSpaceOnLine(ref diagnostics, directive);
            }

            if (!directive.CultureValue.IsMissing && !recognizedCultures.Contains(cultureValue.Value))
            {
                DiagnosticInfo.ReportUnknownCulture(ref diagnostics,
                    (SyntaxNode)directive.CultureValue.ContentToken ?? directive.CultureValue);
            }

            directive.SetDiagnostics(diagnostics);
            
            return directive;
        }

        /// <summary>
        /// Accepts a $culture directive.
        /// </summary>
        private static UvssCultureDirectiveSyntax AcceptCultureDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options)
        {
            return ParseCultureDirective(input, ref position, isDirectiveValid, options, true);
        }

        /// <summary>
        /// Expects a $culture directive and produces a missing node if one is not found.
        /// </summary>
        private static UvssCultureDirectiveSyntax ExpectCultureDirective(
            UvssLexerStream input, ref Int32 position, Boolean isDirectiveValid, UvssParserOptions options)
        {
            return ParseCultureDirective(input, ref position, isDirectiveValid, options, false);
        }

        /// <summary>
        /// Produces a missing block of nodes.
        /// </summary>
        private static UvssBlockSyntax MissingBlock(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
        {
            var block = SyntaxFactory.Block(children);
            block.IsMissing = true;

            GetNodePositionFromLexerPosition(input, position, block);

            return block;
        }

        /// <summary>
        /// Parses a block of nodes.
        /// </summary>
        private static UvssBlockSyntax ParseBlock(
            UvssLexerStream input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenCurlyBraceToken)
                return null;

            var openCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            var contentList = SyntaxListBuilder<SyntaxNode>.Create();
            if (!openCurlyBraceToken.IsMissing)
            {
                while (true)
                {
                    var contentNode = contentParser(input, ref position);
                    if (contentNode == null)
                        break;

                    contentList.Add(contentNode);

                    if (contentNode.IsMissing)
                        break;
                }
            }

            var closeCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.CloseCurlyBraceToken);

            input.Trim(position);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, openCurlyBraceToken);

            if (closeCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeCurlyBraceToken);

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
            UvssLexerStream input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser)
        {
            return ParseBlock(input, ref position, contentParser, true);
        }

        /// <summary>
        /// Expects a block of nodes and produces a missing node if one is not found.
        /// </summary>
        private static UvssBlockSyntax ExpectBlock(
            UvssLexerStream input, ref Int32 position, UvssParserDelegate<SyntaxNode> contentParser)
        {
            return ParseBlock(input, ref position, contentParser, false);
        }

        /// <summary>
        /// Produces a missing identifier.
        /// </summary>
        private static UvssIdentifierSyntax MissingIdentifier(
            UvssLexerStream input, Int32 position)
        {
            return WithPosition(new UvssIdentifierSyntax(
                MissingToken(SyntaxKind.IdentifierToken, input, position)));
        }

        /// <summary>
        /// Parses an identifier.
        /// </summary>
        private static UvssIdentifierBaseSyntax ParseIdentifier(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.OpenBracketToken)
            {
                var openBracketToken =
                    ExpectToken(input, ref position, SyntaxKind.OpenBracketToken);

                var eol = openBracketToken.HasTrailingLineBreaks;

                var start = position;
                var count = 0;
                var text = new StringBuilder();

                if (!eol && !openBracketToken.HasTrailingLineBreaks)
                {
                    while (!input.IsPastEndOfStream(position))
                    {
                        var type = input[position].Type;
                        if (type == UvssLexerTokenType.CloseBracket ||
                            type == UvssLexerTokenType.EndOfLine)
                        {
                            break;
                        }

                        text.Append(input[position].Text);
                        position++;
                        count++;
                    }
                }

                var identifierToken = default(SyntaxToken);
                if (count > 0)
                {
                    var identifierTokenTrivia = ConvertTriviaList(AccumulateTrivia(input, ref position));
                    if (openBracketToken.IsMissing)
                    {
                        identifierToken = MissingToken(SyntaxKind.IdentifierToken, input, position);
                    }
                    else
                    {
                        identifierToken = new SyntaxToken(SyntaxKind.IdentifierToken, text.ToString(), null, identifierTokenTrivia);
                        GetNodePositionFromLexerPosition(input, start, identifierToken);
                    }

                    eol = identifierToken.HasTrailingLineBreaks;
                }
                else
                {
                    identifierToken = MissingToken(SyntaxKind.IdentifierToken, input, position);
                }

                var closeBracketToken = openBracketToken.IsMissing ? MissingToken(SyntaxKind.CloseBracketToken, input, position) :
                    ExpectTokenOnSameLine(input, ref position, SyntaxKind.CloseBracketToken, ref eol);

                var identifier = WithPosition(new UvssEscapedIdentifierSyntax(
                    openBracketToken,
                    identifierToken,
                    closeBracketToken));

                var diagnostics = default(ICollection<DiagnosticInfo>);

                if (openBracketToken.IsMissing)
                    DiagnosticInfo.ReportMissingNode(ref diagnostics, openBracketToken);

                if (identifierToken.IsMissing)
                    DiagnosticInfo.ReportMissingNode(ref diagnostics, identifierToken);

                if (closeBracketToken.IsMissing)
                    DiagnosticInfo.ReportMissingNode(ref diagnostics, closeBracketToken);

                identifier.SetDiagnostics(diagnostics);

                return identifier;
            }
            else
            {
                return ParseUnescapedIdentifier(input, ref position, accept);
            }
        }

        /// <summary>
        /// Parses an unescaped identifier.
        /// </summary>
        private static UvssIdentifierSyntax ParseUnescapedIdentifier(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind != SyntaxKind.IdentifierToken)
                return accept ? null : MissingIdentifier(input, position);

            var identifierToken =
                ExpectToken(input, ref position, SyntaxKind.IdentifierToken);

            return WithPosition(new UvssIdentifierSyntax(
                identifierToken));
        }

        /// <summary>
        /// Accepts an identifier.
        /// </summary>
        private static UvssIdentifierBaseSyntax AcceptIdentifier(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseIdentifier(input, ref position, true);
        }

        /// <summary>
        /// Accepts an unescaped identifier.
        /// </summary>
        private static UvssIdentifierSyntax AcceptUnescapedIdentifier(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseUnescapedIdentifier(input, ref position, true);
        }

        /// <summary>
        /// Expects an identifier and produces a missing node if one is not found.
        /// </summary>
        private static UvssIdentifierBaseSyntax ExpectIdentifier(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseIdentifier(input, ref position, false);
        }

        /// <summary>
        /// Expects an unescaped identifier and produces a missing node if one is not found.
        /// </summary>
        private static UvssIdentifierSyntax ExpectUnescapedIdentifier(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseUnescapedIdentifier(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing property name.
        /// </summary>
        private static UvssPropertyNameSyntax MissingPropertyName(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var propertyName = default(UvssPropertyNameSyntax);

            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.IdentifierToken)
                return propertyName;

            var diagnostics = default(ICollection<DiagnosticInfo>);

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

                propertyName = WithPosition(new UvssPropertyNameSyntax(
                    attachedPropertyOwnerNameIdentifier,
                    periodToken,
                    propertyNameIdentifier));
            }
            else
            {
                var propertyNameIdentifier = firstPart;

                propertyName = WithPosition(new UvssPropertyNameSyntax(
                    null,
                    null,
                    propertyNameIdentifier));
            }
            
            if (propertyName.PropertyNameIdentifier.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, propertyName.PropertyNameIdentifier);

            propertyName.SetDiagnostics(diagnostics);

            return propertyName;
        }

        /// <summary>
        /// Accepts a property name.
        /// </summary>
        private static UvssPropertyNameSyntax AcceptPropertyName(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyName(input, ref position, true);
        }

        /// <summary>
        /// Expects a property name and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyNameSyntax ExpectPropertyName(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyName(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing event name.
        /// </summary>
        private static UvssEventNameSyntax MissingEventName(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var eventName = default(UvssEventNameSyntax);

            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.IdentifierToken)
                return eventName;

            var diagnostics = default(ICollection<DiagnosticInfo>);

            var firstPart =
                ExpectIdentifier(input, ref position);

            var periodToken =
                AcceptToken(input, ref position, SyntaxKind.PeriodToken);

            if (periodToken != null)
            {
                var attachedEventOwnerNameIdentifier =
                    firstPart;

                var eventNameIdentifier =
                    ExpectIdentifier(input, ref position);

                eventName = WithPosition(new UvssEventNameSyntax(
                    attachedEventOwnerNameIdentifier,
                    periodToken,
                    eventNameIdentifier));
            }
            else
            {
                var eventNameIdentifier = firstPart;

                eventName = WithPosition(new UvssEventNameSyntax(
                    null,
                    null,
                    eventNameIdentifier));                
            }

            if (eventName.EventNameIdentifier.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, eventName.EventNameIdentifier);

            eventName.SetDiagnostics(diagnostics);

            return eventName;
        }

        /// <summary>
        /// Accepts an event name.
        /// </summary>
        private static UvssEventNameSyntax AcceptEventName(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventName(input, ref position, true);
        }

        /// <summary>
        /// Expects an event name and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventNameSyntax ExpectEventName(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventName(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax MissingNavigationExpression(
            UvssLexerStream input, Int32 position)
        {
            return new UvssNavigationExpressionSyntax(
                MissingToken(SyntaxKind.PipeToken, input, position),
                MissingPropertyName(input, position),
                null,
                MissingToken(SyntaxKind.AsKeyword, input, position),
                MissingIdentifier(input, position));
        }

        /// <summary>
        /// Parses a navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax ParseNavigationExpression(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.PipeToken)
                return null;

            var pipeToken =
                ExpectToken(input, ref position, SyntaxKind.PipeToken);

            var propertyName =
                ExpectPropertyName(input, ref position);

            var indexer =
                AcceptNavigationExpressionIndexer(input, ref position);

            var asKeyword =
                ExpectToken(input, ref position, SyntaxKind.AsKeyword);

            var typeNameIdentifier =
                ExpectIdentifier(input, ref position);

            var expression = WithPosition(new UvssNavigationExpressionSyntax(
                pipeToken,
                propertyName,
                indexer,
                asKeyword,
                typeNameIdentifier));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (pipeToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, pipeToken);

            if (asKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, asKeyword);

            if (typeNameIdentifier.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, typeNameIdentifier);

            expression.SetDiagnostics(diagnostics);

            return expression;
        }

        /// <summary>
        /// Accepts a navigation expression.
        /// </summary>
        private static UvssNavigationExpressionSyntax AcceptNavigationExpression(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseNavigationExpression(input, ref position, true);
        }

        /// <summary>
        /// Expects a navigation expression and produces a missing node if one is not found.
        /// </summary>
        private static UvssNavigationExpressionSyntax ExpectNavigationExpression(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseNavigationExpression(input, ref position, false);
        }

        /// <summary>
        /// Parses a navigation expression indexer.
        /// </summary>
        private static UvssNavigationExpressionIndexerSyntax ParseNavigationExpressionIndexer(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenBracketToken)
                return null;

            var openBracketToken =
                ExpectToken(input, ref position, SyntaxKind.OpenBracketToken);

            var numberToken = SyntaxKindFromNextToken(input, position) == SyntaxKind.CloseBracketToken ?
                MissingToken(SyntaxKind.NumberToken, input, position) :
                ExpectToken(input, ref position, null);

            var closeBracketToken =
                ExpectToken(input, ref position, SyntaxKind.CloseBracketToken);

            var indexer = new UvssNavigationExpressionIndexerSyntax(
                openBracketToken,
                numberToken,
                closeBracketToken);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (numberToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, numberToken);
            else
            {
                var index = 0;
                if (numberToken.Kind != SyntaxKind.NumberToken || !Int32.TryParse(numberToken.Text, out index))
                    DiagnosticInfo.ReportIndexMustBeIntegerValue(ref diagnostics, numberToken);
            }

            if (closeBracketToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeBracketToken);

            indexer.SetDiagnostics(diagnostics);

            return WithPosition(indexer);
        }

        /// <summary>
        /// Accepts a navigation expression indexer.
        /// </summary>
        private static UvssNavigationExpressionIndexerSyntax AcceptNavigationExpressionIndexer(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseNavigationExpressionIndexer(input, ref position, true);
        }

        /// <summary>
        /// Expects a navigation expression indexer and produces a missing node if one is not found.
        /// </summary>
        private static UvssNavigationExpressionIndexerSyntax ExpectNavigationExpressionIndexer(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseNavigationExpressionIndexer(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing selector.
        /// </summary>
        private static UvssSelectorSyntax MissingSelector(
            UvssLexerStream input, Int32 position)
        {
            return WithPosition(new UvssSelectorSyntax(
                MissingList<SyntaxNode>(input, position)));
        }

        /// <summary>
        /// Parses a selector.
        /// </summary>
        private static UvssSelectorSyntax ParseSelector(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var components = AcceptList(
                input, ref position, AcceptSelectorPartOrCombinator);

            if (components.Node == null)
            {
                if (accept)
                    return null;

                var componentsBuilder = new SyntaxListBuilder<SyntaxNode>(1);
                componentsBuilder.Add(MissingToken(SyntaxKind.IdentifierToken, input, position));
                components = componentsBuilder.ToList();

                return WithPosition(new UvssSelectorSyntax(components));
            }

            var selector = WithPosition(new UvssSelectorSyntax(
                components));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            var expectingPart = false;
            for (int i = 0; i < selector.Components.Count; i++)
            {
                if (i + 1 == selector.Components.Count)
                    expectingPart = true;

                var component = selector.Components[i];
                var componentIsCombinator = !(component is UvssSelectorPartBaseSyntax);
                if (componentIsCombinator)
                {
                    if (expectingPart)
                    {
                        DiagnosticInfo.ReportInvalidSelector(ref diagnostics, selector);
                        break;
                    }
                    expectingPart = true;
                }
                else
                {
                    expectingPart = false;
                }
            }

            selector.SetDiagnostics(diagnostics);

            return selector;
        }

        /// <summary>
        /// Accepts a selector.
        /// </summary>
        private static UvssSelectorSyntax AcceptSelector(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelector(input, ref position, true);
        }

        /// <summary>
        /// Expects a selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorSyntax ExpectSelector(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelector(input, ref position, false);
        }

        /// <summary>
        /// Parses a selector with an optional trailing navigation expression.
        /// </summary>
        private static UvssSelectorWithNavigationExpressionSyntax ParseSelectorWithNavigationExpression(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var selector =
                ExpectSelector(input, ref position);

            if (accept && selector.IsMissing)
                return null;

            var navigationExpression =
                AcceptNavigationExpression(input, ref position);

            var selectorWithNavExp = new UvssSelectorWithNavigationExpressionSyntax(
                selector,
                navigationExpression);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            selectorWithNavExp.SetDiagnostics(diagnostics);

            return WithPosition(selectorWithNavExp);
        }

        /// <summary>
        /// Accepts a selector with an optional trailing navigation expression.
        /// </summary>
        private static UvssSelectorWithNavigationExpressionSyntax AcceptSelectorWithNavigationExpression(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorWithNavigationExpression(input, ref position, true);
        }

        /// <summary>
        /// Expects a selector with an optional trailing navigation expression and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorWithNavigationExpressionSyntax ExpectSelectorWithNavigationExpression(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorWithNavigationExpression(input, ref position, false);
        }

        /// <summary>
        /// Accepts a selector part or combinator.
        /// </summary>
        private static SyntaxNode AcceptSelectorPartOrCombinator(
            UvssLexerStream input, ref Int32 position)
        {
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
        /// Creates an invalid selector part from the specified sequence of lexer tokens.
        /// </summary>
        private static UvssInvalidSelectorPartSyntax CreateInvalidSelectorPart(
            UvssLexerStream input, Int32 start, Int32 count, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            var componentsBuilder = SyntaxListBuilder<SyntaxToken>.Create();
            for (int i = 0; i < count; i++)
            {
                var componentInput = input[start + i];
                var componentOutput = ConvertToken(componentInput,
                    leadingTrivia: (i == 0) ? leadingTrivia : null,
                    trailingTrivia: (i + 1 == count) ? trailingTrivia : null);
                componentsBuilder.Add(componentOutput);
            }
            var components = componentsBuilder.ToList();

            var invalidSelectorPart = new UvssInvalidSelectorPartSyntax(
                components);

            var diagnostics = default(ICollection<DiagnosticInfo>);
            DiagnosticInfo.ReportInvalidSelectorPart(ref diagnostics, invalidSelectorPart);

            invalidSelectorPart.SetDiagnostics(diagnostics);

            return WithPosition(invalidSelectorPart);
        }

        /// <summary>
        /// Creates a selector part type from the current position in the specified sequence of lexer tokens.
        /// </summary>
        private static Boolean CreateSelectorPartType(
            UvssLexerStream input, String[] pieces, ref Int32 position, ref Int32 offset, ref Int32 line, ref Int32 column, out UvssSelectorPartTypeSyntax partType)
        {
            partType = null;

            if (position >= pieces.Length)
                return true;

            var selectedTypeText = pieces[position];
            if (!IsValidIdentifier(selectedTypeText) && selectedTypeText != "*")
                return true;
            
            var selectedTypeIdentifierToken =
                new SyntaxToken(SyntaxKind.IdentifierToken, selectedTypeText);
            selectedTypeIdentifierToken.Position = offset;
            selectedTypeIdentifierToken.Line = line;
            selectedTypeIdentifierToken.Column = column;

            var selectedTypeIdentifier = default(UvssIdentifierSyntax);
            selectedTypeIdentifier = WithPosition(
                new UvssIdentifierSyntax(selectedTypeIdentifierToken));

            position++;

            var exclamationMarkToken = default(SyntaxToken);

            if (position < pieces.Length)
            {
                var exclamationMarkText = pieces[position];
                if (exclamationMarkText == "!")
                {
                    exclamationMarkToken = new SyntaxToken(
                        SyntaxKind.ExclamationMarkToken, exclamationMarkText, null, null);
                    exclamationMarkToken.Position = offset + selectedTypeText.Length;
                    exclamationMarkToken.Line = line;
                    exclamationMarkToken.Column = column + selectedTypeText.Length;

                    position++;
                }
            }
            
            partType = WithPosition(new UvssSelectorPartTypeSyntax(
                selectedTypeIdentifier,
                exclamationMarkToken));

            return true;
        }

        /// <summary>
        /// Creates a selector part name from the current position in the specified sequence of lexer tokens.
        /// </summary>
        private static Boolean CreateSelectorPartName(
            UvssLexerStream input, String[] pieces, ref Int32 position, ref Int32 offset, ref Int32 line, ref Int32 column, out UvssSelectorPartNameSyntax partName)
        {
            partName = null;

            if (position + 1 >= pieces.Length)
                return true;

            var hashText = pieces[position];
            if (hashText != "#")
                return true;

            var identifierText = pieces[position + 1];
            if (!IsValidIdentifier(identifierText))
                return false;

            position += 2;

            var hashToken =
                new SyntaxToken(SyntaxKind.HashToken, hashText, null, null);
            hashToken.Position = offset;
            hashToken.Line = line;
            hashToken.Column = column;

            var selectedNameIdentifierToken =
                new SyntaxToken(SyntaxKind.IdentifierToken, identifierText, null, null);
            selectedNameIdentifierToken.Position = offset + hashText.Length;
            selectedNameIdentifierToken.Line = line;
            selectedNameIdentifierToken.Column = column + hashText.Length;

            var selectedNameIdentifier = WithPosition(new UvssIdentifierSyntax(
                selectedNameIdentifierToken));

            partName = WithPosition(new UvssSelectorPartNameSyntax(
                hashToken,
                selectedNameIdentifier));

            return true;
        }

        /// <summary>
        /// Creates a selector part class from the current position in the specified sequence of lexer tokens.
        /// </summary>
        private static Boolean CreateSelectorPartClass(
            UvssLexerStream input, String[] pieces, ref Int32 position, ref Int32 offset, ref Int32 line, ref Int32 column, out UvssSelectorPartClassSyntax partClass)
        {
            partClass = null;

            if (position + 1 >= pieces.Length)
                return true;
            
            var periodText = pieces[position];
            if (periodText != ".")
                return true;

            var identifierText = pieces[position + 1];
            if (!IsValidIdentifier(identifierText))
                return false;

            position += 2;

            var periodToken =
                new SyntaxToken(SyntaxKind.PeriodToken, periodText, null, null);
            periodToken.Position = offset;
            periodToken.Line = line;
            periodToken.Column = column;

            var selectedClassIdentifier = WithPosition(new UvssIdentifierSyntax(
                new SyntaxToken(SyntaxKind.IdentifierToken, identifierText, null, null)));
            selectedClassIdentifier.Position = offset + periodText.Length;
            selectedClassIdentifier.Line = line;
            selectedClassIdentifier.Column = column + periodText.Length;
            
            partClass = WithPosition(new UvssSelectorPartClassSyntax(
                periodToken,
                selectedClassIdentifier));

            return true;
        }

        /// <summary>
        /// Creates a selector part class list from the current position in the specified sequence of lexer tokens.
        /// </summary>
        private static Boolean CreateSelectorPartClasses(
            UvssLexerStream input, String[] pieces, ref Int32 position, ref Int32 offset, ref Int32 line, ref Int32 column, out SyntaxList<UvssSelectorPartClassSyntax> partClasses)
        {
            partClasses = null;

            var listStart = position;
            var listBuilder = SyntaxListBuilder<UvssSelectorPartClassSyntax>.Create();
            while (position < pieces.Length)
            {
                var listItem = default(UvssSelectorPartClassSyntax);
                if (!CreateSelectorPartClass(input, pieces, ref position, ref offset, ref line, ref column, out listItem))
                {
                    position = listStart;
                    return false;
                }

                if (listItem == null)
                    break;

                listBuilder.Add(listItem);
            }

            partClasses = 
                listBuilder.ToList();

            return true;
        }

        /// <summary>
        /// Creates a selector part pseudo-class from the current position in the specified sequence of lexer tokens.
        /// </summary>
        private static Boolean CreateSelectorPartPseudoClass(
            UvssLexerStream input, String[] pieces, ref Int32 position, ref Int32 offset, ref Int32 line, ref Int32 column, out UvssPseudoClassSyntax partPseudoClass)
        {
            partPseudoClass = null;

            if (position + 1 >= pieces.Length)
                return true;

            var colonText = pieces[position];
            if (colonText != ":")
                return true;

            var identifierText = pieces[position + 1];
            if (!IsValidIdentifier(identifierText))
                return false;

            position += 2;

            var colonToken =
                new SyntaxToken(SyntaxKind.ColonToken, colonText, null, null);
            colonToken.Position = offset;
            colonToken.Line = line;
            colonToken.Column = column;

            var classNameIdentifierToken =
                new SyntaxToken(SyntaxKind.IdentifierToken, identifierText, null, null);
            classNameIdentifierToken.Position = offset + colonText.Length;
            classNameIdentifierToken.Line = line;
            classNameIdentifierToken.Column = column + colonText.Length;

            var classNameIdentifier = WithPosition(new UvssIdentifierSyntax(
                classNameIdentifierToken));

            partPseudoClass = WithPosition(new UvssPseudoClassSyntax(
                colonToken,
                classNameIdentifier));

            return true;
        }
        
        /// <summary>
        /// Parses a selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax ParseSelectorPart(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var leadingTriviaList = AccumulateTrivia(input, ref position, isLeading: true);
            var leadingTrivia = ConvertTriviaList(leadingTriviaList);
            
            var partStart = position;
            var partCount = 0;
            var partLength = 0;

            while (!input.IsPastEndOfStream(position))
            {
                var lexerTokenType = input[position].Type;
                if (lexerTokenType != UvssLexerTokenType.Asterisk &&
                    lexerTokenType != UvssLexerTokenType.Identifier &&
                    lexerTokenType != UvssLexerTokenType.Keyword &&
                    lexerTokenType != UvssLexerTokenType.Period &&
                    lexerTokenType != UvssLexerTokenType.Hash &&
                    lexerTokenType != UvssLexerTokenType.ExclamationMark &&
                    lexerTokenType != UvssLexerTokenType.Colon)
                {
                    break;
                }
                
                partCount++;
                partLength += input[position++].SourceLength;
            }

            var trailingTriviaList = AccumulateTrivia(input, ref position, isLeading: false);
            var trailingTrivia = ConvertTriviaList(trailingTriviaList);

            var partTextBuilder = new StringBuilder(partLength);
            for (int i = 0; i < partCount; i++)
                partTextBuilder.Append(input[partStart + i].Text);

            var partPieces = Regex.Split(partTextBuilder.ToString(), @"(#|\.|:|!)")
                .Where(x => !String.IsNullOrEmpty(x)).ToArray();

            if (partPieces.Length == 0)
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);

            var partPosition = 0;

            var partType = default(UvssSelectorPartTypeSyntax);
            var partName = default(UvssSelectorPartNameSyntax);
            var partClasses = default(SyntaxList<UvssSelectorPartClassSyntax>);
            var partPseudoClass = default(UvssPseudoClassSyntax);

            var offset = input[partStart].SourceOffset;
            var line = input[partStart].SourceLine;
            var column = input[partStart].SourceColumn;

            if (!CreateSelectorPartType(input, partPieces, ref partPosition, ref offset, ref line, ref column, out partType))
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);

            if (!CreateSelectorPartName(input, partPieces, ref partPosition, ref offset, ref line, ref column, out partName))
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);

            if (!CreateSelectorPartClasses(input, partPieces, ref partPosition, ref offset, ref line, ref column, out partClasses))
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);

            if (!CreateSelectorPartPseudoClass(input, partPieces, ref partPosition, ref offset, ref line, ref column, out partPseudoClass))
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);

            if (partPosition != partPieces.Length)
                return CreateInvalidSelectorPart(input, partStart, partCount, leadingTrivia, trailingTrivia);
            
            var selectorPart = new UvssSelectorPartSyntax(
                partType,
                partName,
                partClasses,
                partPseudoClass);
            selectorPart.ChangeTrivia(leadingTrivia, trailingTrivia);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            selectorPart.SetDiagnostics(diagnostics);

            return WithPosition(selectorPart);
        }

        /// <summary>
        /// Accepts a selector part.
        /// </summary>
        private static UvssSelectorPartBaseSyntax AcceptSelectorPart(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorPart(input, ref position, true);
        }

        /// <summary>
        /// Expects a selector part and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorPartBaseSyntax ExpectSelectorPart(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorPart(input, ref position, false);
        }
        
        /// <summary>
        /// Produces a missing parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax MissingSelectorWithParentheses(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var selectorContent =
                ExpectSelector(input, ref position);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            var selector = WithPosition(new UvssSelectorWithParenthesesSyntax(
                openParenToken,
                selectorContent,
                closeParenToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, openParenToken);

            if (selectorContent.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, selectorContent);

            if (closeParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeParenToken);

            selector.SetDiagnostics(diagnostics);

            return selector;
        }

        /// <summary>
        /// Accepts a parentheses-enclosed selector.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax AcceptSelectorWithParentheses(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorWithParentheses(input, ref position, true);
        }

        /// <summary>
        /// Expects a parentheses-enclosed selector and produces a missing node if one is not found.
        /// </summary>
        private static UvssSelectorWithParenthesesSyntax ExpectSelectorWithParentheses(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseSelectorWithParentheses(input, ref position, false);
        }

        /// <summary>
        /// Parses a property value token from the lexer stream.
        /// </summary>
        private static SyntaxToken ParsePropertyValueToken(
            UvssLexerStream input, ref Int32 position, Boolean accept, SyntaxKind terminator, ref Boolean eol)
        {
            if (eol)
                return accept ? null : MissingToken(SyntaxKind.PropertyValueToken, input, position);

            var startpos = position;
            var start = position + CountTrivia(input, position, acceptEndOfLine: false);
            var end = start;
            var length = 0;
            var lastNonWhitespace = start;

            while (!input.IsPastEndOfStream(end))
            {
                var current = input[end];
                var currentKind = SyntaxKindFromLexerTokenType(current);

                if (currentKind == SyntaxKind.EndOfLineTrivia)
                {
                    eol = true;
                    break;
                }

                if (currentKind == terminator ||
                    currentKind == SyntaxKind.ImportantKeyword)
                {
                    break;
                }

                if (currentKind != SyntaxKind.WhitespaceTrivia)
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

            GetNodePositionFromLexerPosition(input, startpos, valueToken);

            return valueToken;
        }

        /// <summary>
        /// Accepts a property value token.
        /// </summary>
        private static SyntaxToken AcceptPropertyValueToken(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValueToken(input, ref position, true, SyntaxKind.SemiColonToken, ref eol);
        }

        /// <summary>
        /// Expects a property value token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueToken(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValueToken(input, ref position, false, SyntaxKind.SemiColonToken, ref eol);
        }

        /// <summary>
        /// Expects a property value token on the same line as previous nodes and
        /// produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueTokenOnSameLine(
            UvssLexerStream input, ref Int32 position, ref Boolean eol)
        {
            return ParsePropertyValueToken(input, ref position, false, SyntaxKind.SemiColonToken, ref eol);
        }

        /// <summary>
        /// Accepts a brace-enclosed property value token.
        /// </summary>
        private static SyntaxToken AcceptPropertyValueTokenWithBraces(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValueToken(input, ref position, true, SyntaxKind.CloseCurlyBraceToken, ref eol);
        }

        /// <summary>
        /// Accepts a brace-enclosed property value token on the same line as previous nodes.
        /// </summary>
        private static SyntaxToken AcceptPropertyValueTokenWithBracesOnSameLine(
            UvssLexerStream input, ref Int32 position, ref Boolean eol)
        {
            return ParsePropertyValueToken(input, ref position, true, SyntaxKind.CloseCurlyBraceToken, ref eol);
        }

        /// <summary>
        /// Expects a brace-enclosed property value token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueTokenWithBraces(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValueToken(input, ref position, false, SyntaxKind.CloseCurlyBraceToken, ref eol);
        }

        /// <summary>
        /// Expects a brace-enclosed property value token on the same line as previous nodes and
        /// produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectPropertyValueTokenWithBracesOnSameLine(
            UvssLexerStream input, ref Int32 position, ref Boolean eol)
        {
            return ParsePropertyValueToken(input, ref position, false, SyntaxKind.CloseCurlyBraceToken, ref eol);
        }

        /// <summary>
        /// Produces a missing property value.
        /// </summary>
        private static UvssPropertyValueSyntax MissingPropertyValue(
            UvssLexerStream input, Int32 position)
        {
            return WithPosition(new UvssPropertyValueSyntax(
                MissingToken(SyntaxKind.PropertyValueToken, input, position)));
        }

        /// <summary>
        /// Parses a property value.
        /// </summary>
        private static UvssPropertyValueSyntax ParsePropertyValue(
            UvssLexerStream input, ref Int32 position, Boolean accept, ref Boolean eol)
        {
            var contentToken =
                ExpectPropertyValueTokenOnSameLine(input, ref position, ref eol);

            if (accept && contentToken.IsMissing)
                return null;

            return WithPosition(new UvssPropertyValueSyntax(
                contentToken));
        }

        /// <summary>
        /// Accepts a property value.
        /// </summary>
        private static UvssPropertyValueSyntax AcceptPropertyValue(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValue(input, ref position, true, ref eol);
        }

        /// <summary>
        /// Expects a property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueSyntax ExpectPropertyValue(
            UvssLexerStream input, ref Int32 position)
        {
            var eol = false;
            return ParsePropertyValue(input, ref position, false, ref eol);
        }

        /// <summary>
        /// Expects a property value on the same line as previous nodes and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueSyntax ExpectPropertyValueOnSameLine(
            UvssLexerStream input, ref Int32 position, ref Boolean eol)
        {
            if (eol)
                return MissingPropertyValue(input, position);

            return ParsePropertyValue(input, ref position, false, ref eol);
        }

        /// <summary>
        /// Produces a missing brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax MissingPropertyValueWithBraces(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenCurlyBraceToken)
                return null;

            var openCurlyBraceToken =
                ExpectToken(input, ref position, SyntaxKind.OpenCurlyBraceToken);

            var eol = IsEndOfLine(openCurlyBraceToken);

            var contentToken = openCurlyBraceToken.IsMissing ? null :
                AcceptPropertyValueTokenWithBracesOnSameLine(input, ref position, ref eol);

            var closeCurlyBraceToken = openCurlyBraceToken.IsMissing ? MissingToken(SyntaxKind.CloseCurlyBraceToken, input, position) :
                ExpectTokenOnSameLine(input, ref position, SyntaxKind.CloseCurlyBraceToken, ref eol);

            var value = WithPosition(new UvssPropertyValueWithBracesSyntax(
                openCurlyBraceToken,
                contentToken,
                closeCurlyBraceToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, openCurlyBraceToken);

            if (closeCurlyBraceToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeCurlyBraceToken);

            value.SetDiagnostics(diagnostics);

            return value;
        }

        /// <summary>
        /// Accepts a brace-enclosed property value.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax AcceptPropertyValueWithBraces(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyValueWithBraces(input, ref position, true);
        }

        /// <summary>
        /// Expects a brace-enclosed property value and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyValueWithBracesSyntax ExpectPropertyValueWithBraces(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyValueWithBraces(input, ref position, false);
        }

        /// <summary>
        /// Accepts any node which is valid inside of a rule set body.
        /// </summary>
        private static SyntaxNode AcceptRuleSetBodyNode(
            UvssLexerStream input, ref Int32 position)
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

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.IdentifierToken ||
                    kind == SyntaxKind.OpenBracketToken ||
                    kind == SyntaxKind.TransitionKeyword ||
                    kind == SyntaxKind.TriggerKeyword ||
                    kind == SyntaxKind.CloseCurlyBraceToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInRuleSetBody);
            return empty;
        }

        /// <summary>
        /// Produces a missing rule set.
        /// </summary>
        private static UvssRuleSetSyntax MissingRuleSet(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssRuleSetSyntax(
                MissingSeparatedList<UvssSelectorWithNavigationExpressionSyntax>(input, position),
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses a rule set.
        /// </summary>
        private static UvssRuleSetSyntax ParseRuleSet(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var diagnostics = default(ICollection<DiagnosticInfo>);

            var selectors =
                AcceptSeparatedList(input, ref position, ExpectSelectorWithNavigationExpression, AcceptComma);

            if (accept && selectors.Node == null)
                return null;

            var body =
                ExpectBlock(input, ref position, AcceptRuleSetBodyNode);

            var ruleSet = WithPosition(new UvssRuleSetSyntax(
                selectors,
                body));

            ruleSet.SetDiagnostics(diagnostics);

            return ruleSet;
        }

        /// <summary>
        /// Accepts a rule set.
        /// </summary>
        private static UvssRuleSetSyntax AcceptRuleSet(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseRuleSet(input, ref position, true);
        }

        /// <summary>
        /// Expects a rule set and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSetSyntax ExpectRuleSet(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseRuleSet(input, ref position, false);
        }

        /// <summary>
        /// Parses a rule.
        /// </summary>
        private static UvssRuleSyntax ParseRule(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var propertyName = accept ?
                AcceptPropertyName(input, ref position) :
                ExpectPropertyName(input, ref position);

            if (accept && propertyName == null)
                return null;

            var eol = IsEndOfLine(propertyName);

            var colonToken =
                ExpectTokenOnSameLine(input, ref position, SyntaxKind.ColonToken, ref eol);

            var value =
                ExpectPropertyValueOnSameLine(input, ref position, ref eol);

            var qualifierToken = 
                AcceptTokenOnSameLine(input, ref position, SyntaxKind.ImportantKeyword, ref eol);

            var semiColonToken =
                ExpectTokenOnSameLine(input, ref position, SyntaxKind.SemiColonToken, ref eol);

            input.Trim(position);

            var rule = WithPosition(new UvssRuleSyntax(
                propertyName,
                colonToken,
                value,
                qualifierToken,
                semiColonToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (colonToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, colonToken);

            if (value.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, value);

            if (semiColonToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, semiColonToken);

            rule.SetDiagnostics(diagnostics);

            return rule;
        }

        /// <summary>
        /// Accepts a rule.
        /// </summary>
        private static UvssRuleSyntax AcceptRule(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseRule(input, ref position, true);
        }

        /// <summary>
        /// Expects a rule and produces a missing node if one is not found.
        /// </summary>
        private static UvssRuleSyntax ExpectRule(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseRule(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing transition.
        /// </summary>
        private static UvssTransitionSyntax MissingTransition(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TransitionKeyword)
                return null;

            var eol = false;

            var transitionKeyword =
                ExpectToken(input, ref position, SyntaxKind.TransitionKeyword);
            
            var argumentList =
                ExpectTransitionArgumentList(input, ref position);

            var colonToken =
                ExpectTokenOnSameLine(input, ref position, SyntaxKind.ColonToken, ref eol);

            var value = 
                ExpectPropertyValueOnSameLine(input, ref position, ref eol);

            var qualifierToken = 
                AcceptTokenOnSameLine(input, ref position, SyntaxKind.ImportantKeyword, ref eol);

            var semiColonToken = 
                ExpectTokenOnSameLine(input, ref position, SyntaxKind.SemiColonToken, ref eol);

            input.Trim(position);

            var transition = WithPosition(new UvssTransitionSyntax(
                transitionKeyword,
                argumentList,
                colonToken,
                value,
                qualifierToken,
                semiColonToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (transitionKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, transitionKeyword);

            if (!argumentList.IsMissing)
            {
                if (argumentList.Arguments.Count < 2)
                    DiagnosticInfo.ReportTransitionHasTooFewArguments(ref diagnostics, argumentList.Arguments.Node ?? argumentList);
                else if (argumentList.Arguments.Count > 3)
                    DiagnosticInfo.ReportTransitionHasTooManyArguments(ref diagnostics, argumentList.Arguments.Node ?? argumentList);
            }

            if (colonToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, colonToken);

            if (value.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, value);

            if (semiColonToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, semiColonToken);

            transition.SetDiagnostics(diagnostics);

            return transition;
        }

        /// <summary>
        /// Accepts a transition.
        /// </summary>
        private static UvssTransitionSyntax AcceptTransition(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTransition(input, ref position, true);
        }

        /// <summary>
        /// Expects a transition and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionSyntax ExpectTransition(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTransition(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax MissingTransitionArgumentList(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var arguments = openParenToken.IsMissing ? MissingSeparatedList<SyntaxNode>(input, position) :
                AcceptSeparatedList<SyntaxNode>(input, ref position, ExpectIdentifier, AcceptComma);

            var closeParenToken = openParenToken.IsMissing ? MissingToken(SyntaxKind.CloseParenthesesToken, input, position) :
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            var argumentList = WithPosition(new UvssTransitionArgumentListSyntax(
                openParenToken,
                arguments,
                closeParenToken));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, openParenToken);

            if (closeParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeParenToken);

            argumentList.SetDiagnostics(diagnostics);

            return argumentList;
        }

        /// <summary>
        /// Accepts a transition argument list.
        /// </summary>
        private static UvssTransitionArgumentListSyntax AcceptTransitionArgumentList(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTransitionArgumentList(input, ref position, true);
        }

        /// <summary>
        /// Expects a transition argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssTransitionArgumentListSyntax ExpectTransitionArgumentList(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTransitionArgumentList(input, ref position, false);
        }

        /// <summary>
        /// Accepts an event trigger argument.
        /// </summary>
        private static SyntaxNode AcceptEventTriggerArgument(
            UvssLexerStream input, ref Int32 position)
        {
            var nextTokenKind = SyntaxKindFromNextToken(input, position);
            if (nextTokenKind == SyntaxKind.CloseParenthesesToken || nextTokenKind == SyntaxKind.EndOfFileToken)
                return null;

            var token = AcceptToken(input, ref position,
                SyntaxKind.HandledKeyword,
                SyntaxKind.SetHandledKeyword);

            if (token != null)
                return token;

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.HandledKeyword ||
                    kind == SyntaxKind.SetHandledKeyword ||
                    kind == SyntaxKind.CloseParenthesesToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInEventTriggerArgumentList);
            return empty;
        }

        /// <summary>
        /// Produces a missing event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList MissingEventTriggerArgumentList(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.OpenParenthesesToken)
                return null;

            var openParenToken =
                ExpectToken(input, ref position, SyntaxKind.OpenParenthesesToken);

            var argumentListContent =
                AcceptSeparatedList(input, ref position, AcceptEventTriggerArgument, AcceptComma);

            var closeParenToken =
                ExpectToken(input, ref position, SyntaxKind.CloseParenthesesToken);

            var argumentList = WithPosition(new UvssEventTriggerArgumentList(
                openParenToken,
                argumentListContent,
                closeParenToken));
            
            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (openParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, openParenToken);

            if (closeParenToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, closeParenToken);

            argumentList.SetDiagnostics(diagnostics);

            return argumentList;
        }

        /// <summary>
        /// Accepts an event trigger argument list.
        /// </summary>
        private static UvssEventTriggerArgumentList AcceptEventTriggerArgumentList(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventTriggerArgumentList(input, ref position, true);
        }

        /// <summary>
        /// Expects an event trigger argument list and produces a missing node if one is not found.
        /// </summary>
        private static UvssEventTriggerArgumentList ExpectEventTriggerArgumentList(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventTriggerArgumentList(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing comparison operator.
        /// </summary>
        private static SyntaxToken MissingComparisonOperator(
            UvssLexerStream input, Int32 position)
        {
            return MissingToken(SyntaxKind.None, input, position);
        }

        /// <summary>
        /// Parses a comparison operator.
        /// </summary>
        private static SyntaxToken ParseComparisonOperator(
            UvssLexerStream input, ref Int32 position, Boolean accept)
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
            UvssLexerStream input, ref Int32 position)
        {
            return ParseComparisonOperator(input, ref position, true);
        }

        /// <summary>
        /// Expects a comparison operator and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectComparisonOperator(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseComparisonOperator(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax MissingPropertyTriggerCondition(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var propertyName = accept ?
                AcceptPropertyName(input, ref position) :
                ExpectPropertyName(input, ref position);

            if (accept && propertyName == null)
                return null;

            var comparisonOperatorToken =
                ExpectComparisonOperator(input, ref position);

            var propertyValue =
                ExpectPropertyValueWithBraces(input, ref position);

            var condition = WithPosition(new UvssPropertyTriggerConditionSyntax(
                propertyName,
                comparisonOperatorToken,
                propertyValue));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (comparisonOperatorToken.IsMissing)
                DiagnosticInfo.ReportPropertyTriggerMissingComparisonOperator(ref diagnostics, comparisonOperatorToken);
            
            condition.SetDiagnostics(diagnostics);

            return condition;
        }

        /// <summary>
        /// Accepts a property trigger condition.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax AcceptPropertyTriggerCondition(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyTriggerCondition(input, ref position, true);
        }

        /// <summary>
        /// Expects a property trigger condition and produces a missing node if one is not found.
        /// </summary>
        private static UvssPropertyTriggerConditionSyntax ExpectPropertyTriggerCondition(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyTriggerCondition(input, ref position, false);
        }

        /// <summary>
        /// Accepts a "play-storyboard" trigger action.
        /// </summary>
        private static UvssPlayStoryboardTriggerActionSyntax AcceptPlayStoryboardTriggerAction(
            UvssLexerStream input, ref Int32 position)
        {
            var playStoryboardKeyword =
                ExpectToken(input, ref position, SyntaxKind.PlayStoryboardKeyword);

            var selector =
                AcceptSelectorWithParentheses(input, ref position);

            var value =
                ExpectPropertyValueWithBraces(input, ref position);

            var action = new UvssPlayStoryboardTriggerActionSyntax(
                playStoryboardKeyword,
                selector,
                value);

            input.Trim(position);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (playStoryboardKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, playStoryboardKeyword);
                        
            action.SetDiagnostics(diagnostics);

            return action;
        }

        /// <summary>
        /// Accepts a "play-sfx" trigger action.
        /// </summary>
        private static UvssPlaySfxTriggerActionSyntax AcceptPlaySfxTriggerAction(
            UvssLexerStream input, ref Int32 position)
        {
            var playSfxKeyword =
                ExpectToken(input, ref position, SyntaxKind.PlaySfxKeyword);

            var value =
                ExpectPropertyValueWithBraces(input, ref position);

            var action = new UvssPlaySfxTriggerActionSyntax(
                playSfxKeyword,
                value);

            input.Trim(position);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (playSfxKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, playSfxKeyword);
            
            action.SetDiagnostics(diagnostics);

            return action;
        }

        /// <summary>
        /// Accepts a "set" trigger action.
        /// </summary>
        private static UvssSetTriggerActionSyntax AcceptSetTriggerAction(
            UvssLexerStream input, ref Int32 position)
        {
            var setKeyword =
                ExpectToken(input, ref position, SyntaxKind.SetKeyword);

            var propertyName =
                ExpectPropertyName(input, ref position);

            var selector =
                AcceptSelectorWithParentheses(input, ref position);

            var value =
                ExpectPropertyValueWithBraces(input, ref position);

            var action = new UvssSetTriggerActionSyntax(
                setKeyword,
                propertyName,
                selector,
                value);

            input.Trim(position);

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (setKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, setKeyword);
            
            action.SetDiagnostics(diagnostics);

            return action;
        }

        /// <summary>
        /// Accepts a trigger action.
        /// </summary>
        private static UvssTriggerActionBaseSyntax AcceptTriggerAction(
            UvssLexerStream input, ref Int32 position)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind != SyntaxKind.PlayStoryboardKeyword &&
                nextKind != SyntaxKind.PlaySfxKeyword &&
                nextKind != SyntaxKind.SetKeyword)
            {
                return null;
            }

            switch (nextKind)
            {
                case SyntaxKind.PlayStoryboardKeyword:
                    return AcceptPlayStoryboardTriggerAction(input, ref position);

                case SyntaxKind.PlaySfxKeyword:
                    return AcceptPlaySfxTriggerAction(input, ref position);

                case SyntaxKind.SetKeyword:
                    return AcceptSetTriggerAction(input, ref position);

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Accepts nodes which are valid inside of the body of a trigger.
        /// </summary>
        private static SyntaxNode AcceptTriggerBodyNode(
            UvssLexerStream input, ref Int32 position)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var action = AcceptTriggerAction(input, ref position);
            if (action != null)
                return action;

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.SetKeyword ||
                    kind == SyntaxKind.PlaySfxKeyword ||
                    kind == SyntaxKind.PlayStoryboardKeyword ||
                    kind == SyntaxKind.CloseCurlyBraceToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInTriggerBody);
            return empty;
        }

        /// <summary>
        /// Parses a proeprty trigger.
        /// </summary>
        private static UvssPropertyTriggerSyntax ParsePropertyTrigger(
            UvssLexerStream input, ref Int32 position, Boolean accept)
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
                AcceptSeparatedList(input, ref position, ExpectPropertyTriggerCondition);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var body =
                ExpectBlock(input, ref position, AcceptTriggerBodyNode);

            input.Trim(position);

            var trigger = WithPosition(new UvssPropertyTriggerSyntax(
                triggerKeyword,
                propertyKeyword,
                conditions,
                qualifierToken,
                body));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            trigger.SetDiagnostics(diagnostics);

            if (triggerKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, triggerKeyword);

            if (propertyKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, propertyKeyword);

            trigger.SetDiagnostics(diagnostics);

            return trigger;
        }

        /// <summary>
        /// Accepts a property trigger.
        /// </summary>
        private static UvssPropertyTriggerSyntax AcceptPropertyTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyTrigger(input, ref position, true);
        }

        /// <summary>
        /// Expects a property trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssPropertyTriggerSyntax ExpectPropertyTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParsePropertyTrigger(input, ref position, false) ??
                new UvssPropertyTriggerSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Parses an event trigger.
        /// </summary>
        private static UvssEventTriggerSyntax ParseEventTrigger(
            UvssLexerStream input, ref Int32 position, Boolean accept)
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
                ExpectBlock(input, ref position, AcceptTriggerBodyNode);

            input.Trim(position);

            var trigger = WithPosition(new UvssEventTriggerSyntax(
                triggerKeyword,
                eventKeyword,
                eventName,
                argumentList,
                qualifierToken,
                body));
            
            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (triggerKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, triggerKeyword);

            if (eventKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, eventKeyword);

            if (argumentList != null && !argumentList.IsMissing)
            {
                if (argumentList.Arguments.Count == 0)
                    DiagnosticInfo.ReportEventTriggerHasTooFewArguments(ref diagnostics, argumentList.Arguments.Node ?? argumentList);
                
                var duplicateArguments = argumentList.ArgumentTokens.GroupBy(x => x.Text)
                    .Select(x => new { Token = x.Key, Count = x.Count() }).Where(x => x.Count > 1);
                if (duplicateArguments.Any())
                {
                    foreach (var duplicateArgument in duplicateArguments)
                    {
                        DiagnosticInfo.ReportEventTriggerHasDuplicateArguments(ref diagnostics, 
                            argumentList.Arguments.Node ?? argumentList, duplicateArgument.Token);
                    }
                }
            }

            trigger.SetDiagnostics(diagnostics);

            return trigger;
        }

        /// <summary>
        /// Accepts an event trigger.
        /// </summary>
        private static UvssEventTriggerSyntax AcceptEventTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventTrigger(input, ref position, true);
        }

        /// <summary>
        /// Expects an event trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssEventTriggerSyntax ExpectEventTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseEventTrigger(input, ref position, false) ??
                new UvssEventTriggerSyntax() { IsMissing = true };
        }

        /// <summary>
        /// Parses an incomplete trigger.
        /// </summary>
        private static UvssIncompleteTriggerSyntax ParseIncompleteTrigger(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TriggerKeyword)
                return null;

            var triggerKeyword =
                ExpectToken(input, ref position, SyntaxKind.TriggerKeyword);

            var qualifierToken =
                AcceptToken(input, ref position, SyntaxKind.ImportantKeyword);

            var body =
                ExpectBlock(input, ref position, AcceptTriggerBodyNode);

            input.Trim(position);

            var trigger = WithPosition(new UvssIncompleteTriggerSyntax(
                triggerKeyword,
                qualifierToken,
                body));

            var diagnostics = default(ICollection<DiagnosticInfo>);
            DiagnosticInfo.ReportIncompleteTrigger(ref diagnostics, trigger);

            trigger.SetDiagnostics(diagnostics);

            return trigger;
        }

        /// <summary>
        /// Accepts an incomplete trigger.
        /// </summary>
        private static UvssIncompleteTriggerSyntax AcceptIncompleteTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseIncompleteTrigger(input, ref position, true);
        }

        /// <summary>
        /// Expects an incomplete trigger and produces a missing node if one does not exist.
        /// </summary>
        private static UvssIncompleteTriggerSyntax ExpectIncompleteTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseIncompleteTrigger(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax MissingTrigger(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            var propertyTrigger =
                AcceptPropertyTrigger(input, ref position);

            if (propertyTrigger != null)
                return propertyTrigger;

            var eventTrigger =
                AcceptEventTrigger(input, ref position);

            if (eventTrigger != null)
                return eventTrigger;

            return accept ? null : ExpectIncompleteTrigger(input, ref position);
        }

        /// <summary>
        /// Accepts a trigger.
        /// </summary>
        private static UvssTriggerBaseSyntax AcceptTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTrigger(input, ref position, true);
        }

        /// <summary>
        /// Expects a trigger and produces a missing node if one is not found.
        /// </summary>
        private static UvssTriggerBaseSyntax ExpectTrigger(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseTrigger(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax MissingStoryboard(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
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
            UvssLexerStream input, ref Int32 position)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var storyboardTarget = AcceptStoryboardTarget(input, ref position);
            if (storyboardTarget != null)
                return storyboardTarget;

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.TargetKeyword ||
                    kind == SyntaxKind.CloseCurlyBraceToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInStoryboardBody);
            return empty;
        }

        /// <summary>
        /// Parses a storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax ParseStoryboard(
            UvssLexerStream input, ref Int32 position, Boolean accept)
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

            var storyboard = WithPosition(new UvssStoryboardSyntax(
                atSignToken,
                nameIdentifier,
                loopIdentifier,
                body));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (atSignToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, atSignToken);

            if (nameIdentifier.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, nameIdentifier);

            if (loopIdentifier != null)
            {
                if (!KnownLoopBehaviors.IsKnownLoopBehavior(loopIdentifier.Text))
                    DiagnosticInfo.ReportUnrecognizedLoopType(ref diagnostics, loopIdentifier);
            }

            storyboard.SetDiagnostics(diagnostics);

            return storyboard;
        }

        /// <summary>
        /// Accepts a storyboard declaration.
        /// </summary>
        private static UvssStoryboardSyntax AcceptStoryboard(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseStoryboard(input, ref position, true);
        }

        /// <summary>
        /// Expects a storyboard declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardSyntax ExpectStoryboard(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseStoryboard(input, ref position, false);
        }

        /// <summary>
        /// Accepts any node which is valid in the body of a storyboard target.
        /// </summary>
        private static SyntaxNode AcceptStoryboardTargetBodyNode(
            UvssLexerStream input, ref Int32 position)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var animation = AcceptAnimation(input, ref position);
            if (animation != null)
                return animation;

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.AnimationKeyword ||
                    kind == SyntaxKind.CloseCurlyBraceToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInStoryboardTargetBody);
            return empty;
        }

        /// <summary>
        /// Produces a missing storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax MissingStoryboardTarget(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
        {
            return WithPosition(new UvssStoryboardTargetSyntax(
                MissingToken(SyntaxKind.TargetKeyword, input, position),
                default(SeparatedSyntaxList<UvssIdentifierBaseSyntax>),
                null,
                MissingBlock(input, position, children)));
        }

        /// <summary>
        /// Parses a storyboard target declaration.
        /// </summary>
        private static UvssStoryboardTargetSyntax ParseStoryboardTarget(
            UvssLexerStream input, ref Int32 position, Boolean accept)
        {
            if (accept && SyntaxKindFromNextToken(input, position) != SyntaxKind.TargetKeyword)
                return null;

            var targetKeyword =
                ExpectToken(input, ref position, SyntaxKind.TargetKeyword);

            var typeNameIdentifier =
                AcceptSeparatedList(input, ref position, ExpectIdentifier);

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
            UvssLexerStream input, ref Int32 position)
        {
            return ParseStoryboardTarget(input, ref position, true);
        }

        /// <summary>
        /// Expects a storyboard target declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssStoryboardTargetSyntax ExpectStoryboardTarget(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseStoryboardTarget(input, ref position, false);
        }

        /// <summary>
        /// Accepts any node which is valid in the body of an animation.
        /// </summary>
        private static SyntaxNode AcceptAnimationBodyNode(
            UvssLexerStream input, ref Int32 position)
        {
            var nextKind = SyntaxKindFromNextToken(input, position);
            if (nextKind == SyntaxKind.CloseCurlyBraceToken || nextKind == SyntaxKind.EndOfFileToken)
                return null;

            var animationKeyframe = AcceptAnimationKeyframe(input, ref position);
            if (animationKeyframe != null)
                return animationKeyframe;

            var empty = ExpectEmptyStatement(input, ref position, kind =>
            {
                return
                    kind == SyntaxKind.KeyframeKeyword ||
                    kind == SyntaxKind.CloseCurlyBraceToken ||
                    kind == SyntaxKind.EndOfFileToken;
            });
            AddDiagnosticsToSkippedSyntaxTrivia(empty, DiagnosticInfo.ReportUnexpectedTokenInAnimationBody);
            return empty;
        }

        /// <summary>
        /// Produces a missing animation declaration.
        /// </summary>
        private static UvssAnimationSyntax MissingAnimation(
            UvssLexerStream input, Int32 position, params SyntaxNode[] children)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
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
            
            return animation;
        }

        /// <summary>
        /// Accepts an animation declaration.
        /// </summary>
        private static UvssAnimationSyntax AcceptAnimation(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseAnimation(input, ref position, true);
        }

        /// <summary>
        /// Expects an animation declaration and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationSyntax ExpectAnimation(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseAnimation(input, ref position, false);
        }

        /// <summary>
        /// Produces a missing animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax MissingAnimationKeyframe(
            UvssLexerStream input, Int32 position)
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
            UvssLexerStream input, ref Int32 position, Boolean accept)
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

            input.Trim(position);

            var keyframe = WithPosition(new UvssAnimationKeyframeSyntax(
                keyframeKeyword,
                timeToken,
                easingIdentifier,
                value));

            var diagnostics = default(ICollection<DiagnosticInfo>);

            if (keyframeKeyword.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, keyframeKeyword);

            if (timeToken.IsMissing)
                DiagnosticInfo.ReportMissingNode(ref diagnostics, timeToken);

            if (easingIdentifier != null)
            {
                if (!KnownEasingFunctions.IsKnownEasingFunction(easingIdentifier.Text))
                    DiagnosticInfo.ReportUnrecognizedEasingFunction(ref diagnostics, easingIdentifier);
            }
            
            keyframe.SetDiagnostics(diagnostics);

            return keyframe;
        }

        /// <summary>
        /// Accepts an animation keyframe.
        /// </summary>
        private static UvssAnimationKeyframeSyntax AcceptAnimationKeyframe(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseAnimationKeyframe(input, ref position, true);
        }

        /// <summary>
        /// Expects an animation keyframe and produces a missing node if one is not found.
        /// </summary>
        private static UvssAnimationKeyframeSyntax ExpectAnimationKeyframe(
            UvssLexerStream input, ref Int32 position)
        {
            return ParseAnimationKeyframe(input, ref position, false);
        }

        /// <summary>
        /// Parses an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax ParseEmptyStatement(
            UvssLexerStream input, ref Int32 position, Predicate<SyntaxKind> isNotSkipped, Boolean accept)
        {
            var trivia = AccumulateTrivia(input, ref position,
                isCurrentTokenTrivia: true,
                isLeading: true,
                isNotSkippedTriviaKind: isNotSkipped);

            var emptyToken = new SyntaxToken(SyntaxKind.EmptyToken, null);
            GetNodePositionFromLexerPosition(input, position, emptyToken);

            var emptyStatement = WithPosition(new UvssEmptyStatementSyntax(emptyToken)
                .WithLeadingTrivia(trivia));

            return WithPosition(emptyStatement);
        }

        /// <summary>
        /// Accepts an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax AcceptEmptyStatement(
            UvssLexerStream input, ref Int32 position, Predicate<SyntaxKind> checkAllowedToken)
        {
            return null;
        }

        /// <summary>
        /// Expects an empty statement.
        /// </summary>
        private static UvssEmptyStatementSyntax ExpectEmptyStatement(
            UvssLexerStream input, ref Int32 position, Predicate<SyntaxKind> checkAllowedToken)
        {
            return ParseEmptyStatement(input, ref position, checkAllowedToken, false);
        }

        /// <summary>
        /// Accepts a comma token.
        /// </summary>
        private static SyntaxToken AcceptComma(
            UvssLexerStream input, ref Int32 position)
        {
            return AcceptToken(input, ref position, SyntaxKind.CommaToken);
        }

        /// <summary>
        /// Expects a comma token and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectComma(
            UvssLexerStream input, ref Int32 position)
        {
            return ExpectToken(input, ref position, SyntaxKind.CommaToken);
        }

        /// <summary>
        /// Produces a missing token of the specified kind.
        /// </summary>
        private static SyntaxToken MissingToken(SyntaxKind kind,
            UvssLexerStream input, Int32 position)
        {
            var token = new SyntaxToken(kind, null) { IsMissing = true };
            GetNodePositionFromLexerPosition(input, position, token);
            return token;
        }

        /// <summary>
        /// Accepts a syntax token of the specified kind.
        /// </summary>
        private static SyntaxToken AcceptToken(
            UvssLexerStream input, ref Int32 position, SyntaxKind acceptedKind)
        {
            var foundKind = SyntaxKindFromNextToken(input, position);
            if (foundKind == acceptedKind)
            {
                return GetNextToken(input, ref position);
            }
            return null;
        }

        /// <summary>
        /// Accepts a syntax token of any of the specified kinds.
        /// </summary>
        private static SyntaxToken AcceptToken(
            UvssLexerStream input, ref Int32 position, params SyntaxKind[] acceptedKinds)
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
            UvssLexerStream input, ref Int32 position, SyntaxKind? expectedKind)
        {
            var token = GetNextToken(input, ref position);
            if (expectedKind != null && token.Kind != expectedKind)
            {
                RestoreToken(input, ref position, token);
                var missingToken = new SyntaxToken(expectedKind ?? SyntaxKind.None, null) { IsMissing = true };
                GetNodePositionFromLexerPosition(input, position, missingToken);
                return missingToken;
            }
            return token;
        }

        /// <summary>
        /// Accepts a syntax token of the specified kind on the same line as previous nodes.
        /// </summary>
        private static SyntaxToken AcceptTokenOnSameLine(
            UvssLexerStream input, ref Int32 position, SyntaxKind acceptedKind, ref Boolean eol)
        {
            if (eol)
            {
                return null;
            }
            else
            {
                var token = AcceptToken(input, ref position, acceptedKind);
                if (token != null && token.HasTrailingLineBreaks)
                    eol = true;

                return token;
            }
        }

        /// <summary>
        /// Expects a syntax token of the specified kind on the same line as previous nodes
        /// and produces a missing node if one is not found.
        /// </summary>
        private static SyntaxToken ExpectTokenOnSameLine(
            UvssLexerStream input, ref Int32 position, SyntaxKind expectedKind, ref Boolean eol)
        {
            if (eol)
            {
                return MissingToken(expectedKind, input, position);
            }
            else
            {
                var token = ExpectToken(input, ref position, expectedKind);
                if (token.HasTrailingLineBreaks)
                    eol = true;

                return token;
            }            
        }
        
        /// <summary>
        /// Restores the specified token to the input stream and modifies the current stream
        /// position accordingly.
        /// </summary>
        /// <param name="input">The lexer token stream.</param>
        /// <param name="position">The current position in the lexer token stream.</param>
        /// <param name="token">The token to restore to the stream.</param>
        private static void RestoreToken(
            UvssLexerStream input, ref Int32 position, SyntaxToken token)
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
        /// Adds diagnostics to skipped tokens trivia which are contained by the specified node's descendants.
        /// </summary>
        private static void AddDiagnosticsToSkippedSyntaxTrivia(SyntaxNode node, SkippedTokensDiagnosticsReporter reporter)
        {
            if (node == null)
                return;

            var trivia = node as SkippedTokensTriviaSyntax;
            if (trivia != null)
            {
                var diagnostics = default(ICollection<DiagnosticInfo>);
                reporter?.Invoke(ref diagnostics, trivia);
                trivia.SetDiagnostics(diagnostics);
            }
            else
            {
                var leading = node.GetLeadingTrivia();
                if (leading != null)
                {
                    if (leading.IsList)
                    {
                        for (int i = 0; i < leading.SlotCount; i++)
                        {
                            var child = leading.GetSlot(i) as SkippedTokensTriviaSyntax;
                            if (child != null)
                            {
                                AddDiagnosticsToSkippedSyntaxTrivia(child, reporter);
                                break;
                            }
                        }
                    }
                    else { AddDiagnosticsToSkippedSyntaxTrivia(leading, reporter); }
                }

                var trailing = node.GetTrailingTrivia();
                if (trailing != null)
                {
                    if (trailing != null)
                    {
                        for (int i = 0; i < trailing.SlotCount; i++)
                        {
                            var child = trailing.GetSlot(i) as SkippedTokensTriviaSyntax;
                            if (child != null)
                            {
                                AddDiagnosticsToSkippedSyntaxTrivia(child, reporter);
                                break;
                            }
                        }
                    }
                    else { AddDiagnosticsToSkippedSyntaxTrivia(trailing, reporter); }
                }
            }
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
        /// <param name="isCurrentTokenTrivia">A value indicating whether to treat
        /// the current token as trivia, even if it otherwise wouldn't be considered trivia.</param>
        /// <param name="isLeading">A value indicating whether this is leading trivia.</param>
        /// <param name="isNotSkippedTriviaKind">A predicate which determines whether a given token
        /// should be considered skipped trivia.</param>
        /// <returns>The list of accumulated trivia, or null if no trivia was accumulated.</returns>
        private static IList<SyntaxTrivia> AccumulateTrivia(
            UvssLexerStream input, ref Int32 position, 
            Boolean isCurrentTokenTrivia = false,
            Boolean isLeading = false,
            Predicate<SyntaxKind> isNotSkippedTriviaKind = null)
        {
            var triviaList = default(List<SyntaxTrivia>);

            var treatNextTokenAsSkipped = false;
            var treatNextNonTriviaTokenAsTrivia = isCurrentTokenTrivia;

            while (!input.IsPastEndOfStream(position))
            {
                var token = input[position];
                if (!IsTrivia(token))
                {
                    if (!treatNextNonTriviaTokenAsTrivia)
                        break;

                    var tokenKind = SyntaxKindFromLexerTokenType(token);
                    if (isNotSkippedTriviaKind != null && isNotSkippedTriviaKind(tokenKind))
                        break;

                    treatNextTokenAsSkipped = true;
                }
                else
                {
                    treatNextNonTriviaTokenAsTrivia = false;
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

        // The list of cultures installed on this machine.
        private static readonly IEnumerable<String> recognizedCultures;

        // Regular expression which matches valid identifiers.
        private static readonly Regex regexValidIdentifier = new Regex(@"\G[_\-a-zA-Z][_\-a-zA-Z0-9]*",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);
    }
}
