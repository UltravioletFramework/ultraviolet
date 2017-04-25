using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for building separated syntax lists.
    /// </summary>
    /// <typeparam name="TNode">The type of node contained by the list being built.</typeparam>
    public struct SeparatedSyntaxListBuilder<TNode> where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatedSyntaxListBuilder{TNode}"/> structure
        /// with the specified initial size.
        /// </summary>
        /// <param name="size">The initial size of the list builder.</param>
        public SeparatedSyntaxListBuilder(Int32 size)
            : this(new SyntaxListBuilder(size))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatedSyntaxListBuilder{TNode}"/> structure
        /// from the specified <see cref="SyntaxListBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SyntaxListBuilder"/> which is encapsulated by this instance.</param>
        public SeparatedSyntaxListBuilder(SyntaxListBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Implicitly converts a <see cref="SeparatedSyntaxListBuilder{TNode}"/> instance 
        /// to its underlying <see cref="SyntaxListBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SeparatedSyntaxListBuilder{TNode}"/> to convert.</param>
        /// <returns>The specified syntax list builder's underlying <see cref="SyntaxListBuilder"/> instance.</returns>
        public static implicit operator SyntaxListBuilder(SeparatedSyntaxListBuilder<TNode> builder)
        {
            return builder.builder;
        }

        /// <summary>
        /// Implicitly converts a <see cref="SeparatedSyntaxListBuilder{TNode}"/> instance 
        /// to a <see cref="SyntaxList{TNode}"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="SeparatedSyntaxListBuilder{TNode}"/> to convert.</param>
        /// <returns>The <see cref="SeparatedSyntaxList{TNode}"/> that was built by the specified list builder.</returns>
        public static implicit operator SeparatedSyntaxList<TNode>(SeparatedSyntaxListBuilder<TNode> builder)
        {
            return builder.ToList();
        }

        /// <summary>
        /// Creates a new syntax list builder.
        /// </summary>
        /// <returns>The <see cref="SeparatedSyntaxListBuilder{TNode}"/> instance that was created.</returns>
        public static SeparatedSyntaxListBuilder<TNode> Create()
        {
            return new SeparatedSyntaxListBuilder<TNode>(8);
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
        /// Adds the specified separator token to the end of the list.
        /// </summary>
        /// <param name="separatorToken">The separator token to add to the list.</param>
        public void AddSeparator(SyntaxToken separatorToken) =>
            builder.Add(separatorToken);

        /// <summary>
        /// Adds the specified collection of items to the end of the list.
        /// </summary>
        /// <param name="nodes">A separated syntax list containing the items to add to this list.</param>
        /// <param name="count">The number of items to add to this list.</param>
        public void AddRange(SeparatedSyntaxList<TNode> nodes, Int32 count)
        {
            var list = nodes.GetWithSeparators();
            builder.AddRange(list, Count, Math.Min(count * 2, list.Count));
        }

        /// <summary>
        /// Creates a <see cref="SyntaxNode"/> which represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SyntaxNode"/> which represents the builder's list.</returns>
        public SyntaxNode ToListNode() => builder.ToListNode();

        /// <summary>
        /// Creates a <see cref="SeparatedSyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <returns>A <see cref="SeparatedSyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SeparatedSyntaxList<TNode> ToList() =>
            new SeparatedSyntaxList<TNode>(new SyntaxList<SyntaxNode>(builder.ToListNode()));

        /// <summary>
        /// Creates a <see cref="SeparatedSyntaxList{TNode}"/> that represents the builder's list.
        /// </summary>
        /// <typeparam name="TDerived">The type of node contained by the created list.</typeparam>
        /// <returns>A <see cref="SeparatedSyntaxList{TNode}"/> that represents the builder's list.</returns>
        public SeparatedSyntaxList<TDerived> ToList<TDerived>() where TDerived : SyntaxNode =>
            new SeparatedSyntaxList<TDerived>(new SyntaxList<SyntaxNode>(builder.ToListNode()));
        
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
