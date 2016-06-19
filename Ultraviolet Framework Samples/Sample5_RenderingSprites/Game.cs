using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample5_RenderingSprites.Assets;
using UltravioletSample.Sample5_RenderingSprites.Input;

namespace UltravioletSample.Sample5_RenderingSprites
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 5 - Rendering Sprites", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public class Game : SampleApplicationBase2
    {
        public Game()
            : base("TwistedLogik", "Sample 5 - Rendering Sprites", uv => uv.GetInput().GetActions())
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

            this.sprite = this.content.Load<Sprite>(GlobalSpriteID.Explosion);
            this.spriteBatch = SpriteBatch.Create();

            this.controller1 = new SpriteAnimationController();
            this.controller2 = new SpriteAnimationController();
            this.controller3 = new SpriteAnimationController();
            this.controller4 = new SpriteAnimationController();

            base.OnLoadingContent();
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

            Ultraviolet.GetContent().Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
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
