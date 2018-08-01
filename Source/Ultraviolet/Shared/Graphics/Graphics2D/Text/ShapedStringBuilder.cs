using System;
using System.Runtime.CompilerServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a mutable, expandable buffer of <see cref="ShapedChar"/> values.
    /// </summary>
    /// <remarks>The <see cref="ShapedStringBuilder"/> class does not enforce that all of its source data has the same
    /// font, language, script, or direction; mixing and matching these properties within a single buffer will produce
    /// nonsensical results, so application code should perform these checks where necessary.</remarks>
    public sealed partial class ShapedStringBuilder : ISegmentableShapedStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringBuilder"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity of this instance's buffer, in characters.</param>
        public ShapedStringBuilder(Int32 capacity = 0)
            : this(null, capacity)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringBuilder"/> class.
        /// </summary>
        /// <param name="value">The string that is used to initialize this instance.</param>
        /// <param name="capacity">The initial capacity of this instance's buffer, in characters.</param>
        public ShapedStringBuilder(ShapedString value, Int32 capacity = 0)
        {
            Contract.EnsureRange(capacity >= 0, nameof(capacity));

            if (capacity == 0)
                capacity = DefaultCapacity;

            this.buffer = new ShapedChar[capacity];

            if (value != null)
            {
                value.CopyTo(0, this.buffer, 0, value.Length);
                this.length = value.Length;
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="ShapedStringBuilder"/> to a new <see cref="ShapedString"/> instance.
        /// </summary>
        /// <param name="fontFace">The font face with which the string was created.</param>
        /// <param name="language">The name of the language which this string contains.</param>
        /// <param name="script">A <see cref="TextScript"/> value specifying which script which this string contains.</param>
        /// <param name="direction">A <see cref="TextDirection"/> value specifying the direction in which this string should be written.</param>
        /// <returns>The <see cref="ShapedString"/> instance which was created.</returns>
        public ShapedString ToShapedString(UltravioletFontFace fontFace, String language, TextScript script, TextDirection direction) =>
            new ShapedString(fontFace, language, script, direction, buffer, 0, length);

        /// <summary>
        /// Removes all characters from the current <see cref="StringBuilder"/> instance.
        /// </summary>
        /// <returns>A reference to this instance after the clear operation has completed.</returns>
        public ShapedStringBuilder Clear()
        {
            this.Length = 0;
            return this;
        }

        /// <summary>
        /// Appends a shaped character to the end of the current <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public ShapedStringBuilder Append(ShapedChar value)
        {
            EnsureCapacity(length + 1);
            buffer[length++] = value;
            return this;
        }

        /// <summary>
        /// Appends a specified number of copies of a shaped character to the end of the 
        /// current <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="repeatCount">The number of times to append <paramref name="repeatCount"/>.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public ShapedStringBuilder Append(ShapedChar value, Int32 repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                Append(value);

            return this;
        }

        /// <summary>
        /// Appends the contents of the specified <see cref="TextShaper"/> to the end of the 
        /// current <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        /// <param name="shaper">The <see cref="TextShaper"/> instance from which to append values.</param>
        /// <param name="fontFace">The font face with which to shape the string.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public ShapedStringBuilder Append(TextShaper shaper, UltravioletFontFace fontFace)
        {
            Contract.Require(shaper, nameof(shaper));

            shaper.AppendTo(this, fontFace);

            return this;
        }

        /// <summary>
        /// Copies the characters from a specified segment of this instance to a specified segment 
        /// of a destination <see cref="ShapedChar"/> array.
        /// </summary>
        /// <param name="sourceIndex">The starting position in this instance where characters will be copied from. The index is zero-based.</param>
        /// <param name="destination">The array where characters will be copied.</param>
        /// <param name="destinationIndex">The starting position in <paramref name="destination"/> where characters will be copied. The index is zero-based.</param>
        /// <param name="count">The number of characters to be copied.</param>
        public void CopyTo(Int32 sourceIndex, ShapedChar[] destination, Int32 destinationIndex, Int32 count)
        {
            Contract.EnsureRange(sourceIndex + count <= length, nameof(count));

            Array.Copy(buffer, sourceIndex, destination, destinationIndex, count);
        }

        /// <summary>
        /// Gets the <see cref="ShapedChar"/> object at the specified position 
        /// in the current <see cref="ShapedString"/> object.
        /// </summary>
        /// <param name="index">A position in the current string.</param>
        /// <returns>The object at position <paramref name="index"/>.</returns>
        [IndexerName("Chars")]
        public ShapedChar this[Int32 index]
        {
            get
            {
                Contract.EnsureRange(index < Length, nameof(index));

                return buffer[index];
            }
        }

        /// <summary>
        /// Gets or sets the length of the current <see cref="ShapedStringBuilder"/> object.
        /// </summary>
        public Int32 Length
        {
            get => length;
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                var difference = value - length;
                if (difference > 0)
                {
                    Append(new ShapedChar(), difference);
                }
                else
                {
                    length = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be contained in the 
        /// memory allocated by the current instance.
        /// </summary>
        public Int32 Capacity
        {
            get => buffer.Length;
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));
                Contract.EnsureRange(value >= Length, nameof(value));

                if (buffer.Length != value)
                {
                    var newbuf = new ShapedChar[value];
                    Array.Copy(buffer, 0, newbuf, 0, Length);
                    buffer = newbuf;
                }
            }
        }

        /// <summary>
        /// Ensures that the instance's buffer has at least the specified capacity.
        /// </summary>
        private void EnsureCapacity(Int32 c)
        {
            if (buffer.Length >= c)
                return;

            var newlen = (buffer.Length * 3) / 2;
            var newbuf = new ShapedChar[newlen];
            Array.Copy(buffer, 0, newbuf, 0, length);
            buffer = newbuf;
        }

        // The buffer that contains the builder's character data.
        private const Int32 DefaultCapacity = 16;
        private ShapedChar[] buffer;
        private Int32 length;
    }
}
