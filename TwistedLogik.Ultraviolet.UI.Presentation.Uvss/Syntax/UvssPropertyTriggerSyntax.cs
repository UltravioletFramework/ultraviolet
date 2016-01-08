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
        internal UvssPropertyTriggerSyntax()
            : this(null, null, default(SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax>), null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> conditions,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.PropertyTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            ChangeParent(triggerKeyword);

            this.PropertyKeyword = propertyKeyword;
            ChangeParent(propertyKeyword);

            this.Conditions = conditions;
            ChangeParent(conditions.Node);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 5;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return PropertyKeyword;
                case 2: return Conditions.Node;
                case 3: return QualifierToken;
                case 4: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's "property" keyword.
        /// </summary>
        public SyntaxToken PropertyKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's conditions.
        /// </summary>
        public SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> Conditions { get; internal set; }

        /// <summary>
        /// Gets the trigger's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the trigger's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyTrigger(this);
        }
    }
}
