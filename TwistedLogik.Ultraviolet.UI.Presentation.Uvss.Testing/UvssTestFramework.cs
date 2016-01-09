using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Testing
{
    /// <summary>
    /// Represents the test framework for UVSS unit tests.
    /// </summary>
    public abstract class UvssTestFramework : NucleusTestFramework
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
