using System;
using System.Security;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Represent a pointer to an unmanaged string where each character is 8 bits.
    /// </summary>
    public struct StringPtr8 : IEquatable<StringPtr8>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringPtr8"/> structure from the specified <c>null</c>-terminated string.
        /// </summary>
        /// <param name="ptr">A pointer to the <c>null</c>-terminated string data.</param>
        [SecurityCritical]
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
        [SecurityCritical]
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
        [SecuritySafeCritical]
        public static explicit operator IntPtr(StringPtr8 ptr)
        {
            return ptr.ptr;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="ptr1">The first object to compare.</param>
        /// <param name="ptr2">The second object to compare.</param>
        /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
        [SecuritySafeCritical]
        public static Boolean operator ==(StringPtr8 ptr1, StringPtr8 ptr2)
        {
            return ptr1.Equals(ptr2);
        }

        /// <summary>
        /// Determines whether the specified objects are unequal.
        /// </summary>
        /// <param name="ptr1">The first object to compare.</param>
        /// <param name="ptr2">The second object to compare.</param>
        /// <returns><c>true</c> if the specified objects are unequal; otherwise, <c>false</c>.</returns>
        [SecuritySafeCritical]
        public static Boolean operator !=(StringPtr8 ptr1, StringPtr8 ptr2)
        {
            return !ptr1.Equals(ptr2);
        }

        /// <summary>
        /// Gets the instance's hash code.
        /// </summary>
        /// <returns>The instance's hash code.</returns>
        [SecuritySafeCritical]
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 29 + ptr.GetHashCode();
                hash = hash * 29 + length.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        [SecuritySafeCritical]
        public override String ToString()
        {
            unsafe { return new String((sbyte*)ptr, 0, length); }
        }

        /// <summary>
        /// Determines whether the specified object is equal to this object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns><c>true</c> ifthis object is equal to the specified object; otherwise, <c>false</c>.</returns>
        [SecuritySafeCritical]
        public override Boolean Equals(Object obj)
        {
            return obj is StringPtr8 && Equals((StringPtr8)obj);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns><c>true</c> ifthis object is equal to the specified object; otherwise, <c>false</c>.</returns>
        [SecuritySafeCritical]
        public Boolean Equals(StringPtr8 obj)
        {
            return obj.ptr == this.ptr && obj.length == this.length;
        }

        /// <summary>
        /// Converts the string pointer to a pointer to an unspecified type.
        /// </summary>
        /// <returns>A pointer to the string data.</returns>
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public unsafe void* ToPointer()
        {
            return ptr.ToPointer();
        }

        /// <summary>
        /// Gets a <c>null</c> string pointer.
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
            [SecuritySafeCritical]
            get
            {
                Contract.EnsureRange(index >= 0 && index < length, "index");
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
        [SecurityCritical]
        private readonly IntPtr ptr;
        private readonly Int32 length;
    }
}
