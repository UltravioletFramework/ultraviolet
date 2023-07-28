using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a collection of dependency properties are set to certain values.
    /// </summary>
    public sealed partial class UvssPropertyTrigger : UvssTrigger, IDependencyPropertyChangeNotificationSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTrigger"/> class.
        /// </summary>
        /// <param name="isImportant">A value indicating whether the trigger is considered important.</param>
        public UvssPropertyTrigger(Boolean isImportant)
            : base(isImportant)
        {

        }

        /// <inheritdoc/>
        void IDependencyPropertyChangeNotificationSubscriber.ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop)
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
        public UvssPropertyTriggerConditionCollection Conditions
        {
            get { return conditions; }
        }

        /// <inheritdoc/>
        protected override void Attach(DependencyObject dobj)
        {
            if (IsAttachedTo(dobj))
                return;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(Ultraviolet, dobj, condition.PropertyName.Owner, condition.PropertyName.Name);
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.PropertyName, dobj.GetType()));

                DependencyProperty.RegisterChangeNotification(dobj, dprop, this);
            }

            base.Attach(dobj);

            Evaluate(dobj);
        }

        /// <inheritdoc/>
        protected override void Detach(DependencyObject dobj)
        {
            if (!IsAttachedTo(dobj))
                return;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(Ultraviolet, dobj, condition.PropertyName.Owner, condition.PropertyName.Name);
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.PropertyName, dobj.GetType()));

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
        private readonly UvssPropertyTriggerConditionCollection conditions = 
            new UvssPropertyTriggerConditionCollection();
    }
}
