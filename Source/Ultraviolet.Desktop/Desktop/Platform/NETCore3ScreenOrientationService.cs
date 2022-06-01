using Ultraviolet.Platform;

namespace Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the .NET Core 3.0 platform.
    /// </summary>
    public sealed class NETCore3ScreenOrientationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCore3ScreenOrientationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public NETCore3ScreenOrientationService(IUltravioletDisplay display)
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
