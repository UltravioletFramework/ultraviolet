using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents an exception that is thrown when the <see cref="UvssLexer"/> class encounters an error
    /// during the process of tokenizing a source text.
    /// </summary>
    public sealed class UvssLexerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssLexerException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="line">The line on which the error occurred.</param>
        /// <param name="column">The column at which the error occurred.</param>
        public UvssLexerException(String errorMessage, String errorCode, Int32 line, Int32 column)
            : base(errorMessage)
        {
            this.ErrorCode = errorCode;
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// Gets the error code for the error that occurred.
        /// </summary>
        public String ErrorCode { get; }

        /// <summary>
        /// Gets the line number on which the error occurred.
        /// </summary>
        public Int32 Line { get; }
        
        /// <summary>
        /// Gets the column number at which the error occurred.
        /// </summary>
        public Int32 Column { get; }
    }
}
