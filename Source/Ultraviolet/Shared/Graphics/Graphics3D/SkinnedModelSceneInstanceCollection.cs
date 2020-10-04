using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a <see cref="SkinnedModelInstance"/>'s collection of scene instances.
    /// </summary>
    public class SkinnedModelSceneInstanceCollection : ModelResourceCollection<SkinnedModelSceneInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelSceneInstanceCollection"/> class.
        /// </summary>
        /// <param name="scenes">The scene instances to add to the collection.</param>
        /// <param name="defaultSceneLogicalIndex">The logical index of the model's default scene.</param>
        public SkinnedModelSceneInstanceCollection(IEnumerable<SkinnedModelSceneInstance> scenes, Int32? defaultSceneLogicalIndex = null)
            : base(scenes)
        {
            if (defaultSceneLogicalIndex.HasValue)
                DefaultScene = this[defaultSceneLogicalIndex.Value];
        }

        /// <summary>
        /// Gets the model's default scene.
        /// </summary>
        public SkinnedModelSceneInstance DefaultScene { get; private set; }
    }
}
