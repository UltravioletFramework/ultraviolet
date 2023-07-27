using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the base class for effect implementation processors.
    /// </summary>
    /// <typeparam name="TInput">The type of input being processed.</typeparam>
    public abstract class EffectImplementationProcessor<TInput> : ContentProcessor<TInput, EffectImplementation>
    {
        /// <summary>
        /// Gets or sets the collection of externs which are provided to the effect upon compilation.
        /// </summary>
        public Dictionary<String, String> Externs { get; set; }

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

            return (GL.IsGLES ? esShader : null) ?? glShader;
        }
    }
}
