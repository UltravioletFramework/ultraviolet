using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents a variable which is sliding from one value to another.
    /// </summary>
    internal class SlidingValue
    {
        /// <summary>
        /// Initializes a new instance of the SlidingValue class.
        /// </summary>
        /// <param name="getter">An action which retrieved the value of the sliding variable.</param>
        /// <param name="setter">An action which updates the sliding variable.</param>
        public SlidingValue(Func<Single> getter, Action<Single> setter)
        {
            Contract.Require(getter, "getter");
            Contract.Require(setter, "setter");

            this.getter = getter;
            this.setter = setter;
        }

        /// <summary>
        /// Slides the variable to the specified target value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="duration">The number of milliseconds over which to slide the value.</param>
        public void Slide(Single target, Double duration)
        {
            Contract.EnsureRange(duration >= 0, "duration");

            Stop();

            if (duration == 0)
            {
                setter(target);
            }
            else
            {
                this.sliding  = true;
                this.origin   = getter();
                this.target   = target;
                this.elapsed  = 0;
                this.duration = duration;
            }
        }

        /// <summary>
        /// Stops sliding.
        /// </summary>
        public void Stop()
        {
            sliding = false;
        }

        /// <summary>
        /// Updates the sliding variable's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public void Update(UltravioletTime time)
        {
            if (!sliding)
                return;

            elapsed += time.ElapsedTime.TotalMilliseconds;
            if (elapsed >= duration)
            {
                elapsed = duration;
                sliding = false;
            }

            var interpolationFactor = (Single)(elapsed / duration);
            var interpolatedValue = MathUtil.Lerp(origin, target, interpolationFactor);

            setter(interpolatedValue);
        }

        // State values.
        private Boolean sliding;
        private Single origin;
        private Single target;
        private Double elapsed;
        private Double duration;
        private Func<Single> getter;
        private Action<Single> setter;
    }
}
