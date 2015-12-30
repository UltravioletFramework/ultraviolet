using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation keyframe.
    /// </summary>
    public sealed class UvssAnimationKeyframeSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationKeyframeSyntax"/> class.
        /// </summary>
        internal UvssAnimationKeyframeSyntax(
            SyntaxToken keyframeKeyword,
            SyntaxToken timeToken,
            SyntaxToken easingToken,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.AnimationKeyframe)
        {
            this.KeyframeKeyword = keyframeKeyword;
            this.TimeToken = timeToken;
            this.EasingToken = easingToken;
            this.Value = value;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return KeyframeKeyword;
                case 1: return TimeToken;
                case 2: return EasingToken;
                case 3: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The keyframe declaration token.
        /// </summary>
        public SyntaxToken KeyframeKeyword;

        /// <summary>
        /// The keyframe time token.
        /// </summary>
        public SyntaxToken TimeToken;

        /// <summary>
        /// The keyframe easing token.
        /// </summary>
        public SyntaxToken EasingToken;

        /// <summary>
        /// The keyframe value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value;
    }
}
