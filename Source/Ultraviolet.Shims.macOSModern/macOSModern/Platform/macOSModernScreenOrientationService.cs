using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOSModern.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the macOS platform.
    /// </summary>
    public sealed class macOSModernScreenOrientationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="macOSModernScreenOrientationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public macOSModernScreenOrientationService(IUltravioletDisplay display)
            : base(display)
        {

        }

        /// <inheritdoc/>
        public override ScreenRotation ScreenRotation
        {
            get { return ScreenRotation.Rotation0; }
        }
    }
}
