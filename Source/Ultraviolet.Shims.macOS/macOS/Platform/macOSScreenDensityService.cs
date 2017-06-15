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
            this.display = display;

            Refresh();
        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            var oldDeviceScale = deviceScale;
            var oldDensityX = densityX;
            var oldDensityY = densityY;
            var oldDensityScale = densityScale;
            var oldDensityBucket = densityBucket;

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

            return
                oldDeviceScale != deviceScale ||
                oldDensityX != densityX ||
                oldDensityY != densityY ||
                oldDensityScale != densityScale ||
                oldDensityBucket != densityBucket;
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
        private readonly IUltravioletDisplay display;
        private Single deviceScale;
        private Single densityScale;
        private Single densityX;
        private Single densityY;
        private ScreenDensityBucket densityBucket;
	}
}

