using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a selector in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssSelector : IEnumerable<UvssSelectorPart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelector"/> class.
        /// </summary>
        /// <param name="parts">A collection containing the selector's parts.</param>
        /// <param name="navexp">The selector's navigation expression, if it has one.</param>
        internal UvssSelector(IEnumerable<UvssSelectorPart> parts, UvssNavigationExpression navexp = null)
        {
            this.parts = parts.ToList();
            this.priority = parts.Sum(x => x.Priority);
            this.text = String.Join(" ", parts.Select(x => x.ToString()));
            this.pseudoClass = parts.Where(x => !String.IsNullOrEmpty(x.PseudoClass)).Select(x => x.PseudoClass).SingleOrDefault();
            this.navexp = navexp;
        }
        
        /// <inheritdoc/>
        public override String ToString() =>
            (navexp == null) ? Text : String.Format("{0} | {1}", Text, navexp);

        /// <summary>
        /// Gets a value indicating whether this selector has a higher (or equal) priority than the specified selector.
        /// </summary>
        /// <param name="selector">The selector to compare to this selector.</param>
        /// <returns><c>true</c> if this selector's priority is higher than or the same as the specified selector; otherwise, <c>false</c>.</returns>
        public Boolean IsHigherPriorityThan(UvssSelector selector)
        {
            Contract.Require(selector, "selector");
            
            if (IsDirectlyTargeted && !selector.IsDirectlyTargeted)
                return true;

            if (!IsDirectlyTargeted && selector.IsDirectlyTargeted)
                return false;

            return Priority >= selector.Priority;
        }

        /// <summary>
        /// Gets the selector's relative priority.
        /// </summary>
        public Int32 Priority
        {
            get { return priority; }
        }

        /// <summary>
        /// Gets the selector's text.
        /// </summary>
        public String Text
        {
            get { return text; }
        }

        /// <summary>
        /// Gets the pseudo-class associated with this selector.
        /// </summary>
        public String PseudoClass
        {
            get { return pseudoClass; }
        }

        /// <summary>
        /// Gets the selector's optional navigation expression, if it has one.
        /// </summary>
        public UvssNavigationExpression NavigationExpression
        {
            get { return navexp; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a directly targeted rule (i.e., it's last selector part specifies an element by name).
        /// </summary>
        public Boolean IsDirectlyTargeted
        {
            get
            {
                if (parts.Count == 0)
                    return false;

                return parts[parts.Count - 1].HasID;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this selector matches the view's resource manager.
        /// </summary>
        /// <returns><c>true</c> if this selector matches the view's resource manager; otherwise, <c>false</c>.</returns>
        public Boolean IsViewResourceSelector
        {
            get
            {
                if (parts.Count != 1)
                    return false;

                var part = parts[0];

                return !part.HasClasses && !part.HasID && String.Equals(part.Element, "view", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the selector matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="root">The topmost root element to consider when tracing ancestors.</param>
        /// <returns><c>true</c> if the selector matches the specified UI element; otherwise, <c>false</c>.</returns>
        public Boolean MatchesElement(UIElement element, UIElement root = null)
        {
            if (parts.Count == 0)
                return false;

            var firstSelectorPart = parts[parts.Count - 1];

            if (!ElementMatchesSelectorPart(root, element, firstSelectorPart))
                return false;

            var current = element;
            var qualifier = firstSelectorPart.Qualifier;
            for (var i = parts.Count - 2; i >= 0; i--)
            {
                if (!AncestorMatchesSelectorPart(ref current, parts[i], root, qualifier))
                {
                    return false;
                }
                qualifier = parts[i].Qualifier;
            }
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified UI element matches the specified selector part.
        /// </summary>
        /// <param name="root">The root element being animated.</param>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="part">The selector part to evaluate.</param>
        /// <returns><c>true</c> if the element matches the selector part; otherwise, <c>false</c>.</returns>
        private static Boolean ElementMatchesSelectorPart(UIElement root, UIElement element, UvssSelectorPart part)
        {
            if (element.Parent == null && String.Equals(part.Element, "document", StringComparison.OrdinalIgnoreCase))
                return true;

            if (part.HasID)
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null && !String.IsNullOrWhiteSpace(frameworkElement.Name))
                {
                    if (!String.Equals(frameworkElement.Name, part.ID, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (part.PseudoClass != null)
            {
                if (String.Equals(part.PseudoClass, "storyboard-root", StringComparison.OrdinalIgnoreCase))
                {
                    if (element != root)
                        return false;
                }
            }

            if (part.HasElement)
            {
                var partElementType = default(Type);
                if (!element.Ultraviolet.GetUI().GetPresentationFoundation().GetKnownType(part.Element, false, out partElementType))
                    return false;

                if (part.ElementIsExact)
                {
                    if (partElementType != element.GetType())
                        return false;
                }
                else
                {
                    if (!partElementType.IsAssignableFrom(element.GetType()))
                        return false;
                }
            }

            if (part.HasClasses)
            {
                foreach (var className in part.Classes)
                {
                    if (!element.Classes.Contains(className))
                        return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified UI element has an ancestor that matches the specified selector part.
        /// </summary>
        /// <param name="element">The UI element to evaluate. If a match is found, this variable will be updated to contain the matching element.</param>
        /// <param name="part">The selector part to evaluate.</param>
        /// <param name="root">The topmost root element to consider when tracing ancestors.</param>
        /// <param name="qualifier">A qualifier which specifies the relationship between the element and its ancestor.</param>
        /// <returns><c>true</c> if the element matches the selector part; otherwise, <c>false</c>.</returns>
        private static Boolean AncestorMatchesSelectorPart(ref UIElement element, UvssSelectorPart part, UIElement root, UvssSelectorPartQualifier qualifier)
        {
            if (qualifier == UvssSelectorPartQualifier.TemplatedChild)
            {
                var fe = element as FrameworkElement;
                if (fe != null && fe.TemplatedParent != null)
                {
                    var feTemplatedParent = (UIElement)fe.TemplatedParent;
                    if (ElementMatchesSelectorPart(feTemplatedParent, feTemplatedParent, part))
                    {
                        element = feTemplatedParent;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                var current = element;
                while (true)
                {
                    if (current == root)
                        return false;

                    current = ((qualifier == UvssSelectorPartQualifier.LogicalChild) ? LogicalTreeHelper.GetParent(current) : VisualTreeHelper.GetParent(current)) as UIElement;
                    if (current == null)
                        break;

                    if (ElementMatchesSelectorPart(root, current, part))
                    {
                        element = current;
                        return true;
                    }

                    if (qualifier != UvssSelectorPartQualifier.None)
                        break;
                }
                return false;
            }
        }

        // State values.
        private readonly List<UvssSelectorPart> parts;
        private readonly Int32 priority;
        private readonly String text;
        private readonly String pseudoClass;
        private readonly UvssNavigationExpression navexp;
    }
}
