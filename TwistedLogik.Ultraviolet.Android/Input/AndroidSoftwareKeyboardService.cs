using System;
using Android.Text;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.Android.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidSoftwareKeyboardService : SoftwareKeyboardService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard(KeyboardMode mode)
        {
            if (Activity == null)
                return false;

            switch (mode)
            {
                case KeyboardMode.Text:
                    Activity.KeyboardInputType = InputTypes.ClassText;
                    break;

                case KeyboardMode.Number:
                    Activity.KeyboardInputType = InputTypes.ClassNumber;
                    break;

                case KeyboardMode.Phone:
                    Activity.KeyboardInputType = InputTypes.ClassPhone;
                    break;

                case KeyboardMode.Datetime:
                    Activity.KeyboardInputType = InputTypes.ClassDatetime;
                    break;
            }

            OnShowingSoftwareKeyboard();

            return true;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            if (Activity == null)
                return false;

            OnHidingSoftwareKeyboard();

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