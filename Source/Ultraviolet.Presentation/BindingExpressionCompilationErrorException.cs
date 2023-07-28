using System;
using System.Xml.Linq;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during the process of compiling binding expressions.
    /// </summary>
    public sealed class BindingExpressionCompilationErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationErrorException"/> class.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        public BindingExpressionCompilationErrorException(BindingExpressionCompilationError error)
            : base(error.ErrorText)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationErrorException"/> class.
        /// </summary>
        /// <param name="source">The <see cref="XObject"/> which is the source of the error.</param>
        /// <param name="filename">The filename of the file which is the source of the error.</param>
        /// <param name="message">The error message.</param>
        public BindingExpressionCompilationErrorException(XObject source, String filename, String message)
            : base(message)
        {
            this.Error = new BindingExpressionCompilationError(source, filename, message);
        }

        /// <summary>
        /// Gets the error that occurred.
        /// </summary>
        public BindingExpressionCompilationError Error
        {
            get;
            private set;
        }
    }
}
