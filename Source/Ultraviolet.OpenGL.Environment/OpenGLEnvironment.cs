using System;
using Ultraviolet.Platform;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents a factory method which produces instances of the <see cref="OpenGLEnvironment"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The <see cref="OpenGLEnvironment"/> instance which was created.</returns>
    public delegate OpenGLEnvironment OpenGLEnvironmentFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the interface that OpenGL uses to communicate with the underlying platform environment.
    /// </summary>
    public abstract class OpenGLEnvironment : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEnvironment"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected OpenGLEnvironment(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Gets a pointer to the OpenGL function with the specified name.
        /// </summary>
        /// <param name="functionName">The name of the OpenGL function for which to retrieve a function pointer.</param>
        /// <returns>A pointer which represents the requested OpenGL function.</returns>
        public abstract IntPtr GetProcAddress(String functionName);

        /// <summary>
        /// Attempts to create an OpenGL context using the current set of requested settings.
        /// </summary>
        /// <returns>A pointer to the created context, or <see cref="IntPtr.Zero"/> if context creation failed.</returns>
        public abstract IntPtr CreateOpenGLContext();

        /// <summary>
        /// Deletes the specified OpenGL context.
        /// </summary>
        /// <param name="openGLContext">A pointer to the OpenGL context to delete.</param>
        public abstract void DeleteOpenGLContext(IntPtr openGLContext);

        /// <summary>
        /// Designates the window that is currently associated with the specified OpenGL context.
        /// </summary>
        /// <param name="window">The window to designate as current.</param>
        /// <param name="openGLContext">A pointer to the OpenGL context.</param>
        public abstract void DesignateCurrentWindow(IUltravioletWindow window, IntPtr openGLContext);

        /// <summary>
        /// Draws the OpenGL framebuffer.
        /// </summary>
        /// <param name="time"></param>
        public abstract void DrawFramebuffer(UltravioletTime time);

        /// <summary>
        /// Swaps the OpenGL framebuffers.
        /// </summary>
        public abstract void SwapFramebuffers();

        /// <summary>
        /// Clears any errors that were caused by previous function invocations.
        /// </summary>
        public abstract void ClearErrors();

        /// <summary>
        /// Throws an exception corresponding to the previous platform error, if applicable.
        /// </summary>
        public abstract void ThrowPlatformErrorException();

        /// <summary>
        /// Requests an OpenGL ES profile if <paramref name="isGLES"/> is true; otherwise, requests an OpenGL Core profile.
        /// </summary>
        /// <param name="isGLES">A value indicating whether to request an OpenGL ES profile.</param>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestOpenGLProfile(Boolean isGLES);

        /// <summary>
        /// Requests an OpenGL context which supports at least the specified version of the OpenGL spec.
        /// </summary>
        /// <param name="version">The version of the OpenGL spec that must be supported by the context.</param>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestOpenGLVersion(Version version);

        /// <summary>
        /// Requests a debug context, if <paramref name="debug"/> is set; otherwise, requests a non-debug context.
        /// </summary>
        /// <param name="debug">A value indicating whether to request a debug context.</param>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestDebugContext(Boolean debug);

        /// <summary>
        /// Requests that the depth buffer be set to the specifized size in bits.
        /// </summary>
        /// <param name="depthSize">The number of bits to request in the depth buffer.</param>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestDepthSize(Int32 depthSize);

        /// <summary>
        /// Requests that the stencil buffer be set to the specifized size in bits.
        /// </summary>
        /// <param name="stencilSize">The number of bits to request in the stencil buffer.</param>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestStencilSize(Int32 stencilSize);

        /// <summary>
        /// Requests a 24-bit framebuffer.
        /// </summary>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Request24BitFramebuffer();

        /// <summary>
        /// Requests a 32-bit framebuffer.
        /// </summary>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean Request32BitFramebuffer();

        /// <summary>
        /// Requests a framebuffer with SRGB support.
        /// </summary>
        /// <returns><see langword="true"/> if the request was successful (even if it could not be fulfilled by the platform). Otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RequestSrgbCapableFramebuffer();

        /// <summary>
        /// Sets the OpenGL swap interval.
        /// </summary>
        /// <param name="swapInterval">A value representing the OpenGL swap interval; 0 for immediate updates, 1 for vertical sync, or -1 for adaptive sync.</param>
        /// <returns><see langword="true"/> if the swap interval was set; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean SetSwapInterval(Int32 swapInterval);

        /// <summary>
        /// Gets a value indicating whether the framebuffer is sRGB-capable.
        /// </summary>
        public abstract Boolean IsFramebufferSrgbCapable { get; }
    }
}
