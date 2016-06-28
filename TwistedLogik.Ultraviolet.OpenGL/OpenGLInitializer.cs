using System;
using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents an object capable of querying the OpenGL driver for information 
    /// required by the Gluon initialization process.
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
