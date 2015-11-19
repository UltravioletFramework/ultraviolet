using System;
using SafeProjectName.Assets;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.WindowsForms;

namespace SafeProjectName
{
    /// <summary>
    /// The primary form in an Ultraviolet tool application.
    /// </summary>
    public partial class UltravioletToolForm : UltravioletForm
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletToolForm class.
        /// </summary>
        public UltravioletToolForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <param name="tickmode">The appliction's tick mode.</param>
        /// <returns>The application's Ultraviolet context.</returns>
        protected override UltravioletContext OnCreatingUltravioletContext(out UltravioletTickMode tickmode)
        {
            tickmode = UltravioletTickMode.Idle;
            return new OpenGLUltravioletContext(this, new OpenGLUltravioletConfiguration()
            {
                Headless = true
            });
        }

        /// <summary>
        /// Called when the application is initializing.
        /// </summary>
        protected override void OnInitializing()
        {
            Localization.Strings.LoadFromDirectory("Content", "Localization");

            base.OnInitializing();
        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadContentManifests();

            this.spriteBatch = SpriteBatch.Create();
            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);

            this.textRenderer = new TextRenderer();

            base.OnLoadingContent();
        }

        /// <summary>
        /// Loads the game's content manifest files.
        /// </summary>
        protected void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            uvContent.Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
        }

        /// <summary>
        /// Occurs when the Ultraviolet context is updating its state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected override void OnUpdating(UltravioletTime time)
        {
            base.OnUpdating(time);
        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// Raises the Closed event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
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
        private void ultravioletPanel1_Drawing(Object sender, EventArgs e)
        {
            spriteBatch.Begin();

            var size = Ultraviolet.GetPlatform().Windows.GetCurrent().ClientSize;
            var settings = new TextLayoutSettings(spriteFont, size.Width, size.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "Welcome to the |c:FFFF00C0|Ultraviolet Framework|c|!", Vector2.Zero, Color.White, settings);

            spriteBatch.End();
        }

        /// <summary>
        /// Handles the Drawing event for ultravioletPanel2.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void ultravioletPanel2_Drawing(object sender, EventArgs e)
        {
            Ultraviolet.GetGraphics().Clear(Color.Black);

            spriteBatch.Begin();

            var size = Ultraviolet.GetPlatform().Windows.GetCurrent().ClientSize;
            var settings = new TextLayoutSettings(spriteFont, size.Width, size.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "This is a |c:FF00FF00|secondary tool window|c|.", Vector2.Zero, Color.White, settings);

            spriteBatch.End();
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the application's execution.
        private ContentManager content;

        // Game resources.
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
    }
}
