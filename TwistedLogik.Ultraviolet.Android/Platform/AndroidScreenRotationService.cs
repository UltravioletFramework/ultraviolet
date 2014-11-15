using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenRotationService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidScreenRotationService : ScreenRotationService
    {
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