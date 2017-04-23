using System;
using Ultraviolet.Core;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader source assets.
    /// </summary>
    [Preserve(AllMembers = true)]
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
