using System;
using System.Text;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.OpenGL;

namespace UvStress
{
#if ANDROID
    [Android.App.Activity(Label = "UvStress", MainLauncher = true, ConfigurationChanges =
        Android.Content.PM.ConfigChanges.Orientation |
        Android.Content.PM.ConfigChanges.ScreenSize |
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public partial class Game : UltravioletActivity
#else
    public partial class Game : UltravioletApplication
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game() : base("Ultraviolet", "UvStress")
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
            if (!SetFileSourceFromManifestIfExists("UvStress.Content.uvarc"))
                UsePlatformSpecificFileSource();

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

            this.blankTexture = Texture2D.CreateTexture(1, 1);
            this.blankTexture.SetData(new[] { Color.White });

            this.effect = BasicEffect.Create();

            this.vbuffer = VertexBuffer.Create<VertexPositionColor>(3);
            this.vbuffer.SetData<VertexPositionColor>(new[]
            {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(1, -1, 0), Color.Lime),
                new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),
            });

            this.geometryStream = GeometryStream.Create();
            this.geometryStream.Attach(this.vbuffer);

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

            var gfx = Ultraviolet.GetGraphics();
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var drawableWidth = window.DrawableSize.Width;
            var drawableHeight = window.DrawableSize.Height;
            var aspectRatio = drawableWidth / (float)drawableHeight;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            for (int i = 0; i < 5000; i++)
            {
                var x = rng.Next(drawableWidth);
                var y = rng.Next(drawableHeight);
                spriteBatch.DrawSprite(sprite[rng.Next(sprite.AnimationCount)].Controller, new Vector2(x, y));
            }
            spriteBatch.End();

            effect.World = Matrix.CreateRotationY((float)(2.0 * Math.PI * time.TotalTime.TotalSeconds));
            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
            effect.VertexColorEnabled = true;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gfx.SetRasterizerState(RasterizerState.CullNone);
                gfx.SetGeometryStream(geometryStream);
                gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

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
        
        // Triangle geometry.
        private BasicEffect effect;
        private VertexBuffer vbuffer;
        private GeometryStream geometryStream;

        // Text formatting.
        private readonly StringBuilder strBuffer = new StringBuilder();
        private readonly StringFormatter strFormatter = new StringFormatter();

        // Performance history.
        private readonly Single[] fpsHistory = new Single[128];
    }
}
