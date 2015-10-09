using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using UvDebugSandbox.UI.Screens;

namespace UvDebugSandbox.UI.Dialogs
{
    /// <summary>
    /// Represents a confirmation dialog.
    /// </summary>
    public partial class ConfirmationDialog : Modal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialog"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the dialog box.</param>
        public ConfirmationDialog(DebugScreen owner)
            : this(owner, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialog"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the dialog box.</param>
        /// <param name="header">The dialog's header text.</param>
        /// <param name="text">The dialog's content text.</param>
        public ConfirmationDialog(DebugScreen owner, String header, String text)
        {
            Contract.Require(owner, "owner");

            this.screen = new DialogScreen(this, owner.GlobalContent);
            this.screen.Header = header;
            this.screen.Text = text;
        }

        /// <inheritdoc/>
        public override UIScreen Screen
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return screen;
            }
        }

        /// <summary>
        /// Gets or sets the dialog's header text.
        /// </summary>
        public String Header
        {
            get { return screen.Header; }
            set { screen.Header = value; }
        }

        /// <summary>
        /// Gets or sets the dialog's content text.
        /// </summary>
        public String Text
        {
            get { return screen.Text; }
            set { screen.Text = value; }
        }

        // Property values.
        private readonly DialogScreen screen;
    }
}
