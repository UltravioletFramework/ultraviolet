namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a list of syntax nodes.
    /// </summary>
    public abstract class SyntaxList : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxList"/> class.
        /// </summary>
        protected SyntaxList()
            : base(SyntaxKind.List)
        {

        }
    }
}
