using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the result of compiling binding expressions.
    /// </summary>
    public sealed class BindingExpressionCompilationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionCompilationResult"/> class.
        /// </summary>
        /// <param name="message">A message clarifying why the compilation failed.</param>
        /// <param name="errors">A collection of errors which were produced during compilation.</param>
        internal BindingExpressionCompilationResult(String message, IEnumerable<BindingExpressionCompilationError> errors)
        {
            this.message = message;

            if (errors != null)
            {
                this.errors.AddRange(errors);
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingExpressionCompilationResult"/> class which represents a successful compilation.
        /// </summary>
        /// <returns>The <see cref="BindingExpressionCompilationResult"/> that was created.</returns>
        public static BindingExpressionCompilationResult CreateSucceeded()
        {
            return new BindingExpressionCompilationResult(null, null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingExpressionCompilationResult"/> class which represents a failed compilation.
        /// </summary>
        /// <param name="message">A message clarifying why compilation failed.</param>
        /// <param name="errors">A collection of errors which were produced during compilation.</param>
        /// <returns>The <see cref="BindingExpressionCompilationResult"/> that was created.</returns>
        public static BindingExpressionCompilationResult CreateFailed(String message, IEnumerable<BindingExpressionCompilationError> errors = null)
        {
            return new BindingExpressionCompilationResult(message, errors);
        }

        /// <summary>
        /// Gets a message clarifying why compilation failed, if compilation was not successful.
        /// </summary>
        public String Message
        {
            get { return message; }
        }

        /// <summary>
        /// Gets a value indicating whether compilation succeeded.
        /// </summary>
        public Boolean Succeeded
        {
            get { return errors.Count == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether compilation failed.
        /// </summary>
        public Boolean Failed
        {
            get { return errors.Count > 0; }
        }

        /// <summary>
        /// Gets the collection of errors which were produced during compilation.
        /// </summary>
        public IEnumerable<BindingExpressionCompilationError> Errors
        {
            get { return errors; }
        }

        // Property values.
        private readonly String message;
        private readonly List<BindingExpressionCompilationError> errors = new List<BindingExpressionCompilationError>();
    }
}
