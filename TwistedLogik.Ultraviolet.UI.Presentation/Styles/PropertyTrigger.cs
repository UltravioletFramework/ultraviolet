using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a collection of dependency properties are set to certain values.
    /// </summary>
    public sealed class PropertyTrigger : Trigger, IDependencyPropertyChangeNotificationSubscriber
    {
        /// <inheritdoc/>
        void IDependencyPropertyChangeNotificationSubscriber.ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop)
        {
            Evaluate(dobj);
        }

        /// <summary>
        /// Gets the trigger's collection of conditions.
        /// </summary>
        public PropertyTriggerConditionCollection Conditions
        {
            get { return conditions; }
        }

        /// <inheritdoc/>
        protected internal override void Attach(DependencyObject dobj)
        {
            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(condition.DependencyPropertyName, dobj.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                Evaluate(dobj);

                DependencyProperty.RegisterChangeNotification(dobj, dprop, this);
            }
        }

        /// <inheritdoc/>
        protected internal override void Detach(DependencyObject dobj)
        {
            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(condition.DependencyPropertyName, dobj.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                // TODO: Clear triggered property values

                DependencyProperty.UnregisterChangeNotification(dobj, dprop, this);
            }
        }

        /// <summary>
        /// Evaluates the trigger's conditions against the specified object and, if they are true, activates the trigger.
        /// </summary>
        private void Evaluate(DependencyObject dobj)
        {
            if (conditions.Evaluate(dobj))
            {
                // TODO
            }
        }

        // Property values.
        private readonly PropertyTriggerConditionCollection conditions = 
            new PropertyTriggerConditionCollection();
    }
}
