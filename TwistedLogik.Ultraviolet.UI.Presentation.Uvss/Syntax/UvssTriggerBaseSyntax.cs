using System.Collections.Generic;

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
        
        /// <summary>
        /// Gets the trigger's collection of actions.
        /// </summary>
        public abstract IEnumerable<UvssTriggerActionBaseSyntax> Actions { get; }
    }
}
