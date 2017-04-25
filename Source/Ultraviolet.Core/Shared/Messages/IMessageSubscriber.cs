
namespace Ultraviolet.Core.Messages
{
    /// <summary>
    /// Represents an object which can subscribe to a message queue.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message which is received by this subscriber.</typeparam>
    public interface IMessageSubscriber<TMessageType>
    {
        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void ReceiveMessage(TMessageType type, MessageData data);
    }
}
