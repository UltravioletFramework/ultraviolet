namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents the types of nodes, tokens, and trivia in a UVSS syntax tree.
    /// </summary>
    public enum SyntaxKind
    {
        /// <summary>
        /// The syntax kind of the node, token, or trivia is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Trivia representing a single line comment.
        /// </summary>
        SingleLineCommentTrivia,

        /// <summary>
        /// Trivia representing a multi-line comment.
        /// </summary>
        MultiLineCommentTrivia,

        /// <summary>
        /// Trivia representing syntactically meaningless white space.
        /// </summary>
        WhitespaceTrivia,

        /// <summary>
        /// The "play-storyboard" keyword.
        /// </summary>
        PlayStoryboardKeyword,

        /// <summary>
        /// The "set-handled" keyword.
        /// </summary>
        SetHandledKeyword,

        /// <summary>
        /// The "transition" keyword.
        /// </summary>
        TransitionKeyword,

        /// <summary>
        /// The "important" keyword.
        /// </summary>
        ImportantKeyword,

        /// <summary>
        /// The "animation" keyword.
        /// </summary>
        AnimationKeyword,

        /// <summary>
        /// The "play-sfx" keyword.
        /// </summary>
        PlaySfxKeyword,

        /// <summary>
        /// The "property" keyword.
        /// </summary>
        PropertyKeyword,

        /// <summary>
        /// The "keyframe" keyword.
        /// </summary>
        KeyframeKeyword,

        /// <summary>
        /// The "trigger" keyword.
        /// </summary>
        TriggerKeyword,

        /// <summary>
        /// The "handled" keyword.
        /// </summary>
        HandledKeyword,

        /// <summary>
        /// The "target" keyword.
        /// </summary>
        TargetKeyword,

        /// <summary>
        /// The "event" keyword.
        /// </summary>
        EventKeyword,

        /// <summary>
        /// The "set" keyword.
        /// </summary>
        SetKeyword,

        /// <summary>
        /// The "as" keyword.
        /// </summary>
        AsKeyword,

        /// <summary>
        /// An identifier token.
        /// </summary>
        IdentifierToken,

        /// <summary>
        /// A number token.
        /// </summary>
        NumberToken,

        /// <summary>
        /// A comma (",") token.
        /// </summary>
        CommaToken,

        /// <summary>
        /// A colon (":") token.
        /// </summary>
        ColonToken,

        /// <summary>
        /// A semi-colon (";") token.
        /// </summary>
        SemiColonToken,

        /// <summary>
        /// An at sign ("@") token.
        /// </summary>
        AtSignToken,

        /// <summary>
        /// A hash ("#") token.
        /// </summary>
        HashToken,

        /// <summary>
        /// A period (".") token.
        /// </summary>
        PeriodToken,

        /// <summary>
        /// An exclamation mark ("!") token.
        /// </summary>
        ExclamationMarkToken,

        /// <summary>
        /// An open parentheses ("(") token.
        /// </summary>
        OpenParenthesesToken,

        /// <summary>
        /// A close parentheses (")") token.
        /// </summary>
        CloseParenthesesToken,

        /// <summary>
        /// An open curly brace ("{") token.
        /// </summary>
        OpenCurlyBraceToken,

        /// <summary>
        /// A close curly brace ("}") token.
        /// </summary>
        CloseCurlyBraceToken,

        /// <summary>
        /// An asterisk ("*") token.
        /// </summary>
        AsteriskToken,

        /// <summary>
        /// A double greater than (">>") token.
        /// </summary>
        GreaterThanGreaterThanToken,

        /// <summary>
        /// A greater than followed by a question mark (">?") token.
        /// </summary>
        GreaterThanQuestionMarkToken,
        
        /// <summary>
        /// A space (" ") token.
        /// </summary>
        SpaceToken,

        /// <summary>
        /// An equals ("=") token.
        /// </summary>
        EqualsToken,

        /// <summary>
        /// A not equals ("&lt;&gt;") token.
        /// </summary>
        NotEqualsToken,

        /// <summary>
        /// A less than ("&lt;") token.
        /// </summary>
        LessThanToken,

        /// <summary>
        /// A greater than (">") token.
        /// </summary>
        GreaterThanToken,

        /// <summary>
        /// A less than or equal to ("&lt;=") token.
        /// </summary>
        LessThanEqualsToken,

        /// <summary>
        /// A greater than or equal to (">=") token.
        /// </summary>
        GreaterThanEqualsToken,

        /// <summary>
        /// A pipe ("|") token.
        /// </summary>
        PipeToken,

        /// <summary>
        /// A property value token.
        /// </summary>
        PropertyValueToken,

        /// <summary>
        /// A list of nodes.
        /// </summary>
        List,

        /// <summary>
        /// A block.
        /// </summary>
        Block,

        /// <summary>
        /// A styling rule set.
        /// </summary>
        RuleSet,

        /// <summary>
        /// A styling rule.
        /// </summary>
        Rule,
        
        /// <summary>
        /// A selector.
        /// </summary>
        Selector,

        /// <summary>
        /// A selector, enclosed by parentheses.
        /// </summary>
        SelectorWithParentheses,

        /// <summary>
        /// A selector part.
        /// </summary>
        SelectorPart,

        /// <summary>
        /// A selector sub-part.
        /// </summary>
        SelectorSubPart,

        /// <summary>
        /// A pseudo-class.
        /// </summary>
        PseudoClass,

        /// <summary>
        /// The name of a styled property.
        /// </summary>
        PropertyName,

        /// <summary>
        /// The value applied to a styled property.
        /// </summary>
        PropertyValue,

        /// <summary>
        /// The value applied to a styled property, enclosed by curly braces.
        /// </summary>
        PropertyValueWithBraces,

        /// <summary>
        /// The name of a styled event.
        /// </summary>
        EventName,

        /// <summary>
        /// An event trigger.
        /// </summary>
        EventTrigger,

        /// <summary>
        /// An event trigger's argument list.
        /// </summary>
        EventTriggerArgumentList,

        /// <summary>
        /// A property trigger.
        /// </summary>
        PropertyTrigger,

        /// <summary>
        /// A property trigger evaluator.
        /// </summary>
        PropertyTriggerEvaluation,

        /// <summary>
        /// A play-storyboard trigger action.
        /// </summary>
        PlayStoryboardTriggerAction,

        /// <summary>
        /// A play-sfx trigger action.
        /// </summary>
        PlaySfxTriggerAction,

        /// <summary>
        /// A set trigger action.
        /// </summary>
        SetTriggerAction,

        /// <summary>
        /// A visual transition.
        /// </summary>
        Transition,

        /// <summary>
        /// A visual transition's argument list.
        /// </summary>
        TransitionArgumentList,

        /// <summary>
        /// A storyboard.
        /// </summary>
        Storyboard,

        /// <summary>
        /// A storyboard target.
        /// </summary>
        StoryboardTarget,

        /// <summary>
        /// An animation.
        /// </summary>
        Animation,

        /// <summary>
        /// An animation keyframe.
        /// </summary>
        AnimationKeyframe,

        /// <summary>
        /// A navigation expression.
        /// </summary>
        NavigationExpression,

        /// <summary>
        /// A document root node.
        /// </summary>
        UvssDocument,
    }
}
