using System;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a structure which can store any .NET primitive value type.
    /// </summary>
    public unsafe struct PrimitiveUnion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Byte value)
        {
            data = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        [CLSCompliant(false)]
        public PrimitiveUnion(SByte value)
        {
            data = *(Byte*)&value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Char value)
        {
            data = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Int16 value)
        {
            data = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        [CLSCompliant(false)]
        public PrimitiveUnion(UInt16 value)
        {
            data = *(Int16*)&value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Int32 value)
        {
            data = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        [CLSCompliant(false)]
        public PrimitiveUnion(UInt32 value)
        {
            data = *(Int32*)&value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Int64 value)
        {
            data = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        [CLSCompliant(false)]
        public PrimitiveUnion(UInt64 value)
        {
            data = *(Int64*)&value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Single value)
        {
            data = *(Int32*)&value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveUnion"/> structure.
        /// </summary>
        /// <param name="value">The value to store within the parameter.</param>
        public PrimitiveUnion(Double value)
        {
            data = *(Int64*)&value;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="Byte"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Byte AsByte()
        {
            fixed (Int64* pData = &data)
                return *(Byte*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="SByte"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public SByte AsSByte()
        {
            fixed (Int64* pData = &data)
                return *(SByte*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="Char"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Char AsChar()
        {
            fixed (Int64* pData = &data)
                return *(Char*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="Int16"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int16 AsInt16()
        {
            fixed (Int64* pData = &data)
                return *(Int16*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="UInt16"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt16 AsUInt16()
        {
            fixed (Int64* pData = &data)
                return *(UInt16*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="Int32"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int32 AsInt32()
        {
            fixed (Int64* pData = &data)
                return *(Int32*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="UInt32"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt32 AsUInt32()
        {
            fixed (Int64* pData = &data)
                return *(UInt32*)pData;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="Int64"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int64 AsInt64()
        {
            return data;
        }

        /// <summary>
        /// Converts the parameter to an <see cref="UInt64"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt64 AsUInt64()
        {
            fixed (Int64* pData = &data)
                return *(UInt64*)pData;
        }

        /// <summary>
        /// Converts the parameter to a <see cref="Single"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Single AsSingle()
        {
            fixed (Int64* pData = &data)
                return *(Single*)pData;
        }

        /// <summary>
        /// Converts the parameter to a <see cref="Double"/> value.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Double AsDouble()
        {
            fixed (Int64* pData = &data)
                return *(Double*)pData;
        }

        // State values.
        private Int64 data;
    }
}
