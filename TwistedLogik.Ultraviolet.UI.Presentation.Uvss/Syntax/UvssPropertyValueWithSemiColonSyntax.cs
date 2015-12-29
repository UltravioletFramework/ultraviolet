using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value terminated by a semi-colon.
    /// </summary>
    public class UvssPropertyValueWithSemiColonSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithSemiColonSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithSemiColonSyntax()
            : base(SyntaxKind.PropertyValueWithSemiColon)
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
