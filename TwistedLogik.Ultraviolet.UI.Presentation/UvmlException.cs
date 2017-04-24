using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the exception that is thrown when an error is encountered during Ultraviolet Markup Language (UVML) interpretation.
    /// </summary>
    [Serializable]
    public sealed class UvmlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UvmlException(String message) : base(message) { }
    }
}
