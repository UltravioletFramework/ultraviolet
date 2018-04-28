using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the argument list for a visual transition.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.TransitionArgumentList)]
    public sealed class UvssTransitionArgumentListSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionArgumentListSyntax"/> class.
        /// </summary>
        internal UvssTransitionArgumentListSyntax()
            : this(null, default(SeparatedSyntaxList<SyntaxNode>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionArgumentListSyntax"/> class.
        /// </summary>
        internal UvssTransitionArgumentListSyntax(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> arguments,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.TransitionArgumentList)
        {
            this.OpenParenToken = openParenToken;
            ChangeParent(openParenToken);

            this.Arguments = arguments;
            ChangeParent(arguments.Node);

            this.CloseParenToken = closeParenToken;
            ChangeParent(closeParenToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionArgumentListSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssTransitionArgumentListSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.OpenParenToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.OpenParenToken);

            this.Arguments = reader.ReadSeparatedSyntaxList<SyntaxNode>(version);
            ChangeParent(this.Arguments.Node);

            this.CloseParenToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.CloseParenToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(OpenParenToken, version);
            writer.Write(Arguments, version);
            writer.Write(CloseParenToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return Arguments.Node;
                case 2: return CloseParenToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the open parenthesis that introduces the argument list.
        /// </summary>
        public SyntaxToken OpenParenToken { get; internal set; }

        /// <summary>
        /// Gets the list's arguments.
        /// </summary>
        public SeparatedSyntaxList<SyntaxNode> Arguments { get; internal set; }

        /// <summary>
        /// Gets the collection of identifiers that represent the list's arguments.
        /// </summary>
        public IEnumerable<UvssIdentifierBaseSyntax> ArgumentIdentifiers
        {
            get
            {
                for (int i = 0; i < Arguments.Count; i++)
                {
                    var child = Arguments[i] as UvssIdentifierBaseSyntax;
                    if (child != null)
                        yield return child;
                }
            }
        }

        /// <summary>
        /// Gets the close parenthesis that terminates the argument list.
        /// </summary>
        public SyntaxToken CloseParenToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitTransitionArgumentList(this);
        }
    }
}
