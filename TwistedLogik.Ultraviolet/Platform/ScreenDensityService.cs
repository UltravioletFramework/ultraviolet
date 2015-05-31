using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Platform
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
            Contract.Require(display, "display");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ScreenDensityService"/> class.
        /// </summary>
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
                case ScreenDensityBucket.Medium:
                    return "mdpi";
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
    }
}
