using System;
using System.Runtime.InteropServices;
using System.Security;
using System.IO;

namespace TwistedLogik.Nucleus.Collections.Specialized
{
    /// <summary>
    /// Represents a variable-length stream of value-type objects which are accessed via pointers.
    /// </summary>
    [SecurityCritical]
    public unsafe class UnsafeObjectStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeObjectStream"/> class with default values for its capacities.
        /// </summary>
        public UnsafeObjectStream()
            : this(0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeObjectStream"/> class with the specified capacities.
        /// </summary>
        /// <param name="capacityInObjects">The stream's capacity in number of objects.</param>
        /// <param name="capacityInBytes">The stream's capacity in bytes.</param>
        public UnsafeObjectStream(Int32 capacityInObjects, Int32 capacityInBytes)
        {
            Contract.EnsureRange(capacityInObjects >= 0, "capacityInObjects");
            Contract.EnsureRange(capacityInBytes >= 0, "capacityInBytes");

            this.index = (capacityInObjects == 0) ? EmptyIndex : new Int32[capacityInObjects];
            this.data = (capacityInBytes == 0) ? EmptyData : new Byte[capacityInBytes];
        }

        /// <summary>
        /// Removes all data from the stream.
        /// </summary>
        public void Clear()
        {
            lengthInBytes = 0;
            lengthInObjects = 0;

            position = 0;

            if (hasAcquiredPointers)
            {
                ReleasePointers();
                AcquirePointers();
            }
        }

        /// <summary>
        /// Prepares the stream for reading or writing by acquiring pointers to its underlying buffers.
        /// While pointers are acquired, these buffers will be pinned in memory.
        /// </summary>
        public void AcquirePointers()
        {
            Contract.EnsureNot(hasAcquiredPointers, NucleusStrings.UnsafeStreamHasAlreadyAcquiredPointers);

            hasAcquiredPointers = true;

            gcData = GCHandle.Alloc(data, GCHandleType.Pinned);
            pData0 = (Byte*)gcData.AddrOfPinnedObject().ToPointer();
            pData = pData0 + position;
        }

        /// <summary>
        /// Releases the pointers which were acquired by <see cref="AcquirePointers()"/> and unpins
        /// the stream's underlying buffers.
        /// </summary>
        public void ReleasePointers()
        {
            Contract.Ensure(hasAcquiredPointers, NucleusStrings.UnsafeStreamMustAcquirePointers);

            hasAcquiredPointers = false;

            gcData.Free();
            pData0 = null;
            pData = null;
        }

        /// <summary>
        /// Moves the stream's data pointer to the specified position in bytes.
        /// </summary>
        /// <param name="offset">The offset from the seek origin (in bytes) to which the stream's data pointer will be moved.</param>
        /// <param name="origin">A <see cref="SeekOrigin"/> value specifying the origin of the seek operation.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        [CLSCompliant(false)]
        public void* Seek(Int32 offset, SeekOrigin origin)
        {
            Contract.Ensure(hasAcquiredPointers, NucleusStrings.UnsafeStreamMustAcquirePointers);

            var target = GetPositionFromSeek(offset, origin);
            if (target < 0 || target > lengthInBytes)
                throw new ArgumentOutOfRangeException("offset");
            
            position = target;
            pData = pData0 + target;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the object with the specified index.
        /// </summary>
        /// <param name="offset">The index of the object to which the stream's data pointer will be moved.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        [CLSCompliant(false)]
        public void* SeekObject(Int32 offset)
        {
            Contract.Ensure(hasAcquiredPointers, NucleusStrings.UnsafeStreamMustAcquirePointers);
            Contract.EnsureRange(offset >= 0 && offset <= lengthInObjects, "offset");

            position = (offset == lengthInObjects) ? lengthInBytes : index[offset];
            pData = pData0 + position;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the specified position in bytes.
        /// </summary>
        /// <param name="offset">The offset from the seek origin (in bytes) to which the stream's data pointer will be moved.</param>
        /// <param name="origin">A <see cref="SeekOrigin"/> value specifying the origin of the seek operation.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        public IntPtr SafeSeek(Int32 offset, SeekOrigin origin)
        {
            return (IntPtr)Seek(offset, origin);
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the object with the specified index.
        /// </summary>
        /// <param name="offset">The index of the object to which the stream's data pointer will be moved.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        public IntPtr SafeSeekObject(Int32 offset)
        {
            return (IntPtr)SeekObject(offset);
        }
        
        /// <summary>
        /// Reserves enough space in the stream for one additional object of the specified size.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes to reserve.</param>
        public void Reserve(Int32 numberOfBytes)
        {
            Contract.EnsureRange(numberOfBytes > 0, "numberOfBytes");

            EnsureIndexCapacity(lengthInObjects + 1);
            EnsureDataCapacity(lengthInBytes + numberOfBytes);
        }

        /// <summary>
        /// Reserves enough space in the stream for the specified number of additional objects with the
        /// specified total size in bytes.
        /// </summary>
        /// <param name="numberOfObjects">The number of objects to reserve.</param>
        /// <param name="numberOfBytes">The number of bytes to reserve.</param>
        public void ReserveMultiple(Int32 numberOfObjects, Int32 numberOfBytes)
        {
            Contract.Ensure(numberOfObjects > 0, "numberOfObjects");
            Contract.Ensure(numberOfBytes > 0, "numberOfBytes");

            EnsureIndexCapacity(lengthInObjects + numberOfObjects);
            EnsureDataCapacity(lengthInBytes + numberOfBytes);
        }

        /// <summary>
        /// Marks the current data pointer position as the beginning of an object and advances the data pointer by the specified number of bytes.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes by which to advance the pointer.</param>
        public void FinalizeObject(Int32 numberOfBytes)
        {
            Contract.EnsureRange(numberOfBytes > 0, "numberOfBytes");
            Contract.EnsureRange(position + numberOfBytes <= CapacityInBytes, "numberOfBytes");
            Contract.Ensure(lengthInObjects + 1 <= CapacityInBytes, NucleusStrings.BufferLengthExceeded);

            index[lengthInObjects++] = position;

            position += numberOfBytes;

            if (hasAcquiredPointers)
                pData = pData0 + position;

            lengthInBytes += numberOfBytes;
        }

        /// <summary>
        /// Gets a value indicating whether the stream is currently locked and accessible 
        /// </summary>
        public Boolean HasAcquiredPointers
        {
            get { return hasAcquiredPointers; }
        }

        /// <summary>
        /// Gets the number of objects in the stream.
        /// </summary>
        public Int32 LengthInObjects
        {
            get { return lengthInObjects; }
        }

        /// <summary>
        /// Gets the number of bytes in the stream.
        /// </summary>
        public Int32 LengthInBytes
        {
            get { return lengthInBytes; }
        }        
                
        /// <summary>
        /// Gets the stream's total capacity for objects.
        /// </summary>
        public Int32 CapacityInObjects
        {
            get { return index.Length; }
        }

        /// <summary>
        /// Gets the stream's total capacity for bytes.
        /// </summary>
        public Int32 CapacityInBytes
        {
            get { return data.Length; }
        }

        /// <summary>
        /// Gets a pointer to the stream's current position within its internal data buffer.
        /// </summary>
        public IntPtr Data
        {
            get
            {
                Contract.Ensure(hasAcquiredPointers, NucleusStrings.UnsafeStreamMustAcquirePointers);

                return (IntPtr)pData;
            }
        }

        /// <summary>
        /// Gets a pointer to the beginning of the stream's internal data buffer.
        /// </summary>
        public IntPtr Data0
        {
            get
            {
                Contract.Ensure(hasAcquiredPointers, NucleusStrings.UnsafeStreamMustAcquirePointers);

                return (IntPtr)pData0;
            }
        }

        /// <summary>
        /// Ensures that the data buffer has at least the specified capacity, and expands
        /// the buffer if it does not.
        /// </summary>
        private void EnsureDataCapacity(Int32 capacity)
        {
            if (capacity <= CapacityInBytes)
                return;

            var size = Math.Max(capacity, (data.Length == 0) ? DefaultDataCapacity : data.Length * 2);
            var temp = new Byte[size];
            if (lengthInBytes > 0)
            {
                Array.Copy(data, temp, lengthInBytes);
            }
            data = temp;

            if (hasAcquiredPointers)
            {
                ReleasePointers();
                AcquirePointers();
            }
        }

        /// <summary>
        /// Ensures that the index buffer has at least the specified capacity, and expands
        /// the buffer if it does not.
        /// </summary>
        private void EnsureIndexCapacity(Int32 capacity)
        {
            if (capacity <= CapacityInObjects)
                return;

            var size = Math.Max(capacity, (index.Length == 0) ? DefaultIndexCapacity : index.Length * 2);
            var temp = new Int32[size];
            if (lengthInObjects > 0)
            {
                Array.Copy(index, temp, lengthInObjects);
            }
            index = temp;
        }

        /// <summary>
        /// Gets the byte position to which the specified seek operation will move the stream.
        /// </summary>
        private Int32 GetPositionFromSeek(Int32 offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return offset;

                case SeekOrigin.Current:
                    return position + offset;

                case SeekOrigin.End:
                    return lengthInBytes + offset;

                default:
                    throw new ArgumentException("origin");
            }
        }

        // State values.
        private const Int32 DefaultDataCapacity = 256;
        private const Int32 DefaultIndexCapacity = 8;
        private static readonly Byte[] EmptyData = new Byte[0];
        private static readonly Int32[] EmptyIndex = new Int32[0];
        private GCHandle gcData;
        private Int32 position;
        private Byte* pData0;
        private Byte* pData;
        private Byte[] data;
        private Int32[] index;

        // Property values.
        private Boolean hasAcquiredPointers;
        private Int32 lengthInObjects;
        private Int32 lengthInBytes;
    }
}
