using System;
using System.Text;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a wrapper around a <see cref="StringBuilder"/> which maintains a version number that is incremented
    /// every time the underlying buffer is changed.
    /// </summary>
    public class VersionedStringBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringBuilder"/> class.
        /// </summary>
        public VersionedStringBuilder()
        {
            this.stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringBuilder"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity of the string builder.</param>
        public VersionedStringBuilder(Int32 capacity)
        {
            this.stringBuilder = new StringBuilder(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringBuilder"/> class.
        /// </summary>
        /// <param name="value">The initial value of the string builder.</param>
        public VersionedStringBuilder(String value)
        {
            this.stringBuilder = new StringBuilder(value);
        }

        /// <summary>
        /// Explicitly converts a <see cref="VersionedStringBuilder"/> instance to a <see cref="String"/> instance.
        /// </summary>
        /// <param name="vsb">The <see cref="VersionedStringBuilder"/> to convert.</param>
        /// <returns>The <see cref="String"/> instance that was created.</returns>
        public static explicit operator String(VersionedStringBuilder vsb)
        {
            return vsb.ToString();
        }

        /// <summary>
        /// Explicitly converts a <see cref="VersionedStringBuilder"/> instance to a <see cref="StringBuilder"/> instance.
        /// </summary>
        /// <param name="vsb">The <see cref="VersionedStringBuilder"/> to convert.</param>
        /// <returns>The <see cref="StringBuilder"/> instance that was created.</returns>
        public static explicit operator StringBuilder(VersionedStringBuilder vsb)
        {
            return vsb.stringBuilder;
        }

        /// <summary>
        /// Converts the string builder to a <see cref="String"/> instance.
        /// </summary>
        /// <returns>The string that was created.</returns>
        public override String ToString()
        {
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts a substring of the string builder to a <see cref="String"/> instance.
        /// </summary>
        /// <param name="startIndex">The index of the first character in the substring to convert.</param>
        /// <param name="length">The number of characters in the substring to convert.</param>
        /// <returns>The string that was created.</returns>
        public String ToString(Int32 startIndex, Int32 length)
        {
            return stringBuilder.ToString(startIndex, length);
        }

        /// <summary>
        /// Clears the string builder.
        /// </summary>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Clear()
        {
            stringBuilder.Clear();
            version++;
            return this;
        }

        /// <summary>
        /// Appends a character at the end of the string builder.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Append(String value)
        {
            if (value != null)
            {
                stringBuilder.Append(value);
                version++;
            }
            return this;
        }

        /// <summary>
        /// Appends the contents of the specified string builder at the end of the string builder.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Append(StringBuilder value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    stringBuilder.Append(value[i]);
                }
                version++;
            }
            return this;
        }

        /// <summary>
        /// Appends the contents of the specified string builder at the end of the string builder.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Append(VersionedStringBuilder value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    stringBuilder.Append(value[i]);
                }
                version++;
            }
            return this;
        }

        /// <summary>
        /// Appends the contents of the specified string source at the end of the string builder.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Append(VersionedStringSource value)
        {
            if (value.IsValid)
            {
                if (value.IsSourcedFromString)
                {
                    Append((String)value);
                }
                else
                {
                    Append((VersionedStringBuilder)value);
                }
            }
            return this;
        }

        /// <summary>
        /// Appends a string at the end of the string builder.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Append(Char value)
        {
            stringBuilder.Append(value);
            version++;
            return this;
        }

        /// <summary>
        /// Inserts a string at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index at which to insert the value.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Insert(Int32 index, String value)
        {
            if (value != null)
            {
                stringBuilder.Insert(index, value);
                version++;
            }
            else
            {
                if (index < 0 || index > stringBuilder.Length)
                    throw new ArgumentOutOfRangeException("index");
            }
            return this;
        }

        /// <summary>
        /// Inserts the contents of the specified string builder at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index at which to insert the value.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Insert(Int32 index, StringBuilder value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    stringBuilder.Insert(index + i, value[i]);
                }
                version++;
            }
            else
            {
                if (index < 0 || index > stringBuilder.Length)
                    throw new ArgumentOutOfRangeException("index");
            }
            return this;
        }

        /// <summary>
        /// Inserts the contents of the specified string builder at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index at which to insert the value.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Insert(Int32 index, VersionedStringBuilder value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    stringBuilder.Insert(index + i, value[i]);
                }
                version++;
            }
            else
            {
                if (index < 0 || index > stringBuilder.Length)
                    throw new ArgumentOutOfRangeException("index");
            }
            return this;
        }

        /// <summary>
        /// Inserts the contents of the specified string source at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index at which to insert the value.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Insert(Int32 index, VersionedStringSource value)
        {
            if (value.IsValid)
            {
                if (value.IsSourcedFromString)
                {
                    Insert(index, (String)value);
                }
                else
                {
                    Insert(index, (VersionedStringBuilder)value);
                }
            }
            else
            {
                if (index < 0 || index > stringBuilder.Length)
                    throw new ArgumentOutOfRangeException("index");
            }
            return this;
        }

        /// <summary>
        /// Inserts a character at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index at which to insert the value.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Insert(Int32 index, Char value)
        {
            stringBuilder.Insert(index, value);
            version++;
            return this;
        }

        /// <summary>
        /// Removes the specified character range from the string builder.
        /// </summary>
        /// <param name="startIndex">The index of the first character to remove.</param>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>The current instance of <see cref="VersionedStringBuilder"/>.</returns>
        public VersionedStringBuilder Remove(Int32 startIndex, Int32 length)
        {
            stringBuilder.Remove(startIndex, length);
            version++;
            return this;
        }

        /// <summary>
        /// Gets or sets the character at the specified index within the string builder.
        /// </summary>
        /// <param name="index">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the string builder.</returns>
        public Char this[Int32 index]
        {
            get { return stringBuilder[index]; }
            set
            {
                stringBuilder[index] = value;
                version++;
            }
        }

        /// <summary>
        /// Gets the string builder's current version number.
        /// </summary>
        public Int64 Version
        {
            get { return version; }
        }

        /// <summary>
        /// Gets or sets the string builder's length.
        /// </summary>
        public Int32 Length
        {
            get { return stringBuilder.Length; }
            set
            {
                stringBuilder.Length = value;
                version++;
            }
        }

        // The StringBuilder which is wrapped by this instance.
        private readonly StringBuilder stringBuilder;
        private Int64 version;        
    }
}
