namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Contains extension methods for the <see cref="SyntaxNode"/> class.
    /// </summary>
    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Creates a new syntax tree based on the specified syntax tree which has normalized white space.
        /// </summary>
        /// <param name="this">The node which represents the root of the syntax tree to normalize.</param>
        /// <returns>The node which is at the root of the normalized syntax tree.</returns>
        public static SyntaxNode NormalizeWhitespace(this SyntaxNode @this)
        {
            return SyntaxNormalizer.Normalize(@this);
        }
    }
}
