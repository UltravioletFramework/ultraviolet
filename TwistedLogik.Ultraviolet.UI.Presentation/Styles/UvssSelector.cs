using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Styles
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
            
            if (this.parts.Count == 1)
            {
                var part = this.parts[0];

                this.priority = part.Priority;
                this.text = part.ToString();
            }
            else
            {
                var builder = SelectorTextBuilder.Value;

                foreach (var part in this.parts)
                {
                    this.priority += part.Priority;

                    if (builder.Length > 0)
                        builder.Append(' ');

                    builder.Append(part.ToString());
                }

                this.text = builder.ToString();
                builder.Length = 0;
            }

            this.navexp = navexp;
        }
        
        /// <inheritdoc/>
        public override String ToString() =>
            (navexp == null) ? Text : String.Format("{0} | {1}", Text, navexp);

        /// <summary>
        /// Compares the priority of this selector with the priority of another selector
        /// and returns a value which represents their relative order.
        /// </summary>
        /// <param name="selector">The selector to compare to this selector.</param>
        /// <returns>A value which represents the relative order of the objects being compared.</returns>
        public Int32 ComparePriority(UvssSelector selector)
        {
            Contract.Require(selector, nameof(selector));

            if (IsDirectlyTargeted && !selector.IsDirectlyTargeted)
                return 1;

            if (!IsDirectlyTargeted && selector.IsDirectlyTargeted)
                return -1;

            return priority.CompareTo(selector.priority);
        }
        
        /// <summary>
        /// Gets the selector part at the specified index within the selector.
        /// </summary>
        /// <param name="index">The index of the selector part to retrieve.</param>
        /// <returns>The selector part at the specified index within the selector.</returns>
        public UvssSelectorPart this[Int32 index]
        {
            get { return parts[index]; }
        }

        /// <summary>
        /// Gets the number of parts in the selector.
        /// </summary>
        public Int32 PartCount
        {
            get { return parts.Count; }
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

                return parts[parts.Count - 1].HasName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this selector matches the view's resource manager.
        /// </summary>
        /// <returns><see langword="true"/> if this selector matches the view's resource manager; otherwise, <see langword="false"/>.</returns>
        public Boolean IsViewResourceSelector
        {
            get
            {
                if (parts.Count != 1)
                    return false;

                var part = parts[0];

                return !part.HasClasses && !part.HasName && String.Equals(part.Type, "view", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the selector matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="root">The topmost root element to consider when tracing ancestors.</param>
        /// <returns><see langword="true"/> if the selector matches the specified UI element; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the element matches the selector part; otherwise, <see langword="false"/>.</returns>
        private static Boolean ElementMatchesSelectorPart(UIElement root, UIElement element, UvssSelectorPart part)
        {
            if (element.Parent == null && String.Equals(part.Type, "document", StringComparison.OrdinalIgnoreCase))
                return true;

            if (part.HasName)
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null && !String.IsNullOrWhiteSpace(frameworkElement.Name))
                {
                    if (!String.Equals(frameworkElement.Name, part.Name, StringComparison.OrdinalIgnoreCase))
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

            if (part.HasType)
            {
                var partElementType = default(Type);
                if (!element.Ultraviolet.GetUI().GetPresentationFoundation().GetKnownType(part.Type, false, out partElementType))
                    return false;

                if (part.HasExactType)
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
        /// <returns><see langword="true"/> if the element matches the selector part; otherwise, <see langword="false"/>.</returns>
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

        // String builder used to compile selectors text from parts.
        private static readonly Lazy<StringBuilder> SelectorTextBuilder =
            new Lazy<StringBuilder>(() => new StringBuilder(256));

        // State values.
        private readonly List<UvssSelectorPart> parts;
        private readonly Int32 priority;
        private readonly String text;
        private readonly UvssNavigationExpression navexp;
    }
}
