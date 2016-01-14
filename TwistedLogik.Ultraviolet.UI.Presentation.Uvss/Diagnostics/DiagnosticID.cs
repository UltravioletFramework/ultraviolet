namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Diagnostics
{
    /// <summary>
    /// Identifies the kinds of warnings and errors reported via diagnostics.
    /// </summary>
    public enum DiagnosticID
    {
        /// <summary>
        /// A token was expected but not found.
        /// </summary>
        MissingToken,

        /// <summary>
        /// A rule set or storyboard was expected, but another token was found.
        /// </summary>
        RuleSetOrStoryboardExpected,

        /// <summary>
        /// A rule, transition, or trigger was expected, but another token was found.
        /// </summary>
        RuleTransitionOrTriggerExpected,

        /// <summary>
        /// An event trigger argument was expected, but another token was found.
        /// </summary>
        EventTriggerArgumentExpected,

        /// <summary>
        /// A trigger action was expected, but another token was found.
        /// </summary>
        TriggerActionExpected,

        /// <summary>
        /// A storyboard target was expected, but another token was found.
        /// </summary>
        StoryboardTargetExpected,
        
        /// <summary>
        /// An animation was expected, but another token was found.
        /// </summary>
        AnimationExpected,

        /// <summary>
        /// An animation keyframe was expected, but another token was found.
        /// </summary>
        AnimationKeyframeExpected,

        /// <summary>
        /// An event trigger's argument list has an insufficient number of arguments.
        /// </summary>
        EventTriggerHasTooFewArguments,

        /// <summary>
        /// An event trigger's argument list has duplicated arguments.
        /// </summary>
        EventTriggerHasDuplicateArguments,
        
        /// <summary>
        /// A visual transition argument list has an insufficient number of arguments.
        /// </summary>
        TransitionHasTooFewArguments,

        /// <summary>
        /// A visual transition argument list has too many arguments.
        /// </summary>
        TransitionHasTooManyArguments,

        /// <summary>
        /// A trigger has failed to specify its type.
        /// </summary>
        IncompleteTrigger,

        /// <summary>
        /// A loop type was not recognized by the parser.
        /// </summary>
        UnrecognizedLoopType,

        /// <summary>
        /// An easing function was not recognized by the parser.
        /// </summary>
        UnrecognizedEasingFunction,
    }
}
