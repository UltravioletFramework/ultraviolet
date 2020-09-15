using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a <see cref="Model"/> with additional data to support skinned animation.
    /// </summary>
    public class SkinnedModel : Model
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="scenes">The model's list of scenes.</param>
        /// <param name="textures">The model's list of textures.</param>
        public SkinnedModel(UltravioletContext uv, IList<ModelScene> scenes, IList<Texture2D> textures = null)
            : base(uv, scenes, textures)
        {

        }
    }
}
