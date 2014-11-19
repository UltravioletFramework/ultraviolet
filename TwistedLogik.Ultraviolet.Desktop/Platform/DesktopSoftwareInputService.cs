using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareInputService"/> class for dekstop platforms.
    /// </summary>
    public sealed class DesktopSoftwareInputService : SoftwareInputService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard()
        {
            return false;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            return false;
        }
    }
}