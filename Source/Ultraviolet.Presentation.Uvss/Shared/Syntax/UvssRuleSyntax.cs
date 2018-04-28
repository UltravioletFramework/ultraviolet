using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS rule.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Rule)]
    public sealed class UvssRuleSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSyntax"/> class.
        /// </summary>
        internal UvssRuleSyntax()
            : this(null, null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSyntax"/> class.
        /// </summary>
        internal UvssRuleSyntax(
            UvssPropertyNameSyntax propertyName,
            SyntaxToken colonToken,
            UvssPropertyValueSyntax value,
            SyntaxToken qualifierToken,
            SyntaxToken semiColonToken)
            : base(SyntaxKind.Rule)
        {
            this.PropertyName = propertyName;
            ChangeParent(PropertyName);

            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.Value = value;
            ChangeParent(value);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.SemiColonToken = semiColonToken;
            ChangeParent(semiColonToken);

            SlotCount = 5;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        private UvssRuleSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PropertyName = reader.ReadSyntaxNode<UvssPropertyNameSyntax>(version);
            ChangeParent(this.PropertyName);

            this.ColonToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ColonToken);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueSyntax>(version);
            ChangeParent(this.Value);

            this.QualifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.QualifierToken);

            this.SemiColonToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.SemiColonToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PropertyName, version);
            writer.Write(ColonToken, version);
            writer.Write(Value, version);
            writer.Write(QualifierToken, version);
            writer.Write(SemiColonToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PropertyName;
                case 1: return ColonToken;
                case 2: return Value;
                case 3: return QualifierToken;
                case 4: return SemiColonToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the name of the property being styled.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the colon that separates the property name from its value.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// Gets the styled property value.
        /// </summary>
        public UvssPropertyValueSyntax Value { get; internal set; }

        /// <summary>
        /// Gets the rule's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the rule's terminating semi-colon token.
        /// </summary>
        public SyntaxToken SemiColonToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitRule(this);
        }
    }
}
