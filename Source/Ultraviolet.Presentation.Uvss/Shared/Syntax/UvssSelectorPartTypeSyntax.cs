using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies the selected type.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorPartType)]
    public sealed class UvssSelectorPartTypeSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartTypeSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartTypeSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartTypeSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartTypeSyntax(
            UvssIdentifierSyntax selectedTypeIdentifier,
            SyntaxToken exclamationMarkToken)
            : base(SyntaxKind.SelectorPartType)
        {
            this.SelectedTypeIdentifier = selectedTypeIdentifier;
            ChangeParent(selectedTypeIdentifier);

            this.ExclamationMarkToken = exclamationMarkToken;
            ChangeParent(exclamationMarkToken);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartTypeSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorPartTypeSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.SelectedTypeIdentifier = reader.ReadSyntaxNode<UvssIdentifierSyntax>(version);
            ChangeParent(this.SelectedTypeIdentifier);

            this.ExclamationMarkToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ExclamationMarkToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(SelectedTypeIdentifier, version);
            writer.Write(ExclamationMarkToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SelectedTypeIdentifier;
                case 1: return ExclamationMarkToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected type.
        /// </summary>
        public UvssIdentifierSyntax SelectedTypeIdentifier { get; internal set; }

        /// <summary>
        /// Gets the optional exclamation mark token which indicates
        /// that this is an exact type.
        /// </summary>
        public SyntaxToken ExclamationMarkToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartType(this);
        }
    }
}
