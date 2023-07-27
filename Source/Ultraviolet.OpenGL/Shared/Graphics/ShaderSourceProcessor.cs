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
            ShaderStage stage = ShaderStage.Unknown;
            if (metadata?.Extension?.Contains(".frag") ?? false)
            {
                stage = ShaderStage.Fragment;
            }
            else if (metadata?.Extension?.Contains(".vert") ?? false)
            {
                stage = ShaderStage.Vertex;
            }
            return ShaderSource.ProcessRawSource(manager, metadata, input, stage);
        }
    }
}
