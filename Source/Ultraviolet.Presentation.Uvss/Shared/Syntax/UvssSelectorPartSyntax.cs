using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector part.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorPart)]
    public sealed class UvssSelectorPartSyntax : UvssSelectorPartBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartSyntax()
            : this(null, null, default(SyntaxList<UvssSelectorPartClassSyntax>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssSelectorPartSyntax(
            UvssSelectorPartTypeSyntax selectedType,
            UvssSelectorPartNameSyntax selectedName,
            SyntaxList<UvssSelectorPartClassSyntax> selectedClasses,
            UvssPseudoClassSyntax pseudoClass)
            : base(SyntaxKind.SelectorPart)
        {
            this.SelectedType = selectedType;
            ChangeParent(selectedType);

            this.SelectedName = selectedName;
            ChangeParent(selectedName);

            this.SelectedClasses = selectedClasses;
            ChangeParent(selectedClasses.Node);

            this.PseudoClass = pseudoClass;
            ChangeParent(pseudoClass);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPartSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorPartSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.SelectedType = reader.ReadSyntaxNode<UvssSelectorPartTypeSyntax>(version);
            ChangeParent(this.SelectedType);

            this.SelectedName = reader.ReadSyntaxNode<UvssSelectorPartNameSyntax>(version);
            ChangeParent(this.SelectedName);

            this.SelectedClasses = reader.ReadSyntaxNode<UvssSelectorPartClassSyntax>(version);
            ChangeParent(this.SelectedClasses.Node);

            this.PseudoClass = reader.ReadSyntaxNode<UvssPseudoClassSyntax>(version);
            ChangeParent(this.PseudoClass);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(SelectedType, version);
            writer.Write(SelectedName, version);
            writer.Write(SelectedClasses, version);
            writer.Write(PseudoClass, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return SelectedType;
                case 1: return SelectedName;
                case 2: return SelectedClasses.Node;
                case 3: return PseudoClass;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the selector's selected type, if any.
        /// </summary>
        public UvssSelectorPartTypeSyntax SelectedType { get; internal set; }

        /// <summary>
        /// Gets the selector's selected name, if any.
        /// </summary>
        public UvssSelectorPartNameSyntax SelectedName { get; internal set; }

        /// <summary>
        /// Gets the selector's selected classes, if any.
        /// </summary>
        public SyntaxList<UvssSelectorPartClassSyntax> SelectedClasses { get; internal set; }
            
        /// <summary>
        /// Gets the selector's pseudo-class.
        /// </summary>
        public UvssPseudoClassSyntax PseudoClass { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorPart(this);
        }
    }
}
