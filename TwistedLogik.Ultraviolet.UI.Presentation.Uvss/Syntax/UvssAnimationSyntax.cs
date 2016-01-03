using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation.
    /// </summary>
    public sealed class UvssAnimationSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationSyntax"/> class.
        /// </summary>
        internal UvssAnimationSyntax(
            SyntaxToken animationKeyword, 
            UvssPropertyNameSyntax propertyName, 
            UvssNavigationExpressionSyntax navigationExpression, 
            UvssBlockSyntax body)
            : base(SyntaxKind.Animation)
        {
            this.AnimationKeyword = animationKeyword;
            ChangeParent(animationKeyword);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.NavigationExpression = navigationExpression;
            ChangeParent(navigationExpression);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AnimationKeyword;
                case 1: return PropertyName;
                case 2: return NavigationExpression;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the "animation" keyword which begins the declaration.
        /// </summary>
        public SyntaxToken AnimationKeyword { get; internal set; }

        /// <summary>
        /// Gets the name of the animated property.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the navigation expression for the animated property.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression { get; internal set; }
        
        /// <summary>
        /// Gets the animation's body block.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitAnimation(this);
        }
    }
}
