using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// <para>Represents a display mode.</para>
    /// <para>A display mode encapsulates all of the parameters of a display device, 
    /// include its resolution, bit depth, and refresh rate.</para>
    /// </summary>
    public sealed class DisplayMode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMode"/> class.
        /// </summary>
        /// <param name="width">The display mode's width in pixels.</param>
        /// <param name="height">The display mode's height in pixels.</param>
        /// <param name="bpp">The display mode's bit depth.</param>
        /// <param name="refresh">The display mode's refresh rate in hertz.</param>
        public DisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");
            Contract.EnsureRange(bpp > 0, "bpp");

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
        /// Gets the display's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the display's height in pixels.
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
        /// Gets the display's refresh rate in hertz.
        /// </summary>
        public Int32 RefreshRate
        {
            get;
            private set;
        }
    }
}
