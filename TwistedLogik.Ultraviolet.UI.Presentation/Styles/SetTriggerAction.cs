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
        /// <param name="dprop">The dependency property which is set by this action.</param>
        /// <param name="selector">A UVSS selector which specifies the target (or targets) of the action.</param>
        /// <param name="value">The value which is provided by this action.</param>
        internal SetTriggerAction(DependencyProperty dprop, UvssSelector selector, String value)
        {
            Contract.Require(dprop, "dprop");
            Contract.RequireNotEmpty(value, "value");

            this.dprop    = dprop;
            this.selector = selector;
            this.value    = value;
        }

        /// <inheritdoc/>
        public override void Activate(DependencyObject dobj)
        {
            if (selector == null)
            {
                dobj.SetTriggeredValue(dprop, this);
            }
            else
            {
                var element = dobj as UIElement;
                if (element != null && element.View != null)
                {
                    element.View.Select(selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        e.SetTriggeredValue(action.dprop, action);
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
                dobj.ClearTriggeredValue(dprop, this);
            }
            else
            {
                var element = dobj as UIElement;
                if (element != null && element.View != null)
                {
                    element.View.Select(selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        e.ClearTriggeredValue(action.dprop, action);
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
        private readonly DependencyProperty dprop;
        private readonly UvssSelector selector;
        private readonly String value;
        private Object valueCache;
    }
}
