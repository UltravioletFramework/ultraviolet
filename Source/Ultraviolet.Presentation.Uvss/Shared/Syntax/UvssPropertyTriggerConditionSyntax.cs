using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger condition.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PropertyTriggerCondition)]
    public sealed class UvssPropertyTriggerConditionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerConditionSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerConditionSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerConditionSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerConditionSyntax(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken comparisonOperatorToken,
            UvssPropertyValueWithBracesSyntax propertyValue)
            : base(SyntaxKind.PropertyTriggerCondition)
        {
            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.ComparisonOperatorToken = comparisonOperatorToken;
            ChangeParent(comparisonOperatorToken);

            this.PropertyValue = propertyValue;
            ChangeParent(propertyValue);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerConditionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPropertyTriggerConditionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PropertyName = reader.ReadSyntaxNode<UvssPropertyNameSyntax>(version);
            ChangeParent(this.PropertyName);

            this.ComparisonOperatorToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ComparisonOperatorToken);

            this.PropertyValue = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.PropertyValue);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PropertyName, version);
            writer.Write(ComparisonOperatorToken, version);
            writer.Write(PropertyValue, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PropertyName;
                case 1: return ComparisonOperatorToken;
                case 2: return PropertyValue;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the name of the property being evaluated.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the comparison operator being applied to the property.
        /// </summary>
        public SyntaxToken ComparisonOperatorToken { get; internal set; }

        /// <summary>
        /// Gets the value being compared against the property.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax PropertyValue { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyTriggerCondition(this);
        }
    }
}
