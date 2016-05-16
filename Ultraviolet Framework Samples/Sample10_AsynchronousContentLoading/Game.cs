using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample10_AsynchronousContentLoading.Assets;
using UltravioletSample.Sample10_AsynchronousContentLoading.Input;
using UltravioletSample.Sample10_AsynchronousContentLoading.UI;
using UltravioletSample.Sample10_AsynchronousContentLoading.UI.Screens;

namespace UltravioletSample.Sample10_AsynchronousContentLoading
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 10 - Asynchronous Content Loading", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Sample 10 - Asynchronous Content Loading")
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
            SetFileSourceFromManifestIfExists("UltravioletSample.Content.uvarc");

            base.OnInitialized();
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
            base.OnDrawing(time);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadInputBindings();
            LoadContentManifests();

            var screenService = new UIScreenService(content);
            var screen        = screenService.Get<LoadingScreen>();

            screen.SetContentManager(this.content);
            screen.AddLoadingStep("Loading, please wait...");
            screen.AddLoadingStep(Ultraviolet.GetContent().Manifests["Global"]);
            screen.AddLoadingDelay(2500);
            screen.AddLoadingStep("Loading interface...");
            screen.AddLoadingStep(screenService.Load);
            screen.AddLoadingDelay(2500);
            screen.AddLoadingStep("Reticulating splines...");
            screen.AddLoadingDelay(2500);

            Ultraviolet.GetUI().GetScreens().Open(screen, TimeSpan.Zero);

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
            uvContent.Manifests["Global"]["Textures"].PopulateAssetLibrary(typeof(GlobalTextureID));
        }

        private ContentManager content;
    }
}
