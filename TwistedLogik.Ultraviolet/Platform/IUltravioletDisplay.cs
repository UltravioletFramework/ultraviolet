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
        /// Converts inches to display pixels.
        /// </summary>
        /// <param name="inches">The value in inches to convert.</param>
        /// <returns>The converted value in display units.</returns>
        Double InchesToPixels(Double inches);

        /// <summary>
        /// Converts display independent pixels (1/96 of an inch) to display pixels.
        /// </summary>
        /// <param name="dips">The value in display independent units to convert.</param>
        /// <returns>The converted value in display units.</returns>
        Double DipsToPixels(Double dips);

        /// <summary>
        /// Converts inches to display independent pixels (1/96 of an inch).
        /// </summary>
        /// <param name="inches">The value in inches to convert.</param>
        /// <returns>The converted value in display independent units.</returns>
        Double InchesToDips(Double inches);

        /// <summary>
        /// Converts display pixels to display independent pixels (1/96 of an inch).
        /// </summary>
        /// <param name="pixels">The value in display units to convert.</param>
        /// <returns>The converted value in display independent units.</returns>
        Double PixelsToDips(Double pixels);

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
        /// Gets the scaling factor for device independent pixels.
        /// </summary>
        Single DensityScale
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
