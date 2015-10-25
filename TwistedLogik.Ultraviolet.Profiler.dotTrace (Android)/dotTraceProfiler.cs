using System;

namespace TwistedLogik.Ultraviolet.Profiler.dotTrace
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
    }
}