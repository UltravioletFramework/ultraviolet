using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvTestRunner.Models
{
    /// <summary>
    /// Represents the response to a query regarding the status of a test run.
    /// </summary>
    public class TestRunStatusResponse
    {
        /// <summary>
        /// Gets or sets the test run's identifier within the database.
        /// </summary>
        public Int64 TestRunID { get; set; }

        /// <summary>
        /// Gets or sets the test run's current status.
        /// </summary>
        public TestRunStatus TestRunStatus { get; set; }
    }
}
