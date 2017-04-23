using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a strongly-typed weak reference.
    /// </summary>
    /// <typeparam name="T">The type of object being referenced.</typeparam>
    public class WeakReference<T> : WeakReference where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">The object to track, or <see langword="null"/>.</param>
        internal WeakReference(T target)
            : base(target)
        { }

        /// <summary>
        /// Creates a new weak reference to the specified object.
        /// </summary>
        /// <param name="target">The object to track, or <see langword="null"/>.</param>
        /// <returns>The weak reference that was created.</returns>
        public static WeakReference<T> Create(T target)
        {
            if (target == null)
                return WeakNullReference<T>.Singleton;

            return new WeakReference<T>(target);
        }

        /// <summary>
        /// Gets the object which is being tracked by the weak reference.
        /// </summary>
        public new T Target => (T)base.Target;
    }
}