using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidScreenRotationService : ScreenRotationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidScreenRotationService"/> class.
        /// </summary>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve rotation information.</param>
        public AndroidScreenRotationService(IUltravioletDisplay display)
            : base(display)
        {

        }

        /// <summary>
        /// Updates the cached screen rotation value.
        /// </summary>
        /// <param name="rotation">The screen's current rotation.</param>
        public static void UpdateScreenRotation(ScreenRotation rotation)
        {
            AndroidScreenRotationService.screenRotation = rotation;
        }

        /// <inheritdoc/>
        public override ScreenRotation ScreenRotation
        {
            get { return screenRotation; }
        }

        // Property values.
        private static ScreenRotation screenRotation;
    }
}