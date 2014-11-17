using System;
using Android.App;
using Android.Util;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Android.Platform
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

        /// <summary>
        /// Gets the current Android activity.
        /// </summary>
        public static Activity Activity
        {
            get { return activity; }
            internal set
            {
                activity = value;
                metrics  = null;

                if (value != null)
                {
                    metrics = new DisplayMetrics();
                    value.WindowManager.DefaultDisplay.GetMetrics(metrics);
                }
            }
        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the horizontal axis.
        /// </summary>
        public override Single DensityX
        {
            get
            {
                return (metrics == null) ? 0 : metrics.Xdpi;
            }
        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the vertical axis.
        /// </summary>
        public override Single DensityY
        {
            get
            {
                return (metrics == null) ? 0 : metrics.Ydpi;
            }
        }

        /// <summary>
        /// Gets the screen's density bucket.
        /// </summary>
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