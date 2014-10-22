using System;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the exception that is thrown when an error occurs within the application's layout engine.
    /// </summary>
    [Serializable]
    public sealed class LayoutEngineException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEngineException"/> class.
        /// </summary>
        public LayoutEngineException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEngineException"/> class with the specified exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public LayoutEngineException(String message) : base() { }
    }
}
