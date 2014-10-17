using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    public partial class TextParser
    {
        /// <summary>
        /// Represents a token produced by parsing formatted text.
        /// </summary>
        public struct Token
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the Token structure.
            /// </summary>
            /// <param name="text">The token's text.</param>
            /// <param name="style">The token's style index.</param>
            internal Token(StringSegment text, Int32 style)
            {
                this.text = text;
                this.style = style;
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Converts the object to a human-readable string.
            /// </summary>
            /// <returns>A human-readable string that represents the object.</returns>
            public override String ToString()
            {
                return Text.ToString();
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the token's text.
            /// </summary>
            public StringSegment Text
            {
                get { return text; }
            }

            /// <summary>
            /// Gets the token's style index.
            /// </summary>
            public Int32 Style
            {
                get { return style; }
            }

            /// <summary>
            /// Gets a value indicating whether this is a white space character.
            /// </summary>
            public Boolean IsWhiteSpace
            {
                get { return !text.IsEmpty && Char.IsWhiteSpace(text[0]); }
            }

            /// <summary>
            /// Gets a value indicating whether this is a new line character.
            /// </summary>
            public Boolean IsNewLine
            {
                get { return !text.IsEmpty && text[0] == '\n'; }
            }

            #endregion

            #region Private Fields

            // Property values.
            private readonly StringSegment text;
            private readonly Int32 style;

            #endregion
        }
    }
}