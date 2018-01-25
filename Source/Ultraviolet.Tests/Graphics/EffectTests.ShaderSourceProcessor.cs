using System;
using Ultraviolet.Content;
using Ultraviolet.OpenGL.Graphics;

namespace Ultraviolet.Tests.Graphics
{
    partial class EffectTests
    {
        /// <summary>
        /// Loads the source code for a shader after running it through the source processor.
        /// </summary>
        [ContentProcessor]
        private class ShaderSourceProcessor : ContentProcessor<String, String>
        {
            /// <inheritdoc/>
            public override String Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
            {
                return ShaderSource.ProcessRawSource(manager, metadata, input)?.Source ?? String.Empty;
            }
        }
    }
}
