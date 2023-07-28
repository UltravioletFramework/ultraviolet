using System;
using System.Linq.Expressions;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents the base class for pools.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the pool.</typeparam>
    public abstract class Pool<T> : IPool<T>, IPool, IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public abstract T Retrieve();

        /// <inheritdoc/>
        Object IPool.Retrieve()
        {
            return Retrieve();
        }

        /// <inheritdoc/>
        public virtual PooledObjectScope<T> RetrieveScoped()
        {
            return new PooledObjectScope<T>(this, Retrieve());
        }

        /// <inheritdoc/>
        PooledObjectScope<Object> IPool.RetrieveScoped()
        {
            return new PooledObjectScope<Object>(this, Retrieve());
        }

        /// <inheritdoc/>
        public abstract void Release(T instance);

        /// <inheritdoc/>
        void IPool.Release(Object instance)
        {
            Release((T)instance);
        }

        /// <inheritdoc/>
        public abstract void ReleaseRef(ref T instance);

        /// <inheritdoc/>
        void IPool.ReleaseRef(ref Object instance)
        {
            Release((T)instance);
            instance = null;
        }

        /// <inheritdoc/>
        public abstract Int32 Count { get; }

        /// <inheritdoc/>
        public abstract Int32 Capacity { get; }

        /// <summary>
        /// Creates a default allocator for the pooled type.
        /// </summary>
        /// <returns>The allocator that was created.</returns>
        protected static Func<T> CreateDefaultAllocator()
        {
            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ctor == null)
                throw new InvalidOperationException(CoreStrings.MissingDefaultCtor.Format(typeof(T).FullName));

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
            }
            else
            {
                return () => (T)ctor.Invoke(null);
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {

        }
    }
}
