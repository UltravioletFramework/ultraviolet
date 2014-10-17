using System.Runtime.InteropServices;
using System.Security;

namespace TwistedLogik.Nucleus.Performance
{
    partial class GCDebug
    {
        /// <summary>
        /// Contains native methods for interacting with CLR Profiler.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("ProfilerOBJ.dll", CharSet = CharSet.Unicode)]
            public static extern void LogComment(string comment);

            [DllImport("ProfilerOBJ.dll")]
            public static extern bool GetAllocationLoggingActive();

            [DllImport("ProfilerOBJ.dll")]
            public static extern void SetAllocationLoggingActive(bool active);

            [DllImport("ProfilerOBJ.dll")]
            public static extern bool GetCallLoggingActive();

            [DllImport("ProfilerOBJ.dll")]
            public static extern void SetCallLoggingActive(bool active);

            [DllImport("ProfilerOBJ.dll")]
            public static extern bool DumpHeap(uint timeOut);
        }
    }
}
