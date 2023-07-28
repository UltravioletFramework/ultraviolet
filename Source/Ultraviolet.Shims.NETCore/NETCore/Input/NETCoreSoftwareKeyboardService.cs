using System;
using Ultraviolet.Input;

namespace Ultraviolet.Shims.NETCore.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for the .NET Core 3.0 platform.
    /// </summary>
    public sealed class NETCoreSoftwareKeyboardService : SoftwareKeyboardService
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
