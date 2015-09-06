using System;

namespace UvTestViewer.Models
{
    /// <summary>
    /// Represents cached test information which has been retrieved from a test results file.
    /// </summary>
    public class CachedTestInfo
    {
        /// <summary>
        /// Gets or sets the test's name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the test's description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the test's status.
        /// </summary>
        public CachedTestInfoStatus Status { get; set; }
    }
}