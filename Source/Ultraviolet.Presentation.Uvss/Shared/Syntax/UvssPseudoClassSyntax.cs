using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS pseudo-class.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PseudoClass)]
    public sealed class UvssPseudoClassSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPseudoClassSyntax"/> class.
        /// </summary>
        internal UvssPseudoClassSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPseudoClassSyntax"/> class.
        /// </summary>
        internal UvssPseudoClassSyntax(
            SyntaxToken colonToken,
            UvssIdentifierBaseSyntax classNameIdentifier)
            : base(SyntaxKind.PseudoClass)
        {
            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.ClassNameIdentifier = classNameIdentifier;
            ChangeParent(classNameIdentifier);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPseudoClassSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPseudoClassSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.ColonToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ColonToken);

            this.ClassNameIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.ClassNameIdentifier);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(ColonToken, version);
            writer.Write(ClassNameIdentifier, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ColonToken;
                case 1: return ClassNameIdentifier;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the pseudo-class' leading colon.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// Gets the pseudo-class' name identifier.
        /// </summary>
        public UvssIdentifierBaseSyntax ClassNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPseudoClass(this);
        }
    }
}
