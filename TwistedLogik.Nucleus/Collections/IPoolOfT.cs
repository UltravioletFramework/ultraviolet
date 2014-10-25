
namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents a pool of pre-allocated objects.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the pool.</typeparam>
    public interface IPool<T> : IPool
    {
        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The object that was retrieved from the pool.</returns>
        new T Retrieve();

        /// <summary>
        /// Retrieves a scoped object from the pool.
        /// </summary>
        /// <returns>A <see cref="PooledObjectScope{T}"/> that represents the lifetime of 
        /// the object that was retrieved from the pool.</returns>
        new PooledObjectScope<T> RetrieveScoped();

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void Release(T instance);

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void ReleaseRef(ref T instance);
    }
}
