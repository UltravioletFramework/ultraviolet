using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

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

        }

        /// <summary>
        /// Retrieves a clock from the pool.
        /// </summary>
        /// <param name="storyboard">The storyboard that will be driven by this clock.</param>
        /// <returns>The clock that was retrieved.</returns>
        public StoryboardClock Retrieve(Storyboard storyboard)
        {
            Contract.Require(storyboard, "storyboard");

            var clock = pool.Retrieve();
            clock.Storyboard = storyboard;

            return clock;
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void Release(StoryboardClock clock)
        {
            Contract.Require(clock, "clock");

            clock.Storyboard = null;
            pool.Release(clock);
        }

        /// <summary>
        /// Releases a clock into the pool.
        /// </summary>
        /// <param name="clock">The clock to release into the pool.</param>
        public void ReleaseRef(ref StoryboardClock clock)
        {
            Contract.Require(clock, "clock");

            clock.Storyboard = null;
            pool.ReleaseRef(ref clock);
        }

        /// <summary>
        /// Gets the pool's singleton instance.
        /// </summary>
        public static StoryboardClockPool Instance
        {
            get { return instance.Value; }
        }

        // Storyboard clocks.
        private ExpandingPool<StoryboardClock> pool = 
            new ExpandingPool<StoryboardClock>(64, () => new StoryboardClock());

        // The singleton instance of the clock pool.
        private static UltravioletSingleton<StoryboardClockPool> instance = 
            new UltravioletSingleton<StoryboardClockPool>((uv) => new StoryboardClockPool(uv));
    }
}
