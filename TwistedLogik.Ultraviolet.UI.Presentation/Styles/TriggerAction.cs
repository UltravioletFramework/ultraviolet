
namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents an action perform by a trigger.
    /// </summary>
    public abstract class TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAction"/> class.
        /// </summary>
        internal TriggerAction()
        {

        }

        /// <summary>
        /// Activates the action.
        /// </summary>
        /// <param name="dobj">The dependency object for which to activate the action.</param>
        public virtual void Activate(DependencyObject dobj) { }

        /// <summary>
        /// Deactivates the action.
        /// </summary>
        /// <param name="dobj">The dependency object for which to deactivate the action.</param>
        public virtual void Deactivate(DependencyObject dobj) { }
    }
}
