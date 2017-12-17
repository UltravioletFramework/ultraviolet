﻿using System;
using System.Linq;
using System.Collections.Generic;
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
        /// Gets a dictionary which associates uniforms with their preferred sampler indices.
        /// </summary>
        public IDictionary<String, Int32> PreferredSamplerIndices { get; } = new Dictionary<String, Int32>();
    }
}
