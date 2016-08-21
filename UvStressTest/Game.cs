using System;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace UvStressTest
{
    public partial class Game : UltravioletApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game() : base("TwistedLogik", "UvStressTest")
        {
            PlatformSpecificInitialization();
        }

        /// <summary>
        /// Performs any necessary platform-specific initialization steps.
        /// </summary>
        partial void PlatformSpecificInitialization();

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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            SetFileSourceFromManifestIfExists("UvStressTest.Content.uvarc");

            Ultraviolet.GetPlatform().Windows.GetPrimary().Caption = "UvStressTest";

            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            this.sprite = content.Load<Sprite>("Sprites/Hexagons");
            this.spriteFont = content.Load<SpriteFont>("Fonts/SegoeUI");
            this.spriteBatch = SpriteBatch.Create();

            this.blankTexture = Texture2D.Create(1, 1);
            this.blankTexture.SetData(new[] { Color.White });

            GC.Collect(2);

            base.OnLoadingContent();
        }

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time)
        {
            var fps = Ultraviolet.GetGraphics().FrameRate;

            Array.Copy(fpsHistory, 1, fpsHistory, 0, fpsHistory.Length - 1);
            fpsHistory[fpsHistory.Length - 1] = fps;

            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var drawableWidth = window.DrawableSize.Width;
            var drawableHeight = window.DrawableSize.Height;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            for (int i = 0; i < 100000; i++)
            {
                var x = rng.Next(drawableWidth);
                var y = rng.Next(drawableHeight);
                spriteBatch.DrawSprite(sprite[rng.Next(sprite.AnimationCount)].Controller, new Vector2(x, y));
            }
            spriteBatch.End();

            strFormatter.Reset();
            strFormatter.AddArgument(fps);
            strFormatter.Format("{0:decimals:2}", strBuffer);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            for (int i = 0; i < fpsHistory.Length; i++)
            {
                var historyFps = fpsHistory[i];
                var historyWidth = drawableWidth / fpsHistory.Length;
                var historyHeight = (Int32)(historyFps * 2.0);
                var historyX = i * historyWidth;
                var historyY = drawableHeight - historyHeight;
                var historyColor = historyFps < 55 ? (historyFps < 40 ? Color.Red : Color.Yellow) : Color.Lime;
                spriteBatch.Draw(blankTexture, new RectangleF(historyX, historyY, historyWidth, historyHeight), historyColor * 0.5f);
            }
            spriteBatch.Draw(blankTexture, new RectangleF(0, drawableHeight - 120, drawableWidth, 1), Color.White);
            spriteBatch.DrawString(spriteFont, strBuffer, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.OnDrawing(time);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref spriteBatch);
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        // Game resources.
        private readonly Random rng = new Random();
        private ContentManager content;
        private Sprite sprite;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private Texture2D blankTexture;

        // Text formatting.
        private readonly StringBuilder strBuffer = new StringBuilder();
        private readonly StringFormatter strFormatter = new StringFormatter();

        // Performance history.
        private readonly Single[] fpsHistory = new Single[128];
    }
}
