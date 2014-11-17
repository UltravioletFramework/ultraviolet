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
