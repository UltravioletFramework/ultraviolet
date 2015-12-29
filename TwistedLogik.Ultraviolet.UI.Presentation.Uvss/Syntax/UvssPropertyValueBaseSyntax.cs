namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for property values.
    /// </summary>
    public abstract class UvssPropertyValueBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssPropertyValueBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
    }
}
