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
        /// <summary>
        /// Gets a pointer to the OpenGL function with the specified name.
        /// All valid OpenGL functions must return a valid pointer on all platforms.
        /// </summary>
        /// <param name="name">The name of the OpenGL function to retrieve.</param>
        /// <returns>A function pointer representing the specified OpenGL function.</returns>
        public IntPtr GetProcAddress(String name)
        {
            return SDL.GL_GetProcAddress(name);
        }
    }
}
