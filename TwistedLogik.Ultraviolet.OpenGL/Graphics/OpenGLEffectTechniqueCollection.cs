using System.Collections.Generic;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the EffectTechniqueCollection class.
    /// </summary>
    public sealed class OpenGLEffectTechniqueCollection : EffectTechniqueCollection
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectTechniqueCollection class.
        /// </summary>
        /// <param name="techniques">A collection of effect techniques to add to the collection.</param>
        public OpenGLEffectTechniqueCollection(IEnumerable<OpenGLEffectTechnique> techniques)
        {
            if (techniques != null)
            {
                foreach (var technique in techniques)
                {
                    AddInternal(technique);
                }
            }
        }
    }
}
