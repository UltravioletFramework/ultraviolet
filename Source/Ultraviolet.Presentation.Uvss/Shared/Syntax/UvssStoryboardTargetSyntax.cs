using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS storyboard target.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.StoryboardTarget)]
    public sealed class UvssStoryboardTargetSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class.
        /// </summary>
        internal UvssStoryboardTargetSyntax()
            : this(null, default(SeparatedSyntaxList<UvssIdentifierBaseSyntax>), null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class.
        /// </summary>
        internal UvssStoryboardTargetSyntax(
            SyntaxToken targetKeyword,
            SeparatedSyntaxList<UvssIdentifierBaseSyntax> filters,
            UvssSelectorWithParenthesesSyntax selector,
            UvssBlockSyntax body)
            : base(SyntaxKind.StoryboardTarget)
        {
            this.TargetKeyword = targetKeyword;
            ChangeParent(targetKeyword);

            this.Filters = filters;
            ChangeParent(filters.Node);

            this.Selector = selector;
            ChangeParent(selector);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssStoryboardTargetSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.TargetKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TargetKeyword);

            this.Filters = reader.ReadSeparatedSyntaxList<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.Filters.Node);

            this.Selector = reader.ReadSyntaxNode<UvssSelectorWithParenthesesSyntax>(version);
            ChangeParent(this.Selector);

            this.Body = reader.ReadSyntaxNode<UvssBlockSyntax>(version);
            ChangeParent(this.Body);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(TargetKeyword, version);
            writer.Write(Filters, version);
            writer.Write(Selector, version);
            writer.Write(Body, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TargetKeyword;
                case 1: return Filters.Node;
                case 2: return Selector;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the target's "target" keyword.
        /// </summary>
        public SyntaxToken TargetKeyword { get; internal set; }

        /// <summary>
        /// Gets the target's optional type name.
        /// </summary>
        public SeparatedSyntaxList<UvssIdentifierBaseSyntax> Filters { get; internal set; }
        
        /// <summary>
        /// Gets the target's selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector { get; internal set; }

        /// <summary>
        /// Gets the target's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitStoryboardTarget(this);
        }
    }
}
