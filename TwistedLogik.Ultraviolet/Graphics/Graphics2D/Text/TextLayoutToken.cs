using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a token of formatted text after it has been positioned by the layout engine.
    /// </summary>
    public struct TextLayoutToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutToken"/> structure.
        /// </summary>
        /// <param name="text">The token's text.</param>
        /// <param name="bounds">The token's bounds relative to its layout region.</param>
        /// <param name="fontFace">The token's font face.</param>
        /// <param name="icon">The token's icon.</param>
        /// <param name="color">The token's color.</param>
        internal TextLayoutToken(StringSegment text, Rectangle bounds, SpriteFontFace fontFace, InlineIconInfo? icon, Color? color)
        {
            this.text = text;
            this.bounds = bounds;
            this.fontFace = fontFace;
            this.icon = icon;
            this.color = color;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("\"{0}\" @ {1}, {2}", text, bounds.X, bounds.Y);
        }

        /// <summary>
        /// Gets a value indicating whether this token's style matches another token's style.
        /// </summary>
        /// <param name="token">The <see cref="TextLayoutToken"/> to compare against this token.</param>
        /// <returns><c>true</c> if the tokens have matching styles; otherwise, <c>false</c>.</returns>
        public Boolean MatchesStyle(TextLayoutToken token)
        {
            return MatchesStyleRef(ref token);
        }

        /// <summary>
        /// Gets a value indicating whether this token's style matches another token's style.
        /// </summary>
        /// <param name="token">The <see cref="TextLayoutToken"/> to compare against this token.</param>
        /// <returns><c>true</c> if the tokens have matching styles; otherwise, <c>false</c>.</returns>
        [CLSCompliant(false)]
        public Boolean MatchesStyleRef(ref TextLayoutToken token)
        {
            return
                this.color == token.color &&
                this.fontFace == token.fontFace &&
                this.icon.GetValueOrDefault().Equals(token.icon.GetValueOrDefault());
        }

        /// <summary>
        /// Gets the token's bounding rectangle, translated to the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate to which to translate the token's bounding rectangle.</param>
        /// <param name="y">The y-coordinate to which to translate the token's bounding rectangle.</param>
        /// <returns>The token's bounding rectangle.</returns>
        public Rectangle GetBounds(Int32 x, Int32 y)
        {
            return new Rectangle(bounds.X + x, bounds.Y + y, bounds.Width, bounds.Height);
        }

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public StringSegment Text
        {
            get { return text; }
        }

        /// <summary>
        /// Gets the token's bounds relative to its layout region.
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
            internal set { bounds = value; }
        }

        /// <summary>
        /// Gets the token's font face.
        /// </summary>
        public SpriteFontFace FontFace
        {
            get { return fontFace; }
        }

        /// <summary>
        /// Gets the token's icon.
        /// </summary>
        public InlineIconInfo? Icon
        {
            get { return icon; }
        }

        /// <summary>
        /// Gets the token's color.
        /// </summary>
        public Color? Color
        {
            get { return color; }
        }

        // Property values.
        private readonly StringSegment text;
        private Rectangle bounds;
        private readonly SpriteFontFace fontFace;
        private readonly InlineIconInfo? icon;
        private readonly Color? color;
    }
}
