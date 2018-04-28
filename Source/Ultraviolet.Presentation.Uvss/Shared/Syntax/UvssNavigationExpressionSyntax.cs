using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS navigation expression.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.NavigationExpression)]
    public sealed class UvssNavigationExpressionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionSyntax()
            : this(null, null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssNavigationExpressionSyntax(
            SyntaxToken pipeToken,
            UvssPropertyNameSyntax propertyName,
            UvssNavigationExpressionIndexerSyntax indexer,
            SyntaxToken asKeyword,
            UvssIdentifierBaseSyntax typeNameIdentifier)
            : base(SyntaxKind.NavigationExpression)
        {
            this.PipeToken = pipeToken;
            ChangeParent(pipeToken);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.Indexer = indexer;
            ChangeParent(indexer);

            this.AsKeyword = asKeyword;
            ChangeParent(asKeyword);

            this.TypeNameIdentifier = typeNameIdentifier;
            ChangeParent(typeNameIdentifier);

            SlotCount = 5;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNavigationExpressionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssNavigationExpressionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PipeToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PipeToken);

            this.PropertyName = reader.ReadSyntaxNode<UvssPropertyNameSyntax>(version);
            ChangeParent(this.PropertyName);

            this.Indexer = reader.ReadSyntaxNode<UvssNavigationExpressionIndexerSyntax>(version);
            ChangeParent(this.Indexer);

            this.AsKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.AsKeyword);

            this.TypeNameIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.TypeNameIdentifier);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PipeToken, version);
            writer.Write(PropertyName, version);
            writer.Write(Indexer, version);
            writer.Write(AsKeyword, version);
            writer.Write(TypeNameIdentifier, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PipeToken;
                case 1: return PropertyName;
                case 2: return Indexer;
                case 3: return AsKeyword;
                case 4: return TypeNameIdentifier;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the navigation expression's pipe token.
        /// </summary>
        public SyntaxToken PipeToken { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's property name.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's optional indexer.
        /// </summary>
        public UvssNavigationExpressionIndexerSyntax Indexer { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's "as" keyword.
        /// </summary>
        public SyntaxToken AsKeyword { get; internal set; }

        /// <summary>
        /// Gets the navigation expression's conversion type name.
        /// </summary>
        public UvssIdentifierBaseSyntax TypeNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitNavigationExpression(this);
        }
    }
}
