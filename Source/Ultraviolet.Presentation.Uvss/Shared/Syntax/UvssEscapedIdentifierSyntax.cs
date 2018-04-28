using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS identifier which has been escaped using square brackets.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.EscapedIdentifier)]
    public sealed class UvssEscapedIdentifierSyntax : UvssIdentifierBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssEscapedIdentifierSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssEscapedIdentifierSyntax(
            SyntaxToken openBracketToken,
            SyntaxToken identifierToken,
            SyntaxToken closeBracketToken)
            : base(SyntaxKind.EscapedIdentifier)
        {
            this.OpenBracketToken = openBracketToken;
            ChangeParent(openBracketToken);

            this.IdentifierToken = identifierToken;
            ChangeParent(identifierToken);

            this.CloseBracketToken = closeBracketToken;
            ChangeParent(closeBracketToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEmptyStatementSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssEscapedIdentifierSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenBracketToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenBracketToken);

            this.IdentifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.IdentifierToken);

            this.CloseBracketToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseBracketToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenBracketToken, version);
            writer.Write(IdentifierToken, version);
            writer.Write(CloseBracketToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenBracketToken;
                case 1: return IdentifierToken;
                case 2: return CloseBracketToken;
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
        /// Gets the open bracket which introduces the escaped keyword.
        /// </summary>
        public SyntaxToken OpenBracketToken { get; internal set; }
        
        /// <summary>
        /// Gets the identifier which this node represents.
        /// </summary>
        public SyntaxToken IdentifierToken { get; internal set; }

        /// <summary>
        /// Gets the close bracket which terminates the escaped keyword.
        /// </summary>
        public SyntaxToken CloseBracketToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEscapedIdentifier(this);
        }
    }
}
