
namespace UvTestRunner.Models
{
    /// <summary>
    /// Represents the status of a particular test run.
    /// </summary>
    public enum TestRunStatus
    {
        /// <summary>
        /// The test run has been created but has not yet started running.
        /// </summary>
        Pending,

        /// <summary>
        /// The test run is in the process of running.
        /// </summary>
        Running,

        /// <summary>
        /// The test run has completed successfully.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The test run failed to complete.
        /// </summary>
        Failed,
    }
}
