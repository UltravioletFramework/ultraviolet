using System;
using System.Drawing;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL;

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

        /// <summary>
        /// Specifies the application's audio subsystem assembly.
        /// </summary>
        /// <param name="audioSubsystem">The fully-qualified name of the application's audio subsystem assembly.</param>
        /// <returns>The Ultraviolet test application.</returns>
        public IUltravioletTestApplication WithAudioSubsystem(String audioSubsystem)
        {
            this.audioSubsystem = audioSubsystem;
            return this;
        }

        /// <summary>
        /// Specifies the application's initialization code.
        /// </summary>
        /// <param name="initializer">An action which will initialize the application.</param>
        /// <returns>The Ultraviolet test application.</returns>
        public IUltravioletTestApplication WithInitialization(Action<UltravioletContext> initializer)
        {
            if (this.initializer != null)
                throw new InvalidOperationException("Initialization has already been configured.");

            this.initializer = initializer;
            return this;
        }

        /// <summary>
        /// Specifies the application's content loading code.
        /// </summary>
        /// <param name="loader">An action which will load the unit test's required content.</param>
        /// <returns>The Ultraviolet test application.</returns>
        public IUltravioletTestApplication WithContent(Action<ContentManager> loader)
        {
            if (this.content != null)
                throw new InvalidOperationException("Content loading has already been configured.");

            this.loader = loader;
            return this;
        }

        /// <summary>
        /// Renders a scene and outputs the resulting image.
        /// </summary>
        /// <param name="renderer">An action which will render the desired scene.</param>
        /// <returns>A bitmap containing the result of rendering the specified scene.</returns>
        public Bitmap Render(Action<UltravioletContext> renderer)
        {
            if (headless)
                throw new InvalidOperationException("Cannot render a headless window.");

            this.shouldExit = () => true;
            this.renderer = renderer;
            this.Run();

            return bmp;
        }

        /// <summary>
        /// Runs the application until the specified predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate that evaluates when the application should exit.</param>
        public void RunUntil(Func<Boolean> predicate)
        {
            this.shouldExit = predicate;
            this.Run();
        }

        /// <summary>
        /// Runs the application until the specified period of time has elapsed.
        /// </summary>
        /// <param name="time">The amount of time for which to run the application.</param>
        public void RunFor(TimeSpan time)
        {
            var target = DateTime.UtcNow + time;
            RunUntil(() => DateTime.UtcNow >= target);
        }

        /// <summary>
        /// Runs a single frame of the application.
        /// </summary>
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
            if (!String.IsNullOrEmpty(audioSubsystem))
            {
                configuration.AudioSubsystemAssembly = audioSubsystem;
            }
            return new OpenGLUltravioletContext(this, configuration);
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
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
                rbuffer = RenderBuffer2D.Create(RenderBufferFormat.Color, window.ClientSize.Width, window.ClientSize.Height);
                rtarget = RenderTarget2D.Create(window.ClientSize.Width, window.ClientSize.Height);
                rtarget.Attach(rbuffer);
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
            if (shouldExit())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        /// <summary>
        /// Called when the scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        protected override void OnDrawing(UltravioletTime time)
        {
            Ultraviolet.GetGraphics().SetRenderTarget(rtarget);
            Ultraviolet.GetGraphics().Clear(Color.Black);

            if (renderer != null)
            {
                renderer(Ultraviolet);
            }

            Ultraviolet.GetGraphics().SetRenderTarget(null);

            bmp = ConvertRenderTargetToBitmap(rtarget);

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
        private String audioSubsystem;
        private Func<Boolean> shouldExit;
        private ContentManager content;
        private Action<UltravioletContext> initializer;
        private Action<ContentManager> loader;
        private Action<UltravioletContext> renderer;
        private Bitmap bmp;

        // The render target to which the test scene will be rendered.
        private RenderTarget2D rtarget;
        private RenderBuffer2D rbuffer;
    }
}
