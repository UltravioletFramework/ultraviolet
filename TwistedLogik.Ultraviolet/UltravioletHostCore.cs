using System;
using System.Diagnostics;
using System.Threading;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Contains core functionality for Ultraviolet host processes.
    /// </summary>
    public sealed class UltravioletHostCore : IUltravioletComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletHostCore"/> class.
        /// </summary>
        /// <param name="host">The Ultraviolet host.</param>
        public UltravioletHostCore(IUltravioletHost host)
        {
            Contract.Require(host, "host");

            this.host = host;
            this.tickTimer.Start();
        }

        /// <summary>
        /// Resets the timers used to determine how much time has passed since the last calls
        /// to <see cref="UltravioletContext.Draw"/> and <see cref="UltravioletContext.Update"/>.
        /// </summary>
        public void ResetElapsed()
        {
            tickTimer.Restart();
            frameTimer.Restart();
        }

        /// <summary>
        /// Advances the application state by one tick when the application is in a suspended state.
        /// </summary>
        public void RunOneTickSuspended()
        {
            host.Ultraviolet.UpdateSuspended();
            if (InactiveSleepTime.TotalMilliseconds > 0)
            {
                Thread.Sleep(InactiveSleepTime);
            }
        }

        /// <summary>
        /// Advances the application state by one tick.
        /// </summary>
        public void RunOneTick()
        {
            var uv = host.Ultraviolet;

            if (!host.IsActive && InactiveSleepTime.TotalMilliseconds > 0)
                Thread.Sleep(InactiveSleepTime);
 
            uv.ProcessWorkItems();

            if (IsFixedTimeStep)
            {
                if (tickTimer.Elapsed.TotalMilliseconds >= targetElapsedTime.TotalMilliseconds)
                {
                    tickTimer.Restart();
                    tickElapsed -= targetElapsedTime.TotalMilliseconds * (int)(tickElapsed / targetElapsedTime.TotalMilliseconds);

                    const Double CatchUpThreshold = 1.05;
                    if (frameElapsed > 0 && frameElapsed > targetElapsedTime.TotalMilliseconds * CatchUpThreshold)
                    {
                        const Int32 RunningSlowlyFrameCount = 5;
                        runningSlowlyFrames = RunningSlowlyFrameCount;
                        isRunningSlowly = true;

                        const Int32 CatchUpFrameLimit = 10;
                        var catchUpUpdates = Math.Min(CatchUpFrameLimit, (int)(frameElapsed / targetElapsedTime.TotalMilliseconds));

                        for (int i = 0; i < catchUpUpdates; i++)
                        {
                            timeTrackerUpdate.Increment(targetElapsedTime, isRunningSlowly);
                            timeTrackerDraw.Increment(targetElapsedTime, isRunningSlowly);
                            if (!UpdateContext(uv, timeTrackerUpdate.Time))
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (isRunningSlowly)
                        {
                            runningSlowlyFrames--;
                            if (runningSlowlyFrames == 0)
                            {
                                isRunningSlowly = false;
                            }
                        }
                    }

                    frameTimer.Restart();

                    var uvTimeUpdate = timeTrackerUpdate.Increment(targetElapsedTime, isRunningSlowly);
                    if (!UpdateContext(uv, uvTimeUpdate))
                    {
                        return;
                    }

                    var uvTimeDraw = timeTrackerDraw.Increment(targetElapsedTime, isRunningSlowly);
                    uv.Draw(uvTimeDraw);

                    frameElapsed = frameTimer.Elapsed.TotalMilliseconds;
                }
            }
            else
            {
                var time = tickTimer.Elapsed.TotalMilliseconds;
                tickTimer.Restart();

                var uvTimeDelta  = TimeSpan.FromTicks((long)(time * TimeSpan.TicksPerMillisecond));
                var uvTimeUpdate = timeTrackerUpdate.Increment(uvTimeDelta, false);
                if (!UpdateContext(uv, uvTimeUpdate))
                {
                    return;
                }

                var uvTimeDraw = uvTimeUpdate;
                uv.Draw(uvTimeDraw);
            }
        }

        /// <summary>
        /// Gets the default value for TargetElapsedTime.
        /// </summary>
        public static TimeSpan DefaultTargetElapsedTime
        {
            get { return defaultTargetElapsedTime; }
        }

        /// <summary>
        /// Gets the default value for InactiveSleepTime.
        /// </summary>
        public static TimeSpan DefaultInactiveSleepTime
        {
            get { return defaultInactiveSleepTime; }
        }

        /// <summary>
        /// Gets the default value for IsFixedTimeStep.
        /// </summary>
        public static Boolean DefaultIsFixedTimeStep
        {
            get { return defaultIsFixedTimeStep; }
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return host.Ultraviolet; }
        }

        /// <summary>
        /// Gets or sets the target time between frames when the application is running on a fixed time step.
        /// </summary>
        public TimeSpan TargetElapsedTime
        {
            get { return targetElapsedTime; }
            set { targetElapsedTime = value; }
        }

        /// <summary>
        /// Gets or sets the amount of time to sleep every frame when
        /// the application's primary window is inactive.
        /// </summary>
        public TimeSpan InactiveSleepTime
        {
            get { return inactiveSleepTime; }
            set { inactiveSleepTime = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the application is running on a fixed time step.
        /// </summary>
        public Boolean IsFixedTimeStep
        {
            get { return isFixedTimeStep; }
            set { isFixedTimeStep = value; }
        }

        /// <summary>
        /// Updates the specified context.
        /// </summary>
        /// <param name="uv">The Ultraviolet context to update.</param>
        /// <param name="time">Time elapsed since the last update.</param>
        /// <returns><c>true</c> if the host should continue processing; otherwise, <c>false</c>.</returns>
        private Boolean UpdateContext(UltravioletContext uv, UltravioletTime time)
        {
            uv.Update(time);
            return !uv.Disposed;
        }

        // The Ultraviolet host.
        private readonly IUltravioletHost host;

        // Default values.
        private static readonly TimeSpan defaultTargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
        private static readonly TimeSpan defaultInactiveSleepTime = TimeSpan.FromMilliseconds(20);
        private static readonly Boolean defaultIsFixedTimeStep = true;

        // Current tick state.
        private readonly UltravioletTimeTracker timeTrackerUpdate = new UltravioletTimeTracker();
        private readonly UltravioletTimeTracker timeTrackerDraw   = new UltravioletTimeTracker();
        private readonly Stopwatch tickTimer  = new Stopwatch();
        private readonly Stopwatch frameTimer = new Stopwatch();
        private Double tickElapsed;
        private Double frameElapsed;
        private TimeSpan targetElapsedTime = defaultTargetElapsedTime;
        private TimeSpan inactiveSleepTime = defaultInactiveSleepTime;
        private Boolean isFixedTimeStep    = defaultIsFixedTimeStep;
        private Boolean isRunningSlowly    = false;
        private Int32 runningSlowlyFrames;
    }
}
