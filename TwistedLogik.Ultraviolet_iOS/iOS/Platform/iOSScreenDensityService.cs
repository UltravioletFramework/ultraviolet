using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.iOS.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for the iOS platform.
    /// </summary>
    public sealed class iOSScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iOSScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public iOSScreenDensityService(IUltravioletDisplay display)
            : base(display)
        {
        }

        /// <inheritdoc/>
        public override Single DensityScale
        {
            get
            {
                // TODO
                return 1f;
            }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get
            {
                // TODO
                return 96f;
            }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get
            {
                // TODO
                return 96f;
            }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get
            {
                // TODO
                return ScreenDensityBucket.Desktop;
            }
        }
    }
}