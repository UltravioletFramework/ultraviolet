using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the Presentation Foundation's pool of <see cref="StoryboardInstance"/> objects.
    /// </summary>
    internal partial class StoryboardInstancePool : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardInstancePool"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private StoryboardInstancePool(UltravioletContext uv)
            : base(uv)
        {
            uv.GetUI().Updating += StoryboardInstancePool_Updating;
        }

        /// <summary>
        /// Retrieves a storyboard instance from the pool.
        /// </summary>
        /// <param name="owner">The object that owns the storyboard instance.</param>
        /// <returns>The storyboard instance that was retrieved.</returns>
        public UpfPool<StoryboardInstance>.PooledObject Retrieve(Object owner)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            return pool.Retrieve(owner);
        }

        /// <summary>
        /// Releases a storyboard instance into the pool.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance to release into the pool.</param>
        public void Release(UpfPool<StoryboardInstance>.PooledObject storyboardInstance)
        {
            Contract.Require(storyboardInstance, nameof(storyboardInstance));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            pool.Release(storyboardInstance);
        }

        /// <summary>
        /// Releases a storyboard instance into the pool.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance to release into the pool.</param>
        public void ReleaseRef(ref UpfPool<StoryboardInstance>.PooledObject storyboardInstance)
        {
            Contract.Require(storyboardInstance, nameof(storyboardInstance));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            pool.Release(storyboardInstance);

            storyboardInstance = null;
        }

        /// <summary>
        /// Ensures that the underlying pool exists.
        /// </summary>
        public void Initialize()
        {
            if (pool != null)
                return;

            this.pool = new PoolImpl(Ultraviolet, 32, 256, () => new StoryboardInstance());
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static StoryboardInstancePool Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Gets the number of active objects which have been allocated from the pool.
        /// </summary>
        public Int32 Active
        {
            get { return (pool == null) ? 0 : pool.Active; }
        }

        /// <summary>
        /// Gets the number of available objects in the pool.
        /// </summary>
        public Int32 Available
        {
            get { return (pool == null) ? 0 : pool.Available; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                Ultraviolet.GetUI().Updating -= StoryboardInstancePool_Updating;

                SafeDispose.DisposeRef(ref pool);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the Presentation Foundation's pool of storyboard instances when the UI subsystem is updated.
        /// </summary>
        private void StoryboardInstancePool_Updating(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            if (pool == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginUpdate();

            pool.Update();

            upf.PerformanceStats.EndUpdate();
        }

        // The pool of available clock objects.
        private UpfPool<StoryboardInstance> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<StoryboardInstancePool> instance =
            new UltravioletSingleton<StoryboardInstancePool>(uv => new StoryboardInstancePool(uv));
    }
}
