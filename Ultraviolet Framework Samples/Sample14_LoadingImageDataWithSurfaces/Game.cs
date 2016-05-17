using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
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
#endif
    public class Game : SampleApplicationBase1
    {
        public Game()
            : base("TwistedLogik", "Sample 14 - Loading Image Data with Surfaces", uv => uv.GetInput().GetActions())
        {

        }

        public static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        /// <inheritdoc/>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            return new OpenGLUltravioletContext(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        /// <inheritdoc/>
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

            var font = content.Load<SpriteFont>(GlobalFontID.SegoeUI);

            for (int y = 0; y < surface.Width; y++)
            {
                for (int x = 0; x < surface.Height; x++)
                {
                    var cellColor = data[(y * surface.Width) + x];
                    var cellText = $"{cellColor:x}";
                    var cellPosition = new Vector2(cx, cy);

                    spriteBatch.DrawString(font, cellText, cellPosition, cellColor);

                    cx = cx + CellWidth;
                }
                cx = origX;
                cy = cy + CellHeight;
            }

            spriteBatch.End();

            base.OnDrawing(time);
        }

        /// <inheritdoc/>
        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");
            LoadContentManifests(this.content);

            this.spriteBatch = SpriteBatch.Create();

            // Texture2D lives in GPU memory, so we can't read data out of it once it's been uploaded to the device.
            // Surface2D lives in CPU memory, so we can directly manipulate it without involving the graphics driver.
            // In this sample, we just load its color data into an array for later use in OnDrawing().
            this.surface = this.content.Load<Surface2D>("Data/Face");
            this.texture = this.surface.CreateTexture();
            this.data = new Color[surface.Width * surface.Height];
            surface.GetData(this.data);

            base.OnLoadingContent();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void LoadContentManifests(ContentManager content)
        {
            base.LoadContentManifests(content);

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
        }

        // Application resources
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private Surface2D surface;
        private Texture2D texture;
        private Color[] data;
    }
}
