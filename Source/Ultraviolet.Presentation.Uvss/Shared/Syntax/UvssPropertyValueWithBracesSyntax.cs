using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value enclosed in curly braces.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PropertyValueWithBraces)]
    public sealed class UvssPropertyValueWithBracesSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax(
            SyntaxToken openCurlyBraceToken,
            SyntaxToken contentToken,
            SyntaxToken closeCurlyBraceToken)
            : base(SyntaxKind.PropertyValueWithBraces)
        {
            this.OpenCurlyBrace = openCurlyBraceToken;
            ChangeParent(openCurlyBraceToken);

            this.ContentToken = contentToken;
            ChangeParent(contentToken);

            this.CloseCurlyBrace = closeCurlyBraceToken;
            ChangeParent(closeCurlyBraceToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPropertyValueWithBracesSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenCurlyBrace = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenCurlyBrace);

            this.ContentToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ContentToken);

            this.CloseCurlyBrace = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseCurlyBrace);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenCurlyBrace, version);
            writer.Write(ContentToken, version);
            writer.Write(CloseCurlyBrace, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBrace;
                case 1: return ContentToken;
                case 2: return CloseCurlyBrace;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the value's opening curly brace.
        /// </summary>
        public SyntaxToken OpenCurlyBrace { get; internal set; }

        /// <summary>
        /// Gets the value's content token.
        /// </summary>
        public SyntaxToken ContentToken { get; internal set; }

        /// <summary>
        /// Gets the value's closing curly brace.
        /// </summary>
        public SyntaxToken CloseCurlyBrace { get; internal set; }

        /// <inheritdoc/>
        public override String Value
        {
            get { return ContentToken?.Text; }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyValueWithBraces(this);
        }
    }
}
