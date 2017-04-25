using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a "set" trigger action, which sets the value of a dependency property when it is activated.
    /// </summary>
    public sealed class SetTriggerAction : TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTriggerAction"/> class.
        /// </summary>
        /// <param name="selector">A UVSS selector which specifies the target (or targets) of the action.</param>
        /// <param name="propertyName">The styling name of the dependency property which is set by this action.</param>
        /// <param name="propertyValue">The value to which the action sets its associated dependency property.</param>
        internal SetTriggerAction(UvssSelector selector, DependencyName propertyName, DependencyValue propertyValue)
        {
            this.selector = selector;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

        /// <inheritdoc/>
        public override void Activate(UltravioletContext uv, DependencyObject dobj)
        {
            if (selector == null)
            {
                var dprop = DependencyProperty.FindByStylingName(uv, dobj, propertyName.Owner, propertyName.Name);
                if (dprop != null)
                {
                    var clear = propertyValue.IsEmpty;
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
                    var rooted = selector.PartCount == 0 ? false :
                        String.Equals(selector[0].PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                    var target = rooted ? dobj as UIElement : null;

                    element.View.Select(target, selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        var propName = action.propertyName;
                        var propValue = action.propertyValue;

                        var dprop = DependencyProperty.FindByStylingName(e.Ultraviolet, e, propName.Owner, propName.Name);
                        if (dprop != null)
                        {
                            if (propValue.IsEmpty)
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
            base.Activate(uv, dobj);
        }

        /// <inheritdoc/>
        public override void Deactivate(UltravioletContext uv, DependencyObject dobj)
        {
            if (selector == null)
            {
                var dprop = DependencyProperty.FindByStylingName(uv, dobj, propertyName.Owner, propertyName.Name);
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
                    var rooted = selector.PartCount == 0 ? false :
                        String.Equals(selector[0].PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                    var target = rooted ? dobj as UIElement : null;

                    element.View.Select(target, selector, this, (e, s) =>
                    {
                        var action = (SetTriggerAction)s;
                        var dprop = DependencyProperty.FindByStylingName(e.Ultraviolet, e, action.propertyName.Owner, action.propertyName.Name);
                        if (dprop != null)
                        {
                            e.ClearTriggeredValue(dprop, action);
                        }
                    });
                }
            }
            base.Deactivate(uv, dobj);
        }

        /// <summary>
        /// Gets the value which is set by this action.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <returns>The value which is set by this action.</returns>
        public T GetValue<T>()
        {
            if (valueCache == null || valueCache.GetType() != typeof(T))
                valueCache = ObjectResolver.FromString(propertyValue.Value, typeof(T), propertyValue.Culture);

            return (T)valueCache;
        }
        
        /// <summary>
        /// Gets the UVSS selector which specifies the target (or targets) of the action.
        /// </summary>
        /// <value>A <see cref="UvssSelector"/> which specifies the target of the action.</value>
        public UvssSelector Selector
        {
            get { return selector; }
        }

        /// <summary>
        /// Gets the name of the dependency property which is set by the action.
        /// </summary>
        /// <value>A <see cref="Presentation.DependencyName"/> which contains the UVSS styling name of 
        /// the dependency property which is set by the action.</value>
        public DependencyName PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets the value to which the action sets its associated dependency property.
        /// </summary>
        /// <value>A <see cref="Presentation.DependencyValue"/> that represents the value which will be set by the action.
        /// The value will be parsed into the appropriate type via the <see cref="ObjectLoader"/> class.</value>
        public DependencyValue PropertyValue
        {
            get { return propertyValue; }
        }
        
        // State values.
        private readonly UvssSelector selector;
        private readonly DependencyName propertyName;
        private readonly DependencyValue propertyValue;
        private Object valueCache;
    }
}
