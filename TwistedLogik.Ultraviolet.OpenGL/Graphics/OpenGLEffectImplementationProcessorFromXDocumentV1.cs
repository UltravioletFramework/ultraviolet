using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets from version 1 XML definition files.
    /// </summary>
    internal sealed class OpenGLEffectImplementationProcessorFromXDocumentV1 : ContentProcessor<XDocument, EffectImplementation>
    {
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

                var passElements = techniqueElements.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                writer.Write(passElements.Count());

                foreach (var passElement in passElements)
                {
                    var passName = (String)passElement.Attribute("Name");
                    writer.Write(passName);

                    var vertexShaderAsset = GetShader(passElement, "VertexShader");
                    if (String.IsNullOrEmpty(vertexShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(vertexShaderAsset);

                    var fragmentShaderAsset = GetShader(passElement, "FragmentShader");
                    if (String.IsNullOrEmpty(fragmentShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(fragmentShaderAsset);
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

                    var vertexShaderAsset = reader.ReadString();
                    if (String.IsNullOrEmpty(vertexShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    var vertexShader = manager.Load<OpenGLVertexShader>(vertexShaderAsset);

                    var fragmentShaderAsset = reader.ReadString();
                    if (String.IsNullOrEmpty(fragmentShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    var fragmentShader = manager.Load<OpenGLFragmentShader>(fragmentShaderAsset);

                    var programs = new[] { new OpenGLShaderProgram(manager.Ultraviolet, vertexShader, fragmentShader, true) };
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

                var passElements = techniqueElements.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                foreach (var passElement in passElements)
                {
                    var passName = (String)passElement.Attribute("Name");

                    var vertexShaderAsset = GetShader(passElement, "VertexShader");
                    if (String.IsNullOrEmpty(vertexShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    var vertexShader = manager.Load<OpenGLVertexShader>(vertexShaderAsset, false);

                    var fragmentShaderAsset = GetShader(passElement, "FragmentShader");
                    if (String.IsNullOrEmpty(fragmentShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    var fragmentShader = manager.Load<OpenGLFragmentShader>(fragmentShaderAsset, false);

                    var programs = new[] { new OpenGLShaderProgram(manager.Ultraviolet, vertexShader, fragmentShader, true) };
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
            if (gl.IsGLES)
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
    }
}
