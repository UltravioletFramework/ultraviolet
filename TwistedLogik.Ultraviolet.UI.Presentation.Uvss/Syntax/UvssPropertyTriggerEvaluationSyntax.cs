using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger evaluation.
    /// </summary>
    public class UvssPropertyTriggerEvaluationSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerEvaluationSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerEvaluationSyntax()
            : base(SyntaxKind.PropertyTriggerEvaluation)
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
