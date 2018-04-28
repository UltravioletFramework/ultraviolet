using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the indexing operator of a UVSS navigation expression.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.NavigationExpressionIndexer)]
    public sealed class UvssNavigationExpressionIndexerSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionIndexerSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionIndexerSyntax()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionIndexerSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionIndexerSyntax(
            SyntaxToken openBracketToken,
            SyntaxToken numberToken,
            SyntaxToken closeBracketToken)
            : base(SyntaxKind.NavigationExpressionIndexer)
        {
            this.OpenBracketToken = openBracketToken;
            ChangeParent(openBracketToken);

            this.NumberToken = numberToken;
            ChangeParent(numberToken);

            this.CloseBracketToken = closeBracketToken;
            ChangeParent(closeBracketToken);

            this.SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionIndexerSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssNavigationExpressionIndexerSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenBracketToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenBracketToken);

            this.NumberToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.NumberToken);

            this.CloseBracketToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseBracketToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenBracketToken, version);
            writer.Write(NumberToken, version);
            writer.Write(CloseBracketToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenBracketToken;
                case 1: return NumberToken;
                case 2: return CloseBracketToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the index operator's open bracket token.
        /// </summary>
        public SyntaxToken OpenBracketToken { get; internal set; }

        /// <summary>
        /// Gets the index operator's close bracket token.
        /// </summary>
        public SyntaxToken NumberToken { get; internal set; }

        /// <summary>
        /// Gets the index operator's close bracket token.
        /// </summary>
        public SyntaxToken CloseBracketToken { get; internal set; }

        /// <summary>
        /// Gets the index value.
        /// </summary>
        /// <returns>The indexer's index value, or null if the 
        /// indexer does not have a valid value.</returns>
        public Int32? GetValue()
        {
            if (NumberToken == null)
                return null;

            var value = 0;
            if (Int32.TryParse(NumberToken.Text, out value))
                return value;

            return null;
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitNavigationExpressionIndex(this);
        }
    }
}
