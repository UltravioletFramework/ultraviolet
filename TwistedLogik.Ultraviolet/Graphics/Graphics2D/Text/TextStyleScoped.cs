using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents an object which is scoped to a text style within the text layout or rendering engines.
    /// </summary>
    /// <typeparam name="T">The type of object which is associated with a style scope.</typeparam>
    public struct TextStyleScoped<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextStyleScoped{T}"/> structure.
        /// </summary>
        /// <param name="value">The scoped value.</param>
        /// <param name="scope">The style scope identifier.</param>
        internal TextStyleScoped(T value, Int32 scope)
        {
            this.value = value;
            this.scope = scope;
        }

        /// <summary>
        /// Gets the scoped value.
        /// </summary>
        public T Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the style scope identifier.
        /// </summary>
        public Int32 Scope
        {
            get { return scope; }
        }

        // Property values.
        private readonly T value;
        private readonly Int32 scope;
    }
}
