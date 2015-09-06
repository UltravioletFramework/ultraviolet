using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class PresentationFoundationPool<TPooledType>
    {
        /// <summary>
        /// Represents a pooled object which has been assigned to a particular owner.
        /// </summary>
        private class Assignment
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Assignment"/> class.
            /// </summary>
            /// <param name="@object">The pooled object which is being assigned.</param>
            public Assignment(TPooledType @object)
            {
                this.Owner = new WeakReference(null);
                this.Object = @object;
            }

            /// <summary>
            /// Assigns the pooled object to the specified owner.
            /// </summary>
            /// <param name="owner">The element to which to assign the pooled object.</param>
            public void Assign(UIElement owner)
            {
                IsActive = (owner != null);
                Owner.Target = owner;
            }

            /// <summary>
            /// Gets the object which owns the pooled object.
            /// </summary>
            public WeakReference Owner
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the pooled object that has been assigned.
            /// </summary>
            public TPooledType Object
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets a value indicating whether this assignment is active.
            /// </summary>
            public Boolean IsActive
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets a value indicating whether the object's owner has been garbage collected.
            /// </summary>
            public Boolean IsCollected
            {
                get { return IsActive && !Owner.IsAlive; }
            }
        }
    }
}
