using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the EffectTechnique class.
    /// </summary>
    public sealed class OpenGLEffectTechnique : EffectTechnique
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectTechnique class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The technique's name.</param>
        /// <param name="passes">The technique's effect passes.</param>
        public OpenGLEffectTechnique(UltravioletContext uv, String name, IEnumerable<OpenGLEffectPass> passes)
            : base(uv)
        {
            this.name = name ?? String.Empty;
            this.passes = new OpenGLEffectPassCollection(passes);
        }
        
        /// <summary>
        /// Gets the effect pass's name.
        /// </summary>
        public override String Name
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return name;
            }
        }

        /// <summary>
        /// Gets the effect technique's collection of passes.
        /// </summary>
        public override EffectPassCollection Passes
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return passes;
            }
        }

        // Property values.
        private readonly String name;
        private readonly EffectPassCollection passes;
    }
}
