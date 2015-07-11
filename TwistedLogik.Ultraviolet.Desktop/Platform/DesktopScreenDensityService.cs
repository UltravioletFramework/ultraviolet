using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for desktop platforms.
    /// </summary>
    public sealed unsafe partial class DesktopScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public DesktopScreenDensityService(UltravioletContext uv, IUltravioletDisplay display)
            : base(display)
        {
            if (InitWindows8_1(uv, display))
                return;

            InitFallback(uv, display);
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
                left   = display.Bounds.Left,
                top    = display.Bounds.Top,
                right  = display.Bounds.Right,
                bottom = display.Bounds.Bottom 
            };

            var hmonitor = IntPtr.Zero;

            Win32.EnumDisplayMonitors(IntPtr.Zero, &rect, (hdc, lprcClip, lprcMonitor, dwData) =>
            {
                hmonitor = hdc;
                return false;
            }, IntPtr.Zero);

            if (hmonitor == IntPtr.Zero)
                throw new InvalidOperationException("TODO");

            UInt32 x, y;
            Win32.GetDpiForMonitor(hmonitor, 0, out x, out y);

            this.densityX     = x;
            this.densityY     = y;
            this.densityScale = x / 96f;

            return true;
        }

        /// <summary>
        /// Retrieves DPI information in the general case.
        /// </summary>
        private Boolean InitFallback(UltravioletContext uv, IUltravioletDisplay display)
        {
            using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                this.densityX     = graphics.DpiX;
                this.densityY     = graphics.DpiY;
                this.densityScale = graphics.DpiX / 96f;
            }
            return true;
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
            get { return ScreenDensityBucket.Desktop; }
        }

        // State values.
        private Single densityX;
        private Single densityY;
        private Single densityScale;
    }
}
