using System;

namespace UvTestRunner.Models
{
    /// <summary>
    /// Represents the response to a test run creation request.
    /// </summary>
    public class TestRunCreationResponse
    {
        /// <summary>
        /// Gets or sets the test run's identifier within the database.
        /// </summary>
        public Int64 TestRunID { get; set; }
    }
}
