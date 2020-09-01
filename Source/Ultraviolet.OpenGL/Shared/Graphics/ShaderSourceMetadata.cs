using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains metadata associated with a <see cref="ShaderSource"/> instance.
    /// </summary>
    public sealed class ShaderSourceMetadata
    {
        /// <summary>
        /// Adds the contents of the specified <see cref="ShaderSourceMetadata"/> instance to this instance.
        /// </summary>
        /// <param name="ssmd">The <see cref="ShaderSourceMetadata"/> instance to concatenate to this instance.</param>
        public void Concat(ShaderSourceMetadata ssmd)
        {
            Contract.Require(ssmd, nameof(ssmd));

            foreach (var kvp in ssmd.PreferredSamplerIndices)
                AddPreferredSamplerIndex(kvp.Key, kvp.Value);

            foreach (var hint in ssmd.ParameterHints)
                AddParameterHint(hint);

            foreach (var hint in ssmd.CameraHints)
                AddCameraHint(hint.Key, hint.Value);
        }

        /// <summary>
        /// Adds a preferred sampler index to this instance's list.
        /// </summary>
        /// <param name="uniform">The uniform for which to specify a preferred sampler index.</param>
        /// <param name="sampler">The index of the preferred sampler for the specified uniform.</param>
        public void AddPreferredSamplerIndex(String uniform, Int32 sampler)
        {
            PreferredSamplerIndices.Remove(uniform);

            if (PreferredSamplerIndices.Any(x => x.Value == sampler))
                throw new ArgumentException(OpenGLStrings.SamplerDirectiveAlreadyInUse.Format(uniform, sampler));

            PreferredSamplerIndices[uniform] = sampler;
        }

        /// <summary>
        /// Adds a parameter hint to this instance's list.
        /// </summary>
        /// <param name="parameter">The name of the effect parameter that should be created.</param>
        public void AddParameterHint(String parameter)
        {
            ParameterHints.Add(parameter);
        }

        /// <summary>
        /// Adds a camera hint to this instance's list.
        /// </summary>
        /// <param name="parameter">The name of the camera parameter that should be correlated to the specified uniform.</param>
        /// <param name="uniform">The name of the uniform that represents the specified camera parameter.</param>
        public void AddCameraHint(String parameter, String uniform)
        {
            CameraHints[parameter] = uniform;
        }

        /// <summary>
        /// Gets a dictionary which associates uniforms with their preferred sampler indices.
        /// </summary>
        public IDictionary<String, Int32> PreferredSamplerIndices { get; } = new Dictionary<String, Int32>();

        /// <summary>
        /// Gets a dictionary which associates camera parameters with uniform names.
        /// </summary>
        public IDictionary<String, String> CameraHints { get; } = new Dictionary<String, String>();

        /// <summary>
        /// Gets a set which contains the list of specified parameter names for this shader.
        /// </summary>
        public ISet<String> ParameterHints { get; } = new HashSet<String>();
    }
}
