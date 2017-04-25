using System;

namespace Ultraviolet.Presentation
{
    partial class UpfPool<TPooledType>
    {
        /// <summary>
        /// Represents the internal interface for pooled objects.
        /// </summary>
        private interface IPooledObject
        {
            /// <summary>
            /// Assigns the pooled object to the specified owner.
            /// </summary>
            /// <param name="owner">The element to which to assign the pooled object.</param>
            void Assign(Object owner);

            /// <summary>
            /// Gets the object which owns the pooled object.
            /// </summary>
            WeakReference Owner { get; }

            /// <summary>
            /// Gets the pooled object that has been assigned.
            /// </summary>
            TPooledType Value { get; }

            /// <summary>
            /// Gets a value indicating whether this assignment is active.
            /// </summary>
            Boolean IsActive { get; }

            /// <summary>
            /// Gets a value indicating whether the object's owner has been garbage collected.
            /// </summary>
            Boolean IsCollected { get; }

            /// <summary>
            /// Gets the previous object in the pool's chain of objects.
            /// </summary>
            IPooledObject Previous { get; set; }

            /// <summary>
            /// Gets the next object in the pool's chain of objects.
            /// </summary>
            IPooledObject Next { get; set; }

            /// <summary>
            /// Gets or sets the pool list to which the object currently belongs.
            /// </summary>
            UpfPoolList CurrentPoolList { get; set; }
        }
    }
}
