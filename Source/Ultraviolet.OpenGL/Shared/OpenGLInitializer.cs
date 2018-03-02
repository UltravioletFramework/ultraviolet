using System;
using Ultraviolet.OpenGL.Bindings;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents an object capable of querying the OpenGL driver for information 
    /// required by Ultraviolet's OpenGL initialization process.
    /// </summary>
    public sealed class OpenGLInitializer : IOpenGLInitializer
    {
        /// <inheritdoc/>
        public void Prepare()
        {

        }

        /// <inheritdoc/>
        public void Cleanup()
        {
            SDL_ClearError();
        }

        /// <inheritdoc/>
        public IntPtr GetProcAddress(String name)
        {
            return SDL_GL_GetProcAddress(name);
        }
    }
}
