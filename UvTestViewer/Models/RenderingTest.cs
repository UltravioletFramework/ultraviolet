using System;

namespace UvTestViewer.Models
{
    /// <summary>
    /// Represents the result of a particular rendering text.
    /// </summary>
    public class RenderingTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingTest"/> class.
        /// </summary>
        /// <param name="name">The test's name.</param>
        /// <param name="expected">The path to the "expected" image.</param>
        /// <param name="actual">The path to the "actual" image.</param>
        /// <param name="diff">The path to the "diff" image.</param>
        public RenderingTest(String name, String expected, String actual, String diff)
        {
            this.Name     = name;
            this.Expected = expected;
            this.Actual   = actual;
            this.Diff     = diff;
        }

        /// <summary>
        /// Gets the test's name.
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the "expected" image.
        /// </summary>
        public String Expected
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the "actual" image.
        /// </summary>
        public String Actual
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the "diff" image.
        /// </summary>
        public String Diff
        {
            get;
            private set;
        }
    }
}