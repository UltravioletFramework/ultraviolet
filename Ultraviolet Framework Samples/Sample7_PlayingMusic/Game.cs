using System;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample7_PlayingMusic.Assets;
using UltravioletSample.Sample7_PlayingMusic.Input;

namespace UltravioletSample.Sample7_PlayingMusic
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 7 - Playing Music", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public class Game : SampleApplicationBase2
    {
        public Game()
            : base("TwistedLogik", "Sample 7 - Playing Music", uv => uv.GetInput().GetActions())
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
            LoadContentManifests(content);
            LoadLocalizationDatabases(content);

            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            this.stringBuffer = new StringBuilder();
            this.stringFormatter = new StringFormatter();
            this.textRenderer = new TextRenderer();
            this.textLayoutCommands = new TextLayoutCommandStream();

            this.song = this.content.Load<Song>(GlobalSongID.DeepHaze);
            this.songPlayer = SongPlayer.Create();
            this.songPlayer.Play(this.song);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            songPlayer.Update(time);

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var width  = window.ClientSize.Width;
            var height = window.ClientSize.Height;

            stringFormatter.Reset();
            stringFormatter.AddArgument(songPlayer.Position.Minutes);
            stringFormatter.AddArgument(songPlayer.Position.Seconds);
            stringFormatter.AddArgument(songPlayer.Duration.Minutes);
            stringFormatter.AddArgument(songPlayer.Duration.Seconds);
            stringFormatter.Format("{0:pad:2}:{1:pad:2} / {2:pad:2}:{3:pad:2}", stringBuffer);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            var attribution = 
                "|c:FFFFFF00|Now Playing|c|\n\n" +
                "\"|c:FFFFFF00|Deep Haze|c|\" by Kevin MacLeod (incompetech.com)\n" +
                "Licensed under Creative Commons: By Attribution 3.0\n" +
                "|c:FF808080|http://creativecommons.org/licenses/by/3.0/|c|\n\n\n";
            var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
            textRenderer.CalculateLayout(attribution, textLayoutCommands, settings);
            textRenderer.Draw(spriteBatch, textLayoutCommands, Vector2.Zero, Color.White);

            var timerSize = spriteFont.Regular.MeasureString(stringBuffer);
            var timerPosition = new Vector2(
                (Int32)(textLayoutCommands.Bounds.Left + ((textLayoutCommands.Bounds.Width - timerSize.Width) / 2f)), 
                (Int32)(textLayoutCommands.Bounds.Bottom - timerSize.Height));
            spriteBatch.DrawString(spriteFont.Regular, stringBuffer, timerPosition, Color.White);

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
            Ultraviolet.GetContent().Manifests["Global"]["Songs"].PopulateAssetLibrary(typeof(GlobalSongID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private StringBuilder stringBuffer;
        private StringFormatter stringFormatter;
        private TextRenderer textRenderer;
        private TextLayoutCommandStream textLayoutCommands;
        private Song song;
        private SongPlayer songPlayer;
    }
}
