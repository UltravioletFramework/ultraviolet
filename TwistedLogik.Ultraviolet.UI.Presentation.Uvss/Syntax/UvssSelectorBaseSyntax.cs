namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base type for UVSS selectors.
    /// </summary>
    public abstract class UvssSelectorBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssSelectorBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
    }
}
