using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains identifiers for the Presentation Foundation's profiler sections.
    /// </summary>
    public static class PresentationProfilerSection
    {
        /// <summary>
        /// Identifies the profiler section that starts at the beginning of <see cref="PresentationFoundation.PerformLayout"/> and
        /// ends at the end of that method.
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
