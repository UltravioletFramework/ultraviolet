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
            ChangeParent(keyframeKeyword);

            this.TimeToken = timeToken;
            ChangeParent(timeToken);

            this.EasingToken = easingToken;
            ChangeParent(easingToken);

            this.Value = value;
            ChangeParent(value);

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
        /// Gets the keyframe declaration token.
        /// </summary>
        public SyntaxToken KeyframeKeyword { get; internal set; }

        /// <summary>
        /// Gets the keyframe time token.
        /// </summary>
        public SyntaxToken TimeToken { get; internal set; }

        /// <summary>
        /// Gets the keyframe easing token.
        /// </summary>
        public SyntaxToken EasingToken { get; internal set; }

        /// <summary>
        /// The keyframe value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitAnimationKeyframe(this);
        }
    }
}
