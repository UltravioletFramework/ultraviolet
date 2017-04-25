using System;
using Ultraviolet.Core;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="ScreenDensityService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="ScreenDensityService"/> that was created.</returns>
    public delegate ScreenDensityService ScreenDensityServiceFactory(IUltravioletDisplay display);

    /// <summary>
    /// Represents a service which retrieve the pixel density (DPI) of the specified display device.
    /// </summary>
    public abstract class ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        protected ScreenDensityService(IUltravioletDisplay display)
        {
            Contract.Require(display, nameof(display));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ScreenDensityService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which 
        /// to retrieve a screen density service.</param>
        /// <returns>The instance of <see cref="ScreenDensityService"/> that was created.</returns>
        public static ScreenDensityService Create(IUltravioletDisplay display)
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<ScreenDensityServiceFactory>()(display);
        }

        /// <summary>
        /// Gets the short name of the specified density bucket.
        /// </summary>
        /// <param name="bucket">The <see cref="ScreenDensityBucket"/> value for which to retrieve a short name.</param>
        /// <returns>The short name of the specified density bucket.</returns>
        public static String GetDensityBucketName(ScreenDensityBucket bucket)
        {
            switch (bucket)
            {
                case ScreenDensityBucket.Desktop:
                    return "pcdpi";
                case ScreenDensityBucket.Low:
                    return "ldpi";
                case ScreenDensityBucket.Retina:
                    return "retina";
                case ScreenDensityBucket.Medium:
                    return "mdpi";
                case ScreenDensityBucket.RetinaHD:
                    return "retinahd";
                case ScreenDensityBucket.High:
                    return "hdpi";
                case ScreenDensityBucket.ExtraHigh:
                    return "xhdpi";
                case ScreenDensityBucket.ExtraExtraHigh:
                    return "xxhdpi";
                case ScreenDensityBucket.ExtraExtraExtraHigh:
                    return "xxxhdpi";
            }
            throw new ArgumentException("bucket");
        }

        /// <summary>
        /// Gets the number of physical pixels per logical pixel on devices with high density display modes
        /// like Retina or Retina HD.
        /// </summary>
        public abstract Single DeviceScale
        {
            get;
        }

        /// <summary>
        /// Gets the scaling factor for device independent pixels.
        /// </summary>
        public abstract Single DensityScale
        {
            get;
        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the horizontal axis.
        /// </summary>
        public abstract Single DensityX
        {
            get;
        }

        /// <summary>
        /// Gets the screen's density in dots per inch along the vertical axis.
        /// </summary>
        public abstract Single DensityY
        {
            get;
        }

        /// <summary>
        /// Gets the screen's density bucket.
        /// </summary>
        public abstract ScreenDensityBucket DensityBucket
        {
            get;
        }

        /// <summary>
        /// Attempts to guess at the appropriate <see cref="ScreenDensityBucket"/> for the specified density scale.
        /// </summary>
        /// <param name="scale">The density scale for which to guess a density bucket.</param>
        /// <returns>The <see cref="ScreenDensityBucket"/> which was guessed for the specified density scale.</returns>
        protected static ScreenDensityBucket GuessBucketFromDensityScale(Single scale)
        {
            if (scale >= 6f)
                return ScreenDensityBucket.ExtraExtraExtraHigh;

            if (scale >= 5f)
                return ScreenDensityBucket.ExtraExtraHigh;

            if (scale >= 3f)
                return ScreenDensityBucket.ExtraHigh;

            if (scale >= 2.5f)
                return ScreenDensityBucket.High;

            if (scale >= 1.5f)
                return ScreenDensityBucket.Medium;

            if (scale >= 1.25f)
                return ScreenDensityBucket.Low;

            return ScreenDensityBucket.Desktop;
        }
    }
}
