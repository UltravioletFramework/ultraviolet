using System;

namespace Ultraviolet.Core.Messages
{
    /// <summary>
    /// Represents the base class for message data.
    /// </summary>
    public class MessageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData"/> class.
        /// </summary>
        protected MessageData()
        {

        }

        /// <summary>
        /// Resets the message data to its default state.
        /// </summary>
        public virtual void Reset()
        {

        }

        /// <summary>
        /// Evaluates another message data object to determine if it can be merged with this object.
        /// </summary>
        /// <param name="other">The message data object to evaluate against this message data object.</param>
        /// <returns>A message data object that represents the merged result, or <see langword="null"/> if the objects cannot be merged.</returns>
        public virtual MessageData EvaluateMerge(MessageData other)
        {
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this message data is able to be merged with other message data.
        /// </summary>
        public virtual Boolean IsMergeable
        {
            get { return false; }
        }

        /// <summary>
        /// Represents an empty message data object.
        /// </summary>
        public static readonly MessageData Empty = new MessageData();
    }
}
