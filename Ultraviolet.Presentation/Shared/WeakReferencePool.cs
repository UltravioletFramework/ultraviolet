using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation
{
    internal partial class WeakReferencePool : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferencePool"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public WeakReferencePool(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Retrieves a weak reference from the pool.
        /// </summary>
        /// <returns>The weak reference that was retrieved.</returns>
        public WeakReference Retrieve()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            return pool.Retrieve();
        }

        /// <summary>
        /// Releases a weak reference into the pool.
        /// </summary>
        /// <param name="weakReference">The weak reference to release into the pool.</param>
        public void Release(WeakReference weakReference)
        {
            Contract.Require(weakReference, nameof(weakReference));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            weakReference.Target = null;
            pool.Release(weakReference);
        }

        /// <summary>
        /// Releases a weak reference into the pool.
        /// </summary>
        /// <param name="weakReference">The weak reference to release into the pool.</param>
        public void ReleaseRef(ref WeakReference weakReference)
        {
            Contract.Require(weakReference, nameof(weakReference));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            weakReference.Target = null;
            pool.Release(weakReference);

            weakReference = null;
        }

        /// <summary>
        /// Initializes the pool.
        /// </summary>
        public void Initialize()
        {
            if (pool != null)
                return;

            this.pool = new ExpandingPool<WeakReference>(16, 64, () => new WeakReference(null));
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static WeakReferencePool Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Gets the number of active objects which have been allocated from the pool.
        /// </summary>
        public Int32 Active
        {
            get { return (pool == null) ? 0 : pool.Count; }
        }

        /// <summary>
        /// Gets the number of available objects in the pool.
        /// </summary>
        public Int32 Available
        {
            get { return (pool == null) ? 0 : pool.Capacity - pool.Count; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                SafeDispose.DisposeRef(ref pool);
            }

            base.Dispose(disposing);
        }

        // The pool of available weak references.
        private ExpandingPool<WeakReference> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<WeakReferencePool> instance =
            new UltravioletSingleton<WeakReferencePool>(uv => new WeakReferencePool(uv));
    }
}
