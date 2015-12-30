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
        /// Creates a new parentheses-enclosed selector with automatically created parenthesis tokens.
        /// </summary>
        /// <param name="selector">The enclosed selector.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParentheses(UvssSelectorSyntax selector)
        {
            return SelectorWithParentheses(
                Token(SyntaxKind.OpenParenthesesToken, "("),
                selector,
                Token(SyntaxKind.CloseParenthesesToken, ")"));
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
        /// <param name="pseudoClass">The name of the selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByName(String name, String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByName(name) }),
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
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByClass(@class) }),
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
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartByType(type) }),
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
            return new UvssSelectorPartSyntax(List(new[] { SelectorSubPartBySpecificType(type) }),
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
        /// Creates a new pseudo-class specifier using an automatically-generated colon token and an identifier
        /// token with the specified class name value.
        /// </summary>
        /// <param name="className">The pseudo-class' name.</param>
        /// <returns>The <see cref="UvssPseudoClassSyntax"/> instance that was created.</returns>
        public static UvssPseudoClassSyntax PseudoClass(String className)
        {
            return new UvssPseudoClassSyntax(
                Token(SyntaxKind.ColonToken, ":"),
                Token(SyntaxKind.IdentifierToken, className));
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
        /// Creates a new event name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="event">The event name.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(String @event)
        {
            return new UvssEventNameSyntax(
                null,
                null,
                Token(SyntaxKind.IdentifierToken, @event));
        }

        /// <summary>
        /// Creates a new event name using new tokens created from the specified identifiers.
        /// </summary>
        /// <param name="owner">The owner of the attached event.</param>
        /// <param name="event">The event name.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(String owner, String @event)
        {
            return new UvssEventNameSyntax(
                Token(SyntaxKind.IdentifierToken, owner),
                Token(SyntaxKind.PeriodToken, "."),
                Token(SyntaxKind.IdentifierToken, @event));
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
            return new UvssPropertyValueSyntax(
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
        /// <param name="important">A value indicating whether this rule has the !important qualifier.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> instance that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueSyntax value,
            Boolean important = false)
        {
            return new UvssRuleSyntax(propertyName,
                Token(SyntaxKind.ColonToken, ":"), value,
                important ? ImportantKeyword() : null,
                Token(SyntaxKind.SemiColonToken, ";"));
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
        /// Creates a new token representing the !important keyword.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken ImportantKeyword()
        {
            return new SyntaxToken(SyntaxKind.ImportantKeyword, "!important", Whitespace(" "), null);
        }

        /// <summary>
        /// Creates a new property trigger using automatically created "trigger" and "property" keyword tokens.
        /// </summary>
        /// <param name="evaluationList">The property trigger's evaluations list.</param>
        /// <param name="body">The property trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            SeparatedSyntaxList<UvssPropertyTriggerEvaluationSyntax> evaluationList,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return new UvssPropertyTriggerSyntax(
                Token(SyntaxKind.TriggerKeyword, "trigger", null, Whitespace(" ")),
                Token(SyntaxKind.PropertyKeyword, "property", null, Whitespace(" ")),
                evaluationList, important ? ImportantKeyword() : null, body);
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
            SeparatedSyntaxList<UvssPropertyTriggerEvaluationSyntax> evaluationList,
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
        /// <returns>The <see cref="UvssPropertyTriggerEvaluationSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerEvaluationSyntax PropertyTriggerEvaluation(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken comparisonOperatorToken,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPropertyTriggerEvaluationSyntax(propertyName, comparisonOperatorToken, value);
        }

        /// <summary>
        /// Creates a new equals comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken EqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.EqualsToken, "=", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new not equals comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken NotEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.NotEqualsToken, "<>", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new less than comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken LessThanComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.LessThanToken, "<", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new greater than comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken GreaterThanComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.GreaterThanToken, ">", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new less than or equal to comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken LessThanEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.LessThanEqualsToken, "<=", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new greater than or equal to comparison.
        /// </summary>
        /// <param name="leadingTrivia">The token's leading trivia.</param>
        /// <param name="trailingTrivia">The token's trailing trivia.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken GreaterThanEqualsComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new SyntaxToken(SyntaxKind.GreaterThanEqualsToken, ">=", leadingTrivia, trailingTrivia);
        }

        /// <summary>
        /// Creates a new play-storyboard trigger action using an automatically created play-storyboard keyword token.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPlayStoryboardTriggerActionSyntax(
                Token(SyntaxKind.PlayStoryboardKeyword, "play-storyboard"), null, value);
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
            return new UvssPlayStoryboardTriggerActionSyntax(
                Token(SyntaxKind.PlayStoryboardKeyword, "play-storyboard"), selector, value);
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
            return new UvssPlaySfxTriggerActionSyntax(Token(SyntaxKind.PlaySfxKeyword, "play-sfx"), value);
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
            return new UvssSetTriggerActionSyntax(
                Token(SyntaxKind.SetKeyword, "set"), propertyName, null, value);
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
            return new UvssSetTriggerActionSyntax(
                Token(SyntaxKind.SetKeyword, "set"), propertyName, selector, value);
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
            return new UvssEventTriggerSyntax(
                Token(SyntaxKind.TriggerKeyword, "trigger", null, Whitespace(" ")),
                Token(SyntaxKind.EventKeyword, "event", null, Whitespace(" ")),
                eventName, null, important ? ImportantKeyword() : null, body);
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
            return new UvssEventTriggerSyntax(
                Token(SyntaxKind.TriggerKeyword, "trigger", null, Whitespace(" ")),
                Token(SyntaxKind.EventKeyword, "event", null, Whitespace(" ")),
                eventName, argumentList, important ? ImportantKeyword() : null, body);
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
                builder.Add(Token(SyntaxKind.SetHandledKeyword, "set-handled"));

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
                Token(SyntaxKind.OpenParenthesesToken, "("),
                arguments,
                Token(SyntaxKind.CloseParenthesesToken, ")"));
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
        /// Creates a new storyboard declaration using an automatically created tokens.
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(String name, UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(
                Token(SyntaxKind.AtSignToken, "@"),
                Token(SyntaxKind.IdentifierToken, name, null, Whitespace(" ")),
                null, body);
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
                Token(SyntaxKind.AtSignToken, "@"),
                Token(SyntaxKind.IdentifierToken, name, null, Whitespace(" ")),
                Token(SyntaxKind.IdentifierToken, loop),
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
                Token(SyntaxKind.TargetKeyword, "target", null, Whitespace(" ")),
                null, null, body);
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
                Token(SyntaxKind.TargetKeyword, "target", null, Whitespace(" ")),
                Token(SyntaxKind.IdentifierToken, typeName), null, body);
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
                Token(SyntaxKind.TargetKeyword, "target", null, Whitespace(" ")),
                Token(SyntaxKind.IdentifierToken, typeName), selector, body);
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
                Token(SyntaxKind.AnimationKeyword, "animation", null, Whitespace(" ")),
                propertyName, navigationExpression, body);
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
                Token(SyntaxKind.KeyframeKeyword, "keyframe", null, Whitespace(" ")),
                Token(SyntaxKind.NumberToken, time.ToString(CultureInfo.InvariantCulture), null, Whitespace(" ")),
                (easing == null) ? null : Token(SyntaxKind.IdentifierToken, easing, null, Whitespace(" ")), value);
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
                Token(SyntaxKind.NavigationExpressionOperatorToken, "|", Whitespace(" "), Whitespace(" ")),
                propertyName,
                Token(SyntaxKind.AsKeyword, "as", Whitespace(" "), Whitespace(" ")),
                Token(SyntaxKind.IdentifierToken, typeName));
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
