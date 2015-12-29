namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes which are separated by some delimiter.
    /// </summary>
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

        // The list that this separated list encapsulates.
        private readonly SyntaxList<SyntaxNode> list;
    }
}
