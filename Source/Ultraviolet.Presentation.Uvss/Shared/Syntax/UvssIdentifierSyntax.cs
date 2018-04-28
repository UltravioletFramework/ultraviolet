using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS identifier.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Identifier)]
    public sealed class UvssIdentifierSyntax : UvssIdentifierBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssIdentifierSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssIdentifierSyntax(
            SyntaxToken identifierToken)
            : base(SyntaxKind.Identifier)
        {
            this.IdentifierToken = identifierToken;
            ChangeParent(identifierToken);
            
            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIdentifierSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssIdentifierSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.IdentifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.IdentifierToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(IdentifierToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return IdentifierToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override String Text
        {
            get { return IdentifierToken?.Text; }
        }
        
        /// <summary>
        /// Gets the identifier which this node represents.
        /// </summary>
        public SyntaxToken IdentifierToken { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitIdentifier(this);
        }
    }
}
