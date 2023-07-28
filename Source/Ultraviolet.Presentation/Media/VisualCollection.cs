using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a collection of visual objects.
    /// </summary>
    public partial class VisualCollection : IList<Visual>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualCollection"/> class.
        /// </summary>
        /// <param name="parent">The visual parent of objects which are added to this collection.</param>
        public VisualCollection(Visual parent)
        {
            Contract.Require(parent, nameof(parent));

            this.parent = parent;
        }

        /// <summary>
        /// Gets the <see cref="Visual"/> with the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the visual to retrieve.</param>
        /// <returns>The <see cref="Visual"/> with the specified index within the collection.</returns>
        public Visual Get(Int32 index)
        {
            var list = storage;
            if (list == null)
                throw new ArgumentOutOfRangeException("index");

            return list[index];
        }

        /// <summary>
        /// Gets the <see cref="Visual"/> with the specified position in the collection's z-order.
        /// </summary>
        /// <param name="index">The position within the z-order of the visual to retrieve.</param>
        /// <returns>The <see cref="Visual"/> with the specified position in the collection's z-order.</returns>
        public Visual GetByZOrder(Int32 index)
        {
            if (storageByZIndex != null)
            {
                return storage[storageByZIndex[index]];
            }
            return storage[index];
        }

        /// <inheritdoc/>
        public void CopyTo(Visual[] array, Int32 arrayIndex)
        {
            if (storage == null)
                return;

            storage.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (storage == null)
                return;

            foreach (var child in storage)
            {
                RemoveVisualChild(child);
            }

            storage.Clear();

            if (storageByZIndex != null)
                storageByZIndex.Clear();
        }

        /// <inheritdoc/>
        public void Add(Visual item)
        {
            Contract.Require(item, nameof(item));

            AddVisualChild(item);
            storage = storage ?? new List<Visual>();
            storage.Add(item);

            if (storageByZIndex != null)
                storageByZIndex.Add(storage.Count - 1);

            UpdateSort();
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, Visual item)
        {
            Contract.Require(item, nameof(item));

            AddVisualChild(item);
            storage = storage ?? new List<Visual>();
            storage.Insert(index, item);

            if (storageByZIndex != null)
                storageByZIndex.Add(storage.Count - 1);

            UpdateSort();
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            if (storage == null)
                throw new ArgumentOutOfRangeException("index");

            var element = storage[index];

            RemoveVisualChild(element);
            storage.RemoveAt(index);

            RebuildZIndexStorage();
            UpdateSort();
        }

        /// <inheritdoc/>
        public Boolean Remove(Visual item)
        {
            Contract.Require(item, nameof(item));

            if (storage == null)
                return false;

            if (VisualTreeHelper.GetParent(item) != Parent)
                return false;

            RemoveVisualChild(item);
            if (storage.Remove(item))
            {
                RebuildZIndexStorage();
                UpdateSort();

                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Contains(Visual item)
        {
            return storage != null && storage.Contains(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(Visual item)
        {
            return storage == null ? -1 : storage.IndexOf(item);
        }

        /// <inheritdoc/>
        public Visual this[Int32 index]
        {
            get
            {
                Contract.EnsureRange(storage != null, nameof(index));

                return storage[index];
            }
            set
            {
                Contract.Require(value, nameof(value));
                Contract.EnsureRange(storage != null, nameof(index));

                var existing = storage[index];
                RemoveVisualChild(existing);
                
                storage[index] = value;
                AddVisualChild(value);
                
                RebuildZIndexStorage();
                UpdateSort();
            }
        }

        /// <summary>
        /// Gets the visual parent of objects which are added to this collection.
        /// </summary>
        public Visual Parent
        {
            get { return parent; }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return storage == null ? 0 : storage.Count; }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates that the collection should maintain a list of visuals sorted according to their z-index.
        /// </summary>
        internal void SortByZIndex()
        {
            if (storageByZIndex == null)
            {
                zIndexComparer = new ZIndexComparer(this);
                storageByZIndex = new List<Int32>(storage.Capacity);
                RebuildZIndexStorage();
            }

            UpdateSort();
        }

        /// <summary>
        /// Adds the specified child to the collection's parent.
        /// </summary>
        /// <param name="child">The child to add to the collection's parent.</param>
        private void AddVisualChild(Visual child)
        {
            if (child.VisualParent != null)
            {
                child.VisualParent.RemoveVisualChild(child);
            }
            parent.AddVisualChild(child);
        }

        /// <summary>
        /// Removes the specified child from the collection's parent.
        /// </summary>
        /// <param name="child">The child to remove from the collection's parent.</param>
        private void RemoveVisualChild(Visual child)
        {
            parent.RemoveVisualChild(child);
        }

        /// <summary>
        /// Sorts the collection, if necessary.
        /// </summary>
        private void UpdateSort()
        {
            if (storageByZIndex != null)
                storageByZIndex.Sort(zIndexComparer);
        }

        /// <summary>
        /// Rebuilds the list that maintains z-index sorting.
        /// </summary>
        private void RebuildZIndexStorage()
        {
            if (storageByZIndex == null)
                return;

            storageByZIndex.Clear();

            if (storage == null)
                return;

            if (storageByZIndex.Capacity < storage.Count)
                storageByZIndex.Capacity = storage.Count;

            for (int i = 0; i < storage.Count; i++)
            {
                storageByZIndex.Add(i);
            }            
        }

        // Property values.
        private readonly Visual parent;

        // State values.
        private Comparer<Int32> zIndexComparer;
        private List<Visual> storage;
        private List<Int32> storageByZIndex;

        // An empty storage list for enumerating through an empty collection.
        private static readonly List<Visual> emptyStorage = new List<Visual>(0);
    }
}
