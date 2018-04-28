using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation keyframe.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.AnimationKeyframe)]
    public sealed class UvssAnimationKeyframeSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationKeyframeSyntax"/> class.
        /// </summary>
        internal UvssAnimationKeyframeSyntax()
            : this(null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationKeyframeSyntax"/> class.
        /// </summary>
        internal UvssAnimationKeyframeSyntax(
            SyntaxToken keyframeKeyword,
            SyntaxToken timeToken,
            UvssIdentifierBaseSyntax easingIdentifier,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.AnimationKeyframe)
        {
            this.KeyframeKeyword = keyframeKeyword;
            ChangeParent(keyframeKeyword);

            this.TimeToken = timeToken;
            ChangeParent(timeToken);

            this.EasingIdentifier = easingIdentifier;
            ChangeParent(easingIdentifier);

            this.Value = value;
            ChangeParent(value);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationKeyframeSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssAnimationKeyframeSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.KeyframeKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.KeyframeKeyword);

            this.TimeToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TimeToken);

            this.EasingIdentifier = reader.ReadSyntaxNode<UvssIdentifierBaseSyntax>(version);
            ChangeParent(this.EasingIdentifier);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.Value);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(KeyframeKeyword, version);
            writer.Write(TimeToken, version);
            writer.Write(EasingIdentifier, version);
            writer.Write(Value, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return KeyframeKeyword;
                case 1: return TimeToken;
                case 2: return EasingIdentifier;
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
        /// Gets the keyframe easing identifier.
        /// </summary>
        public UvssIdentifierBaseSyntax EasingIdentifier { get; internal set; }

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
