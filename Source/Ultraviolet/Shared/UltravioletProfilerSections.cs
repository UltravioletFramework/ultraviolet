using System;

namespace Ultraviolet
{
    /// <summary>
    /// Contains identifiers for the Ultraviolet Framework's profiler sections.
    /// </summary>
    public static class UltravioletProfilerSections
    {        
        /// <summary>
        /// Identifies the profiler section that starts at the beginning of a frame and ends at the end of a frame.
        /// </summary>
        public const String Frame = "UV_Frame";

        /// <summary>
        /// Identifies the profiler section that starts at the beginning of <see cref="UltravioletContext.Draw(UltravioletTime)"/> and
        /// ends and the end of that method.
        /// </summary>
        public const String Draw = "UV_Draw";

        /// <summary>
        /// Identifies the profiler section that starts at the beginning of <see cref="UltravioletContext.Update(UltravioletTime)"/> and
        /// ends and the end of that method.
        /// </summary>
        public const String Update = "UV_Update";
    }
}
