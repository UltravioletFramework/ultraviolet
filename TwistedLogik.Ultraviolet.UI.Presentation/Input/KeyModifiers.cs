using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the key modifiers 
    /// </summary>
    public struct KeyModifiers
    {
        /// <summary>
        /// Provides a compact internal representation of the keyboard modifier states.
        /// </summary>
        [Flags]
        private enum KeyModifierFlags : byte
        {
            None   = 0,
            Ctrl   = 0x1,
            Alt    = 0x2,
            Shift  = 0x4,
            Repeat = 0x8,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyModifiers"/> structure.
        /// </summary>
        /// <param name="ctrl">A value indicating whether the Control modifier is active.</param>
        /// <param name="alt">A value indicating whether the Alt modifier is active.</param>
        /// <param name="shift">A value indicating whether the Shift modifier is active.</param>
        /// <param name="repeat">A value indicating whether the key press is the result of a repeated event.</param>
        internal KeyModifiers(Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            flags = KeyModifierFlags.None;

            if (ctrl)
                flags |= KeyModifierFlags.Ctrl;

            if (alt)
                flags |= KeyModifierFlags.Alt;

            if (shift)
                flags |= KeyModifierFlags.Shift;

            if (repeat)
                flags |= KeyModifierFlags.Repeat;
        }

        /// <summary>
        /// Gets a value indicating whether the Control modifier is active.
        /// </summary>
        public Boolean Control
        {
            get { return (flags & KeyModifierFlags.Ctrl) == KeyModifierFlags.Ctrl; }
        }

        /// <summary>
        /// Gets a value indicating whether the Alt modifier is active.
        /// </summary>
        public Boolean Alt
        {
            get { return (flags & KeyModifierFlags.Alt) == KeyModifierFlags.Alt; }
        }

        /// <summary>
        /// Gets a value indicating whether the Shift modifier is active.
        /// </summary>
        public Boolean Shift
        {
            get { return (flags & KeyModifierFlags.Shift) == KeyModifierFlags.Shift; }
        }

        /// <summary>
        /// Gets a value indicating whether the key press is the result of a repeated event.
        /// </summary>
        public Boolean Repeat
        {
            get { return (flags & KeyModifierFlags.Repeat) == KeyModifierFlags.Repeat; }
        }

        // State values.
        private readonly KeyModifierFlags flags;
    }
}
