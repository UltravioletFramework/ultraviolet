using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a description of an effect.
    /// </summary>
    internal sealed class EffectDescription
    {
        /// <summary>
        /// Gets the list of parameters exposed by the effect.
        /// </summary>
        [JsonProperty(Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<String> Parameters { get; set; }

        /// <summary>
        /// Gets the list of techniques exposed by the effect.
        /// </summary>
        [JsonProperty(Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
        public IEnumerable<EffectTechniqueDescription> Techniques { get; set; }
    }

    /// <summary>
    /// Represents a description of an effect technique.
    /// </summary>
    internal sealed class EffectTechniqueDescription
    {
        /// <summary>
        /// Gets the effect technique's name.
        /// </summary>
        [JsonProperty(Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String Name { get; set; }

        /// <summary>
        /// Gets the list of passes exposed by the technique.
        /// </summary>
        [JsonProperty(Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
        public IEnumerable<EffectPassDescription> Passes { get; set; }
    }

    /// <summary>
    /// Represents a description of an effect pass.
    /// </summary>
    internal sealed class EffectPassDescription
    {
        /// <summary>
        /// Gets the effect pass' name.
        /// </summary>
        [JsonProperty(Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String Name { get; set; }

        /// <summary>
        /// Gets the effect stages included in this pass.
        /// </summary>
        [JsonProperty(Required = Required.Always, NullValueHandling = NullValueHandling.Include)]
        public EffectStagesDescription Stages { get; set; }
    }

    /// <summary>
    /// Represents the stages of an effect pass.
    /// </summary>
    internal sealed class EffectStagesDescription
    {
        /// <summary>
        /// Gets the asset path of the vertex shader relative to the effect file.
        /// </summary>
        [JsonProperty(Required = Required.Always, NullValueHandling = NullValueHandling.Include, PropertyName = "vert")]
        public String VertexShader { get; set; }

        /// <summary>
        /// Gets the asset path of the ES vertex shader relative to the effect file.
        /// </summary>
        [JsonProperty(Required = Required.Default, NullValueHandling = NullValueHandling.Ignore, PropertyName = "es_vert")]
        public String VertexShaderES { get; set; }

        /// <summary>
        /// Gets the asset path of the fragment shader relative to the effect file.
        /// </summary>
        [JsonProperty(Required = Required.Always, NullValueHandling = NullValueHandling.Include, PropertyName = "frag")]
        public String FragmentShader { get; set; }

        /// <summary>
        /// Gets the asset path of the ES fragment shader relative to the effect file.
        /// </summary>
        [JsonProperty(Required = Required.Default, NullValueHandling = NullValueHandling.Ignore, PropertyName = "es_frag")]
        public String FragmentShaderES { get; set; }
    }
}
