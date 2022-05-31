using System;
using Ultraviolet.Content;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader source assets.
    /// </summary>
    [ContentProcessor]
    public sealed class ShaderSourceProcessor : ContentProcessor<String, ShaderSource>
    {
        /// <inheritdoc/>
        public override ShaderSource Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return ShaderSource.ProcessRawSource(manager, metadata, input);
        }
    }
}
