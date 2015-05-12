
namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger specified by an Ultraviolet stylesheet. Triggers can be used
    /// to modify the property values of a dependency object when certain conditions are met.
    /// </summary>
    public abstract class Trigger
    {
        /// <summary>
        /// Attaches the trigger to the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object to which to attach the trigger.</param>
        protected internal abstract void Attach(DependencyObject dobj);

        /// <summary>
        /// Detaches the trigger from the specified dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object from which to detatch the trigger.</param>
        protected internal abstract void Detach(DependencyObject dobj);

        /// <summary>
        /// Gets the trigger's collection of associated actions.
        /// </summary>
        public TriggerActionCollection Actions
        {
            get { return actions; }
        }

        /// <summary>
        /// Activates the trigger's associated actions.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the trigger's actions.</param>
        protected void Activate(DependencyObject dobj)
        {
            actions.Activate(dobj);
        }

        /// <summary>
        /// Deactivates the trigger's associated actions.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the trigger's actions.</param>
        protected void Deactivate(DependencyObject dobj)
        {
            actions.Deactivate(dobj);
        }

        // State values.
        private readonly TriggerActionCollection actions = 
            new TriggerActionCollection();
    }
}
