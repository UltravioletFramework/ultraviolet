namespace Ultraviolet.Profiler.dotTrace
{
    public enum dotTraceControlState
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
