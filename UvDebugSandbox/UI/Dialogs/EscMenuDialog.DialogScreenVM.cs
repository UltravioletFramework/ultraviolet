using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Dialogs
{
    partial class EscMenuDialog
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
            internal DialogScreenVM(EscMenuDialog dialog)
            {
                Contract.Require(dialog, "dialog");

                this.dialog = dialog;
            }

            /// <summary>
            /// Handles the "Resume" button being clicked.
            /// </summary>
            public void HandleClickResume(DependencyObject dobj, ref RoutedEventData data)
            {
                dialog.Close(false);
            }

            /// <summary>
            /// Handles the "Exit" button being clicked.
            /// </summary>
            public void HandleClickExit(DependencyObject dobj, ref RoutedEventData data)
            {
                dialog.Close(true, TimeSpan.Zero);
            }

            /// <summary>
            /// Handles the "Exit to Desktop" button being clicked.
            /// </summary>
            public void HandleClickExitToDesktop(DependencyObject dobj, ref RoutedEventData data)
            {
                dialog.Screen.Ultraviolet.Host.Exit();
            }

            // State values.
            private readonly EscMenuDialog dialog;
        }
    }
}