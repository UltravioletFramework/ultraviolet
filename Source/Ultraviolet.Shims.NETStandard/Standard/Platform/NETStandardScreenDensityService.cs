using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETStandard.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for the .NET Standard 2.0 platform.
    /// </summary>
    public sealed partial class NETStandardScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public NETStandardScreenDensityService(UltravioletContext uv, IUltravioletDisplay display)
            : base(display)
        {
            Contract.Require(uv, nameof(uv));

            this.uv = uv;
            this.display = display;

            Refresh();
        }

        /// <summary>
        /// Retrieves DPI information when running on Windows 8.1 and higher.
        /// </summary>
        private Boolean InitWindows8_1(UltravioletContext uv, IUltravioletDisplay display)
        {
            if (uv.Platform != UltravioletPlatform.Windows || Environment.OSVersion.Version < new Version(6, 3))
                return false;

            var rect = new Win32.RECT
            {
                left = display.Bounds.Left,
                top = display.Bounds.Top,
                right = display.Bounds.Right,
                bottom = display.Bounds.Bottom
            };

            var hmonitor = IntPtr.Zero;

            unsafe
            {
                Win32.EnumDisplayMonitors(IntPtr.Zero, &rect, (hdc, lprcClip, lprcMonitor, dwData) =>
                {
                    hmonitor = hdc;
                    return false;
                }, IntPtr.Zero);
            }

            if (hmonitor == IntPtr.Zero)
                return false;

            UInt32 x, y;
            Win32.GetDpiForMonitor(hmonitor, 0, out x, out y);

            this.densityX = x;
            this.densityY = y;
            this.densityScale = x / 96f;
            this.densityBucket = GuessBucketFromDensityScale(densityScale);

            return true;
        }

        /// <summary>
        /// Retrieves DPI information on macOS when the Mac-specific compatibility shim is missing.
        /// </summary>
        private Boolean InitMacOS(UltravioletContext uv, IUltravioletDisplay display)
        {
            if (uv.Platform != UltravioletPlatform.macOS)
                return false;

            this.densityX = 96f;
            this.densityY = 96f;
            this.densityScale = 1.0f;

            return true;
        }

        /// <summary>
        /// Retrieves DPI information in the general case.
        /// </summary>
        private Boolean InitFallback(UltravioletContext uv, IUltravioletDisplay display)
        {
            using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                this.densityX = graphics.DpiX;
                this.densityY = graphics.DpiY;
                this.densityScale = graphics.DpiX / 96f;
                this.densityBucket = GuessBucketFromDensityScale(densityScale);
            }
            return true;
        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            var oldDensityX = densityX;
            var oldDensityY = densityY;
            var oldDensityScale = densityScale;
            var oldDensityBucket = densityBucket;

            if (!InitWindows8_1(uv, display) && !InitMacOS(uv, display))
                InitFallback(uv, display);

            return oldDensityX != densityX || oldDensityY != densityY || oldDensityScale != densityScale || oldDensityBucket != densityBucket;
        }

        /// <inheritdoc/>
        public override Single DeviceScale
        {
            get { return 1f; }
        }

        /// <inheritdoc/>
        public override Single DensityScale
        {
            get { return densityScale; }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get { return densityX; }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get { return densityY; }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get { return densityBucket; }
        }

        // State values.
        private readonly UltravioletContext uv;
        private readonly IUltravioletDisplay display;
        private Single densityX;
        private Single densityY;
        private Single densityScale;
        private ScreenDensityBucket densityBucket;
    }
}