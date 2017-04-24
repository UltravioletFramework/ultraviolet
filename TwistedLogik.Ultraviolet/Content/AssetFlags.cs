using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the flags that can be used to specify the properties of a particular asset
    /// within an instance of the <see cref="ContentManager"/> class.
    /// </summary>
    [Flags]
    public enum AssetFlags
    {
        /// <summary>
        /// The asset has no special behavior.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The asset will not be removed from the content manager's cache during
        /// a <see cref="UltravioletMessages.LowMemory"/> event.
        /// </summary>
        PreserveThroughLowMemory = 0x0001,
    }
}
