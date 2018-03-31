using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.OpenGL;
using Ultraviolet.Platform;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Styles;
using UvDebug.Input;
using UvDebug.UI;
using UvDebug.UI.Screens;

namespace UvDebug
{
    /// <summary>
    /// Represents the main application object.
    /// </summary>
#if ANDROID
    [Android.App.Activity(Label = "UvDebug", MainLauncher = true, ConfigurationChanges =
        Android.Content.PM.ConfigChanges.Orientation |
        Android.Content.PM.ConfigChanges.ScreenSize |
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public partial class Game : UltravioletActivity
#else
    public partial class Game : UltravioletApplication
#endif
    {
        /// <summary>
        /// Initializes a new instance of the Game 
        /// </summary>
        public Game() : base("Ultraviolet", "UvDebug")
        {
            Diagnostics.DrawDiagnosticsVisuals = true;
            PlatformSpecificInitialization();
        }

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
            configuration.SupportsHighDensityDisplayModes = true;
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
#endif

            return new OpenGLUltravioletContext(this, configuration);
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            if (!SetFileSourceFromManifestIfExists("UvDebug.Content.uvarc"))
                UsePlatformSpecificFileSource();

#if !ANDROID && !IOS
            global::Ultraviolet.FreeType2.FreeTypeFontPlugin.Initialize(Ultraviolet);
#endif

            base.OnInitialized();
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
                LoadLocalizationPlugins();
                LoadLocalizationDatabases();
                LoadInputBindings();
                LoadContentManifests();
                LoadPresentation();

                this.screenService = new UIScreenService(content);

                var surface = content.Load<Ultraviolet.Graphics.Surface3D>("Textures/test_0");

                GC.Collect(2);
                
                var screen = screenService.Get<GameMenuScreen>();
                Ultraviolet.GetUI().GetScreens().Open(screen);
            }
            
            base.OnLoadingContent();
        }

        /// <summary>
        /// Loads the application's localization plugins.
        /// </summary>
        protected void LoadLocalizationPlugins()
        {
            var fss = FileSystemService.Create();
            var plugins = content.GetAssetFilePathsInDirectory(Path.Combine("Localization", "Plugins"), "*.dll");
            foreach (var plugin in plugins)
            {
                try
                {
                    var asm = Assembly.Load(plugin);
                    Localization.LoadPlugins(asm);
                }
                catch (Exception e) when (e is BadImageFormatException || e is FileLoadException) { }
            }
        }

        /// <summary>
        /// Loads the application's localization databases.
        /// </summary>
        protected void LoadLocalizationDatabases()
        {
            var fss = FileSystemService.Create();
            var databases = content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (var database in databases)
            {
                using (var stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }

        /// <summary>
        /// Loads the game's input bindings.
        /// </summary>
        protected void LoadInputBindings()
        {
            var inputBindingsPath = Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
            Ultraviolet.GetInput().GetActions().Load(inputBindingsPath, throwIfNotFound: false);
        }

        /// <summary>
        /// Saves the game's input bindings.
        /// </summary>
        protected void SaveInputBindings()
        {
            var inputBindingsPath = Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
            Ultraviolet.GetInput().GetActions().Save(inputBindingsPath);
        }

        /// <summary>
        /// Loads the game's content manifest files.
        /// </summary>
        protected void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);
        }

        /// <summary>
        /// Loads files necessary for the Presentation Foundation.
        /// </summary>
        protected void LoadPresentation()
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.RegisterKnownTypes(GetType().Assembly);

            if (!ShouldRunInServiceMode())
            {
                globalStyleSheet = GlobalStyleSheet.Create();
                globalStyleSheet.Append(content, "UI/DefaultUIStyles");
                globalStyleSheet.Append(content, "UI/GameStyles");
                upf.SetGlobalStyleSheet(globalStyleSheet);

                CompileBindingExpressions();
                upf.LoadCompiledExpressions();
            }
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
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
        /// Called when the application is being shut down.
        /// </summary>
        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
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
        /// Performs any platform-specific initialization.
        /// </summary>
        partial void PlatformSpecificInitialization();

        /// <summary>
        /// Gets a value indicating whether the game should run in service mode.
        /// </summary>
        /// <returns><see langword="true"/> if the game should run in service mode; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the game should compile binding expressions; otherwise, <see langword="false"/>.</returns>
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

                var sw = System.Diagnostics.Stopwatch.StartNew();
                upf.CompileExpressionsIfSupported("Content", flags);
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
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
