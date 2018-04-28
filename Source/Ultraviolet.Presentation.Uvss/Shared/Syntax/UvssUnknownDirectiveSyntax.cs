using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents an unrecognized UVSS directive.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.UnknownDirective)]
    public sealed class UvssUnknownDirectiveSyntax : UvssDirectiveSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssUnknownDirectiveSyntax"/> class.
        /// </summary>
        internal UvssUnknownDirectiveSyntax()
            : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssUnknownDirectiveSyntax"/> class.
        /// </summary>
        internal UvssUnknownDirectiveSyntax(
            SyntaxToken directiveToken)
            : base(SyntaxKind.UnknownDirective)
        {
            this.DirectiveToken = directiveToken;
            ChangeParent(directiveToken);

            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssUnknownDirectiveSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssUnknownDirectiveSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.DirectiveToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.DirectiveToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(DirectiveToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return DirectiveToken;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the token that contains the $culture directive.
        /// </summary>
        public SyntaxToken DirectiveToken { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitUnknownDirective(this);
        }
    }
}
