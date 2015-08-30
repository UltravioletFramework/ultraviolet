using System;
using System.Drawing;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// An Ultraviolet application used for unit testing.
    /// </summary>
    internal class UltravioletTestApplication : UltravioletApplication, IUltravioletTestApplication
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletTestApplication class.
        /// </summary>
        /// <param name="headless">A value indicating whether to create a headless context.</param>
        public UltravioletTestApplication(Boolean headless = false)
            : base("TwistedLogik", "Ultraviolet Unit Tests")
        {
            PreserveApplicationSettings = false;
            
            this.headless = headless;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithAudioSubsystem(String audioSubsystem)
        {
            this.audioSubsystem = audioSubsystem;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithPresentationFoundationConfigured()
        {
            configureUPF = true;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithInitialization(Action<UltravioletContext> initializer)
        {
            if (this.initializer != null)
                throw new InvalidOperationException("Initialization has already been configured.");

            this.initializer = initializer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithContent(Action<ContentManager> loader)
        {
            if (this.content != null)
                throw new InvalidOperationException("Content loading has already been configured.");

            this.loader = loader;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication SkipFrames(Int32 frameCount)
        {
            Contract.EnsureRange(frameCount >= 0, "frameCount");

            framesToSkip = frameCount;
            return this;
        }

        /// <inheritdoc/>
        public Bitmap Render(Action<UltravioletContext> renderer)
        {
            if (headless)
                throw new InvalidOperationException("Cannot render a headless window.");

            this.shouldExit = () => true;

            this.renderer = renderer;
            this.Run();

            return bmp;
        }

        /// <inheritdoc/>
        public void RunUntil(Func<Boolean> predicate)
        {
            this.shouldExit = predicate;
            this.Run();
        }

        /// <inheritdoc/>
        public void RunFor(TimeSpan time)
        {
            var target = DateTime.UtcNow + time;
            RunUntil(() => DateTime.UtcNow >= target);
        }

        /// <inheritdoc/>
        public void RunForOneFrame()
        {
            RunFor(TimeSpan.Zero);
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <returns>The Ultraviolet context.</returns>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration() { Headless = headless };
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };

            if (!String.IsNullOrEmpty(audioSubsystem))
                configuration.AudioSubsystemAssembly = audioSubsystem;

            if (configureUPF)
                PresentationFoundation.Configure(configuration);
            
            return new OpenGLUltravioletContext(this, configuration);
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            if (!headless)
                Ultraviolet.GetPlatform().Windows.GetPrimary().ClientSize = new Size2(480, 360);

            if (initializer != null)
            {
                initializer(Ultraviolet);
            }
            
            base.OnInitialized();
        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected override void OnLoadingContent()
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();

            if (!headless)
            {
                rtargetColorBuffer = RenderBuffer2D.Create(RenderBufferFormat.Color, window.ClientSize.Width, window.ClientSize.Height);
                rtargetDepthStencilBuffer = RenderBuffer2D.Create(RenderBufferFormat.Depth24Stencil8, window.ClientSize.Width, window.ClientSize.Height);
                rtarget = RenderTarget2D.Create(window.ClientSize.Width, window.ClientSize.Height);
                rtarget.Attach(rtargetColorBuffer);
                rtarget.Attach(rtargetDepthStencilBuffer);
            }

            if (loader != null)
            {
                content = ContentManager.Create("Content");
                loader(content);
            }

            base.OnLoadingContent();
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected override void OnUpdating(UltravioletTime time)
        {
            if (framesToSkip == 0)
            {
                if (shouldExit())
                {
                    Exit();
                }
            }
            base.OnUpdating(time);
        }

        /// <summary>
        /// Called when the scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        protected override void OnDrawing(UltravioletTime time)
        {
            if (framesToSkip == 0)
            {
                Ultraviolet.GetGraphics().SetRenderTarget(rtarget);
                Ultraviolet.GetGraphics().Clear(Color.Black);

                if (renderer != null)
                {
                    renderer(Ultraviolet);
                }

                Ultraviolet.GetGraphics().SetRenderTarget(null);
                bmp = ConvertRenderTargetToBitmap(rtarget);
            }
            else
            {
                framesToSkip--;
            }
            base.OnDrawing(time);
        }

        /// <summary>
        /// Converts the specified render target to a bitmap image.
        /// </summary>
        /// <param name="rt">The render target to convert.</param>
        /// <returns>The converted bitmap image.</returns>
        private Bitmap ConvertRenderTargetToBitmap(RenderTarget2D rt)
        {
            var data = new Color[rt.Width * rt.Height];
            rt.GetData(data);

            var bmp    = new Bitmap(rtarget.Width, rtarget.Height);
            var pixel  = 0;
            for (int y = 0; y < rtarget.Height; y++)
            {
                for (int x = 0; x < rtarget.Width; x++)
                {
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb((int)data[pixel++].ToArgb()));
                }
            }

            return bmp;
        }

        // State values.
        private readonly Boolean headless;
        private Boolean configureUPF;
        private String audioSubsystem;
        private Func<Boolean> shouldExit;
        private ContentManager content;
        private Action<UltravioletContext> initializer;
        private Action<ContentManager> loader;
        private Action<UltravioletContext> renderer;
        private Bitmap bmp;
        private Int32 framesToSkip;

        // The render target to which the test scene will be rendered.
        private RenderTarget2D rtarget;
        private RenderBuffer2D rtargetColorBuffer;
        private RenderBuffer2D rtargetDepthStencilBuffer;
    }
}
