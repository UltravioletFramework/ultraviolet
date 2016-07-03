using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.OSX.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for Mac OS X.
    /// </summary>
    public sealed class OSXSoftwareKeyboardService : SoftwareKeyboardService
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
            get { return textInputRegion; }
            set
            {
                if (textInputRegion != value)
                {
                    textInputRegion = value;
                    UltravioletContext.RequestCurrent()?.Messages.Publish(
                        UltravioletMessages.TextInputRegionChanged, null);
                }
            }
        }

        // Property values.
        private Rectangle? textInputRegion;
    }
}
