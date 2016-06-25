﻿using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads fragment shader assets.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class FragmentShaderProcessor : ShaderProcessor<OpenGLFragmentShader>
    {
        /// <inheritdoc/>
        public override OpenGLFragmentShader Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            var source = ReplaceIncludes(manager, metadata, input);
            return new OpenGLFragmentShader(manager.Ultraviolet, new[] { source });
        }
    }
}
