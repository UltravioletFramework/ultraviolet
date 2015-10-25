using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UltravioletProfilerBase"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="UltravioletProfilerBase"/> that was created.</returns>
    public delegate UltravioletProfilerBase UltravioletProfilerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the base class for Ultraviolet profilers.
    /// </summary>
    public abstract class UltravioletProfilerBase : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletProfilerBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UltravioletProfilerBase(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Begins a profiler section.
        /// </summary>
        /// <param name="name">The name of the section to begin.</param>
        public abstract void BeginSection(String name);

        /// <summary>
        /// Ends a profiler section.
        /// </summary>
        /// <param name="name">The name of the section to begin.</param>
        public abstract void EndSection(String name);

        /// <summary>
        /// Begins a profiler snapshot.
        /// </summary>
        public abstract void BeginSnapshot();

        /// <summary>
        /// Ends the current profiler snapshot.
        /// </summary>
        public abstract void EndSnapshot();

        /// <summary>
        /// Enables a profiler section.
        /// </summary>
        /// <param name="name">The name of the section to enable.</param>
        /// <remarks>Only enabled sections will be included in profiler snapshots.</remarks>
        public abstract void EnableSection(String name);

        /// <summary>
        /// Disables a profiler section.
        /// </summary>
        /// <param name="name">The name of the section to enable.</param>
        /// <remarks>Only enabled sections will be included in profiler snapshots.</remarks>
        public abstract void DisableSection(String name);
    }
}
