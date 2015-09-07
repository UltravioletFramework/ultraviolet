using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents a pool of <see cref="SimpleClock"/> instances.
    /// </summary>
    internal class SimpleClockPool : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleClockPool"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private SimpleClockPool(UltravioletContext uv)
            : base(uv)
        {
            uv.GetUI().Updating += SimpleClockPool_Updating;

            this.pool = new UpfPool<SimpleClock>(uv, 32, 256, () => new SimpleClock(LoopBehavior.None, TimeSpan.Zero));
        }

        /// <summary>
        /// Retrieves a clock from the pool.
        /// </summary>
        /// <param name="owner">The object that owns the clock.</param>
        /// <returns>The clock that was retrieved.</returns>
        public UpfPool<SimpleClock>.PooledObject Retrieve(Object owner)
        {
            var clock = pool.Retrieve(owner);
            var clockValue = clock.Value;
            
            clockValue.PooledObject = clock;
            return clock;
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void Release(UpfPool<SimpleClock>.PooledObject clock)
        {
            Contract.Require(clock, "clock");

            clock.Value.PooledObject = null;
            pool.Release(clock);
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void ReleaseRef(ref UpfPool<SimpleClock>.PooledObject clock)
        {
            Contract.Require(clock, "clock");

            clock.Value.PooledObject = null;
            pool.Release(clock);

            clock = null;
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static SimpleClockPool Instance
        {
            get { return instance.Value; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Ultraviolet.Disposed)
            {
                Ultraviolet.GetUI().Updating -= SimpleClockPool_Updating;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the active clock instances when the UI subsystem is updated.
        /// </summary>
        private void SimpleClockPool_Updating(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            pool.Update(time, (value, state) =>
            {
                value.Value.Update((UltravioletTime)state);
            });
        }
        
        // The pool of available clock objects.
        private UpfPool<SimpleClock> pool;

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<SimpleClockPool> instance = 
            new UltravioletSingleton<SimpleClockPool>((uv) => new SimpleClockPool(uv));
    }
}
