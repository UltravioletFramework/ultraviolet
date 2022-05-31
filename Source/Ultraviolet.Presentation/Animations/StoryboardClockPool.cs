using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the Presentation Foundation's internal pool of <see cref="StoryboardClock"/> objects.
    /// </summary>
    internal partial class StoryboardClockPool : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardClockPool"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private StoryboardClockPool(UltravioletContext uv)
            : base(uv)
        {
            uv.GetUI().Updating += StoryboardClockPool_Updating;
        }

        /// <summary>
        /// Retrieves a clock from the pool.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance that will be driven by this clock.</param>
        /// <returns>The clock that was retrieved.</returns>
        public UpfPool<StoryboardClock>.PooledObject Retrieve(StoryboardInstance storyboardInstance)
        {
            Contract.Require(storyboardInstance, nameof(storyboardInstance));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            var clock = pool.Retrieve(storyboardInstance);

            clock.Value.StoryboardInstance = storyboardInstance;
            return clock;
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void Release(UpfPool<StoryboardClock>.PooledObject clock)
        {
            Contract.Require(clock, nameof(clock));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            clock.Value.StoryboardInstance.Stop();
            clock.Value.StoryboardInstance = null;
            pool.Release(clock);
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void ReleaseRef(ref UpfPool<StoryboardClock>.PooledObject clock)
        {
            Contract.Require(clock, nameof(clock));
            Contract.EnsureNotDisposed(this, Disposed);

            Initialize();

            clock.Value.StoryboardInstance.Stop();
            clock.Value.StoryboardInstance = null;
            pool.Release(clock);

            clock = null;
        }

        /// <summary>
        /// Ensures that the underlying pool exists.
        /// </summary>
        public void Initialize()
        {
            if (pool != null)
                return;

            this.pool = new PoolImpl(Ultraviolet, 32, 256, () => new StoryboardClock());
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static StoryboardClockPool Instance
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
                Ultraviolet.GetUI().Updating -= StoryboardClockPool_Updating;

                SafeDispose.DisposeRef(ref pool);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the active clock instances when the UI subsystem is updated.
        /// </summary>
        private void StoryboardClockPool_Updating(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            if (pool == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.BeginUpdate();

            pool.Update(time, (value, state) =>
            {
                value.Value.Update((UltravioletTime)state);
            });

            upf.PerformanceStats.EndUpdate();
        }

        // Storyboard clocks.
        private UpfPool<StoryboardClock> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<StoryboardClockPool> instance = 
            new UltravioletSingleton<StoryboardClockPool>(uv => new StoryboardClockPool(uv));
    }
}
