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
        /// Gets or sets the GPU vendor for which the tests were run.
        /// </summary>
        public GpuVendor Vendor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the friendly display name for the test run's associated hardware vendor.
        /// </summary>
        public String VendorDisplayName
        {
            get
            {
                switch (Vendor)
                {
                    case GpuVendor.Intel:
                        return "Intel HD Graphics";
                    case GpuVendor.Nvidia:
                        return "NVIDIA";
                    case GpuVendor.Amd:
                        return "AMD";
                }
                return "unknown";
            }
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