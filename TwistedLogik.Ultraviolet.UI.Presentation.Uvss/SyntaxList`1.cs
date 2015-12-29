namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes.
    /// </summary>
    public struct SyntaxList<TNode> where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxList{TNode}"/> structure.
        /// </summary>
        /// <param name="node">The node that represents the list.</param>
        public SyntaxList(SyntaxNode node)
        {
            this.node = node;
        }

        // The node that represents the list.
        private readonly SyntaxNode node;
    }
}
