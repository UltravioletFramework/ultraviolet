using Ultraviolet.Core.Messages;
using Ultraviolet.Platform;

namespace Ultraviolet.Messages
{
    /// <summary>
    /// Represents the message data for a Display Density Changed message.
    /// </summary>
    public sealed class DisplayDensityChangedMessageData : MessageData
    {
        /// <summary>
        /// Gets or sets the display which changed density.
        /// </summary>
        public IUltravioletDisplay Display
        {
            get;
            set;
        }
    }
}
