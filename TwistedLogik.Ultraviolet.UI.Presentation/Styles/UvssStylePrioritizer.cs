using System;
using System.Collections.Generic;

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
        /// <param name="selector">The selector which caused this style to be considered.</param>
        /// <param name="style">The style to add to the prioritizer.</param>
        public void Add(UvssSelector selector, UvssStyle style)
        {
            var priority = CalculatePriorityFromSelector(selector, style.IsImportant);

            PrioritizedStyle existing;
            if (!styles.TryGetValue(style.CanonicalName, out existing) || existing.Priority <= priority)
            {
                styles[style.CanonicalName] = new PrioritizedStyle(style, selector, priority);
            }
        }

        /// <summary>
        /// Adds a trigger to the prioritizer.
        /// </summary>
        /// <param name="selector">The selector which caused this style to be considered.</param>
        /// <param name="trigger">The trigger to add to the prioritizer.</param>
        public void Add(UvssSelector selector, Trigger trigger)
        {
            var priority = CalculatePriorityFromSelector(selector, false);

            PrioritizedTrigger existing;
            if (!triggers.TryGetValue(trigger.CanonicalName, out existing) || existing.Priority <= priority)
            {
                triggers[trigger.CanonicalName] = new PrioritizedTrigger(trigger, selector, priority);
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

                element.ApplyStyle(style, selector, dprop);
            }

            foreach (var kvp in triggers)
            {
                if (kvp.Value.Trigger.Actions.Count == 0)
                    continue;

                kvp.Value.Trigger.Attach(element);
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
        private readonly Dictionary<String, PrioritizedStyle> styles = new Dictionary<String, PrioritizedStyle>();
        private readonly Dictionary<String, PrioritizedTrigger> triggers = new Dictionary<String, PrioritizedTrigger>();
    }
}
