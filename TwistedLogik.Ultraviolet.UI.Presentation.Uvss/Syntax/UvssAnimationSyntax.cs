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
        /// The "animation" keyword which begins the declaration.
        /// </summary>
        public SyntaxToken AnimationKeyword { get; internal set; }

        /// <summary>
        /// The name of the animated property.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// The navigation expression for the animated property.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression { get; internal set; }
        
        /// <summary>
        /// The animation's body block.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }
    }
}
