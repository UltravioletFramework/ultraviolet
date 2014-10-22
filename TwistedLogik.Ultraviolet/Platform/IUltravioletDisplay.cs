using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a display device.
    /// </summary>
    public interface IUltravioletDisplay
    {
        /// <summary>
        /// Gets the display device's supported display modes.
        /// </summary>
        /// <returns>A collection containing the display device's supported <see cref="DisplayMode"/> values.</returns>
        IEnumerable<DisplayMode> GetSupportedDisplayModes();

        /// <summary>
        /// Gets the display's bounds.
        /// </summary>
        Rectangle Bounds
        {
            get;
        }
    }
}
