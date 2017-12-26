using System.Collections.Generic;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the EffectPassCollection class.
    /// </summary>
    public sealed class OpenGLEffectPassCollection : EffectPassCollection
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectPassCollection class.
        /// </summary>
        /// <param name="passes">The collection of effect passes to add to the collection.</param>
        public OpenGLEffectPassCollection(IEnumerable<OpenGLEffectPass> passes)
        {
            if (passes != null)
            {
                foreach (var pass in passes)
                {
                    AddInternal(pass);
                }
            }
        }
    }
}
