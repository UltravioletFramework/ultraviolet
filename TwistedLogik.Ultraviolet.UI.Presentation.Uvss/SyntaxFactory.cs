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
        /// Creates a new identifier token.
        /// </summary>
        /// <param name="text">The identifier text.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Identifier(String text)
        {
            return new UvssIdentifier(text, null, null);
        }

        /// <summary>
        /// Creates a new number token.
        /// </summary>
        /// <param name="value">The numeric value of the token.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Number(Int32 value)
        {
            return new SyntaxToken(SyntaxKind.NumberToken,
                value.ToString(CultureInfo.InvariantCulture), null, null);
        }

        /// <summary>
        /// Creates a new number token.
        /// </summary>
        /// <param name="value">The numeric value of the token.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Number(Single value)
        {
            return new SyntaxToken(SyntaxKind.NumberToken,
                value.ToString(CultureInfo.InvariantCulture), null, null);
        }

        /// <summary>
        /// Creates a new number token.
        /// </summary>
        /// <param name="value">The numeric value of the token.</param>
        /// <returns>The <see cref="SyntaxToken"/> instance that was created.</returns>
        public static SyntaxToken Number(Double value)
        {
            return new SyntaxToken(SyntaxKind.NumberToken,
                value.ToString(CultureInfo.InvariantCulture), null, null);
        }

        /// <summary>
        /// Creates a new visual descendant combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken VisualDescendantCombinator()
        {
            return new UvssPunctuation(SyntaxKind.SpaceToken);
        }

        /// <summary>
        /// Creates a new visual child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken VisualChildCombinator()
        {
            return new UvssPunctuation(SyntaxKind.GreaterThanToken);
        }

        /// <summary>
        /// Creates a new logical child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken LogicalChildCombinator()
        {
            return new UvssPunctuation(SyntaxKind.GreaterThanQuestionMarkToken);
        }

        /// <summary>
        /// Creates a new templated child combinator token.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> that was created.</returns>
        public static SyntaxToken TemplatedChildCombinator()
        {
            return new UvssPunctuation(SyntaxKind.GreaterThanGreaterThanToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is
        /// equal to the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken EqualsComparison()
        {
            return new UvssPunctuation(SyntaxKind.EqualsToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is
        /// not equal to the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken NotEqualsComparison()
        {
            return new UvssPunctuation(SyntaxKind.NotEqualsToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is less
        /// than the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken LessThanComparison(
            SyntaxNode leadingTrivia = null,
            SyntaxNode trailingTrivia = null)
        {
            return new UvssPunctuation(SyntaxKind.LessThanToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is 
        /// greater than the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken GreaterThanComparison()
        {
            return new UvssPunctuation(SyntaxKind.GreaterThanToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is
        /// less than or equal to the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken LessThanEqualsComparison()
        {
            return new UvssPunctuation(SyntaxKind.LessThanEqualsToken);
        }

        /// <summary>
        /// Creates a new comparison operator which determines whether the target property is
        /// greater than or equal to the comparison value.
        /// </summary>
        /// <returns>The <see cref="SyntaxToken"/> instance which was created.</returns>
        public static SyntaxToken GreaterThanEqualsComparison()
        {
            return new UvssPunctuation(SyntaxKind.GreaterThanEqualsToken);
        }

        /// <summary>
        /// Creates an empty list.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <returns>The <see cref="SyntaxList{TNode}"/> that was created.</returns>
        public static SyntaxList<TNode> List<TNode>()
            where TNode : SyntaxList
        {
            return default(SyntaxList<TNode>);
        }

        /// <summary>
        /// Creates a list of nodes from a sequence of nodes.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the list.</typeparam>
        /// <param name="nodes">The collection of nodes with which to populate the list.</param>
        /// <returns>The <see cref="SyntaxList{TNode}"/> that was created.</returns>
        public static SyntaxList<TNode> List<TNode>(
            IEnumerable<TNode> nodes)
            where TNode : SyntaxNode
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
        public static SeparatedSyntaxList<TNode> SeparatedList<TNode>()
            where TNode : SyntaxNode
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
            IEnumerable<TNode> nodes)
            where TNode : SyntaxNode
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
                    builder.AddSeparator(new SyntaxToken(SyntaxKind.CommaToken, ","));
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
            IEnumerable<TNode> nodes,
            IEnumerable<SyntaxToken> separators)
            where TNode : SyntaxNode
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
        /// Creates a new document root node.
        /// </summary>
        /// <param name="content">The document's content.</param>
        /// <returns>The <see cref="UvssDocumentSyntax"/> instance that was created.</returns>
        public static UvssDocumentSyntax Document(
            SyntaxList<SyntaxNode> content)
        {
            return new UvssDocumentSyntax(content,
                new SyntaxToken(SyntaxKind.EndOfFileToken, null, null, null));
        }

        /// <summary>
        /// Creates a new document root node.
        /// </summary>
        /// <param name="content">The document's content.</param>
        /// <param name="endOfFileToken">The document's end-of-file token.</param>
        /// <returns>The <see cref="UvssDocumentSyntax"/> instance that was created.</returns>
        public static UvssDocumentSyntax Document(
            SyntaxList<SyntaxNode> content,
            SyntaxToken endOfFileToken)
        {
            return new UvssDocumentSyntax(
                content,
                endOfFileToken);
        }

        /// <summary>
        /// Creates a new rule set node.
        /// </summary>
        /// <param name="selector">The rule set's selector.</param>
        /// <param name="body">The rule set's body.</param>
        /// <returns>The <see cref="UvssRuleSetSyntax"/> instance that was created.</returns>
        public static UvssRuleSetSyntax RuleSet(
            UvssSelectorSyntax selector,
            UvssBlockSyntax body)
        {
            return new UvssRuleSetSyntax(
                SeparatedList(new[] { selector }),
                body);
        }

        /// <summary>
        /// Creates a new rule set node.
        /// </summary>
        /// <param name="selectors">The rule set's selectors.</param>
        /// <param name="body">The rule set's body.</param>
        /// <returns>The <see cref="UvssRuleSetSyntax"/> instance that was created.</returns>
        public static UvssRuleSetSyntax RuleSet(
            IEnumerable<UvssSelectorSyntax> selectors,
            UvssBlockSyntax body)
        {
            return new UvssRuleSetSyntax(
                SeparatedList(selectors),
                body);
        }

        /// <summary>
        /// Creates a new rule set node.
        /// </summary>
        /// <param name="selectors">The rule set's list of selectors.</param>
        /// <param name="body">The rule set's body.</param>
        /// <returns>The <see cref="UvssRuleSetSyntax"/> instance that was created.</returns>
        public static UvssRuleSetSyntax RuleSet(
            SeparatedSyntaxList<UvssSelectorSyntax> selectors,
            UvssBlockSyntax body)
        {
            return new UvssRuleSetSyntax(
                selectors,
                body);
        }

        /// <summary>
        /// Creates a new parentheses-enclosed universal selector.
        /// </summary>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax UniversalSelectorWithParentheses(
            String pseudoClass = null)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                UniversalSelector(pseudoClass),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector which selects the element with the specified name.
        /// </summary>
        /// <param name="selectedName">The name of the selected element.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParenthesesByName(
            String selectedName,
            String pseudoClass = null)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SelectorByName(selectedName, pseudoClass),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector which selects the specified class.
        /// </summary>
        /// <param name="selectedClass">The name of the selected class.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParenthesesByClass(
            String selectedClass,
            String pseudoClass = null)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SelectorByClass(selectedClass, pseudoClass),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector which selects the specified type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParenthesesByType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SelectorByType(selectedType, pseudoClass),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector which selects the specified exact type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParenthesesBySpecificType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SelectorBySpecificType(selectedType, pseudoClass),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new parentheses-enclosed selector.
        /// </summary>
        /// <param name="selector">The enclosed selector.</param>
        /// <returns>The <see cref="UvssSelectorWithParenthesesSyntax"/> instance that was created.</returns>
        public static UvssSelectorWithParenthesesSyntax SelectorWithParentheses(
            UvssSelectorSyntax selector)
        {
            return new UvssSelectorWithParenthesesSyntax(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                selector,
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
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
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax UniversalSelector(
            String pseudoClass = null)
        {
            return new UvssSelectorSyntax(
                List(new[] { UniversalSelectorPart(pseudoClass) }));
        }

        /// <summary>
        /// Creates a new selector which selects the element with the specified name.
        /// </summary>
        /// <param name="selectedName">The name of the selected element.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByName(
            String selectedName,
            String pseudoClass = null)
        {
            return new UvssSelectorSyntax(
                List(new[] { SelectorPartByName(selectedName, pseudoClass) }));
        }

        /// <summary>
        /// Creates a new selector which selects the specified class.
        /// </summary>
        /// <param name="selectedClass">The name of the selected class.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByClass(
            String selectedClass,
            String pseudoClass = null)
        {
            return new UvssSelectorSyntax(
                List(new[] { SelectorPartByClass(selectedClass, pseudoClass) }));
        }

        /// <summary>
        /// Creates a new selector which selects the specified type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorByType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorSyntax(
                List(new[] { SelectorPartByType(selectedType, pseudoClass) }));
        }

        /// <summary>
        /// Creates a new selector which selects a specific type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> instance that was created.</returns>
        public static UvssSelectorSyntax SelectorBySpecificType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorSyntax(
                List(new[] { SelectorPartBySpecificType(selectedType, pseudoClass) }));
        }

        /// <summary>
        /// Creates a new selector.
        /// </summary>
        /// <param name="components">The selector's list of components.</param>
        /// <returns>The <see cref="UvssSelectorSyntax"/> that was created.</returns>
        public static UvssSelectorSyntax Selector(SyntaxList<SyntaxNode> components)
        {
            return new UvssSelectorSyntax(components);
        }

        /// <summary>
        /// Creates a new universal selector part.
        /// </summary>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax UniversalSelectorPart(
            String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                List(new[] { UniversalSelectorSubPart() }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part which selects the element with the specified name.
        /// </summary>
        /// <param name="selectedName">The name of the selected element.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByName(
            String selectedName,
            String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                List(new[] { SelectorSubPartByName(selectedName) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part which selects the specified class.
        /// </summary>
        /// <param name="selectedClass">The name of the selected class.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByClass(
            String selectedClass,
            String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                List(new[] { SelectorSubPartByClass(selectedClass) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part which selects the specified type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartByType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                List(new[] { SelectorSubPartByType(selectedType) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part which selects a specific type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <param name="pseudoClass">The name of the selector part's pseudo-class, if any.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPartBySpecificType(
            String selectedType,
            String pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                List(new[] { SelectorSubPartBySpecificType(selectedType) }),
                (pseudoClass == null) ? null : PseudoClass(pseudoClass));
        }

        /// <summary>
        /// Creates a new selector part.
        /// </summary>
        /// <param name="subParts">The list of sub-parts that make up the selector part.</param>
        /// <param name="pseudoClass">The selector part's pseudo-class.</param>
        /// <returns>The <see cref="UvssSelectorPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorPartSyntax SelectorPart(
            SyntaxList<UvssSelectorSubPartSyntax> subParts,
            UvssPseudoClassSyntax pseudoClass = null)
        {
            return new UvssSelectorPartSyntax(
                subParts,
                pseudoClass);
        }

        /// <summary>
        /// Creates a new universal selector sub-part.
        /// </summary>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax UniversalSelectorSubPart()
        {
            return SelectorSubPart(
                null,
                new UvssPunctuation(SyntaxKind.AsteriskToken),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part which selects the element with the specified name.
        /// </summary>
        /// <param name="selectedName">The name of the selected element.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByName(
            String selectedName)
        {
            return SelectorSubPart(
                new UvssPunctuation(SyntaxKind.HashToken),
                new UvssIdentifier(selectedName),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part which selects the specified class.
        /// </summary>
        /// <param name="selectedClass">The name of the selected class.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByClass(
            String selectedClass)
        {
            return SelectorSubPart(
                new UvssPunctuation(SyntaxKind.PeriodToken),
                new UvssIdentifier(selectedClass),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part which selects the specified type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartByType(
            String selectedType)
        {
            return SelectorSubPart(
                null,
                new UvssIdentifier(selectedType),
                null);
        }

        /// <summary>
        /// Creates a new selector sub-part which selects the a specific type.
        /// </summary>
        /// <param name="selectedType">The name of the selected type.</param>
        /// <returns>The <see cref="UvssSelectorSubPartSyntax"/> instance that was created.</returns>
        public static UvssSelectorSubPartSyntax SelectorSubPartBySpecificType(
            String selectedType)
        {
            return SelectorSubPart(
                null,
                new UvssIdentifier(selectedType),
                new UvssPunctuation(SyntaxKind.ExclamationMarkToken));
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
            return new UvssSelectorSubPartSyntax(
                leadingQualifierToken,
                textToken,
                trailingQualifierToken);
        }

        /// <summary>
        /// Creates a new pseudo-class specifier.
        /// </summary>
        /// <param name="className">The pseudo-class' name.</param>
        /// <returns>The <see cref="UvssPseudoClassSyntax"/> instance that was created.</returns>
        public static UvssPseudoClassSyntax PseudoClass(
            String className)
        {
            return PseudoClass(
                new UvssPunctuation(SyntaxKind.ColonToken),
                new UvssIdentifier(className)
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
            return new UvssPseudoClassSyntax(
                colonToken,
                classNameToken);
        }

        /// <summary>
        /// Creates an empty block.
        /// </summary>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block()
        {
            return new UvssBlockSyntax(
                new UvssPunctuation(SyntaxKind.OpenCurlyBraceToken),
                default(SyntaxList<SyntaxNode>),
                new UvssPunctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new block.
        /// </summary>
        /// <param name="content">The block's content.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(
            SyntaxList<SyntaxNode> content)
        {
            return Block(
                new UvssPunctuation(SyntaxKind.OpenCurlyBraceToken),
                content,
                new UvssPunctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new block.
        /// </summary>
        /// <param name="content">The block's content.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(
            IEnumerable<SyntaxNode> content)
        {
            return Block(
                new UvssPunctuation(SyntaxKind.OpenCurlyBraceToken),
                List(content),
                new UvssPunctuation(SyntaxKind.CloseCurlyBraceToken));
        }

        /// <summary>
        /// Creates a new block.
        /// </summary>
        /// <param name="content">The block's content.</param>
        /// <returns>The <see cref="UvssBlockSyntax"/> instance that was created.</returns>
        public static UvssBlockSyntax Block(
            params SyntaxNode[] content)
        {
            return Block(
                new UvssPunctuation(SyntaxKind.OpenCurlyBraceToken),
                List(content),
                new UvssPunctuation(SyntaxKind.CloseCurlyBraceToken));
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
        /// Creates a new property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(
            String propertyName)
        {
            return PropertyName(
                null,
                null,
                new UvssIdentifier(propertyName));
        }

        /// <summary>
        /// Creates a new property name.
        /// </summary>
        /// <param name="attachedPropertyOwnerName">The name of the attached property's owner type.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(
            String attachedPropertyOwnerName,
            String propertyName)
        {
            return PropertyName(
                new UvssIdentifier(attachedPropertyOwnerName),
                new UvssPunctuation(SyntaxKind.PeriodToken),
                new UvssIdentifier(propertyName));
        }

        /// <summary>
        /// Creates a new property name.
        /// </summary>
        /// <param name="attachedPropertyOwnerNameToken">The name of the attached property's owner type.</param>
        /// <param name="periodToken">The period token that separates the owner name from the property name.</param>
        /// <param name="propertyNameToken">The name of the property.</param>
        /// <returns>The <see cref="UvssPropertyNameSyntax"/> instance that was created.</returns>
        public static UvssPropertyNameSyntax PropertyName(
            SyntaxToken attachedPropertyOwnerNameToken,
            SyntaxToken periodToken,
            SyntaxToken propertyNameToken)
        {
            return new UvssPropertyNameSyntax(
                attachedPropertyOwnerNameToken,
                periodToken,
                propertyNameToken);
        }

        /// <summary>
        /// Creates a new event name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(
            String eventName)
        {
            return EventName(
                null,
                null,
                new UvssIdentifier(eventName));
        }

        /// <summary>
        /// Creates a new event name.
        /// </summary>
        /// <param name="attachedEventOwnerName">The name of the attached event's owner type.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The <see cref="UvssEventNameSyntax"/> instance that was created.</returns>
        public static UvssEventNameSyntax EventName(
            String attachedEventOwnerName,
            String eventName)
        {
            return EventName(
                new UvssIdentifier(attachedEventOwnerName),
                new UvssPunctuation(SyntaxKind.PeriodToken),
                new UvssIdentifier(eventName));
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
        /// Creates a new property value.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueSyntax PropertyValue(
            String value)
        {
            return PropertyValue(
                new SyntaxToken(SyntaxKind.PropertyValueToken, value));
        }

        /// <summary>
        /// Creates a new property value.
        /// </summary>
        /// <param name="contentToken">The value's content token.</param>
        /// <returns>The <see cref="UvssPropertyValueSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueSyntax PropertyValue(
            SyntaxToken contentToken)
        {
            return new UvssPropertyValueSyntax(contentToken);
        }

        /// <summary>
        /// Creates a new brace-enclosed property value.
        /// </summary>
        /// <param name="value">The property value.</param>
        /// <returns>The <see cref="UvssPropertyValueWithBracesSyntax"/> instance that was created.</returns>
        public static UvssPropertyValueWithBracesSyntax PropertyValueWithBraces(
            String value)
        {
            return PropertyValueWithBraces(
                new UvssPunctuation(SyntaxKind.OpenCurlyBraceToken),
                new SyntaxToken(SyntaxKind.PropertyValueToken, value),
                new UvssPunctuation(SyntaxKind.CloseCurlyBraceToken));
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
            return new UvssPropertyValueWithBracesSyntax(
                openCurlyBraceToken,
                contentToken,
                closeCurlyBraceToken);
        }

        /// <summary>
        /// Creates a new styling rule.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="propertyValue">The value of the styled property.</param>
        /// <param name="important">A value indicating whether this rule has the !important qualifier.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> instance that was created.</returns>
        public static UvssRuleSyntax Rule(
            String propertyName,
            String propertyValue,
            Boolean important = false)
        {
            return new UvssRuleSyntax(
                PropertyName(propertyName),
                new UvssPunctuation(SyntaxKind.ColonToken),
                PropertyValue(propertyValue),
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                new UvssPunctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new styling rule.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="propertyValue">The value of the styled property.</param>
        /// <param name="important">A value indicating whether this rule has the !important qualifier.</param>
        /// <returns>The <see cref="UvssRuleSyntax"/> instance that was created.</returns>
        public static UvssRuleSyntax Rule(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueSyntax propertyValue,
            Boolean important = false)
        {
            return new UvssRuleSyntax(
                propertyName,
                new UvssPunctuation(SyntaxKind.ColonToken),
                propertyValue,
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                new UvssPunctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new styling rule.
        /// </summary>
        /// <param name="propertyName">The name of the styled property.</param>
        /// <param name="colonToken">The colon token that separates the property name from its value.</param>
        /// <param name="value">The value of the styled property.</param>
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
            return new UvssRuleSyntax(
                propertyName,
                colonToken,
                value,
                qualifierToken,
                semiColonToken);
        }

        /// <summary>
        /// Creates a new property trigger.
        /// </summary>
        /// <param name="condition">The trigger's condition.</param>
        /// <param name="body">The trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            UvssPropertyTriggerConditionSyntax condition,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return new UvssPropertyTriggerSyntax(
                new UvssKeyword(SyntaxKind.TriggerKeyword),
                new UvssKeyword(SyntaxKind.PropertyKeyword),
                SeparatedList(new[] { condition }),
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null, body);
        }

        /// <summary>
        /// Creates a new property trigger.
        /// </summary>
        /// <param name="conditions">The property trigger's conditions.</param>
        /// <param name="body">The property trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> conditions,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return PropertyTrigger(
                new UvssKeyword(SyntaxKind.TriggerKeyword),
                new UvssKeyword(SyntaxKind.PropertyKeyword),
                conditions,
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                body);
        }

        /// <summary>
        /// Creates a new property trigger.
        /// </summary>
        /// <param name="triggerKeyword">The property trigger's "trigger" keyword.</param>
        /// <param name="propertyKeyword">The property trigger's "property" keyword.</param>
        /// <param name="conditions">The property trigger's conditions.</param>
        /// <param name="qualifierToken">The property trigger's qualifier token.</param>
        /// <param name="body">The property trigger's body.</param>
        /// <returns>The <see cref="UvssPropertyTriggerSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerSyntax PropertyTrigger(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> conditions,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
        {
            return new UvssPropertyTriggerSyntax(
                triggerKeyword, propertyKeyword, conditions, qualifierToken, body);
        }

        /// <summary>
        /// Creates a new property trigger evaluation.
        /// </summary>
        /// <param name="propertyName">The name of the property being evaluated.</param>
        /// <param name="comparisonOperatorToken">The comparison operator used to perform the evaluation.</param>
        /// <param name="propertyValue">The value which is being compared to the property value.</param>
        /// <returns>The <see cref="UvssPropertyTriggerConditionSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerConditionSyntax PropertyTriggerCondition(
            String propertyName,
            SyntaxToken comparisonOperatorToken,
            String propertyValue)
        {
            return new UvssPropertyTriggerConditionSyntax(
                PropertyName(propertyName),
                comparisonOperatorToken,
                PropertyValueWithBraces(propertyValue));
        }

        /// <summary>
        /// Creates a new property trigger evaluation.
        /// </summary>
        /// <param name="propertyName">The name of the property being evaluated.</param>
        /// <param name="comparisonOperatorToken">The comparison operator used to perform the evaluation.</param>
        /// <param name="propertyValue">The value which is being compared to the property value.</param>
        /// <returns>The <see cref="UvssPropertyTriggerConditionSyntax"/> instance that was created.</returns>
        public static UvssPropertyTriggerConditionSyntax PropertyTriggerCondition(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken comparisonOperatorToken,
            UvssPropertyValueWithBracesSyntax propertyValue)
        {
            return new UvssPropertyTriggerConditionSyntax(
                propertyName,
                comparisonOperatorToken,
                propertyValue);
        }

        /// <summary>
        /// Creates a new event trigger.
        /// </summary>
        /// <param name="eventName">The trigger's event name.</param>
        /// <param name="body">The trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssEventTriggerSyntax"/> instance that was created.</returns>
        public static UvssEventTriggerSyntax EventTrigger(
            UvssEventNameSyntax eventName,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return EventTrigger(
                new UvssKeyword(SyntaxKind.TriggerKeyword),
                new UvssKeyword(SyntaxKind.EventKeyword),
                eventName,
                null,
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                body);
        }

        /// <summary>
        /// Creates a new event trigger.
        /// </summary>
        /// <param name="eventName">The trigger's event name.</param>
        /// <param name="argumentList">The trigger's argument list.</param>
        /// <param name="body">The trigger's body.</param>
        /// <param name="important">A value indicating whether this trigger has the !important qualifier.</param>
        /// <returns>The <see cref="UvssEventTriggerSyntax"/> instance that was created.</returns>
        public static UvssEventTriggerSyntax EventTrigger(
            UvssEventNameSyntax eventName,
            UvssEventTriggerArgumentList argumentList,
            UvssBlockSyntax body,
            Boolean important = false)
        {
            return EventTrigger(
                new UvssKeyword(SyntaxKind.TriggerKeyword),
                new UvssKeyword(SyntaxKind.EventKeyword),
                eventName,
                argumentList,
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                body);
        }

        /// <summary>
        /// Creates a new event trigger.
        /// </summary>
        /// <param name="triggerKeyword">The trigger's "trigger" keyword.</param>
        /// <param name="eventKeyword">The trigger's "event" keyword.</param>
        /// <param name="eventName">The trigger's event name.</param>
        /// <param name="argumentList">The trigger's argument list.</param>
        /// <param name="qualifierToken">The trigger's qualifier token.</param>
        /// <param name="body">The trigger's body.</param>
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
                triggerKeyword,
                eventKeyword,
                eventName,
                argumentList,
                qualifierToken,
                body);
        }

        /// <summary>
        /// Creates a new trigger action which plays a storyboard.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            String value)
        {
            return new UvssPlayStoryboardTriggerActionSyntax(
                new UvssKeyword(SyntaxKind.PlayStoryboardKeyword),
                null,
                PropertyValueWithBraces(value));
        }

        /// <summary>
        /// Creates a new trigger action which plays a storyboard.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPlayStoryboardTriggerActionSyntax(
                new UvssKeyword(SyntaxKind.PlayStoryboardKeyword),
                null,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which plays a storyboard.
        /// </summary>
        /// <param name="selector">The trigger action's selector.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlayStoryboardTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlayStoryboardTriggerActionSyntax PlayStoryboardTriggerAction(
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
        {
            return PlayStoryboardTriggerAction(
                new UvssKeyword(SyntaxKind.PlayStoryboardKeyword),
                selector,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which plays a storyboard.
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
            return new UvssPlayStoryboardTriggerActionSyntax(
                playStoryboardKeyword,
                selector,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which plays a sound effect.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlaySfxTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlaySfxTriggerActionSyntax PlaySfxTriggerAction(
            String value)
        {
            return PlaySfxTriggerAction(
                new UvssKeyword(SyntaxKind.PlaySfxKeyword),
                PropertyValueWithBraces(value));
        }

        /// <summary>
        /// Creates a new trigger action which plays a sound effect.
        /// </summary>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlaySfxTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlaySfxTriggerActionSyntax PlaySfxTriggerAction(
            UvssPropertyValueWithBracesSyntax value)
        {
            return PlaySfxTriggerAction(
                new UvssKeyword(SyntaxKind.PlaySfxKeyword),
                value);
        }

        /// <summary>
        /// Creates a new trigger action which plays a sound effect.
        /// </summary>
        /// <param name="playSfxKeyword">The trigger action's "play-sfx" keyword.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssPlaySfxTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssPlaySfxTriggerActionSyntax PlaySfxTriggerAction(
            SyntaxToken playSfxKeyword,
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssPlaySfxTriggerActionSyntax(
                playSfxKeyword,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which sets a property value.
        /// </summary>
        /// <param name="propertyName">The name of the property which is set by the trigger.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssSetTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssSetTriggerActionSyntax SetTriggerAction(
            String propertyName,
            String value)
        {
            return SetTriggerAction(
                new UvssKeyword(SyntaxKind.SetKeyword),
                PropertyName(propertyName),
                null,
                PropertyValueWithBraces(value));
        }

        /// <summary>
        /// Creates a new trigger action which sets a property value.
        /// </summary>
        /// <param name="propertyName">The name of the property which is set by the trigger.</param>
        /// <param name="value">The trigger action's value.</param>
        /// <returns>The <see cref="UvssSetTriggerActionSyntax"/> instance that was created.</returns>
        public static UvssSetTriggerActionSyntax SetTriggerAction(
            UvssPropertyNameSyntax propertyName,
            UvssPropertyValueWithBracesSyntax value)
        {
            return SetTriggerAction(
                new UvssKeyword(SyntaxKind.SetKeyword),
                propertyName,
                null,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which sets a property value.
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
                new UvssKeyword(SyntaxKind.SetKeyword),
                propertyName,
                selector,
                value);
        }

        /// <summary>
        /// Creates a new trigger action which sets a property value.
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
            return new UvssSetTriggerActionSyntax(
                setKeyword,
                propertyName,
                selector,
                value);
        }

        /// <summary>
        /// Creates a new event trigger argument list.
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
                builder.Add(new SyntaxToken(SyntaxKind.HandledKeyword, "handled"));

            if (sethandled)
            {
                if (builder.Count > 0)
                {
                    builder.AddSeparator(new UvssPunctuation(SyntaxKind.CommaToken));
                }
                builder.Add(new SyntaxToken(SyntaxKind.SetHandledKeyword, "set-handled"));
            }

            return EventTriggerArgumentList(builder.ToList());
        }

        /// <summary>
        /// Creates a new event trigger argument list.
        /// </summary>
        /// <param name="arguments">The arguments in the list.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            IEnumerable<SyntaxNode> arguments)
        {
            return new UvssEventTriggerArgumentList(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SeparatedList(arguments),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new event trigger argument list.
        /// </summary>
        /// <param name="arguments">The arguments in the list.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            params SyntaxNode[] arguments)
        {
            return new UvssEventTriggerArgumentList(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                SeparatedList(arguments),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new event trigger argument list.
        /// </summary>
        /// <param name="arguments">The arguments in the list.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            SeparatedSyntaxList<SyntaxNode> arguments)
        {
            return new UvssEventTriggerArgumentList(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                arguments,
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new event trigger argument list.
        /// </summary>
        /// <param name="openParenToken">The open parenthesis that introduces the argument list.</param>
        /// <param name="arguments">The arguments in the list.</param>
        /// <param name="closeParenToken">The close parenthesis that terminates the argument list.</param>
        /// <returns>The <see cref="UvssEventTriggerArgumentList"/> instance that was created.</returns>
        public static UvssEventTriggerArgumentList EventTriggerArgumentList(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> arguments,
            SyntaxToken closeParenToken)
        {
            return new UvssEventTriggerArgumentList(
                openParenToken,
                arguments,
                closeParenToken);
        }

        /// <summary>
        /// Creates a new visual transition.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group that contains the transition states.</param>
        /// <param name="visualStateEnd">The name of the visual state that the element must end in to begin the transition.</param>
        /// <param name="storyboardName">The name of the storyboard that is played when the transition is triggered.</param>
        /// <param name="important">A value indicating whether the transition has the !important qualifier.</param>
        /// <returns>The <see cref="UvssTransitionSyntax"/> instance that was created.</returns>
        public static UvssTransitionSyntax Transition(
            String visualStateGroup,
            String visualStateEnd,
            String storyboardName,
            Boolean important = false)
        {
            return Transition(
                new UvssKeyword(SyntaxKind.TransitionKeyword),
                TransitionArgumentList(visualStateGroup, visualStateEnd),
                new UvssPunctuation(SyntaxKind.ColonToken),
                new UvssIdentifier(storyboardName),
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                new UvssPunctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new visual transition.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group that contains the transition states.</param>
        /// <param name="visualStateStart">The name of the visual state that the element must start in to begin the transition.</param>
        /// <param name="visualStateEnd">The name of the visual state that the element must end in to begin the transition.</param>
        /// <param name="storyboardName">The name of the storyboard that is played when the transition is triggered.</param>
        /// <param name="important">A value indicating whether the transition has the !important qualifier.</param>
        /// <returns>The <see cref="UvssTransitionSyntax"/> instance that was created.</returns>
        public static UvssTransitionSyntax Transition(
            String visualStateGroup,
            String visualStateStart,
            String visualStateEnd,
            String storyboardName,
            Boolean important = false)
        {
            return Transition(
                new UvssKeyword(SyntaxKind.TransitionKeyword),
                TransitionArgumentList(visualStateGroup, visualStateStart, visualStateEnd),
                new UvssPunctuation(SyntaxKind.ColonToken),
                new UvssIdentifier(storyboardName),
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                new UvssPunctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new visual transition.
        /// </summary>
        /// <param name="argumentList">The transition's argument list.</param>
        /// <param name="storyboardName">The name of the storyboard that is played when the transition is triggered.</param>
        /// <param name="important">A value indicating whether the transition has the !important qualifier.</param>
        /// <returns>The <see cref="UvssTransitionSyntax"/> instance that was created.</returns>
        public static UvssTransitionSyntax Transition(
            UvssTransitionArgumentListSyntax argumentList,
            String storyboardName,
            Boolean important = false)
        {
            return Transition(
                new UvssKeyword(SyntaxKind.TransitionKeyword),
                argumentList,
                new UvssPunctuation(SyntaxKind.ColonToken),
                new UvssIdentifier(storyboardName),
                important ? new UvssKeyword(SyntaxKind.ImportantKeyword) : null,
                new UvssPunctuation(SyntaxKind.SemiColonToken));
        }

        /// <summary>
        /// Creates a new visual transition.
        /// </summary>
        /// <param name="transitionKeyword">The "transition" keyword that introduces the transition.</param>
        /// <param name="argumentList">The transition's argument list.</param>
        /// <param name="colonToken">The colon that separates the transition declaration from its value.</param>
        /// <param name="storyboardNameToken">The name of the storyboard that is played when the transition is triggered.</param>
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
            return new UvssTransitionSyntax(
                transitionKeyword, 
                argumentList, 
                colonToken, 
                storyboardNameToken, 
                qualifierToken, 
                semiColonToken);
        }

        /// <summary>
        /// Creates a new transition argument list.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group that contains the transition states.</param>
        /// <param name="visualStateEnd">The name of the visual state that the element must end in to begin the transition.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            String visualStateGroup, 
            String visualStateEnd)
        {
            return TransitionArgumentList(
                visualStateGroup, 
                null, 
                visualStateEnd);
        }

        /// <summary>
        /// Creates a new transition argument list.
        /// </summary>
        /// <param name="visualStateGroup">The name of the visual state group that contains the transition states.</param>
        /// <param name="visualStateStart">The name of the visual state that the element must start in to begin the transition.</param>
        /// <param name="visualStateEnd">The name of the visual state that the element must end in to begin the transition.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            String visualStateGroup, 
            String visualStateStart, 
            String visualStateEnd)
        {
            var builder = SeparatedSyntaxListBuilder<SyntaxNode>.Create();
            builder.Add(Identifier(visualStateGroup));

            if (visualStateStart != null)
            {
                builder.AddSeparator(new UvssPunctuation(SyntaxKind.CommaToken));
                builder.Add(new UvssIdentifier(visualStateStart));
            }

            builder.AddSeparator(new UvssPunctuation(SyntaxKind.CommaToken));
            builder.Add(new UvssIdentifier(visualStateEnd));

            return TransitionArgumentList(
                new UvssPunctuation(SyntaxKind.OpenParenthesesToken),
                builder.ToList(),
                new UvssPunctuation(SyntaxKind.CloseParenthesesToken));
        }

        /// <summary>
        /// Creates a new transition argument list.
        /// </summary>
        /// <param name="openParenToken">The open parenthesis that introduces the argument list.</param>
        /// <param name="arguments">The list's arguments.</param>
        /// <param name="closeParenToken">The close parenthesis that terminates the argument list.</param>
        /// <returns>The <see cref="UvssTransitionArgumentListSyntax"/> instance that was created.</returns>
        public static UvssTransitionArgumentListSyntax TransitionArgumentList(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> arguments,
            SyntaxToken closeParenToken)
        {
            return new UvssTransitionArgumentListSyntax(
                openParenToken, 
                arguments, 
                closeParenToken);
        }
        
        /// <summary>
        /// Creates a new storyboard declaration
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(String name, UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(
                new UvssPunctuation(SyntaxKind.AtSignToken),
                new UvssIdentifier(name),
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard declaration.
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="loop">The storyboard's loop specifier.</param>
        /// <param name="body">The storyboard's body.</param>
        /// <returns>The <see cref="UvssStoryboardSyntax"/> instance that was created.</returns>
        public static UvssStoryboardSyntax Storyboard(String name, String loop, UvssBlockSyntax body)
        {
            return new UvssStoryboardSyntax(
                new UvssPunctuation(SyntaxKind.AtSignToken),
                new UvssIdentifier(name),
                new UvssIdentifier(loop),
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
            return new UvssStoryboardSyntax(
                atSignToken, 
                nameToken, 
                loopToken, 
                body);
        }
        
        /// <summary>
        /// Creates a new storyboard target declaration.
        /// </summary>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                new UvssKeyword(SyntaxKind.TargetKeyword),
                null, 
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration.
        /// </summary>
        /// <param name="typeName">The storyboard target's targeted type.</param>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(
            String typeName,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                new UvssKeyword(SyntaxKind.TargetKeyword),
                new UvssIdentifier(typeName), 
                null, 
                body);
        }

        /// <summary>
        /// Creates a new storyboard target declaration.
        /// </summary>
        /// <param name="typeName">The storyboard target's targeted type.</param>
        /// <param name="selector">The storyboard target's selector.</param>
        /// <param name="body">The storyboard target's body.</param>
        /// <returns>The <see cref="UvssStoryboardTargetSyntax"/> instance that was created.</returns>
        public static UvssStoryboardTargetSyntax StoryboardTarget(
            String typeName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssBlockSyntax body)
        {
            return new UvssStoryboardTargetSyntax(
                new UvssKeyword(SyntaxKind.TargetKeyword),
                new UvssIdentifier(typeName), 
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
            return new UvssStoryboardTargetSyntax(
                targetKeyword, 
                typeNameToken, 
                selector, 
                body);
        }
        
        /// <summary>
        /// Creates a new animation declaration.
        /// </summary>
        /// <param name="propertyName">The animation's property name.</param>
        /// <param name="body">The animation's body.</param>
        /// <returns>The <see cref="UvssAnimationSyntax"/> instance that was created.</returns>
        public static UvssAnimationSyntax Animation(
            UvssPropertyNameSyntax propertyName,
            UvssBlockSyntax body)
        {
            return Animation(
                propertyName, 
                null, 
                body);
        }

        /// <summary>
        /// Creates a new animation declaration.
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
                new UvssKeyword(SyntaxKind.AnimationKeyword),
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
            return new UvssAnimationSyntax(
                animationKeyword, 
                propertyName, 
                navigationExpression, 
                body);
        }
        
        /// <summary>
        /// Creates a new animation keyframe declaration.
        /// </summary>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        /// <param name="value">The keyframe's property value.</param>
        /// <returns>The <see cref="UvssAnimationKeyframeSyntax"/> instance that was created.</returns>
        public static UvssAnimationKeyframeSyntax AnimationKeyframe(
            Int32 time, 
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssAnimationKeyframeSyntax(
                new UvssKeyword(SyntaxKind.KeyframeKeyword),
                Number(time),
                null, 
                value);
        }

        /// <summary>
        /// Creates a new animation keyframe declaration.
        /// </summary>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        /// <param name="easing">The keyframe's easing name.</param>
        /// <param name="value">The keyframe's property value.</param>
        /// <returns>The <see cref="UvssAnimationKeyframeSyntax"/> instance that was created.</returns>
        public static UvssAnimationKeyframeSyntax AnimationKeyframe(
            Int32 time, 
            String easing, 
            UvssPropertyValueWithBracesSyntax value)
        {
            return new UvssAnimationKeyframeSyntax(
                new UvssKeyword(SyntaxKind.KeyframeKeyword),
                Number(time),
                (easing == null) ? null : new UvssIdentifier(easing), 
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
            return new UvssAnimationKeyframeSyntax(
                keyframeKeyword, 
                timeToken, 
                easingToken, 
                value);
        }
        
        /// <summary>
        /// Creates a new navigation expression.
        /// </summary>
        /// <param name="propertyName">The property name of the navigation target.</param>
        /// <param name="typeName">The name of the type which is being converted to by the expression.</param>
        /// <returns>The <see cref="UvssNavigationExpressionSyntax"/> instance that was created.</returns>
        public static UvssNavigationExpressionSyntax NavigationExpression(
            UvssPropertyNameSyntax propertyName, 
            String typeName)
        {
            return new UvssNavigationExpressionSyntax(
                new UvssPunctuation(SyntaxKind.PipeToken),
                propertyName,
                new UvssKeyword(SyntaxKind.AsKeyword),
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
            return new UvssNavigationExpressionSyntax(
                pipeToken,
                propertyName, 
                asKeyword, 
                typeNameToken);
        }
    }
}
