using System;
using System.Text;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="StringBuilder"/> class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the contents of a <see cref="VersionedStringSource"/> to the end of the string builder.
        /// </summary>
        /// <param name="this">The <see cref="StringBuilder"/> instance.</param>
        /// <param name="value">The <see cref="VersionedStringSource"/> to append to the string builder.</param>
        /// <returns>The current <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder AppendVersionedStringSource(this StringBuilder @this, VersionedStringSource value)
        {
            Contract.Require(@this, "this");

            if (value.IsValid)
            {
                if (value.IsSourcedFromString)
                {
                    var source = (String)value;
                    @this.Append(source);
                }
                else
                {
                    var source = (VersionedStringBuilder)value;
                    for (int i = 0; i < source.Length; i++)
                    {
                        @this.Append(source[i]);
                    }
                }
            }

            return @this;
        }

        /// <summary>
        /// Inserts the contents of a <see cref="VersionedStringSource"/> into the string builder at the specified position.
        /// </summary>
        /// <param name="this">The <see cref="StringBuilder"/> instance.</param>
        /// <param name="index">The index at which to begin inserting the value.</param>
        /// <param name="value">The <see cref="VersionedStringSource"/> to insert into the string builder.</param>
        /// <returns>The current <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder InsertVersionedStringSource(this StringBuilder @this, Int32 index, VersionedStringSource value)
        {
            Contract.Require(@this, "this");
            Contract.EnsureRange(index >= 0 && index <= @this.Length, nameof(index));

            if (value.IsValid)
            {
                if (value.IsSourcedFromString)
                {
                    var source = (String)value;
                    @this.Insert(index, source);
                }
                else
                {
                    var source = (VersionedStringBuilder)value;
                    for (int i = 0; i < source.Length; i++)
                    {
                        @this.Insert(index + i, source[i]);
                    }
                }
            }

            return @this;
        }
    }
}
