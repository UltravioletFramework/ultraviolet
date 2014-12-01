using System;
using System.Linq;
using System.Linq.Expressions;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents a pre-allocated pool of objects which can expand if all of its objects are consumed.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the pool.</typeparam>
    public class ExpandingPool<T> : IPool<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandingPool{T}"/> class.
        /// </summary>
        /// <param name="capacity">The pool's initial capacity.</param>
        /// <param name="allocator">A function which allocates new instances of <typeparamref name="T"/>.</param>
        public ExpandingPool(Int32 capacity, Func<T> allocator = null)
            : this(capacity, Int32.MaxValue, allocator)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandingPool{T}"/> class.
        /// </summary>
        /// <param name="capacity">The pool's initial capacity.</param>
        /// <param name="watermark">The pool's watermark value, which indicates the maximum size of the pool.</param>
        /// <param name="allocator">A function which allocates new instances of <typeparamref name="T"/>.</param>
        public ExpandingPool(Int32 capacity, Int32 watermark, Func<T> allocator = null)
        {
            Contract.EnsureRange(capacity >= 0, "capacity");
            Contract.EnsureRange(watermark >= 1, "watermark");
            Contract.EnsureRange(watermark >= capacity, "watermark");

            this.watermark  = watermark;
            this.allocator  = allocator ?? CreateDefaultAllocator();
            this.disposable = typeof(T).GetInterfaces().Contains(typeof(IDisposable));

            ExpandStorage(capacity);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public T Retrieve()
        {
            if (storage.Length == count)
            {
                if (storage.Length == watermark)
                {
                    watermarkAllocations++;
                    return allocator();
                }
                ExpandStorage();
            }
            return storage[count++];
        }

        /// <inheritdoc/>
        public PooledObjectScope<T> RetrieveScoped()
        {
            return new PooledObjectScope<T>(this, Retrieve());
        }

        /// <inheritdoc/>
        public void Release(T instance)
        {
            if (watermarkAllocations + count == 0)
                throw new InvalidOperationException(NucleusStrings.PoolImbalance);

            if (count == 0)
            {
                watermarkAllocations--;
            }
            else
            {
                storage[--count] = instance;
            }
        }

        /// <inheritdoc/>
        public void ReleaseRef(ref T instance)
        {
            if (watermarkAllocations + count == 0)
                throw new InvalidOperationException(NucleusStrings.PoolImbalance);

            if (count == 0)
            {
                watermarkAllocations--;
            }
            else
            {
                storage[--count] = instance;
                instance = default(T);
            }
        }

        /// <inheritdoc/>
        Object IPool.Retrieve()
        {
            return Retrieve();
        }

        /// <inheritdoc/>
        PooledObjectScope<Object> IPool.RetrieveScoped()
        {
            return new PooledObjectScope<Object>(this, Retrieve());
        }

        /// <inheritdoc/>
        void IPool.Release(Object instance)
        {
            Release((T)instance);
        }

        /// <inheritdoc/>
        void IPool.ReleaseRef(ref Object instance)
        {
            Release((T)instance);
            instance = null;
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return count; }
        }

        /// <inheritdoc/>
        public Int32 Capacity
        {
            get { return storage.Length; }
        }

        /// <summary>
        /// Gets the number of objects that were allocated as a result of reaching the pool's watermark.
        /// </summary>
        public Int32 WatermarkAllocations
        {
            get { return watermarkAllocations; }
        }

        /// <summary>
        /// Gets the pool's watermark value, which indicates its maximum size.
        /// </summary>
        /// <remarks>Beyond this point, the pool will simply return newly-allocated objects instead of pooled objects. 
        /// Such objects should still be released back into the pool for bookkeeping purposes.</remarks>
        public Int32 Watermark
        {
            get { return watermark; }
        }

        /// <summary>
        /// Disposes of all of the objects in the pool, if <typeparamref name="T"/> implements <see cref="System.IDisposable"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; false if the object is being finalized.</param>
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
            var capacity = Math.Min(watermark, ((3 * storage.Length) / 2) + 1);
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
        private readonly Boolean disposable;
        private T[] storage;
        private Int32 count;
        private Int32 watermark;
        private Int32 watermarkAllocations;
    }
}
