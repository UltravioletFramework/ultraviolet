using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using Ultraviolet.TestFramework;
using Ultraviolet.TestFramework.Graphics;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDL_Keymod;

namespace Ultraviolet.TestApplication
{
    /// <summary>
    /// An Ultraviolet application used for unit testing.
    /// </summary>
    public partial class UltravioletTestApplication : UltravioletApplication, IUltravioletTestApplication
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletTestApplication class.
        /// </summary>
        /// <param name="headless">A value indicating whether to create a headless context.</param>
        /// <param name="serviceMode">A value indicating whether to create a service mode context.</param>
        public UltravioletTestApplication(Boolean headless = false, Boolean serviceMode = false)
            : base("Ultraviolet", "Ultraviolet Unit Tests")
        {
            PreserveApplicationSettings = false;

            this.headless = headless;
            this.serviceMode = serviceMode;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithAudioImplementation(AudioImplementation audioImplementation)
        {
            this.audioImplementation = audioImplementation;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithConfiguration(Action<UltravioletConfiguration> configurer)
        {
            this.configurer = configurer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication WithPlugin(UltravioletPlugin plugin)
        {
            if (plugin == null)
                return this;

            if (this.plugins == null)
                this.plugins = new List<UltravioletPlugin>();

            this.plugins.Add(plugin);
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
        public IUltravioletTestApplication WithDispose(Action disposer)
        {
            if (this.disposer != null)
                throw new InvalidOperationException("Disposal has already been configured.");

            this.disposer = disposer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication SkipFrames(Int32 frameCount)
        {
            Contract.EnsureRange(frameCount >= 0, nameof(frameCount));

            framesToSkip = frameCount;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication OnFrameStart(Int32 frame, Action<IUltravioletTestApplication> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.FrameStart, frame, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication OnUpdate(Action<IUltravioletTestApplication, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Update, -1, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication OnUpdate(Int32 update, Action<IUltravioletTestApplication, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Update, update, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication OnRender(Action<IUltravioletTestApplication, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Render, -1, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplication OnRender(Int32 render, Action<IUltravioletTestApplication, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Render, render, action));

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
                return 
                    !frameActions.Any(x => x.ActionType == FrameActionType.FrameStart && x.ActionIndex >= frameCount) &&
                    !frameActions.Any(x => x.ActionType == FrameActionType.Render && x.ActionIndex >= renderCount) &&
                    !frameActions.Any(x => x.ActionType == FrameActionType.Update && x.ActionIndex >= updateCount);
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
                    type = (uint)SDL_KEYDOWN,
                    windowID = (uint)Ultraviolet.GetPlatform().Windows.GetPrimary().ID,                     
                    keysym = new SDL_Keysym()
                    {
                        keycode = (SDL_Keycode)key,
                        scancode = (SDL_Scancode)scancode,                      
                        mod = 
                            (ctrl ? KMOD_CTRL : KMOD_NONE) |
                            (alt ? KMOD_ALT : KMOD_NONE) |
                            (shift ? KMOD_SHIFT : KMOD_NONE),
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
                    type = (uint)SDL_KEYUP,
                    windowID = (uint)Ultraviolet.GetPlatform().Windows.GetPrimary().ID,
                    keysym = new SDL_Keysym()
                    {
                        keycode = (SDL_Keycode)key,
                        scancode = (SDL_Scancode)scancode,
                        mod =
                            (ctrl ? KMOD_CTRL : KMOD_NONE) |
                            (alt ? KMOD_ALT : KMOD_NONE) |
                            (shift ? KMOD_SHIFT : KMOD_NONE),
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

            if (audioImplementation != null)
                configuration.SelectAudioImplementation(audioImplementation.Value);

            configurer?.Invoke(configuration);

            if (plugins != null)
            {
                foreach (var plugin in plugins)
                    configuration.Plugins.Add(plugin);
            }

            return new OpenGLUltravioletContext(this, configuration);
        }        

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            if (!headless)
                Ultraviolet.GetPlatform().Windows.GetPrimary().ClientSize = new Size2(480, 360);

            initializer?.Invoke(Ultraviolet);

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
                // to a NPOT render buffer??? So we have to fix it with this stupid hack???
                var width = MathUtil.FindNextPowerOfTwo(window.DrawableSize.Width);
                var height = MathUtil.FindNextPowerOfTwo(window.DrawableSize.Height);

                rtargetColorBuffer = Texture2D.CreateRenderBuffer(RenderBufferFormat.Color, width, height);
                rtargetDepthStencilBuffer = Texture2D.CreateRenderBuffer(RenderBufferFormat.Depth24Stencil8, width, height);
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
            RunFrameActions(FrameActionType.Update, updateCount, time);

            if (framesToSkip == 0)
            {
                if (shouldExit())
                {
                    Exit();
                }
            }

            updateCount++;

            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time)
        {
            RunFrameActions(FrameActionType.Render, renderCount, time);

            if (framesToSkip == 0)
            {
                var window = 
                    Ultraviolet.GetPlatform().Windows.GetPrimary();

                var compositor = window.Compositor as ITestFrameworkCompositor;
                if (compositor != null)
                    compositor.TestFrameworkRenderTarget = rtarget;
                else
                {
                    Ultraviolet.GetGraphics().SetRenderTarget(rtarget);
                    Ultraviolet.GetGraphics().Clear(Color.Black);
                }

                renderer?.Invoke(Ultraviolet);

                if (compositor != null)
                {
                    window.Compositor.Compose();
                    window.Compositor.Present();
                }

                Ultraviolet.GetGraphics().SetRenderTargetToBackBuffer();
                Ultraviolet.GetGraphics().Clear(Color.CornflowerBlue);
                bmp = ConvertRenderTargetToBitmap(rtarget);
            }
            else
            {
                framesToSkip--;
            }

            renderCount++;

            base.OnDrawing(time);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                disposer?.Invoke();
                content?.Dispose();
                content = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs at the start of a frame.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private void OnFrameStart(UltravioletContext uv)
        {
            if (frameCount == 0)
                startTime = DateTime.UtcNow;

            RunFrameActions(FrameActionType.FrameStart, frameCount, null);
            frameCount++;
        }

        /// <summary>
        /// Runs the specified set of frame actions.
        /// </summary>
        private void RunFrameActions(FrameActionType actionType, Int32 actionIndex, UltravioletTime time)
        {
            if (frameActions == null)
                return;

            var actions = frameActions.Where(x => x.ActionType == actionType && (x.ActionIndex < 0 || x.ActionIndex == actionIndex));
            foreach (var action in actions)
            {
                switch (actionType)
                {
                    case FrameActionType.FrameStart:
                        ((Action<IUltravioletTestApplication>)action.Action)(this);
                        break;

                    default:
                        ((Action<IUltravioletTestApplication, UltravioletTime>)action.Action)(this, time);
                        break;
                }
            }
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
            var windowWidth = window.DrawableSize.Width;
            var windowHeight = window.DrawableSize.Height;

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
                        var rawColor = data[pixel];
                        
                        bmp.SetPixel(x, y, 
                            System.Drawing.Color.FromArgb(255, 
                            System.Drawing.Color.FromArgb((Int32)rawColor.ToArgb())));
                    }
                    pixel++;
                }
            }

            return bmp;
        }

        // State values.
        private readonly Boolean headless;
        private readonly Boolean serviceMode;
        private AudioImplementation? audioImplementation;
        private Func<Boolean> shouldExit;
        private ContentManager content;
        private Action<UltravioletConfiguration> configurer;
        private Action<UltravioletContext> initializer;
        private Action<ContentManager> loader;
        private Action<UltravioletContext> renderer;
        private Action disposer;
        private Bitmap bmp;
        private Int32 updateCount;
        private Int32 renderCount;
        private Int32 frameCount;
        private Int32 framesToSkip;
        private DateTime startTime;
        private List<FrameAction> frameActions;
        private List<UltravioletPlugin> plugins;

        // The render target to which the test scene will be rendered.
        private RenderTarget2D rtarget;
        private RenderBuffer2D rtargetColorBuffer;
        private RenderBuffer2D rtargetDepthStencilBuffer;
    }
}