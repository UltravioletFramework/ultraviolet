using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections.Specialized
{
    /// <summary>
    /// Represents a strongly-typed null weak reference.
    /// </summary>
    /// <typeparam name="T">The type of object being referenced.</typeparam>
    public sealed class WeakKeyComparer<T> : IEqualityComparer<Object> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyComparer{T}"/> class.
        /// </summary>
        internal WeakKeyComparer()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">The underlying equality comparer.</param>
        internal WeakKeyComparer(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            this.comparer = comparer;
        }

        /// <summary>
        /// Gets the hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to retrieve a hash code.</param>
        /// <returns>The hash code for the specified object.</returns>
        public Int32 GetHashCode(Object obj)
        {
            var key = obj as WeakKeyReference<T>;
            return (key == null) ? comparer.GetHashCode((T)obj) : key.Hashcode;
        }

        /// <inheritdoc/>
        public new Boolean Equals(Object x, Object y)
        {
            var xIsDead = false;
            var yIsDead = false;
            var xTarget = GetTarget(x, out xIsDead);
            var yTarget = GetTarget(y, out yIsDead);

            if (xIsDead)
                return yIsDead ? x == y : false;

            if (yIsDead)
                return false;

            return comparer.Equals(xTarget, yTarget);
        }

        /// <summary>
        /// Gets the target of the specified object if it is a weak key, and returns a value indicating
        /// whether the reference is alive. Otherwise, returns the object itself.
        /// </summary>
        private static T GetTarget(Object obj, out Boolean isDead)
        {
            var target = default(T);
            var wref = obj as WeakKeyReference<T>;
            if (wref != null)
            {
                target = wref.Target;
                isDead = !wref.IsAlive;
            }
            else
            {
                target = (T)obj;
                isDead = false;
            }
            return target;
        }

        // State values.
        private readonly IEqualityComparer<T> comparer;
    }
}
