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
        internal UvssAnimationKeyframeSyntax()
            : base(SyntaxKind.AnimationKeyframe)
        {

        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
