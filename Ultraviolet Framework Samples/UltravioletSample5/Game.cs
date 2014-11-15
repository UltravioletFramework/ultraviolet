using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Assets;
using UltravioletSample.Input;

namespace UltravioletSample
{
#if ANDROID
    [Android.App.Activity(Label = "Ultraviolet Sample 5", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 5")
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

        protected override void OnInitialized()
        {
            const String Archive = "UltravioletSample.Content.uvarc";

            if (GetType().Assembly.GetManifestResourceStream(Archive) != null)
            {
                SetFileSourceFromManifest(Archive);
            }
            else
            {
                if (Ultraviolet.Platform == UltravioletPlatform.Android)
                {
                    throw new InvalidOperationException("Unable to set the file system source archive.");
                }
            }

            base.OnInitialized();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            this.sprite.Update(time);

            if (time.TotalTime.TotalMilliseconds > 250 && !controller1.IsPlaying)
            {
                controller1.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 500 && !controller2.IsPlaying)
            {
                controller2.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 750 && !controller3.IsPlaying)
            {
                controller3.PlayAnimation(sprite["Explosion"]);
            }
            if (time.TotalTime.TotalMilliseconds > 1000 && !controller4.IsPlaying)
            {
                controller4.PlayAnimation(sprite["Explosion"]);
            }

            controller1.Update(time);
            controller2.Update(time);
            controller3.Update(time);
            controller4.Update(time);

            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.DrawSprite(this.sprite["Explosion"].Controller, new Vector2(32, 32));            
            spriteBatch.DrawSprite(controller1, new Vector2(132, 32));
            spriteBatch.DrawSprite(controller2, new Vector2(232, 32));
            spriteBatch.DrawSprite(controller3, new Vector2(332, 32));
            spriteBatch.DrawSprite(controller4, new Vector2(432, 32));
            
            spriteBatch.End();

            base.OnDrawing(time);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadInputBindings();
            LoadContentManifests();

            this.sprite = this.content.Load<Sprite>(GlobalSpriteID.Explosion);
            this.spriteBatch = SpriteBatch.Create();

            this.controller1 = new SpriteAnimationController();
            this.controller2 = new SpriteAnimationController();
            this.controller3 = new SpriteAnimationController();
            this.controller4 = new SpriteAnimationController();

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

            uvContent.Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private ContentManager content;
        private Sprite sprite;
        private SpriteAnimationController controller1;
        private SpriteAnimationController controller2;
        private SpriteAnimationController controller3;
        private SpriteAnimationController controller4;
        private SpriteBatch spriteBatch;
    }
}
