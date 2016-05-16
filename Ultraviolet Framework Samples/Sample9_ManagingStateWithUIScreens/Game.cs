using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample9_ManagingStateWithUIScreens.Assets;
using UltravioletSample.Sample9_ManagingStateWithUIScreens.Input;
using UltravioletSample.Sample9_ManagingStateWithUIScreens.UI;
using UltravioletSample.Sample9_ManagingStateWithUIScreens.UI.Screens;

namespace UltravioletSample.Sample9_ManagingStateWithUIScreens
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 9 - Managing State with UI Screens", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Sample 9 - Managing State with UI Screens")
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
            var screen        = screenService.Get<SampleScreen1>();
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

            uvContent.Manifests["Global"]["Textures"].PopulateAssetLibrary(typeof(GlobalTextureID));
        }

        private ContentManager content;
    }
}
