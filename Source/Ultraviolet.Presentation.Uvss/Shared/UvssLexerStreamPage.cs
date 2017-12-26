using System;

namespace Ultraviolet.Presentation.Uvss
{
    partial class UvssLexerStream
    {
        /// <summary>
        /// Represents a contiguous sequence of lexer tokens.
        /// </summary>
        private class UvssLexerStreamPage
        {
            /// <summary>
            /// Adds a token to the end of the page, if space is available.
            /// </summary>
            /// <param name="token">The token to add to the end of the page.</param>
            /// <returns>true if the token was added to the page; otherwise, false.</returns>
            public Boolean Add(UvssLexerToken token)
            {
                if (count == Size)
                    return false;

                tokens[count++] = token;

                return true;
            }

            /// <summary>
            /// Gets the lexer token at the specified index within the page.
            /// </summary>
            /// <param name="index">The index of the token.</param>
            /// <returns>The lexer token at the specified index within the page.</returns>
            public UvssLexerToken this[Int32 index]
            {
                get { return tokens[index]; }
            }
            
            /// <summary>
            /// Gets the number of tokens which have been generated for this page.
            /// </summary>
            public Int32 Count
            {
                get { return count; }
            }

            /// <summary>
            /// Gets the total number of tokens which can be contained by this page.
            /// </summary>
            public Int32 Capacity
            {
                get { return tokens.Length; }
            }

            /// <summary>
            /// The number of tokens in a page.
            /// </summary>
            public const Int32 Size = 32;

            // Property values.
            private readonly UvssLexerToken[] tokens = new UvssLexerToken[Size];
            private Int32 count;
        }
    }
}
