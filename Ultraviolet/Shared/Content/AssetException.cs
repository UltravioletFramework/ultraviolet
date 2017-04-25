using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the exception that is thrown when an error occurs while retrieving an asset.
    /// </summary>
    [Serializable]
    public sealed class AssetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetException"/> class.
        /// </summary>
        public AssetException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetException"/> class with the specified exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AssetException(String message) : base(message) { }
    }
}
