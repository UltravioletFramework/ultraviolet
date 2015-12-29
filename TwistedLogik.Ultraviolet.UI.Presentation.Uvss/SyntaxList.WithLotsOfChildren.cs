using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with lots of children.
        /// </summary>
        internal sealed class WithLotsOfChildren : WithManyChildrenBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithLotsOfChildren"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithLotsOfChildren(ArrayElement<SyntaxNode>[] children)
                : base(children)
            {

            }
        }
    }
}
