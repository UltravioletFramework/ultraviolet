using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector.
    /// </summary>
    public sealed class UvssSelectorSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax()
            : this(default(SyntaxList<SyntaxNode>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax(
            SyntaxList<SyntaxNode> components,
            UvssNavigationExpressionSyntax navigationExpression)
            : base(SyntaxKind.Selector)
        {
            this.Components = components;
            ChangeParent(components.Node);

            this.NavigationExpression = navigationExpression;
            ChangeParent(navigationExpression);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Components.Node;
                case 1: return NavigationExpression;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the selector's list of components.
        /// </summary>
        public SyntaxList<SyntaxNode> Components { get; internal set; }

        /// <summary>
        /// Gets a collection containing the selector's parts.
        /// </summary>
        public IEnumerable<UvssSelectorPartBaseSyntax> Parts
        {
            get
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    var child = Components[i] as UvssSelectorPartBaseSyntax;
                    if (child != null)
                        yield return child;
                }
            }
        }

        /// <summary>
        /// Gets a collection containing the selector's combinators.
        /// </summary>
        public IEnumerable<SyntaxToken> Combinators
        {
            get
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    var child = Components[i] as SyntaxToken;
                    if (child != null)
                    {
                        if (child.Kind == SyntaxKind.SpaceToken ||
                            child.Kind == SyntaxKind.GreaterThanToken ||
                            child.Kind == SyntaxKind.GreaterThanGreaterThanToken ||
                            child.Kind == SyntaxKind.GreaterThanQuestionMarkToken)
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the selector's navigation expression.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelector(this);
        }
    }
}
