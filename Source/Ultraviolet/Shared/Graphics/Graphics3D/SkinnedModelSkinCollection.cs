using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of skins associated with a particular <see cref="SkinnedModel"/> instance.
    /// </summary>
    public class SkinnedModelSkinCollection : IEnumerable<Skin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelSkinCollection"/> class.
        /// </summary>
        /// <param name="skins">The skins to add to the collection.</param>
        public SkinnedModelSkinCollection(IEnumerable<Skin> skins)
        {
            if (skins != null)
            {
                this.skins = skins.ToArray();
                this.skinsByNode = new Skin[1 + skins.SelectMany(x => x.Nodes).Max(x => x.LogicalIndex)];
                this.skinsByName = this.skins.Where(x => !String.IsNullOrEmpty(x.Name)).ToDictionary(x => x.Name);

                foreach (var skin in skins)
                {
                    foreach (var node in skin.Nodes)
                        this.skinsByNode[node.LogicalIndex] = skin;
                }
            }
            else
            {
                this.skins = new Skin[0];
                this.skinsByNode = new Skin[0];
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

        /// <summary>
        /// Gets the skin at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the skin to retrieve.</param>
        /// <returns>The skin at the specified index within the collection.</returns>
        public Skin this[Int32 index] => skins[index];

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> for the collection.
        /// </summary>
        /// <returns>An <see cref="ArrayEnumerator{T}"/> which will enumerate through the collection.</returns>
        ArrayEnumerator<Skin> GetEnumerator() => new ArrayEnumerator<Skin>(skins);

        /// <inheritdoc/>
        IEnumerator<Skin> IEnumerable<Skin>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of skins in the collection.
        /// </summary>
        public Int32 Count => skins.Length;

        // Skin collections.
        private readonly Skin[] skins;
        private readonly Skin[] skinsByNode;
        private readonly Dictionary<String, Skin> skinsByName;
    }
}
