using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when an input binding is pressed or released.
    /// </summary>
    /// <param name="binding">The input binding that raised the event.</param>
    public delegate void InputBindingEventHandler(InputBinding binding);

    /// <summary>
    /// Represents an input binding.
    /// </summary>
    public abstract class InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the InputBinding class.
        /// </summary>
        internal InputBinding()
        {

        }

        /// <summary>
        /// Adjusts the input binding's priority by the specified amount.
        /// </summary>
        /// <remarks>Higher values indicate higher priority.</remarks>
        /// <param name="priorityAdjustment">The amount by which to adjust the binding's priority, 
        /// or null to reset the input binding's priority adjustment.</param>
        public void AdjustPriority(Int32? priorityAdjustment)
        {
            this.priorityAdjustment = priorityAdjustment;
        }

        /// <summary>
        /// Updates the binding's state.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same button configuration as the specified input binding.
        /// </summary>
        /// <param name="binding">The input binding to compare against this input binding.</param>
        /// <returns>true if the specified input binding uses the same device and the same button 
        /// configuration as this input binding; otherwise, false.</returns>
        public abstract Boolean UsesSameButtons(InputBinding binding);

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same primary buttons as the specified input binding.
        /// </summary>
        /// <param name="binding">The input binding to compare against this input binding.</param>
        /// <returns>true if the specified input binding uses the same device and the same primary 
        /// buttons as this input binding; otherwise, false.</returns>
        public abstract Boolean UsesSamePrimaryButtons(InputBinding binding);

        /// <summary>
        /// Gets a value indicating whether the binding is down.
        /// </summary>
        /// <returns>true if the binding is down; otherwise, false.</returns>
        public abstract Boolean IsDown();

        /// <summary>
        /// Gets a value indicating whether the binding is up.
        /// </summary>
        /// <returns>true if the binding is up; otherwise, false.</returns>
        public abstract Boolean IsUp();

        /// <summary>
        /// Gets a value indicating whether the binding was pressed this frame.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>true if the binding was pressed this frame; otherwise, false.</returns>
        public abstract Boolean IsPressed(Boolean ignoreRepeats = true);

        /// <summary>
        /// Gets a value indicating whether the binding was released this frame.
        /// </summary>
        /// <returns>true if the binding was released this frame; otherwise, false.</returns>
        public abstract Boolean IsReleased();

        /// <summary>
        /// Gets the binding's current state.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>The binding's current state.</returns>
        public ButtonState GetState(Boolean ignoreRepeats = true)
        {
            var state = IsDown() ? ButtonState.Down : ButtonState.Up;

            if (IsPressed(ignoreRepeats))
                state |= ButtonState.Pressed;

            if (IsReleased())
                state |= ButtonState.Released;

            return state;
        }

        /// <summary>
        /// Gets the input binding's priority relative to other input bindings
        /// which use the same primary buttons.
        /// </summary>
        /// <remarks>Higher values indicate higher priority.</remarks>
        public Int32 Priority
        {
            get { return priorityAdjustment.GetValueOrDefault() + CalculatePriority(); }
        }

        /// <summary>
        /// Gets a value indicating whether this binding's priority has been adjusted.
        /// </summary>
        public Boolean PriorityIsAdjusted
        {
            get { return priorityAdjustment.HasValue; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this binding is enabled.
        /// </summary>
        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Occurs when the binding is pressed.
        /// </summary>
        public event InputBindingEventHandler Pressed;

        /// <summary>
        /// Occurs when the binding is released.
        /// </summary>
        public event InputBindingEventHandler Released;

        /// <summary>
        /// Creates an XML element that represents the binding.
        /// </summary>
        /// <param name="name">The name to give to the created XML element.</param>
        /// <returns>An XML element that represents the binding.</returns>
        internal abstract XElement ToXml(String name = null);
        
        /// <summary>
        /// Calculates the binding's priority relative to other bindings with the same primary buttons.
        /// </summary>
        /// <returns>The binding's priority relative to other bindings with the same primary buttons.</returns>
        protected abstract Int32 CalculatePriority();

        /// <summary>
        /// Raises the Pressed event.
        /// </summary>
        protected virtual void OnPressed()
        {
            var temp = Pressed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Released event.
        /// </summary>
        protected virtual void OnReleased()
        {
            var temp = Released;
            if (temp != null)
            {
                temp(this);
            }
        }

        // Property values.
        private Int32? priorityAdjustment;
        private Boolean enabled = true;
    }
}
