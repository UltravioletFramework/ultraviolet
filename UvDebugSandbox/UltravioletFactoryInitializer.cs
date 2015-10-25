using System.Diagnostics;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Profiler.dotTrace;

namespace UvDebugSandbox
{
    /// <summary>
    /// Represents the factory initializer for this application.
    /// </summary>
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
            factory.SetFactoryMethod<UltravioletProfilerFactory>(uv => new dotTraceProfiler(uv));
        }
    }
}
