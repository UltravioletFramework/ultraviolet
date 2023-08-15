using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.Platform;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Styles;
using Ultraviolet.SDL2;
using UvDebug.Input;
using UvDebug.UI;
using UvDebug.UI.Screens;

namespace UvDebug
{
    /// <summary>
    /// Represents the main application object.
    /// </summary>
    public partial class Game : UltravioletApplicationAdapter
    {
        /// <summary>
        /// Initializes a new instance of the Game 
        /// </summary>
        public Game(String[] args, IUltravioletApplicationAdapterHost host) : base(host)
        {
            resolveContent = args?.Contains("-resolve:content") ?? false;
            compileContent = args?.Contains("-compile:content") ?? false;
            compileExpressions = args?.Contains("-compile:expressions") ?? false;

            Diagnostics.DrawDiagnosticsVisuals = true;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(UltravioletConfiguration configuration)
        {
            var graphicsConfig = OpenGLGraphicsConfiguration.Default;
            graphicsConfig.MultiSampleBuffers = 1;
            graphicsConfig.MultiSampleSamples = 8;
            graphicsConfig.SrgbBuffersEnabled = false;
            graphicsConfig.SrgbDefaultForTexture2D = false;

            configuration.SupportsHighDensityDisplayModes = true;
            configuration.EnableServiceMode = ShouldRunInServiceMode();
            configuration.WatchViewFilesForChanges = ShouldDynamicallyReloadContent();
            configuration.Plugins.Add(new OpenGLGraphicsPlugin(graphicsConfig));
            configuration.Plugins.Add(new BASSAudioPlugin());
            configuration.Plugins.Add(new FreeTypeFontPlugin());
            configuration.Plugins.Add(new PresentationFoundationPlugin());

#if DEBUG
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };
#endif
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
                LoadTestGeometry();
               
                this.screenService = new UIScreenService(content);

                GC.Collect(2);
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
            var inputBindingsPath = Path.Combine(Host.GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
            Ultraviolet.GetInput().GetActions().Load(inputBindingsPath, throwIfNotFound: false);
        }

        /// <summary>
        /// Saves the game's input bindings.
        /// </summary>
        protected void SaveInputBindings()
        {
            var inputBindingsPath = Path.Combine(Host.GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
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
#if !IMGUI
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
#endif
        }

        /// <summary>
        /// Loads 3D geometry used for testing.
        /// </summary>
        protected void LoadTestGeometry()
        {
            /*
            vertexBuffer = VertexBuffer.Create<VertexPositionColorTexture>(5);
            vertexBuffer.SetData(new VertexPositionColorTexture[]
            {
                new VertexPositionColorTexture { Position = new Vector3(-1f,   0f, -1f), Color = Color.Red, TextureCoordinate = new Vector2(1, 0) },
                new VertexPositionColorTexture { Position = new Vector3( 1f,   0f, -1f), Color = Color.Lime, TextureCoordinate = new Vector2(0, 1) },
                new VertexPositionColorTexture { Position = new Vector3( 1f,   0f,  1f), Color = Color.Blue },
                new VertexPositionColorTexture { Position = new Vector3(-1f,   0f,  1f), Color = Color.Yellow },
                new VertexPositionColorTexture { Position = new Vector3( 0f, 1.5f,  0f), Color = Color.Magenta, TextureCoordinate = new Vector2(0, 0) },
            });
            */

            vertexBuffer = VertexBuffer.Create<VertexPositionNormalTexture>(36);
            vertexBuffer.SetData(new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, -1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(0, -1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, -1f, 0) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, -1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(0, -1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, -1f, 0) },

                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f) },

                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(1f, 0f, 0f) },

                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(1f, 0f, 0f) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f) },

                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(-1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(-1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(-1f, 0f, 0f) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(-1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(-1f, 0f, 0f) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(-1f, 0f, 0f) },

                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(0, 1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 1f, 0) },

                new VertexPositionNormalTexture { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 1f, 0) },
                new VertexPositionNormalTexture { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(0, 1f, 0) },
            });

            //indexBuffer = IndexBuffer.Create(IndexBufferElementType.Int16, 18);
            //indexBuffer.SetData(new Int16[] { 2, 1, 0, 0, 3, 2, 0, 1, 4, 1, 2, 4, 2, 3, 4, 3, 0, 4 });

            geometryStream = GeometryStream.Create();
            geometryStream.Attach(vertexBuffer);
            //geometryStream.Attach(indexBuffer);

            effect = BasicEffect.Create();
            effect.EnableStandardLighting();

            rasterizerStateSolid = RasterizerState.Create();
            rasterizerStateSolid.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerStateSolid.FillMode = FillMode.Solid;

            rasterizerStateWireframe = RasterizerState.Create();
            rasterizerStateWireframe.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerStateWireframe.FillMode = FillMode.Wireframe;

            texture = content.Load<Texture2D>(@"Textures\Triangle");
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                this.Host.Exit();
            }
            base.OnUpdating(time);
        }

        /// <summary>
        /// Called when the application's scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        protected override void OnDrawing(UltravioletTime time)
        {
            var gfx = Ultraviolet.GetGraphics();
            var window = Ultraviolet.GetPlatform().Windows.GetCurrent();
            var aspectRatio = window.DrawableSize.Width / (Single)window.DrawableSize.Height;

            effect.World = Matrix.CreateRotationY((float)(2.0 * Math.PI * (time.TotalTime.TotalSeconds / 10.0)));
            effect.View = Matrix.CreateLookAt(new Vector3(0, 3, 6), new Vector3(0, 0.75f, 0), Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);

            gfx.SetGeometryStream(geometryStream);

            void DrawGeometry(RasterizerState rasterizerState, DepthStencilState depthStencilState)
            {
                foreach (var pass in this.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    gfx.SetRasterizerState(rasterizerState);
                    gfx.SetDepthStencilState(depthStencilState);
                    gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
                    //gfx.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount / 3);
                }
            }

            effect.FogColor = Color.Red;
            effect.PreferPerPixelLighting = true;
            effect.LightingEnabled = true;
            effect.SrgbColor = false;
            effect.VertexColorEnabled = false;
            effect.DiffuseColor = Color.White;
            effect.TextureEnabled = false;
            effect.Texture = texture;
            DrawGeometry(rasterizerStateSolid, DepthStencilState.Default);

            if (!GL.IsGLES)
            {
                effect.LightingEnabled = false;
                effect.FogEnabled = false;
                effect.VertexColorEnabled = false;
                effect.DiffuseColor = Color.Black;
                effect.TextureEnabled = false;
                DrawGeometry(rasterizerStateWireframe, DepthStencilState.None);
            }
            
            base.OnDrawing(time);
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
#if !IMGUI
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
#endif
        }        
        
        // The global content manager.  Manages any content that should remain loaded for the duration of the game's execution.
        private ContentManager content;

        // State values.
        private GlobalStyleSheet globalStyleSheet;
        private UIScreenService screenService;
        private Boolean resolveContent;
        private Boolean compileContent;
        private Boolean compileExpressions;

        // 3D geometry testing.
        private GeometryStream geometryStream;
        private VertexBuffer vertexBuffer;
        private BasicEffect effect;
        private RasterizerState rasterizerStateSolid;
        private RasterizerState rasterizerStateWireframe;
        private Texture2D texture;
    }
}
