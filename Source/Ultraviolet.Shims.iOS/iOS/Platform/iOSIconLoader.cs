using Ultraviolet.Graphics;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.iOS.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for the iOS platform.
    /// </summary>
    public sealed class iOSIconLoader : IconLoader
    {
        /// <inheritdoc/>
        public override Surface2D LoadIcon()
        {
            return null;
        }
    }
}
