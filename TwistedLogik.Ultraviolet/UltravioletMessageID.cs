using System;
using System.Threading;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents an identifier for a message send through the Ultraviolet context's message queue. 
    /// </summary>
    public struct UltravioletMessageID : IEquatable<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletMessageID"/> structure.
        /// </summary>
        /// <param name="name">The message type's name.</param>
        /// <param name="value">The message's identifier value.</param>
        private UltravioletMessageID(String name, Int32 value)
        {
            this.name  = name;
            this.value = value;
        }

        /// <summary>
        /// Compares two Ultraviolet message identifiers for equality.
        /// </summary>
        /// <param name="id1">The first <see cref="UltravioletMessageID"/>.</param>
        /// <param name="id2">The second <see cref="UltravioletMessageID"/>.</param>
        /// <returns><c>true</c> if the specified identifiers are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(UltravioletMessageID id1, UltravioletMessageID id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Compares two Ultraviolet message identifiers for inequality.
        /// </summary>
        /// <param name="id1">The first <see cref="UltravioletMessageID"/>.</param>
        /// <param name="id2">The second <see cref="UltravioletMessageID"/>.</param>
        /// <returns><c>true</c> if the specified identifiers are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(UltravioletMessageID id1, UltravioletMessageID id2)
        {
            return !id1.Equals(id2);
        }

        /// <summary>
        /// Acquires an unused message identifier.
        /// </summary>
        /// <param name="name">The message type's name.</param>
        /// <returns>The <see cref="UltravioletMessageID"/> that was acquired.</returns>
        public static UltravioletMessageID Acquire(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            var id = Interlocked.Increment(ref counter);
            return new UltravioletMessageID(name, id);
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return name ?? "INVALID";
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return obj is UltravioletMessageID && Equals((UltravioletMessageID)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(UltravioletMessageID obj)
        {
            return obj.value == this.value;
        }

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
