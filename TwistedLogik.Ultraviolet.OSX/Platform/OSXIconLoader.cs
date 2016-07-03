using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.OSX.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="IconLoader"/> class for Mac OS X.
    /// </summary>
    public sealed class OSXIconLoader : IconLoader
	{
        /// <inheritdoc/>
        public override Surface2D LoadIcon()
        {
            /* Application icon should be handled in Info.plist */
            return null;
        }
	}
}

