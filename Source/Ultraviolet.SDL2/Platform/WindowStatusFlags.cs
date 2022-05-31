using System;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents flags which can be applied to a window in order to track various states.
    /// </summary>
    [Flags]
    internal enum WindowStatusFlags : byte
    {
        /// <summary>
        /// The window has no status flags.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The window is an OpenGL window.
        /// </summary>
        OpenGL = 0x01,
        
        /// <summary>
        /// The window is a Vulkan window.
        /// </summary>
        Vulkan = 0x02,

        // 0x04 - Reserved (Unknown)

        // 0x08 - Reserved (Unknown)

        /// <summary>
        /// The window has input focus.
        /// </summary>
        Focused = 0x10,

        /// <summary>
        /// The window is minimized.
        /// </summary>
        Minimized = 0x20,

        /// <summary>
        /// The window hasn't been shown yet.
        /// </summary>
        Unshown = 0x40,

        /// <summary>
        /// The window has been bound for rendering.
        /// </summary>
        BoundForRendering = 0x80,
    }
}
