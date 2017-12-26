using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
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
            this.rules.Clear();
            this.triggers.Clear();
        }

        /// <summary>
        /// Adds a styling rule to the prioritizer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="selector">The selector which caused this style to be considered.</param>
        /// <param name="navigationExpression">The navigation expression associated with the style.</param>
        /// <param name="rule">The styling rule to add to the prioritizer.</param>
        /// <param name="index">The index of the rule's rule set within the style sheet.</param>
        public void Add(UltravioletContext uv, UvssSelector selector, NavigationExpression? navigationExpression, UvssRule rule, Int32 index)
        {
            Contract.Require(uv, nameof(uv));

            var key = new StyleKey(rule.CanonicalName, navigationExpression);
            var priority = CalculatePriorityFromSelector(selector, rule.IsImportant);

            PrioritizedStyle existing;
            if (!rules.TryGetValue(key, out existing))
            {
                rules[key] = new PrioritizedStyle(rule, selector, priority, index);
            }
            else
            {
                var comparison = selector.ComparePriority(existing.Selector);
                if (comparison == 0 && index > existing.Index)
                    comparison = 1;

                if (comparison > 0 && (rule.IsImportant || !existing.Style.IsImportant))
                {
                    rules[key] = new PrioritizedStyle(rule, selector, priority, index);
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
        /// <param name="index">The index of the trigger's rule set within the style sheet.</param>
        public void Add(UltravioletContext uv, UvssSelector selector, NavigationExpression? navigationExpression, UvssTrigger trigger, Int32 index)
        {
            Contract.Require(uv, nameof(uv));

            var key = new StyleKey(trigger.CanonicalName, navigationExpression);
            var priority = CalculatePriorityFromSelector(selector, false);

            PrioritizedTrigger existing;
            if (!triggers.TryGetValue(key, out existing))
            {
                triggers[key] = new PrioritizedTrigger(trigger, selector, priority, index);
            }
            else
            {
                var comparison = selector.ComparePriority(existing.Selector);
                if (comparison == 0 && index > existing.Index)
                    comparison = 1;

                if (comparison > 0 && (trigger.IsImportant || !existing.Trigger.IsImportant))
                {
                    triggers[key] = new PrioritizedTrigger(trigger, selector, priority, index);
                }
            }
        }

        /// <summary>
        /// Applies the current set of styles to the specified element.
        /// </summary>
        /// <param name="element">The element to which to apply the prioritizer's styles.</param>
        public void Apply(UIElement element)
        {
            foreach (var kvp in rules)
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
        private readonly Dictionary<StyleKey, PrioritizedStyle> rules = new Dictionary<StyleKey, PrioritizedStyle>();
        private readonly Dictionary<StyleKey, PrioritizedTrigger> triggers = new Dictionary<StyleKey, PrioritizedTrigger>();
    }
}
