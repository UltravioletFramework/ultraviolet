using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger.
    /// </summary>
    public sealed class UvssPropertyTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SeparatedSyntaxList<UvssPropertyTriggerEvaluationSyntax> evaluations,
            UvssBlockSyntax body)
            : base(SyntaxKind.PropertyTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            this.PropertyKeyword = propertyKeyword;
            this.Evaluations = evaluations;
            this.Body = body;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return PropertyKeyword;
                case 2: return Evaluations.Node;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword;

        /// <summary>
        /// The trigger's "property" keyword.
        /// </summary>
        public SyntaxToken PropertyKeyword;

        /// <summary>
        /// The trigger's evaluations list.
        /// </summary>
        public SeparatedSyntaxList<UvssPropertyTriggerEvaluationSyntax> Evaluations;

        /// <summary>
        /// The trigger's body.
        /// </summary>
        public UvssBlockSyntax Body;
    }
}
