using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a node in a model hierarchy.
    /// </summary>
    public class ModelNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the node within its parent model.</param>
        /// <param name="name">The node's name.</param>
        /// <param name="mesh">The node's associated mesh.</param>
        /// <param name="children">The node's list of child nodes.</param>
        /// <param name="transform">The node's transform matrix.</param>
        public ModelNode(Int32 logicalIndex, String name, ModelMesh mesh, IList<ModelNode> children, Matrix transform)
        {
            this.LogicalIndex = logicalIndex;
            this.Name = name;
            this.Mesh = mesh;
            this.Mesh?.SetParentModelNode(this);
            this.Children = new ModelNodeCollection(children);
            this.Transform = transform;

            foreach (var child in Children)
                child.SetParentModelNode(this);
        }

        /// <summary>
        /// Gets the logical index of the node within its parent model.
        /// </summary>
        public Int32 LogicalIndex { get; }

        /// <summary>
        /// Gets the node's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the <see cref="Model"/> that contains this node.
        /// </summary>
        public Model ParentModel { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelScene"/> that contains this node.
        /// </summary>
        public ModelScene ParentModelScene { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelNode"/> that contains this node.
        /// </summary>
        public ModelNode ParentModelNode { get; private set; }

        /// <summary>
        /// Gets the node's associated mesh.
        /// </summary>
        public ModelMesh Mesh { get; }

        /// <summary>
        /// Gets the node's collection of child nodes.
        /// </summary>
        public ModelNodeCollection Children { get; }

        /// <summary>
        /// Gets the node's transform.
        /// </summary>
        public Matrix Transform { get; }

        /// <summary>
        /// Sets the node's parent model.
        /// </summary>
        /// <param name="parent">The scene's parent model.</param>
        internal void SetParentModel(Model parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModel != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModel = parent;

            Mesh?.SetParentModel(parent);

            foreach (var child in Children)
                child.SetParentModel(parent);
        }

        /// <summary>
        /// Sets the node's parent scene.
        /// </summary>
        /// <param name="parent">The node's parent scene.</param>
        internal void SetParentModelScene(ModelScene parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModelScene != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModelScene = parent;

            Mesh?.SetParentModelScene(parent);

            foreach (var child in Children)
                child.SetParentModelScene(parent);
        }

        /// <summary>
        /// Sets the node's parent node.
        /// </summary>
        /// <param name="parent">The node's parent node.</param>
        internal void SetParentModelNode(ModelNode parent)
        {
            Contract.Require(parent, nameof(parent));

            if (this.ParentModelNode != null)
                throw new InvalidOperationException(UltravioletStrings.ModelParentLinkAlreadyExists);

            this.ParentModelNode = parent;
        }
    }
}