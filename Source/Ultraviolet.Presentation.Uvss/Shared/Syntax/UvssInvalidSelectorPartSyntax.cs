using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents an invalid UVSS selector part.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.InvalidSelectorPart)]
    public sealed class UvssInvalidSelectorPartSyntax : UvssSelectorPartBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssInvalidSelectorPartSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssInvalidSelectorPartSyntax(
            SyntaxList<SyntaxToken> components)
            : base(SyntaxKind.InvalidSelectorPart)
        {
            this.Components = components;
            ChangeParent(components.Node);

            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssInvalidSelectorPartSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.Components = reader.ReadSyntaxList<SyntaxToken>(version);
            ChangeParent(this.Components.Node);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(Components, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Components.Node;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector part's list of components.
        /// </summary>
        public SyntaxList<SyntaxToken> Components { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitInvalidSelectorPart(this);
        }
    }
}
