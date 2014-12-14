using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Layout.Animation;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents one of a UI element's visual state groups.
    /// </summary>
    public class VisualStateGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateGroup"/> class.
        /// </summary>
        /// <param name="element">The UI element that owns the visual state group.</param>
        /// <param name="name">The visual state group's name.</param>
        internal VisualStateGroup(UIElement element, String name)
        {
            Contract.Require(element, "element");
            Contract.RequireNotEmpty(name, "name");

            this.element = element;
            this.name    = name;
        }

        /// <summary>
        /// Gets the visual state with the specified name.
        /// </summary>
        /// <param name="state">The name of the visual state to retrieve.</param>
        /// <returns>The visual state with the specified name, or <c>null</c> if no such state exists.</returns>
        public VisualState Get(String state)
        {
            VisualState vs;
            states.TryGetValue(state, out vs);
            return vs;
        }
        
        /// <summary>
        /// Creates a new visual state within the group.
        /// </summary>
        /// <param name="state">The name of the visual state to create.</param>
        /// <returns><c>true</c> if the visual state was created; otherwise, <c>false</c>.</returns>
        public Boolean Create(String state)
        {
            Contract.RequireNotEmpty(state, "state");

            if (states.ContainsKey(state))
                return false;

            var vs = new VisualState(state);
            states[state] = vs;

            if (defaultState == null)
                defaultState = vs;

            return true;
        }

        /// <summary>
        /// Destroys the specified visual state.
        /// </summary>
        /// <param name="state">The name of the visual state to destroy.</param>
        /// <returns><c>true</c> if the visual state was destroyed; otherwise, <c>false</c>.</returns>
        public Boolean Destroy(String state)
        {
            Contract.RequireNotEmpty(state, "state");

            VisualState vs;
            if (states.TryGetValue(state, out vs))
            {
                states.Remove(state);

                if (defaultState == vs)
                    defaultState = states.Any() ? states.First().Value : null;

                if (currentState == vs && defaultState != null)
                    GoToState(defaultState.Name);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the group contains a visual state with the specified name.
        /// </summary>
        /// <param name="state">The name of the visual state to evaluate.</param>
        /// <returns><c>true</c> if the group contains a visual state with the specified name; otherwise, <c>false</c>.</returns>
        public Boolean IsDefined(String state)
        {
            Contract.RequireNotEmpty(state, "state");

            return states.ContainsKey(state);
        }

        /// <summary>
        /// Transitions the group into the specified state, if it exists.
        /// </summary>
        /// <param name="state">The name of the state into which to transition the group.</param>
        /// <returns><c>true</c> if the group was transitioned into the specified state; otherwise, <c>false</c>.</returns>
        public Boolean GoToState(String state)
        {
            Contract.RequireNotEmpty(state, "state");

            var vs = Get(state);
            if (vs == null)
                return false;

            if (currentState == vs)
                return false;

            currentState = vs;

            if (currentState.Transition != null)
            {
                currentTransition = currentState.Transition;
                currentTransition.Begin(element);
            }
            else
            {
                if (currentTransition != null)
                {
                    currentTransition.Stop(element);
                    currentTransition = null;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the UI element that owns the visual state group.
        /// </summary>
        public UIElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the visual state group's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the name of the group's current state.
        /// </summary>
        public String CurrentStateName
        {
            get { return currentState == null ? null : currentState.Name; }
        }

        /// <summary>
        /// Gets the group's current state.
        /// </summary>
        public VisualState CurrentState
        {
            get { return currentState; }
        }

        // Property values.
        private readonly UIElement element;
        private readonly String name;

        // State values.
        private readonly Dictionary<String, VisualState> states = 
            new Dictionary<String, VisualState>(StringComparer.OrdinalIgnoreCase);
        private VisualState defaultState;
        private VisualState currentState;
        private Storyboard currentTransition;
    }
}
