using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a display mode.
    /// </summary>
    public sealed class DisplayMode
    {
        /// <summary>
        /// Initializes a new instance of the DisplayMode class.
        /// </summary>
        public DisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");
            Contract.EnsureRange(bpp > 0, "bpp");
            Contract.EnsureRange(refresh > 0, "refresh");

            this.Width = width;
            this.Height = height;
            this.BitsPerPixel = bpp;
            this.RefreshRate = refresh;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("{0}x{1}x{2} @{3}", Width, Height, BitsPerPixel, RefreshRate);
        }

        /// <summary>
        /// Gets the window's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the window's height in pixels.
        /// </summary>
        public Int32 Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the display's bit depth.
        /// </summary>
        public Int32 BitsPerPixel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the refresh rate in hertz.
        /// </summary>
        public Int32 RefreshRate
        {
            get;
            private set;
        }
    }
}
