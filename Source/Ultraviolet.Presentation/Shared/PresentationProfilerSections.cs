using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains identifiers for the Presentation Foundation's profiler sections.
    /// </summary>
    public static class PresentationProfilerSections
    {
        /// <summary>
        /// Identifies the profiler section that starts at the beginning of layout processing and ends
        /// when layout processing is complete.
        /// </summary>
        public const String Layout = "UPF_Layout";

        /// <summary>
        /// Identifies the profiler section that starts when the Presentation Foundation begins styling elements and ends
        /// when styling is complete.
        /// </summary>
        public const String Style = "UPF_Style";

        /// <summary>
        /// Identifies the profiler section that starts when the Presentation Foundation begins measuring elements and ends
        /// when measuring is complete.
        /// </summary>
        public const String Measure = "UPF_Measure";

        /// <summary>
        /// Identifies the profiler section that starts when the Presentation Foundation begins arranging elements and ends
        /// when arranging is complete.
        /// </summary>
        public const String Arrange = "UPF_Arrange";
    }
}
