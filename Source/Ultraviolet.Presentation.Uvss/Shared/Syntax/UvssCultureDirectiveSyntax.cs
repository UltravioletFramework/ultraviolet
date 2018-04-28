using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS $culture directive.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.CultureDirective)]
    public sealed class UvssCultureDirectiveSyntax : UvssDirectiveSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssCultureDirectiveSyntax"/> class.
        /// </summary>
        internal UvssCultureDirectiveSyntax()
            : this(null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssCultureDirectiveSyntax"/> class.
        /// </summary>
        internal UvssCultureDirectiveSyntax(
            SyntaxToken directiveToken,
            UvssPropertyValueWithBracesSyntax cultureValue)
            : base(SyntaxKind.CultureDirective)
        {
            this.DirectiveToken = directiveToken;
            ChangeParent(directiveToken);

            this.CultureValue = cultureValue;
            ChangeParent(cultureValue);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssCultureDirectiveSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssCultureDirectiveSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.DirectiveToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.DirectiveToken);

            this.CultureValue = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.CultureValue);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(DirectiveToken, version);
            writer.Write(CultureValue, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return DirectiveToken;
                case 1: return CultureValue;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the token that contains the $culture directive.
        /// </summary>
        public SyntaxToken DirectiveToken { get; internal set; }

        /// <summary>
        /// Gets the value that represents the name of the document's culture.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax CultureValue { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitCultureDirective(this);
        }
    }
}
