using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a strongly-typed null weak reference.
    /// </summary>
    /// <typeparam name="T">The type of object being referenced.</typeparam>
    public sealed class WeakNullReference<T> : WeakReference<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakNullReference{T}"/> class.
        /// </summary>
        private WeakNullReference()
            : base(null)
        { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="WeakNullReference{T}"/> class.
        /// </summary>
        public static WeakNullReference<T> Singleton { get; } = new WeakNullReference<T>();

        /// <inheritdoc/>
        public override Boolean IsAlive => true;
    }
}
