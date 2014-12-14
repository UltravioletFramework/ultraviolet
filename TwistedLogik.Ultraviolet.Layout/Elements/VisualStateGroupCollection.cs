using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a collection of visual state groups belonging to a particular control.
    /// </summary>
    public sealed class VisualStateGroupCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateGroupCollection"/> class.
        /// </summary>
        /// <param name="element">The UI element that owns the collection.</param>
        internal VisualStateGroupCollection(UIElement element)
        {
            Contract.Require(element, "element");

            this.element = element;
        }

        /// <summary>
        /// Gets the visual state group with the specified name, if one exists.
        /// </summary>
        /// <param name="group">The name of the visual state group to retrieve.</param>
        /// <returns>The visual state group with the specified name, or <c>null</c> if no such group exists.</returns>
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
        /// <returns><c>true</c> if the visual state group was created; otherwise, <c>false</c>.</returns>
        public Boolean Create(String group, IEnumerable<String> states)
        {
            Contract.RequireNotEmpty(group, "group");
            Contract.Require(states, "states");

            if (IsDefined(group))
                return false;

            Create(group);

            var vsg = Get(group);
            foreach (var state in states)
            {
                vsg.Create(state);
            }
            return true;
        }

        /// <summary>
        /// Creates a new visual state group.
        /// </summary>
        /// <param name="group">The name of the group to create.</param>
        /// <returns><c>true</c> if the group was created; otherwise, <c>false</c>.</returns>
        public Boolean Create(String group)
        {
            Contract.RequireNotEmpty(group, "group");

            if (groups.ContainsKey(group))
                return false;

            groups[group] = new VisualStateGroup(element, group);
            return true;
        }

        /// <summary>
        /// Destroys the specified visual state group.
        /// </summary>
        /// <param name="group">The name of the group to destroy.</param>
        /// <returns><c>true</c> if the group was destroyed; otherwise, <c>false</c>.</returns>
        public Boolean Destroy(String group)
        {
            Contract.RequireNotEmpty(group, "group");

            return groups.Remove(group);
        }        

        /// <summary>
        /// Gets a value indicating whether this collection defines a visual state group
        /// with the specified name.
        /// </summary>
        /// <param name="group">The name of the visual state group to evaluate.</param>
        /// <returns><c>true</c> if the collection defines a visual state group with
        /// the specified name; otherwise, <c>false</c>.</returns>
        public Boolean IsDefined(String group)
        {
            Contract.RequireNotEmpty(group, "group");

            return groups.ContainsKey(group);
        }

        /// <summary>
        /// Transitions the specified group into the specified state, if it exists.
        /// </summary>
        /// <param name="group">The name of the visual state group to transition.</param>
        /// <param name="state">The name of the state into which to transition the group.</param>
        /// <returns><c>true</c> if the specified group was transitioned into the specified state; otherwise, <c>false</c>.</returns>
        public Boolean GoToState(String group, String state)
        {
            var vsg = Get(group);
            if (vsg == null)
                return false;

            return vsg.GoToState(state);
        }

        /// <summary>
        /// Gets the UI element that owns the collection.
        /// </summary>
        public UIElement Element
        {
            get { return element; }
        }

        // Property values.
        private readonly UIElement element;

        // State values.
        private readonly Dictionary<String, VisualStateGroup> groups = 
            new Dictionary<String, VisualStateGroup>(StringComparer.OrdinalIgnoreCase);
    }
}
