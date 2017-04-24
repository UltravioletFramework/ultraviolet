using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for visiting the nodes of a UVSS syntax tree.
    /// </summary>
    public abstract class SyntaxVisitor
    {
        /// <summary>
        /// Visits the specified node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode Visit(SyntaxNode node)
        {
            return node?.Accept(this);
        }

        /// <summary>
        /// Visits the specified syntax node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSyntaxNode(SyntaxNode node)
        {
            return node;
        }

        /// <summary>
        /// Visits the specified syntax token.
        /// </summary>
        /// <param name="token">The token to visit.</param>
        /// <returns>A token which should replace the visited node, or a reference to the visited token
        /// itself if no changes were made.</returns>
        public virtual SyntaxToken VisitSyntaxToken(SyntaxToken token)
        {
            return token;
        }

        /// <summary>
        /// Visits the specified syntax trivia.
        /// </summary>
        /// <param name="trivia">The trivia to visit.</param>
        /// <returns>A node which should replace the visited trivia, or a reference to the visited trivia
        /// itself if no changes were made.</returns>
        public virtual SyntaxTrivia VisitSyntaxTrivia(SyntaxTrivia trivia)
        {
            return trivia;
        }
        
        /// <summary>
        /// Visits the specified animation keyframe node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitAnimationKeyframe(UvssAnimationKeyframeSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified animation node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitAnimation(UvssAnimationSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified block node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitBlock(UvssBlockSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified document node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitDocument(UvssDocumentSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified event name node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitEventName(UvssEventNameSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified incomplete trigger node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitIncompleteTrigger(UvssIncompleteTriggerSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified event trigger argument list node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitEventTriggerArgumentList(UvssEventTriggerArgumentList node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified event trigger node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitEventTrigger(UvssEventTriggerSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified navigation expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitNavigationExpression(UvssNavigationExpressionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified navigation expression index node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitNavigationExpressionIndex(UvssNavigationExpressionIndexerSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified play-sfx trigger action node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPlaySfxTriggerAction(UvssPlaySfxTriggerActionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified play-storyboard trigger action node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPlayStoryboardTriggerAction(UvssPlayStoryboardTriggerActionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified property name node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPropertyName(UvssPropertyNameSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified property trigger condition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPropertyTriggerCondition(UvssPropertyTriggerConditionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified property trigger node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPropertyTrigger(UvssPropertyTriggerSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified property value node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPropertyValue(UvssPropertyValueSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified brace-enclosed property value node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPropertyValueWithBraces(UvssPropertyValueWithBracesSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified pseudo-class node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitPseudoClass(UvssPseudoClassSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified rule set node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitRuleSet(UvssRuleSetSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified rule node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitRule(UvssRuleSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector part node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorPart(UvssSelectorPartSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector part type node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorPartType(UvssSelectorPartTypeSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector part name node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorPartName(UvssSelectorPartNameSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector part class node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorPartClass(UvssSelectorPartClassSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified invalid selector part node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitInvalidSelectorPart(UvssInvalidSelectorPartSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelector(UvssSelectorSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified parentheses-enclosed selector node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorWithParentheses(UvssSelectorWithParenthesesSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified selector with navigation expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSelectorWithNavigationExpression(UvssSelectorWithNavigationExpressionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified set trigger action node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitSetTriggerAction(UvssSetTriggerActionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified storyboard node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitStoryboard(UvssStoryboardSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified storyboard target node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitStoryboardTarget(UvssStoryboardTargetSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified transition argument list node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitTransitionArgumentList(UvssTransitionArgumentListSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified transition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitTransition(UvssTransitionSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified identifier node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitIdentifier(UvssIdentifierSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified escaped identifier node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitEscapedIdentifier(UvssEscapedIdentifierSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified empty statement node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitEmptyStatement(UvssEmptyStatementSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified unknown directive node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitUnknownDirective(UvssUnknownDirectiveSyntax node)
        {
            return VisitSyntaxNode(node);
        }

        /// <summary>
        /// Visits the specified culture directive node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>A node which should replace the visited node, or a reference to the visited node
        /// itself if no changes were made.</returns>
        public virtual SyntaxNode VisitCultureDirective(UvssCultureDirectiveSyntax node)
        {
            return VisitSyntaxNode(node);
        }
    }
}
