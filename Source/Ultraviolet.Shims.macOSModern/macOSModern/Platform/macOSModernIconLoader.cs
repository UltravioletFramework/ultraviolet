using Ultraviolet.Graphics;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOSModern.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for macOS.
    /// </summary>
    public sealed class macOSModernIconLoader : IconLoader
    {
        /// <inheritdoc/>
        public override Surface2D LoadIcon()
        {
            /* Application icon should be handled in Info.plist */
            return null;
        }
    }
}

