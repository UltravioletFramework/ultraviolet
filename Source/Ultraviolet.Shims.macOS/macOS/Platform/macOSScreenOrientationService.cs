using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOS.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the macOS platform.
    /// </summary>
    public sealed class macOSScreenOrientationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="macOSScreenOrientationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public macOSScreenOrientationService (IUltravioletDisplay display)
            : base (display)
        {

        }

        /// <inheritdoc/>
        public override ScreenRotation ScreenRotation {
            get { return ScreenRotation.Rotation0; }
        }
    }
}