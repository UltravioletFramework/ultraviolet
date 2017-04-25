using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the result of an incremental lexing or parsing operation.
    /// </summary>
    public struct IncrementalResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalResult"/> structure.
        /// </summary>
        /// <param name="affectedOffset">The index of the first output token that was affected by the operation.</param>
        /// <param name="affectedCount">The number of output tokens that were affected by the operation.</param>
        internal IncrementalResult(Int32 affectedOffset, Int32 affectedCount)
        {
            this.affectedOffset = affectedOffset;
            this.affectedCount = affectedCount;
        }

        /// <summary>
        /// Gets the index of the first output token that was affected by the operation.
        /// </summary>
        public Int32 AffectedOffset
        {
            get { return affectedOffset; }
        }

        /// <summary>
        /// Gets the number of output tokens that were affected by the operation.
        /// </summary>
        public Int32 AffectedCount
        {
            get { return affectedCount; }
        }

        // Property values.
        private readonly Int32 affectedOffset;
        private readonly Int32 affectedCount;
    }
}
