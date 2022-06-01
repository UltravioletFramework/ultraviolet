using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.UI;

namespace UvDebug.UI.Dialogs
{
    partial class EscMenuDialog
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
            public DialogScreen(EscMenuDialog dialog, ContentManager globalContent)
                : base("Content/UI/Dialogs/EscMenuDialog", "EscMenuDialog", globalContent)
            {
                Contract.Require(dialog, nameof(dialog));

                this.dialog = dialog;
            }

            /// <summary>
            /// Gets the screen's modal dialog.
            /// </summary>
            public EscMenuDialog Dialog
            {
                get { return dialog; }
            }

            /// <inheritdoc/>
            protected override Object CreateViewModel(UIView view)
            {
                return new DialogScreenVM(this);
            }

            // State values.
            private readonly EscMenuDialog dialog;
        }
    }
}
