using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    partial class Selector
    {
        /// <summary>
        /// Represents the collection of selected items.
        /// </summary>
        private class SelectionCollection : INotifyCollectionChanged
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SelectionCollection"/> class.
            /// </summary>
            /// <param name="owner">The <see cref="Selector"/> that owns this collection.</param>
            public SelectionCollection(Selector owner)
            {
                Contract.Require(owner, "owner");

                this.owner = owner;
            }

            /// <summary>
            /// Adds the specified container to the selection collection.
            /// </summary>
            /// <param name="container">The container to add to the collection.</param>
            public void Add(DependencyObject container)
            {
                Contract.Require(container, "container");

                var index = owner.ItemContainerGenerator.IndexFromContainer(container);
                var metadata = new SelectionMetadata(container, index);
                selections.AddLast(metadata);

                OnCollectionItemAdded(container);
            }

            /// <summary>
            /// Removes the specified container from the selection collection.
            /// </summary>
            /// <param name="container">The container to remove from the collection.</param>
            public void Remove(DependencyObject container)
            {
                Contract.Require(container, "container");

                for (var current = selections.First; current != null; current = current.Next)
                {
                    if (current.Value.Container == container)
                    {
                        selections.Remove(current);
                        OnCollectionItemRemoved(container);
                        break;
                    }
                }
            }

            /// <summary>
            /// Sets the value of the <see cref="Selector.IsSelectedProperty"/> dependency property
            /// for every item in the selection collection.
            /// </summary>
            public void SetIsSelectedOnAllItems(Boolean isSelected)
            {
                for (var current = selections.First; current != null; current = current.Next)
                {
                    Selector.SetIsSelected(current.Value.Container, isSelected);
                }
            }

            /// <summary>
            /// Gets the <see cref="Selector"/> that owns this collection.
            /// </summary>
            public Selector Owner
            {
                get { return owner; }
            }

            /// <summary>
            /// Gets the currently selected item.
            /// </summary>
            public Object SelectedItem
            {
                get
                {
                    if (selections.Count == 0)
                        return null;

                    var container = selections.First.Value.Container as DependencyObject;
                    if (container != null)
                    {
                        return owner.ItemContainerGenerator.ItemFromContainer(container);
                    }
                    return null;
                }
            }

            /// <summary>
            /// Gets the index of the currently selected item.
            /// </summary>
            public Int32 SelectedIndex
            {
                get
                {
                    if (selections.Count == 0)
                        return -1;

                    return selections.First.Value.Index;
                }
            }

            /// <summary>
            /// Gets the number of items in the selection collection.
            /// </summary>
            public Int32 Count
            {
                get { return selections.Count; }
            }

            /// <inheritdoc/>
            public event CollectionResetEventHandler CollectionReset;

            /// <inheritdoc/>
            public event CollectionItemAddedEventHandler CollectionItemAdded;

            /// <inheritdoc/>
            public event CollectionItemRemovedEventHandler CollectionItemRemoved;

            /// <summary>
            /// Raises the <see cref="CollectionReset"/> event.
            /// </summary>
            private void OnCollectionReset()
            {
                var temp = CollectionReset;
                if (temp != null)
                {
                    temp(this);
                }
            }

            /// <summary>
            /// Raises the <see cref="CollectionItemAdded"/> event.
            /// </summary>
            /// <param name="item">The item that was added.</param>
            private void OnCollectionItemAdded(Object item)
            {
                var temp = CollectionItemAdded;
                if (temp != null)
                {
                    temp(this, item);
                }
            }

            /// <summary>
            /// Raises the <see cref="CollectionItemRemoved"/> event.
            /// </summary>
            /// <param name="item">The item that was removed.</param>
            private void OnCollectionItemRemoved(Object item)
            {
                var temp = CollectionItemRemoved;
                if (temp != null)
                {
                    temp(this, item);
                }
            }

            // Property values.
            private readonly Selector owner;

            // The current list of selected items.
            private readonly PooledLinkedList<SelectionMetadata> selections = 
                new PooledLinkedList<SelectionMetadata>();
        }
    }
}
