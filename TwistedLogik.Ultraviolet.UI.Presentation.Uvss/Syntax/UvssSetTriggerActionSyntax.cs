using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS set trigger action.
    /// </summary>
    public class UvssSetTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssSetTriggerActionSyntax()
            : base(SyntaxKind.SetTriggerAction)
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
