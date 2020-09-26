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
        public SkinnedModelSceneInstanceCollection(IEnumerable<SkinnedModelSceneInstance> scenes)
            : base(scenes)
        { }
    }
}
