using Ultraviolet.Core;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="ScreenRotationService"/> class.
    /// </summary>
    /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
    /// <returns>The instance of <see cref="ScreenRotationService"/> that was created.</returns>
    public delegate ScreenRotationService ScreenRotationServiceFactory(IUltravioletDisplay display);

    /// <summary>
    /// Represents a service which is responsible for querying the screen's rotation.
    /// </summary>
    public abstract class ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRotationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        protected ScreenRotationService(IUltravioletDisplay display)
        {
            Contract.Require(display, nameof(display));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ScreenRotationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        /// <returns>The instance of <see cref="ScreenRotationService"/> that was created.</returns>
        public static ScreenRotationService Create(IUltravioletDisplay display)
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<ScreenRotationServiceFactory>()(display);
        }

        /// <summary>
        /// Gets the screen's rotation on devices which can be rotated.
        /// </summary>
        public abstract ScreenRotation ScreenRotation
        {
            get;
        }
    }
}
