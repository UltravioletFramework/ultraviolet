using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for rewriting a UVSS syntax tree.
    /// </summary>
    public class SyntaxRewriter : SyntaxVisitor
    {
        /// <summary>
        /// Visits the items in the specified syntax list.
        /// </summary>
        /// <typeparam name="TNode">The type of node in the syntax list.</typeparam>
        /// <param name="list">The list to visit.</param>
        /// <returns>The list which will replace the visited list in the syntax tree.</returns>
        public SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list)
            where TNode : SyntaxNode
        {
            var alternate = default(SyntaxListBuilder<TNode>);

            var i = 0;
            var n = list.Count;

            while (i < n)
            {
                var item = list[i];
                var visited = ((TNode)Visit(item));
                if (item != visited && alternate.IsNull)
                {
                    alternate = new SyntaxListBuilder<TNode>(n);
                    alternate.AddRange(list, 0, i);
                }

                if (!alternate.IsNull)
                {
                    if (visited != null && visited.Kind != SyntaxKind.None)
                        alternate.Add(visited);
                }

                i++;
            }

            return alternate.IsNull ? list : alternate.ToList();
        }

        /// <summary>
        /// Visits the items in the specified separated syntax list.
        /// </summary>
        /// <typeparam name="TNode">The type of node in the syntax list.</typeparam>
        /// <param name="list">The list to visit.</param>
        /// <returns>The list which will replace the visited list in the syntax tree.</returns>
        public SeparatedSyntaxList<TNode> VisitSeparatedList<TNode>(SeparatedSyntaxList<TNode> list)
            where TNode : SyntaxNode
        {
            var alternate = default(SeparatedSyntaxListBuilder<TNode>);

            var i = 0;
            var itemCount = list.Count;
            var separatorCount = list.SeparatorCount;

            while (i < itemCount)
            {
                var item = list[i];
                var visitedItem = Visit(item);

                var separator = default(SyntaxToken);
                var visitedSeparator = default(SyntaxToken);

                if (i < separatorCount)
                {
                    separator = list.GetSeparator(i);
                    visitedSeparator = (SyntaxToken)Visit(separator);
                }

                if ((item != visitedItem || separator != visitedSeparator) && alternate.IsNull)
                {
                    alternate = new SeparatedSyntaxListBuilder<TNode>(itemCount);
                    alternate.AddRange(list, i);
                }

                if (!alternate.IsNull)
                {
                    if (visitedItem != null && visitedItem.Kind != SyntaxKind.None)
                    {
                        alternate.Add(((TNode)visitedItem));
                        if (visitedSeparator != null)
                        {
                            alternate.AddSeparator(visitedSeparator);
                        }
                    }
                    else if (i >= separatorCount && alternate.Count > 0)
                    {
                        alternate.RemoveLast();
                    }
                }

                i++;
            }

            return alternate.IsNull ? list : alternate.ToList();
        }
        
        /// <inheritdoc/>
        public override SyntaxNode VisitAnimation(UvssAnimationSyntax node)
        {
            var unchanged = true;

            var newAnimationKeyword = (SyntaxToken)Visit(node.AnimationKeyword);
            if (newAnimationKeyword != node.AnimationKeyword)
                unchanged = false;

            var newPropertyName = (UvssPropertyNameSyntax)Visit(node.PropertyName);
            if (newPropertyName != node.PropertyName)
                unchanged = false;

            var newNavigationExpression = (UvssNavigationExpressionSyntax)Visit(node.NavigationExpression);
            if (newNavigationExpression != node.NavigationExpression)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssAnimationSyntax(
                newAnimationKeyword, 
                newPropertyName, 
                newNavigationExpression, 
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitAnimationKeyframe(UvssAnimationKeyframeSyntax node)
        {
            var unchanged = true;

            var newKeyframeKeyword = (SyntaxToken)Visit(node.KeyframeKeyword);
            if (newKeyframeKeyword != node.KeyframeKeyword)
                unchanged = false;

            var newTimeToken = (SyntaxToken)Visit(node.TimeToken);
            if (newTimeToken != node.TimeToken)
                unchanged = false;

            var newEasingToken = (SyntaxToken)Visit(node.EasingToken);
            if (newEasingToken != node.EasingToken)
                unchanged = false;

            var newValue = (UvssPropertyValueWithBracesSyntax)Visit(node.Value);
            if (newValue != node.Value)
                unchanged = false;

            return unchanged ? node : new UvssAnimationKeyframeSyntax(
                newKeyframeKeyword,
                newTimeToken,
                newEasingToken,
                newValue);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitBlock(UvssBlockSyntax node)
        {
            var unchanged = true;

            var newOpenCurlyBraceToken = (SyntaxToken)Visit(node.OpenCurlyBraceToken);
            if (newOpenCurlyBraceToken != node.OpenCurlyBraceToken)
                unchanged = false;

            var newContent = VisitList(node.Content);
            if (newContent.Node != node.Content.Node)
                unchanged = false;

            var newCloseCurlyBraceToken = (SyntaxToken)Visit(node.CloseCurlyBraceToken);
            if (newCloseCurlyBraceToken != node.CloseCurlyBraceToken)
                unchanged = false;

            return unchanged ? node : new UvssBlockSyntax(
                newOpenCurlyBraceToken,
                newContent,
                newCloseCurlyBraceToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitDocument(UvssDocumentSyntax node)
        {
            var unchanged = true;

            var newContent = VisitList(node.Content);
            if (newContent.Node != node.Content.Node)
                unchanged = false;

            var newEndOfFileToken = (SyntaxToken)Visit(node.EndOfFileToken);
            if (newEndOfFileToken != node.EndOfFileToken)
                unchanged = false;

            return unchanged ? node : new UvssDocumentSyntax(
                newContent,
                newEndOfFileToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitEventName(UvssEventNameSyntax node)
        {
            var unchanged = true;

            var newAttachedEventOwnerNameToken = ((SyntaxToken)Visit(node.AttachedEventOwnerNameToken));
            if (newAttachedEventOwnerNameToken != node.AttachedEventOwnerNameToken)
                unchanged = false;

            var newPeriodToken = (SyntaxToken)Visit(node.PeriodToken);
            if (newPeriodToken != node.PeriodToken)
                unchanged = false;

            var newEventNameToken = (SyntaxToken)Visit(node.EventNameToken);
            if (newEventNameToken != node.EventNameToken)
                unchanged = false;

            return unchanged ? node : new UvssEventNameSyntax(
                newAttachedEventOwnerNameToken,
                newPeriodToken,
                newEventNameToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitEventTrigger(UvssEventTriggerSyntax node)
        {
            var unchanged = true;

            var newTriggerKeyword = (SyntaxToken)Visit(node.TriggerKeyword);
            if (newTriggerKeyword != node.TriggerKeyword)
                unchanged = false;

            var newEventKeyword = (SyntaxToken)Visit(node.EventKeyword);
            if (newEventKeyword != node.EventKeyword)
                unchanged = false;

            var newEventName = (UvssEventNameSyntax)Visit(node.EventName);
            if (newEventName != node.EventName)
                unchanged = false;

            var newArgumentList = (UvssEventTriggerArgumentList)Visit(node.ArgumentList);
            if (newArgumentList != node.ArgumentList)
                unchanged = false;

            var newQualifierToken = (SyntaxToken)Visit(node.QualifierToken);
            if (newQualifierToken != node.QualifierToken)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssEventTriggerSyntax(
                newTriggerKeyword,
                newEventKeyword,
                newEventName,
                newArgumentList,
                newQualifierToken,
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitEventTriggerArgumentList(UvssEventTriggerArgumentList node)
        {
            var unchanged = true;

            var newOpenParenToken = (SyntaxToken)Visit(node.OpenParenToken);
            if (newOpenParenToken != node.OpenParenToken)
                unchanged = false;

            var newArgumentList = VisitSeparatedList(node.ArgumentList);
            if (newArgumentList.Node != node.ArgumentList.Node)
                unchanged = false;

            var newCloseParentToken = (SyntaxToken)Visit(node.CloseParenToken);
            if (newCloseParentToken != node.CloseParenToken)
                unchanged = false;

            return unchanged ? node : new UvssEventTriggerArgumentList(
                newOpenParenToken,
                newArgumentList,
                newCloseParentToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitNavigationExpression(UvssNavigationExpressionSyntax node)
        {
            var unchanged = true;

            var newPipeToken = (SyntaxToken)Visit(node.PipeToken);
            if (newPipeToken != node.PipeToken)
                unchanged = false;

            var newPropertyName = (UvssPropertyNameSyntax)Visit(node.PropertyName);
            if (newPropertyName != node.PropertyName)
                unchanged = false;

            var newAsKeyword = (SyntaxToken)Visit(node.AsKeyword);
            if (newAsKeyword != node.AsKeyword)
                unchanged = false;

            var newTypeNameToken = (SyntaxToken)Visit(node.TypeNameToken);
            if (newTypeNameToken != node.TypeNameToken)
                unchanged = false;

            return unchanged ? node : new UvssNavigationExpressionSyntax(
                newPipeToken,
                newPropertyName,
                newAsKeyword,
                newTypeNameToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPlaySfxTriggerAction(UvssPlaySfxTriggerActionSyntax node)
        {
            var unchanged = true;

            var newPlaySfxKeyword = (SyntaxToken)Visit(node.PlaySfxKeyword);
            if (newPlaySfxKeyword != node.PlaySfxKeyword)
                unchanged = false;

            var newValue = (UvssPropertyValueWithBracesSyntax)Visit(node.Value);
            if (newValue != node.Value)
                unchanged = false;

            return unchanged ? node : new UvssPlaySfxTriggerActionSyntax(
                newPlaySfxKeyword,
                newValue);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPlayStoryboardTriggerAction(UvssPlayStoryboardTriggerActionSyntax node)
        {
            var unchanged = true;

            var newPlayStoryboardKeyword = (SyntaxToken)Visit(node.PlayStoryboardKeyword);
            if (newPlayStoryboardKeyword != node.PlayStoryboardKeyword)
                unchanged = false;

            var newSelector = (UvssSelectorWithParenthesesSyntax)Visit(node.Selector);
            if (newSelector != node.Selector)
                unchanged = false;

            var newValue = (UvssPropertyValueWithBracesSyntax)Visit(node.Value);
            if (newValue != node.Value)
                unchanged = false;

            return unchanged ? node : new UvssPlayStoryboardTriggerActionSyntax(
                newPlayStoryboardKeyword,
                newSelector,
                newValue);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyName(UvssPropertyNameSyntax node)
        {
            var unchanged = true;

            var newAttachedPropertyOwnerNameToken = (SyntaxToken)Visit(node.AttachedPropertyOwnerNameToken);
            if (newAttachedPropertyOwnerNameToken != node.AttachedPropertyOwnerNameToken)
                unchanged = false;

            var newPeriodToken = (SyntaxToken)Visit(node.PeriodToken);
            if (newPeriodToken != node.PeriodToken)
                unchanged = false;

            var newPropertyNameToken = (SyntaxToken)Visit(node.PropertyNameToken);
            if (newPropertyNameToken != node.PropertyNameToken)
                unchanged = false;

            return unchanged ? node : new UvssPropertyNameSyntax(
                newAttachedPropertyOwnerNameToken,
                newPeriodToken,
                newPropertyNameToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyTrigger(UvssPropertyTriggerSyntax node)
        {
            var unchanged = true;

            var newTriggerKeyword = (SyntaxToken)Visit(node.TriggerKeyword);
            if (newTriggerKeyword != node.TriggerKeyword)
                unchanged = false;

            var newPropertyKeyword = (SyntaxToken)Visit(node.PropertyKeyword);
            if (newPropertyKeyword != node.PropertyKeyword)
                unchanged = false;

            var newConditionList = VisitSeparatedList(node.Conditions);
            if (newConditionList.Node != node.Conditions.Node)
                unchanged = false;

            var newQualifierToken = (SyntaxToken)Visit(node.QualifierToken);
            if (newQualifierToken != node.QualifierToken)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssPropertyTriggerSyntax(
                newTriggerKeyword,
                newPropertyKeyword,
                newConditionList,
                newQualifierToken,
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyTriggerCondition(UvssPropertyTriggerConditionSyntax node)
        {
            var unchanged = true;

            var newPropertyName = (UvssPropertyNameSyntax)Visit(node.PropertyName);
            if (newPropertyName != node.PropertyName)
                unchanged = false;

            var newComparisonOperatorToken = (SyntaxToken)Visit(node.ComparisonOperatorToken);
            if (newComparisonOperatorToken != node.ComparisonOperatorToken)
                unchanged = false;

            var newValue = (UvssPropertyValueWithBracesSyntax)Visit(node.PropertyValue);
            if (newValue != node.PropertyValue)
                unchanged = false;

            return unchanged ? node : new UvssPropertyTriggerConditionSyntax(
                newPropertyName,
                newComparisonOperatorToken,
                newValue);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyValue(UvssPropertyValueSyntax node)
        {
            var unchanged = true;

            var newContentToken = (SyntaxToken)Visit(node.ContentToken);
            if (newContentToken != node.ContentToken)
                unchanged = false;

            return unchanged ? node : new UvssPropertyValueSyntax(
                newContentToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPropertyValueWithBraces(UvssPropertyValueWithBracesSyntax node)
        {
            var unchanged = true;

            var newOpenCurlyBraceToken = (SyntaxToken)Visit(node.OpenCurlyBrace);
            if (newOpenCurlyBraceToken != node.OpenCurlyBrace)
                unchanged = false;

            var newContentToken = (SyntaxToken)Visit(node.ContentToken);
            if (newContentToken != node.ContentToken)
                unchanged = false;

            var newCloseCurlyBraceToken = (SyntaxToken)Visit(node.CloseCurlyBrace);
            if (newCloseCurlyBraceToken != node.CloseCurlyBrace)
                unchanged = false;

            return unchanged ? node : new UvssPropertyValueWithBracesSyntax(
                newOpenCurlyBraceToken,
                newContentToken,
                newCloseCurlyBraceToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitPseudoClass(UvssPseudoClassSyntax node)
        {
            var unchanged = true;

            var newColonToken = (SyntaxToken)Visit(node.ColonToken);
            if (newColonToken != node.ColonToken)
                unchanged = false;

            var newClassNameToken = (SyntaxToken)Visit(node.ClassNameToken);
            if (newClassNameToken != node.ClassNameToken)
                unchanged = false;

            return unchanged ? node : new UvssPseudoClassSyntax(
                newColonToken,
                newClassNameToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitRule(UvssRuleSyntax node)
        {
            var unchanged = true;

            var newPropertyName = (UvssPropertyNameSyntax)Visit(node.PropertyName);
            if (newPropertyName != node.PropertyName)
                unchanged = false;

            var newColonToken = (SyntaxToken)Visit(node.ColonToken);
            if (newColonToken != node.ColonToken)
                unchanged = false;

            var newValue = (UvssPropertyValueSyntax)Visit(node.Value);
            if (newValue != node.Value)
                unchanged = false;

            var newQualifierToken = (SyntaxToken)Visit(node.QualifierToken);
            if (newQualifierToken != node.QualifierToken)
                unchanged = false;

            var newSemiColonToken = (SyntaxToken)Visit(node.SemiColonToken);
            if (newSemiColonToken != node.SemiColonToken)
                unchanged = false;

            return unchanged ? node : new UvssRuleSyntax(
                newPropertyName,
                newColonToken,
                newValue,
                newQualifierToken,
                newSemiColonToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitRuleSet(UvssRuleSetSyntax node)
        {
            var unchanged = true;

            var newSelectorList = VisitSeparatedList(node.Selectors);
            if (newSelectorList.Node != node.Selectors.Node)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssRuleSetSyntax(
                newSelectorList,
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitSelector(UvssSelectorSyntax node)
        {
            var unchanged = true;

            var newComponents = VisitList(node.Components);
            if (newComponents.Node != node.Components.Node)
                unchanged = false;

            var newNavigationExpression = (UvssNavigationExpressionSyntax)Visit(node.NavigationExpression);
            if (newNavigationExpression != node.NavigationExpression)
                unchanged = false;

            return unchanged ? node : new UvssSelectorSyntax(
                newComponents,
                newNavigationExpression);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitSelectorPart(UvssSelectorPartSyntax node)
        {
            var unchanged = true;

            var newSubPartsList = VisitList(node.SubParts);
            if (newSubPartsList.Node != node.SubParts.Node)
                unchanged = false;

            var newPseudoClass = (UvssPseudoClassSyntax)Visit(node.PseudoClass);
            if (newPseudoClass != node.PseudoClass)
                unchanged = false;

            return unchanged ? node : new UvssSelectorPartSyntax(
                newSubPartsList,
                newPseudoClass);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitSelectorSubPart(UvssSelectorSubPartSyntax node)
        {
            var unchanged = true;

            var newLeadingQualifierToken = (SyntaxToken)Visit(node.LeadingQualifierToken);
            if (newLeadingQualifierToken != node.LeadingQualifierToken)
                unchanged = false;

            var newTextToken = (SyntaxToken)Visit(node.TextToken);
            if (newTextToken != node.TextToken)
                unchanged = false;

            var newTrailingQualifierToken = (SyntaxToken)Visit(node.TrailingQualifierToken);
            if (newTrailingQualifierToken != node.TrailingQualifierToken)
                unchanged = false;

            return unchanged ? node : new UvssSelectorSubPartSyntax(
                newLeadingQualifierToken,
                newTextToken,
                newTrailingQualifierToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitSelectorWithParentheses(UvssSelectorWithParenthesesSyntax node)
        {
            var unchanged = true;

            var newOpenParenToken = (SyntaxToken)Visit(node.OpenParenToken);
            if (newOpenParenToken != node.OpenParenToken)
                unchanged = false;

            var newSelector = (UvssSelectorSyntax)Visit(node.Selector);
            if (newSelector != node.Selector)
                unchanged = false;

            var newCloseParenToken = (SyntaxToken)Visit(node.CloseParenToken);
            if (newCloseParenToken != node.CloseParenToken)
                unchanged = false;

            return unchanged ? node : new UvssSelectorWithParenthesesSyntax(
                newOpenParenToken,
                newSelector,
                newCloseParenToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitSetTriggerAction(UvssSetTriggerActionSyntax node)
        {
            var unchanged = true;

            var newSetKeyword = (SyntaxToken)Visit(node.SetKeyword);
            if (newSetKeyword != node.SetKeyword)
                unchanged = false;

            var newPropertyName = (UvssPropertyNameSyntax)Visit(node.PropertyName);
            if (newPropertyName != node.PropertyName)
                unchanged = false;

            var newSelector = (UvssSelectorWithParenthesesSyntax)Visit(node.Selector);
            if (newSelector != node.Selector)
                unchanged = false;

            var newValue = (UvssPropertyValueWithBracesSyntax)Visit(node.Value);
            if (newValue != node.Value)
                unchanged = false;

            return unchanged ? node : new UvssSetTriggerActionSyntax(
                newSetKeyword,
                newPropertyName,
                newSelector,
                newValue);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitStoryboard(UvssStoryboardSyntax node)
        {
            var unchanged = true;

            var newAtSignToken = (SyntaxToken)Visit(node.AtSignToken);
            if (newAtSignToken != node.AtSignToken)
                unchanged = false;

            var newNameToken = (SyntaxToken)Visit(node.NameToken);
            if (newNameToken != node.NameToken)
                unchanged = false;

            var newLoopToken = (SyntaxToken)Visit(node.LoopToken);
            if (newLoopToken != node.LoopToken)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssStoryboardSyntax(
                newAtSignToken,
                newNameToken,
                newLoopToken,
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitStoryboardTarget(UvssStoryboardTargetSyntax node)
        {
            var unchanged = true;

            var newTargetKeyword = (SyntaxToken)Visit(node.TargetKeyword);
            if (newTargetKeyword != node.TargetKeyword)
                unchanged = false;

            var newTypeNameToken = (SyntaxToken)Visit(node.TypeNameToken);
            if (newTypeNameToken != node.TypeNameToken)
                unchanged = false;

            var newSelector = (UvssSelectorWithParenthesesSyntax)Visit(node.Selector);
            if (newSelector != node.Selector)
                unchanged = false;

            var newBody = (UvssBlockSyntax)Visit(node.Body);
            if (newBody != node.Body)
                unchanged = false;

            return unchanged ? node : new UvssStoryboardTargetSyntax(
                newTargetKeyword,
                newTypeNameToken,
                newSelector,
                newBody);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitTransition(UvssTransitionSyntax node)
        {
            var unchanged = true;

            var newTransitionKeyword = (SyntaxToken)Visit(node.TransitionKeyword);
            if (newTransitionKeyword != node.TransitionKeyword)
                unchanged = false;

            var newArgumentList = (UvssTransitionArgumentListSyntax)Visit(node.ArgumentList);
            if (newArgumentList != node.ArgumentList)
                unchanged = false;

            var newColonToken = (SyntaxToken)Visit(node.ColonToken);
            if (newColonToken != node.ColonToken)
                unchanged = false;

            var newStoryboardNameToken = (SyntaxToken)Visit(node.StoryboardNameToken);
            if (newStoryboardNameToken != node.StoryboardNameToken)
                unchanged = false;

            var newQualifierToken = (SyntaxToken)Visit(node.QualifierToken);
            if (newQualifierToken != node.QualifierToken)
                unchanged = false;

            var newSemiColonToken = (SyntaxToken)Visit(node.SemiColonToken);
            if (newSemiColonToken != node.SemiColonToken)
                unchanged = false;

            return unchanged ? node : new UvssTransitionSyntax(
                newTransitionKeyword,
                newArgumentList,
                newColonToken,
                newStoryboardNameToken,
                newQualifierToken,
                newSemiColonToken);
        }

        /// <inheritdoc/>
        public override SyntaxNode VisitTransitionArgumentList(UvssTransitionArgumentListSyntax node)
        {
            var unchanged = true;

            var newOpenParenToken = (SyntaxToken)Visit(node.OpenParenToken);
            if (newOpenParenToken != node.OpenParenToken)
                unchanged = false;

            var newArguments = VisitSeparatedList(node.Arguments);
            if (newArguments.Node != node.Arguments.Node)
                unchanged = false;

            var newCloseParenToken = (SyntaxToken)Visit(node.CloseParenToken);
            if (newCloseParenToken != node.CloseParenToken)
                unchanged = false;

            return unchanged ? node : new UvssTransitionArgumentListSyntax(
                newOpenParenToken,
                newArguments,
                newCloseParenToken);
        }
    }
}
