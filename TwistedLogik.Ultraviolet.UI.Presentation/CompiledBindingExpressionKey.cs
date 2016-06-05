using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a key which identifies an expression by its text and type.
    /// </summary>
    internal struct CompiledBindingExpressionKey : IEquatable<CompiledBindingExpressionKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledBindingExpressionKey"/> structure.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <param name="expression">The text of the expression.</param>
        public CompiledBindingExpressionKey(Type type, String expression)
        {
            this.type       = type;
            this.expression = expression;
        }

        /// <summary>
        /// Evaluates two instances of <see cref="CompiledBindingExpressionKey"/> to determine whether they are equal.
        /// </summary>
        /// <param name="k1">The key on the left side of the operator.</param>
        /// <param name="k2">The key on the right side of the operator.</param>
        /// <returns><see langword="true"/> if the specified keys are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(CompiledBindingExpressionKey k1, CompiledBindingExpressionKey k2)
        {
            return k1.Equals(k2);
        }

        /// <summary>
        /// Evaluates two instances of <see cref="CompiledBindingExpressionKey"/> to determine whether they are unequal.
        /// </summary>
        /// <param name="k1">The key on the left side of the operator.</param>
        /// <param name="k2">The key on the right side of the operator.</param>
        /// <returns><see langword="true"/> if the specified keys are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(CompiledBindingExpressionKey k1, CompiledBindingExpressionKey k2)
        {
            return !k1.Equals(k2);
        }
        
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ((type == null) ? 0 : type.GetHashCode());
                hash = hash * 23 + ((expression == null) ? 0 : expression.GetHashCode());
                return hash;
            }
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is CompiledBindingExpressionKey))
            {
                return false;
            }
            return Equals((CompiledBindingExpressionKey)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(CompiledBindingExpressionKey other)
        {
            return
                this.type == other.type &&
                this.expression == other.expression;
        }

        /// <summary>
        /// Gets the type of the expression.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// Gets the text of the expression.
        /// </summary>
        public String Expression
        {
            get { return expression; }
        }

        // Property values.
        private readonly Type type;
        private readonly String expression;
    }
}
