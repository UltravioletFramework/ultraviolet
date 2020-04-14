using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore2.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the .NET Core 2.0 platform.
    /// </summary>
    public sealed class NETCore2ScreenOrientationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCore2ScreenOrientationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public NETCore2ScreenOrientationService(IUltravioletDisplay display)
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
