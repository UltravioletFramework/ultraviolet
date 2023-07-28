using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of <see cref="ModelNode"/> instances.
    /// </summary>
    public sealed class ModelNodeCollection : UltravioletCollection<ModelNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNodeCollection"/> class.
        /// </summary>
        /// <param name="nodes">The initial list of <see cref="ModelNode"/> objects with which to populate this collection.</param>
        public ModelNodeCollection(IList<ModelNode> nodes = null)
            : base(nodes?.Count ?? 0)
        {
            if (nodes != null)
            {
                this.AddRangeInternal(nodes);
            }
        }
    }
}