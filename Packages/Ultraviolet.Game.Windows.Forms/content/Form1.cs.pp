using System;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.OpenGL;
using Ultraviolet.Windows.Forms;

namespace $RootNamespace$
{
    /// <summary>
    /// The primary form in an Ultraviolet application.
    /// </summary>
    public partial class Form1 : UltravioletForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        protected override UltravioletContext OnCreatingUltravioletContext(out UltravioletTickMode tickmode)
        {
            // NOTE: Making the context headless allows Windows Forms to control window creation for us.
            tickmode = UltravioletTickMode.Idle;
            return new OpenGLUltravioletContext(this, new OpenGLUltravioletConfiguration()
            {
                Headless = true
            });
        }

        /// <inheritdoc/>
        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            // TODO: Load content here

            base.OnLoadingContent();
        }

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            // TODO: Update the game state

            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            SafeDispose.DisposeRef(ref content);

            base.OnClosed(e);
        }

        /// <summary>
        /// Handles the Click event for exitToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Drawing event for ultravioletPanel1.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void ultravioletPanel1_Drawing(object sender, EventArgs e)
        {
            // TODO: Draw the first panel
        }

        /// <summary>
        /// Handles the Drawing event for ultravioletPanel2.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void ultravioletPanel2_Drawing(object sender, EventArgs e)
        {
            // TODO: Draw the second panel
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the application's execution.
        private ContentManager content;
    }
}