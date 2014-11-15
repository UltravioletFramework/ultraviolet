using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopScreenOrientationService : ScreenRotationService
    {
        /// <inheritdoc/>
        public override ScreenRotation ScreenRotation
        {
            get { return ScreenRotation.Rotation0; }
        }
    }
}
