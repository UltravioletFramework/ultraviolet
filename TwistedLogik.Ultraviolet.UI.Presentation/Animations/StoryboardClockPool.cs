using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents the Presentation Foundation's internal pool of <see cref="StoryboardClock"/> objects.
    /// </summary>
    internal class StoryboardClockPool : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardClockPool"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private StoryboardClockPool(UltravioletContext uv)
            : base(uv)
        {
            uv.GetUI().Updating += StoryboardClockPool_Updating;

            this.pool = new UpfPool<StoryboardClock>(uv, 32, 256, () => new StoryboardClock());
        }

        /// <summary>
        /// Retrieves a clock from the pool.
        /// </summary>
        /// <param name="storyboard">The storyboard that will be driven by this clock.</param>
        /// <returns>The clock that was retrieved.</returns>
        public UpfPool<StoryboardClock>.PooledObject Retrieve(Storyboard storyboard)
        {
            Contract.Require(storyboard, "storyboard");

            var clock = pool.Retrieve(storyboard);

            clock.Value.Storyboard = storyboard;
            return clock;
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void Release(UpfPool<StoryboardClock>.PooledObject clock)
        {
            Contract.Require(clock, "clock");

            clock.Value.Storyboard = null;
            pool.Release(clock);
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void ReleaseRef(ref UpfPool<StoryboardClock>.PooledObject clock)
        {
            Contract.Require(clock, "clock");

            clock.Value.Storyboard = null;
            pool.Release(clock);

            clock = null;
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static StoryboardClockPool Instance
        {
            get { return instance.Value; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                Ultraviolet.GetUI().Updating -= StoryboardClockPool_Updating;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the active clock instances when the UI subsystem is updated.
        /// </summary>
        private void StoryboardClockPool_Updating(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            pool.Update(time, (value, state) =>
            {
                value.Value.Update((UltravioletTime)state);
            });
        }

        // Storyboard clocks.
        private UpfPool<StoryboardClock> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<StoryboardClockPool> instance = 
            new UltravioletSingleton<StoryboardClockPool>((uv) => new StoryboardClockPool(uv));
    }
}
