using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
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

            this.pool = new PoolImpl(uv, 32, 256, () => new StoryboardInstance());
        }

        /// <summary>
        /// Retrieves a storyboard instance from the pool.
        /// </summary>
        /// <param name="owner">The object that owns the storyboard instance.</param>
        /// <returns>The storyboard instance that was retrieved.</returns>
        public UpfPool<StoryboardInstance>.PooledObject Retrieve(Object owner)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return pool.Retrieve(owner);
        }

        /// <summary>
        /// Releases a storyboard instance into the pool.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance to release into the pool.</param>
        public void Release(UpfPool<StoryboardInstance>.PooledObject storyboardInstance)
        {
            Contract.Require(storyboardInstance, "storyboardInstance");
            Contract.EnsureNotDisposed(this, Disposed);
            
            pool.Release(storyboardInstance);
        }

        /// <summary>
        /// Releases a storyboard instance into the pool.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance to release into the pool.</param>
        public void ReleaseRef(ref UpfPool<StoryboardInstance>.PooledObject storyboardInstance)
        {
            Contract.Require(storyboardInstance, "storyboardInstance");
            Contract.EnsureNotDisposed(this, Disposed);
            
            pool.Release(storyboardInstance);

            storyboardInstance = null;
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static StoryboardInstancePool Instance
        {
            get { return instance.Value; }
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
            pool.Update();
        }

        // The pool of available clock objects.
        private UpfPool<StoryboardInstance> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<StoryboardInstancePool> instance =
            new UltravioletSingleton<StoryboardInstancePool>((uv) => new StoryboardInstancePool(uv));
    }
}
