using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS set trigger action.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SetTriggerAction)]
    public sealed class UvssSetTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssSetTriggerActionSyntax()
            : this(null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssSetTriggerActionSyntax(
            SyntaxToken setKeyword,
            UvssPropertyNameSyntax propertyName,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.SetTriggerAction)
        {
            this.SetKeyword = setKeyword;
            ChangeParent(setKeyword);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.Selector = selector;
            ChangeParent(selector);

            this.Value = value;
            ChangeParent(value);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSetTriggerActionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSetTriggerActionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.SetKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.SetKeyword);

            this.PropertyName = reader.ReadSyntaxNode<UvssPropertyNameSyntax>(version);
            ChangeParent(this.PropertyName);

            this.Selector = reader.ReadSyntaxNode<UvssSelectorWithParenthesesSyntax>(version);
            ChangeParent(this.Selector);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.Value);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(SetKeyword, version);
            writer.Write(PropertyName, version);
            writer.Write(Selector, version);
            writer.Write(Value, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SetKeyword;
                case 1: return PropertyName;
                case 2: return Selector;
                case 3: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the action's "set" keyword.
        /// </summary>
        public SyntaxToken SetKeyword { get; internal set; }

        /// <summary>
        /// Gets the action's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the action's optional selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector { get; internal set; }

        /// <summary>
        /// Gets the action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSetTriggerAction(this);
        }
    }
}
