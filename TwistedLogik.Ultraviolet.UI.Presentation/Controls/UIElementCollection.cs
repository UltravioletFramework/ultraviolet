using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
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
            Contract.Require(visualParent, "visualParent");

            this.visualParent   = visualParent;
            this.visualChildren = new VisualCollection(visualParent);
            this.logicalParent  = logicalParent;
        }

        /// <inheritdoc/>
        public void CopyTo(UIElement[] array, Int32 arrayIndex)
        {
            visualChildren.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var element in visualChildren)
            {
                RemoveLogicalChild(element);
            }
            visualChildren.Clear();
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public void Add(UIElement element)
        {
            Contract.Require(element, "element");

            AddLogicalChild(element);
            visualChildren.Add(element);
            visualParent.InvalidateMeasure();
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, UIElement item)
        {
            Contract.Require(item, "item");

            AddLogicalChild(item);
            visualChildren.Insert(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            var existing = visualChildren[index];
            RemoveLogicalChild(existing);

            visualChildren.RemoveAt(index);
        }

        /// <inheritdoc/>
        public Boolean Remove(UIElement item)
            {
            Contract.Require(item, "element");

            if (visualChildren.Remove(item))
            {
                AddLogicalChild(item);
                visualParent.InvalidateMeasure();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Contains(UIElement item)
        {
            Contract.Require(item, "item");

            return visualChildren.Contains(item);
        }

        /// <inheritdoc/>
        public Int32 IndexOf(UIElement item)
        {
            Contract.Require(item, "item");

            return visualChildren.IndexOf(item);
        }

        /// <inheritdoc/>
        public UIElement this[Int32 index]
        {
            get { return (UIElement)visualChildren[index]; }
            set
            {
                Contract.Require(value, "value");

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
