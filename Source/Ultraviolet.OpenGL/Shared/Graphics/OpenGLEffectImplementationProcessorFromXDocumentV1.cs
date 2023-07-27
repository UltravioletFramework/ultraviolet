using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets from version 1 XML definition files.
    /// </summary>
    internal sealed class OpenGLEffectImplementationProcessorFromXDocumentV1 : EffectImplementationProcessor<XDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEffectImplementationProcessorFromXDocumentV1"/> class.
        /// </summary>
        /// <param name="parent">The processor's parent instance.</param>
        public OpenGLEffectImplementationProcessorFromXDocumentV1(OpenGLEffectImplementationProcessorFromXDocument parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            writer.Write((Byte)255);
            writer.Write((Byte)255);
            writer.Write((Byte)255);
            writer.Write((Byte)1);

            var techniqueElements = input.Root.Elements("Technique");
            if (!techniqueElements.Any())
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            writer.Write(techniqueElements.Count());

            foreach (var techniqueElement in techniqueElements)
            {
                var techniqueName = (String)techniqueElement.Attribute("Name");
                writer.Write(techniqueName);

                var passElements = techniqueElement.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                writer.Write(passElements.Count());

                foreach (var passElement in passElements)
                {
                    var passName = (String)passElement.Attribute("Name");
                    writer.Write(passName);

                    var vertexShader = (String)passElement.Element("VertexShader");
                    var vertexShaderES = (String)passElement.Element("VertexShaderES");

                    if (String.IsNullOrEmpty(vertexShader))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(vertexShader);
                    writer.Write(vertexShaderES);

                    var fragShader = (String)passElement.Element("VertexShader");
                    var fragShaderES = (String)passElement.Element("VertexShaderES");

                    if (String.IsNullOrEmpty(fragShader))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(fragShader);
                    writer.Write(fragShaderES);
                }
            }
        }

        /// <inheritdoc/>
        public override EffectImplementation ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var techniques = new List<OpenGLEffectTechnique>();
            var techniqueCount = reader.ReadInt32();
            if (techniqueCount == 0)
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            for (int i = 0; i < techniqueCount; i++)
            {
                var techniqueName = reader.ReadString();

                var passes = new List<OpenGLEffectPass>();
                var passCount = reader.ReadInt32();
                if (passCount == 0)
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                for (int j = 0; j < passCount; j++)
                {
                    var passName = reader.ReadString();

                    var vertPathGL = reader.ReadString();
                    var vertPathES = reader.ReadString();
                    var vertPath = GetShaderForCurrentPlatform(vertPathGL, vertPathES);

                    if (String.IsNullOrEmpty(vertPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    vertPath = ResolveDependencyAssetPath(metadata, vertPath);
                    metadata.AddAssetDependency(vertPath);

                    var fragPathGL = reader.ReadString();
                    var fragPathES = reader.ReadString();
                    var fragPath = GetShaderForCurrentPlatform(fragPathGL, fragPathES);

                    if (String.IsNullOrEmpty(fragPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    fragPath = ResolveDependencyAssetPath(metadata, fragPath);
                    metadata.AddAssetDependency(fragPath);

                    var vertShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(vertPath), Externs);
                    var vertShader = new OpenGLVertexShader(manager.Ultraviolet, new[] { vertShaderSource });

                    var fragShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(fragPath), Externs);
                    var fragShader = new OpenGLFragmentShader(manager.Ultraviolet, new[] { fragShaderSource });

                    var programs = new[] { new OpenGLShaderProgram(manager.Ultraviolet, vertShader, fragShader, false) };
                    passes.Add(new OpenGLEffectPass(manager.Ultraviolet, passName, programs));
                }

                techniques.Add(new OpenGLEffectTechnique(manager.Ultraviolet, techniqueName, passes));
            }

            return new OpenGLEffectImplementation(manager.Ultraviolet, techniques);
        }

        /// <inheritdoc/>
        public override EffectImplementation Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var techniques = new List<OpenGLEffectTechnique>();
            var techniqueElements = input.Root.Elements("Technique");
            if (!techniqueElements.Any())
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            foreach (var techniqueElement in techniqueElements)
            {
                var techniqueName = (String)techniqueElement.Attribute("Name");
                var techniquePasses = new List<OpenGLEffectPass>();

                var passElements = techniqueElement.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                foreach (var passElement in passElements)
                {
                    var passName = (String)passElement.Attribute("Name");

                    var vertPath = GetShader(passElement, "VertexShader");
                    if (String.IsNullOrEmpty(vertPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    vertPath = ResolveDependencyAssetPath(metadata, vertPath);
                    metadata.AddAssetDependency(vertPath);

                    var fragPath = GetShader(passElement, "FragmentShader");
                    if (String.IsNullOrEmpty(fragPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    fragPath = ResolveDependencyAssetPath(metadata, fragPath);
                    metadata.AddAssetDependency(fragPath);

                    var vertShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(vertPath), Externs);
                    var vertShader = new OpenGLVertexShader(manager.Ultraviolet, new[] { vertShaderSource });

                    var fragShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(fragPath), Externs);
                    var fragShader = new OpenGLFragmentShader(manager.Ultraviolet, new[] { fragShaderSource });

                    var programs = new[] { new OpenGLShaderProgram(manager.Ultraviolet, vertShader, fragShader, false) };
                    techniquePasses.Add(new OpenGLEffectPass(manager.Ultraviolet, passName, programs));
                }

                techniques.Add(new OpenGLEffectTechnique(manager.Ultraviolet, techniqueName, techniquePasses));
            }

            return new OpenGLEffectImplementation(manager.Ultraviolet, techniques);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

        /// <summary>
        /// Gets the asset path of the specified shader.
        /// </summary>
        /// <param name="element">The XML element that defines the shaders for an effect pass.</param>
        /// <param name="shader">The name of the shader to retrieve.</param>
        /// <returns>The asset path of the specified shader.</returns>
        internal static String GetShader(XElement element, String shader)
        {
            if (GL.IsGLES)
            {
                var vert = (String)element.Element(shader + "ES");
                if (vert == null)
                {
                    vert = (String)element.Element(shader);
                    if (vert == null)
                    {
                        return null;
                    }
                    var extension = Path.GetExtension(vert);
                    return Path.ChangeExtension(Path.GetFileNameWithoutExtension(vert) + "ES", extension);
                }
                return vert;
            }
            return (String)element.Element(shader);
        }

        // State values.
        private readonly OpenGLEffectImplementationProcessorFromXDocument parent;
    }
}
