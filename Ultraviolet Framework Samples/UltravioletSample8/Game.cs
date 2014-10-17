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
using TwistedLogik.Ultraviolet.Input;

namespace UltravioletSample
{
    public class Game : UltravioletApplication
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 8")
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
            foreach (var soundEffectPlayer in soundEffectPlayers)
                soundEffectPlayer.Update(time);

            var keyboard = Ultraviolet.GetInput().GetKeyboard();

            if (keyboard.IsKeyPressed(Key.D1))
                soundEffectPlayers[0].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D2))
                soundEffectPlayers[1].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D3))
                soundEffectPlayers[2].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D4))
                soundEffectPlayers[3].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D5))
                soundEffectPlayers[4].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D6))
                soundEffectPlayers[5].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D7))
                soundEffectPlayers[6].Play(soundEffect);
            if (keyboard.IsKeyPressed(Key.D8))
                soundEffectPlayers[7].Play(soundEffect);

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            var attribution = 
                "Press the |c:FFFFFF00|1-8 number keys|c| to activate one of the sound effect players.\n\n" +
                "\"|c:FFFFFF00|grenade.wav|c|\" by ljudman (http://freesound.org/people/ljudman)\n" +
                "Licensed under Creative Commons: Sampling+\n" +
                "|c:FF808080|http://creativecommons.org/licenses/sampling+/1.0/|c|";
            var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
            textRenderer.CalculateLayout(attribution, textLayoutResult, settings);
            textRenderer.Draw(spriteBatch, textLayoutResult, Vector2.Zero, Color.White);

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
            this.textRenderer     = new TextRenderer();
            this.textLayoutResult = new TextLayoutResult();

            for (int i = 0; i < this.soundEffectPlayers.Length; i++)
            {
                this.soundEffectPlayers[i] = SoundEffectPlayer.Create();
            }
            this.soundEffect = this.content.Load<SoundEffect>(GlobalSoundEffectID.Explosion);

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
            uvContent.Manifests["Global"]["SoundEffects"].PopulateAssetLibrary(typeof(GlobalSoundEffectID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private TextLayoutResult textLayoutResult;
        private SoundEffect soundEffect;
        private readonly SoundEffectPlayer[] soundEffectPlayers = new SoundEffectPlayer[8];
    }
}
