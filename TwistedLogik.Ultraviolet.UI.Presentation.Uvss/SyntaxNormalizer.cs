using System;
using System.Linq;
using System.Text;
using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a syntax visitor which normalizes white space.
    /// </summary>
    internal class SyntaxNormalizer : SyntaxRewriter
    {
        /// <summary>
        /// Normalizes the specified node's white space.
        /// </summary>
        /// <typeparam name="TNode">The type of node to normalize.</typeparam>
        /// <param name="node">The node to normalize.</param>
        /// <returns>The normalized node.</returns>
        public static TNode Normalize<TNode>(TNode node) where TNode : SyntaxNode
        {
            var normalizer = new SyntaxNormalizer();
            return (TNode)normalizer.Visit(node);
        }

        /// <inheritdoc/>
        public override SyntaxToken VisitSyntaxToken(SyntaxToken token)
        {
            var current = token;
            var next = GetNextToken(token);
            var indent = GetIndentationLevel(current);
            var changed = false;
            
            var leadingTrivia = RewriteTrivia(token.GetLeadingTrivia(), 0, 0, indent);
            if (leadingTrivia != token.GetLeadingTrivia())
                changed = true;

            var neededLineBreaks = GetTrailingLineBreakCount(current, next);
            var neededSpaces = GetTrailingSpaceCount(current, next);

            var trailingTrivia = RewriteTrivia(token.GetTrailingTrivia(), neededLineBreaks, neededSpaces, indent);
            if (trailingTrivia != token.GetTrailingTrivia())
                changed = true;

            if (changed)
                return new SyntaxToken(token.Kind, token.Text, leadingTrivia, trailingTrivia);

            return base.VisitSyntaxToken(token);
        }

        /// <summary>
        /// Gets the number of trailing line breaks required after the specified token.
        /// </summary>
        /// <param name="current">The token to evaluate.</param>
        /// <param name="next">The next token after the evaluated token.</param>
        /// <returns>The number of trailing line breaks required after the specified token.</returns>
        private Int32 GetTrailingLineBreakCount(SyntaxToken current, SyntaxToken next)
        {
            // Determine our parent's type, making sure to bust through identifiers.
            var currentKind = current.Kind;
            var currentParent = current.Parent;
            if (currentParent is UvssIdentifierBaseSyntax)
                currentParent = currentParent.Parent;

            var currentGrandparent = currentParent?.Parent;

            var nextKind = next?.Kind;
            var nextParent = next?.Parent;
            if (nextParent is UvssIdentifierBaseSyntax)
                nextParent = nextParent.Parent;

            var nextGrandparent = nextParent?.Parent;

            // NEVER insert line breaks if we're the last token in a document.
            if (next == null || nextKind == SyntaxKind.EndOfFileToken)
                return 0;

            // Insert line breaks after opening curly braces...
            if (currentKind == SyntaxKind.OpenCurlyBraceToken)
            {
                // ...if the braces are part of a block.
                if (currentParent is UvssBlockSyntax)
                    return 1;

                return 0;
            }

            // Insert line breaks after closing curly braces...
            if (currentKind == SyntaxKind.CloseCurlyBraceToken)
            {
                // ...UNLESS we're followed by a comma (as in a condition list).
                if (nextKind == SyntaxKind.CommaToken)
                    return 0;

                // ...we need TWO line breaks after every rule set and storyboard.
                if (currentGrandparent is UvssRuleSetSyntax ||
                    currentGrandparent is UvssStoryboardSyntax)
                {
                    var isEndOfBlock = 
                        nextKind == SyntaxKind.CloseCurlyBraceToken && 
                        nextParent is UvssBlockSyntax;
                    return isEndOfBlock ? 1 : 2;
                }

                return 1;
            }

            // Insert line breaks after semi-colons...
            if (currentKind == SyntaxKind.SemiColonToken)
            {
                return 1;
            }

            // Insert line breaks if the NEXT token is an opening curly brace...
            if (nextKind == SyntaxKind.OpenCurlyBraceToken)
            {
                // ...UNLESS we're in an animation keyframe or a property trigger condition.
                if (currentParent is UvssAnimationKeyframeSyntax ||
                    currentParent is UvssPropertyTriggerConditionSyntax)
                {
                    return 0;
                }

                // ...OR this is a trigger action property value.
                if (nextParent is UvssPropertyValueWithBracesSyntax &&
                    nextGrandparent is UvssTriggerActionBaseSyntax)
                {
                    return 0;
                }

                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Gets the number of trailing spaces required after the specified token.
        /// </summary>
        /// <param name="current">The token to evaluate.</param>
        /// <param name="next">The next token after the evaluated token.</param>
        /// <returns>The number of trailing spaces required after the specified token.</returns>
        private Int32 GetTrailingSpaceCount(SyntaxToken current, SyntaxToken next)
        {
            // Determine our parent's type, making sure to bust through identifiers.
            var currentKind = current.Kind;
            var currentParent = current.Parent;
            if (currentParent is UvssIdentifierBaseSyntax)
                currentParent = currentParent.Parent;

            var currentGrandparent = currentParent?.Parent;

            var nextKind = next?.Kind;
            var nextParent = next?.Parent;
            if (nextParent is UvssIdentifierBaseSyntax)
                nextParent = nextParent.Parent;

            var nextGrandparent = nextParent?.Parent;

            // NEVER insert a space if we're the last token in a document.
            if (next == null || nextKind == SyntaxKind.EndOfFileToken)
                return 0;

            // NEVER insert a space if we're the last token in a block.
            if (next.Kind == SyntaxKind.CloseParenthesesToken && next.Parent is UvssBlockSyntax)
                return 0;
                                    
            // ALWAYS insert a space after certain symbols...
            if (currentKind == SyntaxKind.CommaToken)
                return 1;

            // NEVER insert a space after certain symbols...
            if (currentKind == SyntaxKind.AtSignToken ||
                currentKind == SyntaxKind.OpenParenthesesToken ||
                currentKind == SyntaxKind.OpenBracketToken)
            {
                return 0;
            }

            // ALWAYS insert a space BEFORE certain symbols...
            if (next.Kind == SyntaxKind.PipeToken)
            {
                return 1;
            }

            // NEVER insert a space BEFORE certain symbols...
            if (next.Kind == SyntaxKind.CommaToken ||
                next.Kind == SyntaxKind.SemiColonToken ||
                next.Kind == SyntaxKind.CloseParenthesesToken ||
                next.Kind == SyntaxKind.CloseBracketToken)
            {
                return 0;
            }

            // If we're inside of a selector part...
            if (currentParent is UvssSelectorPartTypeSyntax ||
                currentParent is UvssSelectorPartNameSyntax ||
                currentParent is UvssSelectorPartClassSyntax ||
                currentParent is UvssPseudoClassSyntax)
            {
                // DO insert a space if we're the LAST token in the selector part...
                var selectorPart = FindSelectorPart(current);
                if (selectorPart?.GetLastToken() == current)
                {
                    // UNLESS we're the last part in the selector!
                    var selector = FindSelector(selectorPart);
                    if (selector?.Parts?.Last() == selectorPart)
                        return 0;

                    return 1;
                }
                return 0;
            }

            // If we're inside of a property or event name...
            if (currentParent is UvssPropertyNameSyntax || currentParent is UvssEventNameSyntax)
            {
                // ...DO NOT surround periods with spaces.
                if (currentKind == SyntaxKind.PeriodToken || next.Kind == SyntaxKind.PeriodToken)
                    return 0;
            }

            // If we're inside of a pseudo-class...
            if (currentParent is UvssPseudoClassSyntax)
            {
                // ...DO NOT insert spaces after the leading qualifier.
                if (currentKind == SyntaxKind.ColonToken)
                    return 0;
            }

            // If we're inside of a selector...
            if (currentParent is UvssSelectorPartBaseSyntax || currentParent is UvssPseudoClassSyntax)
            {
                // ...DO NOT insert spaces after leading qualifiers.
                if (currentKind == SyntaxKind.PeriodToken ||
                    currentKind == SyntaxKind.HashToken)
                {
                    return 0;
                }

                // ...DO NOT insert spaces after pseudo-class qualifiers.
                if (currentKind == SyntaxKind.SemiColonToken)
                    return 0;
                
                // ...DO NOT insert spaces between selector sub-parts within the same selector part.
                if (currentParent is UvssSelectorPartSyntax && nextGrandparent == currentGrandparent)
                    return 0;

                // ...DO NOT insert spaces before a pseudo-class.
                if (next.Kind == SyntaxKind.ColonToken && nextParent is UvssPseudoClassSyntax)
                    return 0;
            }

            // DO NOT insert spaces before the colons that follow property names in rules.
            if (next.Kind == SyntaxKind.ColonToken && nextParent is UvssRuleSyntax)
                return 0;

            // DO NOT insert spaces before the colons in transitions.
            if (next.Kind == SyntaxKind.ColonToken && nextParent is UvssTransitionSyntax)
                return 0;

            return 1;
        }

        /// <summary>
        /// Gets a value indicating whether the specified trivia ends with a line break.
        /// </summary>
        private static Boolean TriviaEndsWithLineBreak(SyntaxTrivia trivia)
        {
            if (trivia == null)
                return false;

            return trivia.ToFullString().EndsWith(Environment.NewLine);
        }

        /// <summary>
        /// Gets a value indicating whether the specified trivia ends with a space.
        /// </summary>
        private static Boolean TriviaEndsWithSpace(SyntaxTrivia trivia)
        {
            if (trivia == null)
                return false;

            return trivia.ToFullString().EndsWith(" ");
        }

        /// <summary>
        /// Gets the indentation level of the specified token.
        /// </summary>
        private static Int32 GetIndentationLevel(SyntaxToken token)
        {
            var level = 0;

            var current = token.Parent;
            if (current is UvssBlockSyntax)
            {
                var block = (UvssBlockSyntax)current;
                if (block.OpenCurlyBraceToken == token)
                    level = 1;

                current = current.Parent;
            }

            while (current != null)
            {
                if (current is UvssBlockSyntax)
                    level++;

                current = current.Parent;
            }

            var next = GetNextToken(token);
            if (next != null && next.Parent is UvssBlockSyntax && ((UvssBlockSyntax)next.Parent).CloseCurlyBraceToken == next)
            {
                level--;
            }

            return level;
        }

        /// <summary>
        /// Gets the slot index of the specified token within its parent node.
        /// </summary>
        private static Int32 GetSlotIndexWithinParent(SyntaxNode token)
        {
            var parent = token.Parent;
            if (parent != null)
            {
                for (int i = 0; i < parent.SlotCount; i++)
                {
                    if (parent.GetSlot(i) == token)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Rewrites the specified trivia to include the specified number of trailing
        /// line breaks and spaces, as well as any other additional trivia which are
        /// deemed to be required.
        /// </summary>
        private static SyntaxNode RewriteTrivia(SyntaxNode trivia,
            Int32 neededLineBreaks, Int32 neededSpaces, Int32 indentation)
        {
            var needsLineBreakAfterExistingTrivia = false;
            var needsSpaceAfterExistingTrivia = false;

            if (trivia != null)
            {
                if (trivia.IsList)
                {
                    for (int i = 0; i < trivia.SlotCount; i++)
                    {
                        var item = trivia.GetSlot(i) as SyntaxTrivia;
                        var itemIsLast = (i + 1 == trivia.SlotCount);
                        EvaluateTriviaForRewrite(item,
                            ref needsLineBreakAfterExistingTrivia, ref needsSpaceAfterExistingTrivia, itemIsLast);
                    }
                }
                else
                {
                    EvaluateTriviaForRewrite(trivia as SyntaxTrivia,
                        ref needsLineBreakAfterExistingTrivia, ref needsSpaceAfterExistingTrivia, true);
                }
            }

            var triviaList = default(SyntaxList<SyntaxTrivia>);
            var triviaListBuilder = default(SyntaxListBuilder<SyntaxTrivia>);

            var triviaChanged =
                needsLineBreakAfterExistingTrivia ||
                needsSpaceAfterExistingTrivia ||
                neededLineBreaks > 0 ||
                neededSpaces > 0;

            if (triviaChanged)
            {
                triviaListBuilder = SyntaxListBuilder<SyntaxTrivia>.Create();
                if (trivia != null)
                {
                    triviaList = new SyntaxList<SyntaxTrivia>(trivia);
                    triviaListBuilder.AddRange(triviaList);
                }

                if (needsLineBreakAfterExistingTrivia)
                {
                    triviaListBuilder.Add(GetLineBreakTrivia(1, indentation));
                    neededLineBreaks--;
                    neededSpaces--;
                }
                else
                {
                    if (needsSpaceAfterExistingTrivia)
                    {
                        triviaListBuilder.Add(GetSpaceTrivia(1));
                        neededSpaces--;
                    }
                }

                if (neededLineBreaks > 0)
                {
                    triviaListBuilder.Add(GetLineBreakTrivia(neededLineBreaks, indentation));
                }
                else
                {
                    if (neededSpaces > 0)
                    {
                        triviaListBuilder.Add(GetSpaceTrivia(neededSpaces));
                    }
                }
            }

            return triviaListBuilder.IsNull ? trivia : triviaListBuilder.ToListNode();
        }

        /// <summary>
        /// Creates a new <see cref="SyntaxTrivia"/> representing the specified number of line breaks
        /// followed by the specified number of indentations.
        /// </summary>
        private static SyntaxTrivia GetLineBreakTrivia(Int32 count, Int32 indentation)
        {
            if (count == 1 && indentation == 0)
                return new StructurelessSyntaxTrivia(SyntaxKind.WhitespaceTrivia, Environment.NewLine);

            if (count == 0 && indentation == 1)
                return new StructurelessSyntaxTrivia(SyntaxKind.WhitespaceTrivia, "\t");

            var builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                builder.Append(Environment.NewLine);
            }
            for (int i = 0; i < indentation; i++)
            {
                builder.Append("\t");
            }

            return new StructurelessSyntaxTrivia(SyntaxKind.WhitespaceTrivia, builder.ToString());
        }

        /// <summary>
        /// Creates a new <see cref="SyntaxTrivia"/> representing the specified number of spaces.
        /// </summary>
        private static SyntaxTrivia GetSpaceTrivia(Int32 count)
        {
            return new StructurelessSyntaxTrivia(SyntaxKind.WhitespaceTrivia, new String(' ', count));
        }

        /// <summary>
        /// Gets the token that occurs immediately prior to the specified node.
        /// </summary>
        private static SyntaxToken GetPrevToken(SyntaxNode node)
        {
            var index = GetSlotIndexWithinParent(node);
            if (index < 0)
                return null;

            for (int i = index - 1; i >= 0; i--)
            {
                var sibling = node.Parent.GetSlot(i);
                if (sibling != null)
                {
                    return sibling.GetLastToken();
                }
            }

            return GetPrevToken(node.Parent);
        }

        /// <summary>
        /// Gets the token that occurs immediately after the specified node.
        /// </summary>
        private static SyntaxToken GetNextToken(SyntaxNode node)
        {
            var index = GetSlotIndexWithinParent(node);
            if (index < 0)
                return null;

            for (int i = index + 1; i < node.Parent.SlotCount; i++)
            {
                var sibling = node.Parent.GetSlot(i);
                if (sibling != null)
                {
                    return sibling.GetFirstToken();
                }
            }

            return GetNextToken(node.Parent);
        }

        /// <summary>
        /// Finds the selector which contains the specified node.
        /// </summary>
        private UvssSelectorBaseSyntax FindSelector(SyntaxNode current)
        {
            while (current != null)
            {
                if (current is UvssSelectorBaseSyntax)
                    return (UvssSelectorBaseSyntax)current;

                current = current.Parent;
            }
            return null;
        }

        /// <summary>
        /// Finds the selector part which contains the specified node.
        /// </summary>
        private UvssSelectorPartBaseSyntax FindSelectorPart(SyntaxNode current)
        {
            while (current != null)
            {
                if (current is UvssSelectorPartBaseSyntax)
                    return (UvssSelectorPartBaseSyntax)current;

                current = current.Parent;
            }
            return null;
        }

        /// <summary>
        /// Examines the specified trivia to determine whether it needs any additional trivia
        /// to be added, such as spaces or line breaks.
        /// </summary>
        private static void EvaluateTriviaForRewrite(SyntaxTrivia trivia,
            ref Boolean needsTrailingLineBreak,
            ref Boolean needsTrailingSpace,
            Boolean isLastTrivia)
        {
            if (trivia.Kind == SyntaxKind.SingleLineCommentTrivia)
            {
                if (!TriviaEndsWithLineBreak(trivia) && isLastTrivia)
                    needsTrailingLineBreak = true;
            }
        }
    }
}
