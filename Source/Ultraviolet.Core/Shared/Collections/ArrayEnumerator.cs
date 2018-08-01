using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a value-type enumerator for an array.
    /// </summary>
    public struct ArrayEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayEnumerator{T}"/> structure.
        /// </summary>
        /// <param name="array">The array that is being enumerated.</param>
        public ArrayEnumerator(T[] array)
        {
            Contract.Require(array, nameof(array));

            this.array = array;
            this.position = -1;
            this.disposed = false;
        }

        /// <inheritdoc/>
        public void Dispose() =>
            this.disposed = true;

        /// <inheritdoc/>
        public void Reset()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);

            position = -1;
        }

        /// <inheritdoc/>
        public Boolean MoveNext()
        {
            Contract.EnsureNotDisposed(this, disposed);

            if (position + 1 < array.Length)
            {
                position++;
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        Object System.Collections.IEnumerator.Current => Current;

        /// <inheritdoc/>
        public T Current => (position >= 0 && position < array.Length) ? array[position] : default(T);

        // Enumeration state.
        private readonly T[] array;
        private Int32 position;
        private Boolean disposed;
    }
}
