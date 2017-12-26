namespace Ultraviolet.Presentation.Uvss.Diagnostics
{
    /// <summary>
    /// Represents the severity of a diagnostic message.
    /// </summary>
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// The diagnostic is not displayed to the user through normal means.
        /// </summary>
        Hidden,

        /// <summary>
        /// The diagnostic is informational.
        /// </summary>
        Info,

        /// <summary>
        /// The diagnostic is a warning of code which appears to be a mistake
        /// but which is allowed by the language.
        /// </summary>
        Warning,

        /// <summary>
        /// The diagnostic represents invalid code.
        /// </summary>
        Error,
    }
}
