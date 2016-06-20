using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample8_PlayingSoundEffects.Assets;
using UltravioletSample.Sample8_PlayingSoundEffects.Input;

namespace UltravioletSample.Sample8_PlayingSoundEffects
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 8 - Playing Sound Effects", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public class Game : SampleApplicationBase2
    {
        public Game()
            : base("TwistedLogik", "Sample 8 - Playing Sound Effects", uv => uv.GetInput().GetActions())
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

            this.spriteFont = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            this.textRenderer = new TextRenderer();
            this.textLayoutCommands = new TextLayoutCommandStream();

            for (int i = 0; i < this.soundEffectPlayers.Length; i++)
                this.soundEffectPlayers[i] = SoundEffectPlayer.Create();

            this.soundEffect = this.content.Load<SoundEffect>(GlobalSoundEffectID.Explosion);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            foreach (var soundEffectPlayer in soundEffectPlayers)
                soundEffectPlayer.Update(time);

            var touch = Ultraviolet.GetInput().GetTouchDevice();
            if (touch != null && touch.WasTapped())
            {
                soundEffectPlayers[nextPlayerInSequence].Play(soundEffect);
                nextPlayerInSequence = (nextPlayerInSequence + 1) % soundEffectPlayers.Length;
            }

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
            var width  = window.DrawableSize.Width;
            var height = window.DrawableSize.Height;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            var instruction = Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS ?
                "|c:FFFFFF00|Tap the screen|c| to activate one of the sound effect players." :
                "Press the |c:FFFFFF00|1-8 number keys|c| to activate one of the sound effect players.";
            var attribution = 
                instruction + "\n\n" +
                "\"|c:FFFFFF00|grenade.wav|c|\" by ljudman (http://freesound.org/people/ljudman)\n" +
                "Licensed under Creative Commons: Sampling+\n" +
                "|c:FF808080|http://creativecommons.org/licenses/sampling+/1.0/|c|";

            var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
            textRenderer.CalculateLayout(attribution, textLayoutCommands, settings);
            textRenderer.Draw(spriteBatch, textLayoutCommands, Vector2.Zero, Color.White);

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
            Ultraviolet.GetContent().Manifests["Global"]["SoundEffects"].PopulateAssetLibrary(typeof(GlobalSoundEffectID));
        }

        private ContentManager content;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private TextLayoutCommandStream textLayoutCommands;
        private SoundEffect soundEffect;
        private readonly SoundEffectPlayer[] soundEffectPlayers = new SoundEffectPlayer[8];
        private Int32 nextPlayerInSequence;
    }
}
