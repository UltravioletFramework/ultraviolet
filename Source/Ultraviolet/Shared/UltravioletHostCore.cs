using System;
using System.Diagnostics;
using System.Threading;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Contains core functionality for Ultraviolet host processes.
    /// </summary>
    public sealed partial class UltravioletHostCore : IUltravioletComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletHostCore"/> class.
        /// </summary>
        /// <param name="host">The Ultraviolet host.</param>
        public UltravioletHostCore(IUltravioletHost host)
        {
            Contract.Require(host, nameof(host));

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
        /// Advances the application state while the application is suspended.
        /// </summary>
        public void RunOneTickSuspended()
        {
            var uv = host.Ultraviolet;

            UpdateSystemTimerResolution();

            uv.UpdateSuspended();
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

            UpdateSystemTimerResolution();

            if (!host.IsActive && InactiveSleepTime.TotalMilliseconds > 0)
                Thread.Sleep(InactiveSleepTime);

            var syncContext = SynchronizationContext.Current as UltravioletSynchronizationContext;
            if (syncContext != null)
                syncContext.ProcessWorkItems();

            if (IsFixedTimeStep)
            {
                if (tickTimer.Elapsed.TotalMilliseconds >= targetElapsedTime.TotalMilliseconds)
                {
                    tickTimer.Restart();
                    tickElapsed -= targetElapsedTime.TotalMilliseconds * (int)(tickElapsed / targetElapsedTime.TotalMilliseconds);

                    uv.HandleFrameStart();

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

                    if (!host.IsSuspended)
                    {
                        var uvTimeDraw = timeTrackerDraw.Increment(targetElapsedTime, isRunningSlowly);
                        using (UltravioletProfiler.Section(UltravioletProfilerSections.Draw))
                        {
                            uv.Draw(uvTimeDraw);
                        }
                    }

                    uv.HandleFrameEnd();

                    frameElapsed = frameTimer.Elapsed.TotalMilliseconds;
                }
                else
                {
                    var frameDelay = (int)(targetElapsedTime.TotalMilliseconds - tickTimer.Elapsed.TotalMilliseconds);
                    if (frameDelay >= 1 + systemTimerPeriod)
                    {
                        Thread.Sleep(frameDelay - 1);
                    }
                }
            }
            else
            {
                var time = tickTimer.Elapsed.TotalMilliseconds;
                tickTimer.Restart();

                uv.HandleFrameStart();

                var uvTimeDelta  = TimeSpan.FromTicks((long)(time * TimeSpan.TicksPerMillisecond));
                var uvTimeUpdate = timeTrackerUpdate.Increment(uvTimeDelta, false);
                if (!UpdateContext(uv, uvTimeUpdate))
                {
                    return;
                }

                if (!host.IsSuspended)
                {
                    var uvTimeDraw = uvTimeUpdate;
                    using (UltravioletProfiler.Section(UltravioletProfilerSections.Draw))
                    {
                        uv.Draw(uvTimeDraw);
                    }
                }

                uv.HandleFrameEnd();
            }
        }

        /// <summary>
        /// Cleans up any state after the application has finished its run loop.
        /// </summary>
        public void Cleanup()
        {
            if (Ultraviolet.Platform == UltravioletPlatform.Windows)
            {
                if (systemTimerPeriod > 0)
                    Win32Native.timeEndPeriod(systemTimerPeriod);

                systemTimerPeriod = 0;
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
        /// <returns><see langword="true"/> if the host should continue processing; otherwise, <see langword="false"/>.</returns>
        private Boolean UpdateContext(UltravioletContext uv, UltravioletTime time)
        {
            using (UltravioletProfiler.Section(UltravioletProfilerSections.Update))
            {
                uv.Update(time);
            }
            return !uv.Disposed;
        }

        /// <summary>
        /// Updates the resolution of the system timer on platforms which require it.
        /// </summary>
        private Boolean UpdateSystemTimerResolution()
        {
            if (Ultraviolet.Platform != UltravioletPlatform.Windows)
            {
                systemTimerPeriod = 1u;
                return false;
            }

            var requiredTimerPeriod = Math.Max(1u, host.IsActive ? (IsFixedTimeStep ? 1u: 15u) : (UInt32)InactiveSleepTime.TotalMilliseconds);
            if (requiredTimerPeriod != systemTimerPeriod)
            {
                if (systemTimerPeriod > 0)
                    Win32Native.timeEndPeriod(systemTimerPeriod);

                var result = Win32Native.timeBeginPeriod(requiredTimerPeriod);
                systemTimerPeriod = requiredTimerPeriod;
                return (result == 0);
            }

            return false;
        }

        // The Ultraviolet host.
        private readonly IUltravioletHost host;

        // Default values.
        private static readonly TimeSpan defaultTargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
        private static readonly TimeSpan defaultInactiveSleepTime = TimeSpan.FromMilliseconds(20);
        private static readonly Boolean defaultIsFixedTimeStep = true;

        // Current tick state.
        private readonly UltravioletTimeTracker timeTrackerUpdate = new UltravioletTimeTracker();
        private readonly UltravioletTimeTracker timeTrackerDraw = new UltravioletTimeTracker();
        private readonly Stopwatch tickTimer = new Stopwatch();
        private readonly Stopwatch frameTimer = new Stopwatch();
        private Double tickElapsed;
        private Double frameElapsed;
        private TimeSpan targetElapsedTime = defaultTargetElapsedTime;
        private TimeSpan inactiveSleepTime = defaultInactiveSleepTime;
        private Boolean isFixedTimeStep = defaultIsFixedTimeStep;
        private Boolean isRunningSlowly = false;
        private Int32 runningSlowlyFrames;

        // Current system timer resolution.
        private UInt32 systemTimerPeriod;
    }
}
