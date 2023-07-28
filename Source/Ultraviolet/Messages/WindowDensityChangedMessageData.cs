using Ultraviolet.Core.Messages;
using Ultraviolet.Platform;

namespace Ultraviolet.Messages
{
    /// <summary>
    /// Represents the message data for a Window Density Changed message.
    /// </summary>
    public sealed class WindowDensityChangedMessageData : MessageData
    {
        /// <summary>
        /// Gets or sets the window which changed density.
        /// </summary>
        public IUltravioletWindow Window
        {
            get;
            set;
        }
    }
}
