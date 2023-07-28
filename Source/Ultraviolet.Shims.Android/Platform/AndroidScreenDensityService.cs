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
            EnsureMetrics();
        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            EnsureMetrics();

            if (activity == null)
                return false;

            var oldDensityScale = DensityScale;
            var oldDensityX = DensityX;
            var oldDensityY = DensityY;
            var oldDensityBucket = DensityBucket;

            metrics = activity.ApplicationContext.Resources.DisplayMetrics;

            return
                oldDensityScale != DensityScale ||
                oldDensityX != DensityX ||
                oldDensityY != DensityY ||
                oldDensityBucket != DensityBucket;
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
                EnsureMetrics();
                return (metrics == null) ? 1 : (160f / 96f) * metrics.Density;
            }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get
            {
                EnsureMetrics();
                return (metrics == null) ? 0 : metrics.Xdpi;
            }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get
            {
                EnsureMetrics();
                return (metrics == null) ? 0 : metrics.Ydpi;
            }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get
            {
                EnsureMetrics();

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

        /// <summary>
        /// Ensures that the service has retrieved the display metrics.
        /// </summary>
        private void EnsureMetrics()
        {
            var activity = UltravioletApplication.Instance;
            if (activity == this.activity)
                return;

            if (activity == null)
            {
                this.activity = null;
                this.metrics = null;
            }
            else
            {
                this.activity = activity;
                this.metrics = activity.ApplicationContext.Resources.DisplayMetrics;
            }
        }

        // State values.
        private Activity activity;
        private DisplayMetrics metrics;
    }
}