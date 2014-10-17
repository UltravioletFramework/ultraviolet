using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLEffectImplementationProcessor : ContentProcessor<XDocument, EffectImplementation>
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input)
        {
            var techniqueElements = input.Root.Elements("Technique");
            if (!techniqueElements.Any())
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            writer.Write(techniqueElements.Count());
            
            foreach (var techniqueElement in techniqueElements)
            {
                var techniqueName = techniqueElement.AttributeValueString("Name");
                writer.Write(techniqueName);

                var passElements = techniqueElements.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);
                
                writer.Write(passElements.Count());

                foreach (var passElement in passElements)
                {
                    var passName = passElement.AttributeValueString("Name");
                    writer.Write(passName);

                    var vertexShaderAsset = passElement.ElementValueString("VertexShader");
                    if (String.IsNullOrEmpty(vertexShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(vertexShaderAsset);

                    var fragmentShaderAsset = passElement.ElementValueString("FragmentShader");
                    if (String.IsNullOrEmpty(fragmentShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(fragmentShaderAsset);
                }
            }
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
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

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override EffectImplementation Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var techniques = new List<OpenGLEffectTechnique>();
            var techniqueElements = input.Root.Elements("Technique");
            if (!techniqueElements.Any())
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);
            
            foreach (var techniqueElement in techniqueElements)
            {
                var techniqueName = techniqueElement.AttributeValueString("Name");
                var techniquePasses = new List<OpenGLEffectPass>();

                var passElements = techniqueElements.Elements("Pass");
                if (!passElements.Any())
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                foreach (var passElement in passElements)
                {
                    var passName = passElement.AttributeValueString("Name");

                    var vertexShaderAsset = passElement.ElementValueString("VertexShader");
                    if (String.IsNullOrEmpty(vertexShaderAsset))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    var vertexShader = manager.Load<OpenGLVertexShader>(vertexShaderAsset, false);

                    var fragmentShaderAsset = passElement.ElementValueString("FragmentShader");
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

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
