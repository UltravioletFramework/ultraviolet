using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.Desktop.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopSoftwareKeyboardService : SoftwareKeyboardService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard(KeyboardMode mode)
        {
            return false;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            return false;
        }
        
        /// <inheritdoc/>
        public override Rectangle? TextInputRegion
        {
            get { return null; }
            set { }
        }
    }
}
