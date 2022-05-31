using System;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents an OpenGL resource.
    /// </summary>
    public interface IOpenGLResource : IDisposable
    {
        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        UInt32 OpenGLName { get; }
    }
}
