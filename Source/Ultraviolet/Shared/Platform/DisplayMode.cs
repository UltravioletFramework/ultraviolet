using System;
using Ultraviolet.Core;

namespace Ultraviolet.Platform
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
        /// <param name="displayIndex">The index of the display in which to place a window when it enters this display mode,
        /// or <see langword="null"/> if the window has no preferred display.</param>
        public DisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh, Int32? displayIndex = null)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(bpp > 0, nameof(bpp));

            this.Width = width;
            this.Height = height;
            this.BitsPerPixel = bpp;
            this.RefreshRate = refresh;
            this.DisplayIndex = displayIndex;
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
        /// Determines whether the specified display mode is equivalent to this display mode by comparing
        /// their vertical and horizontal resolutions, as well as optionally comparing their bit depths
        /// and refresh rates.
        /// </summary>
        /// <param name="other">The <see cref="DisplayMode"/> instance to compare to this instance.</param>
        /// <param name="considerBitsPerPixel">A value indicating whether to compare the bit depths of the two display modes.</param>
        /// <param name="considerRefreshRate">A value indicating whether to compare the refresh rates of the two display modes.</param>
        /// <param name="considerDisplay">A value indicating whether to compare the preferred displays of the two display modes.</param>
        /// <returns><see langword="true"/> if the two display modes are equivalent; otherwise, <see langword="false"/>.</returns>
        public Boolean IsEquivalentTo(DisplayMode other, 
            Boolean considerBitsPerPixel = true, 
            Boolean considerRefreshRate = true, 
            Boolean considerDisplay = false)
        {
            if (other == null)
                return false;

            if (other == this)
                return true;

            if (other.Width != Width || other.Height != Height)
                return false;

            if (considerBitsPerPixel && other.BitsPerPixel != BitsPerPixel)
                return false;

            if (considerRefreshRate && other.RefreshRate != RefreshRate)
                return false;

            if (considerDisplay && other.DisplayIndex != DisplayIndex)
                return false;

            return true;
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
        
        /// <summary>
        /// Gets the index of the display in which to place a window when it enters this display mode,
        /// or <see langword="null"/> if the window has no preferred display.
        /// </summary>
        public Int32? DisplayIndex
        {
            get;
            private set;
        }
    }
}
