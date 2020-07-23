using System;
using System.Collections.Generic;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the EffectTechnique class.
    /// </summary>
    public sealed class OpenGLEffectTechnique : EffectTechnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEffectTechnique"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The technique's name.</param>
        /// <param name="passes">The technique's effect passes.</param>
        public OpenGLEffectTechnique(UltravioletContext uv, String name, IEnumerable<OpenGLEffectPass> passes)
            : base(uv)
        {
            this.Name = name ?? String.Empty;
            this.Passes = new OpenGLEffectPassCollection(passes);
        }

        /// <inheritdoc/>
        public override String Name { get; }

        /// <inheritdoc/>
        public override EffectPassCollection Passes { get; }

        /// <summary>
        /// Gets the effect implementation that owns this effect technique.
        /// </summary>
        public OpenGLEffectImplementation EffectImplementation
        {
            get => effectImplementation;
            internal set
            {
                if (effectImplementation != null)
                    throw new InvalidOperationException(OpenGLStrings.EffectTechniqueAlreadyHasImpl);

                effectImplementation = value;
            }
        }

        // Property values.
        private OpenGLEffectImplementation effectImplementation;
    }
}
