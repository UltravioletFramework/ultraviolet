using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represent a pointer to an unmanaged string where each character is 8 bits.
    /// </summary>
    public partial struct StringPtr8 : IEquatable<StringPtr8>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringPtr8"/> structure from the specified <see langword="null"/>-terminated string.
        /// </summary>
        /// <param name="ptr">A pointer to the <see langword="null"/>-terminated string data.</param>
        public StringPtr8(IntPtr ptr)
        {
            this.ptr = ptr;

            var length = 0;
            unsafe
            {
                var p = (sbyte*)ptr;
                while (*p++ != 0)
                    length++;
            }

            this.length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPtr8"/> structure.
        /// </summary>
        /// <param name="ptr">A pointer to the string data.</param>
        /// <param name="length">The number of characters in the string data.</param>
        public StringPtr8(IntPtr ptr, Int32 length)
        {
            this.ptr = ptr;
            this.length = length;
        }

        /// <summary>
        /// Converts the string pointer to an instance of <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="ptr">The string pointer to convert.</param>
        /// <returns>The converted pointer.</returns>
        public static explicit operator IntPtr(StringPtr8 ptr)
        {
            return ptr.ptr;
        }

        /// <inheritdoc/>
        public unsafe override String ToString() => new String((sbyte*)ptr, 0, length);

        /// <summary>
        /// Converts the string pointer to a pointer to an unspecified type.
        /// </summary>
        /// <returns>A pointer to the string data.</returns>
        [CLSCompliant(false)]
        public unsafe void* ToPointer() => ptr.ToPointer();

        /// <summary>
        /// Converts the string pointer to a pointer to an sequence of signed bytes.
        /// </summary>
        /// <returns>A pointer to the string data.</returns>
        [CLSCompliant(false)]
        public unsafe sbyte* ToTypedPointer() => (sbyte*)ptr.ToPointer();

        /// <summary>
        /// Gets a <see langword="null"/> string pointer.
        /// </summary>
        public static StringPtr8 Zero
        {
            get { return new StringPtr8(); }
        }

        /// <summary>
        /// Gets the character at the specified index within the string.
        /// </summary>
        /// <param name="index">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the string.</returns>
        public Char this[Int32 index]
        {
            get
            {
                Contract.EnsureRange(index >= 0 && index < length, nameof(index));
                unsafe
                {
                    return (Char)(*((sbyte*)ptr + index));
                }
            }
        }

        /// <summary>
        /// Gets the length of the string in characters.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
        }

        // Property values.
        private readonly IntPtr ptr;
        private readonly Int32 length;
    }
}
