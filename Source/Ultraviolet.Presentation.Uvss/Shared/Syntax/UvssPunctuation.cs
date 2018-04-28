using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS punctuation token.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Punctuation)]
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
        /// Initializes a new instance of the <see cref="UvssPunctuation"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public UvssPunctuation(BinaryReader reader, Int32 version)
            : base(reader, version)
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
                case SyntaxKind.OpenBracketToken:
                    return "[";
                case SyntaxKind.CloseBracketToken:
                    return "]";
                case SyntaxKind.AsteriskToken:
                    return "*";
                case SyntaxKind.GreaterThanGreaterThanToken:
                    return ">>";
                case SyntaxKind.GreaterThanQuestionMarkToken:
                    return ">?";
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
