using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Layout.Animation;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents one of a UI element's visual states.
    /// </summary>
    public sealed class VisualState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualState"/> class.
        /// </summary>
        /// <param name="name">The visual state's name.</param>
        internal VisualState(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            this.name    = name;
        }

        /// <summary>
        /// Gets the visual state's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the visual state's transition storyboard.
        /// </summary>
        public Storyboard Transition
        {
            get { return transition; }
            set { transition = value; }
        }

        // Property values.
        private readonly String name;
        private Storyboard transition;
    }
}
