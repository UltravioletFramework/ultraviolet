using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS event trigger.
    /// </summary>
    public class UvssEventTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerSyntax"/> class.
        /// </summary>
        internal UvssEventTriggerSyntax()
            : base(SyntaxKind.EventTrigger)
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
