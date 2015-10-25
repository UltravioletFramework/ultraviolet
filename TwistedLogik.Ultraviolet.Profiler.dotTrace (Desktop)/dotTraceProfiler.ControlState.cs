
namespace TwistedLogik.Ultraviolet.Profiler.dotTrace
{
    partial class dotTraceProfiler
    {
        /// <summary>
        /// Represents changes to the state of the dotTrace profiler.
        /// </summary>
        public enum ControlState
        {
            /// <summary>
            /// Profiling state is unchanged.
            /// </summary>
            Unchanged,

            /// <summary>
            /// Profiling should be resumed.
            /// </summary>
            Resume,

            /// <summary>
            /// Profiling should be paused.
            /// </summary>
            Pause,
        }
    }
}
