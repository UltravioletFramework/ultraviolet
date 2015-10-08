using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace UvDebugSandbox.UI.Dialogs
{
    partial class ConfirmationDialog
    {
        /// <summary>
        /// Represents the dialog's screen.
        /// </summary>
        public class DialogScreen : UIScreen
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DialogScreen"/> class.
            /// </summary>
            /// <param name="dialog">The dialog that owns this screen.</param>
            /// <param name="globalContent">The screen's global content manager.</param>
            public DialogScreen(ConfirmationDialog dialog, ContentManager globalContent)
                : base("Content/UI/Dialogs/ConfirmationDialog", "ConfirmationDialog", globalContent)
            {
                Contract.Require(dialog, "dialog");
                
                View.SetViewModel(new DialogScreenVM(dialog));
            }
            
            /// <summary>
            /// Gets or sets the screen's header text.
            /// </summary>
            public String Header
            {
                get { return View.GetViewModel<DialogScreenVM>().Header; }
                set { View.GetViewModel<DialogScreenVM>().Header = value; }
            }

            /// <summary>
            /// Gets or sets the screen's content text.
            /// </summary>
            public String Text
            {
                get { return View.GetViewModel<DialogScreenVM>().Text; }
                set { View.GetViewModel<DialogScreenVM>().Text = value; }
            }
        }
    }
}
