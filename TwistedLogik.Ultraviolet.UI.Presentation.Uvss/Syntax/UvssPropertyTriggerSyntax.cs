using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger.
    /// </summary>
    public class UvssPropertyTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerSyntax()
            : base(SyntaxKind.PropertyTrigger)
        {

        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
