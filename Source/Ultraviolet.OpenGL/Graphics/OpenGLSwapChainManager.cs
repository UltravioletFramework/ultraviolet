using System;
using Ultraviolet.Graphics;
using Ultraviolet.Platform;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents an OpenGL implementation of the <see cref="SwapChainManager"/> class.
    /// </summary>
    public class OpenGLSwapChainManager : SwapChainManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLSwapChainManager"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLSwapChainManager(UltravioletContext uv)
            : base(uv)
        { }

        /// <inheritdoc/>
        public override void DrawAndSwap(UltravioletTime time, 
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawing, 
            Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawn)
        {
            var graphics = (OpenGLUltravioletGraphics)Ultraviolet.GetGraphics();
            var platform = Ultraviolet.GetPlatform();

            var glenv = graphics.OpenGLEnvironment;
            var glcontext = graphics.OpenGLContext;

            foreach (var window in platform.Windows)
            {
                glenv.DesignateCurrentWindow(window, glcontext);

                window.Compositor.BeginFrame();
                window.Compositor.BeginContext(CompositionContext.Scene);

                onWindowDrawing?.Invoke(Ultraviolet, time, window);

                glenv.DrawFramebuffer(time);

                onWindowDrawn?.Invoke(Ultraviolet, time, window);

                window.Compositor.Compose();
                window.Compositor.Present();

                glenv.SwapFramebuffers();
            }

            glenv.DesignateCurrentWindow(null, glcontext);

            graphics.SetRenderTargetToBackBuffer();
            graphics.UpdateFrameCount();
        }
    }
}
