using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property trigger.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.PropertyTrigger)]
    public sealed class UvssPropertyTriggerSyntax : UvssTriggerBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerSyntax()
            : this(null, null, default(SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax>), null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class.
        /// </summary>
        internal UvssPropertyTriggerSyntax(
            SyntaxToken triggerKeyword,
            SyntaxToken propertyKeyword,
            SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> conditions,
            SyntaxToken qualifierToken,
            UvssBlockSyntax body)
            : base(SyntaxKind.PropertyTrigger)
        {
            this.TriggerKeyword = triggerKeyword;
            ChangeParent(triggerKeyword);

            this.PropertyKeyword = propertyKeyword;
            ChangeParent(propertyKeyword);

            this.Conditions = conditions;
            ChangeParent(conditions.Node);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 5;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssPropertyTriggerSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.TriggerKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.TriggerKeyword);

            this.PropertyKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.PropertyKeyword);

            this.Conditions = reader.ReadSeparatedSyntaxList<UvssPropertyTriggerConditionSyntax>(version);
            ChangeParent(this.Conditions.Node);

            this.QualifierToken = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.QualifierToken);

            this.Body = reader.ReadSyntaxNode<UvssBlockSyntax>(version);
            ChangeParent(this.Body);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(TriggerKeyword, version);
            writer.Write(PropertyKeyword, version);
            writer.Write(Conditions, version);
            writer.Write(QualifierToken, version);
            writer.Write(Body, version);
        }
        
        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TriggerKeyword;
                case 1: return PropertyKeyword;
                case 2: return Conditions.Node;
                case 3: return QualifierToken;
                case 4: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the trigger's "trigger" keyword.
        /// </summary>
        public SyntaxToken TriggerKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's "property" keyword.
        /// </summary>
        public SyntaxToken PropertyKeyword { get; internal set; }

        /// <summary>
        /// Gets the trigger's conditions.
        /// </summary>
        public SeparatedSyntaxList<UvssPropertyTriggerConditionSyntax> Conditions { get; internal set; }

        /// <summary>
        /// Gets the trigger's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// Gets the trigger's body.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        public override IEnumerable<UvssTriggerActionBaseSyntax> Actions
        {
            get
            {
                if (Body != null)
                {
                    for (int i = 0; i < Body.Content.Count; i++)
                    {
                        var node = Body.Content[i] as UvssTriggerActionBaseSyntax;
                        if (node != null)
                            yield return node;
                    }
                }
            }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyTrigger(this);
        }
    }
}
