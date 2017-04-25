using System;
using Microsoft.VisualStudio.Text.Classification;
using Ultraviolet.Presentation.Uvss;
using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.VisualStudio.Uvss.Classification
{
    /// <summary>
    /// Represents a method which is invoked when <see cref="UvssClassifierVisitor"/> marks
    /// a span of text for classification.
    /// </summary>
    /// <param name="start">The index of the first character in the span.</param>
    /// <param name="width">The number of characters in the span.</param>
    /// <param name="type">The classification type assigned to the span.</param>
    /// <param name="kind">The kind of node which is being classified.</param>
    public delegate void ClassifierAction(Int32 start, Int32 width, IClassificationType type, SyntaxKind kind);

    /// <summary>
    /// Represents a syntax tree visitor which provides classification spans.
    /// </summary>
    public class UvssClassifierVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssClassifierVisitor"/> class.
        /// </summary>
        /// <param name="registry">The classification type registry service.</param>
        /// <param name="classifier">The action which is called when a span is classified.</param>
        public UvssClassifierVisitor(IClassificationTypeRegistryService registry, ClassifierAction classifier)
        {
            this.typeUvssComment = registry.GetClassificationType("UvssComment");
            this.typeUvssNumber = registry.GetClassificationType("UvssNumber");
            this.typeUvssKeyword = registry.GetClassificationType("UvssKeyword");
            this.typeUvssSelector = registry.GetClassificationType("UvssSelector");
            this.typeUvssPropertyName = registry.GetClassificationType("UvssPropertyName");
            this.typeUvssPropertyValue = registry.GetClassificationType("UvssPropertyValue");
            this.typeUvssStoryboard = registry.GetClassificationType("UvssStoryboard");
            this.typeUvssTypeName = registry.GetClassificationType("UvssTypeName");
            this.typeUvssDirective = registry.GetClassificationType("UvssDirective");

            this.classifier = classifier;
        }

        /// <summary>
        /// Visits the specified syntax node.
        /// </summary>
        /// <param name="node">The syntax node to visit.</param>
        public void Visit(SyntaxNode node)
        {
            if (node == null)
                return;

            if (node is SyntaxToken)
                Visit(node.GetLeadingTrivia());

            switch (node.Kind)
            {
                case SyntaxKind.SingleLineCommentTrivia:
                case SyntaxKind.MultiLineCommentTrivia:
                    VisitCommentTrivia((StructurelessSyntaxTrivia)node);
                    break;

                case SyntaxKind.NumberToken:
                    VisitNumber((SyntaxToken)node);
                    break;

                case SyntaxKind.AnimationKeyword:
                case SyntaxKind.AsKeyword:
                case SyntaxKind.EventKeyword:
                case SyntaxKind.HandledKeyword:
                case SyntaxKind.ImportantKeyword:
                case SyntaxKind.KeyframeKeyword:
                case SyntaxKind.PlaySfxKeyword:
                case SyntaxKind.PlayStoryboardKeyword:
                case SyntaxKind.PropertyKeyword:
                case SyntaxKind.SetHandledKeyword:
                case SyntaxKind.SetKeyword:
                case SyntaxKind.TargetKeyword:
                case SyntaxKind.TransitionKeyword:
                case SyntaxKind.TriggerKeyword:
                    VisitKeyword((SyntaxToken)node);
                    break;

                case SyntaxKind.Selector:
                    VisitSelector((UvssSelectorSyntax)node);
                    break;

                case SyntaxKind.SelectorPart:
                case SyntaxKind.InvalidSelectorPart:
                    VisitSelectorPart((UvssSelectorPartBaseSyntax)node);
                    break;
                    
                case SyntaxKind.PseudoClass:
                    VisitPseudoClass((UvssPseudoClassSyntax)node);
                    break;

                case SyntaxKind.Rule:
                    VisitRule((UvssRuleSyntax)node);
                    break;

                case SyntaxKind.EventName:
                    VisitEventName((UvssEventNameSyntax)node);
                    break;

                case SyntaxKind.PropertyName:
                    VisitPropertyName((UvssPropertyNameSyntax)node);
                    break;
                    
                case SyntaxKind.PropertyValueToken:
                    VisitPropertyValueToken((SyntaxToken)node);
                    break;

                case SyntaxKind.Storyboard:
                    VisitStoryboard((UvssStoryboardSyntax)node);
                    break;

                case SyntaxKind.StoryboardTarget:
                    VisitStoryboardTarget((UvssStoryboardTargetSyntax)node);
                    break;

                case SyntaxKind.AnimationKeyframe:
                    VisitAnimationKeyframe((UvssAnimationKeyframeSyntax)node);
                    break;

                case SyntaxKind.NavigationExpression:
                    VisitNavigationExpression((UvssNavigationExpressionSyntax)node);
                    break;

                case SyntaxKind.UnknownDirective:
                    VisitUnknownDirective((UvssUnknownDirectiveSyntax)node);
                    break;

                case SyntaxKind.CultureDirective:
                    VisitCultureDirective((UvssCultureDirectiveSyntax)node);
                    break;
            }

            for (int i = 0; i < node.SlotCount; i++)
            {
                var child = node.GetSlot(i);
                if (child != null)
                {
                    Visit(child);
                }
            }

            if (node is SyntaxToken)
                Visit(node.GetTrailingTrivia());
        }

        /// <summary>
        /// Visits a comment trivia node.
        /// </summary>
        /// <param name="trivia">The trivia node to visit.</param>
        private void VisitCommentTrivia(StructurelessSyntaxTrivia trivia)
        {
            Style(trivia, typeUvssComment);
        }

        /// <summary>
        /// Visits a number node.
        /// </summary>
        /// <param name="token">The number node to visit.</param>
        private void VisitNumber(SyntaxToken token)
        {
            Style(token, typeUvssNumber);
        }

        /// <summary>
        /// Visits a keyword token node.
        /// </summary>
        /// <param name="token">The token node to visit.</param>
        private void VisitKeyword(SyntaxToken token)
        {
            Style(token, typeUvssKeyword);
        }

        /// <summary>
        /// Visits a selector node.
        /// </summary>
        /// <param name="selector">The selector node to visit.</param>
        private void VisitSelector(UvssSelectorSyntax selector)
        {
            foreach (var combinator in selector.Combinators)
                Style(combinator, typeUvssSelector);
        }

        /// <summary>
        /// Visits a selector part node.
        /// </summary>
        /// <param name="selectorPart">The selector part node to visit.</param>
        private void VisitSelectorPart(UvssSelectorPartBaseSyntax selectorPart)
        {
            Style(selectorPart, typeUvssSelector);
        }

        /// <summary>
        /// Visits a pseudo class node.
        /// </summary>
        /// <param name="pseudoClass">The pseudo class node to visit.</param>
        private void VisitPseudoClass(UvssPseudoClassSyntax pseudoClass)
        {
            Style(pseudoClass, typeUvssSelector);
        }

        /// <summary>
        /// Visits a rule node.
        /// </summary>
        /// <param name="rule">The rule node to visit.</param>
        private void VisitRule(UvssRuleSyntax rule)
        {
            Style(rule.ColonToken, typeUvssPropertyName);
        }

        /// <summary>
        /// Visits an event name node.
        /// </summary>
        /// <param name="eventName">The event name node to visit.</param>
        private void VisitEventName(UvssEventNameSyntax eventName)
        {
            Style(eventName.AttachedEventOwnerNameIdentifier, typeUvssTypeName);
            Style(eventName.EventNameIdentifier, typeUvssPropertyName);
        }

        /// <summary>
        /// Visits a property name node.
        /// </summary>
        /// <param name="propertyName">The property name node to visit.</param>
        private void VisitPropertyName(UvssPropertyNameSyntax propertyName)
        {
            Style(propertyName.AttachedPropertyOwnerNameIdentifier, typeUvssTypeName);
            Style(propertyName.PropertyNameIdentifier, typeUvssPropertyName);
        }

        /// <summary>
        /// Visits a property value token.
        /// </summary>
        /// <param name="propertyValueToken">The property value token to visit.</param>
        private void VisitPropertyValueToken(SyntaxToken propertyValueToken)
        {
            Style(propertyValueToken, typeUvssPropertyValue);
        }

        /// <summary>
        /// Visits a storyboard declaration node.
        /// </summary>
        /// <param name="storyboard">The storyboard declaration node to visit.</param>
        private void VisitStoryboard(UvssStoryboardSyntax storyboard)
        {
            Style(storyboard.AtSignToken, typeUvssStoryboard);
            Style(storyboard.NameIdentifier, typeUvssStoryboard);
            Style(storyboard.LoopIdentifier, typeUvssStoryboard);
        }

        /// <summary>
        /// Visits a storyboard target node.
        /// </summary>
        /// <param name="storyboardTarget">The storyboard target node to visit.</param>
        private void VisitStoryboardTarget(UvssStoryboardTargetSyntax storyboardTarget)
        {
            for (int i = 0; i < storyboardTarget.Filters.Count; i++)
            {
                var filterNode = storyboardTarget.Filters[i];
                Style(filterNode, typeUvssTypeName);
            }
        }

        /// <summary>
        /// Visits an animation keyframe declaration node..
        /// </summary>
        /// <param name="animationKeyframe">The animation keyframe declaration node to visit.</param>
        private void VisitAnimationKeyframe(UvssAnimationKeyframeSyntax animationKeyframe)
        {
            Style(animationKeyframe.EasingIdentifier, typeUvssKeyword);
        }

        /// <summary>
        /// Visits a navigation expression node.
        /// </summary>
        /// <param name="navigationExpression">The navigation expression node to visit.</param>
        private void VisitNavigationExpression(UvssNavigationExpressionSyntax navigationExpression)
        {
            Style(navigationExpression.TypeNameIdentifier, typeUvssTypeName);
        }

        /// <summary>
        /// Visits an unknown directive node.
        /// </summary>
        /// <param name="unknownDirective">The unknown directive node to visit.</param>
        private void VisitUnknownDirective(UvssUnknownDirectiveSyntax unknownDirective)
        {
            Style(unknownDirective.DirectiveToken, typeUvssDirective);
        }

        /// <summary>
        /// Visits a culture directive node.
        /// </summary>
        /// <param name="cultureDirective">The unknown directive node to visit.</param>
        private void VisitCultureDirective(UvssCultureDirectiveSyntax cultureDirective)
        {
            Style(cultureDirective.DirectiveToken, typeUvssDirective);
        }

        /// <summary>
        /// Styles the specified node.
        /// </summary>
        /// <param name="node">The node to style.</param>
        /// <param name="type">The classification type to apply to the node.</param>
        /// <param name="withLeadingTrivia">A value indicating whether to style the node's leading trivia.</param>
        /// <param name="withTrailingTrivia">A value indicating whether to style the node's trailing trivia.</param>
        private void Style(SyntaxNode node, IClassificationType type,
            Boolean withLeadingTrivia = false,
            Boolean withTrailingTrivia = false)
        {
            if (node == null || node.IsMissing || type == null)
                return;

            var start = node.Position + (withLeadingTrivia ? 0 : node.GetLeadingTriviaWidth());
            var width = node.FullWidth - (
                (withLeadingTrivia ? 0 : node.GetLeadingTriviaWidth()) +
                (withTrailingTrivia ? 0 : node.GetTrailingTriviaWidth()));

            classifier(start, width, type, node.Kind);
        }

        // State values.
        private readonly IClassificationType typeUvssComment;
        private readonly IClassificationType typeUvssNumber;
        private readonly IClassificationType typeUvssKeyword;
        private readonly IClassificationType typeUvssSelector;
        private readonly IClassificationType typeUvssPropertyName;
        private readonly IClassificationType typeUvssPropertyValue;
        private readonly IClassificationType typeUvssStoryboard;
        private readonly IClassificationType typeUvssTypeName;
        private readonly IClassificationType typeUvssDirective;
        private readonly ClassifierAction classifier;
    }
}
