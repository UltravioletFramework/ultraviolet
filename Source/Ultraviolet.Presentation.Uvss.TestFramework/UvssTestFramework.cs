using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Presentation.Uvss.TestFramework
{
    /// <summary>
    /// Represents the test framework for UVSS unit tests.
    /// </summary>
    public abstract class UvssTestFramework : CoreTestFramework
    {
        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="node">The syntax node to wrap.</param>
        /// <returns>The wrapped object.</returns>
        protected SyntaxNodeResult<TNode> TheResultingNode<TNode>(TNode node)
            where TNode : SyntaxNode
        {
            return new SyntaxNodeResult<TNode>(node);
        }
    }
}
