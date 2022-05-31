using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a set of values which are used to control the creation of Ultraviolet work items.
    /// </summary>
    [Flags]
    public enum WorkItemOptions
    {
        /// <summary>
        /// No options specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the task should be forced to run asynchronously, even if it
        /// was spawned on the Ultraviolet context thread.
        /// </summary>
        ForceAsynchronousExecution = 1,

        /// <summary>
        /// Indicates that the task returned can be <see langword="null"/> if the work
        /// item was allowed to be executed synchronously.
        /// </summary>
        ReturnNullOnSynchronousExecution = 2,
    }
}
