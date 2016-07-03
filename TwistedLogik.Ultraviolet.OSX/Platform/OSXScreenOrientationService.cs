using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.OSX.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the OS X platform.
    /// </summary>
    public sealed class OSXScreenOrientationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OSXScreenOrientationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public OSXScreenOrientationService(IUltravioletDisplay display)
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
