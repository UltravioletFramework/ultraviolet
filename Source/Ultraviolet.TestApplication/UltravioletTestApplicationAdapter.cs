using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.FMOD;
using Ultraviolet.Graphics;
using Ultraviolet.Image;
using Ultraviolet.Input;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2;
using Ultraviolet.TestFramework;
using Ultraviolet.TestFramework.Graphics;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDL_Keymod;

namespace Ultraviolet.TestApplication
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UltravioletTestApplicationAdapter : UltravioletApplicationAdapter, IUltravioletTestApplicationAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public UltravioletTestApplicationAdapter(UltravioletTestApplication host) 
            : base(host)
        {
            this.host = host;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(UltravioletConfiguration configuration)
        {
            configuration.Headless = this.host.Headless;
            configuration.EnableServiceMode = this.host.ServiceMode;
            configuration.IsHardwareInputDisabled = true;
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };

            var needsGraphicsSubsystem = !plugins?.Any(x => x is OpenGLGraphicsPlugin) ?? true;
            if (needsGraphicsSubsystem)
            {
                plugins = plugins ?? new List<UltravioletPlugin>();
                plugins.Add(new OpenGLGraphicsPlugin());
            }

            var needsAudioSubsystem = !(plugins?.Any(x => x is BASSAudioPlugin || x is FMODAudioPlugin) ?? false);
            if (needsAudioSubsystem)
            {
                plugins = plugins ?? new List<UltravioletPlugin>();
                plugins.Add(new BASSAudioPlugin());
            }

            foreach (var plugin in plugins)
                configuration.Plugins.Add(plugin);

            configurer?.Invoke(configuration);

            base.OnConfiguring(configuration);
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithAudioImplementation(AudioImplementation audioImplementation)
        {
            switch (audioImplementation)
            {
                case AudioImplementation.BASS:
                    return WithPlugin(new BASSAudioPlugin());
                case AudioImplementation.FMOD:
                    return WithPlugin(new FMODAudioPlugin());
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioImplementation));
            }
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithConfiguration(Action<UltravioletConfiguration> configurer)
        {
            this.configurer = configurer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithPlugin(UltravioletPlugin plugin)
        {
            if (plugin == null)
                return this;

            if (this.plugins == null)
                this.plugins = new List<UltravioletPlugin>();

            this.plugins.Add(plugin);
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithInitialization(Action<UltravioletContext> initializer)
        {
            if (this.initializer != null)
                throw new InvalidOperationException("Initialization has already been configured.");

            this.initializer = initializer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithContent(Action<ContentManager> loader)
        {
            if (this.content != null)
                throw new InvalidOperationException("Content loading has already been configured.");

            this.loader = loader;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter WithDispose(Action disposer)
        {
            if (this.disposer != null)
                throw new InvalidOperationException("Disposal has already been configured.");

            this.disposer = disposer;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter SkipFrames(Int32 frameCount)
        {
            Contract.EnsureRange(frameCount >= 0, nameof(frameCount));

            framesToSkip = frameCount;
            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter OnFrameStart(Int32 frame, Action<IUltravioletTestApplicationAdapter> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.FrameStart, frame, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter OnUpdate(Action<IUltravioletTestApplicationAdapter, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Update, -1, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter OnUpdate(Int32 update, Action<IUltravioletTestApplicationAdapter, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Update, update, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter OnRender(Action<IUltravioletTestApplicationAdapter, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Render, -1, action));

            return this;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter OnRender(Int32 render, Action<IUltravioletTestApplicationAdapter, UltravioletTime> action)
        {
            if (frameActions == null)
                frameActions = new List<FrameAction>();

            frameActions.Add(new FrameAction(FrameActionType.Render, render, action));

            return this;
        }

        /// <inheritdoc/>
        public UltravioletImage Render(Action<UltravioletContext> renderer)
        {
            if (this.host.Headless)
                throw new InvalidOperationException("Cannot render a headless window.");

            this.shouldExit = () => true;

            this.renderer = renderer;
            this.host.Run();

            return image;
        }

        /// <inheritdoc/>
        public void RunUntil(Func<Boolean> predicate)
        {
            this.shouldExit = predicate;
            this.host.Run();
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
        protected override void OnInitialized()
        {
            if (!this.host.Headless)
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

            if (!this.host.Headless)
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
                content = ContentManager.Create(Path.Combine("Resources", "Content"));
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
                    this.host.Exit();
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
                    Ultraviolet.GetGraphics().SetViewport(new Viewport(0, 0, window.ClientSize.Width, window.ClientSize.Height));
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
                image = ConvertRenderTargetToImage(rtarget);
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
                        ((Action<IUltravioletTestApplicationAdapter>)action.Action)(this);
                        break;

                    default:
                        ((Action<IUltravioletTestApplicationAdapter, UltravioletTime>)action.Action)(this, time);
                        break;
                }
            }
        }

        /// <summary>
        /// Converts the specified render target to an Ultraviolet image.
        /// </summary>
        /// <param name="rt">The render target to convert.</param>
        /// <returns>The converted image.</returns>
        private UltravioletImage ConvertRenderTargetToImage(RenderTarget2D rt)
        {
            // HACK: Our buffer has been rounded up to the nearest
            // power of two, so at this point we clip it back down
            // to the size of the window.

            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var windowWidth = window.DrawableSize.Width;
            var windowHeight = window.DrawableSize.Height;

            var data = new Color[rt.Width * rt.Height];
            rt.GetData(data);

            var image = new UltravioletImage(windowWidth, windowHeight);
            var pixel = 0;
            for (int y = 0; y < rt.Height; y++)
            {
                for (int x = 0; x < rt.Width; x++)
                {
                    if (x < windowWidth && y < windowHeight)
                    {
                        var rawColor = data[pixel];

                        image.SetPixel(x, y, rawColor.R, rawColor.G, rawColor.B, 255);
                    }
                    pixel++;
                }
            }

            return image;
        }

        // State values.
        private Func<Boolean> shouldExit;
        private ContentManager content = null;
        private Action<UltravioletConfiguration> configurer;
        private Action<UltravioletContext> initializer;
        private Action<ContentManager> loader;
        private Action<UltravioletContext> renderer;
        private Action disposer;
        private UltravioletImage image = null;
        private Int32 updateCount = 0;
        private Int32 renderCount = 0;
        private Int32 frameCount = 0;
        private Int32 framesToSkip = 0;
        private DateTime startTime = default;
        private List<FrameAction> frameActions;
        private List<UltravioletPlugin> plugins;

        // The render target to which the test scene will be rendered.
        private RenderTarget2D rtarget = null;
        private RenderBuffer2D rtargetColorBuffer = null;
        private RenderBuffer2D rtargetDepthStencilBuffer = null;

        private readonly UltravioletTestApplication host;
    }
}
