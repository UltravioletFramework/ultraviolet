using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// An Ultraviolet application used for unit testing.
    /// </summary>
    internal partial class UltravioletTestApplication : UltravioletApplication, IUltravioletTestApplication
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletTestApplication class.
        /// </summary>
        /// <param name="headless">A value indicating whether to create a headless context.</param>
        /// <param name="serviceMode">A value indicating whether to create a service mode context.</param>
        public UltravioletTestApplication(Boolean headless = false, Boolean serviceMode = false)
            : base("TwistedLogik", "Ultraviolet Unit Tests")
        {
            PreserveApplicationSettings = false;
            
            this.headless = headless;
            this.serviceMode = serviceMode;
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
        public IUltravioletTestApplication OnFrame(Int32 frame, Action<IUltravioletTestApplication> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(frame, action));

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
            RunUntil(() => DateTime.UtcNow >= startTime + time);
        }

        /// <inheritdoc/>
        public void RunForOneFrame()
        {
            RunFor(TimeSpan.Zero);
        }

        /// <inheritdoc/>
        public void RunAllFrameActions()
        {
            RunUntil(() =>
            {
                return !frameActions.Any(x => x.Frame >= frameCount);
            });
        }

        /// <inheritdoc/>
        public void SpoofKeyDown(Scancode scancode, Key key, Boolean ctrl, Boolean alt, Boolean shift)
        {
            var data = Ultraviolet.Messages.CreateMessageData<SDL2EventMessageData>();
            data.Event = new SDL_Event()
            {
                key = new SDL_KeyboardEvent()
                {
                    type = (uint)SDL_EventType.KEYDOWN,
                    windowID = (uint)Ultraviolet.GetPlatform().Windows.GetPrimary().ID,                     
                    keysym = new SDL_Keysym()
                    {
                        keycode = (SDL_Keycode)key,
                        scancode = (SDL_Scancode)scancode,                      
                        mod = 
                            (ctrl ? SDL_Keymod.CTRL : SDL_Keymod.NONE) |
                            (alt ? SDL_Keymod.ALT : SDL_Keymod.NONE) |
                            (shift ? SDL_Keymod.SHIFT : SDL_Keymod.NONE),
                    },
                }
            };
            Ultraviolet.Messages.Publish(SDL2UltravioletMessages.SDLEvent, data);
        }

        /// <inheritdoc/>
        public void SpoofKeyUp(Scancode scancode, Key key, Boolean ctrl, Boolean alt, Boolean shift)
        {
            var data = Ultraviolet.Messages.CreateMessageData<SDL2EventMessageData>();
            data.Event = new SDL_Event()
            {
                key = new SDL_KeyboardEvent()
                {
                    type = (uint)SDL_EventType.KEYUP,
                    windowID = (uint)Ultraviolet.GetPlatform().Windows.GetPrimary().ID,
                    keysym = new SDL_Keysym()
                    {
                        keycode = (SDL_Keycode)key,
                        scancode = (SDL_Scancode)scancode,
                        mod =
                            (ctrl ? SDL_Keymod.CTRL : SDL_Keymod.NONE) |
                            (alt ? SDL_Keymod.ALT : SDL_Keymod.NONE) |
                            (shift ? SDL_Keymod.SHIFT : SDL_Keymod.NONE),
                    },
                }
            };
            Ultraviolet.Messages.Publish(SDL2UltravioletMessages.SDLEvent, data);
        }

        /// <inheritdoc/>
        public void SpoofKeyPress(Scancode scancode, Key key, Boolean ctrl, Boolean alt, Boolean shift)
        {
            SpoofKeyDown(scancode, key, ctrl, alt, shift);
            SpoofKeyUp(scancode, key, ctrl, alt, shift);
        }

        /// <inheritdoc/>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            configuration.Headless = headless;
            configuration.EnableServiceMode = serviceMode;
            configuration.IsHardwareInputDisabled = true;
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

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            if (!headless)
                Ultraviolet.GetPlatform().Windows.GetPrimary().ClientSize = new Size2(480, 360);

            if (initializer != null)
            {
                initializer(Ultraviolet);
            }

            Ultraviolet.FrameStart += OnFrameStart;
            
            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void OnShutdown()
        {
            if (!Ultraviolet.Disposed)
            {
                Ultraviolet.FrameStart -= OnFrameStart;
            }
            base.OnShutdown();
        }

        /// <inheritdoc/>
        protected override void OnLoadingContent()
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();

            if (!headless)
            {
                // HACK: AMD drivers produce weird rasterization artifacts when rendering
                // to a NPOT render buffers??? So we have to fix it with this stupid hack???
                var width = MathUtil.FindNextPowerOfTwo(window.ClientSize.Width);
                var height = MathUtil.FindNextPowerOfTwo(window.ClientSize.Height);

                rtargetColorBuffer = RenderBuffer2D.Create(RenderBufferFormat.Color, width, height);
                rtargetDepthStencilBuffer = RenderBuffer2D.Create(RenderBufferFormat.Depth24Stencil8, width, height);
                rtarget = RenderTarget2D.Create(width, height);
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
        /// Occurs at the start of a frame.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private void OnFrameStart(UltravioletContext uv)
        {
            if (frameCount == 0)
                startTime = DateTime.UtcNow;

            if (frameActions != null)
            {
                var actions = frameActions.Where(x => x.Frame == frameCount);
                foreach (var action in actions)
                {
                    action.Action(this);
                }
            }
            frameCount++;
        }

        /// <summary>
        /// Converts the specified render target to a bitmap image.
        /// </summary>
        /// <param name="rt">The render target to convert.</param>
        /// <returns>The converted bitmap image.</returns>
        private Bitmap ConvertRenderTargetToBitmap(RenderTarget2D rt)
        {
            // HACK: Our buffer has been rounded up to the nearest
            // power of two, so at this point we clip it back down
            // to the size of the window.

            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var windowWidth = window.ClientSize.Width;
            var windowHeight = window.ClientSize.Height;

            var data = new Color[rt.Width * rt.Height];
            rt.GetData(data);

            var bmp = new Bitmap(windowWidth, windowHeight);
            var pixel = 0;
            for (int y = 0; y < rt.Height; y++)
            {
                for (int x = 0; x < rt.Width; x++)
                {
                    if (x < windowWidth && y < windowHeight)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.FromArgb((int)data[pixel].ToArgb()));
                    }
                    pixel++;
                }
            }

            return bmp;
        }

        // State values.
        private readonly Boolean headless;
        private readonly Boolean serviceMode;
        private Boolean configureUPF;
        private String audioSubsystem;
        private Func<Boolean> shouldExit;
        private ContentManager content;
        private Action<UltravioletContext> initializer;
        private Action<ContentManager> loader;
        private Action<UltravioletContext> renderer;
        private Bitmap bmp;
        private Int32 frameCount;
        private Int32 framesToSkip;
        private DateTime startTime;
        private List<FrameAction> frameActions;

        // The render target to which the test scene will be rendered.
        private RenderTarget2D rtarget;
        private RenderBuffer2D rtargetColorBuffer;
        private RenderBuffer2D rtargetDepthStencilBuffer;
    }
}
