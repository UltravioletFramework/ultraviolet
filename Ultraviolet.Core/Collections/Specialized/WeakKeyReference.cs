using System;

namespace Ultraviolet.Core.Collections.Specialized
{
    /// <summary>
    /// Represents a key in a weak dictionary.
    /// </summary>
    /// <typeparam name="T">The type of object being referenced.</typeparam>
    public sealed class WeakKeyReference<T> : WeakReference<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyReference{T}"/> class.
        /// </summary>
        /// <param name="key">The key value which is being tracked.</param>
        /// <param name="comparer">The comparer which is used to compare key values.</param>
        internal WeakKeyReference(T key, WeakKeyComparer<T> comparer)
            : base(key)
        {
            this.Hashcode = comparer.GetHashCode(key);
        }

        /// <summary>
        /// Gets the key reference's hashcode.
        /// </summary>
        public Int32 Hashcode { get; }
    }
}
