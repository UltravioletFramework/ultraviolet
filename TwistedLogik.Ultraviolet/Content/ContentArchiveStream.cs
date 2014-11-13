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
        /// <param name="position">The position at which the file data begins within the source stream.</param>
        /// <param name="length">The file data's length in bytes.</param>
        internal ContentArchiveStream(Stream source, Int64 position, Int64 length)
        {
            Contract.Require(source, "source");

            this.source = source;
            this.source.Seek(position, SeekOrigin.Begin);

            this.position = position;
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
                        var pos = position + offset;
                        if (pos < 0 || pos < position)
                        {
                            throw new IOException("TODO");
                        }
                        source.Seek(pos, SeekOrigin.Begin);
                    }
                    break;

                case SeekOrigin.Current:
                    {
                        var pos = source.Position + offset;
                        if (pos < position)
                        {
                            throw new IOException("TODO");
                        }
                        source.Seek(offset, SeekOrigin.Begin);
                    }
                    break;

                case SeekOrigin.End:
                    {
                        var pos = position + length + offset;
                        if (pos < position)
                        {
                            throw new IOException("TODO");
                        }
                        source.Seek(position + length + offset, SeekOrigin.End);
                    }
                    break;
            }

            return Position;
        }

        /// <inheritdoc/>
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Require(buffer, "buffer");
            Contract.EnsureRange(offset >= 0 && offset < buffer.Length, "offset");
            Contract.EnsureRange(count > 0 && count <= Length, "offset");

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

                return source.Position - position;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value >= 0 && value < Length, "value");

                source.Position = position + value;
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
        private Int64 position;
        private Int64 length;
        private Boolean disposed;
    }
}
