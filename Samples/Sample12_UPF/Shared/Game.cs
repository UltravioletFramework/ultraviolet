using System;
using Ultraviolet.Core;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.OpenGL;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Styles;
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
            : base("Ultraviolet", "Sample 12 - UPF", uv => uv.GetInput().GetActions())
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

            this.screenService = new UIScreenService(content);

            GC.Collect(2);

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
                SafeDispose.DisposeRef(ref screenService);
                SafeDispose.DisposeRef(ref globalStyleSheet);
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        private void LoadPresentation()
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();

            var globalStylesheet = GlobalStyleSheet.Create();
            globalStylesheet.Append(content, "UI/DefaultUIStyles");
            upf.SetGlobalStyleSheet(globalStylesheet);

            upf.CompileExpressionsIfSupported("Content");
            upf.LoadCompiledExpressions();
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the game's execution.
        private ContentManager content;

        // State values.
        private GlobalStyleSheet globalStyleSheet;
        private UIScreenService screenService;
    }
}
