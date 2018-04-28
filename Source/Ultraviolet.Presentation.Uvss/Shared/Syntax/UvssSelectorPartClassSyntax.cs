using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the component of a UVSS selector part which specifies a selected class.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorPartClass)]
    public sealed class UvssSelectorPartClassSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartClassSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartClassSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartClassSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartClassSyntax(
            SyntaxToken periodToken,
            UvssIdentifierSyntax selectedClassIdentifier)
            : base(SyntaxKind.SelectorPartClass)
        {
            this.PeriodToken = periodToken;
            ChangeParent(periodToken);

            this.SelectedClassIdentifier = selectedClassIdentifier;
            ChangeParent(selectedClassIdentifier);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartClassSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorPartClassSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PeriodToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PeriodToken);

            this.SelectedClassIdentifier = reader.ReadSyntaxNode<UvssIdentifierSyntax>(version);
            ChangeParent(this.SelectedClassIdentifier);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PeriodToken, version);
            writer.Write(SelectedClassIdentifier, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PeriodToken;
                case 1: return SelectedClassIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector component's leading period token.
        /// </summary>
        public SyntaxToken PeriodToken { get; internal set; }

        /// <summary>
        /// Gets the identifier that specifies the name of the selected class.
        /// </summary>
        public UvssIdentifierSyntax SelectedClassIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPartClass(this);
        }
    }
}
