
namespace Ultraviolet.Presentation.Styles
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
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object for which to activate the action.</param>
        public virtual void Activate(UltravioletContext uv, DependencyObject dobj) { }

        /// <summary>
        /// Deactivates the action.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object for which to deactivate the action.</param>
        public virtual void Deactivate(UltravioletContext uv, DependencyObject dobj) { }
    }
}
