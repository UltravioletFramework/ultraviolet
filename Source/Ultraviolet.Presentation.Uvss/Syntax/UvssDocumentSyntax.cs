using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the root node of a UVSS document.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Document)]
    public sealed class UvssDocumentSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocumentSyntax"/> class.
        /// </summary>
        internal UvssDocumentSyntax()
            : this(default(SyntaxList<SyntaxNode>), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocumentSyntax"/> class.
        /// </summary>
        internal UvssDocumentSyntax(
            SyntaxList<SyntaxNode> content,
            SyntaxToken endOfFileToken)
            : base(SyntaxKind.Document)
        {
            this.Content = content;
            ChangeParent(content.Node);

            this.EndOfFileToken = endOfFileToken;
            ChangeParent(endOfFileToken);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssDocumentSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssDocumentSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.Content = reader.ReadSyntaxList<SyntaxNode>(version);
            ChangeParent(this.Content.Node);

            this.EndOfFileToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.EndOfFileToken);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);
            
            writer.Write(Content, version);
            writer.Write(EndOfFileToken, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Content.Node;
                case 1: return EndOfFileToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the document's content.
        /// </summary>
        public SyntaxList<SyntaxNode> Content { get; internal set; }

        /// <summary>
        /// Gets a collection of the document's directives.
        /// </summary>
        public IEnumerable<UvssDirectiveSyntax> Directives
        {
            get
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var directive = Content[i] as UvssDirectiveSyntax;
                    if (directive != null)
                        yield return directive;
                }
            }
        }

        /// <summary>
        /// Gets a collection of the document's rule sets.
        /// </summary>
        public IEnumerable<UvssRuleSetSyntax> RuleSets
        {
            get
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var ruleSet = Content[i] as UvssRuleSetSyntax;
                    if (ruleSet != null)
                        yield return ruleSet;
                }
            }
        }

        /// <summary>
        /// Gets a collection of the document's storyboards.
        /// </summary>
        public IEnumerable<UvssStoryboardSyntax> Storyboards
        {
            get
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var storyboard = Content[i] as UvssStoryboardSyntax;
                    if (storyboard != null)
                        yield return storyboard;
                }
            }
        }

        /// <summary>
        /// Gets the document's end-of-file token.
        /// </summary>
        public SyntaxToken EndOfFileToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitDocument(this);
        }
    }
}
