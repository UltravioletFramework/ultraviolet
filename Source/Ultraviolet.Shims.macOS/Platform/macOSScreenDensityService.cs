using System;
using AppKit;
using Foundation;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOS.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for macOS.
    /// </summary>
	public sealed class macOSScreenDensityService : ScreenDensityService
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="macOSScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public macOSScreenDensityService(IUltravioletDisplay display)
            : base(display)
        {
            var screen = NSScreen.Screens[display.Index];
            deviceScale = (Single)screen.BackingScaleFactor;

            var density = ((NSValue)screen.DeviceDescription["NSDeviceResolution"]).CGSizeValue;
            densityX = (Single)density.Width * (96f / 72f);
            densityY = (Single)density.Height * (96f / 72f);

            var displayIsScaled = !UltravioletContext.DemandCurrent().SupportsHighDensityDisplayModes;
            if (displayIsScaled)
            {
                densityX /= deviceScale;
                densityY /= deviceScale;
            }

            densityScale = densityX / 96f;
            densityBucket = GuessBucketFromDensityScale(densityScale);
        }

        /// <inheritdoc/>
        public override Single DeviceScale => deviceScale;

        /// <inheritdoc/>
        public override Single DensityScale => densityScale;

        /// <inheritdoc/>
        public override Single DensityX => densityX;

        /// <inheritdoc/>
        public override Single DensityY => densityY;

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket => densityBucket;

        // Property values.
        private readonly Single deviceScale;
        private readonly Single densityScale;
        private readonly Single densityX;
        private readonly Single densityY;
        private readonly ScreenDensityBucket densityBucket;
	}
}

