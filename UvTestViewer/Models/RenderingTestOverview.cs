using System;
using System.Collections.Generic;
using System.Linq;

namespace UvTestViewer.Models
{
    /// <summary>
    /// The view model for the rendering test page.
    /// </summary>
    public class RenderingTestOverview
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingTestOverview"/> class.
        /// </summary>
        public RenderingTestOverview()
        {
            Tests = Enumerable.Empty<RenderingTest>();
        }

        /// <summary>
        /// Gets or sets the identifier of the test run.
        /// </summary>
        public Int64 TestRunID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time at which tests were processed.
        /// </summary>
        public DateTime TimeProcessed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the set of tests to display.
        /// </summary>
        public IEnumerable<RenderingTest> Tests
        {
            get;
            set;
        }
    }
}