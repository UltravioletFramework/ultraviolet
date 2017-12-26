
namespace Ultraviolet.Core.Messages
{
    /// <summary>
    /// Represents an asynchronous queue of messages.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message which is published by the queue.</typeparam>
    public interface IMessageQueue<TMessageType>
    {
        /// <summary>
        /// Creates or retrieves an instance of the specified message data type.
        /// The instance may be retrieved from a pool; if so, it will be returned to the pool 
        /// once it has been published.
        /// </summary>
        /// <typeparam name="TMessageData">The type of message data object to create.</typeparam>
        /// <returns>The instance that was created or retrieved.</returns>
        TMessageData CreateMessageData<TMessageData>() where TMessageData : MessageData, new();

        /// <summary>
        /// Subscribes a receiver to the specified set of message types.
        /// </summary>
        /// <param name="receiver">The receiver to subscribe to the specified message type.</param>
        /// <param name="type">The message type to which to subscribe the receiver.</param>
        void Subscribe(IMessageSubscriber<TMessageType> receiver, TMessageType type);

        /// <summary>
        /// Subscribes a receiver to the specified set of message types.
        /// </summary>
        /// <param name="receiver">The receiver to subscribe to the specified message type.</param>
        /// <param name="types">The message types to which to subscribe the receiver.</param>
        void Subscribe(IMessageSubscriber<TMessageType> receiver, params TMessageType[] types);

        /// <summary>
        /// Unsubcribes a receiver from all message types.
        /// </summary>
        /// <param name="receiver">The receiver to unsubscribe from all message types.</param>
        void Unsubscribe(IMessageSubscriber<TMessageType> receiver);

        /// <summary>
        /// Unsubscribes a receiver from the specified set of message types.
        /// </summary>
        /// <param name="receiver">The receiver to unsubscribe from the specified message type.</param>
        /// <param name="type">The message type from which to unsubscribe the receiver.</param>
        void Unsubscribe(IMessageSubscriber<TMessageType> receiver, TMessageType type);

        /// <summary>
        /// Publishes a message to the queue.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        void Publish(TMessageType type, MessageData data);

        /// <summary>
        /// Publishes a message, bypassing the queue and immediately informing all subscribers.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        void PublishImmediate(TMessageType type, MessageData data);
    }
}
