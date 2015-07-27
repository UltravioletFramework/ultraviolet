﻿using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            Contract.RequireNotEmpty(expression, "expression");

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
    }
}
