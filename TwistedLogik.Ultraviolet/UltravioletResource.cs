using System;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an object which encapsulates some native or implementation-specific resource.
    /// </summary>
    public abstract class UltravioletResource : IUltravioletComponent, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletResource"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected UltravioletResource(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            this.uv = uv;
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            disposed = true;
        }

        // Property values.
        private readonly UltravioletContext uv;

        // State values.
        private Boolean disposed;
    }
}
