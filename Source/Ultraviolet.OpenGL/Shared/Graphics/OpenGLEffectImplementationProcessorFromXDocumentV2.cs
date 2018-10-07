using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets from version 2 XML definition files.
    /// </summary>
    internal sealed class OpenGLEffectImplementationProcessorFromXDocumentV2 : EffectImplementationProcessor<XDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEffectImplementationProcessorFromXDocumentV2"/> class.
        /// </summary>
        /// <param name="parent">The processor's parent instance.</param>
        public OpenGLEffectImplementationProcessorFromXDocumentV2(OpenGLEffectImplementationProcessorFromXDocument parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            var desc = DeserializeDescription(input);

            writer.Write((Byte)255);
            writer.Write((Byte)255);
            writer.Write((Byte)255);
            writer.Write((Byte)2);

            writer.Write(desc.Parameters.Count());

            foreach (var parameter in desc.Parameters)
                writer.Write(parameter);

            if (!desc.Techniques?.Any() ?? false)
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            writer.Write(desc.Techniques.Count());

            foreach (var technique in desc.Techniques)
            {
                writer.Write(technique.Name);

                if (!technique.Passes?.Any() ?? false)
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                writer.Write(technique.Passes.Count());

                foreach (var pass in technique.Passes)
                {
                    writer.Write(pass.Name);

                    if (String.IsNullOrEmpty(pass.Stages.VertexShader))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(pass.Stages.VertexShader);
                    writer.Write(pass.Stages.VertexShaderES);

                    if (String.IsNullOrEmpty(pass.Stages.FragmentShader))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    writer.Write(pass.Stages.FragmentShader);
                    writer.Write(pass.Stages.FragmentShaderES);
                }
            }
        }

        /// <inheritdoc/>
        public override EffectImplementation ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var parameters = new HashSet<String>();
            var parameterCount = reader.ReadInt32();
            for (int i = 0; i < parameterCount; i++)
                parameters.Add(reader.ReadString());

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

            return new OpenGLEffectImplementation(manager.Ultraviolet, techniques, parameters);
        }

        /// <inheritdoc/>
        public override EffectImplementation Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var desc = DeserializeDescription(input);
            var parameters = new HashSet<String>(desc.Parameters);

            var techniques = new List<OpenGLEffectTechnique>();
            if (!desc.Techniques?.Any() ?? false)
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            foreach (var technique in desc.Techniques)
            {
                var techniqueName = technique.Name;
                var techniquePasses = new List<OpenGLEffectPass>();

                if (!technique.Passes?.Any() ?? false)
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                foreach (var pass in technique.Passes)
                {
                    var passName = pass.Name;

                    var vertPath = GetShaderForCurrentPlatform(pass.Stages.VertexShader, pass.Stages.VertexShaderES);
                    if (String.IsNullOrEmpty(vertPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    vertPath = ResolveDependencyAssetPath(metadata, vertPath);
                    metadata.AddAssetDependency(vertPath);

                    var fragPath = GetShaderForCurrentPlatform(pass.Stages.FragmentShader, pass.Stages.FragmentShaderES);
                    if (String.IsNullOrEmpty(fragPath))
                        throw new ContentLoadException(OpenGLStrings.EffectMustHaveVertexAndFragmentShader);

                    fragPath = ResolveDependencyAssetPath(metadata, fragPath);
                    metadata.AddAssetDependency(fragPath);

                    var vertShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(vertPath), Externs);
                    var vertShader = new OpenGLVertexShader(manager.Ultraviolet, new[] { vertShaderSource });

                    var fragShaderSource = ShaderSource.ProcessExterns(manager.Load<ShaderSource>(fragPath), Externs);
                    var fragShader = new OpenGLFragmentShader(manager.Ultraviolet, new[] { fragShaderSource });

                    foreach (var hint in vertShader.ShaderSourceMetadata.ParameterHints)
                        parameters.Add(hint);

                    foreach (var hint in fragShader.ShaderSourceMetadata.ParameterHints)
                        parameters.Add(hint);

                    var programs = new[] { new OpenGLShaderProgram(manager.Ultraviolet, vertShader, fragShader, false) };
                    techniquePasses.Add(new OpenGLEffectPass(manager.Ultraviolet, passName, programs));
                }

                techniques.Add(new OpenGLEffectTechnique(manager.Ultraviolet, techniqueName, techniquePasses));
            }

            return new OpenGLEffectImplementation(manager.Ultraviolet, techniques, parameters);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

        /// <summary>
        /// Creates a new <see cref="EffectDescription"/> object from the specified XML document.
        /// </summary>
        private static EffectDescription DeserializeDescription(XDocument xml)
        {
            var rootXml = xml.Element("Effect");
            if (rootXml == null)
                throw new ContentLoadException(OpenGLStrings.DocumentDoesNotContainEffect);

            var effect = new EffectDescription();

            var parametersXml = rootXml.Element("Parameters")?.Elements("Parameter");
            var parameters = parametersXml?.Select(x => x.Value).ToList() ?? Enumerable.Empty<String>();

            var techniquesXml = rootXml.Element("Techniques")?.Elements("Technique");
            if (!techniquesXml?.Any() ?? false)
                throw new ContentLoadException(OpenGLStrings.EffectMustHaveTechniques);

            var techniques = new List<EffectTechniqueDescription>();

            foreach (var techniqueXml in techniquesXml)
            {
                var technique = new EffectTechniqueDescription();
                var passes = new List<EffectPassDescription>();

                var passesXml = techniqueXml.Element("Passes")?.Elements("Pass");
                if (!passesXml?.Any() ?? false)
                    throw new ContentLoadException(OpenGLStrings.EffectTechniqueMustHavePasses);

                foreach (var passXml in passesXml)
                {
                    var pass = new EffectPassDescription();
                    pass.Name = passXml.Attribute("Name")?.Value;
                    pass.Stages = new EffectStagesDescription();

                    var stagesXml = passXml.Element("Stages");
                    pass.Stages.VertexShader = stagesXml.Element("Vert")?.Value;
                    pass.Stages.VertexShaderES = stagesXml.Element("VertES")?.Value;
                    pass.Stages.FragmentShader = stagesXml.Element("Frag")?.Value;
                    pass.Stages.FragmentShaderES = stagesXml.Element("FragES")?.Value;

                    passes.Add(pass);
                }

                technique.Name = techniqueXml.Attribute("Name").Value;
                technique.Passes = passes;

                techniques.Add(technique);
            }

            effect.Parameters = parameters;
            effect.Techniques = techniques;

            return effect;
        }

        // State values.
        private readonly OpenGLEffectImplementationProcessorFromXDocument parent;
    }
}
