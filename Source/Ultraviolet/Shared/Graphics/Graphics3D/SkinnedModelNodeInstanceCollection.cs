using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a <see cref="SkinnedModelSceneInstance"/>'s collection of node instances.
    /// </summary>
    public class SkinnedModelNodeInstanceCollection : ModelResourceCollection<SkinnedModelNodeInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelNodeInstanceCollection"/> class.
        /// </summary>
        /// <param name="nodes">The node instances to add to the collection.</param>
        public SkinnedModelNodeInstanceCollection(IEnumerable<SkinnedModelNodeInstance> nodes)
            : base(nodes)
        { }
    }
}
