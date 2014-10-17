using System;
using System.Linq;
using System.Linq.Expressions;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents a pre-allocated pool of objects which can expand if all of its objects are consumed.
    /// </summary>
    public class ExpandingPool<T> : IPool<T>
    {
        /// <summary>
        /// Initializes a new instance of the ExpandingPool class.
        /// </summary>
        /// <param name="capacity">The pool's initial capacity.</param>
        /// <param name="allocator">A function which allocates new instances of <typeparamref name="T"/>.</param>
        public ExpandingPool(Int32 capacity, Func<T> allocator = null)
        {
            Contract.EnsureRange(capacity >= 0, "capacity");

            this.allocator = allocator ?? CreateDefaultAllocator();
            this.disposable = typeof(T).GetInterfaces().Contains(typeof(IDisposable));

            ExpandStorage(capacity);
        }

        /// <summary>
        /// Disposes of all of the objects in the pool, if <typeparamref name="T"/> implements <see cref="System.IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The object that was retrieved from the pool.</returns>
        public T Retrieve()
        {
            if (storage.Length == count)
                ExpandStorage();

            return storage[count++];
        }

        /// <summary>
        /// Retrieves a scoped object from the pool.
        /// </summary>
        /// <returns>The scoped object that was retrieved from the pool.</returns>
        public PooledObjectScope<T> RetrieveScoped()
        {
            return new PooledObjectScope<T>(this, Retrieve());
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        public void Release(T instance)
        {
            if (count == 0)
                throw new InvalidOperationException(NucleusStrings.PoolImbalance);

            storage[--count] = instance;
        }
        
        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        public void ReleaseRef(ref T instance)
        {
            if (count == 0)
                throw new InvalidOperationException(NucleusStrings.PoolImbalance);

            storage[--count] = instance;
            instance = default(T);
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns>The object that was retrieved from the pool.</returns>
        Object IPool.Retrieve()
        {
            return Retrieve();
        }

        /// <summary>
        /// Retrieves a scoped object from the pool.
        /// </summary>
        /// <returns>The scoped object that was retrieved from the pool.</returns>
        PooledObjectScope<Object> IPool.RetrieveScoped()
        {
            return new PooledObjectScope<Object>(this, Retrieve());
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void IPool.Release(Object instance)
        {
            Release((T)instance);
        }

        /// <summary>
        /// Releases an object back into the pool.
        /// </summary>
        /// <param name="instance">The object to release.</param>
        void IPool.ReleaseRef(ref Object instance)
        {
            Release((T)instance);
            instance = null;
        }

        /// <summary>
        /// Gets the number of objects in the pool that are currently in use.
        /// </summary>
        public Int32 Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets the total number of objects in the pool.
        /// </summary>
        public Int32 Capacity
        {
            get { return storage.Length; }
        }

        /// <summary>
        /// Disposes of all of the objects in the pool, if <typeparamref name="T"/> implements <see cref="System.IDisposable"/>.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing && disposable)
            {
                foreach (var item in storage)
                {
                    ((IDisposable)item).Dispose();
                }
            }
        }

        /// <summary>
        /// Creates a default allocator for the pooled type.
        /// </summary>
        /// <returns>The allocator that was created.</returns>
        private static Func<T> CreateDefaultAllocator()
        {
            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ctor == null)
                throw new InvalidOperationException(NucleusStrings.MissingDefaultCtor.Format(typeof(T).FullName));

            return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        }

        /// <summary>
        /// Expands the pool's storage capacity to a factor of the pool's current storage capacity.
        /// </summary>
        private void ExpandStorage()
        {
            var capacity = ((3 * storage.Length) / 2) + 1;
            ExpandStorage(capacity);
        }

        /// <summary>
        /// Expands the pool's storage such that it has at least the specified capacity.
        /// </summary>
        /// <param name="capacity">The target capacity after expansion.</param>
        private void ExpandStorage(Int32 capacity)
        {
            if (this.storage != null && this.storage.Length >= capacity)
                return;

            var old = storage;

            storage = new T[capacity];
            if (count > 0)
            {
                Array.Copy(old, storage, count);
            }

            for (int i = 0; i < storage.Length; i++)
                storage[i] = allocator();
        }

        // The underlying storage for the object pool.
        private readonly Func<T> allocator;
        private T[] storage;
        private Int32 count;
        private Boolean disposable;
    }
}
