using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a logically related collection of meshes within a model.
    /// </summary>
    public class ModelScene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelScene"/> class.
        /// </summary>
        /// <param name="name">The scene's name.</param>
        /// <param name="nodes">The scene's list of nodes.</param>
        public ModelScene(String name, IList<ModelNode> nodes = null)
        {
            this.Name = name;
            this.Nodes = new ModelNodeCollection(nodes);
        }

        /// <summary>
        /// Gets the scene's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the scene's collection of nodes.
        /// </summary>
        public ModelNodeCollection Nodes { get; }
    }
}