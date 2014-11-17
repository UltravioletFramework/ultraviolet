using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public DesktopScreenDensityService(IUltravioletDisplay display)
            : base(display)
        {

        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the horizontal axis.
        /// </summary>
        public override Single DensityX
        {
            get { return 72f; }
        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the vertical axis.
        /// </summary>
        public override Single DensityY
        {
            get { return 72f; }
        }

        /// <summary>
        /// Gets the screen's density bucket.
        /// </summary>
        public override ScreenDensityBucket DensityBucket
        {
            get { return ScreenDensityBucket.Desktop; }
        }
    }
}
