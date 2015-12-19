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
        internal UvssSelector(IEnumerable<UvssSelectorPart> parts)
        {
            this.parts       = parts.ToList();
            this.priority    = parts.Sum(x => x.Priority);
            this.text        = String.Join(" ", parts.Select(x => x.ToString()));
            this.pseudoClass = parts.Where(x => !String.IsNullOrEmpty(x.PseudoClass)).Select(x => x.PseudoClass).SingleOrDefault();
        }

        /// <summary>
        /// Parses a selector from the specified string.
        /// </summary>
        /// <param name="str">The string from which to parse a selector.</param>
        /// <returns>The selector that was parsed from the specified string.</returns>
        public static UvssSelector Parse(String str)
        {
            Contract.Require(str, "str");

            if (str == String.Empty)
                return new UvssSelector(Enumerable.Empty<UvssSelectorPart>());
    
            var tokens   = UvssDocument.Lexer.Lex(str);
            var selector = UvssDocument.Parser.ParseSelector(str, tokens);

            return selector;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return Text;
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
        /// Gets a value indicating whether this selector matches the view's resource manager.
        /// </summary>
        /// <returns><c>true</c> if this selector matches the view's resource manager; otherwise, <c>false</c>.</returns>
        public Boolean IsViewResourceSelector()
        {
            if (parts.Count != 1)
                return false;

            var part = parts[0];

            return !part.HasClasses && !part.HasID && String.Equals(part.Element, "view", StringComparison.OrdinalIgnoreCase);
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

                if (!partElementType.IsAssignableFrom(element.GetType()))
                    return false;
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
    }
}
