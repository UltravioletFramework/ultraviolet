using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a collection of dependency properties are set to certain values.
    /// </summary>
    public sealed partial class PropertyTrigger : Trigger, IDependencyPropertyChangeNotificationSubscriber
    {
        /// <inheritdoc/>
        public void ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop)
        {
            Evaluate(dobj);
        }

        /// <inheritdoc/>
        public override String CanonicalName
        {
            get { return conditions.CanonicalName; }
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
            if (IsAttachedTo(dobj))
                return;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(Ultraviolet, dobj, condition.DependencyPropertyName.Owner, condition.DependencyPropertyName.Name);
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                DependencyProperty.RegisterChangeNotification(dobj, dprop, this);
            }

            Evaluate(dobj);

            base.Attach(dobj);
        }

        /// <inheritdoc/>
        protected internal override void Detach(DependencyObject dobj)
        {
            if (!IsAttachedTo(dobj))
                return;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(Ultraviolet, dobj, condition.DependencyPropertyName.Owner, condition.DependencyPropertyName.Name);
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                DependencyProperty.UnregisterChangeNotification(dobj, dprop, this);
            }

            if (IsActivatedOn(dobj))
            {
                Deactivate(dobj);
            }

            base.Detach(dobj);
        }

        /// <summary>
        /// Evaluates the trigger's conditions against the specified object and, if they are true, activates the trigger.
        /// </summary>
        private void Evaluate(DependencyObject dobj)
        {
            if (conditions.Evaluate(Ultraviolet, dobj))
            {
                if (!IsActivatedOn(dobj))
                {
                    Activate(dobj);
                }
            }
            else
            {
                if (IsActivatedOn(dobj))
                {
                    Deactivate(dobj);
                }
            }
        }

        // Property values.
        private readonly PropertyTriggerConditionCollection conditions = 
            new PropertyTriggerConditionCollection();
    }
}
