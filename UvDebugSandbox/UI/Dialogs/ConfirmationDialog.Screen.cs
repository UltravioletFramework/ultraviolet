using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Dialogs
{
    partial class ConfirmationDialog
    {
        /// <summary>
        /// Represents the dialog's screen.
        /// </summary>
        public class DialogScreen : UIScreen, IModalDialog
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DialogScreen"/> class.
            /// </summary>
            /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
            /// <param name="uiScreenService">The screen service which created this screen.</param>
            public DialogScreen(ContentManager globalContent)
                : base("Content/UI/Dialogs/ConfirmationDialog", "ConfirmationDialog", globalContent)
            {

            }

            /// <inheritdoc/>
            public Boolean? DialogResult
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the screen's header text.
            /// </summary>
            public String Header
            {
                get { return View.GetViewModel<ConfirmationDialogViewModel>().Header; }
                set { View.GetViewModel<ConfirmationDialogViewModel>().Header = value; }
            }

            /// <summary>
            /// Gets or sets the screen's content text.
            /// </summary>
            public String Text
            {
                get { return View.GetViewModel<ConfirmationDialogViewModel>().Text; }
                set { View.GetViewModel<ConfirmationDialogViewModel>().Text = value; }
            }

            /// <inheritdoc/>
            protected override void OnViewLoaded()
            {
                if (View != null)
                {
                    View.SetViewModel(new ConfirmationDialogViewModel(this));
                }
                base.OnViewLoaded();
            }
        }
    }
}
