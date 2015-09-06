using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a pool of objects used by the Presentation Foundation.
    /// </summary>
    /// <typeparam name="TPooledType">The type of object which is being pooled.</typeparam>
    internal partial class PresentationFoundationPool<TPooledType> : UltravioletResource where TPooledType : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationPool{TPooledType}"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="capacity">The pool's initial capacity.</param>
        /// <param name="watermark">The pool's high watermark value.</param>
        /// <param name="allocator">The pool's allocator function.</param>
        public PresentationFoundationPool(UltravioletContext uv, Int32 capacity, Int32 watermark, Func<TPooledType> allocator)
            : base(uv)
        {
            Contract.Require(allocator, "allocator");
            Contract.EnsureRange(capacity >= 0, "capacity");
            Contract.EnsureRange(watermark >= 0, "watermark");

            this.gcCounts = new Int32[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            this.watermark = watermark;
            this.allocator = allocator;
            this.pool = new List<Assignment>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                var assignment = new Assignment(allocator());
                pool.Add(assignment);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool and assigns it to the specified owner.
        /// </summary>
        /// <param name="owner">The element that owns the pooled object.</param>
        /// <returns>The object that was retrieved from the pool.</returns>
        public TPooledType Retrieve(UIElement owner)
        {
            Contract.Require(owner, "owner");

            if (active == pool.Count)
            {
                if (active >= watermark)
                {
                    return allocator();
                }
                else
                {
                    var assignment = new Assignment(allocator());
                    assignment.Assign(owner);
                    pool.Add(assignment);
                    active++;
                    return assignment.Object;
                }
            }
            else
            {
                var assignment = pool[active++];
                assignment.Assign(owner);
                return assignment.Object;
            }
        }

        /// <summary>
        /// Releases the specified object owned by the specified element back into the pool.
        /// </summary>
        /// <param name="owner">The element that owns the object to release.</param>
        /// <param name="object">The object to release back into the pool.</param>
        public void Release(UIElement owner, TPooledType @object)
        {
            Contract.Require(owner, "owner");
            Contract.Require(@object, "object");

            for (int i = 0; i < active; i++)
            {
                var assignment = pool[i];
                if (assignment.Owner.Target == owner && assignment.Object == @object)
                {
                    var last = pool[active - 1];
                    pool[i] = last;
                    pool[active - 1] = assignment;
                    assignment.Assign(null);
                    active--;
                    return;
                }
            }

            // The object was not returned to the pool (probably because it was created after hitting the
            // high watermark); make sure it gets disposed if needed
            var disposable = @object as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Updates the state of the pool. Any objects which belong to owners that have been garbage
        /// collected will be returned to the pool.
        /// </summary>
        public void Update()
        {
            var requiresUpdate = false;
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                var count = GC.CollectionCount(i);
                if (count != gcCounts[i])
                {
                    requiresUpdate = true;
                    gcCounts[i] = count;
                }
            }

            if (!requiresUpdate)
                return;

            var anyObjectsWereReleased = false;

            for (int i = 0; i < active; i++)
            {
                var assignment = pool[i];
                if (assignment.IsCollected)
                {
                    assignment.Assign(null);
                    anyObjectsWereReleased = true;
                }
            }

            if (anyObjectsWereReleased)
            {
                SortPool();
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
        /// Gets the pool's high watermark value.
        /// </summary>
        public Int32 Watermark
        {
            get { return watermark; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            foreach (var assignment in pool)
            {
                var disposable = assignment.Object as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sorts the pool so that active objects come first and inactive objects come second.
        /// </summary>
        private void SortPool()
        {
            var ixCurrent = 0;
            var ixActive = 0;

            while (ixCurrent < pool.Count)
            {
                if (!pool[ixCurrent].IsActive)
                {
                    var swapped = false;

                    for (int i = ixCurrent + 1; i < pool.Count; i++)
                    {
                        if (pool[i].IsActive)
                        {
                            ixActive = i;

                            var temp = pool[i];
                            pool[i] = pool[ixCurrent];
                            pool[ixCurrent] = temp;

                            swapped = true;
                        }
                    }

                    if (!swapped)
                        return;
                }
                ixCurrent++;
            }
        }

        // State values.
        private readonly Func<TPooledType> allocator;
        private readonly List<Assignment> pool;
        private Int32 watermark;
        private Int32 active;
        private Int32[] gcCounts;
    }
}
