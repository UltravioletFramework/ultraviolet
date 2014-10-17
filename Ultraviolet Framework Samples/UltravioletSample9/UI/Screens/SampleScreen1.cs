using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI;
using UltravioletSample.Assets;

namespace UltravioletSample.UI.Screens
{
    public class SampleScreen1 : UIScreen
    {
        public SampleScreen1(ContentManager globalContent, UIScreenService uiScreenService)
            : base("SampleScreen1", Path.Combine("Content", "UI", "Screens", "SampleScreen1"))
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;

            this.uiScreenService = uiScreenService;

            this.font         = Content.Load<SpriteFont>("SegoeUI");
            this.blankTexture = globalContent.Load<Texture2D>(GlobalTextureID.Blank);
            this.textRenderer = new TextRenderer();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput && Ultraviolet.GetInput().GetKeyboard().IsKeyPressed(Key.Right))
            {
                var screen = uiScreenService.Get<SampleScreen2>();
                Screens.Open(screen);
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blankTexture, new RectangleF(0, 0, Width, Height), new Color(180, 0, 0));

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "This is SampleScreen1\nPress right arrow key to open SampleScreen2", 
                Vector2.Zero, Color.White * TransitionPosition, settings);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private readonly UIScreenService uiScreenService;

        private readonly SpriteFont font;
        private readonly Texture2D blankTexture;
        private readonly TextRenderer textRenderer;
    }
}
