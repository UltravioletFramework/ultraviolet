using System;

namespace Ultraviolet.Presentation
{
    partial class UpfPool<TPooledType>
    {
        /// <summary>
        /// Represents a pooled object which has been assigned to a particular owner.
        /// </summary>
        public class PooledObject : IPooledObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PooledObject"/> class.
            /// </summary>
            /// <param name="object">The pooled object which is being assigned.</param>
            public PooledObject(TPooledType @object)
            {
                this.Owner = new WeakReference(null);
                this.Value = @object;
            }

            /// <inheritdoc/>
            void IPooledObject.Assign(Object owner)
            {
                IsActive = (owner != null);
                Owner.Target = owner;
            }

            /// <inheritdoc/>
            public WeakReference Owner
            {
                get;
                private set;
            }

            /// <inheritdoc/>
            public TPooledType Value
            {
                get;
                private set;
            }

            /// <inheritdoc/>
            public Boolean IsActive
            {
                get;
                private set;
            }

            /// <inheritdoc/>
            public Boolean IsCollected
            {
                get { return IsActive && !Owner.IsAlive; }
            }

            /// <inheritdoc/>
            IPooledObject IPooledObject.Previous
            {
                get;
                set;
            }

            /// <inheritdoc/>
            IPooledObject IPooledObject.Next
            {
                get;
                set;
            }

            /// <inheritdoc/>
            UpfPoolList IPooledObject.CurrentPoolList
            {
                get;
                set;
            }
        }
    }
}
