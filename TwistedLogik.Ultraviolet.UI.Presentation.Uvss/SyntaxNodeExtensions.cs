using System.Collections.Generic;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains extension methods for the <see cref="SyntaxNode"/> class.
    /// </summary>
    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Creates a new syntax tree based on the specified syntax tree which has normalized white space.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node which represents the root of the syntax tree to normalize.</param>
        /// <returns>The node which is at the root of the normalized syntax tree.</returns>
        public static TSyntax NormalizeWhitespace<TSyntax>(this TSyntax @this)
            where TSyntax : SyntaxNode
        {
            return SyntaxNormalizer.Normalize(@this);
        }
        
        /// <summary>
        /// Creates a copy of the specified node with no trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <returns>A copy of the specified node with node trivia.</returns>
        public static TSyntax WithoutTrivia<TSyntax>(this TSyntax @this)
            where TSyntax : SyntaxNode
        {
            @this.ChangeTrivia(null, null);
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with the specified leading trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <param name="trivia">The leading trivia to set on the created node.</param>
        /// <returns>A copy of the specified node with the specified leading trivia.</returns>
        public static TSyntax WithLeadingTrivia<TSyntax>(this TSyntax @this, SyntaxNode trivia)
            where TSyntax : SyntaxNode
        {
            @this.ChangeLeadingTrivia(trivia);
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with the specified leading trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <param name="trivia">The collection of leading trivia to set on the created node.</param>
        /// <returns>A copy of the specified node with the specified leading trivia.</returns>
        public static TSyntax WithLeadingTrivia<TSyntax>(this TSyntax @this, IEnumerable<SyntaxTrivia> trivia)
            where TSyntax : SyntaxNode
        {
            if (trivia != null)
            {
                var triviaListBuilder = SyntaxListBuilder<SyntaxTrivia>.Create();
                triviaListBuilder.AddRange(trivia);
                @this.ChangeLeadingTrivia(triviaListBuilder.ToListNode());
            }
            else
            {
                @this.ChangeLeadingTrivia(null);
            }
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with no leading trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <returns>A copy of the specified node with no leading trivia.</returns>
        public static TSyntax WithoutLeadingTrivia<TSyntax>(this TSyntax @this)
            where TSyntax : SyntaxNode
        {
            @this.ChangeLeadingTrivia(null);
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with the specified trailing trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <param name="trivia">The leading trivia to set on the created node.</param>
        /// <returns>A copy of the specified node with the specified trailing trivia.</returns>
        public static TSyntax WithTrailingTrivia<TSyntax>(this TSyntax @this, SyntaxNode trivia)
            where TSyntax : SyntaxNode
        {
            @this.ChangeTrailingTrivia(trivia);
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with the specified trailing trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <param name="trivia">The collection of leading trivia to set on the created node.</param>
        /// <returns>A copy of the specified node with the specified trailing trivia.</returns>
        public static TSyntax WithTrailingTrivia<TSyntax>(this TSyntax @this, IEnumerable<SyntaxTrivia> trivia)
            where TSyntax : SyntaxNode
        {
            if (trivia != null)
            {
                var triviaListBuilder = SyntaxListBuilder<SyntaxTrivia>.Create();
                triviaListBuilder.AddRange(trivia);
                @this.ChangeTrailingTrivia(triviaListBuilder.ToListNode());
            }
            else
            {
                @this.ChangeTrailingTrivia(null);
            }
            return @this;
        }

        /// <summary>
        /// Creates a copy of the specified node with no trailing trivia.
        /// </summary>
        /// <typeparam name="TSyntax">The type of node on which this operation is being performed.</typeparam>
        /// <param name="this">The node on which this operation is being performed.</param>
        /// <returns>A copy of the specified node with no trailing trivia.</returns>
        public static TSyntax WithoutTrailingTrivia<TSyntax>(this TSyntax @this)
            where TSyntax : SyntaxNode
        {
            @this.ChangeTrailingTrivia(null);
            return @this;
        }
    }
}
