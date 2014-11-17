using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the exception that is thrown if no file system source is specified on the Android platform.
    /// </summary>
    [Serializable]
    public class MissingFileSystemSourceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingFileSystemSourceException"/> class
        /// with the specified exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public MissingFileSystemSourceException() : base() { }
    }
}