using System;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample11_GamePads.Assets;
using UltravioletSample.Sample11_GamePads.Input;

namespace UltravioletSample.Sample11_GamePads
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 11 - Game Pads", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public class Game : SampleApplicationBase2
    {
        public Game()
            : base("TwistedLogik", "Sample 11 - Game Pads", uv => uv.GetInput().GetActions())
        {
#if IOS
            EnsureAssemblyIsLinked<TwistedLogik.Ultraviolet.BASS.BASSUltravioletAudio>();
#endif
        }

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

            this.spriteBatch = SpriteBatch.Create();
            this.textRenderer = new TextRenderer();
            this.textFormatter = new StringFormatter();
            this.textBuffer = new StringBuilder();

            var xbox360ControllerIcons = content.Load<Sprite>(GlobalSpriteID.Xbox360Controller);

            for (int i = 0; i < xbox360ControllerIcons.AnimationCount; i++)
            {
                this.textRenderer.RegisterIcon(
                    xbox360ControllerIcons[i].Name, 
                    xbox360ControllerIcons[i], 32, 32);
            }

            base.OnLoadingContent();
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
            var winSize = Ultraviolet.GetPlatform().Windows.GetPrimary().ClientSize;

            var x      = 0;
            var y      = 0;
            var width  = winSize.Width / 4;
            var height = winSize.Height;

            for (int i = 0; i < 4; i++)
            {
                DrawGamePadState(i, new Rectangle(x, y, width, height));
                x += width;
            }

            base.OnDrawing(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref spriteBatch);
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }
        
        protected override void LoadContentManifests(ContentManager content)
        {
            base.LoadContentManifests(content);

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            Ultraviolet.GetContent().Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private void DrawGamePadState(Int32 playerIndex, Rectangle area)
        {
            var input = Ultraviolet.GetInput();
            var device = input.GetGamePadForPlayer(playerIndex);
            var font = content.Load<SpriteFont>(GlobalFontID.SegoeUI);

            var x = area.X;
            var y = area.Y;
            var textArea = RectangleF.Empty;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            textFormatter.Reset();
            textFormatter.AddArgument(playerIndex + 1);
            textFormatter.AddArgument(device == null ? "(not connected)" : device.Name);
            textFormatter.AddArgument((device == null) ? "" : device.GetButtonState(TwistedLogik.Ultraviolet.Input.GamePadButton.A).ToString());
            textFormatter.Format("|c:FFFFFF00|Player {0}|c|\n{1}", textBuffer);

            var headerSettings = new TextLayoutSettings(font, area.Width, area.Height, TextFlags.AlignCenter | TextFlags.AlignTop);
            textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, headerSettings);

            y += (Int32)textArea.Height + font.Regular.LineSpacing;

            if (device != null)
            {
                textFormatter.Reset();
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.LeftShoulder) ? "LeftShoulder" : "LeftShoulderDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.RightShoulder) ? "RightShoulder" : "RightShoulderDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.A) ? "AButton" : "AButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.B) ? "BButton" : "BButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.X) ? "XButton" : "XButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.Y) ? "YButton" : "YButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.Back) ? "BackButton" : "BackButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.Start) ? "StartButton" : "StartButtonDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.DPadUp) ? "DPadUp" : "DPadUpDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.DPadDown) ? "DPadDown" : "DPadDownDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.DPadLeft) ? "DPadLeft" : "DPadLeftDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.DPadRight) ? "DPadRight" : "DPadRightDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.LeftStick) ? "LeftJoystick" : "LeftJoystickDisabled");
                textFormatter.AddArgument(device.IsButtonDown(GamePadButton.RightStick) ? "RightJoystick" : "RightJoystickDisabled");
                textFormatter.Format(
                    "|c:FFFFFF00|Buttons|c|\n\n" +
                    "|icon:{0}| |icon:{1}|\n" +
                    "|icon:{2}| |icon:{3}| |icon:{4}| |icon:{5}|\n" +
                    "|icon:{6}| |icon:{7}|\n" +
                    "|icon:{8}| |icon:{9}| |icon:{10}| |icon:{11}|\n" +
                    "|icon:{12}| |icon:{13}|\n\n" +
                    "|c:FFFFFF00|Axes|c|", textBuffer);

                var buttonSettings = new TextLayoutSettings(font, area.Width, area.Height, TextFlags.AlignCenter | TextFlags.AlignTop);
                textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, buttonSettings);

                y += (Int32)textArea.Height + font.Regular.LineSpacing;

                var axesLeftSettings = new TextLayoutSettings(font, area.Width, area.Height, TextFlags.AlignLeft | TextFlags.AlignTop);
                var axesRightSettings = new TextLayoutSettings(font, area.Width, area.Height, TextFlags.AlignRight | TextFlags.AlignTop);

                textFormatter.Reset();
                textFormatter.AddArgument(device.LeftTrigger);
                textFormatter.Format("|icon:LeftTrigger|{0:decimals:2}", textBuffer);

                textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, axesLeftSettings);

                textFormatter.Reset();
                textFormatter.AddArgument(device.RightTrigger);
                textFormatter.Format("{0:decimals:2}|icon:RightTrigger|", textBuffer);

                textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, axesRightSettings);

                y += (Int32)textArea.Height;

                textFormatter.Reset();
                textFormatter.AddArgument(device.LeftJoystickX);
                textFormatter.AddArgument(device.LeftJoystickY);
                textFormatter.Format("|icon:LeftJoystick|\nX={0:decimals:2}\nY={1:decimals:2}", textBuffer);

                textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, axesLeftSettings);

                textFormatter.Reset();
                textFormatter.AddArgument(device.RightJoystickX);
                textFormatter.AddArgument(device.RightJoystickY);
                textFormatter.Format("|icon:RightJoystick|\nX={0:decimals:2}\nY={1:decimals:2}", textBuffer);

                textArea = textRenderer.Draw(spriteBatch, textBuffer, new Vector2(x, y), Color.White, axesRightSettings);
            }

            spriteBatch.End();
        }

        private ContentManager content;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private StringFormatter textFormatter;
        private StringBuilder textBuffer;
    }
}
