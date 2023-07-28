using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the .NET Core platform.
    /// </summary>
    public sealed class NETCoreScreenRotationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCoreScreenRotationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public NETCoreScreenRotationService(IUltravioletDisplay display)
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
