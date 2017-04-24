using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the levels of debug information provided by the Ultraviolet Framework.
    /// </summary>
    [Flags]
    public enum DebugLevels
    {
        /// <summary>
        /// No debug level.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Informational messages.
        /// </summary>
        Info = 0x0001,

        /// <summary>
        /// Non-critical warning messages.
        /// </summary>
        Warning = 0x0002,

        /// <summary>
        /// Critical error messages.
        /// </summary>
        Error = 0x0004,

        /// <summary>
        /// All debug levels.
        /// </summary>
        All = Info | Warning | Error,
    }
}
