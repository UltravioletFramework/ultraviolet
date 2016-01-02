using System;
using System.Collections.Generic;
using System.Globalization;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Contains factory methods for constructing UVSS syntax nodes.
    /// </summary>
    public static class SyntaxFactory
    {
        /// <summary>
        /// Creates white space trivia with the specified text.
        /// </summary>
        /// <param name="text">The white space's text.</param>
        /// <returns>The <see cref="SyntaxTrivia"/> instance that was created.</returns>
        public static SyntaxTrivia Whitespace(String text)
        {
            return new SyntaxTrivia(SyntaxKind.WhitespaceTrivia, text);
        }

        /// <summary>
        /// Creates a comment trivia with the specified text.
        /// </summary>
        /// <param name="text">The comment's text.</param>
        /// <returns>The <see cref="SyntaxTrivia"/> instance that was created.</returns>
        public static SyntaxTrivia Comment(String text)
        {
            var kind = (text?.StartsWith("/*") ?? false) ? 
                SyntaxKind.MultiLineCommentTrivia : 
                SyntaxKind.SingleLineCommentTrivia;

            return new SyntaxTrivia(kind, text);
        }

        /// <summary>
        /// Creates a new keyword token.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        /// <returns>The <see cref="UvssKeyword"/> instance that was created.</returns>
        public static UvssKeyword Keyword(SyntaxKind kind)
        {
            return new UvssKeyword(kind);
        }

        /// <summary>
        /// Creates a new keyword token.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        /// <returns>The <see cref="UvssKeyword"/> instance that was created.</returns>
        public static UvssKeyword Keyword(SyntaxKind kind,
            SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            return new UvssKeyword(kind, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new punctuation token.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the punctuation's kind.</param>
        /// <returns>The <see cref="UvssPunctuation"/> instance that was created.</returns>
        public static UvssPunctuation Punctuation(SyntaxKind kind)
        {
            return new UvssPunctuation(kind);
        }

        /// <summary>
        /// Creates a new punctuation token.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the punctuation's kind.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        /// <returns>The <see cref="UvssPunctuation"/> instance that was created.</returns>
        public static UvssPunctuation Punctuation(SyntaxKind kind,
            SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            return new UvssPunctuation(kind, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new identifier token.
        /// </summary>
        /// <param name="text">The identifier text.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Identifier(String text)
        {
            return Identifier(text, null, null);
        }

        /// <summary>
        /// Creates a new identifier token.
        /// </summary>
        /// <param name="text">The identifier text.</param>
        /// <param name="leadingTrivia">The syntax token's leading trivia, if it has any.</param>
        /// <param name="trailingTrivia">The syntax token's trailing trivia, if it has any.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Identifier(String text,
            SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            return new SyntaxToken(SyntaxKind.IdentifierToken, text, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new number token.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Number(Int32 value)
        {
            return Number(value, null, null);
        }

        /// <summary>
        /// Creates a new number token.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        /// <param name="leadingTrivia">The syntax token's leading trivia, if it has any.</param>
        /// <param name="trailingTrivia">The syntax token's trailing trivia, if it has any.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Number(Int32 value,
            SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            return new SyntaxToken(SyntaxKind.NumberToken, 
                value.ToString(CultureInfo.InvariantCulture), leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new terminal token.
        /// </summary>
        /// <param name="kind">The token's kind.</param>
        /// <param name="text">The token's text.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Token(SyntaxKind kind, String text)
        {
            return new SyntaxToken(kind, text, null, null);
        }

        /// <summary>
        /// Creates a new terminal token.
        /// </summary>
        /// <param name="kind">The token's kind.</param>
        /// <param name="text">The token's text.</param>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Token(SyntaxKind kind, String text,
            SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
        {
            return new SyntaxToken(kind, text, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new end-of-file token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken EndOfFile()
        {
            return new SyntaxToken(SyntaxKind.EndOfFileToken, null, null, null);
        }

        /// <summary>
        /// Creates an empty list.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <returns>The <see cref="SyntaxList{TNode}"/> that was created.</returns>
        public static SyntaxList<TNode> List<TNode>() where TNode : SyntaxList
        {
            return default(SyntaxList<TNode>);
        }

        /// <summary>
        /// Creates a list of nodes from a sequence of nodes.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <param name="nodes">The collection of nodes with which to populate the list.</param>
        /// <returns>The <see cref="SyntaxList{TNode}"/> that was created.</returns>
        public static SyntaxList<TNode> List<TNode>(IEnumerable<TNode> nodes) where TNode : SyntaxNode
        {
            if (nodes != null)
            {
                var collection = nodes as ICollection<TNode>;
                var builder = (collection != null) ?
                    new SyntaxListBuilder<TNode>(collection.Count) : SyntaxListBuilder<TNode>.Create();

                foreach (var node in nodes)
                    builder.Add(node);

                return builder.ToList();
            }
            return default(SyntaxList<TNode>);
        }

        /// <summary>
        /// Creates an empty separated list.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <returns>The <see cref="SeparatedSyntaxList{TNode}"/> that was created.</returns>
        public static SeparatedSyntaxList<TNode> SeparatedList<TNode>() where TNode : SyntaxNode
        {
            return default(SeparatedSyntaxList<TNode>);
        }

        /// <summary>
        /// Creates a separated list of nodes from a sequence of nodes, adding comma separators between them.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <param name="nodes">The collection of nodes with which to populate the list.</param>
        /// <returns>The <see cref="SeparatedSyntaxList{TNode}"/> that was created.</returns>
        public static SeparatedSyntaxList<TNode> SeparatedList<TNode>(
            IEnumerable<TNode> nodes) where TNode : SyntaxNode
        {
            if (nodes == null)
                return default(SeparatedSyntaxList<TNode>);

            var collection = nodes as ICollection<TNode>;
            if (collection != null && collection.Count == 0)
                return default(SeparatedSyntaxList<TNode>);

            using (var enumerator = nodes.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return default(SeparatedSyntaxList<TNode>);

                var firstNode = enumerator.Current;
                if (!enumerator.MoveNext())
                    return new SeparatedSyntaxList<TNode>(List(new[] { firstNode }));

                var builder = new SeparatedSyntaxListBuilder<TNode>(collection?.Count ?? 3);
                builder.Add(firstNode);

                do
                {
                    builder.AddSeparator(Token(SyntaxKind.CommaToken, ","));
                    builder.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());

                return builder.ToList();
            }
        }

        /// <summary>
        /// Creates a separated list of nodes from a sequence of nodes and a sequence of separator tokens.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <param name="nodes">The collection of nodes with which to populate the list.</param>
        /// <param name="separators">The collection of separators with which to populate the list.</param>
        /// <returns>The <see cref="SeparatedSyntaxList{TNode}"/> that was created.</returns>
        public static SeparatedSyntaxList<TNode> SeparatedList<TNode>(
            IEnumerable<TNode> nodes, IEnumerable<SyntaxToken> separators) where TNode : SyntaxNode
        {
            if (nodes != null)
            {
                var builder = SeparatedSyntaxListBuilder<TNode>.Create();
                using (var enumerator = nodes.GetEnumerator())
                {
                    if (separators != null)
                    {
                        foreach (var token in separators)
                        {
                            if (!enumerator.MoveNext())
                                throw new ArgumentException();

                            builder.Add(enumerator.Current);
                            builder.AddSeparator(token);
                        }
                    }

                    if (enumerator.MoveNext())
                    {
                        builder.Add(enumerator.Current);
                        if (enumerator.MoveNext())
                            throw new ArgumentException();
                    }
                }
                return builder.ToList();
            }

            if (separators != null)
                throw new ArgumentException();

            return default(SeparatedSyntaxList<TNode>);
        }

        /// <summary>
        /// Creates a new UVSS document root node using an automatically created end-of-file token.
        /// </summary>
        /// <param name="ruleSetAndStoryboardList">The document's list of rule sets and storyboards.</param>
        /// <returns>The <see cref="UvssDocumentSyntax"/> instance that was created.</returns>
        public static UvssDocumentSyntax Document(
            SyntaxList<SyntaxNode> ruleSetAndStoryboardList)
        {
            return Document(ruleSetAndStoryboardList, EndOfFile());
        }

        /// <summary>
        /// Creates a new UVSS document root node.
        /// </summary>
        /// <param name="ruleSetAndStoryboardList">The document's list of rule sets and storyboards.</param>
        /// <param name="endOfFileToken">The document's end-of-file token.</param>
        /// <returns>The <see cref="UvssDocumentSyntax"/> instance that was created.</returns>
        public static UvssDocumentSyntax Document(
            SyntaxList<SyntaxNode> ruleSetAndStoryboardList,
            SyntaxToken endOfFileToken)
        {
            return new UvssDocumentSyntax(ruleSetAndStoryboardList, endOfFileToken);
        }

        /// <summary>
        /// Creates a new UVSS rule set node.
        /// </summary>
        /// <param name="selectorList">The rule set's list of selectors.</param>
        /// <param name="body">The rule set's body.</param>
        /// <returns>The <see cref="UvssRuleSetSyntax"/> instance that was created.</returns>
        public static UvssRuleSetSyntax RuleSet(
            SeparatedSyntaxList<UvssSelectorSyntax> selectorList, UvssBlockSyntax body)
        {
            return new UvssRuleSetSyntax(selectorList, body);
        }
        
        /// <summary>
        /// Creates a new parentheses-enclosed selector with automatically created parenthesis tokens.
        /// </summary>
        /// <param name="selector">The enclosed selector.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParentheses(UvssSelectorSyntax selector)
        {
            return SelectorWithParentheses(
                Punctuation(SyntaxKind.OpenParenthesesToken),
                selector,
                Punctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector.
        /// </summary>
        /// <param name="openParenToken">The open parenthesis that introduces the selector.</param>
        /// <param name="selector">The enclosed selector.</param>
        /// <param name="closeParenToken">The close parenthesis that terminates the selector.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParentheses(
            SyntaxToken openParenToken,
            UvssSelectorSyntax selector,
            SyntaxToken closeParenToken)
        {
            return new UvssSelectorWithParenthesesSyntax(openParenToken, selector, closeParenToken);
        }

        /// <summary>
        /// Creates a new universal selector.
        /// </summary>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax UniversalSelector()
        {
            return Selector(List(new[] { UniversalSelectorPart() }));
        }

        /// <summary>
        /// Creates a new selector consisting of a single selector part that selects for a named element.
        /// </summary>
        /// <param name="name">The name of the element being selected.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByName(String name)
        {
            return Selector(List(new[] { SelectorPartByName(name) }));
        }

        /// <summary>
        /// Creates a new selector consisting of a single selector part that selects for a class.
        /// </summary>
        /// <param name="class">The name of the class being selected.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByClass(String @class)
        {
            return Selector(List(new[] { SelectorPartByClass(@class) }));
        }

        /// <summary>
        /// Creates a new selector consisting of a single selector part that selects for a type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByType(String type)
        {
            return Selector(List(new[] { SelectorPartByType(type) }));
        }

        /// <summary>
        /// Creates a new selector consisting of a single selector part that selects for a specific type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorBySpecificType(String type)
        {
            return Selector(List(new[] { SelectorPartBySpecificType(type) }));
        }

        /// <summary>
        /// Creates a new selector from the specified list of selector parts and combinators.
        /// </summary>
        /// <param name="partsAndCombinatorsList">The list of parts and combinators that make up the selector.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> that was created.</returns>
        public static UvssSelectorSyntax Selector(SyntaxList<SyntaxNode> partsAndCombinatorsList)
        {
            return new UvssSelectorSyntax(partsAndCombinatorsList);
        }

        /// <summary>
        /// Creates a new universal selector part.
        /// </summary>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax UniversalSelectorPart(String pseudoClass = null)
        {
            return SelectorPart(
                List(new[] { UniversalSelectorSubPart() }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a named element.
        /// </summary>
        /// <param name="name">The name of the element being selected.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByName(String name, String pseudoClass = null)
        {
            return SelectorPart(
                List(new[] { SelectorSubPartByName(name) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a class.
        /// </summary>
        /// <param name="class">The name of the class being selected.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByClass(String @class, String pseudoClass = null)
        {
            return SelectorPart(
                List(new[] { SelectorSubPartByClass(@class) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByType(String type, String pseudoClass = null)
        {
            return SelectorPart(
                List(new[] { SelectorSubPartByType(type) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a specific type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartBySpecificType(String type, String pseudoClass = null)
        {
            return SelectorPart(
                List(new[] { SelectorSubPartBySpecificType(type) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part from the specified list of sub-parts.
        /// </summary>
        /// <param name="subPartsList">The list of sub-parts that make up the selector part.</param>
        /// <param name="pseudoClass">The selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPart(
            SyntaxList<UvssSelectorSubPartSyntax> subPartsList,
            UvssPseudoClassSyntax pseudoClass)
        {
            return new UvssSelectorPartSyntax(subPartsList, pseudoClass);
        }

        /// <summary>
        /// Creates a new universal selector sub-part.
        /// </summary>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax UniversalSelectorSubPart()
        {
            return SelectorSubPart(
                null,
                Punctuation(SyntaxKind.AsteriskToken),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a named element.
        /// </summary>
        /// <param name="name">The name of the element being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByName(String name)
        {
            return SelectorSubPart(
                Punctuation(SyntaxKind.HashToken), 
                Identifier(name), 
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a class.
        /// </summary>
        /// <param name="class">The name of the class being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByClass(String @class)
        {
            return SelectorSubPart(
                Punctuation(SyntaxKind.PeriodToken),
                Identifier(@class),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByType(String type)
        {
            return SelectorSubPart(
                null,
                Identifier(type),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a specific type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartBySpecificType(String type)
        {
            return SelectorSubPart(
                null,
                Identifier(type),
                Punctuation(SyntaxKind.ExclamationMarkToken));
        }

        /// <summary>
        /// Creates a new selector sub-part.
        /// </summary>
        /// <param name="leadingQualifierToken">The sub-part's leading qualifier token.</param>
        /// <param name="textToken">The sub-part's text token.</param>
        /// <param name="trailingQualifierToken">The sub-part's trailing qualifier token.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPart(
            SyntaxToken leadingQualifierToken,
            SyntaxToken textToken,
            SyntaxToken trailingQualifierToken)
        {
            return new UvssSelectorSubPartSyntax(leadingQualifierToken, textToken, trailingQualifierToken);
        }

        /// <summary>
        /// Creates a new pseudo-class specifier using an automatically-generated colon token and an identifier
        /// token with the specified class name value.
        /// </summary>
        /// <param name="className">The pseudo-class' name.</param>
        /// <returns>The <see cref="UvssPseudoClassSyntax"/> instance that was created.</returns>
        public static UvssPseudoClassSyntax PseudoClass(String className)
        {
            return PseudoClass(
                Punctuation(SyntaxKind.ColonToken),
                Identifier(className)
            );
        }

        /// <summary>
        /// Creates a new pseudo-class specifier.
        /// </summary>
        /// <param name="colonToken">The colon token that precedes the class name.</param>
        /// <param name="classNameToken">The identifier token that contains the class name.</param>
        /// <returns></returns>
        public static UvssPseudoClassSyntax PseudoClass(
            SyntaxToken colonToken,
            SyntaxToken classNameToken)
        {
            return new UvssPseudoClassSyntax(colonToken, classNameToken);
        }

        /// <summary>
        /// Creates a new visual descendant combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static UvssPunctuation VisualDescendantCombinator()
        {
            return Punctuation(SyntaxKind.SpaceToken);
        }

        /// <summary>
        /// Creates a new visual child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static UvssPunctuation VisualChildCombinator()
        {
            return Punctuation(SyntaxKind.GreaterThanToken);
        }

        /// <summary>
        /// Creates a new logical child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static UvssPunctuation LogicalChildCombinator()
        {
            return Punctuation(SyntaxKind.GreaterThanQuestionMarkToken);
        }

        /// <summary>
        /// Creates a new templated child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static UvssPunctuation TemplatedChildCombinator()
        {
            return Punctuation(SyntaxKind.GreaterThanGreaterThanToken);
        }

        /// <summary>
        /// Creates an empty block.
        /// </summary>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block()
        {
            return Block(default(SyntaxList<SyntaxNode>));
        }

        /// <summary>
        /// Creates a new block with automatically generated curly brace tokens.
        /// </summary>
        /// <param name="content">The block's content.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(SyntaxList<SyntaxNode> content)
        {
            return Block(
                Punctuation(SyntaxKind.OpenCurlyBraceToken),
                content,
                Punctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new block.
        /// </summary>
        /// <param name="openCurlyBraceToken">The open curly brace that introduces the block.</param>
        /// <param name="content">The block's content.</param>
        /// <param name="closeCurlyBraceToken">The close curly brace that terminates the block.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(
            SyntaxToken openCurlyBraceToken,
            SyntaxList<SyntaxNode> content,
            SyntaxToken closeCurlyBraceToken)
        {
            return new UvssBlockSyntax(openCurlyBraceToken, content, closeCurlyBraceToken);
        }

        /// <summary>
        /// Creates a new property name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(String property)
        {
            return PropertyName(
                null,
                null,
                Identifier(property));
        }

        /// <summary>
        /// Creates a new property name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="owner">The owner of the attached property.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(String owner, String property)
        {
            return PropertyName(
                Identifier(owner),
                Punctuation(SyntaxKind.PeriodToken),
                Identifier(property));
        }

        /// <summary>
        /// Creates a new property name.
        /// </summary>
        /// <param name="attachedPropertyOwnerNameToken">The name of the attached property's owner.</param>
        /// <param name="periodToken">The period that separates the owner name from the property name.</param>
        /// <param name="propertyNameToken">The property name.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(
            SyntaxToken attachedPropertyOwnerNameToken,
            SyntaxToken periodToken,
            SyntaxToken propertyNameToken)
        {
            return new UvssPropertyNameSyntax(attachedPropertyOwnerNameToken, periodToken, propertyNameToken);
        }

        /// <summary>
        /// Creates a new event name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="event">The event name.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(String @event)
        {
            return EventName(
                null,
                null,
                Identifier(@event));
        }

        /// <summary>
        /// Creates a new event name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="owner">The owner of the attached event.</param>
        /// <param name="event">The event name.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(String owner, String @event)
        {
            return EventName(
                Identifier(owner),
                Punctuation(SyntaxKind.PeriodToken),
                Identifier(@event));
        }

        /// <summary>
        /// Creates a new event name.
        /// </summary>
        /// <param name="attachedEventOwnerNameToken">The name of the attached event's owner.</param>
        /// <param name="periodToken">The period that separates the owner name from the event name.</param>
        /// <param name="eventNameToken">The event name.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(
            SyntaxToken attachedEventOwnerNameToken,
            SyntaxToken periodToken,
            SyntaxToken eventNameToken)
        {
            return new UvssEventNameSyntax(attachedEventOwnerNameToken, periodToken, eventNameToken);
        }

        /// <summary>
        /// Creates a new property value using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueSyntax PropertyValue(String value)
        {
            return PropertyValue(
                Token(SyntaxKind.PropertyValueToken, value));
        }

        /// <summary>
        /// Creates a new property value.
        /// </summary>
        /// <param name="contentToken">The value's content token.</param>
        /// <returns>The <see cref="UvssPropertyValueSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueSyntax PropertyValue(SyntaxToken contentToken)
        {
            return new UvssPropertyValueSyntax(contentToken);
        }

        /// <summary>
        /// Creates a new brace-enclosed property value using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(String value)
        {
            return PropertyValueWithBraces(
                Punctuation(SyntaxKind.OpenCurlyBraceToken),
                Token(SyntaxKind.PropertyValueToken, value),
                Punctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new brace-enclosed property value using automatically created curly brace tokens.
        /// </summary>
        /// <param name="contentToken">The value's content token.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(SyntaxToken contentToken)
        {
            return PropertyValueWithBraces(
                Punctuation(SyntaxKind.OpenCurlyBraceToken),
                contentToken,
                Punctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new brace-enclosed property value.
        /// </summary>
        /// <param name="openCurlyBraceToken">The open curly brace token which introduces the value.</param>
        /// <param name="contentToken">The value's content token.</param>
        /// <param name="closeCurlyBraceToken">The close curly brace token which terminates the value.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(
            SyntaxToken openCurlyBraceToken,
            SyntaxToken contentToken,
            SyntaxToken closeCurlyBraceToken)
        {
            return new UvssPropertyValueWithBracesSyntax(openCurlyBraceToken, contentToken, closeCurlyBraceToken);
        }

        /// <summary>
        /// Creates a new styling rule using an automatically created colon token.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="value">The styled property value.</param>
        /// <param name="important">A value indicating whether this rule has the !important qualifier.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> instance that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueSyntax value,
            Boolean important = false)
        {
            return new UvssRuleSyntax(propertyName,
                Punctuation(SyntaxKind.ColonToken), value,
                important ? Keyword(SyntaxKind.ImportantKeyword) : null,
                Punctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new styling rule.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="colonToken">The colon token that separates the property name from its value.</param>
        /// <param name="value">The styled property value.</param>
        /// <param name="qualifierToken">The styling rule's qualifier token.</param>
        /// <param name="semiColonToken">The styling rule's terminating semi-colon token.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken colonToken,
            UvssPropertyValueSyntax value,
            SyntaxToken qualifierToken,
            SyntaxToken semiColonToken)
        {
            return new UvssRuleSyntax(propertyName, colonToken, value, qualifierToken, semiColonToken);
        }
        
        /// <summary>
        /// Creates a new property trigger using automatically created "trigger" and "property" keyword tokens.
        /// </summary>
        /// <param name="evaluationList">The property trigger's evaluations list.</param>
        /// <param name="body">The property trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> evaluationList,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return PropertyTrigger(
                Keyword(SyntaxKind.TriggerKeyword),
                Keyword(SyntaxKind.PropertyKeyword),
                evaluationList, important ? Keyword(SyntaxKind.ImportantKeyword) : null, body);
        }

        /// <summary>
        /// Creates a new property trigger.
        /// </summary>
        /// <param name="triggerKeyword">The property trigger's "trigger" keyword.</param>
        /// <param name="propertyKeyword">The property trigger's "property" keyword.</param>
        /// <param name="evaluationList">The property trigger's evaluations list.</param>
        /// <param name="qualifierToken">The property trigger's qualifier token.</param>
        /// <param name="body">The property trigger's body.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> evaluationList,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
        {
            return new UvssPropertyTriggerSyntax(
                triggerKeyword, propertyKeyword, evaluationList, qualifierToken, body);
        }

        /// <summary>
        /// Creates a new property trigger evaluation.
        /// </summary>
        /// <param name="propertyName">The name of the property being evaluated.</param>
        /// <param name="comparisonOperatorToken">The comparison operator used to perform the evaluation.</param>
        /// <param name="value">The value which is being compared to the property value.</param>
        /// <returns>The <see cref="UvssPropertyTriggerConditionSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerConditionSyntax PropertyTriggerEvaluation(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken comparisonOperatorToken,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPropertyTriggerConditionSyntax(propertyName, comparisonOperatorToken, value);
        }

        /// <summary>
        /// Creates a new equals comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation EqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.EqualsToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new not equals comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation NotEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.NotEqualsToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new less than comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation LessThanComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.LessThanToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new greater than comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation GreaterThanComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.GreaterThanToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new less than or equal to comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation LessThanEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.LessThanEqualsToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new greater than or equal to comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static UvssPunctuation GreaterThanEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return Punctuation(SyntaxKind.GreaterThanEqualsToken, leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new play-storyboard trigger action using an automatically created play-storyboard keyword token.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            UvssPropertyValueWithBracesSyntax value)
        {
            return PlayStoryboardTriggerAction(
                Keyword(SyntaxKind.PlayStoryboardKeyword), 
                null, 
                value);
        }

        /// <summary>
        /// Creates a new play-storyboard trigger action using an automatically created play-storyboard keyword token.
        /// </summary>
        /// <param name="selector">The trigger action's selector.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
        {
            return PlayStoryboardTriggerAction(
                Keyword(SyntaxKind.PlayStoryboardKeyword), 
                selector, 
                value);
        }

        /// <summary>
        /// Creates a new play-storyboard trigger action.
        /// </summary>
        /// <param name="playStoryboardKeyword">The trigger action's "play-storyboard" keyword.</param>
        /// <param name="selector">The trigger action's selector.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            SyntaxToken playStoryboardKeyword,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPlayStoryboardTriggerActionSyntax(playStoryboardKeyword, selector, value);
        }

        /// <summary>
        /// Creates a new play-sfx trigger action using an automatically created play-sfx keyword.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlaySfxTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlaySfxTriggerActionSyntax PlaySfxTriggerAction(
            UvssPropertyValueWithBracesSyntax value)
        {
            return PlaySfxTriggerAction(
                Keyword(SyntaxKind.PlaySfxKeyword),
                value);
        }

        /// <summary>
        /// Creates a new play-sfx trigger action.
        /// </summary>
        /// <param name="playSfxKeyword">The trigger action's "play-sfx" keyword.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlaySfxTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlaySfxTriggerActionSyntax PlaySfxTriggerAction(
            SyntaxToken playSfxKeyword,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPlaySfxTriggerActionSyntax(playSfxKeyword, value);
        }

        /// <summary>
        /// Creates a new set trigger action using an automatically created set keyword.
        /// </summary>
        /// <param name="propertyName">The name of the property which is set by the trigger.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssSetTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssSetTriggerActionSyntax SetTriggerAction(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueWithBracesSyntax value)
        {
            return SetTriggerAction(
                Keyword(SyntaxKind.SetKeyword), 
                propertyName, 
                null, 
                value);
        }

        /// <summary>
        /// Creates a new set trigger action using an automatically created set keyword.
        /// </summary>
        /// <param name="propertyName">The name of the property which is set by the trigger.</param>
        /// <param name="selector">The selector that specifies the trigger's target.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssSetTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssSetTriggerActionSyntax SetTriggerAction(
            UvssPropertyNameSyntax propertyName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
        {
            return SetTriggerAction(
                Keyword(SyntaxKind.SetKeyword),
                propertyName, 
                selector, 
                value);
        }

        /// <summary>
        /// Creates a new set trigger action.
        /// </summary>
        /// <param name="setKeyword">The trigger action's "set" keyword.</param>
        /// <param name="propertyName">The name of the property which is set by the trigger.</param>
        /// <param name="selector">The selector that specifies the trigger's target.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssSetTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssSetTriggerActionSyntax SetTriggerAction(
            SyntaxToken setKeyword,
            UvssPropertyNameSyntax propertyName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssSetTriggerActionSyntax(setKeyword, propertyName, selector, value);
        }

        /// <summary>
        /// Creates a new event trigger using automatically created "trigger" and "event" keywords.
        /// </summary>
        /// <param name="eventName">The event trigger's event name.</param>
        /// <param name="body">The event trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssEventTriggerSyntax"/> instance that was created.</returns>
        public static UvssEventTriggerSyntax EventTrigger(
            UvssEventNameSyntax eventName,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return EventTrigger(
                Keyword(SyntaxKind.TriggerKeyword),
                Keyword(SyntaxKind.EventKeyword),
                eventName, 
                null, 
                important ? Keyword(SyntaxKind.ImportantKeyword) : null,
                body);
        }

        /// <summary>
        /// Creates a new event trigger using automatically created "trigger" and "event" keywords.
        /// </summary>
        /// <param name="eventName">The event trigger's event name.</param>
        /// <param name="argumentList">The event trigger's argument list.</param>
        /// <param name="body">The event trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssEventTriggerSyntax"/> instance that was created.</returns>
        public static UvssEventTriggerSyntax EventTrigger(
            UvssEventNameSyntax eventName,
            UvssEventTriggerArgumentList argumentList,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return EventTrigger(
                Keyword(SyntaxKind.TriggerKeyword),
                Keyword(SyntaxKind.EventKeyword),
                eventName,
                argumentList,
                important ? Keyword(SyntaxKind.ImportantKeyword) : null,
                body);
        }

        /// <summary>
        /// Creates a new event trigger.
        /// </summary>
        /// <param name="triggerKeyword">The event trigger's "trigger" keyword.</param>
        /// <param name="eventKeyword">The event trigger's "event" keyword.</param>
        /// <param name="eventName">The event trigger's event name.</param>
        /// <param name="argumentList">The event trigger's argument list.</param>
        /// <param name="qualifierToken">The event trigger's qualifier token.</param>
        /// <param name="body">The event trigger's body.</param>
        /// <returns>The <see cref="UvssEventTriggerSyntax"/> instance that was created.</returns>
        public static UvssEventTriggerSyntax EventTrigger(
            SyntaxToken triggerKeyword,
            SyntaxToken eventKeyword,
            UvssEventNameSyntax eventName,
            UvssEventTriggerArgumentList argumentList,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
        {
            return new UvssEventTriggerSyntax(
                triggerKeyword, eventKeyword, eventName, argumentList, qualifierToken, body);
        }

        /// <summary>
        /// Creates a new event trigger argument list using automatically created tokens.
        /// </summary>
        /// <param name="handled">A value indicating whether the handled argument is present.</param>
        /// <param name="sethandled">A value indicating whether the set-handled argument is present.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            Boolean handled = false,
            Boolean sethandled = false)
        {
            var builder = SeparatedSyntaxListBuilder<SyntaxNode>.Create();

            if (handled)
                builder.Add(Token(SyntaxKind.HandledKeyword, "handled"));

            if (sethandled)
            {
                if (builder.Count > 0)
                {
                    builder.AddSeparator(Punctuation(SyntaxKind.CommaToken));
                }
                builder.Add(Token(SyntaxKind.SetHandledKeyword, "set-handled"));
            }

            return EventTriggerArgumentList(builder.ToList());
        }

        /// <summary>
        /// Creates a new event trigger argument list using automatically created parenthesis tokens.
        /// </summary>
        /// <param name="arguments">The argument list's list of arguments.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            SeparatedSyntaxList<SyntaxNode> arguments)
        {
            return new UvssEventTriggerArgumentList(
                Punctuation(SyntaxKind.OpenParenthesesToken),
                arguments,
                Punctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new event trigger argument list.
        /// </summary>
        /// <param name="openParenToken">The open parenthesis that introduces the argument list.</param>
        /// <param name="arguments">The argument list's list of arguments.</param>
        /// <param name="closeParenToken">The close parenthesis that terminates the argument list.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> arguments,
            SyntaxToken closeParenToken)
        {
            return new UvssEventTriggerArgumentList(openParenToken, arguments, closeParenToken);
        }

        /// <summary>
        /// Creates a new visual transition using automatically created tokens.
        /// </summary>
        /// <param name="argumentList">The transition's argument list.</param>
        /// <param name="storyboardName">The name of the storyboard associated with the transition.</param>
        /// <param name="important">A value indicating whether the transition has the !important qualifier.</param>
        /// <returns>The <see cref="UvssTransitionSyntax"/> instance that was created.</returns>
        public static UvssTransitionSyntax Transition(
            UvssTransitionArgumentListSyntax argumentList,
            String storyboardName,
            Boolean important = false)
        {
            return Transition(
                Keyword(SyntaxKind.TransitionKeyword),
                argumentList,
                Punctuation(SyntaxKind.ColonToken),
                Identifier(storyboardName),
                important ? Keyword(SyntaxKind.ImportantKeyword) : null,
                Punctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new visual transition.
        /// </summary>
        /// <param name="transitionKeyword">The "transition" keyword that introduces the transition.</param>
        /// <param name="argumentList">The transition's argument list.</param>
        /// <param name="colonToken">The colon that separates the transition declaration from its value.</param>
        /// <param name="storyboardNameToken">The name of the storyboard associated with the transition.</param>
        /// <param name="qualifierToken">The transition's qualifier token.</param>
        /// <param name="semiColonToken">The semi-colon that terminates the transition.</param>
        /// <returns></returns>
        public static UvssTransitionSyntax Transition(
            SyntaxToken transitionKeyword,
            UvssTransitionArgumentListSyntax argumentList,
            SyntaxToken colonToken,
            SyntaxToken storyboardNameToken,
            SyntaxToken qualifierToken,
            SyntaxToken semiColonToken)
        {
            return new UvssTransitionSyntax(transitionKeyword, argumentList, colonToken, 
                storyboardNameToken, qualifierToken, semiColonToken);
        }

        /// <summary>
        /// Creates a new transition argument list using automatically created tokens.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group.</param>
        /// <param name="visualStateEnd">The name of the ending visual state.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            String visualStateGroup, String visualStateEnd)
        {
            return TransitionArgumentList(visualStateGroup, null, visualStateEnd);
        }

        /// <summary>
        /// Creates a new transition argument list using automatically created tokens.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group.</param>
        /// <param name="visualStateStart">The name of the starting visual state.</param>
        /// <param name="visualStateEnd">The name of the ending visual state.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            String visualStateGroup, String visualStateStart, String visualStateEnd)
        {
            var builder = SeparatedSyntaxListBuilder<SyntaxNode>.Create();
            builder.Add(Identifier(visualStateGroup));

            if (visualStateStart != null)
            {
                builder.AddSeparator(Punctuation(SyntaxKind.CommaToken));
                builder.Add(Identifier(visualStateStart));
            }

            builder.AddSeparator(Punctuation(SyntaxKind.CommaToken));
            builder.Add(Identifier(visualStateEnd));

            return TransitionArgumentList(
                Punctuation(SyntaxKind.OpenParenthesesToken),
                builder.ToList(),
                Punctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new transition argument list.
        /// </summary>
        /// <param name="openParenToken">The open parenthesis that introduces the argument list.</param>
        /// <param name="argumentList">The list of arguments.</param>
        /// <param name="closeParenToken">The close parenthesis that terminates the argument list.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> argumentList,
            SyntaxToken closeParenToken)
        {
            return new UvssTransitionArgumentListSyntax(openParenToken, argumentList, closeParenToken);
        }

        /// <summary>
        /// Creates a new storyboard declaration using an automatically created tokens.
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(String name, UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(
                Punctuation(SyntaxKind.AtSignToken),
                Identifier(name),
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard declaration using an automatically created tokens.
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="loop">The storyboard's loop specifier.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(String name, String loop, UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(
                Punctuation(SyntaxKind.AtSignToken),
                Identifier(name),
                Identifier(loop),
                body);
        }

        /// <summary>
        /// Creates a new storyboard declaration.
        /// </summary>
        /// <param name="atSignToken">The at sign token that marks the declaration as a storyboard.</param>
        /// <param name="nameToken">The storyboard's name token.</param>
        /// <param name="loopToken">The storyboard's loop token.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(
            SyntaxToken atSignToken,
            SyntaxToken nameToken,
            SyntaxToken loopToken,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(atSignToken, nameToken, loopToken, body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration using automatically created tokens.
        /// </summary>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                Keyword(SyntaxKind.TargetKeyword),
                null, 
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration using automatically created tokens.
        /// </summary>
        /// <param name="typeName">The storyboard target's targeted type.</param>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(String typeName,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                Keyword(SyntaxKind.TargetKeyword),
                Identifier(typeName), 
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration using automatically created tokens.
        /// </summary>
        /// <param name="typeName">The storyboard target's targeted type.</param>
        /// <param name="selector">The storyboard target's selector.</param>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(String typeName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                Keyword(SyntaxKind.TargetKeyword),
                Identifier(typeName), 
                selector, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration.
        /// </summary>
        /// <param name="targetKeyword">The storyboard target's "target" keyword.</param>
        /// <param name="typeNameToken">The storyboard target's targeted type name.</param>
        /// <param name="selector">The storyboard target's selector.</param>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(
            SyntaxToken targetKeyword,
            SyntaxToken typeNameToken,
            UvssSelectorWithParenthesesSyntax selector,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(targetKeyword, typeNameToken, selector, body);
        }

        /// <summary>
        /// Creates a new animation declaration using automatically created tokens.
        /// </summary>
        /// <param name="propertyName">The animation's property name.</param>
        /// <param name="body">The animation's body.</param>
        /// <returns>The <see cref="UvssAnimationSyntax"/> instance that was created.</returns>
        public static UvssAnimationSyntax Animation(
            UvssPropertyNameSyntax propertyName,
            UvssBlockSyntax body)
        {
            return Animation(propertyName, null, body);
        }

        /// <summary>
        /// Creates a new animation declaration using automatically created tokens.
        /// </summary>
        /// <param name="propertyName">The animation's property name.</param>
        /// <param name="navigationExpression">The animation's navigation expression.</param>
        /// <param name="body">The animation's body.</param>
        /// <returns>The <see cref="UvssAnimationSyntax"/> instance that was created.</returns>
        public static UvssAnimationSyntax Animation(
            UvssPropertyNameSyntax propertyName,
            UvssNavigationExpressionSyntax navigationExpression,
            UvssBlockSyntax body)
        {
            return Animation(
                Keyword(SyntaxKind.AnimationKeyword),
                propertyName, 
                navigationExpression, 
                body);
        }

        /// <summary>
        /// Creates a new animation declaration.
        /// </summary>
        /// <param name="animationKeyword">The animation's "animation" keyword.</param>
        /// <param name="propertyName">The animation's property name.</param>
        /// <param name="navigationExpression">The animation's navigation expression.</param>
        /// <param name="body">The animation's body.</param>
        /// <returns>The <see cref="UvssAnimationSyntax"/> instance that was created.</returns>
        public static UvssAnimationSyntax Animation(
            SyntaxToken animationKeyword,
            UvssPropertyNameSyntax propertyName,
            UvssNavigationExpressionSyntax navigationExpression,
            UvssBlockSyntax body)
        {
            return new UvssAnimationSyntax(animationKeyword, propertyName, navigationExpression, body);
        }

        /// <summary>
        /// Creates a new animation keyframe declaration with automatically generated tokens.
        /// </summary>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        /// <param name="value">The keyframe's property value.</param>
        /// <returns>The <see cref="UvssAnimationKeyframeSyntax"/> instance that was created.</returns>
        public static UvssAnimationKeyframeSyntax AnimationKeyframe(
            Int32 time, UvssPropertyValueWithBracesSyntax value)
        {
            return AnimationKeyframe(time, null, value);
        }

        /// <summary>
        /// Creates a new animation keyframe declaration with automatically generated tokens.
        /// </summary>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        /// <param name="easing">The keyframe's easing name.</param>
        /// <param name="value">The keyframe's property value.</param>
        /// <returns>The <see cref="UvssAnimationKeyframeSyntax"/> instance that was created.</returns>
        public static UvssAnimationKeyframeSyntax AnimationKeyframe(
            Int32 time, String easing, UvssPropertyValueWithBracesSyntax value)
        {
            return AnimationKeyframe(
                Keyword(SyntaxKind.KeyframeKeyword),
                Number(time),
                (easing == null) ? null : Token(SyntaxKind.IdentifierToken, easing), 
                value);
        }

        /// <summary>
        /// Creates a new animation keyframe declaration.
        /// </summary>
        /// <param name="keyframeKeyword">The keyframe's "keyframe" keyword.</param>
        /// <param name="timeToken">The keyframe's time token.</param>
        /// <param name="easingToken">The keyframe's easing token.</param>
        /// <param name="value">The keyframe's property value.</param>
        /// <returns>The <see cref="UvssAnimationKeyframeSyntax"/> instance that was created.</returns>
        public static UvssAnimationKeyframeSyntax AnimationKeyframe(
            SyntaxToken keyframeKeyword,
            SyntaxToken timeToken,
            SyntaxToken easingToken,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssAnimationKeyframeSyntax(keyframeKeyword, timeToken, easingToken, value);
        }

        /// <summary>
        /// Creates a new navigation expression using automatically created tokens.
        /// </summary>
        /// <param name="propertyName">The property name of the navigation target.</param>
        /// <param name="typeName">The name of the type which is being converted to by the expression.</param>
        /// <returns>The <see cref="UvssNavigationExpressionSyntax"/> instance that was created.</returns>
        public static UvssNavigationExpressionSyntax NavigationExpression(
            UvssPropertyNameSyntax propertyName, String typeName)
        {
            return NavigationExpression(
                Punctuation(SyntaxKind.PipeToken),
                propertyName,
                Keyword(SyntaxKind.AsKeyword),
                Identifier(typeName));
        }

        /// <summary>
        /// Creates a new navigation expression.
        /// </summary>
        /// <param name="pipeToken">The pipe token that introduces the navigation expression.</param>
        /// <param name="propertyName">The property name of the navigation target.</param>
        /// <param name="asKeyword">The "as" keyword that introduces the type conversion.</param>
        /// <param name="typeNameToken">The name of the type which is being converted to by the expression.</param>
        /// <returns>The <see cref="UvssNavigationExpressionSyntax"/> instance that was created.</returns>
        public static UvssNavigationExpressionSyntax NavigationExpression(
            SyntaxToken pipeToken,
            UvssPropertyNameSyntax propertyName,
            SyntaxToken asKeyword,
            SyntaxToken typeNameToken)
        {
            return new UvssNavigationExpressionSyntax(pipeToken, propertyName, asKeyword, typeNameToken);
        }
    }
}
