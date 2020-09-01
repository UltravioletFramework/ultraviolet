using System;
using System.Collections.Generic;

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
        /// <param name="name">The node's name.</param>
        /// <param name="mesh">The node's associated mesh.</param>
        /// <param name="children">The node's list of child nodes.</param>
        /// <param name="transform">The node's transform matrix.</param>
        public ModelNode(String name, ModelMesh mesh, IList<ModelNode> children, Matrix transform)
        {
            this.Name = name;
            this.Mesh = mesh;
            this.Children = new ModelNodeCollection(children);
            this.Transform = transform;
        }

        /// <summary>
        /// Gets the node's name.
        /// </summary>
        public String Name { get; }

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
    }
}