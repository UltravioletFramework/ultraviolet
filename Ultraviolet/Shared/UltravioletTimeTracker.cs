using System;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for tracking the amount of time that has passed since an Ultraviolet context was created.
    /// </summary>
    public sealed class UltravioletTimeTracker
    {
        /// <summary>
        /// Resets the time.
        /// </summary>
        /// <returns>The Ultraviolet time value after the reset has been applied.</returns>
        public UltravioletTime Reset()
        {
            time.ElapsedTime     = TimeSpan.Zero;
            time.TotalTime       = TimeSpan.Zero;
            time.IsRunningSlowly = false;
            return time;
        }

        /// <summary>
        /// Increments the time.
        /// </summary>
        /// <param name="ts">The amount by which to increment the time.</param>
        /// <param name="isRunningSlowly">A value indicating whether the application's main loop is taking longer than its target time.</param>
        /// <returns>The Ultraviolet time value after the increment has been applied.</returns>
        public UltravioletTime Increment(TimeSpan ts, Boolean isRunningSlowly)
        {
            time.ElapsedTime = ts;
            time.TotalTime = time.TotalTime.Add(ts);
            time.IsRunningSlowly = isRunningSlowly;
            return time;
        }

        /// <summary>
        /// Gets the current Ultraviolet time value.
        /// </summary>
        public UltravioletTime Time
        {
            get { return time; }
        }

        // The Ultraviolet time value for the current context.
        private readonly UltravioletTime time = new UltravioletTime();
    }
}
