﻿using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using static Ultraviolet.SDL2.Native.SDL_GLattr;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="IUltravioletWindowInfo"/> interface when
    /// using the OpenGL rendering API.
    /// </summary>
    public sealed class SDL2UltravioletWindowInfoOpenGL : SDL2UltravioletWindowInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2UltravioletWindowInfoOpenGL"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uvconfig">The Ultraviolet configuration settings for the current context.</param>
        /// <param name="winconfig">The window configuration settings for the current context.</param>
        public SDL2UltravioletWindowInfoOpenGL(UltravioletContext uv, UltravioletConfiguration uvconfig, SDL2PlatformConfiguration winconfig)
            : base(uv, uvconfig, winconfig)
        {

        }

        /// <summary>
        /// Draws the current window.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        public void Draw(UltravioletTime time)
        {
            var oglwin = (SDL2UltravioletWindow)GetCurrent();
            oglwin.Draw(time);
        }

        /// <summary>
        /// Swaps the back buffer and the front buffer.
        /// </summary>
        public void Swap()
        {
            var oglwin = (SDL2UltravioletWindow)GetCurrent();
            SDL_GL_SwapWindow((IntPtr)oglwin);
        }
        
        /// <summary>
        /// Makes the specified window the current window.
        /// </summary>
        /// <param name="window">The window to make current.</param>
        /// <param name="context">The OpenGL context.</param>
        public void DesignateCurrent(IUltravioletWindow window, IntPtr context)
        {
            if (Current != window)
            {
                OnCurrentWindowChanging();

                UpdateIsCurrentWindow(GetCurrent(), false);
                Current = window;
                UpdateIsCurrentWindow(GetCurrent(), true);

                if (window != null && (window != glwin || window.SynchronizeWithVerticalRetrace != VSync))
                    BindOpenGLContextToWindow(window, context);

                OnCurrentWindowChanged();
            }

            if (Windows.Count == 0 || (window == null && context == IntPtr.Zero))
                BindOpenGLContextToWindow(null, context);
        }

        /// <inheritdoc/>
        protected override void InitializeRenderingAPI(SDL2PlatformConfiguration sdlconfig)
        {
            if (SDL_GL_SetAttribute(SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, sdlconfig.SrgbBuffersEnabled ? 1 : 0) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLEBUFFERS, sdlconfig.MultiSampleBuffers) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLESAMPLES, sdlconfig.MultiSampleSamples) < 0)
                throw new SDL2Exception();
        }

        /// <inheritdoc/>
        protected override Boolean InitializeRenderingAPIFallback(SDL2PlatformConfiguration sdlconfig, Int32 attempt)
        {
            InitializeRenderingAPI(sdlconfig);

            switch (attempt)
            {
                case 0:
                    // Attempt #0: Try turning off sRGB.  
                    if (sdlconfig.SrgbBuffersEnabled)
                    {
                        if (SDL_GL_SetAttribute(SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, 0) < 0)
                            throw new SDL2Exception();

                        return true;
                    }
                    else goto case 1;

                case 1:
                    // Attempt #1: Try turning off multisampling.
                    if (sdlconfig.MultiSampleBuffers > 0 || sdlconfig.MultiSampleSamples > 0)
                    {
                        if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLEBUFFERS, 0) < 0)
                            throw new SDL2Exception();

                        if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLESAMPLES, 0) < 0)
                            throw new SDL2Exception();

                        return true;
                    }
                    else goto case 2;

                case 2:
                    // Attempt #2: Try turning off sRGB and multisampling.
                    if (sdlconfig.SrgbBuffersEnabled)
                    {
                        if (SDL_GL_SetAttribute(SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, 0) < 0)
                            throw new SDL2Exception();

                        if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLEBUFFERS, 0) < 0)
                            throw new SDL2Exception();

                        if (SDL_GL_SetAttribute(SDL_GL_MULTISAMPLESAMPLES, 0) < 0)
                            throw new SDL2Exception();

                        return true;
                    }
                    else goto default;

                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        protected override void OnWindowCleanup(IUltravioletWindow window)
        {
            if (window == glwin && glcontext != IntPtr.Zero)
                BindOpenGLContextToWindow(null, glcontext);

            base.OnWindowCleanup(window);
        }

        /// <summary>
        /// Updates the value of the <see cref="SDL2UltravioletWindow.IsCurrentWindow"/> property for the specified window.
        /// </summary>
        private void UpdateIsCurrentWindow(IUltravioletWindow window, Boolean value)
        {
            var win = window as SDL2UltravioletWindow;
            if (win == null)
                return;

            win.IsCurrentWindow = value;
        }

        /// <summary>
        /// Binds the OpenGL context to the specified window.
        /// </summary>
        private void BindOpenGLContextToWindow(IUltravioletWindow window, IntPtr context)
        {
            var shuttingDown = (window == null && context == IntPtr.Zero);

            if (context == IntPtr.Zero)
            {
                if (glcontext == IntPtr.Zero)
                {
                    return;
                }
                context = glcontext;
            }

            var win = (SDL2UltravioletWindow)(window ?? GetMaster());
            var winptr = (IntPtr)win;
            if (SDL_GL_MakeCurrent(winptr, context) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetSwapInterval(win.SynchronizeWithVerticalRetrace ? 1 : 0) < 0 && Ultraviolet.Platform != UltravioletPlatform.iOS)
            {
                if (!shuttingDown)
                    throw new SDL2Exception();
            }

            glwin = win;
            glcontext = context;

            VSync = win.SynchronizeWithVerticalRetrace;
            
            // If the window was created without the Hidden flag, then we can show it
            var flags = win.CreationFlags;
            if (flags.HasFlag(WindowFlags.Hidden) == false)
                SDL_ShowWindow(winptr);
        }

        // OpenGL context state.
        private IUltravioletWindow glwin;
        private IntPtr glcontext;
    }
}
