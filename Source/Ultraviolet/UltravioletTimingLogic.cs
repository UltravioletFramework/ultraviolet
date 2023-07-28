using System;
using System.Diagnostics;
using System.Threading;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Contains core functionality for Ultraviolet host processes.
    /// </summary>
    public sealed partial class UltravioletTimingLogic : IUltravioletTimingLogic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletTimingLogic"/> class.
        /// </summary>
        /// <param name="host">The Ultraviolet host.</param>
        public UltravioletTimingLogic(IUltravioletHost host)
        {
            Contract.Require(host, nameof(host));

            this.host = host;
            this.tickTimer.Start();
        }

        /// <inheritdoc/>
        public void ResetElapsed()
        {
            tickTimer.Restart();
            if (!IsFixedTimeStep)
            {
                forceElapsedTimeToZero = true;
            }
        }

        /// <inheritdoc/>
        public void RunOneTickSuspended()
        {
            var uv = host.Ultraviolet;

            UpdateSystemTimerResolution();

            uv.UpdateSuspended();
            if (InactiveSleepTime.Ticks > 0)
            {
                Thread.Sleep(InactiveSleepTime);
            }
        }

        /// <inheritdoc/>
        public void RunOneTick()
        {
            var uv = host.Ultraviolet;

            if (SynchronizationContext.Current is UltravioletSynchronizationContext syncContext)
                syncContext.ProcessWorkItems();

            UpdateSystemTimerResolution();

            if (InactiveSleepTime.Ticks > 0 && !host.IsActive)
                Thread.Sleep(InactiveSleepTime);

            var elapsedTicks = tickTimer.Elapsed.Ticks;
            tickTimer.Restart();

            accumulatedElapsedTime += elapsedTicks;
            if (accumulatedElapsedTime > MaxElapsedTime.Ticks)
                accumulatedElapsedTime = MaxElapsedTime.Ticks;

            var gameTicksToRun = 0;
            var timeDeltaDraw = default(TimeSpan);
            var timeDeltaUpdate = default(TimeSpan);

            if (IsFixedTimeStep)
            {
                gameTicksToRun = (Int32)(accumulatedElapsedTime / TargetElapsedTime.Ticks);
                if (gameTicksToRun > 0)
                {
                    lagFrames += (gameTicksToRun == 1) ? -1 : Math.Max(0, gameTicksToRun - 1);

                    if (lagFrames == 0)
                        runningSlowly = false;
                    if (lagFrames > 5)
                        runningSlowly = true;

                    timeDeltaUpdate = TargetElapsedTime;
                    timeDeltaDraw = TimeSpan.FromTicks(gameTicksToRun * TargetElapsedTime.Ticks);
                    accumulatedElapsedTime -= gameTicksToRun * TargetElapsedTime.Ticks;
                }
                else
                {
                    var frameDelay = (Int32)(TargetElapsedTime.TotalMilliseconds - tickTimer.Elapsed.TotalMilliseconds);
                    if (frameDelay >= 1 + systemTimerPeriod)
                    {
                        Thread.Sleep(frameDelay - 1);
                    }
                    return;
                }
            }
            else
            {
                gameTicksToRun = 1;
                if (forceElapsedTimeToZero)
                {
                    timeDeltaUpdate = TimeSpan.Zero;
                    forceElapsedTimeToZero = false;
                }
                else
                {
                    timeDeltaUpdate = TimeSpan.FromTicks(elapsedTicks);
                    timeDeltaDraw = timeDeltaUpdate;
                }
                accumulatedElapsedTime = 0;
                runningSlowly = false;
            }

            if (gameTicksToRun == 0)
                return;

            uv.HandleFrameStart();

            for (var i = 0; i < gameTicksToRun; i++)
            {
                var updateTime = timeTrackerUpdate.Increment(timeDeltaUpdate, runningSlowly);
                if (!UpdateContext(uv, updateTime))
                {
                    return;
                }
            }

            if (!host.IsSuspended)
            {
                var drawTime = timeTrackerDraw.Increment(timeDeltaDraw, runningSlowly);
                using (UltravioletProfiler.Section(UltravioletProfilerSections.Draw))
                {
                    uv.Draw(drawTime);
                }
            }

            uv.HandleFrameEnd();
        }

        /// <inheritdoc/>
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
        public static TimeSpan DefaultTargetElapsedTime { get; } = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);

        /// <summary>
        /// Gets the default value for InactiveSleepTime.
        /// </summary>
        public static TimeSpan DefaultInactiveSleepTime { get; } = TimeSpan.FromMilliseconds(20);

        /// <summary>
        /// Gets the default value for IsFixedTimeStep.
        /// </summary>
        public static Boolean DefaultIsFixedTimeStep { get; } = true;

        /// <inheritdoc/>
        public UltravioletContext Ultraviolet
        {
            get { return host.Ultraviolet; }
        }

        /// <inheritdoc/>
        public TimeSpan TargetElapsedTime { get; set; } = DefaultTargetElapsedTime;

        /// <inheritdoc/>
        public TimeSpan InactiveSleepTime { get; set; } = DefaultInactiveSleepTime;

        /// <inheritdoc/>
        public Boolean IsFixedTimeStep { get; set; } = DefaultIsFixedTimeStep;

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

            var requiredTimerPeriod = Math.Max(1u, host.IsActive ? (IsFixedTimeStep ? 1u : 15u) : (UInt32)InactiveSleepTime.TotalMilliseconds);
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

        // Current tick state.
        private static readonly TimeSpan MaxElapsedTime = TimeSpan.FromMilliseconds(500);
        private readonly UltravioletTimeTracker timeTrackerUpdate = new UltravioletTimeTracker();
        private readonly UltravioletTimeTracker timeTrackerDraw = new UltravioletTimeTracker();
        private readonly Stopwatch tickTimer = new Stopwatch();
        private Int64 accumulatedElapsedTime;
        private Int32 lagFrames;
        private Boolean runningSlowly;
        private Boolean forceElapsedTimeToZero;

        // Current system timer resolution.
        private UInt32 systemTimerPeriod;
    }
}
