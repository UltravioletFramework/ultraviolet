using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class DependencyProperty
    {
        /// <summary>
        /// Represents a pairing between a change notification subscriber and a particular object which the subscriber
        /// is monitoring for changes.
        /// </summary>
        private struct ChangeNotificationKey : IEquatable<ChangeNotificationKey>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChangeNotificationKey"/> structure.
            /// </summary>
            /// <param name="subscriber">The object which is subscribing to change notifications.</param>
            /// <param name="target">The target object that is being monitored by the subscriber.</param>
            public ChangeNotificationKey(IDependencyPropertyChangeNotificationSubscriber subscriber, DependencyObject target)
            {
                this.subscriber = subscriber;
                this.target     = target;
            }

            /// <inheritdoc/>
            public override Int32 GetHashCode()
            {
                unchecked
                {
                    var hash = 17;
                    hash = hash * 23 + subscriber.GetHashCode();
                    hash = hash * 23 + target.GetHashCode();
                    return hash;
                }
            }

            /// <inheritdoc/>
            public override Boolean Equals(Object obj)
            {
                if (!(obj is ChangeNotificationKey))
                {
                    return false;
                }
                return Equals((ChangeNotificationKey)obj);
            }

            /// <summary>
            /// Gets a value indicating whether this object is equal to the specified object.
            /// </summary>
            /// <param name="other">The object to compare to this object.</param>
            /// <returns><c>true</c> if the specified object is equal to this object; otherwise, <c>false</c>.</returns>
            public Boolean Equals(ChangeNotificationKey other)
            {
                return
                    this.subscriber == other.subscriber &&
                    this.target == other.target;
            }

            /// <inheritdoc/>
            public override String ToString()
            {
                return ToString(null);
            }

            /// <summary>
            /// Converts the object to a human-readable string.
            /// </summary>
            /// <param name="provider">A format provider with which to convert the object.</param>
            /// <returns>A human-readable string that represents the object.</returns>
            public String ToString(IFormatProvider provider)
            {
                return String.Format(provider, "{0} subscribed to {1}", subscriber.GetType(), target.GetType());
            }

            /// <summary>
            /// Gets the object that is subscribed to changes.
            /// </summary>
            public IDependencyPropertyChangeNotificationSubscriber Subscriber
            {
                get { return subscriber; }
            }

            /// <summary>
            /// Gets the target object which is being monitored by the subscriber.
            /// </summary>
            public DependencyObject Target
            {
                get { return target; }
            }

            // Property values.
            private readonly IDependencyPropertyChangeNotificationSubscriber subscriber;
            private readonly DependencyObject target;
        }
    }
}
