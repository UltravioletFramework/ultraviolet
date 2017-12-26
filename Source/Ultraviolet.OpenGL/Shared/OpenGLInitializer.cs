using System;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.SDL2.Native;

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
            SDL.ClearError();
        }

        /// <inheritdoc/>
        public IntPtr GetProcAddress(String name)
        {
            return SDL.GL_GetProcAddress(name);
        }
    }
}
