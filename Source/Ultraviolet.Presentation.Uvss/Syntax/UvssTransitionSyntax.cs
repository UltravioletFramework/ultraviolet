using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a visual transition.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Transition)]
    public sealed class UvssTransitionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionSyntax"/> class.
        /// </summary>
        internal UvssTransitionSyntax()
            : this(null, null, null, null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionSyntax"/> class.
        /// </summary>
        internal UvssTransitionSyntax(
            SyntaxToken transitionKeyword,
            UvssTransitionArgumentListSyntax argumentList,
            SyntaxToken colonToken,
            UvssPropertyValueSyntax value,
            SyntaxToken qualifierToken,
            SyntaxToken semiColonToken)
            : base(SyntaxKind.Transition)
        {
            this.TransitionKeyword = transitionKeyword;
            ChangeParent(transitionKeyword);

            this.ArgumentList = argumentList;
            ChangeParent(argumentList);

            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.Value = value;
            ChangeParent(value);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.SemiColonToken = semiColonToken;
            ChangeParent(semiColonToken);

            SlotCount = 6;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssTransitionSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.TransitionKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TransitionKeyword);

            this.ArgumentList = reader.ReadSyntaxNode<UvssTransitionArgumentListSyntax>(version);
            ChangeParent(this.ArgumentList);

            this.ColonToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.ColonToken);

            this.Value = reader.ReadSyntaxNode<UvssPropertyValueSyntax>(version);
            ChangeParent(this.Value);

            this.QualifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.QualifierToken);

            this.SemiColonToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.SemiColonToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(TransitionKeyword, version);
            writer.Write(ArgumentList, version);
            writer.Write(ColonToken, version);
            writer.Write(Value, version);
            writer.Write(QualifierToken, version);
            writer.Write(SemiColonToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TransitionKeyword;
                case 1: return ArgumentList;
                case 2: return ColonToken;
                case 3: return Value;
                case 4: return QualifierToken;
                case 5: return SemiColonToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the transition's "transition" keyword.
        /// </summary>
        public SyntaxToken TransitionKeyword { get; internal set; }

        /// <summary>
        /// Gets the transition's argument list.
        /// </summary>
        public UvssTransitionArgumentListSyntax ArgumentList { get; internal set; }

        /// <summary>
        /// Gets the colon that separates the transition declaration from its value.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// Gets the storyboard name that is associated with the transition.
        /// </summary>
        public UvssPropertyValueSyntax Value { get; internal set; }

        /// <summary>
        /// Gets the transition's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the semi-colon that terminates the transition.
        /// </summary>
        public SyntaxToken SemiColonToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitTransition(this);
        }
    }
}
