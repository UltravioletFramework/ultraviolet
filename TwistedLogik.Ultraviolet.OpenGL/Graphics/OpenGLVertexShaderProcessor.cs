using System;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads vertex shader assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLVertexShaderProcessor : ContentProcessor<String, OpenGLVertexShader>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override OpenGLVertexShader Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new OpenGLVertexShader(manager.Ultraviolet, new[] { input });
        }
    }
}
