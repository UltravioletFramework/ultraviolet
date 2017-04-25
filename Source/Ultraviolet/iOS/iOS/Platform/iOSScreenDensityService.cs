using System;
using System.Runtime.InteropServices;
using Ultraviolet.Platform;
using UIKit;

namespace Ultraviolet.Shims.iOS.Platform
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
        public unsafe iOSScreenDensityService(IUltravioletDisplay display)
            : base(display)
        {
            var buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UltravioletNative.utsname_darwin)));
                if (UltravioletNative.uname(buf) < 0)
                    throw new InvalidOperationException(UltravioletStrings.UnableToRetrieveDeviceName);
                
                var deviceInfo = Marshal.PtrToStructure<UltravioletNative.utsname_darwin>(buf);
                var deviceID = deviceInfo.machine;

                deviceScale = (Single)UIScreen.MainScreen.Scale;                

                switch (deviceID)
                {
                    // Simulator
                    case "i386":
                    case "x86_64":
                        densityX = densityY = (Int32)(72 * deviceScale);
                        break;

                    // iPod Touch 1st-3rd Gen
                    case "iPod1,1":
                    case "iPod2,1":
                    case "iPod3,1":
                        densityX = densityY = 163;
                        break;

                    // iPod Touch 4th-6th Gen
                    case "iPod4,1":
                    case "iPod5,1":
                    case "iPod7,1":
                        densityX = densityY = 326;
                        break;

                    // iPhone, 3G, 3GS
                    case "iPhone1,1":
                    case "iPhone1,2":
                    case "iPhone2,1":
                        densityX = densityY = 163;
                        break;

                    // iPhone 4-6, SE
                    case "iPhone3,3":
                    case "iPhone3,1":
                    case "iPhone4,1":
                    case "iPhone5,1":
                    case "iPhone5,2":
                    case "iPhone5,3":
                    case "iPhone5,4":
                    case "iPhone6,1":
                    case "iPhone6,2":
                    case "iPhone7,2":
                    case "iPhone8,1":
                    case "iPhone8,4":
                        densityX = densityY = 326;
                        break;

                    // iPhone 6 Plus, 6S Plus
                    case "iPhone7,1":
                    case "iPhone8,2":
                        densityX = densityY = 401;
                        break;

                    // iPad
                    case "iPad1,1":
                    case "iPad2,1":
                        densityX = densityY = 132;
                        break;

                    // iPad 3rd Gen, iPad Air
                    case "iPad3,1":
                    case "iPad3,4":
                    case "iPad4,1":
                    case "iPad4,2":
                        densityX = densityY = 264;
                        break;

                    // iPad Mini
                    case "iPad2,5":
                        densityX = densityY = 163;
                        break;

                    // iPad Mini 2, Mini 3, Mini 4
                    case "iPad4,4":
                    case "iPad4,5":
                    case "iPad4,7":
                    case "iPad5,1":
                    case "iPad5,2":
                        densityX = densityY = 326;
                        break;

                    // iPad Pro
                    case "iPad6,3":
                    case "iPad6,8":
                        densityX = densityY = 264;
                        break;

                    // We don't know what this is so just blindly assume 326ppi
                    default:
                        densityX = densityY = 326;
                        break;
                }

                densityX *= 96f / 72f;
                densityY *= 96f / 72f;

                var displayIsScaled = !UltravioletContext.DemandCurrent().SupportsHighDensityDisplayModes;
                if (displayIsScaled)
                {
                    densityX /= deviceScale;
                    densityY /= deviceScale;
                }

                densityScale = densityX / 96f;
                densityBucket = GuessBucketFromDensityScale(densityScale);
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
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