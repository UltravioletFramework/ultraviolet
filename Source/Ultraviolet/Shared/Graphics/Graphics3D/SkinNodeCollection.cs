using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of nodes associated with a particular <see cref="Skin"/> instance.
    /// </summary>
    public class SkinNodeCollection : IEnumerable<ModelNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinNodeCollection"/> class.
        /// </summary>
        /// <param name="nodes">The nodes to add to the collection.</param>
        public SkinNodeCollection(IEnumerable<ModelNode> nodes)
        {
            this.nodes = nodes?.ToArray() ?? new ModelNode[0];
        }

        /// <summary>
        /// Gets the node at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the node to retrieve.</param>
        /// <returns>The node at the specified index within the collection.</returns>
        public ModelNode this[Int32 index] => nodes[index];

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> for the collection.
        /// </summary>
        /// <returns>An <see cref="ArrayEnumerator{T}"/> which will enumerate through the collection.</returns>
        ArrayEnumerator<ModelNode> GetEnumerator() => new ArrayEnumerator<ModelNode>(nodes);

        /// <inheritdoc/>
        IEnumerator<ModelNode> IEnumerable<ModelNode>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of animations in the collection.
        /// </summary>
        public Int32 Count => nodes.Length;

        // Animation collections.
        private readonly ModelNode[] nodes;
    }
}
