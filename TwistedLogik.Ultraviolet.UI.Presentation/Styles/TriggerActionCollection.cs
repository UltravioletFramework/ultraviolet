using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger's collection of associated actions.
    /// </summary>
    public sealed partial class TriggerActionCollection
    {
        /// <summary>
        /// Activates the actions in the collection, with the specified dependency object as their implicit target.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the activated actions.</param>
        internal void Activate(DependencyObject dobj)
        {
            foreach (var action in actions)
            {
                action.Activate(dobj);
            }
        }

        /// <summary>
        /// Deactivates the actions in the collection, with the specified dependency object as their implicit target.
        /// </summary>
        /// <param name="dobj">The dependency object which is the implicit target of the deactivated actions.</param>
        internal void Deactivate(DependencyObject dobj)
        {
            foreach (var action in actions)
            {
                action.Deactivate(dobj);
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        internal void Clear()
        {
            actions.Clear();
        }

        /// <summary>
        /// Adds an action to the collection.
        /// </summary>
        /// <param name="action">The action to add to the collection.</param>
        internal void Add(TriggerAction action)
        {
            Contract.Require(action, "action");

            actions.Add(action);
        }

        /// <summary>
        /// Removes an action from the collection.
        /// </summary>
        /// <param name="action">The action to remove from the collection.</param>
        /// <returns><c>true</c> if the action was removed from the collection; otherwise, <c>false</c>.</returns>
        internal Boolean Remove(TriggerAction action)
        {
            Contract.Require(action, "action");

            return actions.Remove(action);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified action.
        /// </summary>
        /// <param name="action">The action to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified action; otherwise, <c>false</c>.</returns>
        internal Boolean Contains(TriggerAction action)
        {
            Contract.Require(action, "action");

            return actions.Contains(action);
        }

        /// <summary>
        /// Gets the number of actions in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return actions.Count; }
        }

        // The underlying list of trigger actions.
        private readonly List<TriggerAction> actions = 
            new List<TriggerAction>();
    }
}
