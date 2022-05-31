using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a model comprised of one or more scenes, each of which represents one or more logical objects in 3D space.
    /// </summary>
    public class Model : UltravioletResource
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
            this.TotalNodeCount = this.Scenes.Sum(x => x.TotalNodeCount);

            foreach (var scene in Scenes)
                scene.SetParentModel(this);
        }

        /// <summary>
        /// Performs an action on all nodes in the model.
        /// </summary>
        /// <param name="action">The action to perform on each node.</param>
        /// <param name="state">An arbitrary state object to pass to <paramref name="state"/>.</param>
        public void TraverseNodes(Action<ModelNode, Object> action, Object state)
        {
            Contract.Require(action, nameof(action));

            foreach (var scene in Scenes)
                scene.TraverseNodes(action, state);
        }

        /// <summary>
        /// Gets the model's collection of scenes.
        /// </summary>
        public ModelSceneCollection Scenes { get; }

        /// <summary>
        /// Gets the model's collection of textures.
        /// </summary>
        public ModelTextureCollection Textures { get; }

        /// <summary>
        /// Gets the total number of nodes in this model.
        /// </summary>
        public Int32 TotalNodeCount { get; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            Textures.Dispose();
            base.Dispose(disposing);
        }
    }
}