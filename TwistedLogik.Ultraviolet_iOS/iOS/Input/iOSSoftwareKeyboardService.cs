using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.UIKit;
using UIKit;

namespace TwistedLogik.Ultraviolet.iOS.Input
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareKeyboardService"/> class for the iOS platform.
    /// </summary>
    public sealed class iOSSoftwareKeyboardService : SoftwareKeyboardService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard(KeyboardMode mode)
        {
            var controller = (SDL_uikitviewcontroller)UIApplication.SharedApplication?.KeyWindow?.RootViewController;
            if (controller == null)
                return false;
            
            var textField = (UITextField)controller.View.Subviews[0];
            switch (mode)
            {
                case KeyboardMode.Number:
                    textField.KeyboardType = UIKeyboardType.DecimalPad;
                    textField.ReloadInputViews();
                    break;

                case KeyboardMode.Phone:
                    textField.KeyboardType = UIKeyboardType.PhonePad;
                    textField.ReloadInputViews();
                    break;

                case KeyboardMode.Datetime:
                    textField.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
                    textField.ReloadInputViews();
                    break;

                default:
                    textField.KeyboardType = UIKeyboardType.Default;
                    textField.ReloadInputViews();
                    break;
            }

            controller.ShowKeyboard();
            OnShowingSoftwareKeyboard();

            return true;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            var controller = (SDL_uikitviewcontroller)UIApplication.SharedApplication?.KeyWindow?.RootViewController;
            if (controller == null)
                return false;

            controller.HideKeyboard();
            OnHidingSoftwareKeyboard();

            return true;
        }
    }
}