using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using UltravioletSample.UI.Screens;

namespace UltravioletSample.UI.Dialogs
{
    /// <summary>
    /// Represents the dialog box that is shown when the user presses the escape key.
    /// </summary>
    public partial class EscMenuDialog : Modal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EscMenuDialog"/> class.
        /// </summary>
        /// <param name="owner">The screen that owns the dialog box.</param>
        public EscMenuDialog(GameScreenBase owner)
        {
            Contract.Require(owner, "owner");

            this.screen = new DialogScreen(this, owner.GlobalContent);
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

        // Property values.
        private readonly DialogScreen screen;
    }
}
