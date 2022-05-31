using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    public partial class BinaryHeap<T>
    {
        /// <summary>
        /// Represents an enumerator for the binary heap class.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            /// <summary>
            /// Initializes a new instance of the Enumerator structure.
            /// </summary>
            /// <param name="heap">The heap that is being enumerated.</param>
            internal Enumerator(BinaryHeap<T> heap)
            {
                Contract.Require(heap, nameof(heap));

                this.heap = heap;
                this.position = -1;
                this.disposed = false;
            }

            /// <summary>
            /// Releases resources associated with the object.
            /// </summary>
            public void Dispose()
            {
                this.disposed = true;
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);

                if (position + 1 < heap.count)
                {
                    position++;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);

                position = -1;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            Object System.Collections.IEnumerator.Current => Current;

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public T Current => (position >= 0 && position < heap.data.Length) ? heap.data[position] : default(T);

            // Enumeration state.
            private readonly BinaryHeap<T> heap;
            private Int32 position;
            private Boolean disposed;
        }
    }
}
