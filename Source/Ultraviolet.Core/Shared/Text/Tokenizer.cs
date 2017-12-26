using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a class that splits strings into tokens.
    /// </summary>
    public static class Tokenizer
    {
        /// <summary>
        /// Splits the specified string into tokens.
        /// </summary>
        /// <param name="str">The <see cref="String"/> to tokenize.</param>
        /// <param name="tokens">The list to populate with the retrieved tokens.</param>
        public static void Tokenize(this String str, IList<String> tokens)
        {
            Contract.Require(tokens, nameof(tokens));

            Tokenize(str, tokens, Int32.MaxValue);
        }

        /// <summary>
        /// Splits the specified string into tokens.
        /// </summary>
        /// <param name="str">The <see cref="String"/> to tokenize.</param>
        /// <param name="count">The maximum number of tokens to retrieve.</param>
        /// <param name="tokens">The list to populate with the retrieved tokens.</param>
        public static void Tokenize(this String str, IList<String> tokens, Int32 count)
        {
            Contract.Require(tokens, nameof(tokens));

            string remainder = null;
            Tokenize(str, tokens, count, out remainder);
        }

        /// <summary>
        /// Splits the specified string into tokens.
        /// </summary>
        /// <param name="str">The <see cref="String"/> to tokenize.</param>
        /// <param name="tokens">The list to populate with the retrieved tokens.</param>
        /// <param name="count">The maximum number of tokens to retrieve.</param>
        /// <param name="remainder">The portion of the original string that was not tokenized.</param>
        public static void Tokenize(this String str, IList<String> tokens, Int32 count, out String remainder)
        {
            Contract.Require(tokens, nameof(tokens));

            // Assume no remainder.
            remainder = "";

            // Tokenize the string by iterating through its characters and
            // extracting complete words and enclosed strings.
            tokens.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                // If we've read the specified number of tokens, output the rest of the string.
                if (tokens.Count >= count)
                {
                    remainder = str.Substring(i, str.Length - i).Trim();
                    break;
                }

                // Read the next token.
                var tokenStartIndex = Int32.MinValue;
                var tokenEndIndex = Int32.MinValue;
                var tokenDelimiter = (char?)null;
                if (IsStartOfToken(str, i, out tokenDelimiter))
                {
                    // Read until the end of the current token.
                    tokenStartIndex = i;
                    for (int j = i; j < str.Length; j++)
                    {
                        if (IsEndOfToken(str, j, tokenDelimiter))
                        {
                            tokenEndIndex = j;
                            break;
                        }
                    }

                    // Extract the token from the string.
                    var tokenLength = (tokenEndIndex - tokenStartIndex) + 1;
                    tokens.Add(str.Substring(tokenStartIndex, tokenLength));

                    // Advance to the next token.
                    i += tokenLength;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the beginning of a token.
        /// </summary>
        /// <param name="str">The string that contains the character to evaluate.</param>
        /// <param name="index">The index of the character to evaluate.</param>
        /// <param name="delimiter">The character that delimits the start of the token.</param>
        /// <returns><see langword="true"/> if the specified character is the beginning of a token; otherwise, <see langword="false"/>.</returns>
        private static Boolean IsStartOfToken(String str, Int32 index, out Char? delimiter)
        {
            delimiter = null;
            if (index - 1 < 0)
            {
                if (str[index] == '"' || str[index] == '\'')
                {
                    return false;
                }
                return true;
            }
            if (!IsDelimiter(str, index))
            {
                if (str[index - 1] == '"' || str[index - 1] == '\'')
                {
                    delimiter = str[index - 1];
                    return true;
                }
                return Char.IsWhiteSpace(str[index - 1]);
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is the end of a token.
        /// </summary>
        /// <param name="str">The string that contains the character to evaluate.</param>
        /// <param name="index">The index of the character to evaluate.</param>
        /// <param name="delimiter">The character that must delimit the end of the token.</param>
        /// <returns><see langword="true"/> if the specified character is the end of a token; otherwise, <see langword="false"/>.</returns>
        private static Boolean IsEndOfToken(String str, Int32 index, Char? delimiter)
        {
            if (index + 1 >= str.Length)
            {
                return true;
            }
            if (delimiter != null)
            {
                return str[index + 1] == delimiter;
            }
            return Char.IsWhiteSpace(str[index + 1]);
        }

        /// <summary>
        /// Gets a value indicating whether the specified character is a token delimiter.
        /// </summary>
        /// <param name="str">The string that contains the character to evaluate.</param>
        /// <param name="index">The index of the character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is a token delimiter; otherwise, <see langword="false"/>.</returns>
        private static Boolean IsDelimiter(String str, Int32 index)
        {
            char c = str[index];
            return Char.IsWhiteSpace(c) || c == '"' || c == '\'';
        }
    }
}
