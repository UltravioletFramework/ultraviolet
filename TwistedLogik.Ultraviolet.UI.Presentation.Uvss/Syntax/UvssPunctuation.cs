using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS punctuation token.
    /// </summary>
    public class UvssPunctuation : SyntaxToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPunctuation"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the punctuation's kind.</param>
        public UvssPunctuation(SyntaxKind kind)
            : this(kind, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPunctuation"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the punctuation's kind.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        public UvssPunctuation(SyntaxKind kind, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(kind, GetPunctuationText(kind), leadingTrivia, trailingTrivia)
        {

        }
        
        /// <summary>
        /// Gets the text associated with the specified punctuation kind.
        /// </summary>
        /// <param name="kind">The kind of punctuation for which to retrieve text.</param>
        /// <returns>The text associated with the specified punctuation kind.</returns>
        private static String GetPunctuationText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.CommaToken:
                    return ",";
                case SyntaxKind.ColonToken:
                    return ":";
                case SyntaxKind.SemiColonToken:
                    return ";";
                case SyntaxKind.AtSignToken:
                    return "@";
                case SyntaxKind.HashToken:
                    return "#";
                case SyntaxKind.PeriodToken:
                    return ".";
                case SyntaxKind.ExclamationMarkToken:
                    return "!";
                case SyntaxKind.OpenParenthesesToken:
                    return "(";
                case SyntaxKind.CloseParenthesesToken:
                    return ")";
                case SyntaxKind.OpenCurlyBraceToken:
                    return "{";
                case SyntaxKind.CloseCurlyBraceToken:
                    return "}";
                case SyntaxKind.AsteriskToken:
                    return "*";
                case SyntaxKind.GreaterThanGreaterThanToken:
                    return ">>";
                case SyntaxKind.GreaterThanQuestionMarkToken:
                    return ">?";
                case SyntaxKind.SpaceToken:
                    return " ";
                case SyntaxKind.EqualsToken:
                    return "=";
                case SyntaxKind.NotEqualsToken:
                    return "<>";
                case SyntaxKind.LessThanToken:
                    return "<";
                case SyntaxKind.GreaterThanToken:
                    return ">";
                case SyntaxKind.LessThanEqualsToken:
                    return "<=";
                case SyntaxKind.GreaterThanEqualsToken:
                    return ">=";
                case SyntaxKind.PipeToken:
                    return "|";
            }
            throw new InvalidOperationException();
        }
    }
}
