using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS play-storyboard trigger action.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PlayStoryboardTriggerAction)]
    public sealed class UvssPlayStoryboardTriggerActionSyntax : UvssTriggerActionBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlayStoryboardTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlayStoryboardTriggerActionSyntax()
            : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlayStoryboardTriggerActionSyntax"/> class.
        /// </summary>
        internal UvssPlayStoryboardTriggerActionSyntax(
            SyntaxToken playStoryboardKeyword,
            UvssSelectorWithParenthesesSyntax selector,
            UvssPropertyValueWithBracesSyntax value)
            : base(SyntaxKind.PlayStoryboardTriggerAction)
        {
            this.PlayStoryboardKeyword = playStoryboardKeyword;
            ChangeParent(playStoryboardKeyword);

            this.Selector = selector;
            ChangeParent(selector);

            this.Value = value;
            ChangeParent(value);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPlayStoryboardTriggerActionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPlayStoryboardTriggerActionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.PlayStoryboardKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PlayStoryboardKeyword);

            this.Selector = reader.ReadSyntaxNode<UvssSelectorWithParenthesesSyntax>(version);
            ChangeParent(this.Selector);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueWithBracesSyntax>(version);
            ChangeParent(this.Value);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(PlayStoryboardKeyword, version);
            writer.Write(Selector, version);
            writer.Write(Value, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return PlayStoryboardKeyword;
                case 1: return Selector;
                case 2: return Value;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the action's "play-storyboard" keyword.
        /// </summary>
        public SyntaxToken PlayStoryboardKeyword { get; internal set; }

        /// <summary>
        /// Gets the action's optional selector.
        /// </summary>
        public UvssSelectorWithParenthesesSyntax Selector { get; internal set; }

        /// <summary>
        /// Gets the action's value.
        /// </summary>
        public UvssPropertyValueWithBracesSyntax Value { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPlayStoryboardTriggerAction(this);
        }
    }
}
