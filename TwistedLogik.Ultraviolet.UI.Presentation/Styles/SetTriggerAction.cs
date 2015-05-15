using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a "set" trigger action, which sets the value of a dependency property when it is activated.
    /// </summary>
    public sealed class SetTriggerAction : TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTriggerAction"/> class.
        /// </summary>
        /// <param name="dpropName">The styling name of the dependency property which is set by this action.</param>
        /// <param name="selector">A UVSS selector which specifies the target (or targets) of the action.</param>
        /// <param name="value">The value which is provided by this action.</param>
        internal SetTriggerAction(String dpropName, UvssSelector selector, String value)
        {
            Contract.RequireNotEmpty(dpropName, "dpropName");

            this.dpropName = dpropName;
            this.selector  = selector;
            this.value     = value;
        }

        /// <inheritdoc/>
        public override void Activate(DependencyObject dobj)
        {
            if (selector == null)
            {
                var dprop = DependencyProperty.FindByStylingName(dpropName, dobj.GetType());
                if (dprop != null)
                {
                    var clear = String.IsNullOrEmpty(value);
                    if (clear)
                    {
                        dobj.ClearTriggeredValue(dprop);
                    }
                    else
                    {
                        dobj.SetTriggeredValue(dprop, this);
                    }
                }
            }
            else
            {
                var element = dobj as UIElement;
                if (element != null && element.View != null)
                {
                    var rooted = String.Equals(selector.PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                    var target = rooted ? dobj as UIElement : null;

                    element.View.Select(target, selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        var value  = action.value;

                        var dprop = DependencyProperty.FindByStylingName(action.dpropName, e.GetType());
                        if (dprop != null)
                        {
                            var clear = String.IsNullOrEmpty(value);
                            if (clear)
                            {
                                e.ClearTriggeredValue(dprop);
                            }
                            else
                            {
                                e.SetTriggeredValue(dprop, action);
                            }
                        }
                    });
                }
            }
            base.Activate(dobj);
        }

        /// <inheritdoc/>
        public override void Deactivate(DependencyObject dobj)
        {
            if (selector == null)
            {
                var dprop = DependencyProperty.FindByStylingName(dpropName, dobj.GetType());
                if (dprop != null)
                {
                    dobj.ClearTriggeredValue(dprop, this);
                }
            }
            else
            {
                var element = dobj as UIElement;
                if (element != null && element.View != null)
                {
                    var rooted = String.Equals(selector.PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                    var target = rooted ? dobj as UIElement : null;

                    element.View.Select(target, selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        var dprop = DependencyProperty.FindByStylingName(action.dpropName, e.GetType());
                        if (dprop != null)
                        {
                            e.ClearTriggeredValue(dprop, action);
                        }
                    });
                }
            }
            base.Deactivate(dobj);
        }

        /// <summary>
        /// Gets the value which is set by this action.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <returns>The value which is set by this action.</returns>
        public T GetValue<T>()
        {
            if (valueCache == null || valueCache.GetType() != typeof(T))
                valueCache = ObjectResolver.FromString(value, typeof(T));

            return (T)valueCache;
        }

        // State values.
        private readonly String dpropName;
        private readonly UvssSelector selector;
        private readonly String value;
        private Object valueCache;
    }
}
