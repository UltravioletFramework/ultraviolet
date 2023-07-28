using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an object which can provide an instance of the <see cref="ModelNode"/> class for rendering.
    /// </summary>
    public interface IModelNodeProvider<TChild>
        where TChild : class, IModelNodeProvider<TChild>
    {
        /// <summary>
        /// Gets one of the node's child nodes.
        /// </summary>
        /// <param name="index">The index of the node to retrieve within the node's list of child nodes.</param>
        /// <returns>The child node with the specified index.</returns>
        TChild GetChildNode(Int32 index);

        /// <summary>
        /// Gets the provided <see cref="ModelNode"/> instance.
        /// </summary>
        ModelNode ModelNode { get; }

        /// <summary>
        /// Gets the number of nodes which are immediate children of this node.
        /// </summary>
        Int32 ChildNodeCount { get; }

        /// <summary>
        /// Gets the total number of nodes which are descendants of this node.
        /// </summary>
        Int32 TotalNodeCount { get; }
    }
}
