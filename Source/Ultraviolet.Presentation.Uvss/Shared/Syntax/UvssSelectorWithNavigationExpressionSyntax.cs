using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector with a trailing navigation expression.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.SelectorWithNavigationExpression)]
    public sealed class UvssSelectorWithNavigationExpressionSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithNavigationExpressionSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithNavigationExpressionSyntax"/> class.
        /// </summary>
        internal UvssSelectorWithNavigationExpressionSyntax(
            UvssSelectorSyntax selector,
            UvssNavigationExpressionSyntax navigationExpression)
            : base(SyntaxKind.SelectorWithNavigationExpression)
        {
            this.Selector = selector;
            ChangeParent(selector);

            this.NavigationExpression = navigationExpression;
            ChangeParent(navigationExpression);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorWithNavigationExpressionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorWithNavigationExpressionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.Selector = reader.ReadSyntaxNode<UvssSelectorSyntax>(version);
            ChangeParent(this.Selector);

            this.NavigationExpression = reader.ReadSyntaxNode<UvssNavigationExpressionSyntax>(version);
            ChangeParent(this.NavigationExpression);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(Selector, version);
            writer.Write(NavigationExpression, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Selector;
                case 1: return NavigationExpression;
                default:
                    throw new InvalidOperationException();
            }
        }
        
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
        /// Gets the selector's trailing navigation expression.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelectorWithNavigationExpression(this);
        }
    }
}
