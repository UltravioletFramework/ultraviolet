using System;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Dialogs
{
    /// <summary>
    /// Represents the view model for <see cref="ConfirmationDialogScreen"/>.
    /// </summary>
    public class ConfirmationDialogViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialogViewModel"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the dialog box.</param>
        internal ConfirmationDialogViewModel(ConfirmationDialog.DialogScreen owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Closes the dialog box when the "OK" button is clicked.
        /// </summary>
        public void HandleClickOK(DependencyObject dobj, ref RoutedEventData data)
        {
            var uv = owner.Ultraviolet;
            owner.DialogResult = true;
            uv.GetUI().GetScreens().Close(owner);
        }

        /// <summary>
        /// Closes the dialog box when the "Cancel" button is clicked.
        /// </summary>
        public void HandleClickCancel(DependencyObject dobj, ref RoutedEventData data)
        {
            var uv = owner.Ultraviolet;
            owner.DialogResult = false;
            uv.GetUI().GetScreens().Close(owner);
        }
        
        /// <summary>
        /// Gets or sets the dialog's header text.
        /// </summary>
        public String Header
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog's content text.
        /// </summary>
        public String Text
        {
            get;
            set;
        }

        // State values.
        private readonly ConfirmationDialog.DialogScreen owner;
    }
}
