using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.Android.Input
{
    public sealed class AndroidSoftwareKeyboardService : SoftwareKeyboardService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard(KeyboardMode mode)
        {
            if (Activity == null)
                return false;

            Activity.ShowSoftwareKeyboard(mode);

            return true;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            if (Activity == null)
                return false;

            Activity.HideSoftwareKeyboard();

            return true;
        }

        /// <summary>
        /// Gets the current Android activity.
        /// </summary>
        [CLSCompliant(false)]
        public static UltravioletActivity Activity
        {
            get;
            internal set;
        }
    }
}