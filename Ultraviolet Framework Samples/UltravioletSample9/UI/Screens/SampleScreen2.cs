using System;
using System.IO;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.Content;
using UltravioletSample.Assets;

namespace UltravioletSample.UI.Screens
{
    public class SampleScreen2 : UIScreen
    {
        public SampleScreen2(ContentManager globalContent, UIScreenService uiScreenService)
            : base("SampleScreen2", Path.Combine("Content", "UI", "Screens", "SampleScreen2"))
        {
            Contract.Require(uiScreenService, "uiScreenService");

            IsOpaque = true;

            this.uiScreenService = uiScreenService;

            this.font         = Content.Load<SpriteFont>("Garamond");
            this.blankTexture = globalContent.Load<Texture2D>(GlobalTextureID.Blank);
            this.textRenderer = new TextRenderer();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (IsReadyForInput && Ultraviolet.GetInput().GetKeyboard().IsKeyPressed(Key.Left))
            {
                Screens.Close(this);
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawingForeground(UltravioletTime time, SpriteBatch spriteBatch)
        {
            var offset = GetScreenOffset();
            spriteBatch.Draw(blankTexture, new RectangleF(offset, 0, Width, Height), new Color(0, 0, 180));

            var settings = new TextLayoutSettings(font, Width, Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
            textRenderer.Draw(spriteBatch, "This is SampleScreen2\nPress left arrow key to open SampleScreen1", 
                new Vector2(offset, 0), Color.White, settings);

            base.OnDrawingForeground(time, spriteBatch);
        }

        private Int32 GetScreenOffset()
        {
            if (State == UIPanelState.Opening)
            {
                return Width - (Int32)(Width * Easings.EaseOutBounce(TransitionPosition));
            }
            if (State == UIPanelState.Closing)
            {
                return (Int32)(Width * Easings.EaseInSin(1.0f - TransitionPosition));
            }
            return 0;
        }

        private readonly UIScreenService uiScreenService;

        private readonly SpriteFont font;
        private readonly Texture2D blankTexture;
        private readonly TextRenderer textRenderer;
    }
}
