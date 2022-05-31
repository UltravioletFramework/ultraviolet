using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a logically related collection of meshes within a model.
    /// </summary>
    public class ModelScene : IModelSceneProvider<ModelNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelScene"/> class.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the scene within its parent model.</param>
        /// <param name="name">The scene's name.</param>
        /// <param name="nodes">The scene's list of nodes.</param>
        public ModelScene(Int32 logicalIndex, String name, IList<ModelNode> nodes = null)
        {
            this.LogicalIndex = logicalIndex;
            this.Name = name;
            this.Nodes = new ModelNodeCollection(nodes);
            this.TotalNodeCount = this.Nodes.Count + nodes?.Sum(x => x.TotalNodeCount) ?? 0;

            foreach (var node in Nodes)
                node.SetParentModelScene(this);
        }

        /// <inheritdoc/>
        ModelNode IModelSceneProvider<ModelNode>.GetChildNode(Int32 index) => Nodes[index];

        /// <summary>
        /// Performs an action on all nodes in the scene.
        /// </summary>
        /// <param name="action">The action to perform on each node.</param>
        /// <param name="state">An arbitrary state object to pass to <paramref name="action"/>.</param>
        public void TraverseNodes(Action<ModelNode, Object> action, Object state)
        {
            Contract.Require(action, nameof(action));

            foreach (var node in Nodes)
                node.TraverseNodes(action, state);
        }

        /// <summary>
        /// Gets the logical index of the scene within its parent model.
        /// </summary>
        public Int32 LogicalIndex { get; }

        /// <inheritdoc/>
        ModelScene IModelSceneProvider<ModelNode>.ModelScene => this;

        /// <summary>
        /// Gets the scene's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the <see cref="Model"/> that contains this scene.
        /// </summary>
        public Model ParentModel { get; private set; }

        /// <summary>
        /// Gets the scene's collection of nodes.
        /// </summary>
        public ModelNodeCollection Nodes { get; }

        /// <inheritdoc/>
        public Int32 ChildNodeCount => Nodes.Count;

        /// <inheritdoc/>
        public Int32 TotalNodeCount { get; }

        /// <summary>
        /// Sets the scene's parent model.
        /// </summary>
        /// <param name="parent">The scene's parent model.</param>
        internal void SetParentModel(Model parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModel != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModel = parent;

            foreach (var node in Nodes)
                node.SetParentModel(parent);
        }
    }
}