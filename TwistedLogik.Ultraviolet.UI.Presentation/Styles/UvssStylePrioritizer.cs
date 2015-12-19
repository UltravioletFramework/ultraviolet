using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Contains methods for managing a prioritized set of styles which will be applied to an element.
    /// </summary>
    internal partial class UvssStylePrioritizer
    {
        /// <summary>
        /// Resets the style prioritizer's state.
        /// </summary>
        public void Reset()
        {
            this.styles.Clear();
            this.triggers.Clear();
        }

        /// <summary>
        /// Adds a style to the prioritizer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="selector">The selector which caused this style to be considered.</param>
        /// <param name="navigationExpression">The navigation expression associated with the style.</param>
        /// <param name="style">The style to add to the prioritizer.</param>
        public void Add(UltravioletContext uv, UvssSelector selector, NavigationExpression? navigationExpression, UvssStyle style)
        {
            Contract.Require(uv, "uv");

            var key = new StyleKey(style.CanonicalName, navigationExpression);
            var priority = CalculatePriorityFromSelector(selector, style.IsImportant);

            PrioritizedStyle existing;
            if (!styles.TryGetValue(key, out existing))
            {
                styles[key] = new PrioritizedStyle(style, selector, priority);
            }
            else
            {
                if (selector.IsHigherPriorityThan(existing.Selector) && (style.IsImportant || !existing.Style.IsImportant))
                {
                    styles[key] = new PrioritizedStyle(style, selector, priority);
                }
            }
        }

        /// <summary>
        /// Adds a trigger to the prioritizer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="selector">The selector which caused this style to be considered.</param>
        /// <param name="navigationExpression">The navigation expression associated with the style.</param>
        /// <param name="trigger">The trigger to add to the prioritizer.</param>
        public void Add(UltravioletContext uv, UvssSelector selector, NavigationExpression? navigationExpression, Trigger trigger)
        {
            Contract.Require(uv, "uv");

            var key = new StyleKey(trigger.CanonicalName, navigationExpression);
            var priority = CalculatePriorityFromSelector(selector, false);

            PrioritizedTrigger existing;
            if (!triggers.TryGetValue(key, out existing) || selector.IsHigherPriorityThan(existing.Selector))
            {
                triggers[key] = new PrioritizedTrigger(trigger, selector, priority);
            }
        }

        /// <summary>
        /// Applies the current set of styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply the prioritizer's styles.</param>
        public void Apply(UIElement element)
        {
            foreach (var kvp in styles)
            {
                var style    = kvp.Value.Style;
                var selector = kvp.Value.Selector;
                var dprop    = DependencyProperty.FindByStylingName(element.Ultraviolet, element, style.Owner, style.Name);

                element.ApplyStyle(style, selector, kvp.Key.NavigationExpression, dprop);
            }

            foreach (var kvp in triggers)
            {
                if (kvp.Value.Trigger.Actions.Count == 0)
                    continue;

                var target = (DependencyObject)element;
                if (kvp.Key.NavigationExpression.HasValue)
                {
                    target = kvp.Key.NavigationExpression.Value.ApplyExpression(element.Ultraviolet, element);
                }

                if (target != null)
                    kvp.Value.Trigger.AttachInternal(target, true);
            }

            Reset();
        }

        /// <summary>
        /// Calculates the specified selector's relative priority.
        /// </summary>
        /// <param name="selector">The selector to evaluate.</param>
        /// <param name="important">A value indicating whether the style should be considered important.</param>
        /// <returns>The relative priority of the specified selector.</returns>
        private Int32 CalculatePriorityFromSelector(UvssSelector selector, Boolean important)
        {
            const Int32 ImportantStylePriority = 1000000000;
            return selector.Priority + (important ? ImportantStylePriority : 0);
        }

        // State values.
        private readonly Dictionary<StyleKey, PrioritizedStyle> styles = new Dictionary<StyleKey, PrioritizedStyle>();
        private readonly Dictionary<StyleKey, PrioritizedTrigger> triggers = new Dictionary<StyleKey, PrioritizedTrigger>();
    }
}
