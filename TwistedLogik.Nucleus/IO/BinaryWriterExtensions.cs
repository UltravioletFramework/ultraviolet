using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.IO
{
    /// <summary>
    /// Contains extension methods for the BinaryWriter class.
    /// </summary>
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Writes a list to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to </param>
        /// <param name="list">The list to write.</param>
        /// <param name="method">A function which selects the method with which to write the list's items.</param>
        public static void Write<T>(this BinaryWriter writer, IList<T> list, Func<BinaryWriter, Action<T>> method)
        {
            Contract.Require(list, "list");
            Contract.Require(method, "method");

            var methodToInvoke = method(writer);
            if (methodToInvoke == null)
                throw new InvalidOperationException(NucleusStrings.InvalidWriteMethod);

            writer.Write(list.Count());
            foreach (var item in list)
            {
                methodToInvoke(item);
            }
        }

        /// <summary>
        /// Writes a nullable 16-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter writer, Int16? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable 32-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter writer, Int32? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable 64-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter writer, Int64? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable unsigned 16-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public static void Write(this BinaryWriter writer, UInt16? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable unsigned 32-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public static void Write(this BinaryWriter writer, UInt32? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable unsigned 64-bit integer to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public static void Write(this BinaryWriter writer, UInt64? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable single-precision floating point value to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter writer, Single? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a nullable double-precision floating point value to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the value.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter writer, Double? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue)
            {
                writer.Write(value.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a GUID to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the GUID.</param>
        /// <param name="guid">The GUID to write to the stream.</param>
        [SecuritySafeCritical]
        public static void Write(this BinaryWriter writer, Guid guid)
        {
            unsafe
            {
                byte* ptr = (byte*)&guid;
                for (int i = 0; i < 16; i++)
                    writer.Write(*ptr++);
            }
        }

        /// <summary>
        /// Writes a GUID to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the GUID.</param>
        /// <param name="guid">The GUID to write to the stream.</param>
        public static void Write(this BinaryWriter writer, Guid? guid)
        {
            writer.Write(guid.HasValue);
            if (guid.HasValue)
            {
                writer.Write(guid.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Writes a resolved Nucleus Data Object reference to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the data object reference.</param>
        /// <param name="reference">The data object reference to write to the stream.</param>
        public static void Write(this BinaryWriter writer, ResolvedDataObjectReference reference)
        {
            writer.Write(reference.Value);
            writer.Write(reference.Source != null);
            if (reference.Source != null)
            {
                writer.Write(reference.Source);
            }
        }

        /// <summary>
        /// Writes a resolved Nucleus Data Object reference to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the data object reference.</param>
        /// <param name="reference">The data object reference to write to the stream.</param>
        public static void Write(this BinaryWriter writer, ResolvedDataObjectReference? reference)
        {
            writer.Write(reference.HasValue);
            if (reference.HasValue)
            {
                writer.Write(reference.GetValueOrDefault());
            }
        }
    }
}
