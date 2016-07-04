using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;
using UltravioletSample.Sample12_UPF.Input;
using UltravioletSample.Sample12_UPF.UI;
using UltravioletSample.Sample12_UPF.UI.Screens;

namespace UltravioletSample.Sample12_UPF
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 12 - UPF", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public partial class Game : SampleApplicationBase2
    {
        public Game()
            : base("TwistedLogik", "Sample 12 - UPF", uv => uv.GetInput().GetActions())
        {
            PlatformSpecificInitialization();
        }

        partial void PlatformSpecificInitialization();

        public static void Main(String[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            PresentationFoundation.Configure(configuration);

            return new OpenGLUltravioletContext(this, configuration);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");
            LoadContentManifests(this.content);
            LoadLocalizationDatabases(this.content);
            LoadPresentation();

            GC.Collect(2);

            var screenService = new UIScreenService(content);
            var screen = screenService.Get<ExampleScreen>();

            Ultraviolet.GetUI().GetScreens().Open(screen);

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
            base.OnDrawing(time);
        }
        
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        private void LoadPresentation()
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();

            var globalStylesheet = content.Load<UvssDocument>("UI/DefaultUIStyles");
            upf.SetGlobalStyleSheet(globalStylesheet);

            upf.CompileExpressionsIfSupported("Content");
            upf.LoadCompiledExpressions();
        }
        
        private ContentManager content;
    }
}
