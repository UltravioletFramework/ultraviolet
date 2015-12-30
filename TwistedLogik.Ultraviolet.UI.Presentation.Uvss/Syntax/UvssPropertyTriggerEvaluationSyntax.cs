using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger evaluation.
    /// </summary>
    public sealed class UvssPropertyTriggerEvaluationSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerEvaluationSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerEvaluationSyntax(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken comparisonOperatorToken,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.PropertyTriggerEvaluation)
        {
            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.ComparisonOperatorToken = comparisonOperatorToken;
            ChangeParent(comparisonOperatorToken);

            this.Value = value;
            ChangeParent(value);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PropertyName;
                case 1: return ComparisonOperatorToken;
                case 2: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The name of the property being evaluated.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// The comparison operator being applied to the property.
        /// </summary>
        public SyntaxToken ComparisonOperatorToken { get; internal set; }

        /// <summary>
        /// The value being compared against the property.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }
    }
}
