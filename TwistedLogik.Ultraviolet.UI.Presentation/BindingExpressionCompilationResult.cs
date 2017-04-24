using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ultraviolet.Presentation
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
        /// <param name="assembly">The in-memory assembly which was produced, if any.</param>
        internal BindingExpressionCompilationResult(String message, IEnumerable<BindingExpressionCompilationError> errors, Assembly assembly = null)
        {
            this.assembly = assembly;
            this.message = message;

            if (errors != null)
            {
                this.errors.AddRange(errors);
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingExpressionCompilationResult"/> class which represents a successful compilation.
        /// </summary>
        /// <param name="inMemoryAssembly">The in-memory assembly which was produced, if any.</param>
        /// <returns>The <see cref="BindingExpressionCompilationResult"/> that was created.</returns>
        public static BindingExpressionCompilationResult CreateSucceeded(Assembly inMemoryAssembly = null)
        {
            return new BindingExpressionCompilationResult(null, null, inMemoryAssembly);
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
        /// Gets the in-memory assembly which was produced, if any.
        /// </summary>
        public Assembly Assembly
        {
            get { return assembly; }
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
        private readonly Assembly assembly;
        private readonly String message;
        private readonly List<BindingExpressionCompilationError> errors = new List<BindingExpressionCompilationError>();
    }
}
