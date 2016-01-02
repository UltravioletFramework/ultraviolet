using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with two children.
        /// </summary>
        internal sealed class WithTwoChildren : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithTwoChildren"/> class.
            /// </summary>
            /// <param name="child0">The list's first child.</param>
            /// <param name="child1">The list's second child.</param>
            internal WithTwoChildren(SyntaxNode child0, SyntaxNode child1)
            {
                SlotCount = 2;

                this.child0 = child0;
                ChangeParent(child0);

                this.child1 = child1;
                ChangeParent(child1);
            }

            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                switch (index)
                {
                    case 0: return child0;
                    case 1: return child1;
                }
                return null;
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                array[offset].Value = child0;
                array[offset + 1].Value = child1;
            }

            // List children.
            private SyntaxNode child0;
            private SyntaxNode child1;
        }
    }
}
