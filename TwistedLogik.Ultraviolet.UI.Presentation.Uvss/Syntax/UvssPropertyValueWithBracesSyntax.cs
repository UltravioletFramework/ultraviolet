using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value enclosed in curly braces.
    /// </summary>
    public class UvssPropertyValueWithBracesSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax()
            : base(SyntaxKind.PropertyValueWithBraces)
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
