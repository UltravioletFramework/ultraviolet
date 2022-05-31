using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector with enclosing parentheses.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorWithParentheses)]
    public sealed class UvssSelectorWithParenthesesSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithParenthesesSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithParenthesesSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithParenthesesSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithParenthesesSyntax(
            SyntaxToken openParenToken,
            UvssSelectorSyntax selector,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.SelectorWithParentheses)
        {
            this.OpenParenToken = openParenToken;
            ChangeParent(openParenToken);

            this.Selector = selector;
            ChangeParent(selector);

            this.CloseParenToken = closeParenToken;
            ChangeParent(closeParenToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithNavigationExpressionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorWithParenthesesSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenParenToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenParenToken);

            this.Selector = reader.ReadSyntaxNode<UvssSelectorSyntax>(version);
            ChangeParent(this.Selector);

            this.CloseParenToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseParenToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenParenToken, version);
            writer.Write(Selector, version);
            writer.Write(CloseParenToken, version);
        }
        
        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return Selector;
                case 2: return CloseParenToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the open parenthesis that introduces the selector.
        /// </summary>
        public SyntaxToken OpenParenToken { get; internal set; }

        /// <summary>
        /// Gets the enclosed selector.
        /// </summary>
        public UvssSelectorSyntax Selector { get; internal set; }

        /// <inheritdoc/>
        public override SyntaxList<SyntaxNode> Components => 
            Selector?.Components ?? default(SyntaxList<SyntaxNode>);

        /// <inheritdoc/>
        public override IEnumerable<UvssSelectorPartSyntax> Parts => Selector?.Parts;

        /// <inheritdoc/>
        public override IEnumerable<SyntaxToken> Combinators => Selector?.Combinators;

        /// <summary>
        /// Gets the close parenthesis that terminates the selector.
        /// </summary>
        public SyntaxToken CloseParenToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorWithParentheses(this);
        }
    }
}
