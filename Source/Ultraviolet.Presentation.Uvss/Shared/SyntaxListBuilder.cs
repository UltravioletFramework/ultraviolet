using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for building syntax lists.
    /// </summary>
    public class SyntaxListBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxListBuilder"/> class.
        /// </summary>
        /// <param name="size">The size of the list.</param>
        public SyntaxListBuilder(Int32 size)
        {
            this.nodes = new ArrayElement<SyntaxNode>[size];
        }

        /// <summary>
        /// Gets a value indicating whether there are any nodes of the specified kind
        /// in the builder's list.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value specifying the kind
        /// of node for which to seach the list.</param>
        /// <returns>true if there are any nodes of the specified kind in the list; otherwise, false.</returns>
        public Boolean Any(SyntaxKind kind)
        {
            for (int i = 0; i < count; i++)
            {
                if (nodes[i].Value.Kind == kind)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            this.count = 0;
        }
        
        /// <summary>
        /// Removes the last item from the list.
        /// </summary>
        public void RemoveLast()
        {
            if (count == 0)
                throw new InvalidOperationException();

            nodes[count--] = default(ArrayElement<SyntaxNode>);
        }

        /// <summary>
        /// Adds the specified item to the end of the list.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder Add(SyntaxNode item)
        {
            EnsureAdditionalCapacity(1);
            return AddUnsafe(item);
        }

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder AddRange(SyntaxNode list)
        {
            if (list.IsList)
            {
                EnsureAdditionalCapacity(list.SlotCount);

                for (int i = 0; i < list.SlotCount; i++)
                    AddUnsafe(list.GetSlot(i));

                return this;
            }
            else
            {
                return Add(list);
            }
        }

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <param name="offset">The offset within <paramref name="list"/> at which to begin adding items</param>
        /// <param name="length">The number of items to add to the list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder AddRange(SyntaxNode list, Int32 offset, Int32 length)
        {
            if (list.IsList)
            {
                EnsureAdditionalCapacity(length - offset);

                for (int i = offset; i < offset + length; i++)
                    AddUnsafe(list.GetSlot(i));

                return this;
            }
            else
            {
                if (offset > 0)
                    throw new ArgumentOutOfRangeException(nameof(offset));

                if (length != 1)
                    throw new ArgumentOutOfRangeException(nameof(length));

                return Add(list);
            }
        }

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <typeparam name="TNode">The type of node contained by the syntax list being added.</typeparam>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder AddRange<TNode>(SyntaxList<TNode> list) 
            where TNode : SyntaxNode
        {
            return AddRange(list, 0, list.Count);
        }

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <typeparam name="TNode">The type of node contained by the syntax list being added.</typeparam>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <param name="offset">The offset within <paramref name="list"/> at which to begin adding items</param>
        /// <param name="length">The number of items to add to the list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder AddRange<TNode>(SyntaxList<TNode> list, Int32 offset, Int32 length)
            where TNode : SyntaxNode
        {
            EnsureAdditionalCapacity(length - offset);

            for (int i = offset; i < offset + length; i++)
                AddUnsafe(list.ItemUntyped(i));

            return this;
        }

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <typeparam name="TNode">The type of node contained by the collection being added.</typeparam>
        /// <param name="list">A collection containing the items to add to this list.</param>
        /// <returns>A reference to this instance.</returns>
        public SyntaxListBuilder AddRange<TNode>(IEnumerable<TNode> list)
            where TNode : SyntaxNode
        {
            EnsureAdditionalCapacity(list.Count());

            foreach (var item in list)
                AddUnsafe(item);

            return this;
        }

        /// <summary>
        /// Creates a <see cref="SyntaxNode"/> which represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SyntaxNode"/> which represents the builder's list.</returns>
        public SyntaxNode ToListNode()
        {
            switch (count)
            {
                case 0:
                    return null;

                case 1:
                    return nodes[0];

                case 2:
                    return SyntaxList.List(nodes[0], nodes[1]);
            }

            return SyntaxList.List(ToArray());
        }

        /// <summary>
        /// Creates a <see cref="SyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SyntaxList<SyntaxNode> ToList()
        {
            return new SyntaxList<SyntaxNode>(ToListNode());
        }

        /// <summary>
        /// Creates a <see cref="SyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <typeparam name="TDerived">The type of node contained by the created list.</typeparam>
        /// <returns>A <see cref="SyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SyntaxList<TDerived> ToList<TDerived>() where TDerived : SyntaxNode
        {
            return new SyntaxList<TDerived>(ToListNode());
        }

        /// <summary>
        /// Creates an array of <see cref="ArrayElement{T}"/> that represents the builder's list.
        /// </summary>
        /// <returns>An array of <see cref="ArrayElement{T}"/> that represents the builder's list.</returns>
        public ArrayElement<SyntaxNode>[] ToArray()
        {
            var array = new ArrayElement<SyntaxNode>[count];
            for (int i = 0; i < array.Length; i++)
                array[i] = nodes[i];

            return array;
        }

        /// <summary>
        /// Gets or sets the node at the specified index within the list.
        /// </summary>
        /// <param name="index">The index of the node.</param>
        /// <returns>The node at the specified index within the list.</returns>
        public SyntaxNode this[Int32 index]
        {
            get { return nodes[index]; }
            set { nodes[index].Value = value; }
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public Int32 Count => count;

        /// <summary>
        /// Ensures that the list contains enough capacity for the specified number of additional items.
        /// </summary>
        /// <param name="additionalCount">The number of items being added to the list.</param>
        private void EnsureAdditionalCapacity(Int32 additionalCount)
        {
            var currentSize = this.nodes.Length;
            var requiredSize = this.count + additionalCount;
            if (requiredSize <= currentSize)
                return;

            var newSize = requiredSize < 8 ? 8 : 
                requiredSize >= Int32.MaxValue / 2 ? Int32.MaxValue : Math.Max(requiredSize, currentSize * 2);

            Array.Resize(ref this.nodes, newSize);
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        /// <returns>A reference to this instance.</returns>
        private SyntaxListBuilder AddUnsafe(SyntaxNode item)
        {
            Contract.Require(item, nameof(item));

            this.nodes[count].Value = item;
            this.count++;

            return this;
        }

        // The underlying array of list items.
        private ArrayElement<SyntaxNode>[] nodes;
        private Int32 count;
    }
}
