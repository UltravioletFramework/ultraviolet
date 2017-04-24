using System;
using Ultraviolet.OpenGL.Bindings;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the base class for effect implementation processors.
    /// </summary>
    /// <typeparam name="TInput">The type of input being processed.</typeparam>
    public abstract class EffectImplementationProcessor<TInput> : ContentProcessor<TInput, EffectImplementation>
    {
        /// <summary>
        /// Gets the version of the specified shader which is appropriate for the current platform.
        /// </summary>
        /// <param name="glShader">The desktop OpenGL shader.</param>
        /// <param name="esShader">The OpenGL ES shader.</param>
        /// <returns>The appropriate shader for the current platform.</returns>
        protected static String GetShaderForCurrentPlatform(String glShader, String esShader)
        {
            glShader = String.IsNullOrEmpty(glShader) ? null : glShader;
            esShader = String.IsNullOrEmpty(esShader) ? null : esShader;

            return (gl.IsGLES ? esShader : null) ?? glShader;
        }
    }
}
