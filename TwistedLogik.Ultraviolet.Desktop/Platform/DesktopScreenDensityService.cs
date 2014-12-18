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

        /// <inheritdoc/>
        public override Single DensityScale
        {
            get { return 1f; }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get { return 96f; }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get { return 96f; }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get { return ScreenDensityBucket.Desktop; }
        }
    }
}
