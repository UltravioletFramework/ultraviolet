using System;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletClipboardInfo"/>.
    /// </summary>
    public sealed class DummyUltravioletClipboardInfo : IUltravioletClipboardInfo
    {
        /// <inheritdoc/>
        public String Text
        {
            get { return null; }
            set { }
        }
    }
}
