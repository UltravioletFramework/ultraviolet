using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI;
using UltravioletSample.Sample10_AsynchronousContentLoading.Assets;

namespace UltravioletSample.Sample10_AsynchronousContentLoading.UI.Screens
{
    public class LoadingScreen : UIScreen
    {
        public LoadingScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/LoadingScreen", "LoadingScreen", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            this.uiScreenService = uiScreenService;
            this.textRenderer    = new TextRenderer();
            this.blankTexture    = GlobalContent.Load<Texture2D>(GlobalTextureID.Blank);
            this.font            = GlobalContent.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spinnerSprite   = LocalContent.Load<Sprite>("Spinner");
            this.loader          = new AsynchronousContentLoader();

            this.textRenderer.RegisterIcon("spinner", spinnerSprite[0]);

            UpdateMessage(String.Empty);
        }

        public void SetContentManager(ContentManager content)
        {
            loaderContent = content;
        }

        public void AddLoadingStep(String message)
        {
            loader.AddStep(() => { UpdateMessage(message); });
        }

        public void AddLoadingStep(Action step)
        {
            loader.AddStep(step);
        }

        public void AddLoadingStep(Action<ContentManager> step)
        {
            loader.AddStep(step);
        }

        public void AddLoadingStep(ContentManifest manifest)
        {
            loader.AddStep(manifest);
        }

        public void AddLoadingDelay(Int32 milliseconds)
        {
            loader.AddDelay(milliseconds);
        }

        protected override void OnOpened()
        {
            loader.AddGarbageCollection();
            loader.Load(loaderContent);
            base.OnOpened();
        }

        protected override void OnClosing()
        {
            loader.Reset();
            base.OnClosing();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            spinnerSprite.Update(time);

            if (loader.IsLoaded)
            {
                var screen = uiScreenService.Get<GameplayScreen>();
                Screens.OpenBelow(screen, this);
                Screens.Close(this);
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTexture, new RectangleF(0, 0, Width, Height), Color.Black * TransitionPosition);

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, message, Vector2.Zero, Color.White * TransitionPosition, settings);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private void UpdateMessage(String message)
        {
            this.message = "|icon:spinner|\n" + message;
        }

        private readonly UIScreenService uiScreenService;
        private readonly TextRenderer textRenderer;
        private readonly Texture2D blankTexture;
        private readonly Sprite spinnerSprite;
        private readonly SpriteFont font;

        private readonly AsynchronousContentLoader loader;
        private ContentManager loaderContent;
        private String message;
    }
}
