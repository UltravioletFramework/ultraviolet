using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an exception which is produced as a result of failing to compile binding expressions.
    /// </summary>
    public sealed class BindingExpressionCompilationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationFailedException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="result">The result of the compilation that produced this exception.</param>
        public BindingExpressionCompilationFailedException(String message, BindingExpressionCompilationResult result)
            : base(message)
        {
            this.Result = result;
        }

        /// <summary>
        /// Gets the result of the compilation that produced this exception.
        /// </summary>
        public BindingExpressionCompilationResult Result
        {
            get;
            private set;
        }
    }
}
