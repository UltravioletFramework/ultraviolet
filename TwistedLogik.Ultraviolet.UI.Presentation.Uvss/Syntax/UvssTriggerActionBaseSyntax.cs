namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS trigger actions.
    /// </summary>
    public abstract class UvssTriggerActionBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerConditionSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssTriggerActionBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }
    }
}
