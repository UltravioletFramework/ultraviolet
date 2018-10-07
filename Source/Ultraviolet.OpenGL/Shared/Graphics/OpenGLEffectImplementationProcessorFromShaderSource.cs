using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets from shader source files.
    /// </summary>
    [ContentProcessor]
    public sealed partial class OpenGLEffectImplementationProcessorFromShaderSource : EffectImplementationProcessor<String>
    {
        /// <inheritdoc/>
        public override EffectImplementation Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            var isFragShader = (metadata.Extension == ".frag");
            var isVertShader = (metadata.Extension == ".vert");

            if (!isFragShader && !isVertShader)
                throw new InvalidDataException(OpenGLStrings.ImplicitEffectsMustLoadFromShaders.Format(metadata.AssetPath));

            var vertShaderFilePath = isFragShader ? Path.ChangeExtension(metadata.AssetPath, "vert") : metadata.AssetPath;
            var fragShaderFilePath = isFragShader ? metadata.AssetPath : Path.ChangeExtension(metadata.AssetPath, "frag");
            metadata.AddAssetDependency(isFragShader ? vertShaderFilePath : fragShaderFilePath);

            var vertShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(vertShaderFilePath), Externs);
            var vertShader = new OpenGLVertexShader(manager.Ultraviolet, new[] { vertShaderSource });

            var fragShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(fragShaderFilePath), Externs);
            var fragShader = new OpenGLFragmentShader(manager.Ultraviolet, new[] { fragShaderSource });

            var parameters = new HashSet<String>();

            foreach (var hint in vertShader.ShaderSourceMetadata.ParameterHints)
                parameters.Add(hint);

            foreach (var hint in fragShader.ShaderSourceMetadata.ParameterHints)
                parameters.Add(hint);

            var program = new OpenGLShaderProgram(manager.Ultraviolet, vertShader, fragShader, false);
            var pass = new OpenGLEffectPass(manager.Ultraviolet, "Default", new[] { program });
            var technique = new OpenGLEffectTechnique(manager.Ultraviolet, "Default", new[] { pass });

            return new OpenGLEffectImplementation(manager.Ultraviolet, new[] { technique }, parameters);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => false;        
    }
}
