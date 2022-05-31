using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an object which can provide an instance of the <see cref="ModelScene"/> class for rendering.
    /// </summary>
    public interface IModelSceneProvider<TChild>
        where TChild : class, IModelNodeProvider<TChild>
    {
        /// <summary>
        /// Gets one of the scene's child nodes.
        /// </summary>
        /// <param name="index">The index of the node to retrieve within the scene's list of child nodes.</param>
        /// <returns>The child node with the specified index.</returns>
        TChild GetChildNode(Int32 index);

        /// <summary>
        /// Gets the provided <see cref="ModelScene"/> instance.
        /// </summary>
        ModelScene ModelScene { get; }

        /// <summary>
        /// Gets the number of nodes which are immediate children of this scene.
        /// </summary>
        Int32 ChildNodeCount { get; }

        /// <summary>
        /// Gets the total number of nodes which are descendants of this scene.
        /// </summary>
        Int32 TotalNodeCount { get; }
    }
}
