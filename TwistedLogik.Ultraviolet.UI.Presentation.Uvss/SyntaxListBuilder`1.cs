using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for building syntax lists.
    /// </summary>
    /// <typeparam name="TNode">The type of node contained by the list being built.</typeparam>
    public struct SyntaxListBuilder<TNode> where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxListBuilder{TNode}"/> structure
        /// with the specified initial size.
        /// </summary>
        /// <param name="size">The initial size of the list builder.</param>
        public SyntaxListBuilder(Int32 size)
            : this(new SyntaxListBuilder(size))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxListBuilder{TNode}"/> structure
        /// from the specified <see cref="SyntaxListBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SyntaxListBuilder"/> which is encapsulated by this instance.</param>
        public SyntaxListBuilder(SyntaxListBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Implicitly converts a <see cref="SyntaxListBuilder{TNode}"/> instance 
        /// to its underlying <see cref="SyntaxListBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SyntaxListBuilder{TNode}"/> to convert.</param>
        /// <returns>The specified syntax list builder's underlying <see cref="SyntaxListBuilder"/> instance.</returns>
        public static implicit operator SyntaxListBuilder(SyntaxListBuilder<TNode> builder)
        {
            return builder.builder;
        }

        /// <summary>
        /// Implicitly converts a <see cref="SyntaxListBuilder{TNode}"/> instance 
        /// to a <see cref="SyntaxList{TNode}"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SyntaxListBuilder{TNode}"/> to convert.</param>
        /// <returns>The <see cref="SeparatedSyntaxList{TNode}"/> that was built by the specified list builder.</returns>
        public static implicit operator SyntaxList<TNode>(SyntaxListBuilder<TNode> builder)
        {
            return builder.ToList();
        }

        /// <summary>
        /// Creates a new syntax list builder.
        /// </summary>
        /// <returns>The <see cref="SyntaxListBuilder{TNode}"/> instance that was created.</returns>
        public static SyntaxListBuilder<TNode> Create()
        {
            return new SyntaxListBuilder<TNode>(8);
        }

        /// <summary>
        /// Gets a value indicating whether there are any nodes of the specified kind
        /// in the builder's list.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value specifying the kind
        /// of node for which to seach the list.</param>
        /// <returns>true if there are any nodes of the specified kind in the list; otherwise, false.</returns>
        public Boolean Any(SyntaxKind kind) => builder.Any(kind);

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear() => builder.Clear();

        /// <summary>
        /// Removes the last item from the list.
        /// </summary>
        public void RemoveLast() => builder.RemoveLast();

        /// <summary>
        /// Adds the specified item to the end of the list.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void Add(SyntaxNode item) => 
            builder.Add(item);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        public void AddRange(SyntaxNode list) =>
            builder.AddRange(list);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <param name="offset">The offset within <paramref name="list"/> at which to begin adding items</param>
        /// <param name="length">The number of items to add to the list.</param>
        public void AddRange(SyntaxNode list, Int32 offset, Int32 length) =>
            builder.AddRange(list, offset, length);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        public void AddRange(SyntaxList<TNode> list) => 
            builder.AddRange(list);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A syntax list containing the items to add to this list.</param>
        /// <param name="offset">The offset within <paramref name="list"/> at which to begin adding items</param>
        /// <param name="length">The number of items to add to the list.</param>
        public void AddRange(SyntaxList<TNode> list, Int32 offset, Int32 length) =>
            builder.AddRange(list, offset, length);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="list">A collection containing the items to add to this list.</param>
        /// <returns>A reference to this instance.</returns>
        public void AddRange(IEnumerable<TNode> list) =>
            builder.AddRange(list);

        /// <summary>
        /// Creates a <see cref="SyntaxNode"/> which represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SyntaxNode"/> which represents the builder's list.</returns>
        public SyntaxNode ToListNode() => builder.ToListNode();

        /// <summary>
        /// Creates a <see cref="SyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SyntaxList<TNode> ToList() => 
            builder.ToList<TNode>();

        /// <summary>
        /// Creates a <see cref="SyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <typeparam name="TDerived">The type of node contained by the created list.</typeparam>
        /// <returns>A <see cref="SyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SyntaxList<TDerived> ToList<TDerived>() where TDerived : SyntaxNode =>
            builder.ToList<TDerived>();

        /// <summary>
        /// Creates an array of <see cref="ArrayElement{T}"/> that represents the builder's list.
        /// </summary>
        /// <returns>An array of <see cref="ArrayElement{T}"/> that represents the builder's list.</returns>
        public ArrayElement<SyntaxNode>[] ToArray() =>
            builder.ToArray();

        /// <summary>
        /// Gets or sets the node at the specified index within the list.
        /// </summary>
        /// <param name="index">The index of the node.</param>
        /// <returns>The node at the specified index within the list.</returns>
        public TNode this[Int32 index]
        {
            get { return builder[index] as TNode; }
            set { builder[index] = value; }
        }

        /// <summary>
        /// Gets the number of nodes in the list.
        /// </summary>
        public Int32 Count => builder?.Count ?? 0;

        /// <summary>
        /// Gets a value indicating whether this is a null builder.
        /// </summary>
        public Boolean IsNull => builder == null;

        // The underlying syntax list builder used by this instance.
        private readonly SyntaxListBuilder builder;
    }
}
