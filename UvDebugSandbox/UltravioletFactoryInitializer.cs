using System.Diagnostics;
using Ultraviolet;
using Ultraviolet.Core;

namespace UvDebugSandbox
{
    /// <summary>
    /// Represents the factory initializer for this application.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class UltravioletFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            InitializeProfilingFactoryMethods(owner, factory);
        }

        /// <summary>
        /// Initializes factory methods used for performance profiling.
        /// </summary>
        [Conditional("ENABLE_PROFILING")]
        private void InitializeProfilingFactoryMethods(UltravioletContext owner, UltravioletFactory factory)
        {
            // dotTraceProfiler.RegisterProfiler(owner, factory);
        }
    }
}
