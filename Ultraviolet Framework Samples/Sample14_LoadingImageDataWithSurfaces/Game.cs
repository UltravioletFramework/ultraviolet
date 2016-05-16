using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample14_LoadingImageDataWithSurfaces.Assets;
using UltravioletSample.Sample14_LoadingImageDataWithSurfaces.Input;

namespace UltravioletSample.Sample14_LoadingImageDataWithSurfaces
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 14 - Loading Image Data with Surfaces", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Sample 14 - Loading Image Data with Surfaces")
        {

        }

        public static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            return new OpenGLUltravioletContext(this);
        }

        protected override void OnInitialized()
        {
            SetFileSourceFromManifestIfExists("UltravioletSample.Content.uvarc");

            base.OnInitialized();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            // We've loaded our surface data into the 'data' array, and we'll use it now to draw an image to the screen.
            // We'll draw each pixel as a string of text representing that pixel's color.

            spriteBatch.Begin(SpriteSortMode.Deferred, null);

            const Int32 CellWidth = 64;
            const Int32 CellHeight = 24;

            var totalWidth = surface.Width * CellWidth;
            var totalHeight = surface.Height * CellHeight;

            var compositor = Ultraviolet.GetPlatform().Windows.GetPrimary().Compositor;
            var origX = (compositor.Width - totalWidth) / 2;
            var origY = (compositor.Height - totalHeight) / 2;

            var cx = origX;
            var cy = origY;

            for (int y = 0; y < surface.Width; y++)
            {
                for (int x = 0; x < surface.Height; x++)
                {
                    var cellColor = data[(y * surface.Width) + x];
                    var cellText = $"{cellColor:x}";
                    var cellPosition = new Vector2(cx, cy);

                    spriteBatch.DrawString(spriteFontSegoe, cellText, cellPosition, cellColor);

                    cx = cx + CellWidth;
                }
                cx = origX;
                cy = cy + CellHeight;
            }

            spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadInputBindings();
            LoadContentManifests();

            this.spriteFontSegoe = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            this.textRenderer = new TextRenderer();

            // Texture2D lives in GPU memory, so we can't read data out of it once it's been uploaded to the device.
            // Surface2D lives in CPU memory, so we can directly manipulate it without involving the graphics driver.
            // In this sample, we just load its color data into an array for later use in OnDrawing().
            this.surface = this.content.Load<Surface2D>("Data/Face");
            this.texture = this.surface.CreateTexture();
            this.data = new Color[surface.Width * surface.Height];
            surface.GetData(this.data);

            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                // We created the texture, so it isn't managed by the ContentManager.
                // We own it, so we need to dispose of it ourselves!
                SafeDispose.Dispose(texture);
                SafeDispose.Dispose(content);
            }
            base.Dispose(disposing);
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }

        private void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            uvContent.Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
        }

        private Surface2D surface;
        private Texture2D texture;
        private Color[] data;

        private ContentManager content;
        private SpriteFont spriteFontSegoe;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private readonly TextLayoutCommandStream textLayoutCommands = new TextLayoutCommandStream();
    }
}
