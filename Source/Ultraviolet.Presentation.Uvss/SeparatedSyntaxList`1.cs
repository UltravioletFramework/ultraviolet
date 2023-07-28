using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes which are separated by some delimiter.
    /// </summary>
    /// <typeparam name="TNode">The type of syntax node contained by the list.</typeparam>
    public struct SeparatedSyntaxList<TNode> where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatedSyntaxList{TNode}"/> structure.
        /// </summary>
        /// <param name="list">The list that this separated list encapsulates.</param>
        public SeparatedSyntaxList(SyntaxList<SyntaxNode> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Gets the separator at the specified index within the list.
        /// </summary>
        /// <param name="index">The index of the separator to retrieve.</param>
        /// <returns>A <see cref="SyntaxToken"/> that represents the specified separator.</returns>
        public SyntaxToken GetSeparator(Int32 index) => list[(index << 1) + 1] as SyntaxToken;

        /// <summary>
        /// Gets a value indicating whether there are any nodes in the list.
        /// </summary>
        /// <returns>true if there are any nodes in the list; otherwise, false.</returns>
        public Boolean Any()
        {
            return Count > 0;
        }

        /// <summary>
        /// Gets a value indicating whether there are any nodes of the specified kind in the list.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value specifying the kind
        /// of node for which to seach the list.</param>
        /// <returns>true if there are any nodes of the specified kind in the list; otherwise, false.</returns>
        public Boolean Any(SyntaxKind kind)
        {
            for (var i = 0; i < Count; i++)
            {
                var node = this[i];
                if (node?.Kind == kind)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a <see cref="SyntaxList{TNode}"/> that contains this list's nodes and separators.
        /// </summary>
        /// <returns>The <see cref="SyntaxList{TNode}"/> that was created.</returns>
        public SyntaxList<SyntaxNode> GetWithSeparators() => list;

        /// <summary>
        /// Gets the node at the specified index within the list.
        /// </summary>
        /// <param name="index">The index for which to retrieve a node.</param>
        /// <returns>The node at the specified index within the list.</returns>
        public TNode this[Int32 index] => list[index << 1] as TNode;

        /// <summary>
        /// Gets the syntax node that represents this list.
        /// </summary>
        public SyntaxNode Node => list.Node;

        /// <summary>
        /// Gets the number of nodes in this list.
        /// </summary>
        public Int32 Count => (list.Count + 1) >> 1;

        /// <summary>
        /// Gets the number of separators in this list.
        /// </summary>
        public Int32 SeparatorCount => (list.Count) >> 1;

        // The list that this separated list encapsulates.
        private readonly SyntaxList<SyntaxNode> list;
    }
}
