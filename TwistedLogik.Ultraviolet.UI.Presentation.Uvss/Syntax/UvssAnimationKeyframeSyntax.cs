using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation keyframe.
    /// </summary>
    public class UvssAnimationKeyframeSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationKeyframeSyntax"/> class.
        /// </summary>
        internal UvssAnimationKeyframeSyntax(
            SyntaxToken keyframeKeyword,
            SyntaxToken time,
            SyntaxToken easing,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.AnimationKeyframe)
        {
            this.KeyframeKeyword = keyframeKeyword;
            this.Time = time;
            this.Easing = easing;
            this.Value = value;

            SlotCount = 4;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return KeyframeKeyword;
                case 1: return Time;
                case 2: return Easing;
                case 3: return Value;
                default:
                    return null;
            }
        }

        /// <summary>
        /// The keyframe declaration token.
        /// </summary>
        public SyntaxToken KeyframeKeyword;

        /// <summary>
        /// The keyframe time token.
        /// </summary>
        public SyntaxToken Time;

        /// <summary>
        /// The keyframe easing token.
        /// </summary>
        public SyntaxToken Easing;

        /// <summary>
        /// The keyframe value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value;
    }
}
