namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains the types of syntax nodes which are recognized by the serialization system.
    /// </summary>
    internal enum SyntaxNodeType : byte
    {
        Token,
        Keyword,
        Punctuation,

        SkippedTokensTrivia,
        StructurelessTrivia,

        SyntaxListMissing,
        SyntaxListWithTwoChildren,
        SyntaxListWithThreeChildren,
        SyntaxListWithManyChildren,
        SyntaxListWithLotsOfChildren,

        AnimationKeyframe,
        Animation,
        Block,
        Document,
        EmptyStatement,
        EscapedIdentifier,
        EventName,
        EventTriggerArgumentList,
        EventTrigger,
        Identifier,
        IncompleteTrigger,
        InvalidSelectorPart,
        NavigationExpressionIndexer,
        NavigationExpression,
        PlaySfxTriggerAction,
        PlayStoryboardTriggerAction,
        PropertyName,
        PropertyTriggerCondition,
        PropertyTrigger,
        PropertyValue,
        PropertyValueWithBraces,
        PseudoClass,
        RuleSet,
        Rule,
        SelectorPartClass,
        SelectorPartName,
        SelectorPart,
        SelectorPartType,
        Selector,
        SelectorWithNavigationExpression,
        SelectorWithParentheses,
        SetTriggerAction,
        Storyboard,
        StoryboardTarget,
        TransitionArgumentList,
        Transition,

		UnknownDirective,
		CultureDirective,
    }
}
