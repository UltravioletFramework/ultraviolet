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
    public class GameplayScreen : UIScreen
    {
        public GameplayScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/GameplayScreen", "GameplayScreen", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            this.textRenderer    = new TextRenderer();
            this.blankTexture    = GlobalContent.Load<Texture2D>(GlobalTextureID.Blank);
            this.photograph      = GlobalContent.Load<Texture2D>(GlobalTextureID.Photograph);
            this.font            = GlobalContent.Load<SpriteFont>(GlobalFontID.SegoeUI);
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            base.OnUpdating(time);
        }

        protected override void OnDrawingBackground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(photograph, new RectangleF(0, 0, Width, Height), Color.White * TransitionPosition);

            base.OnDrawingBackground(time, spriteBatch);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTexture, new RectangleF(64, 64, Width - 128, Height - 128), Color.Black * 0.75f * TransitionPosition);

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "Welcome to the game!", Vector2.Zero, Color.White * TransitionPosition, settings);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private readonly TextRenderer textRenderer;
        private readonly Texture2D blankTexture;
        private readonly Texture2D photograph;
        private readonly SpriteFont font;
    }
}
