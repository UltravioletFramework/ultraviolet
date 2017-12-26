using System;
using Ultraviolet.Core;

namespace Ultraviolet
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
            Contract.Require(name, nameof(name));

            this.name = name;
        }

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
