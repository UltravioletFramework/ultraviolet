using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            Contract.Require(parent, "parent");

            this.parent = parent;
        }

        /// <inheritdoc/>
        public void CopyTo(Visual[] array, Int32 arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var child in storage)
            {
                RemoveVisualChild(child);
            }
            storage.Clear();
        }

        /// <inheritdoc/>
        public void Add(Visual item)
        {
            Contract.Require(item, "item");

            AddVisualChild(item);
            storage.Add(item);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, Visual item)
        {
            Contract.Require(item, "item");

            AddVisualChild(item);
            storage.Insert(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            var element = storage[index];

            RemoveVisualChild(element);
            storage.RemoveAt(index);
        }

        /// <inheritdoc/>
        public Boolean Remove(Visual item)
        {
            Contract.Require(item, "item");

            if (VisualTreeHelper.GetParent(item) != Parent)
                return false;

            RemoveVisualChild(item);
            return storage.Remove(item);
        }

        /// <inheritdoc/>
        public Boolean Contains(Visual item)
        {
            return storage.Contains(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(Visual item)
        {
            return storage.IndexOf(item);
        }

        /// <inheritdoc/>
        public Visual this[Int32 index]
        {
            get
            {
                return storage[index];
            }
            set
            {
                Contract.Require(value, "value");

                var existing = storage[index];
                RemoveVisualChild(existing);

                storage[index] = value;
                AddVisualChild(value);
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
            get { return storage.Count; }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly
        {
            get { return false; }
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

        // Property values.
        private readonly Visual parent;

        // State values.
        private readonly List<Visual> storage = new List<Visual>(0);
    }
}
