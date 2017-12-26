using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a pool of objects used by the Presentation Foundation.
    /// </summary>
    /// <typeparam name="TPooledType">The type of object which is being pooled.</typeparam>
    internal partial class UpfPool<TPooledType> : UltravioletResource where TPooledType : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpfPool{TPooledType}"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="capacity">The pool's initial capacity.</param>
        /// <param name="watermark">The pool's high watermark value.</param>
        /// <param name="allocator">The pool's allocator function.</param>
        public UpfPool(UltravioletContext uv, Int32 capacity, Int32 watermark, Func<TPooledType> allocator)
            : base(uv)
        {
            Contract.Require(allocator, nameof(allocator));
            Contract.EnsureRange(capacity >= 0, nameof(capacity));
            Contract.EnsureRange(watermark >= 0, nameof(watermark));

            this.gcCounts = new Int32[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            this.watermark = watermark;
            this.allocator = allocator;

            for (int i = 0; i < capacity; i++)
            {
                var poolObject = new PooledObject(allocator());
                AvailableAddLast(poolObject);
            }
        }        

        /// <summary>
        /// Retrieves an object from the pool and assigns it to the specified owner.
        /// </summary>
        /// <param name="owner">The object that owns the pooled object.</param>
        /// <returns>The object that was retrieved from the pool.</returns>
        public PooledObject Retrieve(Object owner)
        {
            Contract.Require(owner, nameof(owner));

            var @object = headAvailable;
            if (@object == null)
            {
                @object = new PooledObject(allocator());
                if (available + active < watermark)
                {
                    ActiveAddLast(@object);
                }
            }
            else
            {
                AvailableRemove(@object);
                ActiveAddLast(@object);
            }

            @object.Assign(owner);
            OnInitializingObject(@object.Value);

            return (PooledObject)@object;
        }

        /// <summary>
        /// Releases the specified object back into the pool.
        /// </summary>
        /// <param name="object">The object to release back into the pool.</param>
        public void Release(PooledObject @object)
        {
            Contract.Require(@object, "object");

            // Object was probably allocated after hitting the high watermark
            var pooledObject = (IPooledObject)@object;
            if (pooledObject.CurrentPoolList == UpfPoolList.None)
                return;

            ActiveRemove(pooledObject);
            OnCleaningUpObject(pooledObject.Value);
            AvailableAddLast(pooledObject);
        }

        /// <summary>
        /// Updates the state of the pool. Any objects which belong to owners that have been garbage
        /// collected will be returned to the pool.
        /// </summary>
        public void Update()
        {
            Update(null, null);
        }

        /// <summary>
        /// Updates the state of the pool. Any objects which belong to owners that have been garbage
        /// collected will be returned to the pool.
        /// </summary>
        /// <param name="state">A state value to pass to <paramref name="updater"/>.</param>
        /// <param name="updater">An action to perform on each active item in the pool.</param>
        public void Update(Object state, Action<PooledObject, Object> updater)
        {
            if (updater != null)
            {
                for (var obj = headActive; obj != null; obj = obj.Next)
                    updater((PooledObject)obj, state);
            }

            if (!HasGarbageCollectionOccurredSinceLastCheck())
                return;

            var current = headActive;

            while (current != null)
            {
                var next = current.Next;
                if (!current.Owner.IsAlive)
                {
                    ActiveRemove(current);
                    AvailableAddLast(current);
                }
                current = next;
            }
        }

        /// <summary>
        /// Gets the number of active objects which have been allocated from the pool.
        /// </summary>
        public Int32 Active
        {
            get { return active; }
        }

        /// <summary>
        /// Gets the number of available objects in the pool.
        /// </summary>
        public Int32 Available
        {
            get { return available; }
        }

        /// <summary>
        /// Gets the pool's high watermark value.
        /// </summary>
        public Int32 Watermark
        {
            get { return watermark; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            for (var current = headActive; current != null; current = current.Next)
            {
                var disposable = current.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            headActive = null;
            tailActive = null;
            headAvailable = null;
            tailAvailable = null;

            active = 0;
            available = 0;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when an object is being retrieved from the pool.
        /// </summary>
        /// <param name="object">The object which is being retrieved from the pool.</param>
        protected virtual void OnInitializingObject(TPooledType @object)
        {

        }

        /// <summary>
        /// Called when an object is being returned to the pool.
        /// </summary>
        /// <param name="object">The object which is being returned to the pool.</param>
        protected virtual void OnCleaningUpObject(TPooledType @object)
        {

        }

        /// <summary>
        /// Gets a value indicating whether a garbage collection has occurred since the last time this method was called.
        /// </summary>
        /// <returns><see langword="true"/> if a garbage collection has occurred; otherwise, <see langword="false"/>.</returns>
        private Boolean HasGarbageCollectionOccurredSinceLastCheck()
        {
            var collectionHasOccurred = false;
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                var count = GC.CollectionCount(i);
                if (count != gcCounts[i])
                {
                    collectionHasOccurred = true;
                    gcCounts[i] = count;
                }
            }

            return collectionHasOccurred;
        }

        /// <summary>
        /// Adds an object to the front of the active list.
        /// </summary>
        /// <param name="object">The object to add.</param>
        private void ActiveAddFirst(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.None)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.Active;

            if (headActive == null)
            {
                tailActive = @object;
                @object.Previous = null;
                @object.Next = null;
            }
            else
            {
                headActive.Previous = @object;
                @object.Previous = null;
                @object.Next = headActive;
            }
            headActive = @object;

            active++;
        }

        /// <summary>
        /// Adds an object to the end of the active list.
        /// </summary>
        /// <param name="object">The object to add.</param>
        private void ActiveAddLast(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.None)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.Active;

            if (tailActive == null)
            {
                headActive = @object;
                @object.Previous = null;
                @object.Next = null;
            }
            else
            {
                tailActive.Next = @object;
                @object.Previous = tailActive;
                @object.Next = null;
            }
            tailActive = @object;

            active++;
        }

        /// <summary>
        /// Removes an object from the available list.
        /// </summary>
        /// <param name="object">The object to remove.</param>
        private void ActiveRemove(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.Active)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.None;

            if (headActive == @object)
                headActive = headActive.Next;

            if (tailActive == @object)
                tailActive = tailActive.Previous;

            var prev = @object.Previous;
            var next = @object.Next;

            if (prev != null)
                prev.Next = next;

            if (next != null)
                next.Previous = prev;

            @object.Previous = null;
            @object.Next = null;

            active--;
        }

        /// <summary>
        /// Adds an object to the front of the available list.
        /// </summary>
        /// <param name="object">The object to add.</param>
        private void AvailableAddFirst(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.None)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.Available;

            if (headAvailable == null)
            {
                tailAvailable = @object;
                @object.Previous = null;
                @object.Next = null;
            }
            else
            {
                headAvailable.Previous = @object;
                @object.Previous = null;
                @object.Next = headAvailable;
            }
            headAvailable = @object;

            available++;
        }

        /// <summary>
        /// Adds an object to the end of the available list.
        /// </summary>
        /// <param name="object">The object to add.</param>
        private void AvailableAddLast(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.None)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.Available;

            if (tailAvailable == null)
            {
                headAvailable = @object;
                @object.Previous = null;
                @object.Next = null;
            }
            else
            {
                tailAvailable.Next = @object;
                @object.Previous = tailAvailable;
                @object.Next = null;
            }
            tailAvailable = @object;

            available++;
        }

        /// <summary>
        /// Removes an object from the available list.
        /// </summary>
        /// <param name="object">The object to remove.</param>
        private void AvailableRemove(IPooledObject @object)
        {
            if (@object.CurrentPoolList != UpfPoolList.Available)
                throw new InvalidOperationException();

            @object.CurrentPoolList = UpfPoolList.None;

            if (headAvailable == @object)
                headAvailable = headAvailable.Next;

            if (tailAvailable == @object)
                tailAvailable = tailAvailable.Previous;

            var prev = @object.Previous;
            var next = @object.Next;

            if (prev != null)
                prev.Next = next;

            if (next != null)
                next.Previous = prev;

            @object.Previous = null;
            @object.Next = null;

            available--;
        }

        // State values.
        private readonly Func<TPooledType> allocator;
        private IPooledObject headActive;
        private IPooledObject tailActive;
        private IPooledObject headAvailable;
        private IPooledObject tailAvailable;
        private Int32 watermark;
        private Int32 active;
        private Int32 available;
        private Int32[] gcCounts;
    }
}
