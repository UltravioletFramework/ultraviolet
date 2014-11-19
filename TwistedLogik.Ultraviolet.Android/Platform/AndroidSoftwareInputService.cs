using System;
using Android.App;
using Android.Views;
using Android.Views.InputMethods;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="SoftwareInputService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidSoftwareInputService : SoftwareInputService
    {
        /// <inheritdoc/>
        public override Boolean ShowSoftwareKeyboard()
        {
            if (Activity == null || View == null)
                return false;

            if (View.RequestFocus())
            {
                var imm = (InputMethodManager)Activity.GetSystemService(global::Android.App.Activity.InputMethodService);
                imm.ShowSoftInput(View, ShowFlags.Forced);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override Boolean HideSoftwareKeyboard()
        {
            if (Activity == null || View == null)
                return false;

            var imm = (InputMethodManager)Activity.GetSystemService(global::Android.App.Activity.InputMethodService);
            imm.HideSoftInputFromWindow(View.WindowToken, HideSoftInputFlags.None, null);
            return true;
        }

        /// <summary>
        /// Gets the current <see cref="UltravioletActivity"/>.
        /// </summary>
        public static UltravioletActivity Activity
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current <see cref="View"/>.
        /// </summary>
        public static View View
        {
            get;
            internal set;
        }
    }
}