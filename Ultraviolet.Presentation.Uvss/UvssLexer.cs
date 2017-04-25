using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents an Ultraviolet StyleSheets (UVSS) lexer which takes a raw string as its input and produces a 
    /// stream of <see cref="UvssLexerToken"/> instances. 
    /// </summary>
    public class UvssLexer
    {
        /// <summary>
        /// Initializes the <see cref="UvssLexer"/> type.
        /// </summary>
        static UvssLexer()
        {
            regexLexer = CreateLexerRegex();
            regexGroupNames = regexLexer.GetGroupNames();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssLexer"/> class.
        /// </summary>
        /// <param name="source">The source which is being lexed.</param>
        /// <param name="options">The lexer's options.</param>
        private UvssLexer(String source, UvssLexerOptions options)
        {
            this.source = source;
            this.options = options;
        }

        /// <summary>
        /// Tokenizes the specified Ultraviolet StyleSheets (UVSS) string.
        /// </summary>
        /// <param name="source">The source string to tokenize.</param>
        /// <param name="options">The lexer's configurable options, or null to use the default options.</param>
        /// <returns>A <see cref="UvssLexerStream"/> instance which produces lexed tokens from the specified source text.</returns>
        public static UvssLexerStream Tokenize(String source, UvssLexerOptions options = null)
        {
            Contract.Require(source, nameof(source));

            var instance = new UvssLexer(source, options ?? UvssLexerOptions.Default);

            return new UvssLexerStream(instance);
        }

        /// <summary>
        /// Emits the next token which is produced from the lexer's source text.
        /// </summary>
        /// <param name="token">The token that was emitted.</param>
        /// <returns>true if a token was emitted; otherwise, false.</returns>
        public Boolean Emit(out UvssLexerToken token)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(options, nameof(options));

            if (position >= source.Length)
            {
                token = default(UvssLexerToken);
                return false;
            }

            var tokenType = default(UvssLexerTokenType);
            var tokenText = default(String);

            var match = regexLexer.Match(source, position);
            if (match.Success)
            {
                if (!GetTokenInfoFromRegexMatch(match, out tokenType, out tokenText))
                {
                    var errorMessage = String.Format(UvssStrings.LexerInvalidToken,
                        match.Value, lineIndex, columnIndex);
                    throw new UvssLexerException(errorMessage, ErrorCodeInvalidToken, lineIndex, columnIndex);
                }

                ReadExtendedToken(source, position, ref tokenType, ref tokenText);

                var sourceOffset = position;
                var sourceLength = tokenText.Length;
                var sourceLine = lineIndex;
                var sourceColumn = columnIndex;

                token = new UvssLexerToken(tokenType,
                    sourceOffset, sourceLength, sourceLine, sourceColumn, tokenText);
            }
            else
            {
                var sourceOffset = position;
                var sourceLength = 1;
                var sourceLine = lineIndex;
                var sourceColumn = columnIndex;

                token = new UvssLexerToken(UvssLexerTokenType.Unknown,
                    sourceOffset, sourceLength, sourceLine, sourceColumn, source[position].ToString());
            }

            HandleLineAndColumnTracking(token.Text, ref lineIndex, ref columnIndex, options);
            position += token.Text.Length;

            return true;
        }

        /// <summary>
        /// Gets the lexer's current position within the source tet.
        /// </summary>
        public Int32 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the line index at the lexer's current position.
        /// </summary>
        public Int32 LineIndex
        {
            get { return lineIndex; }
        }

        /// <summary>
        /// Gets the column index at the lexer's current position.
        /// </summary>
        public Int32 ColumnIndex
        {
            get { return columnIndex; }
        }

        /// <summary>
        /// Given a token type name, returns the corresponding <see cref="UvssLexerTokenType"/> value.
        /// </summary>
        /// <param name="name">The name of the token type.</param>
        /// <returns>The <see cref="UvssLexerTokenType"/> value that corresponds to the specified name.</returns>
        private static UvssLexerTokenType GetTokenTypeFromName(String name)
        {
            switch (name)
            {
                case nameof(UvssLexerTokenType.Unknown):
                    return UvssLexerTokenType.Unknown;

                case nameof(UvssLexerTokenType.EndOfLine):
                    return UvssLexerTokenType.EndOfLine;

                case nameof(UvssLexerTokenType.SingleLineComment):
                    return UvssLexerTokenType.SingleLineComment;

                case nameof(UvssLexerTokenType.MultiLineComment):
                    return UvssLexerTokenType.MultiLineComment;

                case nameof(UvssLexerTokenType.WhiteSpace):
                    return UvssLexerTokenType.WhiteSpace;

                case nameof(UvssLexerTokenType.Keyword):
                    return UvssLexerTokenType.Keyword;

                case nameof(UvssLexerTokenType.Identifier):
                    return UvssLexerTokenType.Identifier;

                case nameof(UvssLexerTokenType.Number):
                    return UvssLexerTokenType.Number;

                case nameof(UvssLexerTokenType.Comma):
                    return UvssLexerTokenType.Comma;

                case nameof(UvssLexerTokenType.Colon):
                    return UvssLexerTokenType.Colon;

                case nameof(UvssLexerTokenType.SemiColon):
                    return UvssLexerTokenType.SemiColon;

                case nameof(UvssLexerTokenType.AtSign):
                    return UvssLexerTokenType.AtSign;

                case nameof(UvssLexerTokenType.Hash):
                    return UvssLexerTokenType.Hash;

                case nameof(UvssLexerTokenType.Period):
                    return UvssLexerTokenType.Period;

                case nameof(UvssLexerTokenType.ExclamationMark):
                    return UvssLexerTokenType.ExclamationMark;

                case nameof(UvssLexerTokenType.OpenParenthesis):
                    return UvssLexerTokenType.OpenParenthesis;

                case nameof(UvssLexerTokenType.CloseParenthesis):
                    return UvssLexerTokenType.CloseParenthesis;

                case nameof(UvssLexerTokenType.OpenCurlyBrace):
                    return UvssLexerTokenType.OpenCurlyBrace;

                case nameof(UvssLexerTokenType.CloseCurlyBrace):
                    return UvssLexerTokenType.CloseCurlyBrace;

                case nameof(UvssLexerTokenType.OpenBracket):
                    return UvssLexerTokenType.OpenBracket;

                case nameof(UvssLexerTokenType.CloseBracket):
                    return UvssLexerTokenType.CloseBracket;

                case nameof(UvssLexerTokenType.Asterisk):
                    return UvssLexerTokenType.Asterisk;

                case nameof(UvssLexerTokenType.GreaterThanGreaterThan):
                    return UvssLexerTokenType.GreaterThanGreaterThan;

                case nameof(UvssLexerTokenType.GreaterThanQuestionMark):
                    return UvssLexerTokenType.GreaterThanQuestionMark;
                    
                case nameof(UvssLexerTokenType.Equals):
                    return UvssLexerTokenType.Equals;

                case nameof(UvssLexerTokenType.NotEquals):
                    return UvssLexerTokenType.NotEquals;

                case nameof(UvssLexerTokenType.LessThan):
                    return UvssLexerTokenType.LessThan;

                case nameof(UvssLexerTokenType.LessThanEquals):
                    return UvssLexerTokenType.LessThanEquals;

                case nameof(UvssLexerTokenType.GreaterThan):
                    return UvssLexerTokenType.GreaterThan;

                case nameof(UvssLexerTokenType.GreaterThanEquals):
                    return UvssLexerTokenType.GreaterThanEquals;

                case nameof(UvssLexerTokenType.Pipe):
                    return UvssLexerTokenType.Pipe;

                case nameof(UvssLexerTokenType.Directive):
                    return UvssLexerTokenType.Directive;

                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        /// <summary>
        /// Given a <see cref="Match"/> instance, determines the type and text of the corresponding lexer token.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> instance to evaluate.</param>
        /// <param name="tokenType">The lexed token's type.</param>
        /// <param name="tokenText">The lexed token's text.</param>
        /// <returns>true if the <see cref="Match"/> represented a valid token; otherwise, false.</returns>
        private static Boolean GetTokenInfoFromRegexMatch(Match match,
            out UvssLexerTokenType tokenType, out String tokenText)
        {
            for (int i = 1; i < match.Groups.Count; i++)
            {
                var group = match.Groups[i];
                if (group.Success)
                {
                    tokenType = GetTokenTypeFromName(regexGroupNames[i]);
                    tokenText = group.Value;
                    return true;
                }
            }

            tokenType = UvssLexerTokenType.Unknown;
            tokenText = null;

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether there is an !important qualifier at the specified
        /// position within the source string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which to begin evaluation.</param>
        /// <returns>true if there is an !important qualifier at the specified position; otherwise, false.</returns>
        private static Boolean GetImportantQualifierAtPosition(String source, Int32 position)
        {
            const String ImportantQualifier = "!important";

            for (int i = 0; i < ImportantQualifier.Length; i++)
            {
                if (source[position + i] != ImportantQualifier[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Reads an extended token, such as a comment, which continues until reaching some termination condition.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The position at which the token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The token's text.</param>
        private static void ReadExtendedToken(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            switch (tokenType)
            {
                case UvssLexerTokenType.SingleLineComment:
                    ReadSingleLineComment(source, position, ref tokenType, ref tokenText);
                    break;

                case UvssLexerTokenType.MultiLineComment:
                    ReadMultiLineComment(source, position, ref tokenType, ref tokenText);
                    break;
            }
        }

        /// <summary>
        /// Reads a single-line comment token starting at the specified position in the source.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The source position at which the comment token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The full text of the single-line comment token.</param>
        private static void ReadSingleLineComment(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            var offset = position;
            var length = tokenText.Length;

            for (int i = position + tokenText.Length; i < source.Length; i++)
            {
                if (source.Length == i + 1)
                {
                    length = source.Length - offset;
                    break;
                }

                if ((source[i] == '\r' && source[i + 1] == '\n') || source[i] == '\n')
                {
                    length = i - offset;
                    break;
                }
            }

            tokenText = source.Substring(offset, length);
        }

        /// <summary>
        /// Reads a multi-line comment token starting at the specified position in the source.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="position">The source position at which the comment token begins.</param>
        /// <param name="tokenType">The token's type.</param>
        /// <param name="tokenText">The full text of the multi-line comment token.</param>
        private static void ReadMultiLineComment(String source, Int32 position,
            ref UvssLexerTokenType tokenType, ref String tokenText)
        {
            var offset = position;
            var length = source.Length - position;

            for (int i = position + tokenText.Length; i < source.Length - 1; i++)
            {
                if (source[i] == '*' && source[i + 1] == '/')
                {
                    length = (i - offset) + 2;
                    break;
                }
            }

            tokenText = source.Substring(offset, length);
        }
        
        /// <summary>
        /// Determines whether the lexer has advanced to a new line and updates the line index accordingly.
        /// In addition, this method moves the column index forward by the length of the current token, 
        /// performing tab expansion as necessary.
        /// </summary>
        /// <param name="text">The text of the token that is being emitted.</param>
        /// <param name="lineIndex">The current line index.</param>
        /// <param name="columnIndex">The current column index.</param>
        /// <param name="options">The lexer's configurable options.</param>
        private static void HandleLineAndColumnTracking(String text, 
            ref Int32 lineIndex, ref Int32 columnIndex, UvssLexerOptions options)
        {
            var newlineMatches = regexNewline.Matches(text);
            if (newlineMatches.Count > 0)
            {
                var lineStartOffset = 0;

                foreach (Match match in newlineMatches)
                {
                    lineIndex++;
                    lineStartOffset = match.Index + match.Length;
                }

                columnIndex = 1 + CalculateTokenLengthInColumns(text, lineStartOffset, options);
            }
            else
            {
                columnIndex += CalculateTokenLengthInColumns(text, 0, options);
            }
        }

        /// <summary>
        /// Calculates the length of the specified token text in columns, performing tab expansion if necessary.
        /// </summary>
        /// <param name="text">The text of the token that is being emitted.</param>
        /// <param name="offset">The offset into the token text at which to begin measuring length.</param>
        /// <param name="options">The lexer's configurable options.</param>
        /// <returns>The length of the specified token text in columns.</returns>
        private static Int32 CalculateTokenLengthInColumns(String text, Int32 offset, UvssLexerOptions options)
        {
            var length = 0;

            for (int i = offset; i < text.Length; i++)
                length += (text[i] == '\t') ? options.TabSize : 1;

            return length;
        }

        /// <summary>
        /// Counts the number of leading white space characters in the specified string.
        /// </summary>
        /// <param name="text">The string in which to count leading white space characters.</param>
        /// <returns>The number of leading white space characters in the specified string.</returns>
        private static Int32 CountLeadingWhiteSpace(String text)
        {
            var length = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (!Char.IsWhiteSpace(text[i]))
                    break;

                length++;
            }

            return length;
        }

        /// <summary>
        /// Counts the number of trailing white space characters in the specified string.
        /// </summary>
        /// <param name="text">The string in which to count trailing white space characters.</param>
        /// <returns>The number of trailing white space characters in the specified string.</returns>
        private static Int32 CountTrailingWhiteSpace(String text)
        {
            var length = 0;

            for (var i = text.Length - 1; i >= 0; i--)
            {
                if (!Char.IsWhiteSpace(text[i]))
                    break;

                length++;
            }

            return (length == text.Length) ? 0 : length;
        }

        /// <summary>
        /// Emits a token into the output stream.
        /// </summary>
        /// <param name="output">The output stream into which to emit the token.</param>
        /// <param name="token">The token to emit into the output stream.</param>
        /// <param name="lineIndex">The current line index.</param>
        /// <param name="columnIndex">The current column index.</param>
        /// <param name="options">The lexer's configurable options.</param>
        /// <returns>The number of characters by which to advance the lexer's position 
        /// within the source string.</returns>
        private static Int32 EmitToken(IList<UvssLexerToken> output, UvssLexerToken token,
            ref Int32 lineIndex, ref Int32 columnIndex, UvssLexerOptions options)
        {
            HandleLineAndColumnTracking(token.Text, ref lineIndex, ref columnIndex, options);
            output.Add(token);

            return token.Text.Length;
        }

        /// <summary>
        /// Creates a new <see cref="Regex"/> instance based on the lexer's production rules.
        /// </summary>
        /// <returns>The <see cref="Regex"/> instance which was created.></returns>
        private static Regex CreateLexerRegex()
        {
            var pattern = String.Join("|", ProductionRules.Select(x => $"(?<{x.Key}>{x.Value})"));
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);
            return regex;
        }

        // The production rules for each of the lexer's token types, in order from highest priority to lowest.
        private static readonly KeyValuePair<UvssLexerTokenType, String>[] ProductionRules = new[]
        {
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.EndOfLine,
                @"\G\r\n|\G\n|\G\r"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.MultiLineComment,
                @"\G/\*"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.SingleLineComment,
                @"\G//"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.WhiteSpace,
                @"\G[^\S\r\n]+"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Directive,
                @"\G\$([_\-a-zA-Z][_\-a-zA-Z0-9]*)?"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Keyword,
                @"\Gplay-storyboard|" +
                @"\Gset-handled|" +
                @"\Gtransition|" +
                @"\G!important|" +
                @"\Ganimation|" +
                @"\Gplay-sfx|" +
                @"\Gproperty|" +
                @"\Gkeyframe|" +
                @"\Gtrigger(?!-root)|" +
                @"\Ghandled|" +
                @"\Gtarget|" +
                @"\Gevent|" +
                @"\Gset|" +
                @"\Gas"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Identifier,
                @"\G[_\-a-zA-Z][_\-a-zA-Z0-9]*"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Number,
                @"\G\d+(\.\d*)?"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Comma,
                @"\G,"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Colon,
                @"\G:"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.SemiColon,
                @"\G;"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.AtSign,
                @"\G@"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Hash,
                @"\G#"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Period,
                @"\G\."),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.ExclamationMark,
                @"\G!"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.OpenParenthesis,
                @"\G\("),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.CloseParenthesis,
                @"\G\)"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.OpenCurlyBrace,
                @"\G\{"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.CloseCurlyBrace,
                @"\G\}"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.OpenBracket,
                @"\G\["),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.CloseBracket,
                @"\G\]"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Asterisk,
                @"\G\*"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThanGreaterThan,
                @"\G\>\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThanQuestionMark,
                @"\G\>\?"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.NotEquals,
                @"\G\<\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.LessThanEquals,
                @"\G\<="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThanEquals,
                @"\G\>="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.LessThan,
                @"\G\<"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.GreaterThan,
                @"\G\>"),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Equals,
                @"\G="),
            new KeyValuePair<UvssLexerTokenType, String>(UvssLexerTokenType.Pipe,
                @"\G\|"),
        };

        // The regex engine used to create tokens.
        private static readonly Regex regexNewline = new Regex("\r\n|\n");
        private static readonly Regex regexLexer;
        private static readonly IList<String> regexGroupNames;

        // Lexer error codes.
        private const String ErrorCodeInvalidToken = "LEX0001";

        // Lexer instance state.
        private String source;
        private UvssLexerOptions options;
        private Int32 position;
        private Int32 lineIndex = 1;
        private Int32 columnIndex = 1;
    }
}
