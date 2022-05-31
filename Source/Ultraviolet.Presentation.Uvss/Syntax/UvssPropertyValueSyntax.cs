using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value terminated by a semi-colon.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PropertyValue)]
    public sealed class UvssPropertyValueSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueSyntax(SyntaxToken contentToken)
            : base(SyntaxKind.PropertyValue)
        {
            this.ContentToken = contentToken;
            ChangeParent(contentToken);

            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPropertyValueSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.ContentToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ContentToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(ContentToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ContentToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the value's content token.
        /// </summary>
        public SyntaxToken ContentToken { get; internal set; }

        /// <inheritdoc/>
        public override String Value
        {
            get { return ContentToken?.Text; }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyValue(this);
        }
    }
}
