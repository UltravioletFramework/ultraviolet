using System;
using System.Text;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="TextShaper"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="capacity">The initial capacity of the text builder.</param>
    /// <returns>The instance of <see cref="TextShaper"/> that was created.</returns>
    public delegate TextShaper TextShaperFactory(UltravioletContext uv, Int32 capacity = 0);

    /// <summary>
    /// Represents a mutable buffer which can be used to perform text shaping.
    /// </summary>
    public abstract class TextShaper : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextShaper"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="capacity">The initial capacity of the text builder.</param>
        public TextShaper(UltravioletContext uv, Int32 capacity)
            : base(uv)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="TextShaper"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity of the text builder.</param>
        /// <returns>The instance of <see cref="TextShaper"/> that was created.</returns>
        public static TextShaper Create(Int32 capacity = 0)
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<TextShaperFactory>()(uv, capacity);
        }

        /// <summary>
        /// Clears the buffer's contents and resets its properties.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Clears the buffer's contents.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Attempts to guess the script, language, and direction of the buffer based on its current contents.
        /// </summary>
        public abstract void GuessUnicodeProperties();

        /// <summary>
        /// Sets the buffer's layout direction.
        /// </summary>
        /// <param name="direction">A <see cref="TextDirection"/> value which describes the buffer's layout direction.</param>
        public abstract void SetDirection(TextDirection direction);

        /// <summary>
        /// Sets the buffer's script type.
        /// </summary>
        /// <param name="script">A <see cref="TextScript"/> value which describes the buffer's script type.</param>
        public abstract void SetScript(TextScript script);

        /// <summary>
        /// Gets the buffer's language.
        /// </summary>
        /// <param name="language">An ISO 639 language code which identifies the buffer's language.</param>
        public abstract void SetLanguage(String language);

        /// <summary>
        /// Gets the buffer's layout direction.
        /// </summary>
        /// <returns>A <see cref="TextDirection"/> value which describes the buffer's layout direction.</returns>
        public abstract TextDirection GetDirection();

        /// <summary>
        /// Gets the buffer's script type.
        /// </summary>
        /// <returns>A <see cref="TextScript"/> value which describes the buffer's script type.</returns>
        public abstract TextScript GetScript();

        /// <summary>
        /// Gets the buffer's language.
        /// </summary>
        /// <returns>An ISO 639 language code which identifies the buffer's language.</returns>
        public abstract String GetLanguage();

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        public abstract void Append(String str);

        /// <summary>
        /// Appends a substring of the specified UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <param name="start">The index of the first character in <paramref name="str"/> to append.</param>
        /// <param name="length">The number of characters from <paramref name="str"/> to append.</param>
        public abstract void Append(String str, Int32 start, Int32 length);

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        public abstract void Append(StringBuilder str);

        /// <summary>
        /// Appends a substring of the specified UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <param name="start">The index of the first character in <paramref name="str"/> to append.</param>
        /// <param name="length">The number of characters from <paramref name="str"/> to append.</param>
        public abstract void Append(StringBuilder str, Int32 start, Int32 length);

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        public abstract void Append(StringSegment str);

        /// <summary>
        /// Gets or sets the number of glyphs in the text builder.
        /// </summary>
        public abstract Int32 Length { get; set; }
    }
}
