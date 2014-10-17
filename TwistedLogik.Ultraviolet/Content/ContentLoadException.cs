using System;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents the exception that is thrown when content fails to load correctly.
    /// </summary>
    [Serializable]
    public class ContentLoadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ContentLoadException class.
        /// </summary>
        public ContentLoadException(String message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ContentLoadException class.
        /// </summary>
        public ContentLoadException(String message, Exception innerException) : base(message, innerException) { }
    }
}
