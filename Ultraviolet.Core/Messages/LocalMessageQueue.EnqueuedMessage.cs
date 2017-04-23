
namespace Ultraviolet.Core.Messages
{
    public partial class LocalMessageQueue<TMessageType>
    {
        /// <summary>
        /// Represents a pending message in the message queue.
        /// </summary>
        protected class EnqueuedMessage
        {
            /// <summary>
            /// The message type.
            /// </summary>
            public TMessageType Type
            {
                get;
                internal set;
            }

            /// <summary>
            /// The message data.
            /// </summary>
            public MessageData Data
            {
                get;
                internal set;
            }
        }
    }
}
