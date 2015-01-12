using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

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
        }

        /// <summary>
        /// Retrieves a clock from the pool.
        /// </summary>
        /// <param name="loopBehavior">A <see cref="LoopBehavior"/> value specifying the clock's loop behavior.</param>
        /// <param name="duration">The clock's duration.</param>
        /// <returns>The clock that was retrieved.</returns>
        public SimpleClock Retrieve(LoopBehavior loopBehavior, TimeSpan duration)
        {
            var clock = pool.Retrieve();
            clock.HandleRetrieved(pool, loopBehavior, duration);
            activeClocks.AddLast(clock);
            return clock;
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void Release(SimpleClock clock)
        {
            Contract.Require(clock, "clock");

            clock.HandleReleased();
            activeClocks.Remove(clock);
            pool.Release(clock);
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void ReleaseRef(ref SimpleClock clock)
        {
            Contract.Require(clock, "clock");

            clock.HandleReleased();
            activeClocks.Remove(clock);
            pool.ReleaseRef(ref clock);
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
            for (var node = activeClocks.First; node != null; node = node.Next)
            {
                node.Value.Update(time);
            }
        }

        // The list of active clock objects.
        private PooledLinkedList<SimpleClock> activeClocks = 
            new PooledLinkedList<SimpleClock>(32);

        // The pool of available clock objects.
        private ExpandingPool<SimpleClock> pool = 
            new ExpandingPool<SimpleClock>(32, () => new SimpleClock(LoopBehavior.None, TimeSpan.Zero));

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<SimpleClockPool> instance = 
            new UltravioletSingleton<SimpleClockPool>((uv) => new SimpleClockPool(uv));
    }
}
