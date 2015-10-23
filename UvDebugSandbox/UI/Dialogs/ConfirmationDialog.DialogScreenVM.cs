using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Dialogs
{
    partial class ConfirmationDialog
    {
        /// <summary>
        /// Represents the view model for <see cref="DialogScreen"/>.
        /// </summary>
        public class DialogScreenVM
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DialogScreenVM"/> class.
            /// </summary>
            /// <param name="dialog">The dialog box that owns the screen.</param>
            internal DialogScreenVM(ConfirmationDialog dialog)
            {
                Contract.Require(dialog, "dialog");

                this.dialog = dialog;
            }

            /// <summary>
            /// Closes the dialog box when the "OK" button is clicked.
            /// </summary>
            public void HandleClickOK(DependencyObject dobj, ref RoutedEventData data)
            {
                dialog.DialogResult = true;
            }

            /// <summary>
            /// Closes the dialog box when the "Cancel" button is clicked.
            /// </summary>
            public void HandleClickCancel(DependencyObject dobj, ref RoutedEventData data)
            {
                dialog.DialogResult = false;
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
            private readonly ConfirmationDialog dialog;
        }
    }
}