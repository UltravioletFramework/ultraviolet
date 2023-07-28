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
        public TextShaper(UltravioletContext uv)
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
        /// Clears the buffer's contents.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Sets the buffer's Unicode properties.
        /// </summary>
        /// <param name="direction">The buffer's layout direction.</param>
        /// <param name="script">The buffer's script type.</param>
        /// <param name="language">The buffer's language.</param>
        public abstract void SetUnicodeProperties(TextDirection direction, TextScript script, String language);

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
        /// Gets the buffer's Unicode properties.
        /// </summary>
        /// <param name="direction">The buffer's layout direction.</param>
        /// <param name="script">The buffer's script type.</param>
        /// <param name="language">The buffer's language.</param>
        public abstract void GetUnicodeProperties(out TextDirection direction, out TextScript script, out String language);

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
        /// Appends a UTF-16 encoded character to the end of the buffer.
        /// </summary>
        /// <param name="c">The character to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(Char c);

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(String str);

        /// <summary>
        /// Appends a substring of the specified UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <param name="start">The index of the first character in <paramref name="str"/> to append.</param>
        /// <param name="length">The number of characters from <paramref name="str"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(String str, Int32 start, Int32 length);

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(StringBuilder str);

        /// <summary>
        /// Appends a substring of the specified UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <param name="start">The index of the first character in <paramref name="str"/> to append.</param>
        /// <param name="length">The number of characters from <paramref name="str"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(StringBuilder str, Int32 start, Int32 length);

        /// <summary>
        /// Appends a UTF-16 encoded string to the end of the buffer.
        /// </summary>
        /// <param name="str">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public abstract TextShaper Append(StringSegment str);

        /// <summary>
        /// Appends the contents of this shaping buffer to the specified <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="ShapedStringBuilder"/> instance to which to append this shaper's contents.</param>
        /// <param name="fontFace">The font face with which to shape the string.</param>
        /// <param name="sourceIndexOffset">The offset which is applied to the source indices assigned to shaped characters in the resulting string.</param>
        public abstract void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace, Int32 sourceIndexOffset = 0);

        /// <summary>
        /// Appends the contents of a subset of this shaping buffer to the specified <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="ShapedStringBuilder"/> instance to which to append this shaper's contents.</param>
        /// <param name="fontFace">The font face with which to shape the string.</param>
        /// <param name="start">The offset of the character in the original string which corresponds to the beginning of the shaped substring.</param>
        /// <param name="length">The number of characters in the raw substring from which to create the shaped substring.</param>
        /// <param name="sourceIndexOffset">The offset which is applied to the source indices assigned to shaped characters in the resulting string.</param>
        public abstract void AppendTo(ShapedStringBuilder builder, UltravioletFontFace fontFace, Int32 start, Int32 length, Int32 sourceIndexOffset = 0);

        /// <summary>
        /// Creates a new <see cref="ShapedString"/> instance from the current contents of the shaping buffer.
        /// </summary>
        /// <param name="fontFace">The font face with which to shape the string.</param>
        /// <param name="sourceIndexOffset">The offset which is applied to the source indices assigned to shaped characters in the resulting string.</param>
        /// <returns>A new shaped string instance.</returns>
        public abstract ShapedString CreateShapedString(UltravioletFontFace fontFace, Int32 sourceIndexOffset = 0);

        /// <summary>
        /// Creates a new <see cref="ShapedString"/> instance from a subset of the current contents of the shaping buffer.
        /// </summary>
        /// <param name="fontFace">The font face with which to shape the string.</param>
        /// <param name="start">The offset of the character in the original string which corresponds to the beginning of the shaped substring.</param>
        /// <param name="length">The number of characters in the raw substring from which to create the shaped substring.</param>
        /// <param name="sourceIndexOffset">The offset which is applied to the source indices assigned to shaped characters in the resulting string.</param>
        /// <returns>A new shaped string instance.</returns>
        public abstract ShapedString CreateShapedString(UltravioletFontFace fontFace, Int32 start, Int32 length, Int32 sourceIndexOffset = 0);

        /// <summary>
        /// Gets the length of the raw string data which is currently contained by the shaper.
        /// </summary>
        public abstract Int32 RawLength { get; }
    }
}
