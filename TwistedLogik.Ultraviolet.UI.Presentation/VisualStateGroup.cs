using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Animations;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents one of a UI element's visual state groups.
    /// </summary>
    public sealed partial class VisualStateGroup : IEnumerable<KeyValuePair<String, VisualState>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateGroup"/> class.
        /// </summary>
        /// <param name="element">The framework element that owns the visual state group.</param>
        /// <param name="name">The visual state group's name.</param>
        internal VisualStateGroup(FrameworkElement element, String name)
        {
            Contract.Require(element, nameof(element));
            Contract.RequireNotEmpty(name, nameof(name));

            this.element = element;
            this.name    = name;
        }

        /// <summary>
        /// Resets the group's visual state transitions.
        /// </summary>
        public void ClearVisualStateTransitions()
        {
            transitions.Clear();
        }

        /// <summary>
        /// Sets the transition storyboard which is applied when moving between the specified visual states.
        /// </summary>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="transition">The storyboard to apply when transitioning between the specified visual states.</param>
        /// <returns><see langword="true"/> if the visual state transition was set; otherwise, <see langword="false"/>.</returns>
        public Boolean SetVisualStateTransition(String from, String to, Storyboard transition)
        {
            Contract.RequireNotEmpty(to, nameof(to));

            VisualStateTransitionKey key;
            if (!GetVisualStateTransitionKey(from, to, out key, transitionMustExist: false))
            {
                transition = null;
                return false;
            }

            if (transition != null)
            {
                transitions[key] = transition;
            }
            else
            {
                transitions.Remove(key);
            }
            return true;
        }

        /// <summary>
        /// Retrieves the transition storyboard which is applied when moving between the specified visual states.
        /// </summary>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="transition">The storyboard which is applied when transitioning between the specified visual states.</param>
        /// <returns><see langword="true"/> if the visual state transition was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetVisualStateTransition(String from, String to, out Storyboard transition)
        {
            Contract.RequireNotEmpty(to, nameof(to));

            VisualStateTransitionKey key;
            if (!GetVisualStateTransitionKey(from, to, out key, transitionMustExist: false))
            {
                transition = null;
                return false;
            }
            return transitions.TryGetValue(key, out transition);
        }

        /// <summary>
        /// Gets the visual state with the specified name.
        /// </summary>
        /// <param name="state">The name of the visual state to retrieve.</param>
        /// <returns>The visual state with the specified name, or <see langword="null"/> if no such state exists.</returns>
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
        /// <returns><see langword="true"/> if the visual state was created; otherwise, <see langword="false"/>.</returns>
        public Boolean Create(String state)
        {
            Contract.RequireNotEmpty(state, nameof(state));

            if (states.ContainsKey(state))
                return false;

            var vs = new VisualState(this, state);
            states[state] = vs;

            if (defaultState == null)
            {
                defaultState = vs;
                if (currentState == null)
                {
                    GoToState(state);
                }
            }

            return true;
        }

        /// <summary>
        /// Destroys the specified visual state.
        /// </summary>
        /// <param name="state">The name of the visual state to destroy.</param>
        /// <returns><see langword="true"/> if the visual state was destroyed; otherwise, <see langword="false"/>.</returns>
        public Boolean Destroy(String state)
        {
            Contract.RequireNotEmpty(state, nameof(state));

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
        /// <returns><see langword="true"/> if the group contains a visual state with the specified name; otherwise, <see langword="false"/>.</returns>
        public Boolean IsDefined(String state)
        {
            Contract.RequireNotEmpty(state, nameof(state));

            return states.ContainsKey(state);
        }

        /// <summary>
        /// Transitions the group into the specified state, if it exists.
        /// </summary>
        /// <param name="state">The name of the state into which to transition the group.</param>
        /// <returns><see langword="true"/> if the group was transitioned into the specified state; otherwise, <see langword="false"/>.</returns>
        public Boolean GoToState(String state)
        {
            Contract.RequireNotEmpty(state, nameof(state));

            var vs = Get(state);
            if (vs == null)
                return false;

            if (currentState == vs)
                return false;

            previousState = currentState;
            currentState = vs;

            VisualStateTransitionKey transitionKey;
            if (GetVisualStateTransitionKey(previousState, currentState, out transitionKey, transitionMustExist: true))
            {
                transitions[transitionKey].Begin(Element);
            }

            return true;
        }

        /// <summary>
        /// Reapplies the group's current state.
        /// </summary>
        public void ReapplyState()
        {
            if (currentState != null)
            {
                VisualStateTransitionKey transitionKey;
                if (GetVisualStateTransitionKey(previousState, currentState, out transitionKey, transitionMustExist: true))
                {
                    transitions[transitionKey].Stop(Element);
                    transitions[transitionKey].Begin(Element);
                }
            }
        }

        /// <summary>
        /// Gets the framework element that owns the visual state group.
        /// </summary>
        public FrameworkElement Element
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

        /// <summary>
        /// Gets the <see cref="VisualStateTransitionKey"/> which corresponds to the specified transition, if the appropriate
        /// visual states and transitions are defined within this group.
        /// </summary>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="key">The visual state transition key which was created.</param>
        /// <param name="transitionMustExist">A value indicating whether the specifiied transition must exist in order for a key to be created.</param>
        /// <returns><see langword="true"/> if the visual state transition key was created; otherwise, <see langword="false"/>.</returns>
        private Boolean GetVisualStateTransitionKey(VisualState from, VisualState to, out VisualStateTransitionKey key, Boolean transitionMustExist)
        {
            key = new VisualStateTransitionKey(from, to);
            if (transitionMustExist && !transitions.ContainsKey(key))
            {
                key = new VisualStateTransitionKey(null, to);
                if (transitionMustExist && !transitions.ContainsKey(key))
                {
                    key = default(VisualStateTransitionKey);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the <see cref="VisualStateTransitionKey"/> which corresponds to the specified transition, if the appropriate
        /// visual states and transitions are defined within this group.
        /// </summary>
        /// <param name="from">The visual state which is being transitioned from.</param>
        /// <param name="to">The visual state which is being transitioned to.</param>
        /// <param name="key">The visual state transition key which was created.</param>
        /// <param name="transitionMustExist">A value indicating whether the specifiied transition must exist in order for a key to be created.</param>
        /// <returns><see langword="true"/> if the visual state transition key was created; otherwise, <see langword="false"/>.</returns>
        private Boolean GetVisualStateTransitionKey(String from, String to, out VisualStateTransitionKey key, Boolean transitionMustExist)
        {
            var vsFrom = (VisualState)null;
            if (from != null)
            {
                vsFrom = Get(from);
                if (vsFrom == null)
                {
                    key = default(VisualStateTransitionKey);
                    return false;
                }
            }

            var vsTo = Get(to);
            if (vsTo == null)
            {
                key = default(VisualStateTransitionKey);
                return false;
            }

            return GetVisualStateTransitionKey(vsFrom, vsTo, out key, transitionMustExist);
        }

        // Property values.
        private readonly FrameworkElement element;
        private readonly String name;

        // State values.
        private readonly Dictionary<String, VisualState> states = 
            new Dictionary<String, VisualState>(StringComparer.OrdinalIgnoreCase);
        private VisualState defaultState;
        private VisualState previousState;
        private VisualState currentState;

        // Visual state transitions for this group.
        private readonly Dictionary<VisualStateTransitionKey, Storyboard> transitions = 
            new Dictionary<VisualStateTransitionKey, Storyboard>();
    }
}
