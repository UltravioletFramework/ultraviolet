
namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="ScreenRotationService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="ScreenRotationService"/> that was created.</returns>
    public delegate ScreenRotationService ScreenRotationServiceFactory();

    /// <summary>
    /// Represents a service which is responsible for querying the screen's rotation.
    /// </summary>
    public abstract class ScreenRotationService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ScreenRotationService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="ScreenRotationService"/> that was created.</returns>
        public static ScreenRotationService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<ScreenRotationServiceFactory>()();
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
