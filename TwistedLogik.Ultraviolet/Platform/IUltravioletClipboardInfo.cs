using System;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Contains methods for interacting with the system clipboard.
    /// </summary>
    public interface IUltravioletClipboardInfo
    {
        /// <summary>
        /// Gets or sets the clipboard text.
        /// </summary>
        String Text
        {
            get;
            set;
        }
    }
}
