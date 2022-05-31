using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Manages node access for a <see cref="SkinnedModelInstance"/>.
    /// </summary>
    internal class SkinnedModelInstanceNodeManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelInstanceNodeManager"/> class.
        /// </summary>
        /// <param name="instance">The <see cref="SkinnedModelInstance"/> that owns this manager.</param>
        public SkinnedModelInstanceNodeManager(SkinnedModelInstance instance)
        {
            Contract.Require(instance, nameof(instance));

            this.nodes = new SkinnedModelNodeInstance[instance.Template.TotalNodeCount];
            instance.TraverseNodes((node, state) =>
            {
                var array = ((SkinnedModelNodeInstance[])state);
                array[node.Template.LogicalIndex] = node;
            }, this.nodes);
        }

        /// <summary>
        /// Gets the node instance with the specified logical index.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the node instance to retrieve.</param>
        /// <returns>The <see cref="SkinnedModelNodeInstance"/> with the specified logical index.</returns>
        public SkinnedModelNodeInstance GetNodeInstanceByLogicalIndex(Int32 logicalIndex) => nodes[logicalIndex];

        // Array that associates node instances with logical indexes.
        private readonly SkinnedModelNodeInstance[] nodes;
    }
}
