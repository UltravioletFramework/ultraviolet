using System;
using Ultraviolet.Core.Messages;

namespace Ultraviolet.BASS.Messages
{
    /// <summary>
    /// Represents the data for an event which indicates that the BASS device has changed.
    /// </summary>
    public sealed class BASSDeviceChangedMessageData : MessageData
    {
        /// <summary>
        /// Gets or sets the identifier of the new device.
        /// </summary>
        public UInt32 DeviceID { get; set; }
    }
}
