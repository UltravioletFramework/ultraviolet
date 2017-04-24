using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an attribute which identifies a property as the compiled implementation of a binding expression.
    /// </summary>
    public sealed class CompiledBindingExpressionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledBindingExpressionAttribute"/> class.
        /// </summary>
        /// <param name="expression">The expression which was compiled.</param>
        public CompiledBindingExpressionAttribute(String expression)
        {
            Contract.RequireNotEmpty(expression, nameof(expression));

            this.Expression = expression;
        }

        /// <summary>
        /// Gets the expression which was compiled.
        /// </summary>
        public String Expression
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name of the simple dependency property which is represented by this binding expression.
        /// </summary>
        public String SimpleDependencyPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner type of the simple dependency property which is represented by this binding expression.
        /// </summary>
        public Type SimpleDependencyPropertyOwner
        {
            get;
            set;
        }
    }
}
