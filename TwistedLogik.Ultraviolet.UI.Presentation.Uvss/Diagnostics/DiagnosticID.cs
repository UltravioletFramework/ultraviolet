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
        /// A token was found but not expected.
        /// </summary>
        UnexpectedToken,

        /// <summary>
        /// An animation is missing its property name.
        /// </summary>
        AnimationMissingPropertyName,
    }
}
