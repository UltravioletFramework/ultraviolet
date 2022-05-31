using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes.
    /// </summary>
    /// <typeparam name="TNode">The type of node contained by this list.</typeparam>
    public struct SyntaxList<TNode> where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxList{TNode}"/> structure.
        /// </summary>
        /// <param name="node">The syntax node that represents the list.</param>
        public SyntaxList(SyntaxNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Implicitly converts a single node to a <see cref="SyntaxList{TNode}"/> instance.
        /// </summary>
        /// <param name="node">The node to convert.</param>
        /// <returns>A <see cref="SyntaxList{TNode}"/> instance that represents the specified node.</returns>
        public static implicit operator SyntaxList<TNode>(TNode node)
        {
            return new SyntaxList<TNode>(node);
        }

        /// <summary>
        /// Implicitly converts a typed list of nodes to an untyped list of nodes.
        /// </summary>
        /// <param name="nodes">The typed list of nodes to convert.</param>
        /// <returns>An untyped <see cref="SyntaxList{TNode}"/> that contains the nodes in the specified type list.</returns>
        public static implicit operator SyntaxList<SyntaxNode>(SyntaxList<TNode> nodes)
        {
            return new SyntaxList<SyntaxNode>(nodes.node);
        }

        /// <summary>
        /// Compares two syntax lists for equality.
        /// </summary>
        /// <param name="left">The list on the left side of the operator.</param>
        /// <param name="right">The list on the right side of the operator.</param>
        /// <returns>true if the lists are equal; otherwise, false.</returns>
        public static Boolean operator ==(SyntaxList<TNode> left, SyntaxList<TNode> right)
        {
            return left.node == right.node;
        }

        /// <summary>
        /// Compares two syntax lists for inequality.
        /// </summary>
        /// <param name="left">The list on the left side of the operator.</param>
        /// <param name="right">The list on the right side of the operator.</param>
        /// <returns>true if the lists are unequal; otherwise, false.</returns>
        public static Boolean operator !=(SyntaxList<TNode> left, SyntaxList<TNode> right)
        {
            return left.node != right.node;
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode() => node?.GetHashCode() ?? 0;

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is SyntaxList<TNode>))
                return false;

            return this.node == ((SyntaxList<TNode>)obj).node;
        }

        /// <summary>
        /// Gets the untyped node at the specified index within the list.
        /// </summary>
        /// <param name="index">The index for which to retrieve a node.</param>
        /// <returns>The untyped node at the specified index within the list.</returns>
        public SyntaxNode ItemUntyped(Int32 index)
        {
            if (node?.IsList ?? false)
            {
                return node.GetSlot(index);
            }

            Contract.EnsureRange(index == 0, nameof(index));
            return node;
        }

        /// <summary>
        /// Gets a value indicating whether there are any nodes in the list.
        /// </summary>
        /// <returns>true if there are any nodes in the list; otherwise, false.</returns>
        public Boolean Any()
        {
            return node != null;
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
                var node = ItemUntyped(i);
                if (node?.Kind == kind)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a <see cref="SeparatedSyntaxList{TNode}"/> that encapsulates this list.
        /// </summary>
        /// <typeparam name="TOther">The type of node contained by the separated list.</typeparam>
        /// <returns>A <see cref="SeparatedSyntaxList{TNode}"/> that enapsulates this list.</returns>
        public SeparatedSyntaxList<TOther> AsSeparatedList<TOther>() where TOther : SyntaxNode
        {
            return new SeparatedSyntaxList<TOther>(new SyntaxList<TOther>(this.node));
        }

        /// <summary>
        /// Gets the node at the specified index within the list.
        /// </summary>
        /// <param name="index">The index for which to retrieve a node.</param>
        /// <returns>The node at the specified index within the list.</returns>
        public TNode this[Int32 index]
        {
            get
            {
                if (node?.IsList ?? false)
                {
                    return node.GetSlot(index) as TNode;
                }

                Contract.EnsureRange(index == 0, nameof(index));
                return node as TNode;
            }
        }

        /// <summary>
        /// Gets the first node in the list.
        /// </summary>
        public TNode First
        {
            get
            {
                if (node?.IsList ?? false)
                {
                    return node.GetSlot(0) as TNode;
                }

                return node as TNode;
            }
        }

        /// <summary>
        /// Gets the last node in the list.
        /// </summary>
        public TNode Last
        {
            get
            {
                if (node?.IsList ?? false)
                {
                    return node.GetSlot(node.SlotCount - 1) as TNode;
                }

                return node as TNode;
            }
        }

        /// <summary>
        /// Gets the syntax node that represents this list.
        /// </summary>
        public SyntaxNode Node => node;

        /// <summary>
        /// Gets the number of nodes in the list.
        /// </summary>
        public Int32 Count
        {
            get
            {
                if (node == null)
                    return 0;

                return node.IsList ? node.SlotCount : 1;
            }
        }

        // The node that represents the list.
        private readonly SyntaxNode node;
    }
}
