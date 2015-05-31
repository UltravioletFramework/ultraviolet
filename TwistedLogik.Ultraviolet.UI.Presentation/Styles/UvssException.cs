using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents the exception that is thrown when an error is encountered during Ultraviolet Style Sheet (UVSS) interpretation.
    /// </summary>
    [Serializable]
    public sealed class UvssException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UvssException(String message) : base(message) { }
    }
}
