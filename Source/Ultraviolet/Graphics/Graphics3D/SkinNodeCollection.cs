using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of nodes associated with a particular <see cref="Skin"/> instance.
    /// </summary>
    public class SkinNodeCollection : ModelResourceCollection<ModelNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinNodeCollection"/> class.
        /// </summary>
        /// <param name="nodes">The nodes to add to the collection.</param>
        public SkinNodeCollection(IEnumerable<ModelNode> nodes)
            : base(nodes)
        { }
    }
}
