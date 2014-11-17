using System;
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

        /// <summary>
        /// Gets the display's rotation on devices which can be rotated.
        /// </summary>
        ScreenRotation Rotation
        {
            get;
        }

        /// <summary>
        /// Gets the display's density in dots per inch along the horizontal axis.
        /// </summary>
        Single DpiX
        {
            get;
        }

        /// <summary>
        /// Gets the display's density in dots per inch along the vertical axis.
        /// </summary>
        Single DpiY
        {
            get;
        }

        /// <summary>
        /// Gets the display's density bucket.
        /// </summary>
        ScreenDensityBucket DensityBucket
        {
            get;
        }
    }
}
