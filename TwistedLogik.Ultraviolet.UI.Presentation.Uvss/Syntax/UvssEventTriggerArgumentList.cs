using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the argument list for an event trigger.
    /// </summary>
    public sealed class UvssEventTriggerArgumentList : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerArgumentList"/> class.
        /// </summary>
        internal UvssEventTriggerArgumentList()
            : this(null, default(SeparatedSyntaxList<SyntaxNode>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTriggerArgumentList"/> class.
        /// </summary>
        internal UvssEventTriggerArgumentList(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> argumentList,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.EventTriggerArgumentList)
        {
            this.OpenParenToken = openParenToken;
            ChangeParent(openParenToken);

            this.ArgumentList = argumentList;
            ChangeParent(argumentList.Node);

            this.CloseParenToken = closeParenToken;
            ChangeParent(closeParenToken);

            SlotCount = 3;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return ArgumentList.Node;
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
        public SeparatedSyntaxList<SyntaxNode> ArgumentList { get; internal set; }

        /// <summary>
        /// Gets the close parenthesis that terminates the argument list.
        /// </summary>
        public SyntaxToken CloseParenToken { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEventTriggerArgumentList(this);
        }
    }
}
