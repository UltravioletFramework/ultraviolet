using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents an identity transformation.
    /// </summary>
    [UvmlKnownType]
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
