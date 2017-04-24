using System;
using System.Collections.Generic;

namespace Ultraviolet.Platform
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
        /// <returns>The converted value in display pixels.</returns>
        Double InchesToPixels(Double inches);

        /// <summary>
        /// Converts display pixels to inches.
        /// </summary>
        /// <param name="pixels">The value in display pixels to convert.</param>
        /// <returns>The converted value in inches.</returns>
        Double PixelsToInches(Double pixels);

        /// <summary>
        /// Converts display independent pixels to display pixels.
        /// </summary>
        /// <param name="dips">The value in display independent pixels to convert.</param>
        /// <returns>The converted value in display pixels.</returns>
        Double DipsToPixels(Double dips);

        /// <summary>
        /// Converts display pixels to display independent pixels.
        /// </summary>
        /// <param name="pixels">The value in display pixels to convert.</param>
        /// <returns>The converted value in display independent pixels.</returns>
        Double PixelsToDips(Double pixels);

        /// <summary>
        /// Converts inches to display independent pixels.
        /// </summary>
        /// <param name="inches">The value in inches to convert.</param>
        /// <returns>The converted value in display independent pixels.</returns>
        Double InchesToDips(Double inches);

        /// <summary>
        /// Converts display independent pixels to inches.
        /// </summary>
        /// <param name="dips">The value in display independent pixels to convert.</param>
        /// <returns>The converted value in inches.</returns>
        Double DipsToInches(Double dips);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in inches to a <see cref="Point2D"/>
        /// value with coordinates in display pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Point2D"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in display pixels.</returns>
        Point2D InchesToPixels(Point2D inches);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in display pixels to a <see cref="Point2D"/>
        /// value with coordinates in inches.
        /// </summary>
        /// <param name="pixels">The <see cref="Point2D"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in inches.</returns>
        Point2D PixelsToInches(Point2D pixels);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in display independent pixels to 
        /// a <see cref="Point2D"/> value with coordinates in display pixels.
        /// </summary>
        /// <param name="dips">The <see cref="Point2D"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in display pixels.</returns>
        Point2D DipsToPixels(Point2D dips);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in display pixels to a <see cref="Point2D"/>
        /// value with coordinates in display independent pixels.
        /// </summary>
        /// <param name="pixels">The <see cref="Point2D"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in display independent pixels.</returns>
        Point2D PixelsToDips(Point2D pixels);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in inches to a <see cref="Point2D"/>
        /// value with coordinates in display independent pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Point2D"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in display independent pixels.</returns>
        Point2D InchesToDips(Point2D inches);

        /// <summary>
        /// Converts a <see cref="Point2D"/> value with coordinates in display independent pixels to a <see cref="Point2D"/>
        /// value with coordinates in inches.
        /// </summary>
        /// <param name="dips">The <see cref="Point2D"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Point2D"/> in inches.</returns>
        Point2D DipsToInches(Point2D dips);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with dimensions in inches to a <see cref="Size2D"/>
        /// value with dimensions in display pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Size2D"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in display pixels.</returns>
        Size2D InchesToPixels(Size2D inches);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with coordinates in display pixels to a <see cref="Size2D"/>
        /// value with coordinates in inches.
        /// </summary>
        /// <param name="pixels">The <see cref="Size2D"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in inches.</returns>
        Size2D PixelsToInches(Size2D pixels);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with dimensions in display independent pixels to 
        /// a <see cref="Size2D"/> value with dimensions in display pixels.
        /// </summary>
        /// <param name="dips">The <see cref="Size2D"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in display pixels.</returns>
        Size2D DipsToPixels(Size2D dips);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with dimensions in display pixels to a <see cref="Size2D"/>
        /// value with dimensions in display independent pixels.
        /// </summary>
        /// <param name="pixels">The <see cref="Size2D"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in display independent pixels.</returns>
        Size2D PixelsToDips(Size2D pixels);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with dimensions in inches to a <see cref="Size2D"/>
        /// value with dimensions in display independent pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Size2D"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in display independent pixels.</returns>
        Size2D InchesToDips(Size2D inches);

        /// <summary>
        /// Converts a <see cref="Size2D"/> value with dimensions in display independent pixels to a <see cref="Size2D"/>
        /// value with dimensions in inches.
        /// </summary>
        /// <param name="dips">The <see cref="Size2D"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Size2D"/> in inches.</returns>
        Size2D DipsToInches(Size2D dips);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in inches 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in display pixels.
        /// </summary>
        /// <param name="inches">The <see cref="RectangleD"/> in inches to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in display pixels.</returns>
        RectangleD InchesToPixels(RectangleD inches);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in display pixels 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in inches.
        /// </summary>
        /// <param name="pixels">The <see cref="RectangleD"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in inches.</returns>
        RectangleD PixelsToInches(RectangleD pixels);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in display independent pixels 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in display pixels.
        /// </summary>
        /// <param name="dips">The <see cref="RectangleD"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in display pixels.</returns>
        RectangleD DipsToPixels(RectangleD dips);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in display pixels 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in display independent pixels.
        /// </summary>
        /// <param name="pixels">The <see cref="RectangleD"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in display independent pixels.</returns>
        RectangleD PixelsToDips(RectangleD pixels);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in inches 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in display independent pixels.
        /// </summary>
        /// <param name="inches">The <see cref="RectangleD"/> in inches to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in display independent pixels.</returns>
        RectangleD InchesToDips(RectangleD inches);

        /// <summary>
        /// Converts a <see cref="RectangleD"/> value with coordinates and dimensions in display independent pixels 
        /// to a <see cref="RectangleD"/> value with coordinates and dimensions in inches.
        /// </summary>
        /// <param name="dips">The <see cref="RectangleD"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="RectangleD"/> in inches.</returns>
        RectangleD DipsToInches(RectangleD dips);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in inches 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in display pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Vector2"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in display pixels.</returns>
        Vector2 InchesToPixels(Vector2 inches);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in display pixels 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in inches.
        /// </summary>
        /// <param name="pixels">The <see cref="Vector2"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in inches.</returns>
        Vector2 PixelsToInches(Vector2 pixels);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in display independent pixels 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in display pixels.
        /// </summary>
        /// <param name="dips">The <see cref="Vector2"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in display pixels.</returns>
        Vector2 DipsToPixels(Vector2 dips);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in display pixels 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in display independent pixels.
        /// </summary>
        /// <param name="pixels">The <see cref="Vector2"/> in display pixels to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in display independent pixels.</returns>
        Vector2 PixelsToDips(Vector2 pixels);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in inches 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in display independent pixels.
        /// </summary>
        /// <param name="inches">The <see cref="Vector2"/> in inches to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in display independent pixels.</returns>
        Vector2 InchesToDips(Vector2 inches);

        /// <summary>
        /// Converts a <see cref="Vector2"/> value with coordinates and dimensions in display independent pixels 
        /// to a <see cref="Vector2"/> value with coordinates and dimensions in inches.
        /// </summary>
        /// <param name="dips">The <see cref="Vector2"/> in display independent pixels to convert.</param>
        /// <returns>The converted <see cref="Vector2"/> in inches.</returns>
        Vector2 DipsToInches(Vector2 dips);
        
        /// <summary>
        /// Gets the display's index within Ultraviolet's display list.
        /// </summary>
        Int32 Index
        {
            get;
        }

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        String Name
        {
            get;
        }

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
        /// Gets the number of physical pixels per logical pixel on devices with high density display modes
        /// like Retina or Retina HD.
        /// </summary>
        Single DeviceScale
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

        /// <summary>
        /// Gets the desktop display mode for this display.
        /// </summary>
        DisplayMode DesktopDisplayMode
        {
            get;
        }
    }
}
