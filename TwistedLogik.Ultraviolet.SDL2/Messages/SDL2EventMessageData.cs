using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Messages
{
    /// <summary>
    /// Represents the message data for an SDL2Event message.
    /// </summary>
    public sealed class SDL2EventMessageData : MessageData
    {
        /// <summary>
        /// Gets or sets the SDL event data.
        /// </summary>
        public SDL_Event Event
        {
            get;
            set;
        }
    }
}
