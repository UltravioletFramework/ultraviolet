using Ultraviolet.Graphics;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidIconLoader : IconLoader
    {
        /// <inheritdoc/>
        public override Surface2D LoadIcon()
        {
            return null;
        }
    }
}