using System;
using Android.App;
using Android.Util;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public AndroidScreenDensityService(IUltravioletDisplay display)
            : base(display)
        {

        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            if (Activity == null)
                return false;

            var oldDensityScale = DensityScale;
            var oldDensityX = DensityX;
            var oldDensityY = DensityY;
            var oldDensityBucket = DensityBucket;

            Activity.WindowManager.DefaultDisplay.GetMetrics(metrics);

            return
                oldDensityScale != DensityScale ||
                oldDensityX != DensityX ||
                oldDensityY != DensityY ||
                oldDensityBucket != DensityBucket;            
        }

        /// <summary>
        /// Gets the current Android activity.
        /// </summary>
        [CLSCompliant(false)]
        public static Activity Activity
        {
            get { return activity; }
            internal set
            {
                activity = value;
                metrics = null;

                if (value != null)
                {
                    metrics = new DisplayMetrics();
                    value.WindowManager.DefaultDisplay.GetMetrics(metrics);
                }
            }
        }

        /// <inheritdoc/>
        public override Single DeviceScale
        {
            get
            {
                return 1f;
            }
        }

        /// <inheritdoc/>
        public override Single DensityScale
        {
            get
            {
                return (metrics == null) ? 1 : (160f / 96f) * metrics.Density;
            }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get
            {
                return (metrics == null) ? 0 : metrics.Xdpi;
            }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get
            {
                return (metrics == null) ? 0 : metrics.Ydpi;
            }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get
            {
                if (metrics == null)
                    return ScreenDensityBucket.Desktop;

                switch (metrics.DensityDpi)
                {
                    case DisplayMetricsDensity.Low:
                        return ScreenDensityBucket.Low;
                    case DisplayMetricsDensity.Tv:
                    case DisplayMetricsDensity.Medium:
                        return ScreenDensityBucket.Medium;
                    case DisplayMetricsDensity.D400:
                    case DisplayMetricsDensity.High:
                        return ScreenDensityBucket.High;
                    case DisplayMetricsDensity.Xhigh:
                        return ScreenDensityBucket.ExtraHigh;
                    case DisplayMetricsDensity.Xxhigh:
                        return ScreenDensityBucket.ExtraExtraHigh;
                }
                return ScreenDensityBucket.ExtraExtraExtraHigh;
            }
        }

        // State values.
        private static Activity activity;
        private static DisplayMetrics metrics;
    }
}