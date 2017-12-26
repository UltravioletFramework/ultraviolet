using System;

namespace Ultraviolet.Profiler.dotTrace
{
    /// <summary>
    /// Represents an Ultraviolet profiler that uses the JetBrains dotTrace API for profiling.
    /// </summary>
    public class dotTraceProfiler : UltravioletProfilerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="dotTraceProfiler"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public dotTraceProfiler(UltravioletContext uv)
            : base(uv)
        {

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

        }

        /// <inheritdoc/>
        public override void TakeSnapshotOfNextFrame()
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void BeginSection(String name)
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void EndSection(String name)
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void BeginSnapshot()
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void EndSnapshot()
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void EnableSection(String name)
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override void DisableSection(String name)
        {
            // Not supported on Android
        }

        /// <inheritdoc/>
        public override Boolean IsTakingSnapshot
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public override Boolean IsTakingSnapshotNextFrame
        {
            get { return false; }
        }
    }
}