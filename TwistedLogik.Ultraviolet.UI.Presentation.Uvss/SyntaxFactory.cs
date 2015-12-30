using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Contains factory methods for constructing UVSS syntax nodes.
    /// </summary>
    public static class SyntaxFactory
    {
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
        /// Creates a new UVSS document root node.
        /// </summary>
        /// <param name="ruleSetList">The document's list of rule sets.</param>
        /// <returns>The <see cref="UvssDocumentSyntax"/> instance that was created.</returns>
        public static UvssDocumentSyntax Document(SyntaxList<UvssRuleSetSyntax> ruleSetList)
        {
            return new UvssDocumentSyntax(ruleSetList);
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
        /// Creates a new selector part consisting of a single sub-part that selects for a named element.
        /// </summary>
        /// <param name="name">The name of the element being selected.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByName(String name)
        {
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByName(name) }));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a class.
        /// </summary>
        /// <param name="class">The name of the class being selected.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByClass(String @class)
        {
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByClass(@class) }));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByType(String type)
        {
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByType(type) }));
        }

        /// <summary>
        /// Creates a new selector part consisting of a single sub-part that selects for a specific type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartBySpecificType(String type)
        {
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartBySpecificType(type) }));
        }

        /// <summary>
        /// Creates a new selector part from the specified list of sub-parts.
        /// </summary>
        /// <param name="subPartsList">The list of sub-parts that make up the selector part.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPart(SyntaxList<UvssSelectorSubPartSyntax> subPartsList) 
        {
            return new UvssSelectorPartSyntax(subPartsList);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a named element.
        /// </summary>
        /// <param name="name">The name of the element being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByName(String name)
        {
            return new UvssSelectorSubPartSyntax(
                Token(SyntaxKind.HashToken, "#"), 
                Token(SyntaxKind.IdentifierToken, name), null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a class.
        /// </summary>
        /// <param name="class">The name of the class being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByClass(String @class)
        {
            return new UvssSelectorSubPartSyntax(
                Token(SyntaxKind.PeriodToken, "."),
                Token(SyntaxKind.IdentifierToken, @class), null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByType(String type)
        {
            return new UvssSelectorSubPartSyntax(null, Token(SyntaxKind.IdentifierToken, type), null);
        }

        /// <summary>
        /// Creates a new selector sub-part that selects for a specific type.
        /// </summary>
        /// <param name="type">The name of the type being selected.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartBySpecificType(String type)
        {
            return new UvssSelectorSubPartSyntax(null,
                Token(SyntaxKind.IdentifierToken, type),
                Token(SyntaxKind.ExclamationMarkToken, "!"));
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
        /// Creates a new visual descendant combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken VisualDescendantCombinator()
        {
            return Token(SyntaxKind.VisualDescendantCombinatorToken, " ");
        }

        /// <summary>
        /// Creates a new visual child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken VisualChildCombinator()
        {
            return Token(SyntaxKind.VisualChildCombinatorToken, ">");
        }

        /// <summary>
        /// Creates a new logical child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken LogicalChildCombinator()
        {
            return Token(SyntaxKind.LogicalChildCombinatorToken, ">?");
        }

        /// <summary>
        /// Creates a new templated child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken TemplatedChildCombinator()
        {
            return Token(SyntaxKind.TemplatedChildCombinatorToken, ">>");
        }

        /// <summary>
        /// Creates a new block with automatically generated curly brace tokens.
        /// </summary>
        /// <param name="content">The block's content.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(SyntaxList<SyntaxNode> content)
        {
            return new UvssBlockSyntax(
                Token(SyntaxKind.OpenCurlyBraceToken, "{"),
                content,
                Token(SyntaxKind.CloseCurlyBraceToken, "}"));
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
            return new UvssPropertyNameSyntax(
                null, 
                null,
                Token(SyntaxKind.IdentifierToken, property));
        }

        /// <summary>
        /// Creates a new property name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="owner">The owner of the attached property.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(String owner, String property)
        {
            return new UvssPropertyNameSyntax(
                Token(SyntaxKind.IdentifierToken, owner),
                Token(SyntaxKind.PeriodToken, "."),
                Token(SyntaxKind.IdentifierToken, property));
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
        /// Creates a new semi-colon-terminated property value using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueWithSemiColonSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithSemiColonSyntax PropertyValueWithSemiColon(String value)
        {
            return new UvssPropertyValueWithSemiColonSyntax(
                Token(SyntaxKind.PropertyValueToken, value), 
                Token(SyntaxKind.SemiColonToken, ";"));
        }

        /// <summary>
        /// Creates a new semi-colon-terminated property value.
        /// </summary>
        /// <param name="contentToken">The value's content token.</param>
        /// <param name="semiColonToken">The value's terminating semi-colon token.</param>
        /// <returns>The <see cref="UvssPropertyValueWithSemiColonSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithSemiColonSyntax PropertyValueWithSemiColon(
            SyntaxToken contentToken,
            SyntaxToken semiColonToken)
        {
            return new UvssPropertyValueWithSemiColonSyntax(contentToken, semiColonToken);
        }

        /// <summary>
        /// Creates a new brace-enclosed property value using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(String value)
        {
            return new UvssPropertyValueWithBracesSyntax(
                Token(SyntaxKind.OpenCurlyBraceToken, "{"),
                Token(SyntaxKind.PropertyValueToken, value),
                Token(SyntaxKind.CloseCurlyBraceToken, "}"));
        }

        /// <summary>
        /// Creates a new brace-enclosed property value using automatically created curly brace tokens.
        /// </summary>
        /// <param name="contentToken">The value's content token.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(SyntaxToken contentToken)
        {
            return new UvssPropertyValueWithBracesSyntax(
                Token(SyntaxKind.OpenCurlyBraceToken, "{"),
                contentToken,
                Token(SyntaxKind.CloseCurlyBraceToken, "}"));
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
        /// <returns>The <see cref="UvssRuleSyntax"/> instance that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueWithSemiColonSyntax value)
        {
            return new UvssRuleSyntax(propertyName, Token(SyntaxKind.ColonToken, ":"), value);
        }

        /// <summary>
        /// Creates a new styling rule.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="colonToken">The colon token that separates the property name from its value.</param>
        /// <param name="value">The styled property value.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName, 
            SyntaxToken colonToken, 
            UvssPropertyValueWithSemiColonSyntax value)
        {
            return new UvssRuleSyntax(propertyName, colonToken, value);
        }
    }
}
