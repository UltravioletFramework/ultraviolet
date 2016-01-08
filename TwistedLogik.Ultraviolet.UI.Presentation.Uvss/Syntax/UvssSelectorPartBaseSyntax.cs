namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base type for UVSS selector parts.
    /// </summary>
    public abstract class UvssSelectorPartBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssSelectorPartBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
    }
}
