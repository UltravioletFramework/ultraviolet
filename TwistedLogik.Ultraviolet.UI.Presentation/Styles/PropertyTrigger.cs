using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a collection of dependency properties are set to certain values.
    /// </summary>
    public sealed partial class PropertyTrigger : Trigger, IDependencyPropertyChangeNotificationSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTrigger"/> class.
        /// </summary>
        public PropertyTrigger()
        {
            cachedDelegateHandleClearingStyles = HandleClearingStyles;
        }

        /// <inheritdoc/>
        public void ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop)
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
            if (IsAttachedTo(dobj))
                return;

            CreateAttachment(dobj);

            dobj.ClearingStyles += cachedDelegateHandleClearingStyles;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(condition.DependencyPropertyName, dobj.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                DependencyProperty.RegisterChangeNotification(dobj, dprop, this);
            }

            Evaluate(dobj);
        }

        /// <inheritdoc/>
        protected internal override void Detach(DependencyObject dobj)
        {
            if (!IsAttachedTo(dobj))
                return;

            var wasActivated = IsActivatedOn(dobj);

            DeleteAttachment(dobj);

            dobj.ClearingStyles -= cachedDelegateHandleClearingStyles;

            foreach (var condition in conditions)
            {
                var dprop = DependencyProperty.FindByStylingName(condition.DependencyPropertyName, dobj.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(condition.DependencyPropertyName, dobj.GetType()));

                DependencyProperty.UnregisterChangeNotification(dobj, dprop, this);
            }

            if (wasActivated)
                Deactivate(dobj);
        }

        /// <summary>
        /// Gets a value indicating whether the property trigger is attached to the specified target.
        /// </summary>
        /// <param name="target">The target to evaluate.</param>
        /// <returns><c>true</c> if the property trigger is attached to the specified target; otherwise, <c>false</c>.</returns>
        private Boolean IsAttachedTo(DependencyObject target)
        {
            return attachments.ContainsKey(target);
        }

        /// <summary>
        /// Gets a value indicating whether the property trigger is activated on the specified target.
        /// </summary>
        /// <param name="target">The target to evaluate.</param>
        /// <returns><c>true</c> if the property trigger is activated on the specified target; otherwise, <c>false</c>.</returns>
        private Boolean IsActivatedOn(DependencyObject target)
        {
            Boolean activated;
            attachments.TryGetValue(target, out activated);
            return activated;
        }

        /// <summary>
        /// Creates an attachment with the specified target.
        /// </summary>
        /// <param name="target">The target to which to attach the trigger.</param>
        private void CreateAttachment(DependencyObject target)
        {
            attachments.Add(target, false);
        }

        /// <summary>
        /// Deletes an attachment with the specified target.
        /// </summary>
        /// <param name="target">The target from which to detach the trigger.</param>
        private void DeleteAttachment(DependencyObject target)
        {
            attachments.Remove(target);
        }

        /// <summary>
        /// Activates the trigger's attachment with the specified target.
        /// </summary>
        /// <param name="target">The target to activate.</param>
        private void ActivateAttachment(DependencyObject target)
        {
            if (attachments.ContainsKey(target))
            {
                attachments[target] = true;
            }
        }

        /// <summary>
        /// Deactivates the trigger's attachment with the specified target.
        /// </summary>
        /// <param name="target">The target to deactivate.</param>
        private void DeactivateAttachment(DependencyObject target)
        {
            if (attachments.ContainsKey(target))
            {
                attachments[target] = false;
            }
        }

        /// <summary>
        /// Evaluates the trigger's conditions against the specified object and, if they are true, activates the trigger.
        /// </summary>
        private void Evaluate(DependencyObject dobj)
        {
            if (conditions.Evaluate(dobj))
            {
                if (!IsActivatedOn(dobj))
                {
                    ActivateAttachment(dobj);
                    Activate(dobj);
                }
            }
            else
            {
                if (IsActivatedOn(dobj))
                {
                    DeactivateAttachment(dobj);
                    Deactivate(dobj);
                }
            }
        }

        /// <summary>
        /// Called when an attached element is clearing its styles.
        /// </summary>
        /// <param name="element">The element that is clearing its styles.</param>
        private void HandleClearingStyles(DependencyObject element)
        {
            if (!IsAttachedTo(element))
                return;

            Detach(element);
        }

        // Property values.
        private readonly PropertyTriggerConditionCollection conditions = 
            new PropertyTriggerConditionCollection();

        // State values.
        private readonly UpfEventHandler cachedDelegateHandleClearingStyles;
        private readonly Dictionary<DependencyObject, Boolean> attachments = 
            new Dictionary<DependencyObject, Boolean>();
    }
}
