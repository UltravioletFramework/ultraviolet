using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the collection which makes up the content of an <see cref="ItemsControl"/>.
    /// </summary>
    public sealed class ItemCollection : ObservableList<Object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCollection"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ItemsControl"/> that owns the collection.</param>
        public ItemCollection(ItemsControl owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }

        /// <summary>
        /// Gets the <see cref="ItemsControl"/> that owns the collection.
        /// </summary>
        internal ItemsControl Owner
        {
            get { return owner; }
        }

        /// <inheritdoc/>
        protected override void OnItemAdded(object item)
        {
            base.OnItemAdded(item);
        }

        /// <inheritdoc/>
        protected override void OnItemRemoved(object item)
        {
            base.OnItemRemoved(item);
        }

        /// <inheritdoc/>
        protected override void OnCleared()
        {
            base.OnCleared();
        }

        // Property values.
        private readonly ItemsControl owner;
    }
}
