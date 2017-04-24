using System;

namespace Ultraviolet.Tooling
{
    /// <summary>
    /// Represents the exception that is thrown when an application's command line arguments
    /// cannot be processed or are syntactically incorrect.
    /// </summary>
    [Serializable]
    public class InvalidCommandLineException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandLineException"/> class.
        /// </summary>
        public InvalidCommandLineException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandLineException"/> class with the specified error message.
        /// </summary>
        public InvalidCommandLineException(String error) { Error = error; }

        /// <summary>
        /// Gets the error message caused by the invalid command line arguments.
        /// </summary>
        public String Error
        {
            get;
            private set;
        }
    }
}
