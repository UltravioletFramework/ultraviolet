namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    partial class SyntaxList
    {
        /// <summary>
        /// Represents a syntax list with many children.
        /// </summary>
        internal sealed class WithManyChildren : WithManyChildrenBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithManyChildren"/> class.
            /// </summary>
            /// <param name="children">An array containing the list's children.</param>
            internal WithManyChildren(ArrayElement<SyntaxNode>[] children)
                : base(children)
            {

            }
        }
    }
}
