using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS selector.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Selector)]
    public sealed class UvssSelectorSyntax : UvssSelectorBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax()
            : this(default(SyntaxList<SyntaxNode>))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class.
        /// </summary>
        internal UvssSelectorSyntax(
            SyntaxList<SyntaxNode> components)
            : base(SyntaxKind.Selector)
        {
            this.Components = components;
            ChangeParent(components.Node);
            
            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssSelectorSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.Components = reader.ReadSyntaxList<SyntaxNode>(version);
            ChangeParent(Components.Node);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(Components, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Components.Node;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override SyntaxList<SyntaxNode> Components { get; }

        /// <inheritdoc/>
        public override IEnumerable<UvssSelectorPartSyntax> Parts
        {
            get
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    var child = Components[i] as UvssSelectorPartSyntax;
                    if (child != null)
                        yield return child;
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<SyntaxToken> Combinators
        {
            get
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    var child = Components[i] as SyntaxToken;
                    if (child != null)
                    {
                        if (child.Kind == SyntaxKind.GreaterThanToken ||
                            child.Kind == SyntaxKind.GreaterThanGreaterThanToken ||
                            child.Kind == SyntaxKind.GreaterThanQuestionMarkToken)
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSelector(this);
        }
    }
}
