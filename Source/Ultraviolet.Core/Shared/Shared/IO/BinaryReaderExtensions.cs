using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Core.IO
{
    /// <summary>
    /// Contains extension methods for the <see cref="BinaryReader"/> class.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a list from the stream.
        /// </summary>
        /// <typeparam name="T">The type of items in the list being read.</typeparam>
        /// <param name="reader">The <see cref="BinaryReader"/> with which to read the list.</param>
        /// <param name="list">The list to write.</param>
        /// <param name="method">A function which selects the method with which to read the list's items.</param>
        public static void ReadList<T>(this BinaryReader reader, IList<T> list, Func<BinaryReader, Func<T>> method)
        {
            Contract.Require(list, nameof(list));
            Contract.Require(method, nameof(method));

            var methodToInvoke = method(reader);
            if (methodToInvoke == null)
                throw new InvalidOperationException(CoreStrings.InvalidWriteMethod);

            list.Clear();

            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var item = methodToInvoke();
                list.Add(item);
            }
        }

        /// <summary>
        /// Reads a <see langword="null"/>able 16-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        public static Int16? ReadNullableInt16(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadInt16();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able 32-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        public static Int32? ReadNullableInt32(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadInt32();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able 64-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        public static Int64? ReadNullableInt64(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadInt64();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able unsigned 16-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        [CLSCompliant(false)]
        public static UInt16? ReadNullableUInt16(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadUInt16();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able unsigned 32-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        [CLSCompliant(false)]
        public static UInt32? ReadNullableUInt32(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadUInt32();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able unsigned 64-bit integer from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        [CLSCompliant(false)]
        public static UInt64? ReadNullableUInt64(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadUInt64();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able single-precision floating point value from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        public static Single? ReadNullableSingle(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadSingle();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see langword="null"/>able double-precision floating point value from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the value.</param>
        /// <returns>The value that was read.</returns>
        public static Double? ReadNullableDouble(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadDouble();
            }
            return null;
        }

        /// <summary>
        /// Reads a <see cref="Guid"/> from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the <see cref="Guid"/>.</param>
        /// <returns>The <see cref="Guid"/> that was read from the stream.</returns>
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        /// <summary>
        /// Reads a <see langword="null"/>able GUID from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the GUID.</param>
        /// <returns>The GUID that was read from the stream.</returns>
        public static Guid? ReadNullableGuid(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadGuid();
            }
            return null;
        }

        /// <summary>
        /// Reads a resolved reference to a Ultraviolet data object from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the data object reference.</param>
        /// <returns>The <see cref="ResolvedDataObjectReference"/> that was read from the stream.</returns>
        public static ResolvedDataObjectReference ReadResolvedDataObjectReference(this BinaryReader reader)
        {
            var value = reader.ReadGuid();
            var source = (string)null;
            var hasSource = reader.ReadBoolean();
            if (hasSource)
            {
                source = reader.ReadString();
            }
            return new ResolvedDataObjectReference(value, source);
        }

        /// <summary>
        /// Reads a <see langword="null"/>able resolved reference to an Ultraviolet data object from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the data object reference.</param>
        /// <returns>The <see cref="Nullable{ResolvedDataObjectReference}"/> that was read from the stream.</returns>
        public static ResolvedDataObjectReference? ReadNullableResolvedDataObjectReference(this BinaryReader reader)
        {
            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadResolvedDataObjectReference();
            }
            return null;
        }
    }
}
