using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger specified by an Ultraviolet style sheet. Triggers can be used
    /// to modify the property values of a dependency object when certain conditions are met.
    /// </summary>
    public abstract class UvssTrigger
    {
        /// <summary>
        /// Initializes a new instance of thhe <see cref="UvssTrigger"/> class.
        /// </summary>
        /// <param name="isImportant">A value indicating whether this trigger is considered important.</param>
        protected UvssTrigger(Boolean isImportant)
        {
            this.IsImportant = isImportant;
        }

        /// <summary>
        /// Attaches the trigger to the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object to which to attach the trigger.</param>
        /// <param name="invokeOnObject">A value indicating whether to invoke <see cref="DependencyObject.AttachTrigger(UvssTrigger)"/> on the dependency object.</param>
        internal void AttachInternal(DependencyObject dobj, Boolean invokeOnObject)
        {
            Attach(dobj);

            if (invokeOnObject)
                dobj.AttachTrigger(this);
        }

        /// <summary>
        /// Attaches the trigger to the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object to which to attach the trigger.</param>
        /// <param name="invokeOnObject">A value indicating whether to invoke <see cref="DependencyObject.DetachTrigger(UvssTrigger)"/> on the dependency object.</param>
        internal void DetachInternal(DependencyObject dobj, Boolean invokeOnObject)
        {
            Detach(dobj);

            if (invokeOnObject)
                dobj.DetachTrigger(this);
        }

        /// <summary>
        /// Attaches the trigger to the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object to which to attach the trigger.</param>
        protected virtual void Attach(DependencyObject dobj)
        {
            CreateAttachment(dobj);
        }

        /// <summary>
        /// Detaches the trigger from the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object from which to detatch the trigger.</param>
        protected virtual void Detach(DependencyObject dobj)
        {
            DeleteAttachment(dobj);
        }

        /// <summary>
        /// Gets the canonical name which uniquely identifies this trigger.
        /// </summary>
        public abstract String CanonicalName
        {
            get;
        }

        /// <summary>
        /// Gets the trigger's collection of associated actions.
        /// </summary>
        public TriggerActionCollection Actions
        {
            get { return actions; }
        }

        /// <summary>
        /// Gets a value indicating whether the style has the !important qualifier.
        /// </summary>
        public Boolean IsImportant { get; }

        /// <summary>
        /// Activates the trigger's associated actions.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the trigger's actions.</param>
        protected void Activate(DependencyObject dobj)
        {
            ActivateAttachment(dobj);
            actions.Activate(Ultraviolet, dobj);
        }

        /// <summary>
        /// Deactivates the trigger's associated actions.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the trigger's actions.</param>
        protected void Deactivate(DependencyObject dobj)
        {
            DeactivateAttachment(dobj);
            actions.Deactivate(Ultraviolet, dobj);
        }

        /// <summary>
        /// Gets a value indicating whether the property trigger is attached to the specified target.
        /// </summary>
        /// <param name="target">The target to evaluate.</param>
        /// <returns><see langword="true"/> if the property trigger is attached to the specified target; otherwise, <see langword="false"/>.</returns>
        protected internal Boolean IsAttachedTo(DependencyObject target)
        {
            return attachments.ContainsKey(target);
        }

        /// <summary>
        /// Gets a value indicating whether the property trigger is activated on the specified target.
        /// </summary>
        /// <param name="target">The target to evaluate.</param>
        /// <returns><see langword="true"/> if the property trigger is activated on the specified target; otherwise, <see langword="false"/>.</returns>
        protected internal Boolean IsActivatedOn(DependencyObject target)
        {
            Boolean activated;
            attachments.TryGetValue(target, out activated);
            return activated;
        }

        /// <summary>
        /// Gets the current Ultraviolet context.
        /// </summary>
        protected UltravioletContext Ultraviolet
        {
            get { return upf.Value.Ultraviolet; }
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
        /// Called when an attached element is clearing its styles.
        /// </summary>
        /// <param name="element">The element that is clearing its styles.</param>
        private void HandleClearingStyles(DependencyObject element)
        {
            if (!IsAttachedTo(element))
                return;

            Detach(element);
        }

        // A singleton reference to the current Presentation Foundation.
        private static readonly UltravioletSingleton<PresentationFoundation> upf =
            new UltravioletSingleton<PresentationFoundation>(uv => uv.GetUI().GetPresentationFoundation());

        // State values.
        private readonly TriggerActionCollection actions = new TriggerActionCollection();
        private readonly Dictionary<DependencyObject, Boolean> attachments = 
            new Dictionary<DependencyObject, Boolean>();
    }
}
