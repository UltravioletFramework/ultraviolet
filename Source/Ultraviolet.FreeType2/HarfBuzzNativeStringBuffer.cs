using System;
using System.Runtime.InteropServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents a string buffer that lives in native memory.
    /// </summary>
    internal unsafe class HarfBuzzNativeStringBuffer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarfBuzzNativeStringBuffer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="capacity">The initial capacity of the buffer, in characters.</param>
        public HarfBuzzNativeStringBuffer(UltravioletContext uv, Int32 capacity = 32)
            : base(uv)
        {
            EnsureCapacity(capacity);
        }

        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~HarfBuzzNativeStringBuffer()
        {
            if (Native != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Native);
                Native = IntPtr.Zero;
            }
        }

        /// <inheritdoc/>
        public override String ToString() => (Length == 0) ? String.Empty : Marshal.PtrToStringUni(Native);

        /// <summary>
        /// Clears the buffer's contents.
        /// </summary>
        public void Clear()
        {
            Length = 0;
            Version++;
        }

        /// <summary>
        /// Appends a character to the buffer.
        /// </summary>
        /// <param name="c">The character to append to the buffer.</param>
        public void Append(Char c)
        {
            EnsureCapacity(length + 1);

            ((Char*)Native)[length] = c;
            ((Char*)Native)[length + 1] = '\0';
            length++;
            Version++;
        }

        /// <summary>
        /// Appends the contents of a string to the buffer.
        /// </summary>
        /// <param name="str">The string to append to the buffer.</param>
        public void Append(String str)
        {
            Contract.Require(str, nameof(str));

            Append(new StringSegment(str));
        }

        /// <summary>
        /// Appends the contents of a string builder to the buffer.
        /// </summary>
        /// <param name="str">The string builder to append to the buffer.</param>
        public void Append(StringBuilder str)
        {
            Contract.Require(str, nameof(str));

            Append(new StringSegment(str));
        }

        /// <summary>
        /// Appends the contents of a string segment to the buffer.
        /// </summary>
        /// <param name="str">The string segment to append to the buffer.</param>
        public void Append(StringSegment str)
        {
            if (str.IsEmpty)
                return;

            EnsureCapacity(length + str.Length);

            for (int i = 0; i < str.Length; i++)
            {
                ((Char*)Native)[length + i] = str[i];
            }
            length = length + str.Length;
            Version++;

            ((Char*)Native)[length] = '\0';
        }

        /// <summary>
        /// Gets the character at the specified index within the buffer.
        /// </summary>
        /// <param name="ix">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the buffer.</returns>
        public Char this[Int32 ix]
        {
            get
            {
                if (ix < 0 || ix > length)
                    throw new ArgumentOutOfRangeException(nameof(ix));

                return ((Char*)Native)[ix];
            }
        }

        /// <summary>
        /// Gets a version number which is incremented every time the buffer's content is changed.
        /// </summary>
        public Int64 Version { get; private set; } = 1;

        /// <summary>
        /// Gets the total capacity of the native buffer in characters.
        /// </summary>
        public Int32 Capacity
        {
            get => capacity;
            set
            {
                capacity = Math.Max(value, 1);

                var newbuf = Marshal.AllocHGlobal(capacity * sizeof(Char));
                ((Char*)newbuf)[capacity - 1] = '\0';

                if (Native != IntPtr.Zero)
                {
                    if (Length > 0)
                    {
                        Buffer.MemoryCopy((void*)Native, (void*)newbuf, capacity * sizeof(Char), length * sizeof(Char));
                    }
                    Marshal.FreeHGlobal(Native);
                }

                Native = newbuf;
            }
        }

        /// <summary>
        /// Gets the total length of the native buffer in characters.
        /// </summary>
        public Int32 Length
        {
            get => length;
            set
            {
                var change = value - length;
                if (change > 0)
                {
                    EnsureCapacity(value);
                    for (int i = 0; i < change; i++)
                    {
                        ((Char*)Native)[length + i] = '\0';
                    }
                }
                else
                {
                    if (value > 0)
                    {
                        ((Char*)Native)[value] = '\0';
                    }
                }
                length = value;
            }
        }

        /// <summary>
        /// Gets the pointer to the native buffer.
        /// </summary>
        public IntPtr Native { get; private set; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Native != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Native);
                Native = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Ensures that the buffer has at least the specified capacity in characters.
        /// </summary>
        private void EnsureCapacity(Int32 desiredCapacityInChars)
        {
            // Remember to account for the null terminator
            desiredCapacityInChars++;

            if (capacity >= desiredCapacityInChars)
                return;

            Capacity = Math.Max(desiredCapacityInChars, (2 * capacity) / 3);
        }

        private Int32 capacity;
        private Int32 length;
    }
}
