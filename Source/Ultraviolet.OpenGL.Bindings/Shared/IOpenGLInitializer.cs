using System;

namespace Ultraviolet.OpenGL.Bindings
{
    /// <summary>
    /// Represents an object capable of querying the OpenGL driver for information 
    /// required by Ultraviolet's OpenGL initialization process.
    /// </summary>
    public interface IOpenGLInitializer
    {
        /// <summary>
        /// Indicates that the initializer should prepare to load OpenGL functions.
        /// </summary>
        void Prepare();

        /// <summary>
        /// Indicates that the initializer is done loading OpenGL functions.
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Gets a pointer to the OpenGL function with the specified name.
        /// All valid OpenGL functions must return a valid pointer on all platforms.
        /// </summary>
        /// <param name="environmentName">The name of the OpenGL function to retrieve.</param>
        /// <returns>A function pointer representing the specified OpenGL function.</returns>
        IntPtr GetProcAddress(String environmentName);
    }
}
