using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a key which identifies an expression by its text and type.
    /// </summary>
    internal partial struct CompiledBindingExpressionKey : IEquatable<CompiledBindingExpressionKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledBindingExpressionKey"/> structure.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <param name="expression">The text of the expression.</param>
        public CompiledBindingExpressionKey(Type type, String expression)
        {
            this.Type = type;
            this.Expression = expression;
        }

        /// <summary>
        /// Gets the type of the expression.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the text of the expression.
        /// </summary>
        public String Expression { get; }
    }
}
