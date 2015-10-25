using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a section of code which is being measured by a profiler.
    /// </summary>
    public struct UltravioletProfilerSection : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletProfilerSection"/> structure.
        /// </summary>
        /// <param name="name"></param>
        internal UltravioletProfilerSection(String name)
        {
            Contract.Require(name, "name");

            this.name = name;
        }

        /// <summary>
        /// Identifies the profiler section that starts at the beginning of a frame and ends at the end of a frame.
        /// </summary>
        public const String Frame = "UV_Frame";

        /// <summary>
        /// Identifies the profiler section that starts at the beginning of <see cref="UltravioletContext.Draw(UltravioletTime)"/> and
        /// ends and the end of that method.
        /// </summary>
        public const String Draw = "UV_Draw";

        /// <summary>
        /// Identifies the profiler section that starts at the beginning of <see cref="UltravioletContext.Update(UltravioletTime)"/> and
        /// ends and the end of that method.
        /// </summary>
        public const String Update = "UV_Update";

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            UltravioletProfiler.EndSection(name);
        }

        /// <summary>
        /// Gets the name of the profiler section.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        // Property values.
        private readonly String name;
    }
}
