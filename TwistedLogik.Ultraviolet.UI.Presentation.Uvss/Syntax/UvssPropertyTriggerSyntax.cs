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
        internal UvssPropertyTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SyntaxNode evaluations,
            SyntaxToken openCurlyBrace,
            SyntaxNode actions,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.PropertyTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            this.PropertyKeyword = propertyKeyword;
            this.Evaluations = evaluations;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Actions = actions;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 6;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return PropertyKeyword;
                case 2: return Evaluations;
                case 3: return OpenCurlyBrace;
                case 4: return Actions;
                case 5: return CloseCurlyBrace;
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
        public SyntaxNode Evaluations;

        /// <summary>
        /// The opening curly brace which introduces the trigger's action list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The trigger's action list.
        /// </summary>
        public SyntaxNode Actions;

        /// <summary>
        /// The closing curly brace which terminates the trigger's action list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
