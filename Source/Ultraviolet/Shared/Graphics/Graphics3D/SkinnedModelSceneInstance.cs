using System;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an animated instance of a <see cref="ModelScene"/>. Each instance represents a particular 
    /// skinned animation at a particular point in time.
    /// </summary>
    public class SkinnedModelSceneInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelSceneInstance"/> class.
        /// </summary>
        /// <param name="template">The <see cref="ModelScene"/> which serves as this instance's template.</param>
        public SkinnedModelSceneInstance(ModelScene template)
        {
            Contract.Require(template, nameof(template));

            this.Template = template;
            this.Nodes = new SkinnedModelNodeInstanceCollection(template.Nodes.Select(x => new SkinnedModelNodeInstance(x)));
        }

        /// <summary>
        /// Performs an action on all nodes in the scene.
        /// </summary>
        /// <param name="action">The action to perform on each node.</param>
        /// <param name="state">An arbitrary state object to pass to <paramref name="action"/>.</param>
        public void TraverseNodes(Action<SkinnedModelNodeInstance, Object> action, Object state)
        {
            Contract.Require(action, nameof(action));

            foreach (var node in Nodes)
                node.TraverseNodes(action, state);
        }

        /// <summary>
        /// Gets the <see cref="ModelScene"/> which serves as this instance's template.
        /// </summary>
        public ModelScene Template { get; }

        /// <summary>
        /// Gets the instance's collection of nodes.
        /// </summary>
        public SkinnedModelNodeInstanceCollection Nodes { get; }
    }
}
