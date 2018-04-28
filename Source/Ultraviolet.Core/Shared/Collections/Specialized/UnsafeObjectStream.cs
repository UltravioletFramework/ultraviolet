using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core.Collections.Specialized
{
    /// <summary>
    /// Represents a variable-length stream of value-type objects which are accessed via pointers.
    /// </summary>
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
            Contract.EnsureRange(capacityInObjects >= 0, nameof(capacityInObjects));
            Contract.EnsureRange(capacityInBytes >= 0, nameof(capacityInBytes));

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

            positionInObjects = 0;
            positionInBytes = 0;

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
            Contract.EnsureNot(hasAcquiredPointers, CoreStrings.UnsafeStreamHasAlreadyAcquiredPointers);

            hasAcquiredPointers = true;

            gcData = GCHandle.Alloc(data, GCHandleType.Pinned);
            pData0 = (Byte*)gcData.AddrOfPinnedObject().ToPointer();
            pData = pData0 + positionInBytes;
        }

        /// <summary>
        /// Releases the pointers which were acquired by <see cref="AcquirePointers()"/> and unpins
        /// the stream's underlying buffers.
        /// </summary>
        public void ReleasePointers()
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

            hasAcquiredPointers = false;

            gcData.Free();
            pData0 = null;
            pData = null;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the object with the specified index.
        /// </summary>
        /// <param name="offset">The index of the object to which the stream's data pointer will be moved.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        [CLSCompliant(false)]
        public void* RawSeekObject(Int32 offset)
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);
            Contract.EnsureRange(offset >= 0 && offset <= lengthInObjects, nameof(offset));

            positionInObjects = offset;
            positionInBytes = (offset == lengthInObjects) ? lengthInBytes : index[offset];
            pData = pData0 + positionInBytes;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the next object.
        /// </summary>
        /// <remarks>This method has no effect if the current stream position is at the end of the stream.</remarks>
        /// <returns>The stream's data pointer after moving to the next object.</returns>
        [CLSCompliant(false)]
        public void* RawSeekForward()
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

            if (positionInObjects == lengthInObjects)
                return pData;

            positionInObjects++;
            positionInBytes = (positionInObjects == lengthInObjects) ? lengthInBytes : index[positionInObjects];
            pData = pData0 + positionInBytes;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the previous object.
        /// </summary>
        /// <remarks>This method has no effect if the current stream position is at the beginning of the stream.</remarks>
        /// <returns>The stream's data pointer after moving to the previous object.</returns>
        [CLSCompliant(false)]
        public void* RawSeekBackward()
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

            if (positionInObjects == 0)
                return pData;

            positionInObjects--;
            positionInBytes = index[positionInObjects];
            pData = pData0 + positionInBytes;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the stream.
        /// </summary>
        /// <returns>The stream's data pointer after moving to the beginning of the stream.</returns>
        [CLSCompliant(false)]
        public void* RawSeekBeginning()
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

            positionInObjects = 0;
            positionInBytes = 0;
            pData = pData0;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the end of the stream.
        /// </summary>
        /// <returns>The stream's data pointer after moving to the end of the stream.</returns>
        [CLSCompliant(false)]
        public void* RawSeekEnd()
        {
            Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

            positionInObjects = lengthInObjects;
            positionInBytes = lengthInBytes;
            pData = pData0 + lengthInBytes;

            return pData;
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the object with the specified index.
        /// </summary>
        /// <param name="offset">The index of the object to which the stream's data pointer will be moved.</param>
        /// <returns>The stream's data pointer after moving to the specified position.</returns>
        public IntPtr SeekObject(Int32 offset)
        {
            return (IntPtr)RawSeekObject(offset);
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the next object.
        /// </summary>
        /// <remarks>This method has no effect if the current stream position is at the end of the stream.</remarks>
        /// <returns>The stream's data pointer after moving to the next object.</returns>
        [CLSCompliant(false)]
        public IntPtr SeekForward()
        {
            return (IntPtr)RawSeekForward();
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the previous object.
        /// </summary>
        /// <remarks>This method has no effect if the current stream position is at the beginning of the stream.</remarks>
        /// <returns>The stream's data pointer after moving to the previous object.</returns>
        [CLSCompliant(false)]
        public IntPtr SeekBackward()
        {
            return (IntPtr)RawSeekBackward();
        }

        /// <summary>
        /// Moves the stream's data pointer to the beginning of the stream.
        /// </summary>
        /// <returns>The stream's data pointer after moving to the beginning of the stream.</returns>
        [CLSCompliant(false)]
        public IntPtr SeekBeginning()
        {
            return (IntPtr)RawSeekBeginning();
        }

        /// <summary>
        /// Moves the stream's data pointer to the end of the stream.
        /// </summary>
        /// <returns>The stream's data pointer after moving to the end of the stream.</returns>
        [CLSCompliant(false)]
        public IntPtr SeekEnd()
        {
            return (IntPtr)RawSeekEnd();
        }

        /// <summary>
        /// Reserves enough space in the stream for one additional object of the specified size.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes to reserve.</param>
        public void Reserve(Int32 numberOfBytes)
        {
            Contract.EnsureRange(numberOfBytes > 0, nameof(numberOfBytes));
            Contract.Ensure(positionInBytes == lengthInBytes, CoreStrings.UnsafeStreamCanOnlyReserveAtStreamEnd);

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
            Contract.Ensure(numberOfObjects > 0, nameof(numberOfObjects));
            Contract.Ensure(numberOfBytes > 0, nameof(numberOfBytes));
            Contract.Ensure(positionInBytes == lengthInBytes, CoreStrings.UnsafeStreamCanOnlyReserveAtStreamEnd);

            EnsureIndexCapacity(lengthInObjects + numberOfObjects);
            EnsureDataCapacity(lengthInBytes + numberOfBytes);
        }

        /// <summary>
        /// Reserves enough space in the stream for the specified number of additional objects with the
        /// specified total size in bytes. The reserved space will be inserted into the stream prior to
        /// the object at the stream's current position.
        /// </summary>
        /// <param name="numberOfObjects">The number of objects to reserve.</param>
        /// <param name="numberOfBytes">The number of bytes to reserve.</param>
        public void ReserveInsert(Int32 numberOfObjects, Int32 numberOfBytes)
        {
            ReserveInsert(positionInObjects, numberOfObjects, numberOfBytes);
        }

        /// <summary>
        /// Reserves enough space in the stream for the specified number of additional objects with the
        /// specified total size in bytes. The reserved space will be inserted into the stream prior to
        /// the object at the specified index. The stream's current position will be moved to the beginning
        /// of the reserved data.
        /// </summary>
        /// <param name="insertPosition">The index at which to insert the reserved space, from 0 (the
        /// beginning of the stream) to LengthInObjects, inclusive (the end of the stream).</param>
        /// <param name="numberOfObjects">The number of objects to reserve.</param>
        /// <param name="numberOfBytes">The number of bytes to reserve.</param>
        public void ReserveInsert(Int32 insertPosition, Int32 numberOfObjects, Int32 numberOfBytes)
        {
            Contract.Ensure(insertPosition >= 0 && insertPosition <= lengthInObjects, nameof(insertPosition));
            Contract.Ensure(numberOfObjects > 0, nameof(numberOfObjects));
            Contract.Ensure(numberOfBytes > 0, nameof(numberOfBytes));

            var insert = (insertPosition < lengthInObjects);
            var insertPositionInBytes = insert ? index[insertPosition] : lengthInBytes;

            EnsureIndexCapacity(lengthInObjects + numberOfObjects);
            EnsureDataCapacity(lengthInBytes + numberOfBytes);

            RawSeekObject(insertPosition);
            
            if (insert)
            {
                var indexCopyOffset = insertPosition;
                var indexCopyLength = lengthInObjects - indexCopyOffset;
                Array.Copy(index, indexCopyOffset, index, indexCopyOffset + numberOfObjects, indexCopyLength);
                Array.Clear(index, indexCopyOffset, numberOfObjects);

                var dataCopyOffset = insertPositionInBytes;
                var dataCopyLength = lengthInBytes - dataCopyOffset;
                Array.Copy(data, dataCopyOffset, data, dataCopyOffset + numberOfBytes, dataCopyLength);

                for (int i = indexCopyOffset + numberOfObjects; i < lengthInObjects + numberOfObjects; i++)
                    index[i] += numberOfBytes;
            }
        }

        /// <summary>
        /// Marks the current data pointer position as the beginning of an object and advances the data pointer by the specified number of bytes.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes by which to advance the pointer.</param>
        public void FinalizeObject(Int32 numberOfBytes)
        {
            Contract.EnsureRange(numberOfBytes > 0, nameof(numberOfBytes));
            Contract.EnsureRange(positionInBytes + numberOfBytes <= CapacityInBytes, nameof(numberOfBytes));
            Contract.Ensure(lengthInObjects + 1 <= CapacityInBytes, CoreStrings.BufferLengthExceeded);

            if (positionInObjects < lengthInObjects && index[positionInObjects] != 0)
                throw new InvalidOperationException(CoreStrings.UnsafeStreamWouldOverwriteObject);

            index[positionInObjects++] = positionInBytes;
            positionInBytes += numberOfBytes;

            if (hasAcquiredPointers)
                pData = pData0 + positionInBytes;

            lengthInObjects++;
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
                Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

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
                Contract.Ensure(hasAcquiredPointers, CoreStrings.UnsafeStreamMustAcquirePointers);

                return (IntPtr)pData0;
            }
        }

        /// <summary>
        /// Gets the stream's current position in the object index.
        /// </summary>
        public Int32 PositionInObjects
        {
            get { return positionInObjects; }
        }

        /// <summary>
        /// Gets the stream's current position in the data buffer.
        /// </summary>
        public Int32 PositionInBytes
        {
            get { return positionInBytes; }
        }

        /// <summary>
        /// Ensures that the data buffer has at least the specified capacity, and expands
        /// the buffer if it does not.
        /// </summary>
        private void EnsureDataCapacity(Int32 capacity)
        {
            if (capacity <= CapacityInBytes)
                return;

            var size = Math.Max(capacity, (data.Length == 0) ? DefaultDataCapacity : (data.Length * 3) / 2 + 1);
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

            var size = Math.Max(capacity, (index.Length == 0) ? DefaultIndexCapacity : (index.Length * 3) / 2 + 1);
            var temp = new Int32[size];
            if (lengthInObjects > 0)
            {
                Array.Copy(index, temp, lengthInObjects);
            }
            index = temp;
        }
        
        // State values.
        private const Int32 DefaultDataCapacity = 256;
        private const Int32 DefaultIndexCapacity = 8;
        private static readonly Byte[] EmptyData = new Byte[0];
        private static readonly Int32[] EmptyIndex = new Int32[0];
        private GCHandle gcData;
        private Int32 positionInBytes;
        private Int32 positionInObjects;
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
