using System;
using Android.Text;
using Ultraviolet.Input;

namespace Ultraviolet.Shims.Android.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidSoftwareKeyboardService : SoftwareKeyboardService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard(KeyboardMode mode)
        {
            var activity = UltravioletApplication.Instance;
            if (activity == null)
                return false;

            switch (mode)
            {
                case KeyboardMode.Text:
                    /* NOTE: We use TextVariationVisiblePassword here to match SDL's default behavior.
                     * This is done to disable text suggestions due to the fact that some keyboards implement
                     * them in a way which is difficult to handle properly. */
                    activity.KeyboardInputType = InputTypes.ClassText | InputTypes.TextVariationVisiblePassword;
                    break;

                case KeyboardMode.Number:
                    activity.KeyboardInputType = InputTypes.ClassNumber;
                    break;

                case KeyboardMode.Phone:
                    activity.KeyboardInputType = InputTypes.ClassPhone;
                    break;

                case KeyboardMode.Datetime:
                    activity.KeyboardInputType = InputTypes.ClassDatetime;
                    break;
            }

            OnShowingSoftwareKeyboard();

            return true;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            var activity = UltravioletApplication.Instance;
            if (activity == null)
                return false;

            OnHidingSoftwareKeyboard();

            return true;
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