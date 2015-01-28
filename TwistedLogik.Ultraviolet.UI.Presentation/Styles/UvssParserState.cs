using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssParser
    {
        /// <summary>
        /// Represents the state of a <see cref="UvssParser"/> while parsing a particular source text.
        /// </summary>
        private class UvssParserState
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UvssParserState"/> class.
            /// </summary>
            /// <param name="source">The source text.</param>
            /// <param name="tokens">The lexer token stream to parse.</param>
            public UvssParserState(String source, IList<UvssLexerToken> tokens)
            {
                this.source   = source;
                this.tokens   = tokens;
                this.position = 0;
            }

            /// <summary>
            /// Consumes the next token in the stream.
            /// </summary>
            /// <returns>The token that was consumed.</returns>
            public UvssLexerToken Consume()
            {
                while (position < tokens.Count && tokens[position].IsComment)
                {
                    position++;
                }
                return tokens[position++];
            }

            /// <summary>
            /// Attempts to consume the next non-whitespace token.
            /// </summary>
            /// <returns>The token that was consumed, or <c>null</c> if no non-whitespace token was found.</returns>
            public UvssLexerToken? TryConsumeNonWhiteSpace()
            {
                while (position < tokens.Count && (tokens[position].TokenType == UvssLexerTokenType.WhiteSpace || tokens[position].IsComment)) 
                { 
                    position++; 
                }
                return position >= tokens.Count ? null : (UvssLexerToken?)tokens[position++];
            }

            /// <summary>
            /// Attempts to consume the next token in the stream.
            /// </summary>
            /// <returns>The token that was consumed, or <c>null</c> if the parser is past the end of the stream.</returns>
            public UvssLexerToken? TryConsume()
            {
                return IsPastEndOfStream ? null : (UvssLexerToken?)Consume();
            }

            /// <summary>
            /// Advances the parser's position in the token stream.
            /// </summary>
            public void Advance()
            {
                do
                {
                    position++;
                }
                while (!IsPastEndOfStream && CurrentToken.IsComment);
            }

            /// <summary>
            /// Advances beyond any whitespace characters starting at the current index within the token stream.
            /// </summary>
            public void AdvanceBeyondWhiteSpace()
            {
                while (!IsPastEndOfStream && (CurrentToken.TokenType == UvssLexerTokenType.WhiteSpace || CurrentToken.IsComment))
                {
                    position++;
                }
            }

            /// <summary>
            /// Gets the source text.
            /// </summary>
            public String Source
            {
                get { return source; }
            }

            /// <summary>
            /// Gets the lexer token stream being parsed.
            /// </summary>
            public IList<UvssLexerToken> Tokens
            {
                get { return tokens; }
            }

            /// <summary>
            /// Gets the token at the parser's current position.
            /// </summary>
            public UvssLexerToken CurrentToken
            {
                get { return tokens[position]; }
            }

            /// <summary>
            /// Gets the parser's position within the lexer token stream.
            /// </summary>
            public Int32 Position
            {
                get { return position; }
                set { position = value; }
            }

            /// <summary>
            /// Gets the number of tokens being parsed.
            /// </summary>
            public Int32 Length
            {
                get { return tokens.Count; }
            }

            /// <summary>
            /// Gets a value indicating whether the parser is beyond the end of its token stream.
            /// </summary>
            public Boolean IsPastEndOfStream
            {
                get { return position >= tokens.Count; }
            }

            // Property values.
            private readonly String source;
            private readonly IList<UvssLexerToken> tokens;
            private Int32 position;
        }
    }
}
