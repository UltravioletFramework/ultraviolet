using System;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents the scope of a pooled object.  When the scope is created, an object will be retrieved
    /// from the specified pool.  When the scope is disposed, the object will be returned to the pool.
    /// </summary>
    /// <typeparam name="T">The type of pooled object that the scope is tracking.</typeparam>
    public struct PooledObjectScope<T> : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledObjectScope{T}"/> structure.
        /// </summary>
        /// <param name="pool">The pool from which to retrieve an object.</param>
        /// <param name="instance">The object instance being managed by the scope.</param>
        internal PooledObjectScope(IPool pool, T instance)
        {
            Contract.Require(pool, nameof(pool));

            this.pool = pool;
            this.instance = instance;
        }

        /// <summary>
        /// Releases the pooled object represented by this scope.
        /// </summary>
        public void Dispose()
        {
            pool.Release(instance);
        }

        /// <summary>
        /// Gets the pooled object represented by this scope.
        /// </summary>
        public T Object
        {
            get { return instance; }
        }

        // The pooled object represented by this scope.
        private readonly IPool pool;
        private readonly T instance;
    }
}
