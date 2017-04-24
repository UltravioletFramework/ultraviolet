using System;

namespace Ultraviolet.SDL2.Input
{
    /// <summary>
    /// An internal representation of the state of a button on an input device.
    /// </summary>
    internal struct InternalButtonState
    {
        /// <summary>
        /// Resets the button's pressed/released state.
        /// </summary>
        public void Reset()
        {
            this.pressed  = false;
            this.repeated = false;
            this.released = false;
        }

        /// <summary>
        /// Puts the button into the "down" state.
        /// </summary>
        /// <param name="repeat">A value indicating whether this is a button repeat event.</param>
        public void OnDown(Boolean repeat)
        {
            this.down = true;
            if (repeat)
            {
                this.repeated = true;
            }
            else
            {
                this.pressed = true;
            }
        }

        /// <summary>
        /// Puts the button into the "up" state.
        /// </summary>
        public void OnUp()
        {
            this.down     = false;
            this.released = true;
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the Down state.
        /// </summary>
        public Boolean Down
        {
            get { return down; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the Up state.
        /// </summary>
        public Boolean Up
        {
            get { return !down; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the Pressed state.
        /// </summary>
        public Boolean Pressed
        {
            get { return pressed; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the Repeated state.
        /// </summary>
        public Boolean Repeated
        {
            get { return repeated; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the Released state.
        /// </summary>
        public Boolean Released
        {
            get { return released; }
        }

        // State values.
        private Boolean down;
        private Boolean pressed;
        private Boolean repeated;
        private Boolean released;
    }
}
