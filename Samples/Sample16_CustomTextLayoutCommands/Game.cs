using System;
using Ultraviolet.Core;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.OpenGL;
using UltravioletSample.Sample16_CustomTextLayoutCommands.Assets;
using UltravioletSample.Sample16_CustomTextLayoutCommands.Input;

namespace UltravioletSample.Sample16_CustomTextLayoutCommands
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 16 - Custom Text Layout Commands", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public partial class Game : SampleApplicationBase2
    {
        public Game()
            : base("Ultraviolet", "Sample 16 - Custom Text Layout Commands", uv => uv.GetInput().GetActions())
        {
            PlatformSpecificInitialization();
        }

        partial void PlatformSpecificInitialization();

        public static void Main(String[] args)
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

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");
            LoadContentManifests(this.content);
            LoadLocalizationDatabases(this.content);

            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            
            this.textRenderer = new TextRenderer();
            this.textBlock = new ScrollingTextBlock(textRenderer, spriteFont,
                "Lorem ipsum dolor sit amet,|delay| consectetur adipiscing elit. |delay:200|" +
                "|speed:5|Donec |speed:1|commodo|speed:5| massa in odio dapibus blandit. |delay:200|" +
                "|speed|Etiam quis auctor magna. |delay:200|Mauris varius lacus eu vulputate pretium. |delay:200|" +
                "|speed:100|Cras a turpis id enim luctus laoreet. |delay:200|" +
                "|speed|Praesent sed faucibus nunc,|delay| in vestibulum libero. |delay:200|" +
                "Sed vitae bibendum ex. |delay:200|" +
                "Etiam vel aliquet ipsum,|delay| sit amet scelerisque est.", 256, 256);            

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (window != null)
            {
                this.textBlock.ChangeSize(
                    window.DrawableSize.Width / 2, 
                    window.DrawableSize.Height / 2);
                this.textBlock.Update(time);
            }

            if (Ultraviolet.GetInput().GetActions().ResetScrollingText.IsPressed() || (Ultraviolet.GetInput().GetPrimaryTouchDevice()?.WasTapped() ?? false))
                textBlock.Reset();

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
                Exit();

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            var settings = new TextLayoutSettings(spriteFont, null, null, TextFlags.Standard);
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
            {
                textRenderer.Draw(spriteBatch, "Tap the screen to reset the scrolling text.", 
                    Vector2.One * 8f, Color.White, settings);
            }
            else
            {
                textRenderer.Draw(spriteBatch, $"Press {Ultraviolet.GetInput().GetActions().ResetScrollingText.Primary} to reset the scrolling text.", 
                    Vector2.One * 8f, Color.White, settings);
            }

            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var x = (window.DrawableSize.Width - textBlock.Width.Value) / 2;
            var y = (window.DrawableSize.Height - textBlock.Height.Value) / 2;

            textBlock.Draw(time, spriteBatch, new Vector2(x, y), Color.White);

            spriteBatch.End();

            base.OnDrawing(time);
        }        

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(spriteBatch);
                SafeDispose.Dispose(content);
            }
            base.Dispose(disposing);
        }

        protected override void LoadContentManifests(ContentManager content)
        {
            base.LoadContentManifests(content);

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private ScrollingTextBlock textBlock;
    }
}
