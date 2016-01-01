using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a syntax visitor which normalizes white space.
    /// </summary>
    internal class SyntaxNormalizer : SyntaxVisitor
    {
        /// <summary>
        /// Normalizes the specified node's white space.
        /// </summary>
        /// <typeparam name="TNode">The type of node to normalize.</typeparam>
        /// <param name="node">The node to normalize.</param>
        /// <returns>The normalized node.</returns>
        public static TNode Normalize<TNode>(TNode node) where TNode : SyntaxNode
        {
            throw new NotImplementedException();
        }
    }
}
