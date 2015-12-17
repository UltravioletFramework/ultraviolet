using System;
using System.IO;
using System.Text;
using SAFE_PROJECT_NAME.Assets;
using SAFE_PROJECT_NAME.Input;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.Platform;

namespace SAFE_PROJECT_NAME
{
    /// <summary>
    /// Represents the main application object.
    /// </summary>
#if ANDROID
    [Android.App.Activity(Label = "GameActivity", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        /// <summary>
        /// Initializes a new instance of the Game 
        /// </summary>
        public Game() : base("YOUR_ORGANIZATION", "PROJECT_NAME") { }

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">An array containing the application's command line arguments.</param>
        public static void Main(String[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <returns>The Ultraviolet context.</returns>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            PopulateConfiguration(configuration);

#if DEBUG
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };
#endif
            return new OpenGLUltravioletContext(this, configuration);
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            SetFileSourceFromManifestIfExists("UltravioletGame.Content.uvarc");

            base.OnInitialized();
        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadLocalizationDatabases();
            LoadInputBindings();
            LoadContentManifests();
            LoadCursors();

            this.spriteBatch = SpriteBatch.Create();
            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);

            this.textRenderer = new TextRenderer();
            this.textFormatter = new StringFormatter();
            this.textBuffer = new StringBuilder();

            GC.Collect(2);

            base.OnLoadingContent();
        }

        /// <summary>
        /// Loads the application's localization databases.
        /// </summary>
        protected void LoadLocalizationDatabases()
        {
            var fss = FileSystemService.Create();
            var databases = content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (var database in databases)
            {
                using (var stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }

        /// <summary>
        /// Loads the game's input bindings.
        /// </summary>
        protected void LoadInputBindings()
        {
            var inputBindingsPath = Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
            Ultraviolet.GetInput().GetActions().Load(inputBindingsPath, throwIfNotFound: false);
        }

        /// <summary>
        /// Saves the game's input bindings.
        /// </summary>
        protected void SaveInputBindings()
        {
            var inputBindingsPath = Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
            Ultraviolet.GetInput().GetActions().Save(inputBindingsPath);
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
        /// Loads the game's cursors.
        /// </summary>
        protected void LoadCursors()
        {
            this.cursors = this.content.Load<CursorCollection>("Cursors/Cursors");
            Ultraviolet.GetPlatform().Cursor = this.cursors["Normal"];
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        /// <summary>
        /// Called when the scene is being rendered.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin();

            textFormatter.Reset();
            textFormatter.AddArgument(Ultraviolet.GetGraphics().FrameRate);
            textFormatter.AddArgument(GC.GetTotalMemory(false) / 1024);
            textFormatter.AddArgument(Environment.Is64BitProcess ? "64-bit" : "32-bit");
            textFormatter.Format("{0:decimals:2} FPS\nAllocated: {1:decimals:2} kb\n{2}", textBuffer);

            spriteBatch.DrawString(spriteFont, textBuffer, Vector2.One * 8f, Color.White);

            var size = Ultraviolet.GetPlatform().Windows.GetCurrent().ClientSize;
            var settings = new TextLayoutSettings(spriteFont, size.Width, size.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "Welcome to the |c:FFFF00C0|Ultraviolet Framework|c|!", Vector2.Zero, Color.White, settings);

            spriteBatch.End();

            base.OnDrawing(time);
        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the game's execution.
        private ContentManager content;

        // Game resources.
        private CursorCollection cursors;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private StringFormatter textFormatter;
        private StringBuilder textBuffer;
    }
}
