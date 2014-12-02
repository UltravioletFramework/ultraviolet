using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Ultraviolet.Layout.Elements;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a selector in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelector : IEnumerable<UvssSelectorPart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelector"/> class.
        /// </summary>
        /// <param name="parts">A collection containing the selector's parts.</param>
        internal UvssSelector(IEnumerable<UvssSelectorPart> parts)
        {
            this.parts = parts.ToList();
        }
        
        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Join(" ", parts.Select(x => x.ToString()));
        }

        /// <summary>
        /// Gets a value indicating whether the selector matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="priority">The selector's priority, if it matches the element.</param>
        /// <returns><c>true</c> if the selector matches the specified UI element; otherwise, <c>false</c>.</returns>
        public Boolean MatchesElement(UIElement element, out Int32 priority)
        {
            priority = 0;

            if (parts.Count == 0)
                return false;

            var firstSelectorPart = parts[parts.Count - 1];

            if (!ElementMatchesSelectorPart(element, firstSelectorPart))
                return false;

            priority = firstSelectorPart.Priority;

            var current = element;
            for (var i = parts.Count - 2; i >= 0; i--)
            {
                if (!AncestorMatchesSelectorPart(ref current, parts[i]))
                {
                    priority = 0;
                    return false;
                }
                priority += parts[i].Priority;
            }

            return true;
        }

        /// <inheritdoc/>
        public List<UvssSelectorPart>.Enumerator GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelectorPart> IEnumerable<UvssSelectorPart>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets a value indicating whether the specified UI element matches the specified selector part.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="part">The selector part to evaluate.</param>
        /// <returns><c>true</c> if the element matches the selector part; otherwise, <c>false</c>.</returns>
        private static Boolean ElementMatchesSelectorPart(UIElement element, UvssSelectorPart part)
        {
            if (!String.IsNullOrEmpty(part.ID))
            {
                if (!String.Equals(element.ID, part.ID, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            if (!String.IsNullOrEmpty(part.Element))
            {
                if (!String.Equals(element.Name, part.Element, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // TODO: Check classes

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified UI element has an ancestor that matches the specified selector part.
        /// </summary>
        /// <param name="element">The UI element to evaluate. If a match is found, this variable will be updated to contain the matching element.</param>
        /// <param name="part">The selector part to evaluate.</param>
        /// <returns><c>true</c> if the element matches the selector part; otherwise, <c>false</c>.</returns>
        private static Boolean AncestorMatchesSelectorPart(ref UIElement element, UvssSelectorPart part)
        {
            var current = element;
            while (true)
            {
                current = current.Container;
                if (current == null)
                    break;

                if (ElementMatchesSelectorPart(current, part))
                {
                    element = current;
                    return true;
                }
            }
            return false;
        }

        // State values.
        private readonly List<UvssSelectorPart> parts;
    }
}
