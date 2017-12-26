using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Animations;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a collection of visual state groups belonging to a particular control.
    /// </summary>
    public sealed partial class VisualStateGroupCollection : IEnumerable<KeyValuePair<String, VisualStateGroup>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateGroupCollection"/> class.
        /// </summary>
        /// <param name="element">The framework element that owns the collection.</param>
        internal VisualStateGroupCollection(FrameworkElement element)
        {
            Contract.Require(element, nameof(element));

            this.element = element;
        }

        /// <summary>
        /// Resets the collection's visual state transitions.
        /// </summary>
        public void ClearVisualStateTransitions()
        {
            foreach (var vsg in groups)
            {
                vsg.Value.ClearVisualStateTransitions();
            }
        }

        /// <summary>
        /// Sets the transition storyboard which is applied when moving between the specified visual states.
        /// </summary>
        /// <param name="group">The visual state group that contains the states.</param>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="transition">The storyboard to apply when transitioning between the specified visual states.</param>
        /// <returns><see langword="true"/> if the visual state transition was set; otherwise, <see langword="false"/>.</returns>
        public Boolean SetVisualStateTransition(String group, String from, String to, Storyboard transition)
        {
            Contract.RequireNotEmpty(group, nameof(group));
            Contract.RequireNotEmpty(to, nameof(to));

            var vsg = Get(group);
            if (vsg == null)
                return false;

            return vsg.SetVisualStateTransition(from, to, transition);
        }

        /// <summary>
        /// Retrieves the transition storyboard which is applied when moving between the specified visual states.
        /// </summary>
        /// <param name="group">The visual state group that contains the states.</param>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="transition">The storyboard which is applied when transitioning between the specified visual states.</param>
        /// <returns><see langword="true"/> if the visual state transition was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetVisualStateTransition(String group, String from, String to, out Storyboard transition)
        {
            Contract.RequireNotEmpty(group, nameof(group));
            Contract.RequireNotEmpty(to, nameof(to));

            var vsg = Get(group);
            if (vsg == null)
            {
                transition = null;
                return false;
            }

            return vsg.GetVisualStateTransition(from, to, out transition);
        }

        /// <summary>
        /// Gets the visual state group with the specified name, if one exists.
        /// </summary>
        /// <param name="group">The name of the visual state group to retrieve.</param>
        /// <returns>The visual state group with the specified name, or <see langword="null"/> if no such group exists.</returns>
        public VisualStateGroup Get(String group)
        {
            VisualStateGroup g;
            groups.TryGetValue(group, out g);
            return g;
        }

        /// <summary>
        /// Creates the specified visual state group.
        /// </summary>
        /// <param name="group">The name of the visual state group which will contain the states.</param>
        /// <param name="states">The list of visual states to create.</param>
        /// <returns><see langword="true"/> if the visual state group was created; otherwise, <see langword="false"/>.</returns>
        public Boolean Create(String group, IEnumerable<String> states)
        {
            Contract.RequireNotEmpty(group, nameof(group));
            Contract.Require(states, nameof(states));

            var hasGroups = groups.Count > 0;

            if (IsDefined(group))
                Destroy(group);

            Create(group);

            var str = String.Empty;

            var vsg = Get(group);
            foreach (var state in states)
            {
                if (str == String.Empty)
                    str = state;

                vsg.Create(state);
            }

            if (!hasGroups && !String.IsNullOrEmpty(str))
                GoToState(group, str);

            return true;
        }

        /// <summary>
        /// Creates a new visual state group.
        /// </summary>
        /// <param name="group">The name of the group to create.</param>
        /// <returns><see langword="true"/> if the group was created; otherwise, <see langword="false"/>.</returns>
        public Boolean Create(String group)
        {
            Contract.RequireNotEmpty(group, nameof(group));

            if (groups.ContainsKey(group))
                return false;

            groups[group] = new VisualStateGroup(element, group);
            return true;
        }

        /// <summary>
        /// Destroys the specified visual state group.
        /// </summary>
        /// <param name="group">The name of the group to destroy.</param>
        /// <returns><see langword="true"/> if the group was destroyed; otherwise, <see langword="false"/>.</returns>
        public Boolean Destroy(String group)
        {
            Contract.RequireNotEmpty(group, nameof(group));

            return groups.Remove(group);
        }        

        /// <summary>
        /// Gets a value indicating whether this collection defines a visual state group
        /// with the specified name.
        /// </summary>
        /// <param name="group">The name of the visual state group to evaluate.</param>
        /// <returns><see langword="true"/> if the collection defines a visual state group with
        /// the specified name; otherwise, <see langword="false"/>.</returns>
        public Boolean IsDefined(String group)
        {
            Contract.RequireNotEmpty(group, nameof(group));

            return groups.ContainsKey(group);
        }

        /// <summary>
        /// Transitions the specified group into the specified state, if it exists.
        /// </summary>
        /// <param name="group">The name of the visual state group to transition.</param>
        /// <param name="state">The name of the state into which to transition the group.</param>
        /// <returns><see langword="true"/> if the specified group was transitioned into the specified state; otherwise, <see langword="false"/>.</returns>
        public Boolean GoToState(String group, String state)
        {
            var vsg = Get(group);
            if (vsg == null)
                return false;

            return vsg.GoToState(state);
        }

        /// <summary>
        /// Reapplies the collection's current set of states.
        /// </summary>
        public void ReapplyStates()
        {
            foreach (var vsg in groups)
            {
                vsg.Value.ReapplyState();
            }
        }

        /// <summary>
        /// Gets the framework element that owns the collection.
        /// </summary>
        public FrameworkElement Element
        {
            get { return element; }
        }

        // Property values.
        private readonly FrameworkElement element;

        // State values.
        private readonly Dictionary<String, VisualStateGroup> groups = 
            new Dictionary<String, VisualStateGroup>(StringComparer.OrdinalIgnoreCase);
    }
}
