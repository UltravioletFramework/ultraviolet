using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a collection of interface elements belonging to a panel.
    /// </summary>
    public sealed partial class UIElementCollection : IEnumerable<UIElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementCollection"/> class.
        /// </summary>
        /// <param name="panel">The <see cref="Panel"/> that owns this collection.</param>
        public UIElementCollection(Panel panel)
        {
            Contract.Require(panel, "panel");

            this.panel = panel;
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (var element in elements)
            {
                element.Parent = null;
            }
            elements.Clear();

            Panel.OnChildrenChanged();
            Panel.InvalidateMeasure();
        }

        /// <summary>
        /// Adds an element to the collection.
        /// </summary>
        /// <param name="element">The element to add to the collection.</param>
        /// <returns><c>true</c> if the element was added to the collection; otherwise, <c>false</c>.</returns>
        public Boolean Add(UIElement element)
        {
            Contract.Require(element, "element");

            element.Parent = Panel;
            elements.Add(element);

            Panel.OnChildrenChanged();
            Panel.InvalidateMeasure();

            return true;
        }

        /// <summary>
        /// Removes an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove from the collection.</param>
        /// <returns><c>true</c> if the element was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(UIElement element)
        {
            Contract.Require(element, "element");

            if (elements.Remove(element))
            {
                element.Parent = null;

                Panel.OnChildrenChanged();
                Panel.InvalidateMeasure();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified element; otherwise, <c>false</c>.</returns>
        public Boolean Contains(UIElement element)
        {
            Contract.Require(element, "element");

            return elements.Contains(element);
        }

        /// <summary>
        /// Brings the specified element to the front of the collection.
        /// </summary>
        /// <param name="element">The element to bring to the front of the collection.</param>
        /// <returns><c>true</c> if the element was brought to the front of the collection; otherwise, <c>false</c>.</returns>
        public Boolean BringToFront(UIElement element)
        {
            Contract.Require(element, "element");

            if (elements.Remove(element))
            {
                elements.Add(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends the specified element to the back of the collection.
        /// </summary>
        /// <param name="element">The element to send to the back of the collection.</param>
        /// <returns><c>true</c> if the element was sent to the back of the collection; otherwise, <c>false</c>.</returns>
        public Boolean SendToBack(UIElement element)
        {
            if (elements.Remove(element))
            {
                elements.Insert(0, element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the container that owns this collection.
        /// </summary>
        public Panel Panel
        {
            get { return panel; }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return elements.Count; }
        }

        /// <summary>
        /// Gets the element at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the element to retrieve.</param>
        /// <returns>The element at the specified index within the collection.</returns>
        public UIElement this[Int32 ix]
        {
            get { return elements[ix]; }
        }

        // Property values.
        private readonly Panel panel;

        // State values.
        private readonly List<UIElement> elements = 
            new List<UIElement>();
    }
}
