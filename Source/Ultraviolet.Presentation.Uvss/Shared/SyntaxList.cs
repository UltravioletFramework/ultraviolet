using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes.
    /// </summary>
    public abstract partial class SyntaxList : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxList"/> class.
        /// </summary>
        protected SyntaxList()
            : base(SyntaxKind.List)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxList"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        protected SyntaxList(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }

        /// <summary>
        /// Creates a missing list.
        /// </summary>
        /// <returns>The list that was created.</returns>
        internal static SyntaxNode Missing()
        {
            return new MissingList();
        }

        /// <summary>
        /// Creates a list with a single child.
        /// </summary>
        /// <param name="child">The list's child.</param>
        /// <returns>The list that was created.</returns>
        internal static SyntaxNode List(SyntaxNode child)
        {
            return child;
        }

        /// <summary>
        /// Creates a list with two children.
        /// </summary>
        /// <param name="child0">The list's first child.</param>
        /// <param name="child1">The list's second child.</param>
        /// <returns>The list that was created.</returns>
        internal static WithTwoChildren List(SyntaxNode child0, SyntaxNode child1)
        {
            return new WithTwoChildren(child0, child1);
        }

        /// <summary>
        /// Creates a list with three children.
        /// </summary>
        /// <param name="child0">The list's first child.</param>
        /// <param name="child1">The list's second child.</param>
        /// <param name="child2">The list's third child.</param>
        /// <returns>The list that was created.</returns>
        internal static WithThreeChildren List(SyntaxNode child0, SyntaxNode child1, SyntaxNode child2)
        {
            return new WithThreeChildren(child0, child1, child2);
        }

        /// <summary>
        /// Creates a list from the specified array.
        /// </summary>
        /// <param name="nodes">The array of nodes from which to create the list.</param>
        /// <returns>The list that was created.</returns>
        internal static SyntaxList List(ArrayElement<SyntaxNode>[] nodes)
        {
            return (nodes.Length < 10) ?
                new WithManyChildren(nodes) :
                (WithManyChildrenBase)new WithLotsOfChildren(nodes);
        }

        /// <summary>
        /// Creates a list from the specified array.
        /// </summary>
        /// <param name="nodes">The array of nodes from which to create the list.</param>
        /// <returns>The list that was created.</returns>
        internal static SyntaxList List(SyntaxNode[] nodes)
        {
            return List(nodes, nodes.Length);
        }

        /// <summary>
        /// Creates a list from the specified array.
        /// </summary>
        /// <param name="nodes">The array of nodes from which to create the list.</param>
        /// <param name="count">The number of nodes in the array to copy into the created list.</param>
        /// <returns>The list that was created.</returns>
        internal static SyntaxList List(SyntaxNode[] nodes, Int32 count)
        {
            var array = new ArrayElement<SyntaxNode>[count];
            for (int i = 0; i < count; i++)
                array[i].Value = nodes[i];

            return List(array);
        }

        /// <summary>
        /// Concatenates two syntax nodes into a single list.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        /// <returns>A node that represents the concatenation of the specified nodes.</returns>
        internal static SyntaxNode Concat(SyntaxNode left, SyntaxNode right)
        {
            if (left == null)
                return right;

            if (right == null)
                return left;

            var temp = default(ArrayElement<SyntaxNode>[]);
            var leftList = left as SyntaxList;
            var rightList = right as SyntaxList;

            if (leftList != null)
            {
                if (rightList != null)
                {
                    temp = new ArrayElement<SyntaxNode>[left.SlotCount + right.SlotCount];
                    leftList.CopyTo(temp, 0);
                    rightList.CopyTo(temp, left.SlotCount);
                    return List(temp);
                }

                temp = new ArrayElement<SyntaxNode>[left.SlotCount + 1];
                leftList.CopyTo(temp, 0);
                temp[left.SlotCount].Value = right;
                return List(temp);
            }

            if (rightList != null)
            {
                temp = new ArrayElement<SyntaxNode>[rightList.SlotCount + 1];
                temp[0].Value = left;
                rightList.CopyTo(temp, 1);
                return List(temp);
            }

            return List(left, right);
        }

        /// <summary>
        /// Copies the list to the specified array.
        /// </summary>
        /// <param name="array">The array to which to copy the list.</param>
        /// <param name="offset">The offset at which to begin copying.</param>
        internal abstract void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset);

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSyntaxNode(this);
        }
    }
}
