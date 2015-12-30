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
            this.PropertyName = propertyName;
            this.NavigationExpression = navigationExpression;
            this.Body = body;

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
        public SyntaxToken AnimationKeyword;

        /// <summary>
        /// The name of the animated property.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName;

        /// <summary>
        /// The navigation expression for the animated property.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression;
        
        /// <summary>
        /// The animation's body block.
        /// </summary>
        public UvssBlockSyntax Body;
    }
}
