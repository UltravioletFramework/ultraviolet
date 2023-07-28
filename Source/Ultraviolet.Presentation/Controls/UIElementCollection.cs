using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a collection of interface elements belonging to a panel.
    /// </summary>
    public partial class UIElementCollection : IList<UIElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementCollection"/> class.
        /// </summary>
        /// <param name="visualParent">The visual parent of items in the collection.</param>
        /// <param name="logicalParent">The logical parent of items in the collection.</param>
        public UIElementCollection(UIElement visualParent, FrameworkElement logicalParent)
        {
            Contract.Require(visualParent, nameof(visualParent));

            this.visualParent   = visualParent;
            this.visualChildren = new VisualCollection(visualParent);
            this.logicalParent  = logicalParent;
        }

        /// <summary>
        /// Gets the <see cref="UIElement"/> with the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>The <see cref="UIElement"/> with the specified index within the collection.</returns>
        public UIElement Get(Int32 index)
        {
            return (UIElement)visualChildren.Get(index);
        }

        /// <summary>
        /// Gets the <see cref="UIElement"/> with the specified position in the collection's z-order.
        /// </summary>
        /// <param name="index">The position within the z-order of the visual to retrieve.</param>
        /// <returns>The <see cref="UIElement"/> with the specified position in the collection's z-order.</returns>
        public UIElement GetByZOrder(Int32 index)
        {
            return (UIElement)visualChildren.GetByZOrder(index);
        }

        /// <inheritdoc/>
        public void CopyTo(UIElement[] array, Int32 arrayIndex)
        {
            visualChildren.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (Count == 0)
                return;

            foreach (var element in visualChildren)
            {
                RemoveLogicalChild(element);
            }
            visualChildren.Clear();
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public void Add(UIElement item)
        {
            Contract.Require(item, nameof(item));

            AddLogicalChild(item);
            visualChildren.Add(item);
            visualParent.InvalidateMeasure();
        }

        /// <summary>
        /// Adds a collection of items to this collection.
        /// </summary>
        /// <param name="items">The collection of items to add to this collection.</param>
        public void AddRange(IEnumerable<UIElement> items)
        {
            Contract.Require(items, nameof(items));

            foreach (var item in items)
            {
                AddLogicalChild(item);
                visualChildren.Add(item);
            }
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, UIElement item)
        {
            Contract.Require(item, nameof(item));

            AddLogicalChild(item);
            visualChildren.Insert(index, item);
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            var existing = visualChildren[index];
            RemoveLogicalChild(existing);

            visualChildren.RemoveAt(index);
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public Boolean Remove(UIElement item)
        {
            Contract.Require(item, nameof(item));

            if (visualChildren.Remove(item))
            {
                RemoveLogicalChild(item);
                visualParent.InvalidateMeasure();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Contains(UIElement item)
        {
            Contract.Require(item, nameof(item));

            return visualChildren.Contains(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(UIElement item)
        {
            Contract.Require(item, nameof(item));

            return visualChildren.IndexOf(item);
        }

        /// <inheritdoc/>
        public UIElement this[Int32 index]
        {
            get { return (UIElement)visualChildren[index]; }
            set
            {
                Contract.Require(value, nameof(value));

                var existing = visualChildren[index];
                RemoveLogicalChild(existing);

                visualChildren[index] = value;
            }
        }

        /// <summary>
        /// Gets the visual parent of items in the collection.
        /// </summary>
        public UIElement VisualParent
        {
            get { return visualParent; }
        }

        /// <summary>
        /// Gets the logical parent of items in the collection.
        /// </summary>
        public FrameworkElement LogicalParent
        {
            get { return logicalParent; }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return visualChildren.Count; }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a copy of the visual child collection which is sorted by z-index.
        /// </summary>
        internal void SortVisualChildrenByZIndex()
        {
            visualChildren.SortByZIndex();
        }

        /// <summary>
        /// Adds the specified child to the collection's parent.
        /// </summary>
        /// <param name="child">The child to add to the collection's parent.</param>
        private void AddLogicalChild(Visual child)
        {
            var uiElement = child as UIElement;
            if (uiElement != null)
            {
                uiElement.Parent = logicalParent;
            }
        }

        /// <summary>
        /// Removes the specified child from the collection's parent.
        /// </summary>
        /// <param name="child">The child to remove from the collection's parent.</param>
        private void RemoveLogicalChild(Visual child)
        {
            var uiElement = child as UIElement;
            if (uiElement != null)
            {
                uiElement.Parent = null;
            }
        }

        // Property values.
        private readonly UIElement visualParent;
        private readonly FrameworkElement logicalParent;

        // State values.
        private readonly VisualCollection visualChildren;
    }
}
