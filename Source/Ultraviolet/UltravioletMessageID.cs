using System;
using System.Threading;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an identifier for a message send through the Ultraviolet context's message queue. 
    /// </summary>
    public partial struct UltravioletMessageID : IEquatable<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletMessageID"/> structure.
        /// </summary>
        /// <param name="name">The message type's name.</param>
        /// <param name="value">The message's identifier value.</param>
        private UltravioletMessageID(String name, Int32 value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Acquires an unused message identifier.
        /// </summary>
        /// <param name="name">The message type's name.</param>
        /// <returns>The <see cref="UltravioletMessageID"/> that was acquired.</returns>
        public static UltravioletMessageID Acquire(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var id = Interlocked.Increment(ref counter);
            return new UltravioletMessageID(name, id);
        }

        /// <inheritdoc/>
        public override String ToString() => name ?? "INVALID";

        /// <summary>
        /// Gets the message identifier's underlying value.
        /// </summary>
        public Int32 Value
        {
            get { return value; }
        }

        // The message identifier's underlying value.
        private static Int32 counter;
        private Int32 value;
        private String name;
    }
}
