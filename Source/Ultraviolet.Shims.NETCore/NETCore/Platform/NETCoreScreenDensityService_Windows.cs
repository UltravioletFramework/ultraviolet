using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for the .NET Core platform when running on Windows.
    /// </summary>
    public sealed partial class NETCoreScreenDensityService_Windows : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCoreScreenDensityService_Windows"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public NETCoreScreenDensityService_Windows(UltravioletContext uv, IUltravioletDisplay display)
            : base(display)
        {
            Contract.Require(uv, nameof(uv));

            this.uv = uv;
            this.display = display;

            Refresh();
        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            var oldDensityX = densityX;
            var oldDensityY = densityY;
            var oldDensityScale = densityScale;
            var oldDensityBucket = densityBucket;

            if (!InitWindows8_1(uv, display))
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

        /// <summary>
        /// Retrieves DPI information when running on Windows 8.1 and higher.
        /// </summary>
        private Boolean InitWindows8_1(UltravioletContext uv, IUltravioletDisplay display)
        {
            if (uv.Platform != UltravioletPlatform.Windows || Environment.OSVersion.Version < new Version(6, 3))
                return false;

            var hmonitor = IntPtr.Zero;
            var rect = new Native.Win32.RECT { left = display.Bounds.Left, top = display.Bounds.Top, right = display.Bounds.Right, bottom = display.Bounds.Bottom };

            unsafe
            {
                Native.Win32.EnumDisplayMonitors(IntPtr.Zero, &rect, (hdc, lprcClip, lprcMonitor, dwData) =>
                {
                    hmonitor = hdc;
                    return false;
                }, IntPtr.Zero);
            }

            if (hmonitor == IntPtr.Zero)
                return false;

            Native.Win32.GetDpiForMonitor(hmonitor, 0, out var x, out var y);

            this.densityX = x;
            this.densityY = y;
            this.densityScale = x / 96f;
            this.densityBucket = GuessBucketFromDensityScale(densityScale);

            return true;
        }

        /// <summary>
        /// Retrieves DPI information in the general case.
        /// </summary>
        private Boolean InitFallback(UltravioletContext uv, IUltravioletDisplay display)
        {
            //using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            //{
            //    this.densityX = graphics.DpiX;
            //    this.densityY = graphics.DpiY;
            //    this.densityScale = graphics.DpiX / 96f;
            //    this.densityBucket = GuessBucketFromDensityScale(densityScale);
            //}

            //return true;
            return false;
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