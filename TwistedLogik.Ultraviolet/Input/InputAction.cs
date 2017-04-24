using System;
using Ultraviolet.Core;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when an input action is pressed or released.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that raised the event.</param>
    public delegate void InputActionEventHandler(InputAction action);

    /// <summary>
    /// Represents an input action associated with one or more input bindings.
    /// </summary>
    public sealed class InputAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputAction"/> class.
        /// </summary>
        internal InputAction(InputActionCollection collection)
        {
            Contract.Require(collection, nameof(collection));

            this.collection = collection;
        }

        /// <summary>
        /// Updates the input action's state.
        /// </summary>
        public void Update()
        {
            released = false;
            if (pressed == null)
            {
                if (enabled && primary != null && primary.IsPressed())
                {
                    pressed = primary;
                    OnPressed();
                    return;
                }
                if (enabled && secondary != null && secondary.IsPressed())
                {
                    pressed = secondary;
                    OnPressed();
                    return;
                }
            }
            else
            {
                if (!enabled || pressed.IsReleased())
                {
                    released = true;
                    pressed = null;
                    OnReleased();
                }
            }
        }

        /// <summary>
        /// Adjusts the input action's priority by the specified amount.
        /// </summary>
        /// <remarks>Higher values indicate higher priority.</remarks>
        /// <param name="priorityAdjustment">The amount by which to adjust the action's priority, 
        /// or <see langword="null"/> to reset the input action's priority adjustment.</param>
        public void AdjustPriority(Int32? priorityAdjustment)
        {
            this.priorityAdjustment = priorityAdjustment;

            if (this.primary != null)
                this.primary.AdjustPriority(priorityAdjustment);

            if (this.secondary != null)
                this.secondary.AdjustPriority(priorityAdjustment);
        }

        /// <summary>
        /// Gets a value indicating whether the binding is down.
        /// </summary>
        /// <returns><see langword="true"/> if the binding is down; otherwise, <see langword="false"/>.</returns>
        public Boolean IsDown()
        {
            return pressed != null && pressed.IsDown();
        }

        /// <summary>
        /// Gets a value indicating whether the binding is up.
        /// </summary>
        /// <returns><see langword="true"/> if the binding is up; otherwise, <see langword="false"/>.</returns>
        public Boolean IsUp()
        {
            return pressed == null || pressed.IsUp();
        }
        
        /// <summary>
        /// Gets a value indicating whether the binding was pressed this frame.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns><see langword="true"/> if the binding was pressed this frame; otherwise, <see langword="false"/>.</returns>
        public Boolean IsPressed(Boolean ignoreRepeats = true)
        {
            return pressed != null && pressed.IsPressed(ignoreRepeats);
        }

        /// <summary>
        /// Gets a value indicating whether the binding was released this frame.
        /// </summary>
        /// <returns><see langword="true"/> if the binding was released this frame; otherwise, <see langword="false"/>.</returns>
        public Boolean IsReleased()
        {
            return released;
        }

        /// <summary>
        /// Gets the binding's current state.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>A <see cref="ButtonState"/> value representing the binding's current state.</returns>
        public ButtonState GetState(Boolean ignoreRepeats = true)
        {
            var state = ButtonState.Up;

            if (IsPressed(ignoreRepeats))
                state |= ButtonState.Pressed;

            if (IsReleased())
                state |= ButtonState.Released;

            if (IsDown())
                state |= ButtonState.Down;

            return state;
        }

        /// <summary>
        /// Gets or sets the primary binding associated with this action.
        /// </summary>
        public InputBinding Primary
        {
            get { return primary; }
            set
            {
                Unregister(primary);
                primary = value;
                Register(primary);
            }
        }

        /// <summary>
        /// Gets or sets the secondary binding associated with this action.
        /// </summary>
        public InputBinding Secondary
        {
            get { return secondary; }
            set
            {
                Unregister(secondary);
                secondary = value;
                Register(secondary);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this action is enabled.
        /// </summary>
        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Occurs when one of the action's associated bindings is pressed.
        /// </summary>
        public event InputActionEventHandler Pressed;

        /// <summary>
        /// Occurs when one of the action's associated bindings is released after being pressed.
        /// </summary>
        public event InputActionEventHandler Released;

        /// <summary>
        /// Registers the specified binding.
        /// </summary>
        /// <param name="binding">The input binding to register.</param>
        private void Register(InputBinding binding)
        {
            if (binding == null)
                return;

            binding.AdjustPriority(priorityAdjustment);
            collection.RegisterBinding(binding);
        }

        /// <summary>
        /// Unregisters the specified input binding.
        /// </summary>
        /// <param name="binding">The input binding to unregister.</param>
        private void Unregister(InputBinding binding)
        {
            if (binding == null)
                return;

            if (binding == pressed)
            {
                released = true;
                pressed = null;
                OnReleased();
            }

            binding.AdjustPriority(null);
            collection.UnregisterBinding(binding);
        }

        /// <summary>
        /// Raises the Pressed event.
        /// </summary>
        private void OnPressed() =>
            Pressed?.Invoke(this);

        /// <summary>
        /// Raises the Released event.
        /// </summary>
        private void OnReleased() =>
            Released?.Invoke(this);

        // The input action collection that owns this action.
        private readonly InputActionCollection collection;

        // Property values.
        private InputBinding primary;
        private InputBinding secondary;
        private Boolean enabled = true;

        // State values.
        private InputBinding pressed;
        private Boolean released;
        private Int32? priorityAdjustment;
    }
}
