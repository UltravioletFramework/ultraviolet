using System;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a pool of pre-allocated objects.
    /// </summary>
    public interface IPool : IDisposable
    {
        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The object that was retrieved from the pool.</returns>
        Object Retrieve();

        /// <summary>
        /// Retrieves a scoped object from the pool.
        /// </summary>
        /// <returns>A <see cref="PooledObjectScope{T}"/> that represents the lifetime of 
        /// the object that was retrieved from the pool.</returns>
        PooledObjectScope<Object> RetrieveScoped();

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void Release(Object instance);

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void ReleaseRef(ref Object instance);

        /// <summary>
        /// Gets the number of objects in the pool that are currently in use.
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Gets the total number of objects in the pool.
        /// </summary>
        Int32 Capacity { get; }
    }
}
