using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a model comprised of one or more scenes, each of which represents one or more logical objects in 3D space.
    /// </summary>
    public sealed class Model : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="scenes">The model's list of scenes.</param>
        /// <param name="textures">The model's list of textures.</param>
        public Model(UltravioletContext uv, IList<ModelScene> scenes, IList<Texture2D> textures = null)
            : base(uv)
        {
            Contract.Require(scenes, nameof(scenes));

            this.Scenes = new ModelSceneCollection(scenes);
            this.Textures = new ModelTextureCollection(textures);
        }

        /// <summary>
        /// Gets the model's collection of scenes.
        /// </summary>
        public ModelSceneCollection Scenes { get; }

        /// <summary>
        /// Gets the model's collection of textures.
        /// </summary>
        public ModelTextureCollection Textures { get; }

        /// <inheritdoc/>
        protected override void Dispose(System.Boolean disposing)
        {
            Textures.Dispose();
            base.Dispose(disposing);
        }
    }
}