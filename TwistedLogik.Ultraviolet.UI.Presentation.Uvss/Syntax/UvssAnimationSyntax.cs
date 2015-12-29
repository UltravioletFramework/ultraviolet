using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation.
    /// </summary>
    public class UvssAnimationSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationSyntax"/> class.
        /// </summary>
        internal UvssAnimationSyntax(
            SyntaxToken animationKeyword, 
            UvssPropertyNameSyntax propertyName, 
            UvssNavigationExpressionSyntax navigationExpression, 
            SyntaxToken openCurlyBrace,
            SyntaxNode content,
            SyntaxToken closeCurlyBrace)
            : base(SyntaxKind.Animation)
        {
            this.AnimationKeyword = animationKeyword;
            this.PropertyName = propertyName;
            this.NavigationExpression = navigationExpression;
            this.OpenCurlyBrace = openCurlyBrace;
            this.Content = content;
            this.CloseCurlyBrace = closeCurlyBrace;

            SlotCount = 6;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AnimationKeyword;
                case 1: return PropertyName;
                case 2: return NavigationExpression;
                case 3: return OpenCurlyBrace;
                case 4: return Content;
                case 5: return CloseCurlyBrace;
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
        /// The opening curly brace that introduces the keyframe list.
        /// </summary>
        public SyntaxToken OpenCurlyBrace;

        /// <summary>
        /// The animation's content.
        /// </summary>
        public SyntaxNode Content;

        /// <summary>
        /// The closing curly brace that terminates the keyframe list.
        /// </summary>
        public SyntaxToken CloseCurlyBrace;
    }
}
