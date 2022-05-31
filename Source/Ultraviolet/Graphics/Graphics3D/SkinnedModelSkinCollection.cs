using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of skins associated with a particular <see cref="SkinnedModel"/> instance.
    /// </summary>
    public class SkinnedModelSkinCollection : ModelResourceCollection<Skin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelSkinCollection"/> class.
        /// </summary>
        /// <param name="skins">The skins to add to the collection.</param>
        public SkinnedModelSkinCollection(IEnumerable<Skin> skins)
            : base(skins)
        {
            if (skins != null)
            {
                this.skinsByNode = new Skin[1 + skins.SelectMany(x => x.Nodes).Max(x => x.LogicalIndex)];
                this.skinsByName = skins.Where(x => !String.IsNullOrEmpty(x.Name)).ToDictionary(x => x.Name);

                foreach (var skin in skins)
                {
                    foreach (var node in skin.Nodes)
                        this.skinsByNode[node.LogicalIndex] = skin;
                }
            }
            else
            {
                this.skinsByNode = Array.Empty<Skin>();
                this.skinsByName = new Dictionary<String, Skin>(0);
            }
        }

        /// <summary>
        /// Attempts to retrieve the skin with the specified name.
        /// </summary>
        /// <param name="name">The name of the skin to retrieve.</param>
        /// <returns>The skin with the specified name, or <see langword="null"/> if no such skin exists.</returns>
        public Skin TryGetSkinByName(String name)
        {
            Contract.Require(name, nameof(name));

            skinsByName.TryGetValue(name, out var skin);
            return skin;
        }

        /// <summary>
        /// Attempts to retrieve the skin associated with the node with the specified logical index.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the node for which to retrieve a skin.</param>
        /// <returns>The skin associated with the specified node, or <see langword="null"/> if no such skin exists.</returns>
        public Skin TryGetSkinByNode(Int32 logicalIndex)
        {
            if (logicalIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(logicalIndex));

            if (logicalIndex >= skinsByNode.Length)
                return null;

            return skinsByNode[logicalIndex];
        }

        // Skin collections.
        private readonly Skin[] skinsByNode;
        private readonly Dictionary<String, Skin> skinsByName;
    }
}
