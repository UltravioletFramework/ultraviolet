using System;
using System.IO;
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
using UltravioletSample.Assets;
using UltravioletSample.Input;

namespace UltravioletSample
{
    public class Game : UltravioletApplication
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 7")
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
            textRenderer.CalculateLayout(attribution, textLayoutResult, settings);
            textRenderer.Draw(spriteBatch, textLayoutResult, Vector2.Zero, Color.White);

            var timerSize = spriteFont.Regular.MeasureString(stringBuffer);
            var timerPosition = new Vector2(
                textLayoutResult.Bounds.Left + ((textLayoutResult.Bounds.Width - timerSize.Width) / 2f), 
                textLayoutResult.Bounds.Bottom - timerSize.Height);
            spriteBatch.DrawString(spriteFont.Regular, stringBuffer, timerPosition, Color.White);

            spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadInputBindings();
            LoadContentManifests();

            this.spriteFont       = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch      = SpriteBatch.Create();
            this.stringBuffer     = new StringBuilder();
            this.stringFormatter  = new StringFormatter();
            this.textRenderer     = new TextRenderer();
            this.textLayoutResult = new TextLayoutResult();

            this.song       = this.content.Load<Song>(GlobalSongID.DeepHaze);
            this.songPlayer = SongPlayer.Create();
            this.songPlayer.Play(this.song);
            
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
            uvContent.Manifests["Global"]["Songs"].PopulateAssetLibrary(typeof(GlobalSongID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private StringBuilder stringBuffer;
        private StringFormatter stringFormatter;
        private TextRenderer textRenderer;
        private TextLayoutResult textLayoutResult;
        private Song song;
        private SongPlayer songPlayer;
    }
}
