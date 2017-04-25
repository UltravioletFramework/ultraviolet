using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the exception that is thrown when content fails to load correctly.
    /// </summary>
    [Serializable]
    public class ContentLoadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLoadException"/> class
        /// with the specified exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ContentLoadException(String message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLoadException"/> class 
        /// with the specified exception message and inner exception..
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception's inner exception.</param>
        public ContentLoadException(String message, Exception innerException) : base(message, innerException) { }
    }
}
