using System;
using System.Collections.Generic;
using JetBrains.dotTrace.Api;
using Ultraviolet.Core;

namespace Ultraviolet.Profiler.dotTrace
{
    /// <summary>
    /// Represents an Ultraviolet profiler that uses the JetBrains dotTrace API for profiling.
    /// </summary>
    public partial class dotTraceProfiler : UltravioletProfilerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="dotTraceProfiler"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public dotTraceProfiler(UltravioletContext uv)
            : base(uv)
        {
            uv.FrameStart += HandleFrameStart;
            uv.FrameEnd += HandleFrameEnd;
        }

        /// <summary>
        /// Registers <see cref="dotTraceProfiler"/> as the profiler for the current Ultraviolet context.
        /// </summary>
        /// <param name="owner">The Ultraviolet context with which to register the profiler.</param>
        /// <param name="factory">The Ultraviolet factory for the Ultraviolet context.</param>
        /// <remarks>This method must be called during the application's initialization phase, before any 
        /// of the static methods on <see cref="UltravioletProfiler"/> have been invoked.</remarks>
        public static void RegisterProfiler(UltravioletContext owner, UltravioletFactory factory)
        {
            Contract.Require(owner, nameof(owner));
            Contract.Require(factory, nameof(factory));

            factory.RemoveFactoryMethod<UltravioletProfilerFactory>();
            factory.SetFactoryMethod<UltravioletProfilerFactory>(uv => new dotTraceProfiler(uv));
        }

        /// <inheritdoc/>
        public override void TakeSnapshotOfNextFrame()
        {
            snapshotNextFrame = true;
        }

        /// <inheritdoc/>
        public override void BeginSection(String name)
        {
            if (IsSectionActive(name))
                throw new InvalidOperationException(dotTraceStrings.SectionAlreadyActive);

            var controlState = UpdateSectionState(name, true, null);
            HandleControlState(name, controlState);
        }

        /// <inheritdoc/>
        public override void EndSection(String name)
        {
            if (!IsSectionActive(name))
                throw new InvalidOperationException(dotTraceStrings.SectionNotActive);

            var controlState = UpdateSectionState(name, false, null);
            HandleControlState(name, controlState);
        }

        /// <inheritdoc/>
        public override void BeginSnapshot()
        {
            if (snapshotting)
                throw new InvalidOperationException(dotTraceStrings.SnapshotAlreadyInProgress);

            snapshotting = true;

            if (numberOfActiveEnabledSections > 0)
            {
                ProfilingControl.Start();
            }
            else
            {
                ProfilingControl.StartPaused();
            }
        }

        /// <inheritdoc/>
        public override void EndSnapshot()
        {
            if (!snapshotting)
                throw new InvalidOperationException(dotTraceStrings.SnapshotNotInProgress);

            if (snapshotNextFrameInProgress)
                throw new InvalidOperationException(dotTraceStrings.SnapshottingFrame);

            snapshotting = false;

            ProfilingControl.Stop();
        }
        
        /// <inheritdoc/>
        public override void EnableSection(String name)
        {
            if (IsSectionEnabled(name))
                throw new InvalidOperationException(dotTraceStrings.SectionAlreadyEnabled);

            var controlState = UpdateSectionState(name, null, true);
            HandleControlState(name, controlState);
        }

        /// <inheritdoc/>
        public override void DisableSection(String name)
        {
            if (!IsSectionEnabled(name))
                throw new InvalidOperationException(dotTraceStrings.SectionNotEnabled);

            var controlState = UpdateSectionState(name, null, false);
            HandleControlState(name, controlState);
        }

        /// <inheritdoc/>
        public override Boolean IsTakingSnapshot
        {
            get { return snapshotting; }
        }

        /// <inheritdoc/>
        public override Boolean IsTakingSnapshotNextFrame
        {
            get { return snapshotNextFrame; }
        }

        /// <summary>
        /// Updates the state of the specified profiling section.
        /// </summary>
        private ControlState UpdateSectionState(String name, Boolean? active, Boolean? enabled)
        {
            var activeAndEnabledPrev = IsSectionActiveAndEnabled(name);
            var activeAndEnabledCountPrev = numberOfActiveEnabledSections;

            if (active.HasValue)
                activeSections[name] = active.Value;

            if (enabled.HasValue)
                enabledSections[name] = enabled.Value;

            var activeAndEnabledCurrent = IsSectionActiveAndEnabled(name);
            if (activeAndEnabledCurrent != activeAndEnabledPrev)
            {
                if (activeAndEnabledCurrent)
                {
                    numberOfActiveEnabledSections++;
                }
                else
                {
                    numberOfActiveEnabledSections--;
                }
            }

            if (activeAndEnabledCountPrev > 0 && numberOfActiveEnabledSections == 0)
                return ControlState.Pause;

            if (activeAndEnabledCountPrev == 0 && numberOfActiveEnabledSections > 0)
                return ControlState.Resume;

            return ControlState.Unchanged;
        }

        /// <summary>
        /// Gets a value indicating whether the specified profiling section is currently active.
        /// </summary>
        private Boolean IsSectionActive(String name)
        {
            Boolean active;
            return activeSections.TryGetValue(name, out active) && active;
        }

        /// <summary>
        /// Gets a value indicating whether the specified profiling section is currently enabled.
        /// </summary>
        private Boolean IsSectionEnabled(String name)
        {
            Boolean enabled;
            return enabledSections.TryGetValue(name, out enabled) && enabled;
        }

        /// <summary>
        /// Gets a value indicating whether the specified profiling section is both active and enabled.
        /// </summary>
        private Boolean IsSectionActiveAndEnabled(String name)
        {
            return IsSectionActive(name) && IsSectionEnabled(name);
        }

        /// <summary>
        /// Processes the specified change to the profiler's control state.
        /// </summary>
        private void HandleControlState(String name, ControlState state)
        {
            switch (state)
            {
                case ControlState.Unchanged:
                    break;

                case ControlState.Resume:
                    if (snapshotting)
                    {
                        ProfilingControl.Resume();
                    }
                    break;
                
                case ControlState.Pause:
                    if (snapshotting)
                    {
                        ProfilingControl.Pause();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        /// <summary>
        /// Called at the start of a frame.
        /// </summary>
        public void HandleFrameStart(UltravioletContext uv)
        {
            if (snapshotNextFrame && !snapshotting)
            {
                snapshotNextFrame = false;
                snapshotNextFrameInProgress = true;
                BeginSnapshot();
            }
        }

        /// <summary>
        /// Called at the end of a frame.
        /// </summary>
        private void HandleFrameEnd(UltravioletContext uv)
        {
            if (snapshotNextFrameInProgress && snapshotting)
            {
                snapshotNextFrameInProgress = false;
                EndSnapshot();
            }
        }

        // Tracks which sections are currently enabled and active.
        private readonly Dictionary<String, Boolean> enabledSections = new Dictionary<String, Boolean>();
        private readonly Dictionary<String, Boolean> activeSections = new Dictionary<String, Boolean>();

        // Profiler state.
        private Boolean snapshotting;
        private Boolean snapshotNextFrame;
        private Boolean snapshotNextFrameInProgress;
        private Int32 numberOfActiveEnabledSections;
    }
}