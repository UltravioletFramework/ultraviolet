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
        /// <param name="skins">The model's list of skins.</param>
        /// <param name="animations">The models' list of animations.</param>
        public SkinnedModel(UltravioletContext uv, IList<ModelScene> scenes, IList<Texture2D> textures, IEnumerable<Skin> skins, IEnumerable<SkinnedAnimation> animations)
            : base(uv, scenes, textures)
        {
            this.Skins = new SkinnedModelSkinCollection(skins);
            this.Animations = new SkinnedAnimationCollection(animations);
        }

        /// <summary>
        /// Gets the model's collection of skins.
        /// </summary>
        public SkinnedModelSkinCollection Skins { get; }

        /// <summary>
        /// Gets the model's collection of animations.
        /// </summary>
        public SkinnedAnimationCollection Animations { get; }
    }
}
