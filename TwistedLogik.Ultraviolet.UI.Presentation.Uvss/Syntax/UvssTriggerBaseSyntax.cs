namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS triggers.
    /// </summary>
    public abstract class UvssTriggerBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTriggerBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssTriggerBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
    }
}
