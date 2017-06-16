using System;
using System.IO;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.OpenGL;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Styles;
using UltravioletSample.Sample13_UPFAdvanced.Input;
using UltravioletSample.Sample13_UPFAdvanced.UI;
using UltravioletSample.Sample13_UPFAdvanced.UI.Screens;

namespace UltravioletSample.Sample13_UPFAdvanced
{
    /// <summary>
    /// Represents the main application object.
    /// </summary>
#if ANDROID
    [Android.App.Activity(Label = "Sample 13 - UPF Advanced", MainLauncher = true, ConfigurationChanges =
        Android.Content.PM.ConfigChanges.Orientation |
        Android.Content.PM.ConfigChanges.ScreenSize |
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public partial class Game : SampleApplicationBase2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game() 
            : base("Ultraviolet", "Sample 13 - UPF Advanced", uv => uv.GetInput().GetActions())
        {
            PlatformSpecificInitialization();
        }

        partial void PlatformSpecificInitialization();

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">An array containing the application's command line arguments.</param>
        public static void Main(String[] args)
        {
            using (var game = new Game())
            {
                game.resolveContent = args.Contains("-resolve:content");
                game.compileContent = args.Contains("-compile:content");
                game.compileExpressions = args.Contains("-compile:expressions");

                game.Run();
            }
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <returns>The Ultraviolet context.</returns>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            configuration.EnableServiceMode = ShouldRunInServiceMode();
            configuration.WatchViewFilesForChanges = ShouldDynamicallyReloadContent();
            PopulateConfiguration(configuration);

            PresentationFoundation.Configure(configuration);

#if DEBUG
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };
            configuration.WatchViewFilesForChanges = true;
#endif
            return new OpenGLUltravioletContext(this, configuration);
        }
        
        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected override void OnLoadingContent()
        {
            ContentManager.GloballySuppressDependencyTracking = !ShouldDynamicallyReloadContent();
            this.content = ContentManager.Create("Content");

            if (Ultraviolet.IsRunningInServiceMode)
            {
                LoadPresentation();
                CompileContent();
                CompileBindingExpressions();
                Environment.Exit(0);
            }
            else
            {
                LoadLocalizationDatabases(content);
                LoadContentManifests(content);
                LoadPresentation();

                this.screenService = new UIScreenService(content);

                GC.Collect(2);

                var screen = screenService.Get<GameMenuScreen>();
                Ultraviolet.GetUI().GetScreens().Open(screen);
            }

            base.OnLoadingContent();
        }
        
        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }
        
        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
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

        /// <summary>
        /// Gets a value indicating whether the game should run in service mode.
        /// </summary>
        /// <returns><c>true</c> if the game should run in service mode; otherwise, <c>false</c>.</returns>
        private Boolean ShouldRunInServiceMode()
        {
            return compileContent || compileExpressions;
        }

        /// <summary>
        /// Gets a value indicating whether the game should compile its content into an archive.
        /// </summary>
        /// <returns></returns>
        private Boolean ShouldCompileContent()
        {
            return compileContent;
        }

        /// <summary>
        /// Gets a value indicating whether the game should compile binding expressions.
        /// </summary>
        /// <returns><c>true</c> if the game should compile binding expressions; otherwise, <c>false</c>.</returns>
        private Boolean ShouldCompileBindingExpressions()
        {
#if DEBUG
            return true;
#else
            return compileExpressions || System.Diagnostics.Debugger.IsAttached;
#endif
        }

        /// <summary>
        /// Gets a value indicating whether the game should enable dynamic reloading of content assets.
        /// </summary>
        private Boolean ShouldDynamicallyReloadContent()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Compiles the game's content into an archive file.
        /// </summary>
        private void CompileContent()
        {
            if (ShouldCompileContent())
            {
                if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
                    throw new NotSupportedException();

                var archive = ContentArchive.FromFileSystem(new[] { "Content" });
                using (var stream = File.OpenWrite("Content.uvarc"))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        archive.Save(writer);
                    }
                }
            }
        }

        /// <summary>
        /// Compiles the game's binding expressions.
        /// </summary>
        private void CompileBindingExpressions()
        {
            if (ShouldCompileBindingExpressions())
            {
                var upf = Ultraviolet.GetUI().GetPresentationFoundation();

                var flags = CompileExpressionsFlags.None;

                if (resolveContent)
                    flags |= CompileExpressionsFlags.ResolveContentFiles;

                if (compileExpressions)
                    flags |= CompileExpressionsFlags.IgnoreCache;

                upf.CompileExpressionsIfSupported("Content", flags);
            }
        }

        /// <summary>
        /// Loads files necessary for the Presentation Foundation.
        /// </summary>
        private void LoadPresentation()
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.RegisterKnownTypes(GetType().Assembly);

            if (!ShouldRunInServiceMode())
            {
                globalStyleSheet = GlobalStyleSheet.Create();
                globalStyleSheet.Append(content, "UI/DefaultUIStyles");
                globalStyleSheet.Append(content, "UI/GameStyles");
                upf.SetGlobalStyleSheet(globalStyleSheet.ToUvssDocument());

                CompileBindingExpressions();
                upf.LoadCompiledExpressions();                
            }
        }

        // The global content manager.  Manages any content that should remain loaded for the duration of the game's execution.
        private ContentManager content;

        // State values.
        private GlobalStyleSheet globalStyleSheet;
        private UIScreenService screenService;
        private Boolean resolveContent;
        private Boolean compileContent;
        private Boolean compileExpressions;
    }
}
