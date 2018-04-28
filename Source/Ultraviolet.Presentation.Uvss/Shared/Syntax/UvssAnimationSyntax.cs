using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS animation.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Animation)]
    public sealed class UvssAnimationSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationSyntax"/> class.
        /// </summary>
        internal UvssAnimationSyntax()
            : this(null, null, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationSyntax"/> class.
        /// </summary>
        internal UvssAnimationSyntax(
            SyntaxToken animationKeyword, 
            UvssPropertyNameSyntax propertyName, 
            UvssNavigationExpressionSyntax navigationExpression, 
            UvssBlockSyntax body)
            : base(SyntaxKind.Animation)
        {
            this.AnimationKeyword = animationKeyword;
            ChangeParent(animationKeyword);

            this.PropertyName = propertyName;
            ChangeParent(propertyName);

            this.NavigationExpression = navigationExpression;
            ChangeParent(navigationExpression);

            this.Body = body;
            ChangeParent(body);

            SlotCount = 4;
            UpdateIsMissing();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssAnimationSyntax"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        internal UvssAnimationSyntax(BinaryReader reader, Int32 version)
            : base(reader, version)
        {
            this.AnimationKeyword = reader.ReadSyntaxNode<SyntaxToken>(version);
            ChangeParent(this.AnimationKeyword);

            this.PropertyName = reader.ReadSyntaxNode<UvssPropertyNameSyntax>(version);
            ChangeParent(this.PropertyName);

            this.NavigationExpression = reader.ReadSyntaxNode<UvssNavigationExpressionSyntax>(version);
            ChangeParent(this.NavigationExpression);

            this.Body = reader.ReadSyntaxNode<UvssBlockSyntax>(version);
            ChangeParent(this.Body);
        }

        /// <inheritdoc/>
        public override void Serialize(BinaryWriter writer, Int32 version)
        {
            base.Serialize(writer, version);

            writer.Write(AnimationKeyword, version);
            writer.Write(PropertyName, version);
            writer.Write(NavigationExpression, version);
            writer.Write(Body, version);
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AnimationKeyword;
                case 1: return PropertyName;
                case 2: return NavigationExpression;
                case 3: return Body;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the "animation" keyword which begins the declaration.
        /// </summary>
        public SyntaxToken AnimationKeyword { get; internal set; }

        /// <summary>
        /// Gets the name of the animated property.
        /// </summary>
        public UvssPropertyNameSyntax PropertyName { get; internal set; }

        /// <summary>
        /// Gets the navigation expression for the animated property.
        /// </summary>
        public UvssNavigationExpressionSyntax NavigationExpression { get; internal set; }
        
        /// <summary>
        /// Gets the animation's body block.
        /// </summary>
        public UvssBlockSyntax Body { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitAnimation(this);
        }
    }
}
