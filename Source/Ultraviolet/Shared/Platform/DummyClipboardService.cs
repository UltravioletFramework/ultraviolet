using System;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a dummy implementation of the <see cref="ClipboardService"/> class.
    /// </summary>
    public sealed class DummyClipboardService : ClipboardService
    {
        /// <inheritdoc/>
        public override String Text
        {
            get { return null; }
            set { }
        }
    }
}
