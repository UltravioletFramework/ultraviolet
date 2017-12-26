using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the exception that is thrown when the Ultraviolet Framework cannot load the compatibility shim
    /// for the current platform.
    /// </summary>
    [Serializable]
    public class InvalidCompatibilityShimException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCompatibilityShimException"/> class
        /// with the specified exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidCompatibilityShimException(String message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCompatibilityShimException"/> class 
        /// with the specified exception message and inner exception..
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception's inner exception.</param>
        public InvalidCompatibilityShimException(String message, Exception innerException) : base(message, innerException) { }
    }
}
