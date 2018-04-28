using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies the selected name.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorPartName)]
    public sealed class UvssSelectorPartNameSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartNameSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartNameSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartNameSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartNameSyntax(
            SyntaxToken hashToken,
            UvssIdentifierSyntax selectedNameIdentifier)
            : base(SyntaxKind.SelectorPartName)
        {
            this.HashToken = hashToken;
            ChangeParent(hashToken);

            this.SelectedNameIdentifier = selectedNameIdentifier;
            ChangeParent(selectedNameIdentifier);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartNameSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorPartNameSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.HashToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.HashToken);

            this.SelectedNameIdentifier = reader.ReadSyntaxNode<UvssIdentifierSyntax>(version);
            ChangeParent(this.SelectedNameIdentifier);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(HashToken, version);
            writer.Write(SelectedNameIdentifier, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return HashToken;
                case 1: return SelectedNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector component's leading hash token.
        /// </summary>
        public SyntaxToken HashToken { get; internal set; }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected element.
        /// </summary>
        public UvssIdentifierSyntax SelectedNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartName(this);
        }
    }
}
