using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a terminal token.
    /// </summary>
    public class SyntaxToken : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
        /// </summary>
        /// <param name="kind">The syntax token's kind.</param>
        /// <param name="text">The syntax token's text.</param>
        public SyntaxToken(SyntaxKind kind, String text)
            : this(kind, text, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
        /// </summary>
        /// <param name="kind">The syntax token's kind.</param>
        /// <param name="text">The syntax token's text.</param>
        /// <param name="leadingTrivia">The syntax token's leading trivia, if it has any.</param>
        /// <param name="trailingTrivia">The syntax token's trailing trivia, if it has any.</param>
        public SyntaxToken(SyntaxKind kind, String text, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(kind)
        {
            this.Text = text;

            if (leadingTrivia != null)
            {
                ExpandWidth(leadingTrivia);
                this.leadingTrivia = leadingTrivia;
            }

            if (trailingTrivia != null)
            {
                ExpandWidth(trailingTrivia);
                this.trailingTrivia = trailingTrivia;
            }
        }

        /// <summary>
        /// Adds the specified leading trivia to a token.
        /// </summary>
        /// <typeparam name="TToken">The type of token to which trivia is being added.</typeparam>
        /// <param name="token">The token to which to add trivia.</param>
        /// <param name="newTrivia">The trivia to add to the specified token.</param>
        /// <returns>A new token which is a clone of the specified token, but with 
        /// the specified leading trivia added.</returns>
        public static TToken AddLeadingTrivia<TToken>(TToken token, SyntaxList<SyntaxNode> newTrivia) 
            where TToken : SyntaxToken
        {
            if (newTrivia.Node == null)
                return token;

            var leadingTrivia = default(SyntaxNode);

            var oldTrivia = new SyntaxList<SyntaxNode>(token.GetLeadingTrivia());
            if (oldTrivia.Node == null)
            {
                leadingTrivia = newTrivia.Node;
            }
            else
            {
                var leadingTriviaBuilder = SyntaxListBuilder<SyntaxNode>.Create();
                leadingTriviaBuilder.AddRange(newTrivia);
                leadingTriviaBuilder.AddRange(oldTrivia);
                leadingTrivia = leadingTriviaBuilder.ToList().Node;
            }

            token.ChangeLeadingTrivia(leadingTrivia);
            return token;
        }

        /// <summary>
        /// Adds the specified trailing trivia to a token.
        /// </summary>
        /// <typeparam name="TToken">The type of token to which trivia is being added.</typeparam>
        /// <param name="token">The token to which to add trivia.</param>
        /// <param name="newTrivia">The trivia to add to the specified token.</param>
        /// <returns>A new token which is a clone of the specified token, but with 
        /// the specified trailing trivia added.</returns>
        public static TToken AddTrailingTrivia<TToken>(TToken token, SyntaxList<SyntaxNode> newTrivia) 
            where TToken : SyntaxToken
        {
            if (newTrivia.Node == null)
                return token;

            var trailingTrivia = default(SyntaxNode);

            var oldTrivia = new SyntaxList<SyntaxNode>(token.GetTrailingTrivia());
            if (oldTrivia.Node == null)
            {
                trailingTrivia = newTrivia.Node;
            }
            else
            {
                var trailingTriviaBuilder = SyntaxListBuilder<SyntaxNode>.Create();
                trailingTriviaBuilder.AddRange(newTrivia);
                trailingTriviaBuilder.AddRange(oldTrivia);
                trailingTrivia = trailingTriviaBuilder.ToList().Node;
            }

            token.ChangeTrailingTrivia(trailingTrivia);
            return token;
        }

        /// <inheritdoc/>
        public override String ToString() => Text;
        
        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override Int32 GetLeadingTriviaWidth()
        {
            return leadingTrivia?.FullWidth ?? 0;
        }

        /// <inheritdoc/>
        public override Int32 GetTrailingTriviaWidth()
        {
            return trailingTrivia?.FullWidth ?? 0;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetLeadingTrivia()
        {
            return leadingTrivia;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetTrailingTrivia()
        {
            return trailingTrivia;
        }

        /// <inheritdoc/>
        public override Boolean IsToken => true;

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public String Text { get; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSyntaxToken(this);
        }

        /// <inheritdoc/>
        internal override void WriteToOrFlatten(TextWriter writer, Stack<SyntaxNode> stack)
        {
            var leadingTrivia = GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                leadingTrivia.WriteTo(writer);
            }

            writer.Write(Text);

            var trailingTrivia = GetTrailingTrivia();
            if (trailingTrivia != null)
            {
                trailingTrivia.WriteTo(writer);
            }
        }
        
        /// <inheritdoc/>
        internal override void ChangeTrivia(SyntaxNode leading, SyntaxNode trailing)
        {
            this.leadingTrivia = leading;
            this.trailingTrivia = trailing;
        }

        /// <inheritdoc/>
        internal override void ChangeLeadingTrivia(SyntaxNode trivia)
        {
            this.leadingTrivia = trivia;
        }

        /// <inheritdoc/>
        internal override void ChangeTrailingTrivia(SyntaxNode trivia)
        {
            this.trailingTrivia = trivia;
        }

        /// <inheritdoc/>
        protected override Int32 ComputeFullWidth()
        {
            return
                GetLeadingTriviaWidth() +
                (Text?.Length ?? 0) +
                GetTrailingTriviaWidth();
        }

        /// <inheritdoc/>
        protected override void UpdateIsMissing()
        {

        }

        // Token trivia.
        private SyntaxNode leadingTrivia;
        private SyntaxNode trailingTrivia;
    }
}
