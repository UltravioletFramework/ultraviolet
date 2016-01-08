using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a missing syntax list.
        /// </summary>
        internal sealed class MissingList : SyntaxList
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MissingList"/> class.
            /// </summary>
            internal MissingList()
            {
                IsMissing = true;
            }

            /// <inheritdoc/>
            public override SyntaxNode GetSlot(Int32 index)
            {
                return null;
            }

            /// <inheritdoc/>
            internal override void CopyTo(ArrayElement<SyntaxNode>[] array, Int32 offset)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
