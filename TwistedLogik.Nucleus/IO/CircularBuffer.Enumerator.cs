using System;
using System.Collections.Generic;

namespace TwistedLogik.Nucleus.IO
{
    /// <summary>
    /// Represents a circular buffer of bytes.
    /// </summary>
    public partial class CircularBuffer
    {
        public struct Enumerator : IEnumerator<Byte>
        {
            internal Enumerator(CircularBuffer buffer)
            {
                if (buffer == null)
                    throw new ArgumentNullException("buffer");
                this.buffer = buffer;
                this.position = 0;
            }

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (buffer == null || position + 1 >= buffer.Length)
                    return false;

                position++;
                return true;
            }

            public void Reset()
            {
                position = -1;
            }

            public Byte Current
            {
                get { return buffer[position]; }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            private CircularBuffer buffer;
            private Int32 position;
        }
    }
}
