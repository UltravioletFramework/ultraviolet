using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an animation which can be applied to a skinned model.
    /// </summary>
    public class SkinnedAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedAnimation"/> class.
        /// </summary>
        /// <param name="name">The animation's unique name.</param>
        /// <param name="modelNodeAnimations">The collection of <see cref="SkinnedModelNodeAnimation"/> instances which make up this animation.</param>
        public SkinnedAnimation(String name, IEnumerable<SkinnedModelNodeAnimation> modelNodeAnimations = null)
        {
            this.Name = name;
            this.Duration = modelNodeAnimations?.Max(x => x?.Duration ?? 0.0) ?? 0.0;
            this.modelNodeAnimations = modelNodeAnimations?.ToArray();
        }

        /// <summary>
        /// Gets the <see cref="SkinnedModelNodeAnimation"/> for the node with the specified logical index.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the node for which to retrieve an animation.</param>
        /// <returns>The <see cref="SkinnedModelNodeAnimation"/> for the specified node, or <see langword="null"/> if no animation exists for that node.</returns>
        public SkinnedModelNodeAnimation GetNodeAnimation(Int32 logicalIndex)
        {
            if (logicalIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(logicalIndex));

            if (logicalIndex >= modelNodeAnimations.Length)
                return null;

            return modelNodeAnimations[logicalIndex];
        }

        /// <summary>
        /// Gets the <see cref="SkinnedModelNodeAnimation"/> for the specified node.
        /// </summary>
        /// <param name="node">The node for which to retrieve an animation.</param>
        /// <returns>The <see cref="SkinnedModelNodeAnimation"/> for the specified node, or <see langword="null"/> if no animation exists for that node.</returns>
        public SkinnedModelNodeAnimation GetNodeAnimation(ModelNode node)
        {
            Contract.Require(node, nameof(node));

            var logicalIndex = node.LogicalIndex;
            if (logicalIndex >= modelNodeAnimations.Length)
                return null;

            var result = modelNodeAnimations[logicalIndex];
            if (result.Node == node)
                return result;

            return null;
        }

        /// <summary>
        /// Gets the animation's unique name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the duration of the animation in fractional seconds.
        /// </summary>
        public Double Duration { get; }

        // The animation's collection of node animations, organized by logical node index.
        private readonly SkinnedModelNodeAnimation[] modelNodeAnimations;
    }
}
