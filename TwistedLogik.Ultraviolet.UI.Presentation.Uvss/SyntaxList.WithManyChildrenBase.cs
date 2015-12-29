using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents the base class for syntax lists with many children.
        /// </summary>
        internal abstract class WithManyChildrenBase : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithManyChildrenBase"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithManyChildrenBase(ArrayElement<SyntaxNode>[] children)
            {
                SlotCount = children.Length;
                this.children = children;
            }
            
            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                return children[index];
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                Array.Copy(children, 0, array, offset, children.Length);
            }

            // List children.
            protected readonly ArrayElement<SyntaxNode>[] children;
        }
    }
}
