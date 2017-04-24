using System;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads vertex shader assets.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class OpenGLVertexShaderProcessor : ContentProcessor<String, OpenGLVertexShader>
    {
        /// <inheritdoc/>
        public override OpenGLVertexShader Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            var source = ShaderSource.ProcessRawSource(manager, metadata, input);
            return new OpenGLVertexShader(manager.Ultraviolet, new[] { (String)source });
        }
    }
}
