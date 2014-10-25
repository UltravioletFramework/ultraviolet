using System;
using System.Diagnostics;
using System.Security;

namespace TwistedLogik.Nucleus.Performance
{
    /// <summary>
    /// Contains methods for profiling and debugging garbage collection.
    /// </summary>
    [SecuritySafeCritical]
    public static partial class GCDebug
    {
        /// <summary>
        /// Initializes the <see cref="GCDebug"/> type.
        /// </summary>
        static GCDebug()
        {
            try
            {
                var active = NativeMethods.GetAllocationLoggingActive();
                NativeMethods.SetAllocationLoggingActive(!active);
                isProfilerAttached = (active != NativeMethods.GetAllocationLoggingActive());
            }
            catch (DllNotFoundException) { isProfilerAttached = false; }
        }

        /// <summary>
        /// Dumps the contents of the managed heap to the attached profiler, if a profiler is attached.
        /// </summary>
        [Conditional("GCDEBUG")]
        public static void DumpHeap()
        {
            if (!isProfilerAttached)
                return;

            NativeMethods.DumpHeap(60000);
        }

        /// <summary>
        /// Writes the specified line of text to the allocation profiler.
        /// </summary>
        /// <param name="line">The line to write to the allocation profiler.</param>
        [Conditional("GCDEBUG")]
        public static void WriteLine(String line)
        {
            if (!isProfilerAttached)
                return;

            NativeMethods.LogComment(line);
        }

        /// <summary>
        /// Writes the specified line of formatted text to the allocation profiler.
        /// </summary>
        /// <param name="fmt">The format string.</param>
        /// <param name="args">The array of objects to write using <paramref name="fmt"/>.</param>
        [Conditional("GCDEBUG")]
        public static void WriteLine(String fmt, params Object[] args)
        {
            if (!isProfilerAttached)
                return;

            NativeMethods.LogComment(String.Format(fmt, args));
        }

        /// <summary>
        /// Activates or deactivates allocation logging.
        /// </summary>
        /// <param name="active">A value indicating whether allocation logging is active.</param>
        [Conditional("GCDEBUG")]
        public static void SetAllocationLoggingActive(Boolean active)
        {
            if (!isProfilerAttached)
                return;

            NativeMethods.SetAllocationLoggingActive(active);
        }

        /// <summary>
        /// Gets a value indicating whether allocation logging is currently active.
        /// </summary>
        /// <returns><c>true</c> if allocation logging is currently active; otherwise, <c>false</c>.</returns>
        public static Boolean GetAllocationLoggingActive()
        {
            if (!isProfilerAttached)
                return false;

            return NativeMethods.GetAllocationLoggingActive();
        }

        /// <summary>
        /// Activates or deactivates call logging.
        /// </summary>
        /// <param name="active">A value indicating whether call logging is active.</param>
        [Conditional("GCDEBUG")]
        public static void SetCallLoggingActive(Boolean active)
        {
            if (!isProfilerAttached)
                return;

            NativeMethods.SetCallLoggingActive(active);
        }

        /// <summary>
        /// Gets a value indicating whether call logging is currently active.
        /// </summary>
        /// <returns><c>true</c> if call logging is currently active; otherwise, <c>false</c>.</returns>
        public static Boolean GetCallLoggingActive()
        {
            if (!isProfilerAttached)
                return false;

            return NativeMethods.GetCallLoggingActive();
        }

        // A value indicating whether CLR Profiler is attached and initialized.
        private static readonly Boolean isProfilerAttached;
    }
}
