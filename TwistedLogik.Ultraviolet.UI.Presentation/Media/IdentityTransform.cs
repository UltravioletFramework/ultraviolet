using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents an identity transformation.
    /// </summary>
    public sealed class IdentityTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix GetValue()
        {
            return Matrix.Identity;
        }

        /// <inheritdoc/>
        public override Matrix GetValueForDisplay(IUltravioletDisplay display)
        {
            return Matrix.Identity;
        }
    }
}
