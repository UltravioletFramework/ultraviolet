using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the set of flags which can be used to configure an instance of <see cref="ContentManager"/>.
    /// </summary>
    [Flags]
    public enum ContentManagerFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Supresses dependency tracking within this content manager, reducing memory usage.
        /// </summary>
        SuppressDependencyTracking = 0x0001,
    }
}
