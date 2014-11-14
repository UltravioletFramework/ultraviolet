using System;
using System.IO;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a <see cref="Stream"/> which exposes a resource contained by a <see cref="ContentArchive"/>.
    /// </summary>
    public sealed class ContentArchiveStream : Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchiveStream"/> class.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/>.</param>
        /// <param name="start">The position at which the file data begins within the source stream.</param>
        /// <param name="length">The file data's length in bytes.</param>
        internal ContentArchiveStream(Stream source, Int64 start, Int64 length)
        {
            Contract.Require(source, "source");

            this.source = source;
            this.source.Seek(start, SeekOrigin.Begin);

            this.start    = start;
            this.length   = length;
        }

        /// <inheritdoc/>
        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            Contract.EnsureNotDisposed(this, disposed);

            switch (origin)
            {
                case SeekOrigin.Begin:
                    {
                        var pos = start + offset;
                        if (pos < 0 || pos < start)
                        {
                            throw new IOException(UltravioletStrings.CannotSeekPastBeginningOfStream);
                        }
                        source.Seek(pos, SeekOrigin.Begin);
                    }
                    break;

                case SeekOrigin.Current:
                    {
                        var pos = source.Position + offset;
                        if (pos < start)
                        {
                            throw new IOException(UltravioletStrings.CannotSeekPastBeginningOfStream);
                        }
                        source.Seek(offset, SeekOrigin.Current);
                    }
                    break;

                case SeekOrigin.End:
                    {
                        var pos = start + length + offset;
                        if (pos < start)
                        {
                            throw new IOException(UltravioletStrings.CannotSeekPastBeginningOfStream);
                        }
                        source.Seek(pos, SeekOrigin.Begin);
                    }
                    break;
            }

            return Position;
        }

        /// <inheritdoc/>
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            Contract.Require(buffer, "buffer");
            Contract.EnsureRange(offset >= 0 && offset < buffer.Length, "offset");
            Contract.EnsureRange(count > 0, "count");
            Contract.EnsureNotDisposed(this, disposed);

            if (Position + count >= Length)
            {
                count = (Int32)(Length - Position);
                if (count <= 0)
                {
                    return 0;
                }
            }

            return source.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void SetLength(Int64 value)
        {
            Contract.EnsureNotDisposed(this, disposed);

            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            Contract.EnsureNotDisposed(this, disposed);

            source.Flush();
        }

        /// <inheritdoc/>
        public override Boolean CanRead
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return source.CanRead;
            }
        }

        /// <inheritdoc/>
        public override Boolean CanSeek
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return source.CanSeek;
            }
        }

        /// <inheritdoc/>
        public override Boolean CanWrite
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return false;
            }
        }

        /// <inheritdoc/>
        public override Int64 Length
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return length;
            }
        }

        /// <inheritdoc/>
        public override Int64 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return source.Position - start;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value >= 0 && value < Length, "value");

                source.Position = start + value;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            disposed = true;

            if (disposing)
            {
                SafeDispose.Dispose(source);
            }

            base.Dispose(disposing);
        }

        // State values.
        private readonly Stream source;
        private Int64 start;
        private Int64 length;
        private Boolean disposed;
    }
}
